using Statim.Jobs;
using Statim.Triggers;

namespace Statim.Extensions;

public static class TimeTriggerExtensions
{
    /// <summary>
    /// Set the start time of the trigger
    /// </summary>
    /// <param name="scheduledJobBuilder"></param>
    /// <param name="startTime"></param>
    /// <returns></returns>
    public static IScheduledJobBuilder<TimeTrigger> SetStart(this IScheduledJobBuilder<TimeTrigger> scheduledJobBuilder,
        DateTime startTime)
    {
        scheduledJobBuilder.Trigger.Start(startTime);
        return scheduledJobBuilder;
    }

    /// <summary>
    /// Set interval of the trigger
    /// </summary>
    /// <param name="scheduledJobBuilder"></param>
    /// <param name="interval"></param>
    /// <returns></returns>
    public static IScheduledJobBuilder<TimeTrigger> TriggerEvery(
        this IScheduledJobBuilder<TimeTrigger> scheduledJobBuilder,
        TimeSpan interval)
    {
        scheduledJobBuilder.Trigger.Interval(interval);
        return scheduledJobBuilder;
    }

    // public static IScheduledJobBuilder<TimeTrigger> AddJob<T>(
    //     this IScheduledJobBuilder<TimeTrigger> scheduledJobBuilder) where T : IJob
    // {
    //     scheduledJobBuilder.JobContainer.AddJob<T>(scheduledJobBuilder.Trigger.Name);
    //     return scheduledJobBuilder;
    // }
}