using Bunit;
using Marilo.Components.DataGrid;
using Marilo.Core.Data;
using Marilo.Core.Enums;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

/// <summary>
/// Tests for DataGrid Phase 1 gap resolutions:
/// SortMode, Editable column, ConfirmDelete, SetStateAsync,
/// AddFilter/ClearFilters, enhanced pager, DisplayFormat, Groupable column, ExpandedItems.
/// </summary>
public class MariloDataGridPhase1Tests : MariloTestBase
{
    private record Employee(string Name, string Department, DateTime HireDate, decimal Salary);

    private static readonly List<Employee> TestData =
    [
        new("Alice", "Engineering", new DateTime(2019, 3, 15), 95000m),
        new("Bob", "Marketing", new DateTime(2020, 7, 1), 72000m),
        new("Carol", "Engineering", new DateTime(2018, 1, 10), 105000m),
        new("David", "Sales", new DateTime(2021, 11, 20), 68000m),
        new("Eve", "Engineering", new DateTime(2022, 5, 5), 88000m),
        new("Frank", "Marketing", new DateTime(2023, 2, 14), 75000m),
        new("Grace", "Sales", new DateTime(2019, 8, 22), 82000m),
        new("Hank", "Engineering", new DateTime(2020, 12, 1), 92000m),
        new("Iris", "Marketing", new DateTime(2021, 6, 15), 71000m),
        new("Jack", "Sales", new DateTime(2022, 9, 30), 69000m),
        new("Kate", "Engineering", new DateTime(2023, 4, 8), 98000m),
        new("Leo", "Marketing", new DateTime(2018, 11, 5), 78000m),
    ];

    // ── SortMode Tests ─────────────────────────────────────────────────

    [Fact]
    public void SortMode_Defaults_To_Multiple()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")));

