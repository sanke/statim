using Statim.Triggers;

namespace Statim.Jobs;

internal class JobContext : IJobContext
{
    public IStatimScheduler Scheduler { get; set; }
    public ITrigger Trigger { get; set; }
}