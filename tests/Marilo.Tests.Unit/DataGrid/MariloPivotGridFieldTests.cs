using Bunit;
using Marilo.Components.DataGrid;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

public class MariloPivotGridFieldTests : MariloTestBase
{
    private static readonly List<object> SalesData = new()
    {
        new SalesRecord("North", "Widget A", 1200),
        new SalesRecord("North", "Widget B", 800),
        new SalesRecord("South", "Widget A", 950),
        new SalesRecord("South", "Widget B", 1100),
        new SalesRecord("East", "Widget A", 700),
        new SalesRecord("East", "Widget B", 1500),
    };

    private record SalesRecord(string Region, string Product, double Revenue);

    [Fact]
    public void PivotGrid_Renders_With_Child_Field_Components()
    {
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData)
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region")
                .Add(c => c.Title, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product")
                .Add(c => c.Title, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")
                .Add(c => c.Title, "Revenue")));

        // Should render the pivot table, not the empty state
        var table = cut.Find("table.mar-pivotgrid__table");
        Assert.NotNull(table);

        // Should have column headers for the products
        var colHeaders = cut.FindAll("th.mar-pivotgrid__col-header");
        Assert.Equal(2, colHeaders.Count); // Widget A, Widget B

        // Should have row headers for the regions
        var rowHeaders = cut.FindAll("th.mar-pivotgrid__row-header");
        Assert.Equal(3, rowHeaders.Count); // North, South, East
    }

    [Fact]
    public void Row_Column_Measure_Fields_Correctly_Categorized()
    {
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData)
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region")
                .Add(c => c.Title, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product")
                .Add(c => c.Title, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")
                .Add(c => c.Title, "Revenue")));

        // Row field "Region" should appear in corner header
        var corner = cut.Find("th.mar-pivotgrid__corner");
        Assert.Contains("Region", corner.TextContent);

        // Column field "Product" drives column headers
        var colHeaders = cut.FindAll("th.mar-pivotgrid__col-header");
        Assert.Contains(colHeaders, h => h.TextContent.Contains("Widget A"));
        Assert.Contains(colHeaders, h => h.TextContent.Contains("Widget B"));

        // Measure field "Revenue" drives cell values (Sum by default)
        var cells = cut.FindAll("td.mar-pivotgrid__cell");
        Assert.True(cells.Count > 0);
        // North + Widget A = 1200
        Assert.Contains(cells, c => c.TextContent.Contains("1,200"));
    }

#pragma warning disable CS0618 // Testing obsolete parameters intentionally
    [Fact]
    public void Old_Parameter_Based_Fields_Still_Work()
    {
        var rowFields = new List<PivotGridField> { new() { Name = "Region", Title = "Region" } };
        var colFields = new List<PivotGridField> { new() { Name = "Product", Title = "Product" } };
        var measureFields = new List<PivotGridField> { new() { Name = "Revenue", Title = "Revenue" } };

        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData)
            .Add(p => p.RowFields, rowFields)
            .Add(p => p.ColumnFields, colFields)
            .Add(p => p.MeasureFields, measureFields)
            .Add(p => p.AggregateFunction, PivotGridAggregateFunction.Sum));

        var table = cut.Find("table.mar-pivotgrid__table");
        Assert.NotNull(table);

        var colHeaders = cut.FindAll("th.mar-pivotgrid__col-header");
        Assert.Equal(2, colHeaders.Count);

        var rowHeaders = cut.FindAll("th.mar-pivotgrid__row-header");
        Assert.Equal(3, rowHeaders.Count);
    }

    [Fact]
    public void Child_Fields_Take_Precedence_Over_Parameter_Fields()
    {
        var legacyRowFields = new List<PivotGridField> { new() { Name = "Product", Title = "Product" } };
        var legacyColFields = new List<PivotGridField> { new() { Name = "Region", Title = "Region" } };
        var legacyMeasureFields = new List<PivotGridField> { new() { Name = "Revenue", Title = "Revenue" } };

        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData)
            .Add(p => p.RowFields, legacyRowFields)
            .Add(p => p.ColumnFields, legacyColFields)
            .Add(p => p.MeasureFields, legacyMeasureFields)
            // Child fields should win: Region as row, Product as column
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region")
                .Add(c => c.Title, "By Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product")
                .Add(c => c.Title, "By Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")));

        // Corner should show child field title "By Region", not legacy "Product"
        var corner = cut.Find("th.mar-pivotgrid__corner");
        Assert.Contains("By Region", corner.TextContent);

        // Column headers should be product values (from child column field), not region values
        var colHeaders = cut.FindAll("th.mar-pivotgrid__col-header");
        Assert.Contains(colHeaders, h => h.TextContent.Contains("Widget A"));
        Assert.Contains(colHeaders, h => h.TextContent.Contains("Widget B"));
    }
