namespace Statim.Jobs;

public interface IJobContainer
{
    void AddJob(IJob job, string triggerName);
    void AddJob<T>(string triggerName) where T : IJob;
    IEnumerable<JobDelegate> GetJobs(string triggerName);
    bool HasJobs(string triggerName);
    void AddJob(string triggerName, Func<CancellationToken, Task> action);
}