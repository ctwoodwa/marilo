using Bunit;
using Marilo.Components.DataGrid;
using Marilo.Core.Models;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

public class MariloTreeListResizeReorderTests : MariloTestBase
{
    private record Employee(int Id, string Name, string Department, int? ParentId = null);

    private static readonly List<Employee> FlatData =
    [
        new(1, "Alice", "Engineering"),
        new(2, "Bob", "Marketing"),
        new(3, "Carol", "Engineering"),
    ];

    // ── Resize tests ─────────────────────────────────────────────

    [Fact]
    public void Resize_Handles_Render_When_Resizable_Is_True()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Resizable, true)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")
                .Add(c => c.Title, "Department")));

        var handles = cut.FindAll(".mar-treelist__resize-handle");
        Assert.Equal(2, handles.Count);
    }

    [Fact]
    public void Resize_Handles_Do_Not_Render_When_Resizable_Is_False()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Resizable, false)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")));

        var handles = cut.FindAll(".mar-treelist__resize-handle");
        Assert.Empty(handles);
    }

    [Fact]
    public void Resizable_Defaults_To_False()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var handles = cut.FindAll(".mar-treelist__resize-handle");
        Assert.Empty(handles);
    }

    // ── Reorder tests ────────────────────────────────────────────

    [Fact]
    public void Headers_Have_Draggable_True_When_Reorderable_Is_True()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Reorderable, true)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")
                .Add(c => c.Title, "Department")));

        var headers = cut.FindAll("th.mar-treelist__th");
        foreach (var th in headers)
        {
            Assert.Equal("true", th.GetAttribute("draggable"));
        }
    }

    [Fact]
    public void Headers_Have_Draggable_False_When_Reorderable_Is_False()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Reorderable, false)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")));

        var headers = cut.FindAll("th.mar-treelist__th");
        foreach (var th in headers)
        {
            Assert.Equal("false", th.GetAttribute("draggable"));
        }
    }

    [Fact]
    public void Reorderable_Defaults_To_False()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")));

        var header = cut.Find("th.mar-treelist__th");
        Assert.Equal("false", header.GetAttribute("draggable"));
    }

    [Fact]
    public void Both_Resizable_And_Reorderable_Can_Be_Enabled_Together()
    {
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Resizable, true)
            .Add(p => p.Reorderable, true)
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name")
                .Add(c => c.Title, "Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")
                .Add(c => c.Title, "Department")));

        // Resize handles present
        var handles = cut.FindAll(".mar-treelist__resize-handle");
        Assert.Equal(2, handles.Count);

        // Headers draggable
        var headers = cut.FindAll("th.mar-treelist__th");
        foreach (var th in headers)
        {
            Assert.Equal("true", th.GetAttribute("draggable"));
        }
    }

    [Fact]
    public void OnColumnReordered_EventCallback_Parameter_Exists()
    {
        // Verifies the component accepts the parameter without error
        TreeListColumnReorderEventArgs? receivedArgs = null;
        var cut = Render<MariloTreeList<Employee>>(parameters => parameters
            .Add(p => p.Data, FlatData)
            .Add(p => p.Reorderable, true)
            .Add(p => p.OnColumnReordered, (TreeListColumnReorderEventArgs args) => { receivedArgs = args; })
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Name"))
            .AddChildContent<MariloTreeListColumn>(col => col
                .Add(c => c.Field, "Department")));

        // Component rendered without error — parameter is accepted
        var headers = cut.FindAll("th.mar-treelist__th");
        Assert.Equal(2, headers.Count);
    }
}
