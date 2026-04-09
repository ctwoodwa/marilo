namespace Marilo.Components.DataDisplay;

public class GanttDependencyCreateEventArgs
{
    public GanttDependency Dependency { get; set; } = default!;
}

public class GanttDependencyDeleteEventArgs
{
    public GanttDependency Dependency { get; set; } = default!;
}
