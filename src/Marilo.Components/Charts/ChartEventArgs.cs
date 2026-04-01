namespace Marilo.Components.Charts;

/// <summary>
/// Event args for chart data point click events.
/// Engine-agnostic — no Chart.js types exposed.
/// </summary>
public class ChartClickEventArgs
{
    /// <summary>The index of the clicked series.</summary>
    public int SeriesIndex { get; init; }

    /// <summary>The index of the clicked data point within the series.</summary>
    public int DataPointIndex { get; init; }

    /// <summary>The category (label) of the clicked data point.</summary>
    public string? Category { get; init; }

    /// <summary>The value of the clicked data point.</summary>
    public double? Value { get; init; }
}
