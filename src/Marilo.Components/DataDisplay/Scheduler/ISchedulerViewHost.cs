namespace Marilo.Components.DataDisplay.Scheduler;

/// <summary>
/// Non-generic interface that allows <see cref="SchedulerViewBase"/> components to register
/// with their parent <see cref="MariloScheduler"/> without knowing the concrete view type.
/// </summary>
public interface ISchedulerViewHost
{
    /// <summary>Registers a view configuration component with this scheduler.</summary>
    void RegisterView(SchedulerViewBase view);

    /// <summary>Unregisters a view configuration component from this scheduler.</summary>
    void UnregisterView(SchedulerViewBase view);
}
