using Marilo.Core.Enums;

namespace Marilo.Core.Models;

[Obsolete("Use MariloTreeListColumn child components instead.")]
public class TreeListColumn
{
    public string Title { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty;
    public string? Width { get; set; }
}

public class TreeListCommandEventArgs<TItem>
{
    public TItem Item { get; set; } = default!;
    public TItem? ParentItem { get; set; }
    public bool IsNew { get; set; }
}

public class TreeListSortEventArgs
{
    public string? Field { get; set; }
    public SortDirection? Direction { get; set; }
}

public class TreeListSelectionEventArgs<TItem>
{
    public IReadOnlyList<TItem> SelectedItems { get; set; } = Array.Empty<TItem>();
}

public class TreeListColumnReorderEventArgs
{
    /// <summary>The field name of the column that was moved.</summary>
    public string Field { get; set; } = string.Empty;

    /// <summary>The original index of the column before the move.</summary>
    public int OldIndex { get; set; }

    /// <summary>The new index of the column after the move.</summary>
    public int NewIndex { get; set; }
}
