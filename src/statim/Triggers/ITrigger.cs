namespace Statim.Triggers;

public interface ITrigger
{ 
    string Name { get; }
    IEnumerable<DateTime> GetTriggerTimes();
}