using Bunit;
using Marilo.Components.DataGrid;
using Marilo.Core.Data;
using Marilo.Core.Enums;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

/// <summary>
/// Tests for DataGrid Phase 2 gap resolutions:
/// Composite filters, auto-gen with attributes, group aggregates,
/// export lifecycle, CancellationToken, popup validation.
/// </summary>
public class MariloDataGridPhase2Tests : MariloTestBase
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
    ];

    // Model with DataAnnotations for auto-generate tests
    private class AnnotatedProduct
    {
        [Display(Name = "Product ID", Order = 1)]
        public int Id { get; set; }

        [Display(Name = "Product Name", Order = 2)]
        public string Name { get; set; } = "";

        [Display(AutoGenerateField = false)]
        public string InternalCode { get; set; } = "";

        [Display(Order = 3)]
        [Editable(false)]
        public decimal Price { get; set; }

        public string Category { get; set; } = "";
    }

    // ── Composite Filter Tests ─────────────────────────────────────────

    [Fact]
    public async Task AddCompositeFilter_And_Requires_All_Conditions()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Department")));

        // AND filter: Department=Engineering AND Name contains "l"
        await cut.InvokeAsync(() => cut.Instance.AddCompositeFilter(new CompositeFilterDescriptor
        {
            LogicalOperator = FilterCompositionOperator.And,
            Filters =
            [
                new FilterDescriptor { Field = "Department", Operator = FilterOperator.Equals, Value = "Engineering" },
                new FilterDescriptor { Field = "Name", Operator = FilterOperator.Contains, Value = "l" }
            ]
        }));

        var rows = cut.FindAll("tbody tr");
        // Alice (Engineering, contains "l") and Carol (Engineering, contains "l") = 2
        Assert.Equal(2, rows.Count);
    }

    [Fact]
    public async Task AddCompositeFilter_Or_Matches_Any_Condition()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Department")));

        // OR filter: Department=Sales OR Department=Marketing
        await cut.InvokeAsync(() => cut.Instance.AddCompositeFilter(new CompositeFilterDescriptor
        {
            LogicalOperator = FilterCompositionOperator.Or,
            Filters =
            [
                new FilterDescriptor { Field = "Department", Operator = FilterOperator.Equals, Value = "Sales" },
                new FilterDescriptor { Field = "Department", Operator = FilterOperator.Equals, Value = "Marketing" }
            ]
        }));

        var rows = cut.FindAll("tbody tr");
        // Sales: David, Grace. Marketing: Bob, Frank = 4
        Assert.Equal(4, rows.Count);
    }

    [Fact]
    public async Task ClearCompositeFilters_Removes_All()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Department")));

        await cut.InvokeAsync(() => cut.Instance.AddCompositeFilter(new CompositeFilterDescriptor
        {
            LogicalOperator = FilterCompositionOperator.And,
            Filters = [new FilterDescriptor { Field = "Department", Operator = FilterOperator.Equals, Value = "Sales" }]
        }));

        Assert.Equal(2, cut.FindAll("tbody tr").Count);

        await cut.InvokeAsync(() => cut.Instance.ClearCompositeFilters());
        Assert.Equal(7, cut.FindAll("tbody tr").Count);
    }

    // ── Auto-Generate with Attributes Tests ────────────────────────────

    [Fact]
    public void AutoGenerate_Respects_Display_Name_Attribute()
    {
        var data = new List<AnnotatedProduct>
        {
            new() { Id = 1, Name = "Widget", InternalCode = "W001", Price = 9.99m, Category = "Tools" }
        };

        var cut = Render<MariloDataGrid<AnnotatedProduct>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.AutoGenerateColumns, true));

        var headers = cut.FindAll("th");
        var headerTexts = headers.Select(h => h.TextContent.Trim()).ToList();

        // [Display(Name = "Product ID")] should use the name
        Assert.Contains("Product ID", headerTexts);
        Assert.Contains("Product Name", headerTexts);
    }

    [Fact]
    public void AutoGenerate_Skips_AutoGenerateField_False()
    {
        var data = new List<AnnotatedProduct>
        {
            new() { Id = 1, Name = "Widget", InternalCode = "W001", Price = 9.99m, Category = "Tools" }
        };

        var cut = Render<MariloDataGrid<AnnotatedProduct>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.AutoGenerateColumns, true));

        var headers = cut.FindAll("th");
        var headerTexts = headers.Select(h => h.TextContent.Trim()).ToList();

        // [Display(AutoGenerateField = false)] should be skipped
        Assert.DoesNotContain("InternalCode", headerTexts);
        Assert.DoesNotContain("Internal Code", headerTexts);
    }

    [Fact]
    public void AutoGenerate_Respects_Display_Order()
    {
        var data = new List<AnnotatedProduct>
        {
            new() { Id = 1, Name = "Widget", InternalCode = "W001", Price = 9.99m, Category = "Tools" }
        };

        var cut = Render<MariloDataGrid<AnnotatedProduct>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.AutoGenerateColumns, true));

        var headers = cut.FindAll("th");
        var headerTexts = headers.Select(h => h.TextContent.Trim()).ToList();

        // Order: Product ID (1), Product Name (2), Price (3), then Category (no order = last)
        Assert.Equal("Product ID", headerTexts[0]);
        Assert.Equal("Product Name", headerTexts[1]);
    }

    [Fact]
    public void AutoGenerate_Respects_Editable_False_Attribute()
    {
        var data = new List<AnnotatedProduct>
        {
            new() { Id = 1, Name = "Widget", InternalCode = "W001", Price = 9.99m, Category = "Tools" }
        };

        var cut = Render<MariloDataGrid<AnnotatedProduct>>(p => p
            .Add(x => x.Data, data)
            .Add(x => x.AutoGenerateColumns, true)
            .Add(x => x.EditMode, GridEditMode.Popup));

        // Enter edit mode on the first row
        var editBtn = cut.FindAll("button").FirstOrDefault(b => b.TextContent == "Edit");
        Assert.NotNull(editBtn);
        editBtn!.Click();

        // Price has [Editable(false)] — should render as disabled in popup
        var popupFields = cut.FindAll(".mar-datagrid-popup-field");
        Assert.NotEmpty(popupFields);

        // Find the Price field's disabled input
        var priceField = popupFields.FirstOrDefault(f => f.TextContent.Contains("Price"));
        Assert.NotNull(priceField);
        var disabledInput = priceField!.QuerySelector("input[disabled]");
        Assert.NotNull(disabledInput);

        // Name has no [Editable] attribute, defaults to true — should NOT be disabled
        var nameField = popupFields.FirstOrDefault(f => f.TextContent.Contains("Product Name"));
        Assert.NotNull(nameField);
        var nameDisabled = nameField!.QuerySelector("input[disabled]");
        Assert.Null(nameDisabled);
    }

    // ── Group Aggregate Tests ──────────────────────────────────────────

    [Fact]
    public async Task Group_Aggregates_Compute_Correctly()
    {
        GridGroupHeaderContext<Employee>? capturedContext = null;

        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.Groupable, true)
            .Add(x => x.GroupFooterTemplate, (GridGroupHeaderContext<Employee> ctx) =>
            {
                capturedContext = ctx;
                return (Microsoft.AspNetCore.Components.RenderFragment)(b =>
                    b.AddContent(0, $"Sum: {ctx.Sum(e => e.Salary)}"));
            })
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Department"))
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Salary")));

        await cut.InvokeAsync(() => cut.Instance.GroupBy("Department"));

        Assert.NotNull(capturedContext);

        // Verify the context has working aggregate methods
        var sum = capturedContext!.Sum(e => e.Salary);
        Assert.True(sum > 0);

        var avg = capturedContext.Average(e => e.Salary);
        Assert.True(avg > 0);

        var min = capturedContext.Min(e => e.Salary);
        var max = capturedContext.Max(e => e.Salary);
        Assert.True(max >= min);
    }

    // ── Export Tests ───────────────────────────────────────────────────

    [Fact]
    public void ExportToCsv_Respects_ExportAllPages_False()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.Pageable, true)
            .Add(x => x.PageSize, 2)
            .Add(x => x.ExportAllPages, false)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")));

        var csv = cut.Instance.ExportToCsv();
        var lines = csv.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // Header + 2 data rows (current page only)
        Assert.Equal(3, lines.Length);
    }

    [Fact]
    public void ExportToCsv_ExportAllPages_True_Exports_All()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.Pageable, true)
            .Add(x => x.PageSize, 2)
            .Add(x => x.ExportAllPages, true)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")));

        var csv = cut.Instance.ExportToCsv();
        var lines = csv.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // Header + 7 data rows (all pages)
        Assert.Equal(8, lines.Length);
    }

    [Fact]
    public async Task ExportToCsvAsync_Fires_Lifecycle_Events()
    {
        var beforeFired = false;
        var afterFired = false;
        string? exportedData = null;

        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.OnBeforeExport, (GridExportEventArgs args) =>
            {
                beforeFired = true;
            })
            .Add(x => x.OnAfterExport, (GridExportEventArgs args) =>
            {
                afterFired = true;
                exportedData = args.Data;
            })
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")));

        await cut.InvokeAsync(() => cut.Instance.ExportToCsvAsync());

        Assert.True(beforeFired);
        Assert.True(afterFired);
        Assert.NotNull(exportedData);
        Assert.Contains("Alice", exportedData!);
    }

    [Fact]
    public async Task ExportToCsvAsync_Cancellable_Via_OnBeforeExport()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.OnBeforeExport, (GridExportEventArgs args) =>
            {
                args.IsCancelled = true;
            })
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")));

        var result = await cut.InvokeAsync(() => cut.Instance.ExportToCsvAsync());

        Assert.Equal(string.Empty, result);
    }

    // ── CancellationToken Tests ────────────────────────────────────────

    [Fact]
    public void GridReadEventArgs_Has_CancellationToken_Property()
    {
        var args = new GridReadEventArgs<Employee>
        {
            Request = new GridState(),
            CancellationToken = CancellationToken.None
        };

        Assert.Equal(CancellationToken.None, args.CancellationToken);
    }

    // ── Popup Validation Tests ─────────────────────────────────────────

    [Fact]
    public void Popup_EditForm_Contains_ValidationSummary()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .Add(x => x.EditMode, GridEditMode.Popup)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.EditorTemplate, (Employee e) => (Microsoft.AspNetCore.Components.RenderFragment)(b =>
                {
                    b.OpenElement(0, "input");
                    b.AddAttribute(1, "type", "text");
                    b.AddAttribute(2, "value", e.Name);
                    b.CloseElement();
                }))));

        // Click Edit to open popup
        var editBtn = cut.FindAll("button").FirstOrDefault(b => b.TextContent == "Edit");
        Assert.NotNull(editBtn);
        editBtn!.Click();

        // Popup should contain EditForm with ValidationSummary
        var validationSummary = cut.FindAll(".mar-datagrid-validation-summary");
        Assert.Single(validationSummary);

        // Save button should be type="submit"
        var saveBtn = cut.FindAll(".mar-datagrid-popup-actions button").FirstOrDefault(b => b.TextContent == "Save");
        Assert.NotNull(saveBtn);
        Assert.Equal("submit", saveBtn!.GetAttribute("type"));
    }

    // ── CompositeFilterDescriptor in State Tests ──────────────────────

    [Fact]
    public async Task GetState_Includes_CompositeFilterDescriptors()
    {
        var cut = Render<MariloDataGrid<Employee>>(p => p
            .Add(x => x.Data, TestData)
            .AddChildContent<MariloGridColumn<Employee>>(col => col
                .Add(c => c.Field, "Department")));

        await cut.InvokeAsync(() => cut.Instance.AddCompositeFilter(new CompositeFilterDescriptor
        {
            LogicalOperator = FilterCompositionOperator.Or,
            Filters =
            [
                new FilterDescriptor { Field = "Department", Operator = FilterOperator.Equals, Value = "Sales" },
                new FilterDescriptor { Field = "Department", Operator = FilterOperator.Equals, Value = "Marketing" }
            ]
        }));

        var state = cut.Instance.GetState();
        Assert.Single(state.CompositeFilterDescriptors);
        Assert.Equal(FilterCompositionOperator.Or, state.CompositeFilterDescriptors[0].LogicalOperator);
        Assert.Equal(2, state.CompositeFilterDescriptors[0].Filters.Count);
    }
}
