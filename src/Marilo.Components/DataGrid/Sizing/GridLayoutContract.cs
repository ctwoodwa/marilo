namespace Marilo.Components.DataGrid.Sizing;

/// <summary>
/// Shared width output consumed by header/body/footer renderers.
/// </summary>
public sealed class GridLayoutContract
{
    public static GridLayoutContract Empty { get; } = new();

    public IReadOnlyDictionary<string, string> WidthById { get; init; } = new Dictionary<string, string>();

    public IReadOnlyList<string> OrderedColumnIds { get; init; } = [];

    public string CenterGridTemplate { get; init; } = string.Empty;
}
