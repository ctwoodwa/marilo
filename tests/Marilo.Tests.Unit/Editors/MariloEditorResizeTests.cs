using Bunit;
using Marilo.Components.Editors;
using Marilo.Core.Enums;
using Xunit;

namespace Marilo.Tests.Unit.Editors;

/// <summary>
/// Tests for Editor table column/row resize and image resize drag handles
/// (GAP-EDITOR-004 — JS Interop Batch 3).
/// </summary>
public class MariloEditorResizeTests : MariloTestBase
{
    [Fact]
    public void Editor_Renders_Contenteditable_In_Edit_Mode()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Tools, new[] { EditorTool.Bold }));

        var contentArea = cut.Find("[contenteditable='true']");
        Assert.NotNull(contentArea);
        Assert.Contains("mar-editor-wysiwyg", contentArea.ClassList);
    }

    [Fact]
    public void Editor_In_ReadOnly_Mode_Does_Not_Have_Contenteditable()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.ReadOnly, true)
            .Add(p => p.Tools, new[] { EditorTool.Bold }));

        // ReadOnly makes contenteditable false — resize handles won't activate
        var contentArea = cut.Find("[contenteditable='false']");
        Assert.NotNull(contentArea);
    }

    [Fact]
    public void JS_Module_Init_Invoked_With_Table_Resize_Support()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Tools, new[] { EditorTool.Bold }));

        // The JS module is loaded via eval(...) — verify the interop was invoked
        var evalInvocations = JSInterop.Invocations
            .Where(i => i.Identifier == "eval")
            .ToList();

        // At least one eval call should contain the resize handler code
        Assert.Contains(evalInvocations, inv =>
        {
            var script = inv.Arguments.FirstOrDefault()?.ToString() ?? "";
            return script.Contains("handleTableMouseMove") &&
                   script.Contains("handleTableMouseDown") &&
                   script.Contains("getResizeEdge");
        });
    }

    [Fact]
    public void JS_Module_Init_Invoked_With_Image_Resize_Support()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Tools, new[] { EditorTool.Bold }));

        var evalInvocations = JSInterop.Invocations
            .Where(i => i.Identifier == "eval")
            .ToList();

        Assert.Contains(evalInvocations, inv =>
        {
            var script = inv.Arguments.FirstOrDefault()?.ToString() ?? "";
            return script.Contains("showImageHandles") &&
                   script.Contains("hideImageHandles") &&
                   script.Contains("startImageResize") &&
                   script.Contains("HANDLE_POSITIONS");
        });
    }

    [Fact]
    public void JS_Script_Contains_Column_Resize_Logic()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Tools, new[] { EditorTool.Bold }));

        var evalInvocations = JSInterop.Invocations
            .Where(i => i.Identifier == "eval")
            .ToList();

        Assert.Contains(evalInvocations, inv =>
        {
            var script = inv.Arguments.FirstOrDefault()?.ToString() ?? "";
            return script.Contains("col-resize") &&
                   script.Contains("getCellsInColumn") &&
                   script.Contains("onTableDrag") &&
                   script.Contains("onTableDragEnd");
        });
    }

    [Fact]
    public void JS_Script_Contains_Row_Resize_Logic()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Tools, new[] { EditorTool.Bold }));

        var evalInvocations = JSInterop.Invocations
            .Where(i => i.Identifier == "eval")
            .ToList();

        Assert.Contains(evalInvocations, inv =>
        {
            var script = inv.Arguments.FirstOrDefault()?.ToString() ?? "";
            return script.Contains("row-resize") &&
                   script.Contains("style.height");
        });
    }

    [Fact]
    public void JS_Script_Contains_Image_Aspect_Ratio_Preservation()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Tools, new[] { EditorTool.Bold }));

        var evalInvocations = JSInterop.Invocations
            .Where(i => i.Identifier == "eval")
            .ToList();

        // Corner handles preserve aspect ratio; Shift overrides
        Assert.Contains(evalInvocations, inv =>
        {
            var script = inv.Arguments.FirstOrDefault()?.ToString() ?? "";
            return script.Contains("aspect") &&
                   script.Contains("e.shiftKey") &&
                   script.Contains("nwse-resize");
        });
    }

    [Fact]
    public void JS_Script_Contains_Handle_Positions_For_All_Eight_Directions()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Tools, new[] { EditorTool.Bold }));

        var evalInvocations = JSInterop.Invocations
            .Where(i => i.Identifier == "eval")
            .ToList();

        Assert.Contains(evalInvocations, inv =>
        {
            var script = inv.Arguments.FirstOrDefault()?.ToString() ?? "";
            return script.Contains("'nw'") &&
                   script.Contains("'n'") &&
                   script.Contains("'ne'") &&
                   script.Contains("'e'") &&
                   script.Contains("'se'") &&
                   script.Contains("'s'") &&
                   script.Contains("'sw'") &&
                   script.Contains("'w'");
        });
    }

    [Fact]
    public void JS_Script_Disposes_Resize_Listeners()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Tools, new[] { EditorTool.Bold }));

        var evalInvocations = JSInterop.Invocations
            .Where(i => i.Identifier == "eval")
            .ToList();

        // Dispose function should clean up resize event listeners and image handles
        Assert.Contains(evalInvocations, inv =>
        {
            var script = inv.Arguments.FirstOrDefault()?.ToString() ?? "";
            // The dispose block must remove table and image event listeners
            return script.Contains("mod.dispose") &&
                   script.Contains("hideImageHandles") &&
                   script.Contains("handleTableMouseMove") &&
                   script.Contains("handleEditorClick");
        });
    }

    [Fact]
    public void JS_Script_Escape_Key_Dismisses_Image_Handles()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Tools, new[] { EditorTool.Bold }));

        var evalInvocations = JSInterop.Invocations
            .Where(i => i.Identifier == "eval")
            .ToList();

        Assert.Contains(evalInvocations, inv =>
        {
            var script = inv.Arguments.FirstOrDefault()?.ToString() ?? "";
            return script.Contains("'Escape'") &&
                   script.Contains("activeImageHandles") &&
                   script.Contains("handleEditorKeyDown");
        });
    }

    [Fact]
    public void JS_Script_Table_Border_Zone_Is_Reasonable()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Tools, new[] { EditorTool.Bold }));

        var evalInvocations = JSInterop.Invocations
            .Where(i => i.Identifier == "eval")
            .ToList();

        // BORDER_ZONE should be a small pixel value (4-6px is typical)
        Assert.Contains(evalInvocations, inv =>
        {
            var script = inv.Arguments.FirstOrDefault()?.ToString() ?? "";
            return script.Contains("BORDER_ZONE = 5");
        });
    }

    [Fact]
    public void JS_Script_Image_Handle_Size_Is_Reasonable()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Tools, new[] { EditorTool.Bold }));

        var evalInvocations = JSInterop.Invocations
            .Where(i => i.Identifier == "eval")
            .ToList();

        // HANDLE_SIZE should be 6-10px
        Assert.Contains(evalInvocations, inv =>
        {
            var script = inv.Arguments.FirstOrDefault()?.ToString() ?? "";
            return script.Contains("HANDLE_SIZE = 8");
        });
    }

    [Fact]
    public void Table_Tool_Is_In_Default_ToolSet_Or_Can_Be_Added()
    {
        // Verify Table tool can be explicitly included
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Tools, new[] { EditorTool.Table, EditorTool.Image }));

        var toolbar = cut.Find("[role='toolbar']");
        var buttons = toolbar.QuerySelectorAll("button");
        Assert.Equal(2, buttons.Length);
    }

    [Fact]
    public void Disabled_Editor_Does_Not_Render_Toolbar()
    {
        var cut = Render<MariloEditor>(parameters => parameters
            .Add(p => p.Disabled, true)
            .Add(p => p.Tools, new[] { EditorTool.Bold }));

        // When disabled, toolbar is not rendered — resize handles cannot be triggered
        var toolbars = cut.FindAll("[role='toolbar']");
        Assert.Empty(toolbars);
    }
}
