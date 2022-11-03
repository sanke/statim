using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Statim.Jobs;

public delegate Task JobDelegate(IServiceProvider provider, CancellationToken token);

public class JobContainer : IJobContainer
{
    private readonly Dictionary<string, List<JobDelegate>> _jobs = new(100);

    public void AddJob(IJob job, string triggerName)
    {
        GetJobList(triggerName).Add((_, token) => job.ExecuteAsync(token));
    }

    public IEnumerable<JobDelegate> GetJobs(string triggerName)
    {
        if (!_jobs.ContainsKey(triggerName))
        {
            return Enumerable.Empty<JobDelegate>();
        }
        
        return _jobs[triggerName];
    }

    public bool HasJobs(string triggerName)
    {
        return _jobs.ContainsKey(triggerName);
    }

    private List<JobDelegate> GetJobList(string triggerName)
    {
        if (triggerName == null)
        {
            throw new ArgumentNullException(nameof(triggerName));
        }

        if (!_jobs.TryGetValue(triggerName, out var list))
        {
            list = new List<JobDelegate>();
            _jobs.Add(triggerName, list);
        }

        return list;
    }

    public void AddJob(string triggerName, Func<CancellationToken, Task> action)
    {
        GetJobList(triggerName).Add((_, token) => action(token));
    }

    public void AddJob<T>(string triggerName) where T : IJob
    {
        Task CreateJobDelegate(IServiceProvider provider, CancellationToken token)
        {
            var method = typeof(ActivatorUtilities).GetMethod(
                nameof(ActivatorUtilities.CreateInstance), BindingFlags.Static | BindingFlags.Public, new[]
                {
                    typeof(IServiceProvider),
                    typeof(Type),
                    typeof(object[])
                });

            var call = Expression.Call(method!, Expression.Constant(provider),
                Expression.Constant(typeof(T)), Expression.Constant(Array.Empty<object>()));

            var obj = (IJob)Expression.Lambda<Func<object>>(call).Compile().Invoke();

            return obj.ExecuteAsync(token);
        }

        GetJobList(triggerName).Add(CreateJobDelegate);
    }
}