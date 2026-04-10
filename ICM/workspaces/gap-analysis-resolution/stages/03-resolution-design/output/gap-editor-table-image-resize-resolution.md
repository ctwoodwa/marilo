# Resolution Design: Editor Table/Image Resize (JS Interop Batch 3)

> Date: 2026-04-10
> Gap: GAP-EDITOR-004 — Table column/row resize + Image resize drag handles
> Scope: single
> Component: MariloEditor

---

## Gap Description

The MariloEditor spec requires:
- **Tables**: columns and rows are resizable via drag handles on borders
- **Images**: resizable via corner/edge drag handles

Currently, the editor's inline JS module (`GetEditorScript()`) has `insertTable` and image insertion but no resize interaction.

## Resolution Design

### Approach: Inline Overlay Handles (No External Library)

Add resize capability directly within the editor's existing inline JS module closure. This approach:
- Keeps all JS in the single `GetEditorScript()` IIFE (no new JS files)
- Uses `mousedown`/`mousemove`/`mouseup` event delegation on the contenteditable div
- Creates ephemeral resize handles (small squares at corners/edges) that appear on hover/click
- Modifies element `style.width`/`style.height` directly, which naturally triggers `onInput` → C# sync

### Table Resize

1. **Column resize**: On `mousemove` over `<td>`/`<th>` borders (right edge ±4px), change cursor to `col-resize`. On `mousedown`, track drag delta and adjust `style.width` on all cells in that column.
2. **Row resize**: On `mousemove` over `<td>`/`<th>` bottom borders (±4px), change cursor to `row-resize`. On `mousedown`, track drag delta and adjust `style.height` on the row's cells.

### Image Resize

1. **Click detection**: On `click` of an `<img>` inside the editor, show 8 resize handles (corners + midpoints) as absolutely positioned `<div>` elements.
2. **Drag resize**: On `mousedown` of a handle, track drag delta and update `img.style.width`/`img.style.height`. Corner handles preserve aspect ratio; edge handles stretch one dimension.
3. **Dismiss**: Clicking elsewhere or pressing Escape removes the handles.

### JS Module Additions

New functions added to the existing `mod` object:
- `mod.initResizeHandlers()` — called from `mod.init()`, sets up event delegation
- Internal helpers (not exported): `showImageHandles(img)`, `hideImageHandles()`, `handleTableBorderDrag()`

### C# Changes

**None required.** The resize operates entirely within the contenteditable DOM. The existing `onInput` debounce already captures dimension changes as HTML mutations.

### Test Strategy

- bUnit tests verify that the JS interop script string contains the resize handler code
- bUnit tests verify `insertTable` + resize scenario: table cells have `style` attributes after simulated interaction
- Note: Full drag interaction is a browser-level behavior; bUnit tests validate script generation and integration points

### Files to Modify

1. `src/Marilo.Components/Editors/MariloEditor.razor` — extend `GetEditorScript()` JS module

### Files to Create

1. `tests/Marilo.Tests.Unit/Editors/MariloEditorResizeTests.cs` — bUnit tests for resize feature

---

## Decision Record

| # | Decision | Rationale |
|---|----------|-----------|
| 1 | Inline JS, no separate module | Matches existing Editor pattern; all other Editor JS is inline in `GetEditorScript()` |
| 2 | Event delegation on contenteditable | Avoids MutationObserver overhead; leverages existing `mousemove`/`mousedown` naturally |
| 3 | Column resize via cell width adjustment | Simpler than CSS `table-layout: fixed` + `colgroup` manipulation; works with existing `insertTable` output |
| 4 | Aspect-ratio preservation for image corners | Standard UX behavior; Shift key overrides to free-form resize |
| 5 | No C# parameters | Resize is always enabled in Edit mode; ReadOnly/Disabled already prevent contenteditable interaction |
