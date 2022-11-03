namespace Statim.Jobs;

public abstract class Job : IJob
{
    public string Name { get; } = Guid.NewGuid().ToString("D");
    public abstract Task ExecuteAsync(CancellationToken cancellationToken);
}