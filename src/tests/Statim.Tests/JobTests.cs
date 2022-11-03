using Moq;
using Statim.Triggers;
using Xunit;

namespace Statim.Tests;

public class TimeTriggerTests
{
    [Fact]
    public void ShouldReturnNextTriggerTime()
    {
        var now = new DateTime(2022, 10, 28, 22, 30, 10);
        var timeMock = new Mock<IDateTimeProvider>();
        timeMock.SetupGet(e => e.UtcNow).Returns(now);

        var trigger = new TimeTrigger().Start(now).Interval(TimeSpan.FromSeconds(10));
        _ = trigger.GetTriggerTimes();
        _ = trigger.GetTriggerTimes();
        DateTime time = trigger.GetTriggerTimes().GetEnumerator().Current;

        Assert.Equal(now.AddSeconds(30), time);
    }
}