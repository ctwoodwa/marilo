using Bunit;
using Marilo.Components.DataGrid;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

public class MariloDataGridPhase3Tests : MariloTestBase
{
    private record Product(int Id, string Name, string Category, decimal Price);

    private static readonly List<Product> Products =
    [
        new(1, "Widget A", "Tools", 10.0m),
        new(2, "Widget B", "Tools", 20.0m),
        new(3, "Gadget C", "Electronics", 30.0m),
        new(4, "Gadget D", "Electronics", 40.0m),
        new(5, "Gizmo E", "Toys", 15.0m),
    ];

    // ── DG-P3-04: CheckBoxList filter mode ─────────────────────────────

    [Fact]
    public void CheckBoxList_FilterMode_Enum_Exists()
    {
        Assert.Equal(3, (int)GridFilterMode.CheckBoxList);
    }

    [Fact]
    public void CheckBoxList_FilterMode_Renders_FilterButton()
    {
        var cut = Render<MariloDataGrid<Product>>(p => p
            .Add(x => x.Data, Products)
            .Add(x => x.FilterMode, GridFilterMode.CheckBoxList)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloGridColumn<Product>>(0);
                builder.AddAttribute(1, nameof(MariloGridColumn<Product>.Field), "Category");
                builder.AddAttribute(2, nameof(MariloGridColumn<Product>.Title), "Category");
                builder.AddAttribute(3, nameof(MariloGridColumn<Product>.Filterable), true);
                builder.CloseComponent();
            }));

        // Filter button should be present in the header
        var filterBtns = cut.FindAll(".mar-datagrid-filter-menu-btn");
        Assert.True(filterBtns.Count >= 1);
    }

    [Fact]
    public void CheckBoxList_FilterButton_OpensPopup()
    {
        var cut = Render<MariloDataGrid<Product>>(p => p
            .Add(x => x.Data, Products)
            .Add(x => x.FilterMode, GridFilterMode.CheckBoxList)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloGridColumn<Product>>(0);
                builder.AddAttribute(1, nameof(MariloGridColumn<Product>.Field), "Category");
                builder.AddAttribute(2, nameof(MariloGridColumn<Product>.Title), "Category");
                builder.AddAttribute(3, nameof(MariloGridColumn<Product>.Filterable), true);
                builder.CloseComponent();
            }));

        // Click the filter button
        var filterBtn = cut.Find(".mar-datagrid-filter-menu-btn");
        filterBtn.Click();

        // Checkbox filter popup should open
        var checkboxItems = cut.FindAll(".mar-datagrid-filter-checkbox-item");
        Assert.True(checkboxItems.Count > 0, "Should show distinct values as checkbox items");
    }

    [Fact]
    public void CheckBoxList_Shows_Distinct_Values()
    {
        var cut = Render<MariloDataGrid<Product>>(p => p
            .Add(x => x.Data, Products)
            .Add(x => x.FilterMode, GridFilterMode.CheckBoxList)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloGridColumn<Product>>(0);
                builder.AddAttribute(1, nameof(MariloGridColumn<Product>.Field), "Category");
                builder.AddAttribute(2, nameof(MariloGridColumn<Product>.Title), "Category");
                builder.AddAttribute(3, nameof(MariloGridColumn<Product>.Filterable), true);
                builder.CloseComponent();
            }));

        var filterBtn = cut.Find(".mar-datagrid-filter-menu-btn");
        filterBtn.Click();

        // Should show 3 distinct categories: Electronics, Tools, Toys
        var checkboxItems = cut.FindAll(".mar-datagrid-filter-checkbox-item");
        Assert.Equal(3, checkboxItems.Count);
    }

    [Fact]
    public void CheckBoxList_HasApplyAndClearButtons()
    {
        var cut = Render<MariloDataGrid<Product>>(p => p
            .Add(x => x.Data, Products)
            .Add(x => x.FilterMode, GridFilterMode.CheckBoxList)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloGridColumn<Product>>(0);
                builder.AddAttribute(1, nameof(MariloGridColumn<Product>.Field), "Category");
                builder.AddAttribute(2, nameof(MariloGridColumn<Product>.Title), "Category");
                builder.AddAttribute(3, nameof(MariloGridColumn<Product>.Filterable), true);
                builder.CloseComponent();
            }));

        var filterBtn = cut.Find(".mar-datagrid-filter-menu-btn");
        filterBtn.Click();

        // Should have Apply and Clear buttons
        var buttons = cut.FindAll(".mar-datagrid-filter-menu-actions button");
        Assert.Equal(2, buttons.Count);
    }

    // ── DG-P3-02: Cell selection ───────────────────────────────────────

    [Fact]
    public void GridSelectionUnit_Enum_Exists()
    {
        Assert.Equal(0, (int)GridSelectionUnit.Row);
        Assert.Equal(1, (int)GridSelectionUnit.Cell);
    }

    [Fact]
    public void SelectionUnit_Defaults_To_Row()
    {
        var cut = Render<MariloDataGrid<Product>>(p => p
            .Add(x => x.Data, Products)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloGridColumn<Product>>(0);
                builder.AddAttribute(1, nameof(MariloGridColumn<Product>.Field), "Name");
                builder.AddAttribute(2, nameof(MariloGridColumn<Product>.Title), "Name");
                builder.CloseComponent();
            }));

        Assert.Equal(GridSelectionUnit.Row, cut.Instance.SelectionUnit);
    }

    [Fact]
    public void SelectionUnit_Cell_Parameter_Accepted()
    {
        var cut = Render<MariloDataGrid<Product>>(p => p
            .Add(x => x.Data, Products)
            .Add(x => x.SelectionUnit, GridSelectionUnit.Cell)
            .Add(x => x.SelectionMode, GridSelectionMode.Single)
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloGridColumn<Product>>(0);
                builder.AddAttribute(1, nameof(MariloGridColumn<Product>.Field), "Name");
                builder.AddAttribute(2, nameof(MariloGridColumn<Product>.Title), "Name");
                builder.CloseComponent();
            }));

        Assert.Equal(GridSelectionUnit.Cell, cut.Instance.SelectionUnit);
    }

    [Fact]
    public void GridCellReference_HasExpectedProperties()
    {
        var product = new Product(1, "Test", "Cat", 10m);
        var cellRef = new GridCellReference<Product>
        {
            Item = product,
            Field = "Name",
            Value = "Test",
            RowIndex = 0
        };

        Assert.Equal(product, cellRef.Item);
        Assert.Equal("Name", cellRef.Field);
        Assert.Equal("Test", cellRef.Value);
        Assert.Equal(0, cellRef.RowIndex);
    }

    [Fact]
    public void CellSelection_SelectedCellsChanged_EventCallback_Accepted()
    {
        IEnumerable<GridCellReference<Product>>? received = null;

        var cut = Render<MariloDataGrid<Product>>(p => p
            .Add(x => x.Data, Products)
            .Add(x => x.SelectionUnit, GridSelectionUnit.Cell)
            .Add(x => x.SelectionMode, GridSelectionMode.Single)
            .Add(x => x.SelectedCellsChanged, EventCallback.Factory.Create<IEnumerable<GridCellReference<Product>>>(this, cells => received = cells))
            .Add(x => x.ChildContent, builder =>
            {
                builder.OpenComponent<MariloGridColumn<Product>>(0);
                builder.AddAttribute(1, nameof(MariloGridColumn<Product>.Field), "Name");
                builder.AddAttribute(2, nameof(MariloGridColumn<Product>.Title), "Name");
                builder.CloseComponent();
            }));

        // Grid renders with cell selection enabled
        Assert.NotNull(cut.Find("table"));
    }
}
