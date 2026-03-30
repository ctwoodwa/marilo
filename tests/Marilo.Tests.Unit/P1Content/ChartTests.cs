using Bunit;
using Marilo.Components.Charts;
using Marilo.Core.Enums;
using Xunit;

namespace Marilo.Tests.Unit.P1Content;

public class ChartTests : MariloTestBase
{
    private record TestData(string Month, double Value);

    private readonly List<TestData> _sampleData =
    [
        new("Jan", 100),
        new("Feb", 200),
        new("Mar", 150),
    ];

    [Fact]
    public void Chart_Renders_SvgContainer()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.Title, "Test Chart")
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

        var container = cut.Find("div.mar-chart-container");
        Assert.NotNull(container);

        var svg = cut.Find("svg");
        Assert.NotNull(svg);
    }

    [Fact]
    public void Chart_Renders_Title()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.Title, "Revenue Chart")
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

        var title = cut.Find(".mar-chart-title");
        Assert.Equal("Revenue Chart", title.TextContent);
    }

    [Fact]
    public void Chart_Renders_SeriesData_As_SvgElements()
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

        var polyline = cut.Find("polyline");
        Assert.NotNull(polyline);

        var circles = cut.FindAll("circle");
        Assert.Equal(3, circles.Count);
    }

    [Fact]
    public void Chart_Column_Renders_Rect_Elements()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Sales");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_sampleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.CategoryField), "Month");
                builder.AddAttribute(5, nameof(MariloChartSeries.Type), ChartSeriesType.Column);
                builder.CloseComponent();
            }));

        var rects = cut.FindAll("rect");
        Assert.Equal(3, rects.Count);
    }

    [Fact]
    public void Chart_Renders_Legend()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Revenue");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_sampleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.CategoryField), "Month");
                builder.AddAttribute(5, nameof(MariloChartSeries.Type), ChartSeriesType.Line);
                builder.CloseComponent();
            }));

        var legend = cut.Find(".mar-chart-legend");
        Assert.NotNull(legend);
        Assert.Contains("Revenue", legend.TextContent);
    }
}
