using Bunit;
using Marilo.Components.DataGrid;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

/// <summary>
/// GREEN-push tests: Rebind(), null-data edge cases, empty-state accessibility,
/// keyboard nav basics, and spec-accuracy validations.
/// </summary>
public class MariloPivotGridGreenTests : MariloTestBase
{
    private static readonly List<object> SalesData = new()
    {
        new SalesRecord("North", "Widget A", 1200),
        new SalesRecord("North", "Widget B", 800),
        new SalesRecord("South", "Widget A", 950),
        new SalesRecord("South", "Widget B", 1100),
    };

    private record SalesRecord(string Region, string Product, double Revenue);

    // ── Rebind ──────────────────────────────────────────────────────────

    [Fact]
    public async Task Rebind_Recomputes_Aggregates_After_Data_Mutation()
    {
        var data = new List<object>(SalesData);

        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, data)
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")));

        // Initial: North + Widget A = 1200
        var cells = cut.FindAll("td.mar-pivotgrid__cell");
        Assert.Contains(cells, c => c.TextContent.Contains("1,200"));

        // Mutate the data in-place
        data.Add(new SalesRecord("North", "Widget A", 300));

        // Before Rebind, the old value is still rendered
        cells = cut.FindAll("td.mar-pivotgrid__cell");
        Assert.Contains(cells, c => c.TextContent.Contains("1,200"));

        // After Rebind, the new sum should appear (1200 + 300 = 1500)
        await cut.InvokeAsync(() => cut.Instance.Rebind());

        cells = cut.FindAll("td.mar-pivotgrid__cell");
        Assert.Contains(cells, c => c.TextContent.Contains("1,500"));
    }

    [Fact]
    public async Task Rebind_On_Empty_Data_Shows_Empty_State()
    {
        var data = new List<object>(SalesData);

        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, data)
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")));

        // Initially has data
        Assert.NotNull(cut.Find("table.mar-pivotgrid__table"));

        // Clear data and rebind
        data.Clear();
        await cut.InvokeAsync(() => cut.Instance.Rebind());

        // Should now show empty state
        var empty = cut.Find(".mar-pivotgrid__empty");
        Assert.NotNull(empty);
        Assert.Equal("status", empty.GetAttribute("role"));
    }

    // ── Null / edge-case data ───────────────────────────────────────────

    [Fact]
    public void Null_Data_Renders_Empty_State()
    {
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, (IEnumerable<object>?)null)
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")));

        var empty = cut.Find(".mar-pivotgrid__empty");
        Assert.NotNull(empty);
        Assert.Equal("status", empty.GetAttribute("role"));
    }

    [Fact]
    public void Empty_Collection_With_Fields_Renders_Empty_State()
    {
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, new List<object>())
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")));

        // Empty data means no keys computed, so empty state renders
        var empty = cut.Find(".mar-pivotgrid__empty");
        Assert.NotNull(empty);
    }

    // ── Empty state messaging ───────────────────────────────────────────

    [Fact]
    public void Empty_State_Contains_Instructional_Message()
    {
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData));
        // No fields = empty state

        var empty = cut.Find(".mar-pivotgrid__empty");
        Assert.Contains("Configure", empty.TextContent);
        Assert.Contains("row", empty.TextContent.ToLowerInvariant());
        Assert.Contains("column", empty.TextContent.ToLowerInvariant());
        Assert.Contains("measure", empty.TextContent.ToLowerInvariant());
    }

    // ── Width/Height sizing ─────────────────────────────────────────────

    [Fact]
    public void Width_And_Height_Applied_To_Container_Style()
    {
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData)
            .Add(p => p.Width, "600px")
            .Add(p => p.Height, "400px")
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")));

        var root = cut.Find(".mar-pivotgrid");
        var style = root.GetAttribute("style") ?? "";
        Assert.Contains("width:600px", style);
        Assert.Contains("height:400px", style);
    }

    // ── Aggregate functions edge cases ──────────────────────────────────

    [Fact]
    public void Average_Aggregate_Computes_Correctly()
    {
        // Add duplicate entries for a known cell
        var data = new List<object>
        {
            new SalesRecord("North", "Widget A", 100),
            new SalesRecord("North", "Widget A", 200),
            new SalesRecord("South", "Widget A", 300),
        };

        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, data)
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")
                .Add(c => c.AggregateFunction, PivotGridAggregateFunction.Average)));

        var cells = cut.FindAll("td.mar-pivotgrid__cell");
        // North + Widget A: avg(100, 200) = 150
        Assert.Contains(cells, c => c.TextContent.Trim() == "150");
        // South + Widget A: avg(300) = 300
        Assert.Contains(cells, c => c.TextContent.Trim() == "300");
    }

    [Fact]
    public void Min_Aggregate_Computes_Correctly()
    {
        var data = new List<object>
        {
            new SalesRecord("North", "Widget A", 100),
            new SalesRecord("North", "Widget A", 200),
        };

        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, data)
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")
                .Add(c => c.AggregateFunction, PivotGridAggregateFunction.Min)));

        var cells = cut.FindAll("td.mar-pivotgrid__cell");
        Assert.Contains(cells, c => c.TextContent.Trim() == "100");
    }

    [Fact]
    public void Max_Aggregate_Computes_Correctly()
    {
        var data = new List<object>
        {
            new SalesRecord("North", "Widget A", 100),
            new SalesRecord("North", "Widget A", 200),
        };

        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, data)
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")
                .Add(c => c.AggregateFunction, PivotGridAggregateFunction.Max)));

        var cells = cut.FindAll("td.mar-pivotgrid__cell");
        Assert.Contains(cells, c => c.TextContent.Trim() == "200");
    }

    // ── CellContext completeness ────────────────────────────────────────

    [Fact]
    public void CellTemplate_Context_Exposes_All_Properties()
    {
        PivotGridCellContext? capturedContext = null;

        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData)
            .Add(p => p.CellTemplate, (RenderFragment<PivotGridCellContext>)(ctx => builder =>
            {
                capturedContext ??= ctx; // capture first cell context
                builder.AddContent(0, ctx.FormattedValue);
            }))
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")));

        Assert.NotNull(capturedContext);
        Assert.False(string.IsNullOrEmpty(capturedContext!.RowKey));
        Assert.False(string.IsNullOrEmpty(capturedContext.ColumnKey));
        Assert.False(string.IsNullOrEmpty(capturedContext.MeasureField));
        Assert.Equal("Revenue", capturedContext.MeasureField);
        Assert.Equal(PivotGridAggregateFunction.Sum, capturedContext.AggregateFunction);
        Assert.NotNull(capturedContext.Value);
        Assert.False(string.IsNullOrEmpty(capturedContext.FormattedValue));
    }

    // ── AdditionalAttributes pass-through ───────────────────────────────

    [Fact]
    public void AdditionalAttributes_Applied_To_Root_Element()
    {
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData)
            .AddUnmatched("data-testid", "my-pivot")
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")));

        var root = cut.Find(".mar-pivotgrid");
        Assert.Equal("my-pivot", root.GetAttribute("data-testid"));
    }
}
