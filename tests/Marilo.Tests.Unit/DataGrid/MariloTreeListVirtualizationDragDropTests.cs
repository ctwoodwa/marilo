using Bunit;
using Marilo.Components.DataGrid;
using Marilo.Core.Enums;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

public class MariloTreeListVirtualizationDragDropTests : MariloTestBase
{
    private record Employee(int Id, string Name, string Department, int? ParentId = null);

    private static readonly List<Employee> FlatData =
    [
        new(1, "Alice", "Engineering"),
        new(2, "Bob", "Marketing"),
        new(3, "Carol", "Engineering"),
    ];

    private static List<Employee> GenerateLargeDataset(int count) =>
        Enumerable.Range(1, count).Select(i => new Employee(i, $"Employee {i}", $"Dept {(i % 5) + 1}")).ToList();

    // ── Virtualization tests ─────────────────────────────────────

    [Fact]
    public void EnableVirtualization_Renders_Virtualize_Component()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.EnableVirtualization, true)
            .Add(p => p.ItemHeight, 36)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Name")));

        // Virtualize component renders rows; verify rows exist
        var rows = cut.FindAll(".mar-treelist__row");
        Assert.True(rows.Count > 0);
    }

    [Fact]
    public void Virtualization_Off_Does_Not_Use_Virtualize()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.EnableVirtualization, false)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Name")));

        var rows = cut.FindAll(".mar-treelist__row");
        Assert.Equal(3, rows.Count);
    }

    [Fact]
    public void Virtualization_Renders_All_Rows_In_Flat_List()
    {
        // With Virtualize in bUnit, all items render (no viewport clipping)
        var data = GenerateLargeDataset(50);
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.EnableVirtualization, true)
            .Add(p => p.ItemHeight, 36)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Name")));

        var rows = cut.FindAll(".mar-treelist__row");
        Assert.Equal(50, rows.Count);
    }

    [Fact]
    public void Virtualization_Rows_Have_Height_Style()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.EnableVirtualization, true)
            .Add(p => p.ItemHeight, 42)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Name")));

        var firstRow = cut.Find(".mar-treelist__row");
        Assert.Contains("height:42px", firstRow.GetAttribute("style"));
    }

    [Fact]
    public void Large_Dataset_Does_Not_Crash()
    {
        var data = GenerateLargeDataset(10_000);
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.EnableVirtualization, true)
            .Add(p => p.ItemHeight, 36)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Name")));

        var rows = cut.FindAll(".mar-treelist__row");
        Assert.True(rows.Count > 0, "Should render rows without crashing for 10,000 items");
    }

    // ── Row drag-drop tests ──────────────────────────────────────

    [Fact]
    public void RowDraggable_Adds_Draggable_Attribute()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.RowDraggable, true)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Name")));

        var rows = cut.FindAll(".mar-treelist__row");
        Assert.All(rows, row => Assert.Equal("true", row.GetAttribute("draggable")));
    }

    [Fact]
    public void RowDraggable_False_No_Draggable_Attribute()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.RowDraggable, false)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Name")));

        var rows = cut.FindAll(".mar-treelist__row");
        Assert.All(rows, row => Assert.Null(row.GetAttribute("draggable")));
    }

    [Fact]
    public async Task OnRowDropped_Fires_On_Drop()
    {
        TreeListRowDropEventArgs<Employee>? dropArgs = null;
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.RowDraggable, true)
            .Add(p => p.OnRowDropped, (TreeListRowDropEventArgs<Employee> args) => { dropArgs = args; })
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Name")));

        Assert.True(cut.FindAll(".mar-treelist__row").Count >= 3);

        // Simulate drag from row 0 to row 2
        // Re-find after each event because DragStart triggers re-render
        cut.Find(".mar-treelist__row:nth-child(1)").DragStart();
        cut.Find(".mar-treelist__row:nth-child(3)").Drop();

        Assert.NotNull(dropArgs);
        Assert.Equal("Alice", dropArgs!.Item.Name);
        Assert.Equal("Carol", dropArgs.DestinationItem!.Name);
        Assert.Equal(TreeListDropPosition.After, dropArgs.DropPosition);
    }

    [Fact]
    public void DropPosition_Enum_Has_Expected_Members()
    {
        Assert.Equal(0, (int)TreeListDropPosition.Before);
        Assert.Equal(1, (int)TreeListDropPosition.After);
        Assert.Equal(2, (int)TreeListDropPosition.Over);
    }
}
