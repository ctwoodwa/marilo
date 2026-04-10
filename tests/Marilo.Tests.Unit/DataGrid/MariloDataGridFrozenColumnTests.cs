using Bunit;
using Marilo.Components.DataGrid;
using Marilo.Components.DataGrid.Sizing;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

public class MariloDataGridFrozenColumnTests : MariloTestBase
{
    private record Product(string Name, string Category, decimal Price, int Stock);

    private static readonly List<Product> TestData =
    [
        new("Widget", "Tools", 19.99m, 100),
        new("Gadget", "Electronics", 49.99m, 50),
        new("Doohickey", "Tools", 9.99m, 200),
    ];

    [Fact]
    public void Column_Without_Locked_Has_No_Sticky_Style()
    {
        var cut = Render<MariloDataGrid<Product>>(parameters => parameters
            .Add(p => p.Data, TestData)
            .AddChildContent<MariloGridColumn<Product>>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Width, "150px"))
            .AddChildContent<MariloGridColumn<Product>>(col => col
                .Add(c => c.Field, "Category")));

        var headers = cut.FindAll("thead th");
        foreach (var th in headers)
        {
            var style = th.GetAttribute("style") ?? "";
            Assert.DoesNotContain("position:sticky", style);
        }
    }

    [Fact]
    public void Locked_Column_Renders_Sticky_Style_On_Header()
    {
        var cut = Render<MariloDataGrid<Product>>(parameters => parameters
            .Add(p => p.Data, TestData)
            .AddChildContent<MariloGridColumn<Product>>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Width, "150px")
                .Add(c => c.Locked, true))
            .AddChildContent<MariloGridColumn<Product>>(col => col
                .Add(c => c.Field, "Category")));

        var headers = cut.FindAll("thead th");
        var firstHeader = headers[0];
        var style = firstHeader.GetAttribute("style") ?? "";
        Assert.Contains("position:sticky", style);
        Assert.Contains("left:0px", style);
        Assert.Contains("z-index:3", style);
    }

    [Fact]
    public void Locked_Column_Renders_Sticky_Style_On_Data_Cells()
    {
        var cut = Render<MariloDataGrid<Product>>(parameters => parameters
            .Add(p => p.Data, TestData)
            .AddChildContent<MariloGridColumn<Product>>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Width, "150px")
                .Add(c => c.Locked, true))
            .AddChildContent<MariloGridColumn<Product>>(col => col
                .Add(c => c.Field, "Category")));

        // Each row's first td should have sticky style
        var rows = cut.FindAll("tbody tr");
        foreach (var row in rows)
        {
            var firstCell = row.QuerySelector("td");
            Assert.NotNull(firstCell);
            var style = firstCell!.GetAttribute("style") ?? "";
            Assert.Contains("position:sticky", style);
            Assert.Contains("left:0px", style);
            Assert.Contains("z-index:2", style);
        }
    }

    [Fact]
    public void FrozenPosition_Start_Applies_Left_Offset()
    {
        var cut = Render<MariloDataGrid<Product>>(parameters => parameters
            .Add(p => p.Data, TestData)
            .AddChildContent<MariloGridColumn<Product>>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Width, "120px")
                .Add(c => c.Locked, true)
                .Add(c => c.FrozenPosition, GridColumnFrozenPosition.Start))
            .AddChildContent<MariloGridColumn<Product>>(col => col
                .Add(c => c.Field, "Category")));

        var header = cut.FindAll("thead th")[0];
        var style = header.GetAttribute("style") ?? "";
        Assert.Contains("left:", style);
        Assert.DoesNotContain("right:", style);
    }

    [Fact]
    public void FrozenPosition_End_Applies_Right_Offset()
    {
        var cut = Render<MariloDataGrid<Product>>(parameters => parameters
            .Add(p => p.Data, TestData)
            .AddChildContent<MariloGridColumn<Product>>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloGridColumn<Product>>(col => col
                .Add(c => c.Field, "Category")
                .Add(c => c.Width, "120px")
                .Add(c => c.Locked, true)
                .Add(c => c.FrozenPosition, GridColumnFrozenPosition.End)));

        var headers = cut.FindAll("thead th");
        var lastHeader = headers[^1];
        var style = lastHeader.GetAttribute("style") ?? "";
        Assert.Contains("right:", style);
        Assert.Contains("position:sticky", style);
    }

    [Fact]
    public void Multiple_Start_Frozen_Columns_Get_Cumulative_Offsets()
    {
        var cut = Render<MariloDataGrid<Product>>(parameters => parameters
            .Add(p => p.Data, TestData)
            .AddChildContent<MariloGridColumn<Product>>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Width, "100px")
                .Add(c => c.Locked, true)
                .Add(c => c.FrozenPosition, GridColumnFrozenPosition.Start))
            .AddChildContent<MariloGridColumn<Product>>(col => col
                .Add(c => c.Field, "Category")
                .Add(c => c.Width, "120px")
                .Add(c => c.Locked, true)
                .Add(c => c.FrozenPosition, GridColumnFrozenPosition.Start))
            .AddChildContent<MariloGridColumn<Product>>(col => col
                .Add(c => c.Field, "Price")));

        var headers = cut.FindAll("thead th");

        // First frozen column: offset 0
        var style0 = headers[0].GetAttribute("style") ?? "";
        Assert.Contains("left:0px", style0);

        // Second frozen column: offset = width of first (100px)
        var style1 = headers[1].GetAttribute("style") ?? "";
        Assert.Contains("left:100px", style1);
    }

    [Fact]
    public void FixedWidthProvider_Computes_FrozenOffsets_Correctly()
    {
        var provider = new FixedWidthProvider();
        var entries = new List<ColumnSizingEntry>
        {
            new("col-a", "100px", 50, null, null, true, GridColumnFrozenPosition.Start),
            new("col-b", "120px", 50, null, null, true, GridColumnFrozenPosition.Start),
            new("col-c", "200px", 50, null),
            new("col-d", "80px", 50, null, null, true, GridColumnFrozenPosition.End),
        };

        var contract = provider.Resolve(entries);

        Assert.Contains("col-a", contract.FrozenColumnIds);
        Assert.Contains("col-b", contract.FrozenColumnIds);
        Assert.Contains("col-d", contract.FrozenColumnIds);
        Assert.DoesNotContain("col-c", contract.FrozenColumnIds);

        Assert.Equal(0, contract.FrozenOffsets["col-a"]);
        Assert.Equal(100, contract.FrozenOffsets["col-b"]);
        Assert.Equal(0, contract.FrozenOffsets["col-d"]);

        Assert.Equal(GridColumnFrozenPosition.Start, contract.FrozenPositions["col-a"]);
        Assert.Equal(GridColumnFrozenPosition.End, contract.FrozenPositions["col-d"]);
    }

    [Fact]
    public void Frozen_Column_Without_Explicit_Width_Gets_Default_150px()
    {
        var provider = new FixedWidthProvider();
        var entries = new List<ColumnSizingEntry>
        {
            new("col-a", null, 50, null, null, true, GridColumnFrozenPosition.Start),
            new("col-b", null, 50, null, null, true, GridColumnFrozenPosition.Start),
        };

        var contract = provider.Resolve(entries);

        // First col at offset 0, second at 150 (default for non-pixel "auto" width)
        Assert.Equal(0, contract.FrozenOffsets["col-a"]);
        Assert.Equal(150, contract.FrozenOffsets["col-b"]);
    }

    [Fact]
    public void Locked_Column_Gets_Locked_Css_Class_On_Header()
    {
        var cut = Render<MariloDataGrid<Product>>(parameters => parameters
            .Add(p => p.Data, TestData)
            .AddChildContent<MariloGridColumn<Product>>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Width, "150px")
                .Add(c => c.Locked, true))
            .AddChildContent<MariloGridColumn<Product>>(col => col
                .Add(c => c.Field, "Category")));

        var headers = cut.FindAll("thead th");
        Assert.Contains("mar-datagrid-col--locked", headers[0].GetAttribute("class") ?? "");
        Assert.DoesNotContain("mar-datagrid-col--locked", headers[1].GetAttribute("class") ?? "");
    }
}
