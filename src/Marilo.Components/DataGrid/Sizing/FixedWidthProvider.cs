using System.Globalization;

namespace Marilo.Components.DataGrid.Sizing;

/// <summary>
/// Resolves explicit widths (or defaults) into a shared immutable contract.
/// </summary>
public sealed class FixedWidthProvider : IColumnWidthProvider
{
    // Unspecified columns should remain auto-sized so the fixed-layout table can fill the container.
    public string DefaultWidth { get; init; } = "auto";

    public GridLayoutContract Resolve(IReadOnlyList<ColumnSizingEntry> columns)
    {
        var widthById = new Dictionary<string, string>(columns.Count);
        var orderedIds = new List<string>(columns.Count);

        foreach (var column in columns)
        {
            var raw = string.IsNullOrWhiteSpace(column.ExplicitWidth) ? DefaultWidth : column.ExplicitWidth!;
            var normalized = NormalizeAndClamp(raw, column.MinWidth, column.MaxWidth);

            widthById[column.Id] = normalized;
            orderedIds.Add(column.Id);
        }

        var template = string.Join(" ", orderedIds.Select(id => widthById[id]));

        return new GridLayoutContract
        {
            WidthById = widthById,
            OrderedColumnIds = orderedIds,
            CenterGridTemplate = template
        };
    }

    private static string NormalizeAndClamp(string width, double min, double? max)
    {
        if (TryParsePixelWidth(width, out var px))
        {
            var clamped = max.HasValue ? Math.Clamp(px, min, max.Value) : Math.Max(px, min);
            return $"{clamped.ToString("0.###", CultureInfo.InvariantCulture)}px";
        }

        if (double.TryParse(width, NumberStyles.Float, CultureInfo.InvariantCulture, out var numberOnly))
        {
            var clamped = max.HasValue ? Math.Clamp(numberOnly, min, max.Value) : Math.Max(numberOnly, min);
            return $"{clamped.ToString("0.###", CultureInfo.InvariantCulture)}px";
        }

        return width;
    }

    private static bool TryParsePixelWidth(string width, out double value)
    {
        value = 0;
        var trimmed = width.Trim();
        if (!trimmed.EndsWith("px", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var numeric = trimmed[..^2].Trim();
        return double.TryParse(numeric, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }
}
