# DataSheet Spec Verification — Stage 01b Results

**Audit date:** 2026-04-10
**Audit scope:** 10 verification sub-tasks GAP-DATASHEET-V01 through -V10 from `datasheet-spec-gaps-2026-04-10.md`
**Method:** Parallel read-only source-vs-spec audits via subagents (two batches of 4 + direct enum check for V09/V10)
**Source baseline:** 1,324 lines across 7 partials in `src/Marilo.Components/DataGrid/MariloDataSheet*`
**Spec baseline:** 9 files in `docs/component-specs/datasheet/`

---

## Headline

Stage 01b verification **surfaced 23 concrete implementation gaps** across 6 of the 10 sub-tasks. V03 (cell range selection) is by far the biggest — an entire feature subsystem is absent. The remaining 5 sub-tasks contain many small, tractable fixes. 4 sub-tasks verified fully clean.

| Sub-task | Area | Verdict | Gap count |
|---|---|---|---|
| V01 | columns-and-schema | MINOR_GAPS | 2 |
| V02 | editing-and-validation | REAL_GAPS | 3 |
| V03 | **selection-and-ranges** | **REAL_GAPS_LARGE** | **10 (cohesive feature)** |
| V04 | bulk-paste-and-clipboard | MINOR_GAPS | 4 |
| V05 | bulk-operations-and-saveall | REAL_GAPS | 5 |
| V06 | virtualization-and-performance | CLEAN | 0 |
| V07 | keyboard-and-accessibility | REAL_GAPS | 9 |
| V08 | theming-and-css-provider | CLEAN | 0 |
| V09 | DataSheetColumnType enum | CLEAN | 0 |
| V10 | CellState enum | CLEAN | 0 |
| **Total** | | | **23 discrete gaps + 1 large feature** |

---

## GAP-DATASHEET-V01 — columns-and-schema — MINOR_GAPS

**Verified present (11):** 6 ColumnType editors (Rendering.cs:140–242), Computed exclusion (Editing.cs:37, 75), Computed exclusion from SaveAll via DirtyFields gating (Data.cs:151–153), Required validation on commit (Data.cs:60–68, 88–105), Format lambda in read mode (Rendering.cs:141, 146), all 9 `MariloDataSheetColumn` parameters (Field, Title, Editable, Required, MinWidth, Width, Format, Options, CellTemplate, Validate).

**Real gaps (2):**

1. **Checkbox `Required=true` does not reject `false`** — `RunColumnValidation` (Data.cs:88–105) only checks null-or-whitespace on the string representation. A checkbox with `Required=true` passes validation when unchecked.
   - **Fix scope:** Add a branch in `RunColumnValidation`: `if (column.ColumnType == Checkbox && column.Required && value is false or null)` → append error.
2. **Number parsing lacks fallback type conversion** — `RenderCellEditor` Number case (Rendering.cs:188) only calls `decimal.TryParse`. The spec (columns-and-schema.md:107) states fallback type conversion for `int`, `double`, `float`, etc. Parse failures silently return `0m`.
   - **Fix scope:** Extract a `ParseNumberValue(string, Type targetType)` helper that dispatches to the target property type via `TypeCode` and returns a tuple `(bool success, object value)`. Call it from `RenderCellEditor` and `ParseCellValue`.

---

## GAP-DATASHEET-V02 — editing-and-validation — REAL_GAPS

**Verified present (14):** Enter edit mode (F2/double-click), commit/cancel (Enter/Tab/Escape), checkbox immediate commit, `CommitCellEdit` atomicity, per-field dirty tracking via `DirtyRowEntry.DirtyFields` HashSet, Required + custom Validate ordering, `OnValidate` can block SaveAll, `ValidateAllAsync` → OnValidate → SaveAll sequencing, cell state Dirty/Invalid/Pristine transitions, `OnRowChanged` firing, `ResetAsync` restores originals, `IsCellEditing`, `DataSheetValidateArgs`/`DataSheetValidationError` shapes, `CellState` enum complete.

**Real gaps (3):**

1. **Field-level dirty state is never cleared on revert** — `CommitCellEdit` (Data.cs:54–58) always adds the field to `DirtyFields`. If a user types something then types the original value back, the field stays marked dirty permanently (cleared only by full `ResetAsync`).
   - **Fix scope:** In `CommitCellEdit`, after writing `newValue`, compare against `entry.Original[field]` using `Equals`. If equal, `entry.DirtyFields.Remove(field)` and if `DirtyFields.Count == 0` also remove the row from `_dirtyRows`. If different, `entry.DirtyFields.Add(field)`.
