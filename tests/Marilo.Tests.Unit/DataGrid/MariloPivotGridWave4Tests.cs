using Bunit;
using Marilo.Components.DataGrid;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

/// <summary>
/// Wave 4 tests: SCSS parity validation and ARIA accessibility for MariloPivotGrid.
/// </summary>
public class MariloPivotGridWave4Tests : MariloTestBase
{
    private static readonly List<object> SalesData = new()
    {
        new SalesRecord("North", "Widget A", 1200),
        new SalesRecord("North", "Widget B", 800),
        new SalesRecord("South", "Widget A", 950),
        new SalesRecord("South", "Widget B", 1100),
    };

    private record SalesRecord(string Region, string Product, double Revenue);

    private IRenderedComponent<MariloPivotGrid> RenderPivotGrid(
        string? ariaLabel = null,
        bool sortable = false)
    {
        return Render<MariloPivotGrid>(parameters =>
        {
            parameters
                .Add(p => p.Data, SalesData)
                .Add(p => p.Sortable, sortable);

            if (ariaLabel != null)
                parameters.Add(p => p.AriaLabel, ariaLabel);

            parameters
                .AddChildContent<MariloPivotGridRowField>(f => f
                    .Add(c => c.Field, "Region")
                    .Add(c => c.Title, "Region"))
                .AddChildContent<MariloPivotGridColumnField>(f => f
                    .Add(c => c.Field, "Product")
                    .Add(c => c.Title, "Product"))
                .AddChildContent<MariloPivotGridMeasureField>(f => f
                    .Add(c => c.Field, "Revenue")
                    .Add(c => c.Title, "Revenue"));
        });
    }

    // ── Test 1: ARIA roles present ──────────────────────────────────────

    [Fact]
    public void Table_Has_Grid_Role_And_Headers_Have_Correct_Roles()
    {
        var cut = RenderPivotGrid();

        // Table must have role="grid"
        var table = cut.Find("table.mar-pivotgrid__table");
        Assert.Equal("grid", table.GetAttribute("role"));

        // Column headers must have role="columnheader"
        var colHeaders = cut.FindAll(".mar-pivotgrid__col-header");
        Assert.True(colHeaders.Count >= 2, "Expected at least 2 column headers");
        foreach (var h in colHeaders)
        {
            Assert.Equal("columnheader", h.GetAttribute("role"));
        }

        // Row headers must have role="rowheader"
        var rowHeaders = cut.FindAll(".mar-pivotgrid__row-header");
        Assert.True(rowHeaders.Count >= 2, "Expected at least 2 row headers");
        foreach (var h in rowHeaders)
        {
            Assert.Equal("rowheader", h.GetAttribute("role"));
        }

        // Data cells must have role="gridcell"
        var cells = cut.FindAll(".mar-pivotgrid__cell");
        Assert.True(cells.Count >= 4, "Expected at least 4 data cells");
        foreach (var c in cells)
        {
            Assert.Equal("gridcell", c.GetAttribute("role"));
        }
    }

    // ── Test 2: Header scopes ───────────────────────────────────────────

    [Fact]
    public void Headers_Have_Correct_Scope_Attributes()
    {
        var cut = RenderPivotGrid();

        // Corner header has scope="col"
        var corner = cut.Find(".mar-pivotgrid__corner");
        Assert.Equal("col", corner.GetAttribute("scope"));

        // Column headers have scope="col"
        var colHeaders = cut.FindAll(".mar-pivotgrid__col-header");
        foreach (var h in colHeaders)
        {
            Assert.Equal("col", h.GetAttribute("scope"));
        }

        // Row headers have scope="row"
        var rowHeaders = cut.FindAll(".mar-pivotgrid__row-header");
        foreach (var h in rowHeaders)
        {
            Assert.Equal("row", h.GetAttribute("scope"));
        }
    }

    // ── Test 3: aria-sort on sortable column headers ────────────────────

    [Fact]
    public void Sortable_Column_Headers_Have_AriaSortNone()
    {
        var cut = RenderPivotGrid(sortable: true);

        var colHeaders = cut.FindAll(".mar-pivotgrid__col-header");
        foreach (var h in colHeaders)
        {
            Assert.Equal("none", h.GetAttribute("aria-sort"));
        }
    }

    [Fact]
    public void NonSortable_Column_Headers_Lack_AriaSort()
    {
        var cut = RenderPivotGrid(sortable: false);

        var colHeaders = cut.FindAll(".mar-pivotgrid__col-header");
        foreach (var h in colHeaders)
        {
            Assert.Null(h.GetAttribute("aria-sort"));
        }
    }

    // ── Test 4: aria-label on the pivot grid container ──────────────────

    [Fact]
    public void Container_Has_Default_AriaLabel()
    {
        var cut = RenderPivotGrid();

        var root = cut.Find(".mar-pivotgrid");
        Assert.Equal("Pivot Grid", root.GetAttribute("aria-label"));
    }

    [Fact]
    public void Container_Uses_Custom_AriaLabel()
    {
        var cut = RenderPivotGrid(ariaLabel: "Sales Pivot Table");

        var root = cut.Find(".mar-pivotgrid");
        Assert.Equal("Sales Pivot Table", root.GetAttribute("aria-label"));
    }

    // ── Test 5: Empty state is accessible ───────────────────────────────

    [Fact]
    public void Empty_State_Has_Status_Role()
    {
        var cut = Render<MariloPivotGrid>(parameters => parameters
            .Add(p => p.Data, SalesData));
        // No child fields registered = empty state

        var empty = cut.Find(".mar-pivotgrid__empty");
        Assert.Equal("status", empty.GetAttribute("role"));
    }

    // ── Test 6: Row headers render as <th> not <td> ─────────────────────

    [Fact]
    public void Row_Headers_Render_As_Th_Elements()
    {
        var cut = RenderPivotGrid();

        var rowHeaders = cut.FindAll(".mar-pivotgrid__row-header");
        foreach (var h in rowHeaders)
        {
            Assert.Equal("TH", h.TagName);
        }
    }

    // ── Test 7: All BEM classes from razor exist in markup ──────────────

    [Fact]
    public void All_BEM_Classes_Present_In_Rendered_Markup()
    {
        var cut = RenderPivotGrid();

        // All classes emitted by the razor template
        Assert.NotNull(cut.Find(".mar-pivotgrid"));
        Assert.NotNull(cut.Find(".mar-pivotgrid__scroll"));
        Assert.NotNull(cut.Find(".mar-pivotgrid__table"));
        Assert.NotNull(cut.Find(".mar-pivotgrid__corner"));
        Assert.True(cut.FindAll(".mar-pivotgrid__col-header").Count > 0);
        Assert.True(cut.FindAll(".mar-pivotgrid__row-header").Count > 0);
        Assert.True(cut.FindAll(".mar-pivotgrid__cell").Count > 0);
    }
}