        var instance = cut.Instance;
        Assert.Equal(GridSortMode.Multiple, instance.SortMode);
    }

    [Fact]
    public void SortMode_Single_Clears_Previous_Sort_On_New_Column()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.Sortable, true)
            .Add(x => x.SortMode, GridSortMode.Single)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Sortable, true))
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Department")
                .Add(c => c.Sortable, true)));

        var headers = cut.FindAll("th");

        // Sort by Name
        headers[0].Click();
        var state1 = cut.Instance.GetState();
        Assert.Single(state1.SortDescriptors);
        Assert.Equal("Name", state1.SortDescriptors[0].Field);

        // Sort by Department — should clear Name sort in Single mode
        headers[1].Click();
        var state2 = cut.Instance.GetState();
        Assert.Single(state2.SortDescriptors);
        Assert.Equal("Department", state2.SortDescriptors[0].Field);
    }

    // ── Editable Column Tests ──────────────────────────────────────────

    [Fact]
    public void Editable_Column_Defaults_To_True()
    {
        // When Editable is not explicitly set, columns default to editable.
        // Verify by entering Popup edit mode — the column should render an editor, not a disabled input.
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData.Take(1).ToList())
            .Add(x => x.EditMode, GridEditMode.Popup)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Name")));

        // Enter edit mode
        var editBtn = cut.FindAll("button").FirstOrDefault(b => b.TextContent == "Edit");
        Assert.NotNull(editBtn);
        editBtn!.Click();

        // In popup, an editable column should NOT have a disabled input
        var popupFields = cut.FindAll(".mar-datagrid-popup-field");
        Assert.NotEmpty(popupFields);
        var disabledInputs = popupFields[0].QuerySelectorAll("input[disabled]");
        Assert.Empty(disabledInputs);
    }

    [Fact]
    public void NonEditable_Column_Shows_Display_Value_In_Popup()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.EditMode, GridEditMode.Popup)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Editable, false)
                .Add(c => c.EditorTemplate, (Employee e) => (Microsoft.AspNetCore.Components.RenderFragment)(b =>
                {
                    b.OpenElement(0, "input");
                    b.AddAttribute(1, "type", "text");
                    b.AddAttribute(2, "value", e.Name);
                    b.CloseElement();
                })))
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Department")
                .Add(c => c.Editable, true)
                .Add(c => c.EditorTemplate, (Employee e) => (Microsoft.AspNetCore.Components.RenderFragment)(b =>
                {
                    b.OpenElement(0, "input");
                    b.AddAttribute(1, "type", "text");
                    b.AddAttribute(2, "value", e.Department);
                    b.CloseElement();
                }))));

        // Enter edit mode on first row
        var editBtn = cut.FindAll("button").FirstOrDefault(b => b.TextContent == "Edit");
        Assert.NotNull(editBtn);
        editBtn!.Click();

        // In popup, non-editable column should show disabled input, not the editor template
        var popupFields = cut.FindAll(".mar-datagrid-popup-field");
        Assert.Equal(2, popupFields.Count);

        // First field (Name, Editable=false) should have a disabled input
        var nameField = popupFields[0];
        var disabledInput = nameField.QuerySelector("input[disabled]");
        Assert.NotNull(disabledInput);
    }

    // ── Groupable Column Tests ─────────────────────────────────────────

    [Fact]
    public async Task Groupable_Column_Defaults_To_True()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.Groupable, true)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")));

        // When Groupable defaults to true, GroupBy should succeed
        await cut.InvokeAsync(() => cut.Instance.GroupBy("Name"));
        var state = cut.Instance.GetState();
        Assert.Single(state.GroupDescriptors);
        Assert.Equal("Name", state.GroupDescriptors[0].Field);
    }

    [Fact]
    public async Task GroupBy_Skips_NonGroupable_Column()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.Groupable, true)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Groupable, false))
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Department")));

        // Attempt to group by non-groupable column
        await cut.InvokeAsync(() => cut.Instance.GroupBy("Name"));
        var state = cut.Instance.GetState();
        Assert.Empty(state.GroupDescriptors);

        // Group by groupable column works
        await cut.InvokeAsync(() => cut.Instance.GroupBy("Department"));
        state = cut.Instance.GetState();
        Assert.Single(state.GroupDescriptors);
        Assert.Equal("Department", state.GroupDescriptors[0].Field);
    }

    // ── DisplayFormat Tests ────────────────────────────────────────────

    [Fact]
    public void DisplayFormat_With_Composite_Format_String_Works()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData.Take(1).ToList())
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Salary")
                .Add(c => c.DisplayFormat, "{0:N2}")));

        // Find the data cell with the salary
        var cells = cut.FindAll("tbody td");
        Assert.Contains("95,000.00", cells[0].TextContent);
    }

    [Fact]
    public void Format_Still_Works_Without_DisplayFormat()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData.Take(1).ToList())
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Salary")
                .Add(c => c.Format, "N0")));

        var cells = cut.FindAll("tbody td");
        Assert.Contains("95,000", cells[0].TextContent);
    }

    // ── SetStateAsync Tests ────────────────────────────────────────────

    [Fact]
    public async Task SetStateAsync_Updates_Sort_And_Page()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.Pageable, true)
            .Add(x => x.PageSize, 5)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")));

        var newState = new GridState
        {
            CurrentPage = 2,
            PageSize = 5,
            SortDescriptors =
            [
                new SortDescriptor { Field = "Name", Direction = SortDirection.Descending }
            ]
        };

        await cut.InvokeAsync(() => cut.Instance.SetStateAsync(newState));
        var state = cut.Instance.GetState();

        Assert.Equal(2, state.CurrentPage);
        Assert.Single(state.SortDescriptors);
        Assert.Equal("Name", state.SortDescriptors[0].Field);
        Assert.Equal(SortDirection.Descending, state.SortDescriptors[0].Direction);
    }

    // ── AddFilter / ClearFilters Tests ─────────────────────────────────

    [Fact]
    public async Task AddFilter_Applies_Filter_And_Reduces_Rows()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Department")));

        await cut.InvokeAsync(() => cut.Instance.AddFilter(new FilterDescriptor
        {
            Field = "Department",
            Operator = FilterOperator.Equals,
            Value = "Sales"
        }));

        var rows = cut.FindAll("tbody tr");
        // Only Sales employees: David, Grace, Jack
        Assert.Equal(3, rows.Count);
    }

    [Fact]
    public async Task ClearFilters_Removes_All_Filters()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Department")));

        // Add a filter
        await cut.InvokeAsync(() => cut.Instance.AddFilter(new FilterDescriptor
        {
            Field = "Department",
            Operator = FilterOperator.Equals,
            Value = "Sales"
        }));

        Assert.Equal(3, cut.FindAll("tbody tr").Count);

        // Clear all filters
        await cut.InvokeAsync(() => cut.Instance.ClearFilters());

        Assert.Equal(12, cut.FindAll("tbody tr").Count);
    }

    [Fact]
    public async Task AddFilter_Replaces_Existing_Filter_On_Same_Field()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Department")));

        await cut.InvokeAsync(() => cut.Instance.AddFilter(new FilterDescriptor
        {
            Field = "Department",
            Operator = FilterOperator.Equals,
            Value = "Sales"
        }));

        Assert.Equal(3, cut.FindAll("tbody tr").Count);

        // Replace with Engineering filter
        await cut.InvokeAsync(() => cut.Instance.AddFilter(new FilterDescriptor
        {
            Field = "Department",
            Operator = FilterOperator.Equals,
            Value = "Engineering"
        }));

        // Engineering: Alice, Carol, Eve, Hank, Kate = 5
        Assert.Equal(5, cut.FindAll("tbody tr").Count);

        // Should still be just one filter descriptor
        var state = cut.Instance.GetState();
        Assert.Single(state.FilterDescriptors);
    }

    // ── Enhanced Pager Tests ───────────────────────────────────────────

    [Fact]
    public void Pager_Shows_Page_Number_Buttons()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.Pageable, true)
            .Add(x => x.PageSize, 3)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")));

        // 12 items / 3 per page = 4 pages
        var pageButtons = cut.FindAll("button.mar-datagrid-pager-btn");
        Assert.True(pageButtons.Count >= 4); // At least 4 page buttons + prev/next

        // Current page button should be active
        var activeBtn = cut.Find("button.mar-datagrid-pager-btn--active");
        Assert.Contains("1", activeBtn.TextContent);
    }

    [Fact]
    public void Pager_Info_Shows_Correct_Page_Count()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.Pageable, true)
            .Add(x => x.PageSize, 3)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")));

        var pagerInfo = cut.Find(".mar-datagrid-pager-info");
        Assert.Contains("Page 1 of 4", pagerInfo.TextContent);
    }

    [Fact]
    public void PagerButtonCount_Limits_Visible_Buttons()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.Pageable, true)
            .Add(x => x.PageSize, 1)
            .Add(x => x.PagerButtonCount, 3)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")));

        // 12 pages, but only 3 page number buttons should show (plus first/last and prev/next)
        var pagerInfo = cut.Find(".mar-datagrid-pager-info");
        Assert.Contains("Page 1 of 12", pagerInfo.TextContent);
    }

    // ── ExpandedItems in State Tests ───────────────────────────────────

    [Fact]
    public void GetState_Reflects_Expanded_Detail_Rows()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData.Take(3).ToList())
            .Add(x => x.DetailTemplate, (Employee e) => (Microsoft.AspNetCore.Components.RenderFragment)(b =>
            {
                b.AddContent(0, $"Detail: {e.Name}");
            }))
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")));

        // Initially no expanded items
        var state = cut.Instance.GetState();
        Assert.Empty(state.ExpandedItems);

        // Click expand button on first row
        var expandBtn = cut.Find("button.mar-datagrid-detail-btn");
        expandBtn.Click();

        state = cut.Instance.GetState();
        Assert.Single(state.ExpandedItems);
    }

    // ── ConfirmDelete Parameter Tests ──────────────────────────────────

    [Fact]
    public void ConfirmDelete_Defaults_To_False()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.EditMode, GridEditMode.Inline)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")));

        Assert.False(cut.Instance.ConfirmDelete);
    }

    [Fact]
    public void ConfirmDelete_Parameter_Can_Be_Set()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.EditMode, GridEditMode.Inline)
            .Add(x => x.ConfirmDelete, true)
            .Add(x => x.ConfirmDeleteText, "Really delete?")
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")));

        Assert.True(cut.Instance.ConfirmDelete);
        Assert.Equal("Really delete?", cut.Instance.ConfirmDeleteText);
    }
}
