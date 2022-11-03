using Statim.Triggers;

namespace Statim.Jobs;

public interface IJobContext
{
    IStatimScheduler Scheduler { get; set; }
    ITrigger Trigger { get; set; }
}