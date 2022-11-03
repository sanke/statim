using Statim.Triggers;

namespace Statim;

public interface IStatimScheduler
{
    /// <summary>
    /// Add existing trigger to the scheduler
    /// </summary>
    /// <param name="trigger"></param>
    void AddTrigger(ITrigger trigger);
    
    /// <summary>
    /// Create a trigger
    /// </summary>
    /// <returns></returns>
    IScheduledJobBuilder<TimeTrigger> CreateTrigger();
}