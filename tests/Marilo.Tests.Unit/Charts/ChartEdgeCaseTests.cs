using Bunit;
using Marilo.Components.Charts;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.Charts;

public class ChartEdgeCaseTests : MariloTestBase
{
    private record TestData(string Month, double Value);
    private record BubbleData(string Label, double Value, double Size);

    private readonly List<TestData> _sampleData =
    [
        new("Jan", 100),
        new("Feb", 200),
        new("Mar", 150),
    ];

    private readonly List<BubbleData> _bubbleData =
    [
        new("A", 100, 10),
        new("B", 200, 30),
        new("C", 150, 20),
    ];

    // ── Pie chart rendering ─────────────────────────────────────────────

    [Fact]
    public void Pie_Series_Renders_Paths()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Sales");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_sampleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.CategoryField), "Month");
                builder.AddAttribute(5, nameof(MariloChartSeries.Type), ChartSeriesType.Pie);
                builder.CloseComponent();
            }));

        var paths = cut.FindAll("path");
        Assert.Equal(3, paths.Count); // one per data point
    }

    [Fact]
    public void Donut_Series_Renders_Paths()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Sales");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_sampleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.CategoryField), "Month");
                builder.AddAttribute(5, nameof(MariloChartSeries.Type), ChartSeriesType.Donut);
                builder.CloseComponent();
            }));

        var paths = cut.FindAll("path");
        Assert.Equal(3, paths.Count);
    }

    // ── ScatterLine rendering ────────���──────────────────────────────────

    [Fact]
    public void ScatterLine_Series_Renders_CirclesAndPolyline()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Scatter");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_sampleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.XField), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.YField), "Value");
                builder.AddAttribute(5, nameof(MariloChartSeries.Type), ChartSeriesType.ScatterLine);
                builder.CloseComponent();
            }));

        // ScatterLine should render both circles and a connecting polyline
        var circles = cut.FindAll("circle");
        Assert.Equal(3, circles.Count);

        var polylines = cut.FindAll("polyline");
        Assert.True(polylines.Count >= 1, "ScatterLine should render a connecting polyline");
    }

    // ── No data state ───────────��───────────────────────────────────────

    [Fact]
    public void Chart_WithNoSeries_Shows_NoData()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.Title, "Empty Chart"));

        var noData = cut.Find(".mar-chart-no-data");
        Assert.Contains("No data", noData.TextContent);
    }

    // ── ShowLegend false ──────────────────────────────────────��─────────

    [Fact]
    public void Chart_ShowLegend_False_HidesLegend()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ShowLegend, false)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Sales");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_sampleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.CategoryField), "Month");
                builder.AddAttribute(5, nameof(MariloChartSeries.Type), ChartSeriesType.Line);
                builder.CloseComponent();
            }));

        var legends = cut.FindAll(".mar-chart-legend");
        Assert.Empty(legends);
    }

    // ── Tooltip element present but hidden ──────────────────────────────

    [Fact]
    public void Chart_Tooltip_RenderedHidden_ByDefault()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Sales");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_sampleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.CategoryField), "Month");
                builder.AddAttribute(5, nameof(MariloChartSeries.Type), ChartSeriesType.Line);
                builder.CloseComponent();
            }));

        var tooltip = cut.Find(".mar-chart-tooltip");
        var style = tooltip.GetAttribute("style") ?? "";
        Assert.Contains("display:none", style);
    }

    // ── ShowTooltips false ──────────────────────────────────────────────

    [Fact]
    public void Chart_ShowTooltips_False_NoTooltipElement()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ShowTooltips, false)
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Sales");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_sampleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.CategoryField), "Month");
                builder.AddAttribute(5, nameof(MariloChartSeries.Type), ChartSeriesType.Line);
                builder.CloseComponent();
            }));

        var tooltips = cut.FindAll(".mar-chart-tooltip");
        Assert.Empty(tooltips);
    }

    // ── Area chart renders polygon + polyline ───────────────────────────

    [Fact]
    public void Area_Series_Renders_Polygon_And_Polyline()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Sales");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_sampleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.CategoryField), "Month");
                builder.AddAttribute(5, nameof(MariloChartSeries.Type), ChartSeriesType.Area);
                builder.CloseComponent();
            }));

        var polygons = cut.FindAll("polygon");
        Assert.Single(polygons); // area fill

        var polylines = cut.FindAll("polyline");
        Assert.Single(polylines); // line stroke
    }

    // ── Empty data source does not crash ────────────────────────────���────

    [Fact]
    public void Chart_EmptyData_DoesNotCrash()
    {
        var emptyData = new List<TestData>();

        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Sales");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)emptyData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.CategoryField), "Month");
                builder.AddAttribute(5, nameof(MariloChartSeries.Type), ChartSeriesType.Line);
                builder.CloseComponent();
            }));

        // Should not crash; renders container but no SVG content
        var container = cut.Find("div.mar-chart-container");
        Assert.NotNull(container);
    }

    // ── Pie chart has legend with category names ────────────────────────

    [Fact]
    public void Pie_Chart_Legend_Shows_Categories()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Sales");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_sampleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.CategoryField), "Month");
                builder.AddAttribute(5, nameof(MariloChartSeries.Type), ChartSeriesType.Pie);
                builder.CloseComponent();
            }));

        var legend = cut.Find(".mar-chart-legend");
        Assert.Contains("Jan", legend.TextContent);
        Assert.Contains("Feb", legend.TextContent);
        Assert.Contains("Mar", legend.TextContent);
    }

    // ── Pie slices have percentage aria-label ────────────────────────────

    [Fact]
    public void Pie_Slices_HavePercentage_AriaLabels()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Sales");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_sampleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.CategoryField), "Month");
                builder.AddAttribute(5, nameof(MariloChartSeries.Type), ChartSeriesType.Pie);
                builder.CloseComponent();
            }));

        // Pie slices include percentage in aria-label
        Assert.Contains("%)", cut.Markup);
    }
}
