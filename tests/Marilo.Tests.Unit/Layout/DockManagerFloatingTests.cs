using Bunit;
using Marilo.Components.Layout;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.Layout;

public class DockManagerFloatingTests : MariloTestBase
{
    // ── Helper ──────────────────────────────────────────────────────────

    private IRenderedComponent<MariloDockManager> RenderDockManager(
        Action<ComponentParameterCollectionBuilder<MariloDockManager>>? extra = null)
    {
        return Render<MariloDockManager>(parameters =>
        {
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloDockPane>(0);
                builder.AddAttribute(1, "Id", "pane1");
                builder.AddAttribute(2, "Title", "Pane 1");
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Content 1")));
                builder.CloseComponent();

                builder.OpenComponent<MariloDockPane>(4);
                builder.AddAttribute(5, "Id", "pane2");
                builder.AddAttribute(6, "Title", "Pane 2");
                builder.AddAttribute(7, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Content 2")));
                builder.CloseComponent();
            }));
            extra?.Invoke(parameters);
        });
    }

    private IRenderedComponent<MariloDockManager> RenderWithFloatingPane(
        string top = "100px", string left = "200px",
        string width = "350px", string height = "250px")
    {
        return Render<MariloDockManager>(parameters =>
        {
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloDockPane>(0);
                builder.AddAttribute(1, "Id", "docked1");
                builder.AddAttribute(2, "Title", "Docked");
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Docked Content")));
                builder.CloseComponent();

                builder.OpenComponent<MariloDockPane>(4);
                builder.AddAttribute(5, "Id", "float1");
                builder.AddAttribute(6, "Title", "Floating");
                builder.AddAttribute(7, "IsFloating", true);
                builder.AddAttribute(8, "FloatingTop", top);
                builder.AddAttribute(9, "FloatingLeft", left);
                builder.AddAttribute(10, "FloatingWidth", width);
                builder.AddAttribute(11, "FloatingHeight", height);
                builder.AddAttribute(12, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Floating Content")));
                builder.CloseComponent();
            }));
        });
    }

    // ── 1. Floating pane renders as overlay with BEM class ──────────────

    [Fact]
    public void FloatingPane_RendersAsOverlay_WithFloatingPaneClass()
    {
        var cut = RenderWithFloatingPane();

        Assert.Contains("mar-dockmanager__floating-pane", cut.Markup);
        Assert.Contains("Floating Content", cut.Markup);
    }

    // ── 2. Floating pane has close button that removes it ───────────────

    [Fact]
    public async Task FloatingPane_CloseButton_RemovesPane()
    {
        string? closedId = null;
        var cut = Render<MariloDockManager>(parameters =>
        {
            parameters.Add(p => p.OnPaneClosed, (string id) => { closedId = id; });
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloDockPane>(0);
                builder.AddAttribute(1, "Id", "float1");
                builder.AddAttribute(2, "Title", "Floating");
                builder.AddAttribute(3, "IsFloating", true);
                builder.AddAttribute(4, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Floating Content")));
                builder.CloseComponent();
            }));
        });

        var closeBtn = cut.Find(".mar-dockmanager__floating-pane-close");
        await cut.InvokeAsync(() => closeBtn.Click());

        Assert.Equal("float1", closedId);
        Assert.DoesNotContain("mar-dockmanager__floating-pane", cut.Markup);
    }

    // ── 3. Float/dock toggle moves pane between tab strip and overlay ───

    [Fact]
    public async Task ToggleFloat_MovesPaneBetweenDockedAndFloating()
    {
        var cut = RenderDockManager();

        // Pane1 starts docked and visible in the tab strip
        Assert.Contains("Pane 1", cut.Find(".mar-dockmanager__tabs").TextContent);
        Assert.DoesNotContain("mar-dockmanager__floating-pane", cut.Markup);

        // Float pane1
        var instance = cut.Instance;
        await cut.InvokeAsync(() => instance.ToggleFloat("pane1"));

        // Now pane1 should be in the floating overlay, not in tab strip
        Assert.Contains("mar-dockmanager__floating-pane", cut.Markup);
        Assert.DoesNotContain("Pane 1", cut.Find(".mar-dockmanager__tabs").TextContent);

        // Dock it back
        await cut.InvokeAsync(() => instance.ToggleFloat("pane1"));

        Assert.DoesNotContain("mar-dockmanager__floating-pane", cut.Markup);
        Assert.Contains("Pane 1", cut.Find(".mar-dockmanager__tabs").TextContent);
    }

    // ── 4. Floating pane respects position and size ─────────────────────

    [Fact]
    public void FloatingPane_RespectsPositionAndSize()
    {
        var cut = RenderWithFloatingPane(
            top: "120px", left: "80px", width: "500px", height: "350px");

        var floatingEl = cut.Find(".mar-dockmanager__floating-pane");
        var style = floatingEl.GetAttribute("style") ?? "";

        Assert.Contains("top:120px", style);
        Assert.Contains("left:80px", style);
        Assert.Contains("width:500px", style);
        Assert.Contains("height:350px", style);
    }

    // ── 5. Floating pane has title bar with title text ──────────────────

    [Fact]
    public void FloatingPane_HasTitleBar_WithTitle()
    {
        var cut = RenderWithFloatingPane();

        var titleBar = cut.Find(".mar-dockmanager__floating-pane-titlebar");
        Assert.Contains("Floating", titleBar.TextContent);
    }

    // ── 6. Floating pane does not appear in tab strip ───────────────────

    [Fact]
    public void FloatingPane_DoesNotAppearInTabStrip()
    {
        var cut = RenderWithFloatingPane();

        var tabsMarkup = cut.Find(".mar-dockmanager__tabs").TextContent;
        Assert.DoesNotContain("Floating", tabsMarkup);
        Assert.Contains("Docked", tabsMarkup);
    }

    // ── 7. Dock button on floating pane re-docks ────────────────────────

    [Fact]
    public async Task FloatingPane_DockButton_ReturnsPaneToTabStrip()
    {
        var cut = RenderWithFloatingPane();

        // Click the dock button on the floating pane
        var dockBtn = cut.Find(".mar-dockmanager__floating-pane-dock");
        await cut.InvokeAsync(() => dockBtn.Click());

        // Floating pane should be gone, pane should be in tab strip
        Assert.DoesNotContain("mar-dockmanager__floating-pane", cut.Markup);
        Assert.Contains("Floating", cut.Find(".mar-dockmanager__tabs").TextContent);
    }

    // ── 8. OnPaneFloat callback fires on toggle ─────────────────────────

    [Fact]
    public async Task ToggleFloat_InvokesOnPaneFloatCallback()
    {
        string? floatedId = null;
        var cut = Render<MariloDockManager>(parameters =>
        {
            parameters.Add(p => p.OnPaneFloat, (string id) => { floatedId = id; });
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloDockPane>(0);
                builder.AddAttribute(1, "Id", "pane1");
                builder.AddAttribute(2, "Title", "Pane 1");
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Content 1")));
                builder.CloseComponent();
            }));
        });

        await cut.InvokeAsync(() => cut.Instance.ToggleFloat("pane1"));

        Assert.Equal("pane1", floatedId);
    }

    // -- 9. OnLayoutChanged fires on float toggle --

    [Fact]
    public async Task ToggleFloat_InvokesOnLayoutChangedCallback()
    {
        int layoutChangedCount = 0;
        var cut = Render<MariloDockManager>(parameters =>
        {
            parameters.Add(p => p.OnLayoutChanged, () => { layoutChangedCount++; });
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloDockPane>(0);
                builder.AddAttribute(1, "Id", "pane1");
                builder.AddAttribute(2, "Title", "Pane 1");
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Content 1")));
                builder.CloseComponent();
            }));
        });

        var initialCount = layoutChangedCount;
        await cut.InvokeAsync(() => cut.Instance.ToggleFloat("pane1"));

        Assert.True(layoutChangedCount > initialCount);
    }

    // -- 10. OnPaneActivated fires on tab click --

    [Fact]
    public async Task TabClick_InvokesOnPaneActivatedCallback()
    {
        string? activatedId = null;
        var cut = RenderDockManager(parameters =>
        {
            parameters.Add(p => p.OnPaneActivated, (string id) => { activatedId = id; });
        });

        var tabs = cut.FindAll(".mar-dockmanager__tab");
        Assert.True(tabs.Count >= 2);
        await cut.InvokeAsync(() => tabs[1].Click());

        Assert.Equal("pane2", activatedId);
    }

    // -- 11. Close last tab in group leaves tab strip empty --

    [Fact]
    public async Task CloseLastTab_LeavesTabStripEmpty()
    {
        string? closedId = null;
        var cut = Render<MariloDockManager>(parameters =>
        {
            parameters.Add(p => p.OnPaneClosed, (string id) => { closedId = id; });
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloDockPane>(0);
                builder.AddAttribute(1, "Id", "only");
                builder.AddAttribute(2, "Title", "Only Tab");
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Solo")));
                builder.CloseComponent();
            }));
        });

        var closeBtn = cut.Find(".mar-dockmanager__action--close");
        await cut.InvokeAsync(() => closeBtn.Click());

        Assert.Equal("only", closedId);
        var tabs = cut.FindAll(".mar-dockmanager__tab");
        Assert.Empty(tabs);
    }

    // -- 12. Multiple docked panes with floating: all coexist --

    [Fact]
    public void NestedSplitWithFloating_BothRender()
    {
        var cut = Render<MariloDockManager>(parameters =>
        {
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloDockPane>(0);
                builder.AddAttribute(1, "Id", "docked-a");
                builder.AddAttribute(2, "Title", "Docked A");
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Docked A Content")));
                builder.CloseComponent();

                builder.OpenComponent<MariloDockPane>(4);
                builder.AddAttribute(5, "Id", "docked-b");
                builder.AddAttribute(6, "Title", "Docked B");
                builder.AddAttribute(7, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Docked B Content")));
                builder.CloseComponent();

                builder.OpenComponent<MariloDockPane>(8);
                builder.AddAttribute(9, "Id", "float-c");
                builder.AddAttribute(10, "Title", "Float C");
                builder.AddAttribute(11, "IsFloating", true);
                builder.AddAttribute(12, "FloatingTop", "40px");
                builder.AddAttribute(13, "FloatingLeft", "60px");
                builder.AddAttribute(14, "FloatingWidth", "300px");
                builder.AddAttribute(15, "FloatingHeight", "200px");
                builder.AddAttribute(16, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Float C Content")));
                builder.CloseComponent();
            }));
        });

        var tabsText = cut.Find(".mar-dockmanager__tabs").TextContent;
        Assert.Contains("Docked A", tabsText);
        Assert.Contains("Docked B", tabsText);
        Assert.DoesNotContain("Float C", tabsText);

        var floatingEl = cut.Find(".mar-dockmanager__floating-pane");
        Assert.Contains("Float C Content", floatingEl.TextContent);
    }

    // ── 13. Drag overlay appears on titlebar mousedown ───────────────

    [Fact]
    public void TitleBarMouseDown_ShowsDragOverlay()
    {
        var cut = RenderWithFloatingPane();

        // No overlay before drag
        Assert.Empty(cut.FindAll(".mar-dockmanager__drag-overlay"));

        // Mousedown on titlebar to start drag
        var titleBar = cut.Find(".mar-dockmanager__floating-pane-titlebar");
        titleBar.MouseDown(new Microsoft.AspNetCore.Components.Web.MouseEventArgs
        {
            ClientX = 100,
            ClientY = 100
        });

        // Drag overlay should now be rendered
        Assert.Single(cut.FindAll(".mar-dockmanager__drag-overlay"));
    }

    // ── 14. Position updates during drag-move ──────────────────────────

    [Fact]
    public void DragMove_UpdatesFloatingPanePosition()
    {
        var cut = RenderWithFloatingPane(top: "100px", left: "200px");

        // Start drag on titlebar
        var titleBar = cut.Find(".mar-dockmanager__floating-pane-titlebar");
        titleBar.MouseDown(new Microsoft.AspNetCore.Components.Web.MouseEventArgs
        {
            ClientX = 210,
            ClientY = 110
        });

        // Move mouse via overlay
        var overlay = cut.Find(".mar-dockmanager__drag-overlay");
        overlay.MouseMove(new Microsoft.AspNetCore.Components.Web.MouseEventArgs
        {
            ClientX = 310,
            ClientY = 210
        });

        // Position should have changed by (100, 100)
        var floatingEl = cut.Find(".mar-dockmanager__floating-pane");
        var style = floatingEl.GetAttribute("style") ?? "";
        Assert.Contains("top:200px", style);
        Assert.Contains("left:300px", style);
    }

    // ── 15. Resize handles render on floating pane ─────────────────────

    [Fact]
    public void FloatingPane_RendersResizeHandles()
    {
        var cut = RenderWithFloatingPane();

        var handles = new[]
        {
            "mar-dockmanager__resize-handle--n",
            "mar-dockmanager__resize-handle--s",
            "mar-dockmanager__resize-handle--e",
            "mar-dockmanager__resize-handle--w",
            "mar-dockmanager__resize-handle--ne",
            "mar-dockmanager__resize-handle--nw",
            "mar-dockmanager__resize-handle--se",
            "mar-dockmanager__resize-handle--sw"
        };

        foreach (var handle in handles)
        {
            Assert.Single(cut.FindAll($".{handle}"));
        }
    }

    // ── 16. Dragging CSS class applied during drag ─────────────────────

    [Fact]
    public void DragMove_AppliesDraggingCssClass()
    {
        var cut = RenderWithFloatingPane();

        // Before drag: no dragging class
        Assert.Empty(cut.FindAll(".mar-dockmanager__floating-pane--dragging"));

        // Start drag
        var titleBar = cut.Find(".mar-dockmanager__floating-pane-titlebar");
        titleBar.MouseDown(new Microsoft.AspNetCore.Components.Web.MouseEventArgs
        {
            ClientX = 100,
            ClientY = 100
        });

        // Dragging class should be applied
        Assert.Single(cut.FindAll(".mar-dockmanager__floating-pane--dragging"));
    }

    // ── 17. Drag ends on mouseup ───────────────────────────────────────

    [Fact]
    public void DragMove_EndsOnMouseUp()
    {
        var cut = RenderWithFloatingPane();

        // Start drag
        var titleBar = cut.Find(".mar-dockmanager__floating-pane-titlebar");
        titleBar.MouseDown(new Microsoft.AspNetCore.Components.Web.MouseEventArgs
        {
            ClientX = 100,
            ClientY = 100
        });

        Assert.Single(cut.FindAll(".mar-dockmanager__drag-overlay"));

        // Release mouse
        var overlay = cut.Find(".mar-dockmanager__drag-overlay");
        overlay.MouseUp(new Microsoft.AspNetCore.Components.Web.MouseEventArgs
        {
            ClientX = 150,
            ClientY = 150
        });

        // Overlay and dragging class should be gone
        Assert.Empty(cut.FindAll(".mar-dockmanager__drag-overlay"));
        Assert.Empty(cut.FindAll(".mar-dockmanager__floating-pane--dragging"));
    }

    // ── 18. Resize handle mousedown starts resize and shows overlay ────

    [Fact]
    public void ResizeHandle_MouseDown_ShowsOverlayAndResizingClass()
    {
        var cut = RenderWithFloatingPane(width: "400px", height: "300px");

        // No overlay initially
        Assert.Empty(cut.FindAll(".mar-dockmanager__drag-overlay"));

        // Mousedown on SE resize handle
        var seHandle = cut.Find(".mar-dockmanager__resize-handle--se");
        seHandle.MouseDown(new Microsoft.AspNetCore.Components.Web.MouseEventArgs
        {
            ClientX = 600,
            ClientY = 400
        });

        // Overlay should appear
        Assert.Single(cut.FindAll(".mar-dockmanager__drag-overlay"));
        // Resizing class should be applied
        Assert.Single(cut.FindAll(".mar-dockmanager__floating-pane--resizing"));
    }

    // -- 19. Reorder on single-tab group is a no-op --

    [Fact]
    public async Task ReorderSingleTab_IsNoOp()
    {
        bool reordered = false;
        var cut = Render<MariloDockManager>(parameters =>
        {
            parameters.Add(p => p.OnTabReordered, (DockTabReorderEventArgs _) => { reordered = true; });
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloDockPane>(0);
                builder.AddAttribute(1, "Id", "solo");
                builder.AddAttribute(2, "Title", "Solo");
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Solo Content")));
                builder.CloseComponent();
            }));
        });

        var tab = cut.Find(".mar-dockmanager__tab");
        await cut.InvokeAsync(() =>
        {
            tab.DragStart();
            tab.DragOver();
            tab.Drop();
            tab.DragEnd();
        });

        Assert.False(reordered);
    }

    // ── 20. Cross-pane tab move fires OnPaneMoved ─────────────────────

    [Fact]
    public async Task CrossPaneMove_FiresOnPaneMoved()
    {
        DockPaneMoveEventArgs? moveArgs = null;
        var cut = Render<MariloDockManager>(parameters =>
        {
            parameters.Add(p => p.OnPaneMoved, (DockPaneMoveEventArgs args) => { moveArgs = args; });
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloDockPane>(0);
                builder.AddAttribute(1, "Id", "pane-a");
                builder.AddAttribute(2, "Title", "Pane A");
                builder.AddAttribute(3, "TabGroupId", "group1");
                builder.AddAttribute(4, "ChildContent", (RenderFragment)(b => b.AddContent(0, "A")));
                builder.CloseComponent();

                builder.OpenComponent<MariloDockPane>(5);
                builder.AddAttribute(6, "Id", "pane-b");
                builder.AddAttribute(7, "Title", "Pane B");
                builder.AddAttribute(8, "TabGroupId", "group2");
                builder.AddAttribute(9, "ChildContent", (RenderFragment)(b => b.AddContent(0, "B")));
                builder.CloseComponent();
            }));
        });

        // Find tabs: group1 has pane-a, group2 has pane-b
        var tabs = cut.FindAll(".mar-dockmanager__tab");
        Assert.Equal(2, tabs.Count);

        // Simulate drag pane-a onto pane-b (cross-group) -- re-query between events
        await cut.InvokeAsync(() => cut.FindAll(".mar-dockmanager__tab")[0].DragStart());
        await cut.InvokeAsync(() => cut.FindAll(".mar-dockmanager__tab")[1].DragOver());
        await cut.InvokeAsync(() => cut.FindAll(".mar-dockmanager__tab")[1].Drop());

        Assert.NotNull(moveArgs);
        Assert.Equal("pane-a", moveArgs!.PaneId);
        Assert.Equal("group1", moveArgs.SourceGroupId);
        Assert.Equal("group2", moveArgs.TargetGroupId);
    }

    // ── 21. Cross-pane move changes pane's group membership ───────────

    [Fact]
    public async Task CrossPaneMove_UpdatesTabGroupId()
    {
        var cut = Render<MariloDockManager>(parameters =>
        {
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloDockPane>(0);
                builder.AddAttribute(1, "Id", "pane-a");
                builder.AddAttribute(2, "Title", "Pane A");
                builder.AddAttribute(3, "TabGroupId", "group1");
                builder.AddAttribute(4, "ChildContent", (RenderFragment)(b => b.AddContent(0, "A")));
                builder.CloseComponent();

                builder.OpenComponent<MariloDockPane>(5);
                builder.AddAttribute(6, "Id", "pane-b");
                builder.AddAttribute(7, "Title", "Pane B");
                builder.AddAttribute(8, "TabGroupId", "group2");
                builder.AddAttribute(9, "ChildContent", (RenderFragment)(b => b.AddContent(0, "B")));
                builder.CloseComponent();
            }));
        });

        // Before move: two groups with one tab each (two .mar-dockmanager__tabs strips)
        var tabStrips = cut.FindAll(".mar-dockmanager__tabs");
        Assert.Equal(2, tabStrips.Count);

        // Drag pane-a to pane-b -- re-query between events
        await cut.InvokeAsync(() => cut.FindAll(".mar-dockmanager__tab")[0].DragStart());
        await cut.InvokeAsync(() => cut.FindAll(".mar-dockmanager__tab")[1].DragOver());
        await cut.InvokeAsync(() => cut.FindAll(".mar-dockmanager__tab")[1].Drop());

        // After move: both panes in group2, so only one group rendered
        var tabStripsAfter = cut.FindAll(".mar-dockmanager__tabs");
        Assert.Single(tabStripsAfter);

        // The group should have data-group-id="group2"
        Assert.Equal("group2", tabStripsAfter[0].GetAttribute("data-group-id"));

        // The single group should contain both tabs
        var tabsAfter = cut.FindAll(".mar-dockmanager__tab");
        Assert.Equal(2, tabsAfter.Count);
    }

    // ── 22. Min width/height constraints enforced during resize ───────

    [Fact]
    public void Resize_RespectsMinWidthConstraint()
    {
        var cut = Render<MariloDockManager>(parameters =>
        {
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloDockPane>(0);
                builder.AddAttribute(1, "Id", "float1");
                builder.AddAttribute(2, "Title", "Floating");
                builder.AddAttribute(3, "IsFloating", true);
                builder.AddAttribute(4, "FloatingWidth", "400px");
                builder.AddAttribute(5, "FloatingHeight", "300px");
                builder.AddAttribute(6, "MinWidth", "200px");
                builder.AddAttribute(7, "MinHeight", "150px");
                builder.AddAttribute(8, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Content")));
                builder.CloseComponent();
            }));
        });

        // Start resize from east handle
        var seHandle = cut.Find(".mar-dockmanager__resize-handle--e");
        seHandle.MouseDown(new Microsoft.AspNetCore.Components.Web.MouseEventArgs
        {
            ClientX = 400,
            ClientY = 150
        });

        // Drag far left to try to shrink below min
        var overlay = cut.Find(".mar-dockmanager__drag-overlay");
        overlay.MouseMove(new Microsoft.AspNetCore.Components.Web.MouseEventArgs
        {
            ClientX = 50,  // delta = -350, would make width 50px < 200px min
            ClientY = 150
        });

        var floatingEl = cut.Find(".mar-dockmanager__floating-pane");
        var style = floatingEl.GetAttribute("style") ?? "";
        // Width should be clamped at 200px (the MinWidth), not 50px
        Assert.Contains("width:200px", style);
    }

    // ── 23. Min height constraint enforced during resize ──────────────

    [Fact]
    public void Resize_RespectsMinHeightConstraint()
    {
        var cut = Render<MariloDockManager>(parameters =>
        {
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloDockPane>(0);
                builder.AddAttribute(1, "Id", "float1");
                builder.AddAttribute(2, "Title", "Floating");
                builder.AddAttribute(3, "IsFloating", true);
                builder.AddAttribute(4, "FloatingWidth", "400px");
                builder.AddAttribute(5, "FloatingHeight", "300px");
                builder.AddAttribute(6, "MinWidth", "100px");
                builder.AddAttribute(7, "MinHeight", "180px");
                builder.AddAttribute(8, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Content")));
                builder.CloseComponent();
            }));
        });

        // Start resize from south handle
        var sHandle = cut.Find(".mar-dockmanager__resize-handle--s");
        sHandle.MouseDown(new Microsoft.AspNetCore.Components.Web.MouseEventArgs
        {
            ClientX = 200,
            ClientY = 300
        });

        // Drag upward to shrink below min height
        var overlay = cut.Find(".mar-dockmanager__drag-overlay");
        overlay.MouseMove(new Microsoft.AspNetCore.Components.Web.MouseEventArgs
        {
            ClientX = 200,
            ClientY = 50   // delta = -250, would make height 50px < 180px min
        });

        var floatingEl = cut.Find(".mar-dockmanager__floating-pane");
        var style = floatingEl.GetAttribute("style") ?? "";
        Assert.Contains("height:180px", style);
    }

    // ── 24. Tab groups render with data-group-id attribute ────────────

    [Fact]
    public void TabGroups_RenderWithGroupIdAttribute()
    {
        var cut = Render<MariloDockManager>(parameters =>
        {
            parameters.Add(p => p.ChildContent, (RenderFragment)(builder =>
            {
                builder.OpenComponent<MariloDockPane>(0);
                builder.AddAttribute(1, "Id", "a");
                builder.AddAttribute(2, "Title", "A");
                builder.AddAttribute(3, "TabGroupId", "left");
                builder.AddAttribute(4, "ChildContent", (RenderFragment)(b => b.AddContent(0, "A")));
                builder.CloseComponent();

                builder.OpenComponent<MariloDockPane>(5);
                builder.AddAttribute(6, "Id", "b");
                builder.AddAttribute(7, "Title", "B");
                builder.AddAttribute(8, "TabGroupId", "right");
                builder.AddAttribute(9, "ChildContent", (RenderFragment)(b => b.AddContent(0, "B")));
                builder.CloseComponent();
            }));
        });

        var tabStrips = cut.FindAll(".mar-dockmanager__tabs");
        Assert.Equal(2, tabStrips.Count);
        Assert.Equal("left", tabStrips[0].GetAttribute("data-group-id"));
        Assert.Equal("right", tabStrips[1].GetAttribute("data-group-id"));
    }

    // ── 25. Default MinWidth/MinHeight are applied when not specified ──

    [Fact]
    public void Resize_UsesDefaultMinConstraints()
    {
        // When MinWidth/MinHeight not specified, defaults to 100px each
        var cut = RenderWithFloatingPane(width: "400px", height: "300px");

        // Start resize from SE corner
        var seHandle = cut.Find(".mar-dockmanager__resize-handle--se");
        seHandle.MouseDown(new Microsoft.AspNetCore.Components.Web.MouseEventArgs
        {
            ClientX = 600,
            ClientY = 400
        });

        // Drag far up-left to try to shrink below default 100px min
        var overlay = cut.Find(".mar-dockmanager__drag-overlay");
        overlay.MouseMove(new Microsoft.AspNetCore.Components.Web.MouseEventArgs
        {
            ClientX = 210,  // delta = -390
            ClientY = 110   // delta = -290
        });

        var floatingEl = cut.Find(".mar-dockmanager__floating-pane");
        var style = floatingEl.GetAttribute("style") ?? "";
        // Should be clamped at default 100px min for both
        Assert.Contains("width:100px", style);
        Assert.Contains("height:100px", style);
    }
}