2. **CellState Saving / Saved transitions never occur** — The enum has the values and the CSS provider has the classes (V08 confirms), but no code ever transitions a cell through them. `GetCellState` (Data.cs:249–262) returns only Pristine/Dirty/Invalid.
   - **Fix scope:** Add a transient per-row `CellState Phase` field. `SaveAllAsync` sets Phase = Saving before firing OnSaveAll, then Phase = Saved on success, with a timed transition back to Pristine after ~1.5s (Task.Delay + StateHasChanged). `GetCellState` checks `Phase` first, then falls through to Dirty/Invalid/Pristine logic.
3. **Per-row validation layer is absent** — Spec discusses row-level validation separately from per-cell. Only column-level `Validate` delegate exists.
   - **Fix scope:** Verify by re-reading the spec whether row-level validation is required. If so, add a `RowValidate` parameter (`Func<TItem, List<DataSheetValidationError>>`) to `MariloDataSheet` and run it in `ValidateAllAsync` after all per-cell validations.

---

## GAP-DATASHEET-V03 — selection-and-ranges — REAL_GAPS_LARGE

**CRITICAL — the single biggest gap on DataSheet.** See dedicated Stage 03 resolution design at `stages/03-resolution-design/output/gap-datasheet-v03-selection-ranges-resolution.md` (written in this same run).

**What the spec requires:** Two-tier selection model — single active cell + rectangular range — driven by Shift+Click, click-drag, Shift+Arrow keys, and Ctrl+A, with a public `DataSheetSelection<TItem>` model and an `OnSelectionChanged` callback. Copy, Paste, Fill Down, and Delete all operate on the range.

**What the source has:** Single active cell (`_activeCellRow`, `_activeCellField`), row-level selection (`HashSet<TItem> _selectedRows` with checkbox + select-all + ToggleRowSelection), and cell operations that target only the active cell or selected rows.

**What's missing:**
1. Shift+Click to extend range from anchor
2. Click-and-drag to create rectangular range (mousedown/mousemove/mouseup plumbing)
3. Shift+Arrow to extend range via keyboard
4. Ctrl+A to select all cells
5. Rectangular range visual highlight (CSS class in `.Rendering.cs` cell loop)
6. Copy-range-as-TSV (current Copy only handles active cell)
7. Fill Down within a range (current Fill Down targets selected rows, not a range rectangle)
8. Delete-clears-range (current Delete clears only active cell)
9. `SelectionChanged` event (no public callback exists)
10. `DataSheetSelection<TItem>` model exposed via `CurrentSelection` property

**Effort estimate:** 3–4 days. Requires Stage 03 resolution design before implementation. **Defer from this loop iteration.**

---

## GAP-DATASHEET-V04 — bulk-paste-and-clipboard — MINOR_GAPS

**Verified present (12):** TSV parsing (Editing.cs:252–255), AllowBulkPaste gating (Editing.cs:246), paste range truncation, read-only/Computed skip (Editing.cs:259), type coercion for Number/Date/Checkbox/Text/Select, post-coercion validation, OnRowChanged per cell, clipboard JS layer (marilo-datasheet.js:81–101), cell error state tracking.

**Real gaps (4):**

1. **Line-ending brittle** — Splits by `\n` only (Editing.cs:252). Windows clipboard data uses `\r\n` and leaves `\r` as trailing characters on cells.
   - **Fix scope:** Normalize `tsvData` with `.Replace("\r\n", "\n").Replace("\r", "\n")` before split.
2. **Type coercion failures silently return defaults** — `ParseCellValue` (Editing.cs:283–293) returns `0m` / `DateTime.MinValue` on parse fail without surfacing an error. Spec requires `CellState.Invalid` with "Invalid number" / "Invalid date" message.
   - **Fix scope:** Change `ParseCellValue` to return `(bool success, object? value, string? error)`. In the paste loop, if `!success`, call `CommitCellEdit` with an explicit validation error matching the spec wording.
3. **Deleted rows not skipped during paste** — Paste loop iterates `_displayRows` with no check for `entry.IsDeleted`. A user can paste into a row they just marked for deletion.
   - **Fix scope:** In the paste loop after `row = _displayRows[startRowIdx + r]`, add `if (_dirtyRows.TryGetValue(GetRowKey(row), out var entry) && entry.IsDeleted) continue;`
4. **Copy ignores `Format` delegate** — JS copies `.textContent` (marilo-datasheet.js:84). Spec says raw value is copied when `Format` exists; formatted display value otherwise.
   - **Fix scope:** Emit the raw value into a `data-raw-value` attribute during cell render, and have the JS copy handler prefer `data-raw-value` over `textContent` when present.

---

## GAP-DATASHEET-V05 — bulk-operations-and-saveall — REAL_GAPS

**Verified present (14):** SaveAll validation flow (Data.cs:142–200), `GetDirtyRows()` shape, `DataSheetSaveArgs`, `OnValidate` + error mapping, `AllowAddRow`/`AllowDeleteRow` gating, `IsSaving` Save button disable, validation blocks SaveAll, `AddRowAsync` uses `Activator.CreateInstance<TItem>()`, row insert at top, `MarkRowDeleted`, `BulkDeleteAsync`, `BulkResetAsync` dict clear, `ResetAsync` restores field values.

