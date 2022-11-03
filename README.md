# Statim job scheduler

.NET job scheduler

## Compatibility

Statim targets .NET Core SDK 6.0 and up

## Getting started

Basic usage

```csharp

// add Statim to your service collection
var host = Host.CreateDefaultBuilder()
    .UseStatim()
    .ConfigureServices(services =>
    {
        services.AddHostedService<TestService>();
    })
    .Build();

await host.RunAsync();

// then via DI you can use IStatimScheduler to schedule jobs
public class TestService : IHostedService
{
    private readonly IStatimScheduler _scheduler;

    public TestService(IStatimScheduler scheduler)
    {
        _scheduler = scheduler;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // run job every 10 seconds
        _scheduler.CreateTrigger()
            .TriggerEvery(TimeSpan.FromSeconds(10))
            // register job as lambda
            .AddJob(async token => Console.WriteLine(DateTime.UtcNow));
            // register IJob object that will resolve it's depedencies from DI
            .AddJob<SomeJob>();
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
```