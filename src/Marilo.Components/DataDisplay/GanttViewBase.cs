using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Marilo.Components.DataDisplay;

/// <summary>
/// Abstract base class for Gantt view configuration components (Day, Week, Month, Year).
/// These are configuration-only components that produce no rendered output.
/// </summary>
public abstract class GanttViewBase : ComponentBase, IDisposable
{
    [CascadingParameter] private IGanttViewHost? ViewHost { get; set; }

    /// <summary>The pixel width of one timeline slot in this view.</summary>
    [Parameter] public double SlotWidth { get; set; }

    /// <summary>Optional explicit start of the visible range. If null, auto-calculated from data.</summary>
    [Parameter] public DateTime? RangeStart { get; set; }

    /// <summary>Optional explicit end of the visible range. If null, auto-calculated from data.</summary>
    [Parameter] public DateTime? RangeEnd { get; set; }

    /// <summary>The view type this component represents.</summary>
    public abstract GanttView ViewType { get; }

    protected override void OnInitialized()
    {
        ViewHost?.RegisterView(this);
    }

    /// <summary>No rendered output — configuration-only component.</summary>
    protected override void BuildRenderTree(RenderTreeBuilder builder) { }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing) ViewHost?.UnregisterView(this);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
