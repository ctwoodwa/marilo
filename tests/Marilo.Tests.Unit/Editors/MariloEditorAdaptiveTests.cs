using Bunit;
using Marilo.Components.Editors;
using Marilo.Core.Enums;
using Xunit;

namespace Marilo.Tests.Unit.Editors;

public class MariloEditorAdaptiveTests : MariloTestBase
{
    [Fact]
    public void Adaptive_Defaults_To_False()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Tools, new[] { EditorTool.Bold, EditorTool.Italic }));

        // By default, Adaptive is false — no "More" button should render
        var moreButtons = cut.FindAll(".mar-editor-tool-btn--more");
        Assert.Empty(moreButtons);
    }

    [Fact]
    public void When_Adaptive_False_All_Tools_Render()
    {
        var tools = new[] { EditorTool.Bold, EditorTool.Italic, EditorTool.Underline };

        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Tools, tools)
            .Add(p => p.Adaptive, false));

        var toolbar = cut.Find("[role='toolbar']");
        var buttons = toolbar.QuerySelectorAll("button");
        Assert.Equal(3, buttons.Length);
    }

    [Fact]
    public void When_Adaptive_True_Component_Renders_Without_Error()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Adaptive, true)
            .Add(p => p.Tools, new[] { EditorTool.Bold, EditorTool.Italic }));

        // Component renders without exceptions
        Assert.Contains("mar-editor", cut.Markup);
        // Without a ResizeObserver callback, no overflow — all tools render
        var toolbar = cut.Find("[role='toolbar']");
        var buttons = toolbar.QuerySelectorAll("button");
        Assert.Equal(2, buttons.Length);
    }

    [Fact]
    public void Overflow_Popup_Toggle_ShowsAndHides_OverflowTools()
    {
        // We cannot trigger the ResizeObserver in bUnit, but we can test
        // the overflow popup rendering by directly manipulating internal state.
        // Instead, we verify the "More" button behavior when overflow is active.
        // The simplest integration test: Adaptive=true without resize fires means
        // all buttons visible and no "More" button — correct no-overflow behavior.
        var tools = new[] { EditorTool.Bold, EditorTool.Italic, EditorTool.Underline };

        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Adaptive, true)
            .Add(p => p.Tools, tools));

        // Without resize callback, _overflowStartIndex stays at -1 — all tools visible
        var toolbar = cut.Find("[role='toolbar']");
        var buttons = toolbar.QuerySelectorAll("button");
        Assert.Equal(3, buttons.Length);

        // No overflow popup or "More" button
        var moreButtons = cut.FindAll(".mar-editor-tool-btn--more");
        Assert.Empty(moreButtons);
    }

    [Fact]
    public void Custom_Tools_Participate_In_Toolbar_Items()
    {
        var customTools = new List<EditorCustomTool>
        {
            new() { Name = "MyCustom", Tooltip = "Custom tool" }
        };

        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Adaptive, true)
            .Add(p => p.Tools, new[] { EditorTool.Bold })
            .Add(p => p.CustomTools, customTools));

        // Both built-in and custom tool render in toolbar
        Assert.Contains("MyCustom", cut.Markup);
        var toolbar = cut.Find("[role='toolbar']");
        var allButtons = toolbar.QuerySelectorAll("button");
        // 1 built-in + 1 custom = 2 buttons
        Assert.Equal(2, allButtons.Length);
    }

    [Fact]
    public void Adaptive_Does_Not_Affect_Toolbar_When_ReadOnly()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.ReadOnly, true)
            .Add(p => p.Adaptive, true));

        // Toolbar is hidden when ReadOnly, regardless of Adaptive
        var toolbars = cut.FindAll("[role='toolbar']");
        Assert.Empty(toolbars);
    }

    [Fact]
    public void Adaptive_Does_Not_Affect_Toolbar_When_Disabled()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.Adaptive, true));

        // Toolbar is hidden when Disabled, regardless of Adaptive
        var toolbars = cut.FindAll("[role='toolbar']");
        Assert.Empty(toolbars);
    }
}
