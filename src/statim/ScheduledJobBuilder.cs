using Statim.Jobs;
using Statim.Triggers;

namespace Statim;

internal class ScheduledJobBuilder<T> : IScheduledJobBuilder<T>
    where T : ITrigger, new()
{
    public T Trigger { get; }
    public IJobContainer JobContainer { get; }

    public ScheduledJobBuilder(IStatimScheduler scheduler, IJobContainer jobContainer)
    {
        JobContainer = jobContainer;
        Trigger = new T();
        scheduler.AddTrigger(Trigger);
    }

    public IScheduledJobBuilder<T> AddJob<TJob>() where TJob : IJob
    {
        JobContainer.AddJob<TJob>(Trigger.Name);
        return this;
    }
    
    public IScheduledJobBuilder<T> AddJob(Func<CancellationToken, Task> func) 
    {
        JobContainer.AddJob(Trigger.Name, func);
        return this;
    }
}