**Real gaps (5):**

1. **CellState Saving/Saved transitions never occur** — Same gap as V02 #2. Consolidate under one fix.
2. **Deleted rows not removed post-save** — After `OnSaveAll` succeeds (Data.cs:183–200), `_displayRows` still contains rows marked `IsDeleted`. Spec states they should be removed.
   - **Fix scope:** After OnSaveAll succeeds, iterate `_dirtyRows` and remove any rows where `entry.IsDeleted` from both `_dirtyRows` and `_displayRows`.
3. **`ResetAsync` does not remove newly added rows** — Spec says "Added rows are removed." Current implementation only clears the `_dirtyRows` dict; new rows linger in `_displayRows`.
   - **Fix scope:** Track added rows in a separate `HashSet<TItem> _addedRows`, or flag them via a `bool IsNew` on `DirtyRowEntry`. In `ResetAsync`, remove new rows from `_displayRows` before clearing dirty state.
4. **Row undelete toggle missing** — `MarkRowDeleted` sets `IsDeleted = true` unconditionally (Data.cs:229–245). Clicking the delete button again should toggle it off.
   - **Fix scope:** Change `entry.IsDeleted = true` to `entry.IsDeleted = !entry.IsDeleted`.
5. **`BulkResetAsync` doesn't restore field values** — Only removes entries from `_dirtyRows`. Field values on the `TItem` instances remain whatever the user last typed.
   - **Fix scope:** Before removing the entry, iterate `entry.DirtyFields` and use reflection (`GridReflectionHelper.SetValue`) to write each field back to `entry.Original[field]`.

---

## GAP-DATASHEET-V06 — virtualization-and-performance — CLEAN

All 11 checked features verified present: `EnableVirtualization` parameter, `<Virtualize>` wrapper (razor:138), OverscanCount=5, sticky header outside virtualize region, browser-measured row height, `ScrollToRowAsync` via JS interop on `data-row-key`, non-virtualized fallback, dirty tracking across non-visible rows, edit state independence from visibility.

Two minor notes (not gaps):
- Height is not enforced; auto-sizing allowed. Safe default.
- Skeleton count is hardcoded 5 rather than viewport-calculated. Minor cosmetic divergence; not tracked as a gap.

---

## GAP-DATASHEET-V07 — keyboard-and-accessibility — REAL_GAPS

**Verified present (14 ARIA + 13/18 shortcuts):**
- ARIA: `role="grid"`, `aria-label`, `aria-rowcount`, `aria-colcount`, `aria-busy` (partial), `role="row"`, `role="columnheader"`, `role="gridcell"`, `aria-readonly`, `aria-invalid`, `title` (error), `aria-hidden` (deleted), `aria-live="polite"` region, `role="toolbar"`, `role="status"`.
- Keys: Arrow Up/Down/Left/Right, Tab, Shift+Tab (column-level), Enter (commit-only), F2, Escape, Delete, Ctrl+S, Ctrl+Z, Ctrl+C (JS), Ctrl+V (JS), Ctrl+D.

**Real gaps (9):**

1. **Enter key does not enter edit mode from inactive cell** — Only commits in edit mode. Spec says Enter = F2 behavior on active cell.
   - **Fix scope:** Add `if (key == "Enter" && !_isEditMode) { EnterEditMode(...); return; }` branch in `HandleKeyDown` before line 210.
2. **Printable characters do not trigger edit mode** — Spec: typing a char enters edit mode and replaces value.
   - **Fix scope:** JS side: detect `key.length == 1` and pass as `printableChar` param to `HandleKeyDown`. C# side: when received on inactive cell with Text/Number type, EnterEditMode and pre-populate with that char.
3. **Space bar does not toggle checkbox** — Spec: Space toggles checkbox without entering edit mode.
   - **Fix scope:** Add `if (key == " " && column.ColumnType == Checkbox) { Toggle(); CommitCellEdit(); return; }` branch.
4. **Ctrl+A not wired** — Spec: select all cells in DataSheet.
   - **Fix scope:** Covered by V03 resolution design (part of the range-selection feature).
5. **`aria-rowindex` not emitted** — Spec requires it on every row (header + data).
   - **Fix scope:** Add `aria-rowindex="@(rowIdx + 1)"` to `<tr>` in `.Rendering.cs` (data rows) and the header row in `.razor`.
6. **`aria-colindex` not emitted** — Same for cells.
   - **Fix scope:** Track `colIdx` in the column loop and emit `aria-colindex="@(colIdx + 1)"` on each `<td>` and `<th>`.
