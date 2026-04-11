using Bunit;
using Marilo.Components.Charts;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.Charts;

public class ChartBatch2Tests : MariloTestBase
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

    // ── GAP-CHART-009: Bubble rendering ──────────────────────────────

    [Fact]
    public void Bubble_Series_Renders_Circles()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Bubbles");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_bubbleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.CategoryField), "Label");
                builder.AddAttribute(5, nameof(MariloChartSeries.SizeField), "Size");
                builder.AddAttribute(6, nameof(MariloChartSeries.Type), ChartSeriesType.Bubble);
                builder.CloseComponent();
            }));

        var circles = cut.FindAll("circle");
        Assert.Equal(3, circles.Count);
    }

    [Fact]
    public void Bubble_Series_HasAria_Labels()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Bubbles");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_bubbleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.CategoryField), "Label");
                builder.AddAttribute(5, nameof(MariloChartSeries.SizeField), "Size");
                builder.AddAttribute(6, nameof(MariloChartSeries.Type), ChartSeriesType.Bubble);
                builder.CloseComponent();
            }));

        // Bubbles should have aria-label with size info
        Assert.Contains("size:", cut.Markup);
    }

    [Fact]
    public void Bubble_Series_CirclesHave_FillOpacity()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Bubbles");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_bubbleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.CategoryField), "Label");
                builder.AddAttribute(5, nameof(MariloChartSeries.SizeField), "Size");
                builder.AddAttribute(6, nameof(MariloChartSeries.Type), ChartSeriesType.Bubble);
                builder.CloseComponent();
            }));

        // Bubbles should have fill-opacity for visual distinction
        Assert.Contains("fill-opacity", cut.Markup);
    }

    // ── GAP-CHART-012: Transitions nullable ─────────────────────────

    [Fact]
    public void Transitions_Defaults_To_Null()
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

        Assert.Null(cut.Instance.Transitions);
    }

    [Fact]
    public void Transitions_Can_Be_Set_False()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.Transitions, false)
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

        Assert.False(cut.Instance.Transitions);
    }

    // ── GAP-CHART-013: OnRender event ───────────────────────────────

    [Fact]
    public void OnRender_EventCallback_Accepted()
    {
        ChartRenderEventArgs? receivedArgs = null;

        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.OnRender, EventCallback.Factory.Create<ChartRenderEventArgs>(this, args => receivedArgs = args))
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

        // The OnRender fires after render
        Assert.NotNull(receivedArgs);
        Assert.Equal(1, receivedArgs!.SeriesCount);
        Assert.Equal(3, receivedArgs.TotalDataPoints);
    }

    [Fact]
    public void ChartRenderEventArgs_HasDimensions()
    {
        ChartRenderEventArgs? receivedArgs = null;

        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.Width, "800px")
            .Add(p => p.Height, "400px")
            .Add(p => p.OnRender, EventCallback.Factory.Create<ChartRenderEventArgs>(this, args => receivedArgs = args))
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

        Assert.NotNull(receivedArgs);
        Assert.Equal("800px", receivedArgs!.Width);
        Assert.Equal("400px", receivedArgs.Height);
    }

    // ── GAP-CHART-014: Tooltip template ─────────────────────────────

    [Fact]
    public void ChartTooltip_Template_Parameter_Accepted()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<ChartTooltip>(0);
                builder.AddAttribute(1, nameof(ChartTooltip.Visible), true);
                builder.AddAttribute(2, nameof(ChartTooltip.Template), (RenderFragment<ChartTooltipContext>)(context => inner =>
                {
                    inner.AddContent(0, $"Custom: {context.Category} = {context.Value}");
                }));
                builder.CloseComponent();

                builder.OpenComponent<MariloChartSeries>(10);
                builder.AddAttribute(11, nameof(MariloChartSeries.Name), "Sales");
                builder.AddAttribute(12, nameof(MariloChartSeries.Data), (IEnumerable<object>)_sampleData.Cast<object>().ToList());
                builder.AddAttribute(13, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(14, nameof(MariloChartSeries.CategoryField), "Month");
                builder.AddAttribute(15, nameof(MariloChartSeries.Type), ChartSeriesType.Line);
                builder.CloseComponent();
            }));

        // Tooltip element exists with display:none initially
        var tooltip = cut.Find(".mar-chart-tooltip");
        Assert.NotNull(tooltip);
    }

    [Fact]
    public void ChartTooltip_Shared_Parameter_Accepted()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<ChartTooltip>(0);
                builder.AddAttribute(1, nameof(ChartTooltip.Shared), true);
                builder.CloseComponent();

                builder.OpenComponent<MariloChartSeries>(10);
                builder.AddAttribute(11, nameof(MariloChartSeries.Name), "Sales");
                builder.AddAttribute(12, nameof(MariloChartSeries.Data), (IEnumerable<object>)_sampleData.Cast<object>().ToList());
                builder.AddAttribute(13, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(14, nameof(MariloChartSeries.CategoryField), "Month");
                builder.AddAttribute(15, nameof(MariloChartSeries.Type), ChartSeriesType.Line);
                builder.CloseComponent();
            }));

        // Chart renders without error with Shared tooltip
        Assert.NotNull(cut.Find("svg"));
    }

    [Fact]
    public void ChartTooltipContext_Has_Expected_Properties()
    {
        var context = new ChartTooltipContext
        {
            SeriesName = "Sales",
            Category = "Jan",
            Value = 100,
            FormattedValue = "100",
            Color = "#ff0000",
            DataItem = new TestData("Jan", 100),
            Percentage = 33.3
        };

        Assert.Equal("Sales", context.SeriesName);
        Assert.Equal("Jan", context.Category);
        Assert.Equal(100, context.Value);
        Assert.Equal("100", context.FormattedValue);
        Assert.Equal("#ff0000", context.Color);
        Assert.NotNull(context.DataItem);
        Assert.Equal(33.3, context.Percentage);
    }

    // ── GAP-CHART-011: Data binding alignment (already resolved) ────

    [Fact]
    public void ChartSeries_HasExpected_DataBinding_Parameters()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Sales");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_sampleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.Field), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.CategoryField), "Month");
                builder.AddAttribute(5, nameof(MariloChartSeries.XField), "X");
                builder.AddAttribute(6, nameof(MariloChartSeries.YField), "Y");
                builder.AddAttribute(7, nameof(MariloChartSeries.SizeField), "Size");
                builder.AddAttribute(8, nameof(MariloChartSeries.Type), ChartSeriesType.Line);
                builder.CloseComponent();
            }));

        // All data binding parameters are accepted
        Assert.NotNull(cut.Find("svg"));
    }

    // ── Regression: scatter/bubble with complex Data and no Field ────
    // Previously crashed with InvalidCastException because the primitive
    // branch short-circuited on empty Field and tried Convert.ToDouble(item).
    [Fact]
    public void Scatter_Series_With_ComplexData_And_NoField_Renders()
    {
        var cut = Render<MariloChart>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenComponent<MariloChartSeries>(0);
                builder.AddAttribute(1, nameof(MariloChartSeries.Name), "Scatter");
                builder.AddAttribute(2, nameof(MariloChartSeries.Data), (IEnumerable<object>)_bubbleData.Cast<object>().ToList());
                builder.AddAttribute(3, nameof(MariloChartSeries.XField), "Value");
                builder.AddAttribute(4, nameof(MariloChartSeries.YField), "Size");
                builder.AddAttribute(5, nameof(MariloChartSeries.Type), ChartSeriesType.Scatter);
                builder.CloseComponent();
            }));

        // Should render without throwing InvalidCastException.
        Assert.NotNull(cut.Find("svg"));
    }
}
