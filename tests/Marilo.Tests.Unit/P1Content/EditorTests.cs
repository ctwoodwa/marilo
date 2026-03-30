using Bunit;
using Marilo.Components.Editors;
using Marilo.Core.Enums;
using Xunit;

namespace Marilo.Tests.Unit.P1Content;

public class EditorTests : MariloTestBase
{
    [Fact]
    public void Editor_Renders_Toolbar_With_Tools()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Tools, new[] { EditorTool.Bold, EditorTool.Italic, EditorTool.Underline }));

        var toolbar = cut.Find(".mar-editor-toolbar");
        Assert.NotNull(toolbar);

        // 3 tool buttons + 1 Preview button
        var buttons = toolbar.QuerySelectorAll("button");
        Assert.Equal(4, buttons.Length);
    }

    [Fact]
    public void Editor_Renders_All_Tools_When_None_Specified()
    {
        var cut = Render<MariloEditor>();

        var toolbar = cut.Find(".mar-editor-toolbar");
        var buttons = toolbar.QuerySelectorAll("button");

        // All EditorTool values + Preview button
        var expectedCount = Enum.GetValues<EditorTool>().Length + 1;
        Assert.Equal(expectedCount, buttons.Length);
    }

    [Fact]
    public void Editor_Renders_ContentArea()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Value, "<p>Hello</p>"));

        var textarea = cut.Find("textarea.mar-editor-content");
        Assert.NotNull(textarea);
    }

    [Fact]
    public void Editor_Hides_Toolbar_When_ReadOnly()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.ReadOnly, true));

        var toolbars = cut.FindAll(".mar-editor-toolbar");
        Assert.Empty(toolbars);
    }

    [Fact]
    public void Editor_Value_Binding_Works()
    {
        string? currentValue = "<p>Initial</p>";

        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Value, currentValue)
            .Add(p => p.ValueChanged, (string val) => currentValue = val));

        var textarea = cut.Find("textarea");
        textarea.Input("<p>Updated</p>");

        Assert.Equal("<p>Updated</p>", currentValue);
    }

    [Fact]
    public void Editor_Renders_With_Placeholder()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Placeholder, "Enter text..."));

        var textarea = cut.Find("textarea");
        Assert.Equal("Enter text...", textarea.GetAttribute("placeholder"));
    }

    [Fact]
    public void Editor_Renders_Container_With_CssProvider_Class()
    {
        var cut = Render<MariloEditor>();

        var container = cut.Find("div.mar-editor");
        Assert.NotNull(container);
    }
}
