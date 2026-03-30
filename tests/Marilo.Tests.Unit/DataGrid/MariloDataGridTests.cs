using Bunit;
using Marilo.Components.DataGrid;
using Marilo.Core.Enums;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

public class MariloDataGridTests : MariloTestBase
{
    private record Employee(string Name, string Department, DateTime HireDate, decimal Salary);

    private static readonly List<Employee> TestData =
    [
        new("Alice", "Engineering", new DateTime(2019, 3, 15), 95000m),
        new("Bob", "Marketing", new DateTime(2020, 7, 1), 72000m),
        new("Carol", "Engineering", new DateTime(2018, 1, 10), 105000m),
        new("David", "Sales", new DateTime(2021, 11, 20), 68000m),
        new("Eve", "Engineering", new DateTime(2022, 5, 5), 88000m),
    ];

    [Fact]
    public void Grid_Renders_Correct_Number_Of_Data_Rows()
    {
        var cut = Render<MariloDataGrid<Employee>>(parameters => parameters
            .Add(p => p.Data, TestData)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Department")));

        var rows = cut.FindAll("tbody tr");
        Assert.Equal(5, rows.Count);
    }

    [Fact]
    public void Grid_Renders_Column_Headers_From_Field()
    {
        var cut = Render<MariloDataGrid<Employee>>(parameters => parameters
            .Add(p => p.Data, TestData)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Department")
                .Add(c => c.Title, "Dept")));

        var headers = cut.FindAll("th");
        Assert.Equal(2, headers.Count);
        Assert.Contains("Name", headers[0].TextContent);
        Assert.Contains("Dept", headers[1].TextContent);
    }

    [Fact]
    public void Sort_Click_Changes_Sort_State()
    {
        var cut = Render<MariloDataGrid<Employee>>(parameters => parameters
            .Add(p => p.Data, TestData)
            .Add(p => p.Sortable, true)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Sortable, true)));

        var header = cut.Find("th");

        // Click to sort ascending
        header.Click();
        var firstRow = cut.Find("tbody tr td");
        Assert.Contains("Alice", firstRow.TextContent);

        // Click again to sort descending
        header.Click();
        firstRow = cut.Find("tbody tr td");
        Assert.Contains("Eve", firstRow.TextContent);
    }

    [Fact]
    public void Pager_Shows_Correct_Page_Count()
    {
        var cut = Render<MariloDataGrid<Employee>>(parameters => parameters
            .Add(p => p.Data, TestData)
            .Add(p => p.Pageable, true)
            .Add(p => p.PageSize, 2)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")));

        var pagerInfo = cut.Find(".mar-datagrid-pager-info");
        Assert.Contains("Page 1 of 3", pagerInfo.TextContent);

        // Only 2 rows should be displayed
        var rows = cut.FindAll("tbody tr");
        Assert.Equal(2, rows.Count);
    }

    [Fact]
    public void Selected_Items_Update_When_Row_Clicked()
    {
        IEnumerable<Employee>? selectedItems = null;

        var cut = Render<MariloDataGrid<Employee>>(parameters => parameters
            .Add(p => p.Data, TestData)
            .Add(p => p.SelectionMode, GridSelectionMode.Single)
            .Add(p => p.SelectedItemsChanged, (IEnumerable<Employee> items) => selectedItems = items)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")));

        var firstRow = cut.Find("tbody tr");
        firstRow.Click();

        Assert.NotNull(selectedItems);
        var selected = selectedItems!.ToList();
        Assert.Single(selected);
        Assert.Equal("Alice", selected[0].Name);
    }
}