#pragma warning restore CS0618

    [Fact]
    public void Empty_PivotGrid_Renders_Without_Error()
    {
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, new List<object>()));

        // Should show empty state message
        var empty = cut.Find("div.mar-pivotgrid__empty");
        Assert.NotNull(empty);
    }

    [Fact]
    public void Aggregate_Function_Respected_In_Rendering()
    {
        // Use Count aggregate via child measure field
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData)
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")
                .Add(c => c.AggregateFunction, PivotGridAggregateFunction.Count)));

        var cells = cut.FindAll("td.mar-pivotgrid__cell");
        Assert.True(cells.Count > 0);

        // North has 1 Widget A and 1 Widget B, so count = 1 for each
        // All cells should contain small integer counts (1)
        Assert.Contains(cells, c => c.TextContent.Trim() == "1");
    }

    [Fact]
    public void PivotGrid_With_No_Fields_Shows_Empty_State()
    {
        // No fields registered and no parameters = empty
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData));

        var empty = cut.Find("div.mar-pivotgrid__empty");
        Assert.NotNull(empty);
    }

    [Fact]
    public void Sortable_Sorts_Row_And_Column_Keys()
    {
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData)
            .Add(p => p.Sortable, true)
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")));

        var rowHeaders = cut.FindAll("th.mar-pivotgrid__row-header");
        // Sorted: East, North, South
        Assert.Equal("East", rowHeaders[0].TextContent.Trim());
        Assert.Equal("North", rowHeaders[1].TextContent.Trim());
        Assert.Equal("South", rowHeaders[2].TextContent.Trim());
    }

    // ── Wave 3: Templates & Formatting ──────────────────────────────────

    [Fact]
    public void CellTemplate_Renders_Custom_Content()
    {
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData)
            .Add(p => p.CellTemplate, (RenderFragment<PivotGridCellContext>)(ctx => builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "class", "custom-cell");
                builder.AddContent(2, $"R:{ctx.RowKey}-C:{ctx.ColumnKey}-V:{ctx.FormattedValue}");
                builder.CloseElement();
            }))
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")));

        var customCells = cut.FindAll("span.custom-cell");
        Assert.True(customCells.Count > 0);

        // Check one specific cell: North + Widget A = 1200
        Assert.Contains(customCells, c => c.TextContent.Contains("R:North-C:Widget A-V:1,200"));
    }

    [Fact]
    public void RowHeaderTemplate_Renders_Custom_Row_Headers()
    {
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData)
            .Add(p => p.RowHeaderTemplate, (RenderFragment<string>)(text => builder =>
            {
                builder.OpenElement(0, "em");
                builder.AddAttribute(1, "class", "custom-row");
                builder.AddContent(2, $"Row: {text}");
                builder.CloseElement();
            }))
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")));

        var customHeaders = cut.FindAll("em.custom-row");
        Assert.Equal(3, customHeaders.Count); // North, South, East
        Assert.Contains(customHeaders, h => h.TextContent.Contains("Row: North"));
    }

    [Fact]
    public void ColumnHeaderTemplate_Renders_Custom_Column_Headers()
    {
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData)
            .Add(p => p.ColumnHeaderTemplate, (RenderFragment<string>)(text => builder =>
            {
                builder.OpenElement(0, "strong");
                builder.AddAttribute(1, "class", "custom-col");
                builder.AddContent(2, $"Col: {text}");
                builder.CloseElement();
            }))
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")));

        var customHeaders = cut.FindAll("strong.custom-col");
        Assert.Equal(2, customHeaders.Count); // Widget A, Widget B
        Assert.Contains(customHeaders, h => h.TextContent.Contains("Col: Widget A"));
        Assert.Contains(customHeaders, h => h.TextContent.Contains("Col: Widget B"));
    }

    [Fact]
    public void Format_String_Formats_Measure_Values()
    {
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData)
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")
                .Add(c => c.Format, "N1")));

        var cells = cut.FindAll("td.mar-pivotgrid__cell");
        Assert.True(cells.Count > 0);

        // North + Widget A = 1200 formatted as N1 -> "1,200.0" (or locale equivalent with one decimal)
        Assert.Contains(cells, c => c.TextContent.Trim().Contains("1,200.0"));
    }

    [Fact]
    public void Default_Rendering_Without_Templates_Still_Works()
    {
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData)
            .AddChildContent<MariloPivotGridRowField>(f => f
                .Add(c => c.Field, "Region"))
            .AddChildContent<MariloPivotGridColumnField>(f => f
                .Add(c => c.Field, "Product"))
            .AddChildContent<MariloPivotGridMeasureField>(f => f
                .Add(c => c.Field, "Revenue")));

        // Standard rendering: no custom elements, just text in cells
        var cells = cut.FindAll("td.mar-pivotgrid__cell");
        Assert.True(cells.Count > 0);
        Assert.Contains(cells, c => c.TextContent.Contains("1,200"));

        // Row headers are plain text
        var rowHeaders = cut.FindAll("th.mar-pivotgrid__row-header");
        Assert.Contains(rowHeaders, h => h.TextContent.Trim() == "North");

        // Column headers are plain text
        var colHeaders = cut.FindAll("th.mar-pivotgrid__col-header");
        Assert.Contains(colHeaders, h => h.TextContent.Trim() == "Widget A");

        // No custom elements present
        Assert.Empty(cut.FindAll("span.custom-cell"));
        Assert.Empty(cut.FindAll("em.custom-row"));
        Assert.Empty(cut.FindAll("strong.custom-col"));
    }
}
