using Microsoft.AspNetCore.Components.Web;

namespace Marilo.Components.DataGrid;

/// <summary>
/// Event arguments for grid row click events (<c>OnRowClick</c>, <c>OnRowDoubleClick</c>).
/// </summary>
/// <typeparam name="TItem">The row data type.</typeparam>
public class GridRowClickEventArgs<TItem>
{
    /// <summary>The data item for the clicked row.</summary>
    public TItem Item { get; init; } = default!;

    /// <summary>The field name of the clicked column, if available.</summary>
    public string? Field { get; init; }

    /// <summary>The original mouse event args from the browser.</summary>
    public MouseEventArgs EventArgs { get; init; } = default!;
}

/// <summary>
/// Event arguments for the <c>OnRead</c> server-side data callback.
/// The consumer must set <see cref="Data"/> and <see cref="Total"/> in their handler.
/// </summary>
/// <typeparam name="TItem">The row data type.</typeparam>
public class GridReadEventArgs<TItem>
{
    /// <summary>The data request containing sort, filter, group, and page descriptors.</summary>
    public GridState Request { get; init; } = default!;

    /// <summary>Set this to the data items for the current page/view. The grid will display these.</summary>
    public IEnumerable<TItem> Data { get; set; } = [];

    /// <summary>Set this to the total number of items (before paging) so the pager can calculate page count.</summary>
    public int Total { get; set; }
}

/// <summary>
/// Event arguments for the <c>OnRowRender</c> callback, allowing per-row CSS customization.
/// </summary>
/// <typeparam name="TItem">The row data type.</typeparam>
public class GridRowRenderEventArgs<TItem>
{
    /// <summary>The data item for the row being rendered.</summary>
    public TItem Item { get; init; } = default!;

    /// <summary>Set additional CSS class(es) to apply to this row.</summary>
    public string? Class { get; set; }

    /// <summary>Set additional inline style(s) to apply to this row.</summary>
    public string? Style { get; set; }
}

/// <summary>
/// Event arguments for the <c>OnCellRender</c> callback on <see cref="MariloGridColumn{TItem}"/>.
/// </summary>
/// <typeparam name="TItem">The row data type.</typeparam>
public class GridCellRenderEventArgs<TItem>
{
    /// <summary>The data item for the row containing this cell.</summary>
    public TItem Item { get; init; } = default!;

    /// <summary>The field name of the column.</summary>
    public string? Field { get; init; }

    /// <summary>The cell value.</summary>
    public object? Value { get; init; }

    /// <summary>Set additional CSS class(es) to apply to this cell.</summary>
    public string? Class { get; set; }

    /// <summary>Set additional inline style(s) to apply to this cell.</summary>
    public string? Style { get; set; }
}

/// <summary>
/// Event arguments for the <c>OnStateChanged</c> event.
/// </summary>
public class GridStateChangedEventArgs
{
    /// <summary>The name of the property that changed (e.g. "Page", "Sort", "Filter").</summary>
    public string PropertyName { get; init; } = "";

    /// <summary>A snapshot of the current grid state.</summary>
    public GridState State { get; init; } = default!;
}
