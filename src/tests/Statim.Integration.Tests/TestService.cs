using Microsoft.Extensions.Hosting;
using Statim.Extensions;

namespace Statim.Integration.Tests;

public class TestService : IHostedService
{
    private readonly IStatimScheduler _scheduler;

    public TestService(IStatimScheduler scheduler)
    {
        _scheduler = scheduler;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _scheduler.CreateTrigger()
            .TriggerEvery(TimeSpan.FromSeconds(10))
            .AddJob(async token => Console.WriteLine(DateTime.UtcNow));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}