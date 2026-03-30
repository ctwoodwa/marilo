using Bunit;
using Marilo.Components.Layout;
using Marilo.Core.Enums;
using Xunit;

namespace Marilo.Tests.Unit.P2Enhancements;

public class TabStripOverflowTests : MariloTestBase
{
    [Fact]
    public void TabStrip_OverflowModeMenu_RendersOverflowButton()
    {
        // Arrange & Act
        var cut = Render<MariloTabStrip>(parameters => parameters
            .Add(p => p.OverflowMode, TabStripOverflowMode.Menu)
            .Add(p => p.MaxVisibleTabs, 2)
            .Add(p => p.ActiveTabId, "t1")
            .AddChildContent<TabStripTab>(tab => tab
                .Add(t => t.Id, "t1")
                .Add(t => t.Title, "Tab 1")
                .AddChildContent("<p>Content 1</p>"))
            .AddChildContent<TabStripTab>(tab => tab
                .Add(t => t.Id, "t2")
                .Add(t => t.Title, "Tab 2")
                .AddChildContent("<p>Content 2</p>"))
            .AddChildContent<TabStripTab>(tab => tab
                .Add(t => t.Id, "t3")
                .Add(t => t.Title, "Tab 3")
                .AddChildContent("<p>Content 3</p>"))
        );

        // Assert — overflow button should be present
        var overflowBtn = cut.Find(".mar-tabs__overflow-btn");
        Assert.NotNull(overflowBtn);
    }

    [Fact]
    public void TabStrip_OverflowModeMenu_HidesOverflowTabsFromMainList()
    {
        // Arrange & Act
        var cut = Render<MariloTabStrip>(parameters => parameters
            .Add(p => p.OverflowMode, TabStripOverflowMode.Menu)
            .Add(p => p.MaxVisibleTabs, 1)
            .Add(p => p.ActiveTabId, "t1")
            .AddChildContent<TabStripTab>(tab => tab
                .Add(t => t.Id, "t1")
                .Add(t => t.Title, "Tab 1")
                .AddChildContent("<p>Content 1</p>"))
            .AddChildContent<TabStripTab>(tab => tab
                .Add(t => t.Id, "t2")
                .Add(t => t.Title, "Tab 2")
                .AddChildContent("<p>Content 2</p>"))
        );

        // Assert — only 1 tab button with role="tab" should be visible in main list
        var tabButtons = cut.FindAll("[role='tab']");
        Assert.Single(tabButtons);
    }

    [Fact]
    public void TabStrip_OverflowModeMenu_ClickOverflowButtonShowsDropdown()
    {
        // Arrange
        var cut = Render<MariloTabStrip>(parameters => parameters
            .Add(p => p.OverflowMode, TabStripOverflowMode.Menu)
            .Add(p => p.MaxVisibleTabs, 1)
            .Add(p => p.ActiveTabId, "t1")
            .AddChildContent<TabStripTab>(tab => tab
                .Add(t => t.Id, "t1")
                .Add(t => t.Title, "Tab 1")
                .AddChildContent("<p>Content 1</p>"))
            .AddChildContent<TabStripTab>(tab => tab
                .Add(t => t.Id, "t2")
                .Add(t => t.Title, "Tab 2")
                .AddChildContent("<p>Content 2</p>"))
        );

        // Act — click the overflow button
        var overflowBtn = cut.Find(".mar-tabs__overflow-btn");
        overflowBtn.Click();

        // Assert — dropdown menu should now be visible
        var menu = cut.Find(".mar-tabs__overflow-menu");
        Assert.NotNull(menu);
    }

    [Fact]
    public void TabStrip_OverflowModeNone_DoesNotRenderOverflowButton()
    {
        // Arrange & Act
        var cut = Render<MariloTabStrip>(parameters => parameters
            .Add(p => p.OverflowMode, TabStripOverflowMode.None)
            .Add(p => p.ActiveTabId, "t1")
            .AddChildContent<TabStripTab>(tab => tab
                .Add(t => t.Id, "t1")
                .Add(t => t.Title, "Tab 1")
                .AddChildContent("<p>Content 1</p>"))
        );

        // Assert — no overflow button
        var overflowBtns = cut.FindAll(".mar-tabs__overflow-btn");
        Assert.Empty(overflowBtns);
    }
}
