# GAP-DATASHEET-V03 — Selection and Ranges Verification (Result)

**Sub-task:** GAP-DATASHEET-V03 from `datasheet-spec-gaps-2026-04-10.md`
**Spec:** `docs/component-specs/datasheet/selection-and-ranges.md` (121 lines, fully read)
**Source audited:** `src/Marilo.Components/DataGrid/MariloDataSheet.Editing.cs` + `.razor.cs`
**Verification date:** 2026-04-10 (cron fires #10/#11)

## Result: **8 confirmed implementation gaps** (not verification tasks)

This sub-audit was flagged in the re-audit as "highest-probability real gap". Confirmed — this is real implementation work, not a paperwork exercise. The DataSheet's selection model currently supports **single active cell + row selection for delete**, but **zero cell-range selection**, and the `Ctrl+D` Fill Down command uses the wrong scope.

The spec itself acknowledges the gap partially at lines 76-78 and 93-108 ("Selection state is currently internal to the component. There is no public API to get or set the selected range programmatically.") but then describes **operations that depend on range selection working** (Shift+Click, Shift+Arrow, Click+Drag, Ctrl+A, Ctrl+C, Ctrl+D Fill Down, Delete-in-range) as if they're functional. Those operations are not implemented.

## Evidence — what the source has vs what the spec requires

### Active cell tracking ✅ PRESENT
Source has `_activeCellRow` and `_activeCellField` at `MariloDataSheet.Editing.cs:15-16`. `ActivateCell` method at `:40-41`. `ClearActiveCell` at `:49-50`. `IsCellActive` style comparison at `:57-59`. Spec "Active Cell" section (lines 26-35) is fully matched.

### Row selection ✅ PRESENT
Source has `HashSet<TItem> _selectedRows` at `MariloDataSheet.razor.cs:17`. `OnSelectAllChanged`, `ToggleRowSelection`, `BulkDeleteAsync`, `BulkResetAsync` at `.razor.cs:140-181`. Spec "Row Selection" section (lines 48-55) is matched.

### Cell range selection ❌ COMPLETELY ABSENT
Grep for `Range|SelectedRange|_rangeStart|_rangeEnd|OnRangeSelect|CellRange|MouseDown|OnMouseDown|shiftKey|ctrlKey|metaKey|DragSelect|_anchor` across all `MariloDataSheet*` files returned **40 matches, all on `_activeCell*` references**. Zero matches on any range-related symbol. No range state, no range rendering, no range API.

---

## Confirmed Gap List

### GAP-DATASHEET-V03-01: Missing rectangular range state
**Severity:** High
**Spec:** `selection-and-ranges.md:37-46` (Rectangular Range Selection section)

**Target:** Source maintains an anchor-cell and extent-cell pair (or equivalent) such that a rectangular range of cells can be tracked separately from the active cell. When no explicit range is set, the "range" degenerates to the single active cell.

**Current:** No range state at all. Only `_activeCellRow` and `_activeCellField` exist.

**Recommended direction:** Add `_rangeAnchorRow` / `_rangeAnchorField` / `_rangeExtentRow` / `_rangeExtentField` internal fields. Introduce an internal `DataSheetCellCoord` record type (or a tuple) for readability. Provide an `IsInRange(row, field)` predicate that the `.Rendering.cs` partial consults when rendering cell classes so range cells get the visual highlight via `DataSheetCellClass(state, isActive, isEditable)` — the spec's CSS provider signature already includes range-compatible flags.

**Status:** Open — confirmed real gap

---

### GAP-DATASHEET-V03-02: Shift+Click does not extend range
**Severity:** High
**Spec:** `selection-and-ranges.md:59-65` (Mouse section, Shift+Click row)

**Target:** Shift+Click on a cell sets `_rangeExtent` to the clicked cell while preserving `_rangeAnchor` at the current active cell. The active cell does not move (anchor stays put). The range rectangle is computed from the two corners.

**Current:** Clicking any cell calls `ActivateCell`, which unconditionally reassigns both `_activeCellRow` and `_activeCellField` at `.Editing.cs:27-28` and `:40-41`. No shift-key handling, no anchor preservation, no range extension.

**Recommended direction:** The cell click handler (in `.Rendering.cs` or `.Editing.cs`) needs to receive a `MouseEventArgs` and inspect `args.ShiftKey`. When true, do not move the anchor — set `_rangeExtentRow`/`_rangeExtentField` to the clicked cell. When false (default), move the anchor AND clear the range.

**Status:** Open — confirmed real gap

---

### GAP-DATASHEET-V03-03: Click-and-drag does not create range
**Severity:** Medium (nice-to-have UX; Shift+Click and Shift+Arrow cover the common case)
**Spec:** `selection-and-ranges.md:59-65` (Mouse section, Click+Drag row)

**Target:** Mouse-down on a cell starts tracking a drag selection; mouse-move over other cells extends the range live; mouse-up finalizes the range.

**Current:** No `@onmousedown`, `@onmousemove`, or `@onmouseup` handlers on any DataSheet cell (grep-confirmed — zero `MouseDown` hits). Only `@onclick`.

**Recommended direction:** Add three mouse handlers on the `<td>` element in `.Rendering.cs`:
- `@onmousedown`: call `ActivateCell(row, field)` and set `_isDraggingRange = true`
- `@onmousemove`: if `_isDraggingRange`, update `_rangeExtent` to the current cell
- `@onmouseup` (on root): set `_isDraggingRange = false`

This depends on V03-01 (range state) landing first.

**Status:** Open — confirmed real gap

---

### GAP-DATASHEET-V03-04: Shift+Arrow does not extend range
**Severity:** High
**Spec:** `selection-and-ranges.md:67-75` (Keyboard section, Shift+Arrow row)

**Target:** Shift+Arrow extends the range extent in the arrow direction while leaving the anchor (active cell) in place.

**Current:** The arrow key handler at `MariloDataSheet.Editing.cs:186-205` only calls `ActivateCell(...)`, which moves the active cell. The handler does not inspect `args.ShiftKey` at all.

**Recommended direction:** In the arrow key branch of `HandleKeyDown` at `.Editing.cs:186`, check `args.ShiftKey`:
- If true: move `_rangeExtent` in the arrow direction; do NOT move `_activeCell`
- If false (current behavior): move `_activeCell` AND clear the range

This depends on V03-01 (range state).

**Status:** Open — confirmed real gap

---

### GAP-DATASHEET-V03-05: Ctrl+A select-all not implemented
**Severity:** Medium
**Spec:** `selection-and-ranges.md:74` (Keyboard section, Ctrl+A row)

**Target:** Ctrl+A selects all cells in the DataSheet as a single rectangular range spanning row 0..end × col 0..end.

**Current:** Grep-confirmed — no `Ctrl+A` branch in the `HandleKeyDown` handler. `.Editing.cs` has Ctrl+Z (undo, line 125), Ctrl+D (fill down, line 140) branches, but no Ctrl+A.

**Recommended direction:** Add a Ctrl+A branch to `HandleKeyDown` that sets `_rangeAnchor` to `(_displayRows[0], _columns[0].Field)` and `_rangeExtent` to `(_displayRows[^1], _columns[^1].Field)`. Depends on V03-01.

**Status:** Open — confirmed real gap

---

### GAP-DATASHEET-V03-06: Ctrl+C copy not implemented
**Severity:** Medium
**Spec:** `selection-and-ranges.md:85-86` + `bulk-paste-and-clipboard.md` (referenced for TSV format)

**Target:** Ctrl+C copies the values of all cells in the selected range as a TSV string to the clipboard.

**Current:** Grep found no Ctrl+C branch. Only paste (`PasteFromClipboard`) is JS-interop wired.

**Recommended direction:** Add a `CopyToClipboard()` method to `MariloDataSheet.Interop.cs`, mirroring the existing `PasteFromClipboard` shape. JS interop invokes `navigator.clipboard.writeText(tsvString)`. Called from a Ctrl+C branch in `HandleKeyDown`. Depends on V03-01.

**Status:** Open — confirmed real gap

---

### GAP-DATASHEET-V03-07: Ctrl+D Fill Down uses wrong scope (row selection instead of cell range)
**Severity:** High — this is a **bug**, not just a missing feature
**Spec:** `selection-and-ranges.md:88` (Fill Down row) — "Copies the value of the active cell (top row of the selection) down to all cells **in the same column within the selected range**. Only editable, non-computed cells are filled."

**Current:** `MariloDataSheet.Editing.cs:140-151` (Ctrl+D handler):
```csharp
if (ctrl && key == "d" && _activeCellRow != null && _activeCellField != null)
{
    var value = GridReflectionHelper.GetValue(_activeCellRow, _activeCellField);
    var startIdx = _displayRows.IndexOf(_activeCellRow);
    // ... iterates _selectedRows HashSet at line 151 ...
    await CommitCellEdit(selectedRow, _activeCellField, value);
}
```

The implementation uses `_selectedRows` (the **row selection HashSet** used for bulk delete) as the Fill Down scope. This is wrong per spec — Fill Down should fill cells in the **cell range**, not the delete-row-selection.

**Consequence:** If the user selects (via row checkboxes) three rows and then presses Ctrl+D, the active cell's value gets filled into those three rows' same-column cells — which may or may not be what the user wants, but is not what the spec promises. If the user presses Ctrl+D with no row selection, Fill Down is a no-op even if a cell range is visually implied.

**Recommended direction:** Rewrite the Ctrl+D branch to iterate cells in the range (`_rangeAnchor` to `_rangeExtent`) within the active cell's column. Skip computed and non-editable columns. Depends on V03-01.

**Also worth noting:** When the range is a single cell (anchor == extent == active cell), Fill Down has nothing to do and should be a no-op — not the current behavior of falling back to row-selection scope.

**Status:** Open — confirmed real gap, existing behavior is a **bug**

---

### GAP-DATASHEET-V03-08: Delete clears only active cell, not range
**Severity:** Medium
**Spec:** `selection-and-ranges.md:89` — "Clears all editable cells in the selected range, setting each to its type's default value"

**Current:** `MariloDataSheet.Editing.cs:175-180` (Delete key handler):
```csharp
if (key == "Delete" && _activeCellRow != null && _activeCellField != null)
{
    var column = _columns.FirstOrDefault(c => c.Field == _activeCellField);
    // ...
    await CommitCellEdit(_activeCellRow, _activeCellField, GetDefaultValue(column));
}
```

Only the single active cell is cleared. No iteration over a range.

**Recommended direction:** Iterate over cells in the range (`_rangeAnchor` to `_rangeExtent`), skip computed/non-editable columns, commit default value for each. Depends on V03-01.

**Status:** Open — confirmed real gap

---

## Summary of V03 Results

| Gap | Severity | Type | Depends on |
|---|---|---|---|
| V03-01 Range state | High | Missing foundation | — |
| V03-02 Shift+Click | High | Missing feature | V03-01 |
| V03-03 Click+Drag | Medium | Missing feature | V03-01 |
| V03-04 Shift+Arrow | High | Missing feature | V03-01 |
| V03-05 Ctrl+A | Medium | Missing feature | V03-01 |
| V03-06 Ctrl+C | Medium | Missing feature | V03-01 |
| V03-07 Ctrl+D wrong scope | High | **Bug** | V03-01 |
| V03-08 Delete only clears active | Medium | Missing feature | V03-01 |

**Severity distribution:** 4 High, 4 Medium, 0 Low, 0 Critical. **V03-01 is the critical path** — nothing else can land until the range state exists. V03-07 is the most visible because it's an existing bug (wrong scope), not just missing functionality.

## Recommended Stage 03 Resolution

Treat the 8 gaps as a single **"DataSheet cell range selection"** implementation batch. All 8 touch the same subsystem (`.Editing.cs` + `.Rendering.cs`) and should land together to avoid half-implemented intermediate states. Estimated scope: 1-2 commits, 8-15 bUnit tests, similar in size to a typical T4 Picker batch from the main workspace.

**Suggested batch layout:**

1. **Batch A — Foundation:** V03-01 (range state fields + `IsInRange` predicate + render highlight)
2. **Batch B — Input:** V03-02 (Shift+Click), V03-03 (Click+Drag), V03-04 (Shift+Arrow), V03-05 (Ctrl+A)
3. **Batch C — Operations:** V03-06 (Ctrl+C), V03-07 (Ctrl+D fix), V03-08 (Delete)

Batches B and C can run in parallel once A lands.

## Human Decisions Needed

**Zero.** Every gap has a clear recommended direction grounded in the spec text.

## Stage 01b Status After V03

- ✅ **V03 complete** — 8 confirmed gaps, promoted from "verification sub-task" to "implementation work for Stage 03"
- ⏳ V01, V02, V04, V05, V06, V07, V08, V09, V10 — remaining 9 verification sub-tasks from the Stage 01 re-audit

Updated guidance for the next cron fire: **V07 (keyboard and accessibility)** is the next highest-probability real-gap sub-task. The arrow-key handling in `.Editing.cs:186-205` is partial, the Ctrl key handlers are incomplete (per V03 findings), and role/ARIA coverage is unverified.