7. **`aria-describedby` missing for error messages** — Only `title` attribute is set; screen readers need a DOM link to an error message element.
   - **Fix scope:** For each invalid cell, render a visually-hidden `<span id="err-{rowKey}-{field}">{error}</span>` and set `aria-describedby="err-{rowKey}-{field}"` on the cell.
8. **`aria-busy` ignores `IsSaving`** — Currently only `IsLoading ? "true" : null` (razor:13). Spec requires both.
   - **Fix scope:** Change to `aria-busy="@((IsLoading || IsSaving) ? "true" : null)"`.
9. **`aria-live` region present but never populated** — `_ariaAnnouncement` string exists but is never set. Spec says announce dirty count, save status, validation errors.
   - **Fix scope:** Populate `_ariaAnnouncement` in: (a) `OnRowChanged` completion ("X rows dirty"), (b) validation failure ("N errors found"), (c) `SaveAllAsync` completion ("Saved X rows" / "Save failed").

Additionally **Tab/Shift+Tab row wrapping** is incomplete — works only within a row's columns. Fix scope: when at last column, advance to first column of next row (and vice versa for Shift+Tab).

---

## GAP-DATASHEET-V08 — theming-and-css-provider — CLEAN

All 7 CSS provider methods present across FluentUI, Material, Bootstrap with signature-matching implementations. All consumer sites correct (root grid, cells, header, rows, toolbar, bulk bar, save footer). Minor Bootstrap deviations documented as deliberate (uses native Bootstrap state classes rather than BEM modifiers, omits `--saving`/`--saved` cell states, adds a `--sortable` modifier despite spec saying to ignore the flag). **Not tracked as gaps.**

---

## GAP-DATASHEET-V09 — DataSheetColumnType enum — CLEAN

All 6 values present, in spec-documented order, with matching XML docs: `Text`, `Number`, `Date`, `Select`, `Checkbox`, `Computed`. Verified at `src/Marilo.Core/Enums/DataSheetColumnType.cs:6–25`.

---

## GAP-DATASHEET-V10 — CellState enum — CLEAN

All 5 values present with matching XML docs: `Pristine`, `Dirty`, `Invalid`, `Saving`, `Saved`. Verified at `src/Marilo.Core/Enums/CellState.cs:6–22`.

---

## Consolidated Gap Roll-Up

### Defer to Stage 03 resolution design (large)

- **V03** cell range selection (entire feature subsystem) → `stages/03-resolution-design/output/gap-datasheet-v03-selection-ranges-resolution.md`

### Tractable fixes — suggested batching for future implementation loops

**Batch F1: Validation correctness (V01 + V02 field-level)**
- V01.1 Checkbox Required enforcement
- V01.2 Number fallback parsing
- V02.1 Field-level dirty state cleanup on revert

**Batch F2: Save/Reset lifecycle (V02 + V05)**
- V02.2 / V05.1 CellState Saving/Saved transitions (shared fix)
- V05.2 Deleted rows removed post-save
- V05.3 ResetAsync removes new rows
- V05.4 Undelete toggle
- V05.5 BulkResetAsync restores values

**Batch F3: Paste hardening (V04)**
- V04.1 `\r\n` normalization
- V04.2 Parse error surfacing to CellState.Invalid
- V04.3 Deleted row skip
- V04.4 Copy honors Format via data-raw-value

**Batch F4: Accessibility (V07)**
- V07.1 Enter key → edit mode
- V07.2 Printable char → edit mode (requires JS change)
- V07.3 Space → checkbox toggle
- V07.5 `aria-rowindex`
- V07.6 `aria-colindex`
- V07.7 `aria-describedby` for errors
- V07.8 `aria-busy` includes `IsSaving`
- V07.9 `aria-live` population
- V07 Tab row wrapping

**V02.3** (per-row validation layer) — spec re-read needed before scoping.

### Tests

Each batch should ship with bUnit tests. Rough counts:
- Batch F1: ~5 tests (checkbox Required, Number parse fallback, revert-clears-dirty)
- Batch F2: ~8 tests (Saving/Saved transitions, delete removal, reset-new-row, undelete, BulkReset restore)
- Batch F3: ~5 tests (`\r\n`, parse error, deleted skip, Format copy)
- Batch F4: ~10 tests (keyboard shortcuts, ARIA attributes)

---

## Human Decisions

**Zero blocking decisions.** All 23 gaps + V03 are mechanical or design-problem-solving work that can proceed without human intervention. V03 alone benefits from human review of the resolution design doc (scope, API shape) but can also proceed on reasonable defaults if deferred.

## Stage Handoff

- Stage 01b status: ✅ **Complete**. All 10 verification sub-tasks executed and documented.
- Next stages:
  - **Stage 03** resolution design for V03 (written this run)
  - **Stage 05** implementation batches F1–F4 (deferred to future loop iterations)
  - **Stage 06** validation after each batch lands
