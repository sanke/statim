namespace Statim.Triggers;

public class TimeTrigger : Trigger
{
    private const int StartupCostMs = 100;
    private TimeSpan _interval;
    private DateTime _lastTrigger;

    public TimeTrigger()
    {
        _lastTrigger = DateTime.UtcNow.AddMilliseconds(StartupCostMs);
    }

    public TimeTrigger Start(DateTime time)
    {
        DateTime utcStart = time.ToUniversalTime();

        if (utcStart < DateTime.UtcNow)
        {
            utcStart = DateTime.UtcNow.AddMilliseconds(StartupCostMs);
        }

        _lastTrigger = utcStart;
        return this;
    }

    public TimeTrigger Interval(TimeSpan period)
    {
        if (period < TimeSpan.Zero)
        {
            throw new ArgumentException("Period can't be negative", nameof(period));
        }

        _interval = period;
        return this;
    }

    public override IEnumerable<DateTime> GetTriggerTimes()
    {
        _lastTrigger += _interval;
        yield return _lastTrigger;
    }
}