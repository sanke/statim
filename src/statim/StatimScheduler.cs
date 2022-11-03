using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Statim.Jobs;
using Statim.Triggers;

namespace Statim;

public class StatimScheduler : IStatimScheduler, IHostedService
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IJobContainer _jobContainer;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly object _sync = new();

    private readonly PriorityQueue<ITrigger, DateTime> _triggerQueue = new();

    public StatimScheduler(IDateTimeProvider dateTimeProvider, IServiceScopeFactory scopeFactory,
        IJobContainer jobContainer)
    {
        _dateTimeProvider = dateTimeProvider;
        _scopeFactory = scopeFactory;
        _jobContainer = jobContainer;
    }

    public IScheduledJobBuilder<TimeTrigger> CreateTrigger()
    {
        return new ScheduledJobBuilder<TimeTrigger>(this, _jobContainer);
    }

    public void AddTrigger(ITrigger trigger)
    {
        var nextTrigger = trigger.GetTriggerTimes();
        lock (_sync)
        {
            _triggerQueue.Enqueue(trigger, nextTrigger.First());
            Monitor.PulseAll(_sync);
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(() => TriggerLoop(cancellationToken), _cancellationTokenSource.Token);
        return Task.CompletedTask;
    }

    private void TriggerLoop(CancellationToken cancellationToken)
    {
        do
        {
            ITrigger? trigger;

            lock (_sync)
            {
                if (_triggerQueue.Count == 0)
                {
                    Monitor.Wait(_sync);
                }

                while (_triggerQueue.TryDequeue(out trigger, out var time))
                {
                    var now = _dateTimeProvider.UtcNow;
                    TimeSpan delay = time - now;

                    if (delay <= TimeSpan.Zero)
                    {
                        DateTime nextTime = trigger.GetTriggerTimes().First();
                        _triggerQueue.Enqueue(trigger, nextTime);
                        goto execute_jobs;
                    }

                    Monitor.Wait(_sync, delay);
                    _triggerQueue.Enqueue(trigger, time);
                }
            }

            execute_jobs:
            if (trigger == null)
            {
                continue;
            }

            IEnumerable<JobDelegate> jobs = _jobContainer.GetJobs(trigger.Name);

            IEnumerable<Task> tasks = jobs.Select(e =>
            {
                var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetService<IJobContext>()!;
                
                context.Scheduler = this;
                context.Trigger = trigger;

                Task task = e(scope.ServiceProvider, cancellationToken)
                    .ContinueWith(_ => scope.Dispose(), CancellationToken.None);
                return task;
            });

            Task.Factory.StartNew(() => Task.WhenAll(tasks), CancellationToken.None, TaskCreationOptions.None,
                TaskScheduler.Default);
        } while (!_cancellationTokenSource.IsCancellationRequested);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}