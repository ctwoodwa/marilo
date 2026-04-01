using Microsoft.AspNetCore.Components;

namespace Marilo.Core.Models;

/// <summary>
/// Represents a tile item in the MariloTileLayout.
/// </summary>
public class TileLayoutItem
{
    public string? Title { get; set; }
    public RenderFragment? Content { get; set; }
    public int ColSpan { get; set; } = 1;
    public int RowSpan { get; set; } = 1;
    public int? Order { get; set; }
}

/// <summary>
/// Event args for tile reorder operations.
/// </summary>
public class TileReorderEventArgs
{
    public TileLayoutItem Item { get; set; } = default!;
    public int OldIndex { get; set; }
    public int NewIndex { get; set; }
}
