namespace Marilo.Core.Models;

/// <summary>
/// Defines a column in a MariloTreeList.
/// </summary>
[Obsolete("Use MariloTreeListColumn child components instead. This POCO-based column API will be removed in a future release.")]
public class TreeListColumn
{
    /// <summary>Column header title.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Property name on TItem to display in this column.</summary>
    public string Field { get; set; } = string.Empty;

    /// <summary>Optional CSS width (e.g., "200px", "30%").</summary>
    public string? Width { get; set; }
}
