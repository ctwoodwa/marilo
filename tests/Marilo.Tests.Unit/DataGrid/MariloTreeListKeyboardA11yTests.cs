using Bunit;
using Marilo.Components.DataGrid;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

public class MariloTreeListKeyboardA11yTests : MariloTestBase
{
    private record Employee(int Id, string Name, string Department, int? ParentId = null);

    private static readonly List<Employee> FlatData =
    [
        new(1, "Alice", "Engineering"),
        new(2, "Bob", "Marketing"),
        new(3, "Carol", "Engineering"),
    ];

    private static readonly List<Employee> HierarchicalFlatData =
    [
        new(1, "Alice", "Engineering"),
        new(2, "Bob", "Engineering", 1),
        new(3, "Carol", "Marketing"),
    ];

    // ── ARIA attribute tests ─────────────────────────────────────

    [Fact]
    public void Table_Has_Role_Treegrid()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Name")));

        var table = cut.Find("table");
        Assert.Equal("treegrid", table.GetAttribute("role"));
    }

    [Fact]
    public void Rows_Have_Role_Row_And_AriaLevel()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var rows = cut.FindAll("tr[role='row']");
        Assert.Equal(3, rows.Count);
        foreach (var row in rows)
        {
            Assert.Equal("1", row.GetAttribute("aria-level"));
        }
    }

    [Fact]
    public void Cells_Have_Role_Gridcell()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var cells = cut.FindAll("td[role='gridcell']");
        Assert.Equal(3, cells.Count); // 3 rows * 1 column
    }

    [Fact]
    public void Headers_Have_Role_Columnheader_And_AriaSort()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Sortable, true)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")
                .Add(c => c.Title, "Department")));

        var headers = cut.FindAll("th[role='columnheader']");
        Assert.Equal(2, headers.Count);
        // Before any sort, aria-sort should be "none"
        foreach (var th in headers)
        {
            Assert.Equal("none", th.GetAttribute("aria-sort"));
        }
    }

    [Fact]
    public void Expanded_Rows_Have_AriaExpanded()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, HierarchicalFlatData)
            .Add(p => p.IdField, "Id")
            .Add(p => p.ParentIdField, "ParentId")
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        // Row for Alice (has children) should have aria-expanded="false" initially
        var rows = cut.FindAll("tr[role='row']");
        var aliceRow = rows[0];
        Assert.Equal("false", aliceRow.GetAttribute("aria-expanded"));
        // Carol has no children — no aria-expanded attribute
        var carolRow = rows[1]; // Carol is root-level (no parent), so second root item
        Assert.Null(carolRow.GetAttribute("aria-expanded"));
    }

    [Fact]
    public void Selected_Rows_Have_AriaSelected()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.SelectionMode, Marilo.Core.Enums.TreeListSelectionMode.Single)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        // Click first row to select
        var rows = cut.FindAll("tr[role='row']");
        rows[0].Click();

        // Re-query after render
        rows = cut.FindAll("tr[role='row']");
        Assert.Equal("true", rows[0].GetAttribute("aria-selected"));
    }

    // ── Navigable parameter tests ────────────────────────────────

    [Fact]
    public void Navigable_Defaults_To_False()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var table = cut.Find("table");
        Assert.Equal("-1", table.GetAttribute("tabindex"));
    }

    [Fact]
    public void Navigable_True_Sets_Tabindex_Zero()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Navigable, true)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var table = cut.Find("table");
        Assert.Equal("0", table.GetAttribute("tabindex"));
    }

    // ── Keyboard navigation tests ────────────────────────────────

    [Fact]
    public void ArrowDown_Moves_Focus_To_Next_Row()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Navigable, true)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var table = cut.Find("table");
        table.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });

        var rows = cut.FindAll("tr[role='row']");
        Assert.Contains("mar-treelist__row--focused", rows[0].GetAttribute("class"));
    }

    [Fact]
    public void ArrowDown_Then_ArrowDown_Moves_To_Second_Row()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Navigable, true)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var table = cut.Find("table");
        table.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });
        table.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });

        var rows = cut.FindAll("tr[role='row']");
        Assert.DoesNotContain("mar-treelist__row--focused", rows[0].GetAttribute("class") ?? "");
        Assert.Contains("mar-treelist__row--focused", rows[1].GetAttribute("class"));
    }

    [Fact]
    public void ArrowUp_Moves_Focus_To_Previous_Row()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Navigable, true)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var table = cut.Find("table");
        // Move down twice, then up once
        table.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });
        table.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });
        table.KeyDown(new KeyboardEventArgs { Key = "ArrowUp" });

        var rows = cut.FindAll("tr[role='row']");
        Assert.Contains("mar-treelist__row--focused", rows[0].GetAttribute("class"));
        Assert.DoesNotContain("mar-treelist__row--focused", rows[1].GetAttribute("class") ?? "");
    }

    [Fact]
    public void Home_Moves_Focus_To_First_Row()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Navigable, true)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var table = cut.Find("table");
        table.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });
        table.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });
        table.KeyDown(new KeyboardEventArgs { Key = "Home" });

        var rows = cut.FindAll("tr[role='row']");
        Assert.Contains("mar-treelist__row--focused", rows[0].GetAttribute("class"));
    }

    [Fact]
    public void End_Moves_Focus_To_Last_Row()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Navigable, true)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var table = cut.Find("table");
        table.KeyDown(new KeyboardEventArgs { Key = "End" });

        var rows = cut.FindAll("tr[role='row']");
        Assert.Contains("mar-treelist__row--focused", rows[2].GetAttribute("class"));
    }

    [Fact]
    public void ArrowRight_Expands_Collapsed_Row()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, HierarchicalFlatData)
            .Add(p => p.IdField, "Id")
            .Add(p => p.ParentIdField, "ParentId")
            .Add(p => p.Navigable, true)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var table = cut.Find("table");
        // Move to first row (Alice — has child Bob)
        table.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });
        // Expand
        table.KeyDown(new KeyboardEventArgs { Key = "ArrowRight" });

        // After expand, Alice's row should have aria-expanded="true"
        var rows = cut.FindAll("tr[role='row']");
        Assert.Equal("true", rows[0].GetAttribute("aria-expanded"));
        // Bob should now be visible (3 rows total: Alice, Bob, Carol)
        Assert.Equal(3, rows.Count);
    }

    [Fact]
    public void ArrowLeft_Collapses_Expanded_Row()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, HierarchicalFlatData)
            .Add(p => p.IdField, "Id")
            .Add(p => p.ParentIdField, "ParentId")
            .Add(p => p.Navigable, true)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var table = cut.Find("table");
        // Move to Alice, expand, then collapse
        table.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });
        table.KeyDown(new KeyboardEventArgs { Key = "ArrowRight" }); // expand
        table.KeyDown(new KeyboardEventArgs { Key = "ArrowLeft" }); // collapse

        var rows = cut.FindAll("tr[role='row']");
        Assert.Equal("false", rows[0].GetAttribute("aria-expanded"));
        // Bob should be hidden again (2 rows: Alice, Carol)
        Assert.Equal(2, rows.Count);
    }

    [Fact]
    public void Keyboard_Nav_Inactive_When_Navigable_Is_False()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Navigable, false)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var table = cut.Find("table");
        table.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });

        // No row should be focused
        var focused = cut.FindAll(".mar-treelist__row--focused");
        Assert.Empty(focused);
    }

    [Fact]
    public void Enter_Triggers_Selection_On_Focused_Row()
    {
        var selectedItems = new List<Employee>();
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Navigable, true)
            .Add(p => p.SelectionMode, Marilo.Core.Enums.TreeListSelectionMode.Single)
            .Add(p => p.SelectedItemsChanged, (IReadOnlyList<Employee> items) => selectedItems = items.ToList())
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var table = cut.Find("table");
        table.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" }); // focus row 0
        table.KeyDown(new KeyboardEventArgs { Key = "Enter" }); // select

        Assert.Single(selectedItems);
        Assert.Equal("Alice", selectedItems[0].Name);
    }
}
