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

    // -- 13. Reorder on single-tab group is a no-op --

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
}
