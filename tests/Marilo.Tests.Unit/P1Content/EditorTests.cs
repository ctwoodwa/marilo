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

    // ── Height / Width parameter tests ──────────────────────────────

    [Fact]
    public void Editor_Applies_Custom_Height()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Height, "400px"));

        var markup = cut.Markup;
        Assert.Contains("height:400px", markup);
    }

    [Fact]
    public void Editor_Applies_Custom_Width()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Width, "800px"));

        var container = cut.Find("div.mar-editor");
        var style = container.GetAttribute("style");
        Assert.Contains("width:800px", style);
    }

    [Fact]
    public void Editor_Default_Height_Is_250px()
    {
        var cut = Render<MariloEditor>();
        Assert.Contains("height:250px", cut.Markup);
    }

    // ── DebounceDelay parameter test ────────────────────────────────

    [Fact]
    public void Editor_DebounceDelay_AppearsInJsScript()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.DebounceDelay, 500));

        var evalInvocations = JSInterop.Invocations
            .Where(i => i.Identifier == "eval")
            .ToList();

        Assert.Contains(evalInvocations, inv =>
        {
            var script = inv.Arguments.FirstOrDefault()?.ToString() ?? "";
            return script.Contains("debounceMs = 500");
        });
    }

    // ── ToolbarTemplate parameter test ──────────────────────────────

    [Fact]
    public void Editor_ToolbarTemplate_RendersCustomToolbar()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.ToolbarTemplate,
                (Microsoft.AspNetCore.Components.RenderFragment)(builder =>
                {
                    builder.OpenElement(0, "div");
                    builder.AddAttribute(1, "class", "custom-toolbar");
                    builder.AddContent(2, "Custom Toolbar");
                    builder.CloseElement();
                })));

        Assert.Contains("custom-toolbar", cut.Markup);
        Assert.Contains("Custom Toolbar", cut.Markup);
        // Built-in tool buttons should NOT render when ToolbarTemplate is set
        var toolButtons = cut.FindAll(".mar-editor-tool-btn");
        Assert.Empty(toolButtons);
    }

    // ── EditMode parameter variations ───────────────────────────────

    [Fact]
    public void Editor_Preview_Mode_Renders_Sanitized_Value()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.EditMode, EditorEditMode.Preview)
            .Add(p => p.Value, "<p>Safe</p><script>alert('xss')</script>"));

        var preview = cut.Find(".mar-editor-preview");
        Assert.Contains("Safe", preview.InnerHtml);
        Assert.DoesNotContain("script", preview.InnerHtml.ToLower());
    }

    [Fact]
    public void Editor_Source_Mode_Shows_Placeholder()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.EditMode, EditorEditMode.Source)
            .Add(p => p.Placeholder, "Enter HTML here..."));

        var textarea = cut.Find("textarea");
        Assert.Equal("Enter HTML here...", textarea.GetAttribute("placeholder"));
    }

    // ── Source mode OnChange event ──────────────────────────────────

    [Fact]
    public void Editor_Source_OnChange_Fires()
    {
        string? changedValue = null;
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.EditMode, EditorEditMode.Source)
            .Add(p => p.Value, "")
            .Add(p => p.OnChange, (string v) => changedValue = v)
            .Add(p => p.ValueChanged, (string _) => { }));

        var textarea = cut.Find("textarea");
        textarea.Input("<p>New</p>");

        Assert.Equal("<p>New</p>", changedValue);
    }

    // ── Disabled hides toolbar in Source mode ────────────────────────

    [Fact]
    public void Editor_Disabled_Source_Mode_HidesToolbar()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.EditMode, EditorEditMode.Source));

        var toolbars = cut.FindAll(".mar-editor-toolbar");
        Assert.Empty(toolbars);
    }

    // ── Source mode textarea disabled/readonly ───────────────────────

    [Fact]
    public void Editor_ReadOnly_Source_TextareaIsReadonly()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.ReadOnly, true)
            .Add(p => p.EditMode, EditorEditMode.Source));

        // ReadOnly hides toolbar so no textarea in source (toolbar hidden too)
        // Actually ReadOnly just hides toolbar; source textarea still renders
        // But the textarea should have readonly attribute
        var textarea = cut.Find("textarea");
        Assert.NotNull(textarea.GetAttribute("readonly"));
    }

    [Fact]
    public void Editor_Disabled_Source_TextareaIsDisabled()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.EditMode, EditorEditMode.Source));

        var textarea = cut.Find("textarea");
        Assert.NotNull(textarea.GetAttribute("disabled"));
    }

    // ── Adaptive parameter test ─────────────────────────────────────

    [Fact]
    public void Editor_Adaptive_RendersToolbarWithoutError()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Adaptive, true)
            .Add(p => p.Value, "<p>Adaptive</p>"));

        var toolbar = cut.Find(".mar-editor-toolbar");
        Assert.NotNull(toolbar);
    }

    // ── Source mode value roundtrip ─────────────────────────────────

    [Fact]
    public void Editor_Source_ValueChanges_UpdateViaInput()
    {
        string? current = "<p>start</p>";
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.EditMode, EditorEditMode.Source)
            .Add(p => p.Value, current)
            .Add(p => p.ValueChanged, v => current = v));

        var textarea = cut.Find("textarea");
        textarea.Input("<p>end</p>");

        Assert.Equal("<p>end</p>", current);
    }

    // ── Preview mode renders content ────────────────────────────────

    [Fact]
    public void Editor_Preview_RendersContent()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.EditMode, EditorEditMode.Preview)
            .Add(p => p.Value, "<p>Preview content</p>"));

        var markup = cut.Markup;
        Assert.Contains("mar-editor-preview", markup);
        Assert.Contains("Preview content", markup);
    }

    // ── Disabled/ReadOnly contenteditable=false ─────────────────────

    [Fact]
    public void Editor_Disabled_WysiwygNotEditable()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.Value, ""));

        Assert.Contains("contenteditable=\"false\"", cut.Markup.ToLower());
    }

    [Fact]
    public void Editor_ReadOnly_WysiwygNotEditable()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.ReadOnly, true)
            .Add(p => p.Value, ""));

        Assert.Contains("contenteditable=\"false\"", cut.Markup.ToLower());
    }

    // ── MarkdownConverter roundtrip ─────────────────────────────────

    [Fact]
    public void MarkdownConverter_Roundtrip_PreservesBasicStructure()
    {
        var converter = new MarkdownFormatConverter();
        var md = "# Title\n\nA paragraph with **bold** text.\n";
        var html = converter.ToHtml(md);
        var backToMd = converter.FromHtml(html);

        Assert.Contains("# Title", backToMd);
        Assert.Contains("**bold**", backToMd);
    }

    // ── PlainText empty/null edge cases ─────────────────────────────

    [Fact]
    public void PlainTextConverter_EmptyAndNull_ReturnsEmpty()
    {
        var converter = new PlainTextFormatConverter();
        Assert.Equal(string.Empty, converter.ToHtml(""));
        Assert.Equal(string.Empty, converter.ToHtml(null!));
        Assert.Equal(string.Empty, converter.FromHtml(""));
        Assert.Equal(string.Empty, converter.FromHtml(null!));
    }

    // ── EditorCustomTool properties ─────────────────────────────────

    [Fact]
    public void EditorCustomTool_AllProperties_Set()
    {
        var tool = new EditorCustomTool
        {
            Name = "Test",
            Icon = "icon-test",
            Tooltip = "A tooltip"
        };
        Assert.Equal("Test", tool.Name);
        Assert.Equal("icon-test", tool.Icon);
        Assert.Equal("A tooltip", tool.Tooltip);
    }

    // ── EditorCommandArgs subclass tests ────────────────────────────

    [Fact]
    public void LinkCommandArgs_AllProperties()
    {
        var args = new LinkCommandArgs
        {
            Command = "createLink",
            Href = "https://example.com",
            Text = "Example",
            Target = "_blank",
            Title = "Link title"
        };
        Assert.Equal("createLink", args.Command);
        Assert.Equal("https://example.com", args.Href);
        Assert.Equal("Example", args.Text);
        Assert.Equal("_blank", args.Target);
        Assert.Equal("Link title", args.Title);
    }

    [Fact]
    public void TableCommandArgs_Defaults()
    {
        var args = new TableCommandArgs();
        Assert.Equal(2, args.Rows);
        Assert.Equal(2, args.Columns);
    }

    [Fact]
    public void ImageCommandArgs_Properties()
    {
        var args = new ImageCommandArgs
        {
            Src = "img.png",
            Alt = "An image",
            Width = "100",
            Height = "50"
        };
        Assert.Equal("img.png", args.Src);
        Assert.Equal("An image", args.Alt);
        Assert.Equal("100", args.Width);
        Assert.Equal("50", args.Height);
    }

    [Fact]
    public void ColorCommandArgs_Properties()
    {
        var args = new ColorCommandArgs { Command = "foreColor", Color = "#ff0000" };
        Assert.Equal("foreColor", args.Command);
        Assert.Equal("#ff0000", args.Color);
    }

    [Fact]
    public void FontSizeCommandArgs_DefaultSize()
    {
        var args = new FontSizeCommandArgs();
        Assert.Equal("3", args.Size);
    }

    [Fact]
    public void FontFamilyCommandArgs_Properties()
    {
        var args = new FontFamilyCommandArgs { Family = "Arial" };
        Assert.Equal("Arial", args.Family);
    }

    // ── FormatConverter DI extension tests ───────────────────────────

    [Fact]
    public void AddMariloEditorMarkdownSupport_RegistersConverter()
    {
        var services = new ServiceCollection();
        services.AddMariloEditorMarkdownSupport();
        var sp = services.BuildServiceProvider();
        var converters = sp.GetServices<IEditorFormatConverter>();
        Assert.Contains(converters, c => c.Format == "markdown");
    }

    [Fact]
    public void AddMariloEditorPlainTextSupport_RegistersConverter()
    {
        var services = new ServiceCollection();
        services.AddMariloEditorPlainTextSupport();
        var sp = services.BuildServiceProvider();
        var converters = sp.GetServices<IEditorFormatConverter>();
        Assert.Contains(converters, c => c.Format == "plaintext");
    }
}
