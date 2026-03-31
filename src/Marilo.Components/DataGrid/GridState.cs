using Marilo.Core.Data;

namespace Marilo.Components.DataGrid;

/// <summary>
/// Represents the current state of a <see cref="MariloDataGrid{TItem}"/>,
/// including paging, sorting, filtering, and grouping descriptors.
/// Can be used with <c>OnStateInit</c> and <c>OnStateChanged</c> to persist and restore state.
/// </summary>
public class GridState
{
    /// <summary>The current page number (1-based).</summary>
    public int CurrentPage { get; set; } = 1;

    /// <summary>The number of items per page.</summary>
    public int PageSize { get; set; } = 10;

    /// <summary>Active sort descriptors.</summary>
    public List<SortDescriptor> SortDescriptors { get; set; } = [];

    /// <summary>Active filter descriptors.</summary>
    public List<FilterDescriptor> FilterDescriptors { get; set; } = [];

    /// <summary>Active group descriptors.</summary>
    public List<GroupDescriptor> GroupDescriptors { get; set; } = [];

    /// <summary>The total number of items after filtering (before paging).</summary>
    public int TotalCount { get; set; }

    /// <summary>The currently selected item keys (for state persistence).</summary>
    public HashSet<object> SelectedKeys { get; set; } = [];
}
