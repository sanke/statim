namespace Statim.Jobs;

public interface IJob<out T> : IJob
{
    public T Parameters { get; }
}

public interface IJob
{
    string Name { get; }
    Task ExecuteAsync(CancellationToken cancellationToken);
}