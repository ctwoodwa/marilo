namespace Marilo.Components.DataGrid.Sizing;

/// <summary>
/// Input model for a single column width resolution pass.
/// </summary>
public sealed record ColumnSizingEntry(
    string Id,
    string? ExplicitWidth,
    double MinWidth,
    double? MaxWidth,
    string? TextAlign = null);
