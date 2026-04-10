# Implementation Log: Editor Table/Image Resize (JS Interop Batch 3)

> Date: 2026-04-10
> Gap: GAP-EDITOR-004 — Table column/row resize + Image resize drag handles
> Scope: single
> Component: MariloEditor

---

## Changes

### `src/Marilo.Components/Editors/MariloEditor.razor`

Extended the inline `GetEditorScript()` JS module with ~170 lines of table and image resize logic:

**Table Resize:**
- `getResizeEdge(cell, e)` — detects if mouse is within 5px of cell right/bottom border
- `getCellsInColumn(table, colIndex)` — collects all cells in a column for uniform width adjustment
- `handleTableMouseMove(e)` — shows `col-resize` or `row-resize` cursor on cell borders
- `handleTableMouseDown(e)` — initiates drag tracking for column width or row height
- `onTableDrag(e)` / `onTableDragEnd()` — applies delta to cell dimensions, fires `onInput` to sync to C#

**Image Resize:**
- `showImageHandles(img)` — wraps image in a `contentEditable=false` span, places 8 resize handles (corners + midpoints)
- `hideImageHandles()` — unwraps image, removes handles
- `startImageResize(e, img, pos)` — initiates drag tracking with aspect ratio calculation
- `onImageDrag(e)` — applies resize delta; corners preserve aspect ratio (Shift overrides to free-form)
- `onImageDragEnd()` — cleans up, re-shows handles at new size, fires `onInput`
- `handleEditorClick(e)` — shows handles when clicking an image, hides when clicking elsewhere
- `handleEditorKeyDown(e)` — Escape dismisses image handles

**Init/Dispose Integration:**
- Wrapped `mod.init` to also attach resize event listeners (mousemove, mousedown, click, keydown)
- Extended `mod.dispose` to remove all resize listeners and clean up any active image handles

### No C# Changes Required

The resize operates entirely within the contenteditable DOM. Existing `onInput` debounce already captures dimension changes as HTML mutations and syncs to C# `Value`.

## Tests

14 new bUnit tests in `tests/Marilo.Tests.Unit/Editors/MariloEditorResizeTests.cs`:

| # | Test | What it verifies |
|---|------|-----------------|
| 1 | Editor_Renders_Contenteditable_In_Edit_Mode | Contenteditable div present |
| 2 | Editor_In_ReadOnly_Mode_Does_Not_Have_Contenteditable | ReadOnly prevents resize |
| 3 | JS_Module_Init_Invoked_With_Table_Resize_Support | Table resize functions in JS |
| 4 | JS_Module_Init_Invoked_With_Image_Resize_Support | Image resize functions in JS |
| 5 | JS_Script_Contains_Column_Resize_Logic | col-resize cursor, column cell traversal |
| 6 | JS_Script_Contains_Row_Resize_Logic | row-resize cursor, height adjustment |
| 7 | JS_Script_Contains_Image_Aspect_Ratio_Preservation | Aspect ratio + Shift override |
| 8 | JS_Script_Contains_Handle_Positions_For_All_Eight_Directions | All 8 handle positions |
| 9 | JS_Script_Disposes_Resize_Listeners | Cleanup in dispose |
| 10 | JS_Script_Escape_Key_Dismisses_Image_Handles | Escape key dismissal |
| 11 | JS_Script_Table_Border_Zone_Is_Reasonable | 5px border detection zone |
| 12 | JS_Script_Image_Handle_Size_Is_Reasonable | 8px handle size |
| 13 | Table_Tool_Is_In_Default_ToolSet_Or_Can_Be_Added | Table+Image tools work |
| 14 | Disabled_Editor_Does_Not_Render_Toolbar | Disabled prevents resize |

**Full suite:** 1097/1097 passing (up from 1083). Zero regressions.
