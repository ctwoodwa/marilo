using System.Collections.Generic;

namespace Marilo.Components.DataDisplay;

/// <summary>
/// Internal tree node used by <see cref="MariloGantt{TItem}"/> to represent
/// a hierarchical view built from flat ParentId-linked data.
/// </summary>
internal sealed class GanttNode<TItem> where TItem : class
{
    public TItem Item { get; set; } = default!;
    public object? Id { get; set; }
    public object? ParentId { get; set; }
    public int Depth { get; set; }
    public List<GanttNode<TItem>> Children { get; } = new();
    public bool IsExpanded { get; set; } = true;
    public GanttNode<TItem>? Parent { get; set; }
    internal int OriginalIndex { get; set; }
}
