using Bunit;
using Marilo.Components.Navigation;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Marilo.Tests.Unit.P2Enhancements;

public class TreeViewTests : MariloTestBase
{
    public record FlatNode(string Id, string? ParentId, string Name);
    public record HierarchicalNode(string Id, string Name, List<HierarchicalNode>? Children = null);
    public record LazyNode(string Id, string? ParentId, string Name, bool HasChildren = false);

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

    // ── Tri-State Checkbox Tests ─────────────────────────────────────────

    [Fact]
    public void TreeView_CheckParent_WithAllowCheckChildren_CascadesToDescendants()
    {
        var data = new List<object>
        {
            new HierarchicalNode("1", "Parent", new List<HierarchicalNode>
            {
                new("2", "Child 1"),
                new("3", "Child 2"),
            }),
        };

        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.IdField, "Id")
            .Add(p => p.TextField, "Name")
            .Add(p => p.ItemsField, "Children")
            .Add(p => p.CheckBoxMode, CheckBoxMode.Multiple)
            .Add(p => p.AllowCheckChildren, true));

        // Expand so children are rendered
        cut.Find(".mar-tree-item__toggle").Click();

        // Find and check the parent checkbox (first checkbox = parent)
        var checkboxes = cut.FindAll(".mar-tree-item__checkbox");
        Assert.True(checkboxes.Count >= 3); // parent + 2 children

        // Check the parent
        checkboxes[0].Change(true);

        // All checkboxes should now be checked (parent + children)
        var updatedCheckboxes = cut.FindAll(".mar-tree-item__checkbox");
        foreach (var cb in updatedCheckboxes)
            Assert.Equal("true", cb.GetAttribute("aria-checked"));
    }

    [Fact]
    public void TreeView_UncheckParent_UnchecksAllDescendants()
    {
        var checkedIds = new List<string> { "1", "2", "3" };

        var data = new List<object>
        {
            new HierarchicalNode("1", "Parent", new List<HierarchicalNode>
            {
                new("2", "Child 1"),
                new("3", "Child 2"),
            }),
        };

        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.IdField, "Id")
            .Add(p => p.TextField, "Name")
            .Add(p => p.ItemsField, "Children")
            .Add(p => p.CheckBoxMode, CheckBoxMode.Multiple)
            .Add(p => p.AllowCheckChildren, true)
            .Add(p => p.CheckedItems, checkedIds));

        // Expand to render children
        cut.Find(".mar-tree-item__toggle").Click();

        // Verify all are checked initially
        var checkboxes = cut.FindAll(".mar-tree-item__checkbox");
        Assert.All(checkboxes, cb => Assert.Equal("true", cb.GetAttribute("aria-checked")));

        // Uncheck the parent
        checkboxes[0].Change(false);

        // All should now be unchecked
        var updatedCheckboxes = cut.FindAll(".mar-tree-item__checkbox");
        foreach (var cb in updatedCheckboxes)
            Assert.Equal("false", cb.GetAttribute("aria-checked"));
    }

    [Fact]
    public void TreeView_CheckAllChildren_WithAllowCheckParents_AutoChecksParent()
    {
        var data = new List<object>
        {
            new HierarchicalNode("1", "Parent", new List<HierarchicalNode>
            {
                new("2", "Child 1"),
                new("3", "Child 2"),
            }),
        };

        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.IdField, "Id")
            .Add(p => p.TextField, "Name")
            .Add(p => p.ItemsField, "Children")
            .Add(p => p.CheckBoxMode, CheckBoxMode.Multiple)
            .Add(p => p.AllowCheckChildren, false)
            .Add(p => p.AllowCheckParents, true));

        // Expand to render children
        cut.Find(".mar-tree-item__toggle").Click();

        var checkboxes = cut.FindAll(".mar-tree-item__checkbox");
        Assert.Equal(3, checkboxes.Count); // parent + 2 children

        // Check both children (indexes 1 and 2)
        checkboxes[1].Change(true);
        checkboxes = cut.FindAll(".mar-tree-item__checkbox");
        checkboxes[2].Change(true);

        // Parent should now be fully checked
        var parentCheckbox = cut.FindAll(".mar-tree-item__checkbox")[0];
        Assert.Equal("true", parentCheckbox.GetAttribute("aria-checked"));
    }

    [Fact]
    public void TreeView_CheckSomeChildren_RendersParentAsIndeterminate()
    {
        var data = new List<object>
        {
            new HierarchicalNode("1", "Parent", new List<HierarchicalNode>
            {
                new("2", "Child 1"),
                new("3", "Child 2"),
            }),
        };

        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.IdField, "Id")
            .Add(p => p.TextField, "Name")
            .Add(p => p.ItemsField, "Children")
            .Add(p => p.CheckBoxMode, CheckBoxMode.Multiple)
            .Add(p => p.AllowCheckChildren, false)
            .Add(p => p.AllowCheckParents, true));

        // Expand to render children
        cut.Find(".mar-tree-item__toggle").Click();

        var checkboxes = cut.FindAll(".mar-tree-item__checkbox");

        // Check only the first child
        checkboxes[1].Change(true);

        // Parent should be indeterminate (aria-checked="mixed")
        var parentCheckbox = cut.FindAll(".mar-tree-item__checkbox")[0];
        Assert.Equal("mixed", parentCheckbox.GetAttribute("aria-checked"));

        // Parent should also have the indeterminate CSS class
        Assert.Contains("mar-tree-item__checkbox--indeterminate", parentCheckbox.GetAttribute("class") ?? "");
    }

    [Fact]
    public void TreeView_AllowCheckChildren_False_PreventsChildCascade()
    {
        var data = new List<object>
        {
            new HierarchicalNode("1", "Parent", new List<HierarchicalNode>
            {
                new("2", "Child 1"),
                new("3", "Child 2"),
            }),
        };

        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.IdField, "Id")
            .Add(p => p.TextField, "Name")
            .Add(p => p.ItemsField, "Children")
            .Add(p => p.CheckBoxMode, CheckBoxMode.Multiple)
            .Add(p => p.AllowCheckChildren, false)
            .Add(p => p.AllowCheckParents, false));

        // Expand to render children
        cut.Find(".mar-tree-item__toggle").Click();

        var checkboxes = cut.FindAll(".mar-tree-item__checkbox");
        Assert.Equal(3, checkboxes.Count);

        // Check the parent
        checkboxes[0].Change(true);

        // Children should NOT be checked (cascade disabled).
        // The parent's rendered aria-checked reflects tri-state: since children are still
        // unchecked, the parent appears indeterminate ("mixed") even though its own ID is
        // in the checked set — this is correct GetCheckState() behaviour.
        var updated = cut.FindAll(".mar-tree-item__checkbox");
        Assert.NotEqual("true", updated[1].GetAttribute("aria-checked")); // child 1 unchanged
        Assert.NotEqual("true", updated[2].GetAttribute("aria-checked")); // child 2 unchanged

        // Parent reflects indeterminate state (some but not all descendants checked)
        Assert.Equal("mixed", updated[0].GetAttribute("aria-checked"));
    }

    [Fact]
    public void TreeView_CheckedItems_UpdatesOnCheckUncheck()
    {
        var checkedItemsReceived = new List<IEnumerable<string>>();

        var data = new List<object>
        {
            new HierarchicalNode("1", "Node A"),
            new HierarchicalNode("2", "Node B"),
        };

        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.IdField, "Id")
            .Add(p => p.TextField, "Name")
            .Add(p => p.ItemsField, "Children")
            .Add(p => p.CheckBoxMode, CheckBoxMode.Multiple)
            .Add(p => p.CheckedItemsChanged, ids => checkedItemsReceived.Add(ids)));

        var checkboxes = cut.FindAll(".mar-tree-item__checkbox");
        Assert.Equal(2, checkboxes.Count);

        // Check the first item
        checkboxes[0].Change(true);
        Assert.Single(checkedItemsReceived);
        Assert.Contains("1", checkedItemsReceived[0]);

        // Uncheck it
        cut.FindAll(".mar-tree-item__checkbox")[0].Change(false);
        Assert.Equal(2, checkedItemsReceived.Count);
        Assert.DoesNotContain("1", checkedItemsReceived[1]);
    }

    // ── Lazy Loading Tests ───────────────────────────────────────────────

    [Fact]
    public void TreeView_LoadChildrenAsync_InvokesOnFirstExpand()
    {
        var loadCallCount = 0;
        object? loadedItem = null;

        // Use flat data with ParentIdField so BuildFlat is called, which honours HasChildrenField
        var data = new List<object>
        {
            new LazyNode("1", null, "Lazy Parent", HasChildren: true),
        };

        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.IdField, "Id")
            .Add(p => p.ParentIdField, "ParentId")
            .Add(p => p.TextField, "Name")
            .Add(p => p.HasChildrenField, "HasChildren")
            .Add(p => p.LoadChildrenAsync, item =>
            {
                loadCallCount++;
                loadedItem = item;
                return Task.FromResult(Enumerable.Empty<object>());
            }));

        Assert.Equal(0, loadCallCount);

        // Expand the node — should trigger load
        cut.Find(".mar-tree-item__toggle").Click();

        Assert.Equal(1, loadCallCount);
        Assert.NotNull(loadedItem);
        Assert.IsType<LazyNode>(loadedItem);
        Assert.Equal("1", ((LazyNode)loadedItem!).Id);
    }

    [Fact]
    public void TreeView_LoadChildrenAsync_DoesNotReinvokeOnSubsequentExpand()
    {
        var loadCallCount = 0;

        var data = new List<object>
        {
            new LazyNode("1", null, "Lazy Parent", HasChildren: true),
        };

        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.IdField, "Id")
            .Add(p => p.ParentIdField, "ParentId")
            .Add(p => p.TextField, "Name")
            .Add(p => p.HasChildrenField, "HasChildren")
            .Add(p => p.LoadChildrenAsync, _ =>
            {
                loadCallCount++;
                return Task.FromResult(Enumerable.Empty<object>());
            }));

        // First expand — should invoke load
        cut.Find(".mar-tree-item__toggle").Click();
        Assert.Equal(1, loadCallCount);

        // Collapse
        cut.Find(".mar-tree-item__toggle").Click();

        // Second expand — should NOT invoke load again (load-once semantics)
        cut.Find(".mar-tree-item__toggle").Click();
        Assert.Equal(1, loadCallCount);
    }

    [Fact]
    public void TreeView_LoadChildrenAsync_ChildrenRenderAfterComplete()
    {
        // Verifies the loading indicator appears while the async callback is pending
        // and disappears once the task completes and StateHasChanged is called.
        var tcs = new TaskCompletionSource<IEnumerable<object>>();

        var data = new List<object>
        {
            new LazyNode("1", null, "Lazy Parent", HasChildren: true),
        };

        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.IdField, "Id")
            .Add(p => p.ParentIdField, "ParentId")
            .Add(p => p.TextField, "Name")
            .Add(p => p.HasChildrenField, "HasChildren")
            .Add(p => p.LoadChildrenAsync, _ => tcs.Task));

        // Before expand: no loading indicator
        Assert.DoesNotContain("mar-tree-item__loading", cut.Markup);

        // Expand: loading begins asynchronously
        cut.Find(".mar-tree-item__toggle").Click();

        // Loading indicator should be visible while task is pending
        Assert.Contains("mar-tree-item__loading", cut.Markup);

        // Complete the load — the async continuation in ToggleNodeAsync removes the loading
        // indicator and calls StateHasChanged. Use WaitForState to flush the async dispatch.
        tcs.SetResult(Enumerable.Empty<object>());
        cut.WaitForState(() => !cut.Markup.Contains("mar-tree-item__loading"));
    }

    // ── Keyboard Navigation Tests ────────────────────────────────────────

    [Fact]
    public void TreeView_ArrowDown_MovesFocusToNextNode()
    {
        var data = new List<object>
        {
            new HierarchicalNode("1", "Node A"),
            new HierarchicalNode("2", "Node B"),
            new HierarchicalNode("3", "Node C"),
        };

        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.IdField, "Id")
            .Add(p => p.TextField, "Name")
            .Add(p => p.ItemsField, "Children"));

        var tree = cut.Find("[role='tree']");

        // ArrowDown should focus the first node, then move to subsequent nodes
        tree.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });
        Assert.Contains("mar-tree-item--focused", cut.Markup);

        // Capture which node is focused after first press
        var firstFocused = cut.FindAll(".mar-tree-item--focused");
        Assert.Single(firstFocused);

        // ArrowDown again should move focus forward
        tree.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });
        var secondFocused = cut.FindAll(".mar-tree-item--focused");
        Assert.Single(secondFocused);

        // The focused node should have changed
        Assert.NotEqual(firstFocused[0].GetAttribute("id"), secondFocused[0].GetAttribute("id"));
    }

    [Fact]
    public void TreeView_ArrowRight_ExpandsCollapsedNode()
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

        var tree = cut.Find("[role='tree']");

        // Children not visible initially
        Assert.DoesNotContain("Child 1", cut.Markup);

        // Focus the parent node first with ArrowDown
        tree.KeyDown(new KeyboardEventArgs { Key = "ArrowDown" });

        // ArrowRight on a collapsed node should expand it
        tree.KeyDown(new KeyboardEventArgs { Key = "ArrowRight" });

        Assert.Contains("Child 1", cut.Markup);
    }

    [Fact]
    public void TreeView_ArrowLeft_CollapsesExpandedNode()
    {
        var data = new List<object>
        {
            new HierarchicalNode("1", "Parent", new List<HierarchicalNode>
            {
                new("2", "Child 1"),
            }),
        };

        // Pre-expand node "1" via ExpandedItems so _expandedIds is populated before key events
        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.IdField, "Id")
            .Add(p => p.TextField, "Name")
            .Add(p => p.ItemsField, "Children")
            .Add(p => p.ExpandedItems, new[] { "1" }));

        var tree = cut.Find("[role='tree']");

        // Children should be visible because node is pre-expanded
        Assert.Contains("Child 1", cut.Markup);

        // ArrowLeft with no prior focus: _focusedNodeId initialises to visibleIds[0]="1" (Parent),
        // which IS expanded, so ToggleNodeAsync collapses it.
        tree.KeyDown(new KeyboardEventArgs { Key = "ArrowLeft" });

        Assert.DoesNotContain("Child 1", cut.Markup);
    }

    [Fact]
    public void TreeView_EnterSpace_SelectsFocusedNode()
    {
        var selectedItems = new List<IEnumerable<string>>();

        var data = new List<object>
        {
            new HierarchicalNode("1", "Node A"),
            new HierarchicalNode("2", "Node B"),
        };

        var cut = Render<MariloTreeView>(parameters => parameters
            .Add(p => p.Data, data)
            .Add(p => p.IdField, "Id")
            .Add(p => p.TextField, "Name")
            .Add(p => p.ItemsField, "Children")
            .Add(p => p.SelectionMode, TreeSelectionMode.Single)
            .Add(p => p.SelectedItemsChanged, ids => selectedItems.Add(ids)));

        var tree = cut.Find("[role='tree']");

        // Press Enter with no prior focus: _focusedNodeId initialises to visibleIds[0]="1"
        // and then Enter/Space selects that node.
        tree.KeyDown(new KeyboardEventArgs { Key = "Enter" });

        Assert.Single(selectedItems);
        Assert.Contains("1", selectedItems[0]);
    }
}
