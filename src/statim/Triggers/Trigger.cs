namespace Statim.Triggers;

public abstract class Trigger : ITrigger
{
    public string Name { get; } = Guid.NewGuid().ToString("D");

    public virtual IEnumerable<DateTime> GetTriggerTimes()
    {
        yield return DateTime.UtcNow;
    }
}