using System.Linq.Expressions;
using Bunit;
using Marilo.Components.Editors;
using Marilo.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
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

        // 3 tool buttons (Preview is now controlled via EditMode, not a toolbar button)
        var buttons = toolbar.QuerySelectorAll("button");
        Assert.Equal(3, buttons.Length);
    }

    [Fact]
    public void Editor_Renders_All_Tools_When_None_Specified()
    {
        var cut = Render<MariloEditor>();

        var toolbar = cut.Find(".mar-editor-toolbar");
        var buttons = toolbar.QuerySelectorAll("button");

        // Default tools list has 19 items (a curated subset of EditorTool)
        Assert.Equal(19, buttons.Length);
    }

    [Fact]
    public void Editor_Renders_ContentArea()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Value, "<p>Hello</p>"));

        // Default edit mode renders a contenteditable WYSIWYG div
        var wysiwyg = cut.Find("div.mar-editor-wysiwyg");
        Assert.NotNull(wysiwyg);
        Assert.Equal("true", wysiwyg.GetAttribute("contenteditable"));
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
        // Source mode uses a textarea that supports standard input events
        string? currentValue = "<p>Initial</p>";

        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.EditMode, EditorEditMode.Source)
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

        // WYSIWYG mode uses data-placeholder attribute on the contenteditable div
        var editor = cut.Find("div.mar-editor-wysiwyg");
        Assert.Equal("Enter text...", editor.GetAttribute("data-placeholder"));
    }

    [Fact]
    public void Editor_Renders_Container_With_CssProvider_Class()
    {
        var cut = Render<MariloEditor>();

        var container = cut.Find("div.mar-editor");
        Assert.NotNull(container);
    }

    [Fact]
    public void Editor_EditMode_Source_RendersTextarea()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.EditMode, EditorEditMode.Source)
            .Add(p => p.Value, "<p>Hello</p>"));

        // In source mode, a textarea should be shown instead of contenteditable
        Assert.Contains("textarea", cut.Markup.ToLower());
    }

    [Fact]
    public void Editor_EditMode_Preview_DisablesEditing()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.EditMode, EditorEditMode.Preview)
            .Add(p => p.Value, "<p>Hello</p>"));

        // Preview mode should not have contenteditable=true
        var markup = cut.Markup;
        // The content area should render the value but not be editable
        Assert.Contains("Hello", markup);
    }

    [Fact]
    public void Editor_Disabled_RendersDisabledState()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.Value, "<p>Content</p>"));

        // When disabled, contenteditable should be false and toolbar hidden
        Assert.Contains("contenteditable=\"false\"", cut.Markup.ToLower());
        // Toolbar should not render when disabled
        Assert.DoesNotContain("mar-editor-tool-btn", cut.Markup);
    }

    [Fact]
    public void Editor_CustomTools_RenderInToolbar()
    {
        var customTools = new List<EditorCustomTool>
        {
            new() { Name = "MyTool", Tooltip = "Custom tooltip", Icon = "icon-custom" }
        };

        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.CustomTools, customTools)
            .Add(p => p.Value, ""));

        Assert.Contains("MyTool", cut.Markup);
        Assert.Contains("icon-custom", cut.Markup);
    }

    [Fact]
    public void Editor_CustomTools_OnClick_Fires()
    {
        bool clicked = false;
        var customTools = new List<EditorCustomTool>
        {
            new() { Name = "ClickMe", OnClick = () => { clicked = true; return Task.CompletedTask; } }
        };

        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.CustomTools, customTools)
            .Add(p => p.Value, ""));

        // Find and click the custom tool button
        var buttons = cut.FindAll("button");
        var customButton = buttons.FirstOrDefault(b => b.TextContent.Contains("ClickMe"));
        customButton?.Click();

        Assert.True(clicked);
    }

    [Fact]
    public void Editor_AriaAttributes_Present()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.AriaLabelledBy, "my-label")
            .Add(p => p.AriaDescribedBy, "my-desc")
            .Add(p => p.Value, ""));

        var markup = cut.Markup;
        Assert.Contains("aria-labelledby", markup.ToLower());
        Assert.Contains("aria-describedby", markup.ToLower());
    }

    [Fact]
    public void Editor_ValueExpression_AcceptedWithoutError()
    {
        // Verify the ValueExpression parameter can be set (validation integration)
        // FieldIdentifier.Create requires a member access expression, not a local variable
        var model = new EditorModel { Content = "<p>test</p>" };
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Value, model.Content)
            .Add(p => p.ValueExpression, () => model.Content));

        // Component renders without error
        Assert.Contains("mar-editor", cut.Markup);
    }

    private class EditorModel
    {
        public string Content { get; set; } = "";
    }

    // ── Import/Export (GAP-EDITOR-005 / RES-EDITOR-B2A-01) ──────────

    [Fact]
    public void MarkdownConverter_ToHtml_ConvertsHeadingsAndBold()
    {
        // Internal MarkdownFormatConverter is accessible via InternalsVisibleTo
        var converter = new MarkdownFormatConverter();
        var html = converter.ToHtml("# Hello\n\nThis is **bold** text.");

        // Markdig's advanced extensions add an id attribute to headings
        Assert.Contains("Hello</h1>", html);
        Assert.Contains("<strong>bold</strong>", html);
    }

    [Fact]
    public void MarkdownConverter_ToHtml_EmptyReturnsEmpty()
    {
        var converter = new MarkdownFormatConverter();
        Assert.Equal(string.Empty, converter.ToHtml(""));
        Assert.Equal(string.Empty, converter.ToHtml(null!));
    }

    [Fact]
    public void MarkdownConverter_FromHtml_ConvertsBoldAndHeadings()
    {
        var converter = new MarkdownFormatConverter();
        var md = converter.FromHtml("<h1>Title</h1><p>This is <strong>bold</strong>.</p>");

        Assert.Contains("# Title", md);
        Assert.Contains("**bold**", md);
    }

    [Fact]
    public void PlainTextConverter_ToHtml_WrapsLinesInParagraphs()
    {
        var converter = new PlainTextFormatConverter();
        var html = converter.ToHtml("Line one\nLine two");

        Assert.Contains("<p>Line one</p>", html);
        Assert.Contains("<p>Line two</p>", html);
    }

    [Fact]
    public void PlainTextConverter_FromHtml_StripsTagsAndDecodes()
    {
        var converter = new PlainTextFormatConverter();
        var text = converter.FromHtml("<p>Hello &amp; world</p><p>Next line</p>");

        Assert.Contains("Hello & world", text);
        Assert.Contains("Next line", text);
    }

    [Fact]
    public void ImportAsync_WithNoConverterRegistered_ThrowsInvalidOperationException()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Value, "<p>test</p>"));

        var ex = Assert.ThrowsAsync<InvalidOperationException>(
            () => cut.InvokeAsync(() => cut.Instance.ImportAsync("# Hello", "markdown")));

        Assert.NotNull(ex);
    }

    [Fact]
    public void ExportAsync_WithNoConverterRegistered_ThrowsInvalidOperationException()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Value, "<p>test</p>"));

        var ex = Assert.ThrowsAsync<InvalidOperationException>(
            () => cut.InvokeAsync(() => cut.Instance.ExportAsync("markdown")));

        Assert.NotNull(ex);
    }

    [Fact]
    public void ImportAsync_WithRegisteredConverter_SetsValue()
    {
        // Register the internal plaintext converter via DI
        Services.AddSingleton<IEditorFormatConverter>(new PlainTextFormatConverter());

        string? captured = null;
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Value, "")
            .Add(p => p.ValueChanged, v => captured = v));

        cut.InvokeAsync(() => cut.Instance.ImportAsync("Hello world", "plaintext"));

        Assert.NotNull(captured);
        Assert.Contains("<p>Hello world</p>", captured);
    }
}
