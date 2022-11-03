using Statim.Jobs;

namespace Statim;

public interface IScheduledJobBuilder<out T>
{
    T Trigger { get; }
    IJobContainer JobContainer { get; }
    
    /// <summary>
    /// Add a job to the trigger which will be executed when the trigger is triggered
    /// All registered service provider services will be accessible for injection in the job <typeparam name="TJob"></typeparam>
    /// </summary>
    /// <typeparam name="TJob"></typeparam>
    /// <returns></returns>
    IScheduledJobBuilder<T> AddJob<TJob>() where TJob : IJob;
    
    /// <summary>
    /// Add a function delegate to the trigger which will be executed when the trigger is triggered
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    IScheduledJobBuilder<T> AddJob(Func<CancellationToken, Task> func);
}