using Bunit;
using Marilo.Components.Navigation;
using Xunit;

namespace Marilo.Tests.Unit.P2Enhancements;

public class TreeViewTests : MariloTestBase
{
    public record FlatNode(string Id, string? ParentId, string Name);
    public record HierarchicalNode(string Id, string Name, List<HierarchicalNode>? Children = null);

    [Fact]
    public void TreeView_RendersItemsFromFlatData()
    {
        var data = new List<object>
        {
            new FlatNode("1", null, "Root A"),
            new FlatNode("2", "1", "Child A1"),
            new FlatNode("3", "1", "Child A2"),
            new FlatNode("4", null, "Root B"),
        };

        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.IdField, "Id")
            .Add(p => p.ParentIdField, "ParentId")
            .Add(p => p.TextField, "Name"));

        // Should render tree items
        var items = cut.FindAll("[role='treeitem']");
        // Root items are rendered; children are inside but collapsed
        Assert.True(items.Count >= 2); // At least Root A and Root B

        // Check that root names are rendered
        Assert.Contains("Root A", cut.Markup);
        Assert.Contains("Root B", cut.Markup);
    }

    [Fact]
    public void TreeView_RendersItemsFromHierarchicalData()
    {
        var data = new List<object>
        {
            new HierarchicalNode("1", "Parent", new List<HierarchicalNode>
            {
                new("2", "Child 1"),
                new("3", "Child 2"),
            }),
            new HierarchicalNode("4", "Standalone"),
        };

        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.IdField, "Id")
            .Add(p => p.TextField, "Name")
            .Add(p => p.ItemsField, "Children"));

        Assert.Contains("Parent", cut.Markup);
        Assert.Contains("Standalone", cut.Markup);
    }

    [Fact]
    public void TreeView_RendersChildContentWhenNoData()
    {
        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.ChildContent, builder =>
            {
                builder.OpenElement(0, "li");
                builder.AddContent(1, "Manual Item");
                builder.CloseElement();
            }));

        Assert.Contains("Manual Item", cut.Markup);
    }

    [Fact]
    public void TreeView_ExpandCollapseNodes()
    {
        var data = new List<object>
        {
            new HierarchicalNode("1", "Parent", new List<HierarchicalNode>
            {
                new("2", "Child 1"),
            }),
        };

        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.IdField, "Id")
            .Add(p => p.TextField, "Name")
            .Add(p => p.ItemsField, "Children"));

        // Children should not be visible initially (collapsed)
        Assert.DoesNotContain("Child 1", cut.Markup);

        // Click expand button
        var toggleBtn = cut.Find(".mar-tree-item__toggle");
        toggleBtn.Click();

        // Children should now be visible
        Assert.Contains("Child 1", cut.Markup);

        // Click again to collapse
        toggleBtn = cut.Find(".mar-tree-item__toggle");
        toggleBtn.Click();

        Assert.DoesNotContain("Child 1", cut.Markup);
    }
}
