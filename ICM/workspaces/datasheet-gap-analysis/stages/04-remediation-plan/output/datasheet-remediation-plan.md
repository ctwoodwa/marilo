# DataSheet Remediation Plan -- Stage 04

**Worker:** `w-datasheet-gap-analysis`
**Session:** `marilo-grid-pipeline-2026-04-11-1200`
**Stage:** `04-remediation-plan` (checkpoint -- STOP before implementation)
**Date:** 2026-04-12
**Component:** MariloDataSheet
**Input:** `stages/03-resolution-design/output/datasheet-resolution-designs.md` (17 designs, 29 records)

---

## Scope

Converts 17 resolution designs (29 records) into atomic implementation tasks grouped by dispatch wave. V03 (7 sub-tasks) is packaged directly from the S03 decomposition.

**Phase D is SKIPPED** -- 10 records blocked on UD-01 (`IDataSheetTheme` contract, orchestrator-only).

**Effort scale:** XS (<30 min), S (30-90 min), M (90 min-3h), L (3h+).

---

## Wave 1 -- Phase A: Unblocked, No Dependencies (8 lanes, 22 records)

All Wave 1 tasks have zero external dependencies. They are parallel-eligible (disjoint file ownership).

---

### TASK-DS-001: Workspace Coverage Audit

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-001` |
| **Source design** | RD-WS01 |
| **Records** | WS-01 |
| **Phase** | A |
| **Wave** | 1 |
| **Effort** | XS |
| **Description** | Populate `_config/coverage-summary.md` with a per-parameter, per-event, and per-keyboard-shortcut test-coverage table. Cross-reference `MariloDataSheet.razor.cs` parameters, `MariloDataSheet.Editing.cs` HandleKeyDown branches, existing bUnit tests, and demo files. Mark each cell Yes/No/Partial with gap-ID references. |
| **Files owned** | `ICM/workspaces/datasheet-gap-analysis/_config/coverage-summary.md` |
| **Acceptance criteria** | File exists. Every public parameter, event, and keyboard shortcut from MariloDataSheet source has a row. Each cell has Yes/No/Partial with notes. |
| **Build verification** | N/A (documentation artifact, no compilation) |

---

### TASK-DS-002: Grid Root tabindex

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-002` |
| **Source design** | RD-SA01 |
| **Records** | SA-01 |
| **Phase** | A |
| **Wave** | 1 |
| **Effort** | XS |
| **Description** | Add `tabindex="0"` to the grid root `<div>` in `MariloDataSheet.razor` (after `class`, before `style`). Matches spec `keyboard-and-accessibility.md:74`. Add bUnit test asserting root `<div role="grid">` has `tabindex="0"`. |
| **Files owned** | `src/Marilo.Components/DataGrid/MariloDataSheet.razor`, bUnit test file for MariloDataSheet |
| **Acceptance criteria** | Grid root `<div role="grid">` contains `tabindex="0"`. bUnit test passes: `cut.Find("[role='grid']").GetAttribute("tabindex").Should().Be("0")`. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. `dotnet test` scoped to DataSheet -- 0 failures. |

---

### TASK-DS-003: Virtualization Threshold Spec + Demo

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-003` |
| **Source design** | RD-UD02-EU01 |
| **Records** | UD-02, EU-01 |
| **Phase** | A |
| **Wave** | 1 |
| **Effort** | S |
| **Description** | (1) Add threshold note to `docs/component-specs/datasheet/virtualization-and-performance.md` stating 10k rows supported with `EnableVirtualization=true` and WASM demo capped at 5k. (2) Create `Virtualization.razor` demo (or extend BulkOperations scenario E) with row-count toggle (100/1k/5k), `EnableVirtualization="true"` for 1k/5k variants. No 10k demo option. Auto-closes VP-datasheet-D03 deferral. |
| **Files owned** | `docs/component-specs/datasheet/virtualization-and-performance.md`, demo page (`Virtualization.razor` or `BulkOperations.razor`) |
| **Acceptance criteria** | Spec contains verbatim threshold text. Demo builds, renders row-count toggle with 100/1k/5k. No 10k option exists. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. Demo page compiles. |

---

### TASK-DS-004: Paste-during-save Guard

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-004` |
| **Source design** | RD-SA08-EU03 |
| **Records** | SA-08, EU-03 |
| **Phase** | A |
| **Wave** | 1 |
| **Effort** | S |
| **Description** | (1) Add `if (IsSaving) return;` early-return guard to `PasteFromClipboard` in `MariloDataSheet.Editing.cs` (~line 428), after the existing `AllowBulkPaste` guard. (2) bUnit test: set `IsSaving = true`, invoke `PasteFromClipboard("foo\tbar")`, assert no cells changed. (3) Demo scenario: button triggers `IsSaving = true` on a timer, user attempts Ctrl+V during save window, visual feedback via `_ariaAnnouncement`. |
| **Files owned** | `src/Marilo.Components/DataGrid/MariloDataSheet.Editing.cs`, bUnit test file, demo page |
| **Acceptance criteria** | Paste is a no-op when `IsSaving = true`. bUnit test confirms dirty count unchanged during save. Demo shows blocked-paste feedback. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. `dotnet test` scoped to DataSheet -- 0 failures. |

---

### TASK-DS-005: Missing aria-live Announcements

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-005` |
| **Source design** | RD-SA13-EU05 |
| **Records** | SA-13, EU-05 |
| **Phase** | A |
| **Wave** | 1 |
| **Effort** | S-M |
| **Description** | Add 3 missing `aria-live` announcements to `SaveAllAsync` in `MariloDataSheet.Data.cs`: (1) "Saving changes." after `_isSaving = true`, (2) "Save failed. {N} validation errors." when validation blocks save (replaces current generic wording), (3) "Save failed. An error occurred." in catch block. Add bUnit tests for each announcement path. Add EU-05 demo: `OnSaveAll` throws on first call, succeeds on second; visible `<pre>` shows aria-live text. |
| **Files owned** | `src/Marilo.Components/DataGrid/MariloDataSheet.Data.cs`, bUnit test file, demo page |
| **Acceptance criteria** | All 3 announcement paths emit correct text. bUnit tests cover saving-start, validation-fail (with count), and exception-fail announcements. Demo shows retry flow. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. `dotnet test` scoped to DataSheet -- 0 failures. |

---

### TASK-DS-006: AddRow ActivateCell

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-006` |
| **Source design** | RD-SA03 |
| **Records** | SA-03 |
| **Phase** | A |
| **Wave** | 1 |
| **Effort** | XS |
| **Description** | Add `ActivateCell(newItem, firstEditableCol.Field)` at the end of `AddRowAsync` in `MariloDataSheet.razor.cs` (~line 240+), after the DirtyFields seeding loop. Finds the first editable, non-computed column. Per spec `bulk-operations-and-saveall.md:119`. bUnit test: invoke `AddRowAsync`, assert active cell is on the new row's first editable column. |
| **Files owned** | `src/Marilo.Components/DataGrid/MariloDataSheet.razor.cs`, bUnit test file |
| **Acceptance criteria** | After `AddRowAsync`, `_activeCellRow` equals the new item and `_activeCellField` equals the first editable column's Field. bUnit test passes. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. `dotnet test` scoped to DataSheet -- 0 failures. |

---

### TASK-DS-007: Reset Clears Undo Buffer

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-007` |
| **Source design** | RD-SA04 |
| **Records** | SA-04 |
| **Phase** | A |
| **Wave** | 1 |
| **Effort** | XS |
| **Description** | Add `_undoBuffer.Clear();` to `ResetAsync` in `MariloDataSheet.Data.cs` (~line 517), after `_dirtyRows.Clear()` and before `ClearActiveCell()`. Per spec `bulk-operations-and-saveall.md:162`. bUnit test: edit a cell (populates undo buffer), call `ResetAsync`, invoke Ctrl+Z, assert cell is NOT reverted (undo buffer was cleared). |
| **Files owned** | `src/Marilo.Components/DataGrid/MariloDataSheet.Data.cs`, bUnit test file |
| **Acceptance criteria** | Ctrl+Z after `ResetAsync` is a no-op (undo buffer cleared). bUnit test passes. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. `dotnet test` scoped to DataSheet -- 0 failures. |

---

### TASK-DS-008: Copy-Paste Round-Trip Demo

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-008` |
| **Source design** | RD-EU02 |
| **Records** | EU-02 |
| **Phase** | A |
| **Wave** | 1 |
| **Effort** | XS |
| **Description** | Add copy-paste round-trip demo scenario: grid with formatted columns (currency, date), user copies cells (Ctrl+C), pastes back (Ctrl+V), demo verifies `data-raw-value` contract is preserved (raw value survives round-trip, not formatted display string). Include visual indicator: "Raw value preserved through round-trip". |
| **Files owned** | Demo page (BulkOperations or Clipboard demo) |
| **Acceptance criteria** | Demo builds, renders, and exercises the Format + data-raw-value round-trip path. Visual indicator confirms raw value preservation. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. Demo page compiles. |

---

## Wave 2 -- Phase B: Pre-Approved Escalations (3 lanes, 3 records)

All 3 items were escalation-gated in S02; the orchestrator pre-approved directions in tick 12. Each is now unblocked.

---

### TASK-DS-009: AddRow Append vs. Prepend (Spec Fix)

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-009` |
| **Source design** | RD-SA02 |
| **Records** | SA-02 |
| **Phase** | B |
| **Wave** | 2 |
| **Effort** | XS |
| **Description** | Spec fix (orchestrator pre-approved). Change `bulk-operations-and-saveall.md:117` from "Appends the row to the end of the data collection" to "Prepends the row at the top of the data collection (insert at index 0). New rows appear at the top of the sheet, following the common spreadsheet convention for data entry." Source behavior (`Insert(0, newItem)`) is correct. |
| **Files owned** | `docs/component-specs/datasheet/bulk-operations-and-saveall.md` |
| **Acceptance criteria** | Spec line 117 matches the new wording. No source changes. |
| **Build verification** | N/A (spec-only change). |

---

### TASK-DS-010: Saving->Saved Transition (Spec Fix)

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-010` |
| **Source design** | RD-SA05 |
| **Records** | SA-05 |
| **Phase** | B |
| **Wave** | 2 |
| **Effort** | XS |
| **Description** | Spec fix (orchestrator pre-approved). Update cell-state transition table at `bulk-operations-and-saveall.md:104-107`: replace "IsSaving set to false" with description of the component-driven `Task.Delay(_savedStateDurationMs)` transition. Add clarifying note that `IsSaving` controls toolbar state while cell-level transitions are component-managed. |
| **Files owned** | `docs/component-specs/datasheet/bulk-operations-and-saveall.md` |
| **Acceptance criteria** | Transition table wording matches actual `SaveAllAsync` behavior. Clarifying note present. No source changes. |
| **Build verification** | N/A (spec-only change). |

---

### TASK-DS-011: Double-Click Edit Entry (Source Fix)

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-011` |
| **Source design** | RD-SA09 |
| **Records** | SA-09 |
| **Phase** | B |
| **Wave** | 2 |
| **Effort** | S |
| **Description** | Source fix (orchestrator pre-approved). (1) Add `ondblclick` handler in `MariloDataSheet.Rendering.cs` (~line 122-123) alongside existing `onclick`. (2) Add `OnCellDoubleClick` method in `MariloDataSheet.Editing.cs`: for editable non-checkbox columns, call `EnterEditMode(row, field)` directly. Computed/non-editable cells activate only. Checkbox columns are a no-op (double-toggle is confusing). (3) bUnit test: double-click enters edit mode; single-click still activates first. |
| **Files owned** | `src/Marilo.Components/DataGrid/MariloDataSheet.Rendering.cs`, `src/Marilo.Components/DataGrid/MariloDataSheet.Editing.cs`, bUnit test file |
| **Acceptance criteria** | Double-click on editable cell enters edit mode directly. Single-click path unchanged. Checkbox columns unaffected. bUnit test passes. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. `dotnet test` scoped to DataSheet -- 0 failures. |

---

## Wave 3 -- Phase C: Complex / Large (2 lanes, 3 records)

Wave 3 contains the critical-path range selection model (V03, decomposed into 7 sub-tasks) and the frozen column source half (VP-07). These two lanes are parallel-eligible (disjoint files).

---

### TASK-DS-012: Selection State Model (V03.1)

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-012` |
| **Source design** | RD-V03, Sub-task V03.1 |
| **Records** | V03 (partial) |
| **Phase** | C |
| **Wave** | 3a |
| **Effort** | S |
| **Description** | Create new file `src/Marilo.Components/DataGrid/DataSheetSelectionState.cs`. Internal generic class `DataSheetSelectionState<TItem>` with: AnchorRow/AnchorField, ExtentRow/ExtentField, `HasRange` property, `ClearRange()` method, `SetSingleCell(row, field)` method. This is the foundation for all subsequent V03 sub-tasks. |
| **Files owned** | `src/Marilo.Components/DataGrid/DataSheetSelectionState.cs` |
| **Acceptance criteria** | Class compiles. `HasRange` returns false when anchor equals extent. `SetSingleCell` sets both anchor and extent. `ClearRange` nulls extent. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. |

---

### TASK-DS-013: Integrate Selection State into DataSheet (V03.2)

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-013` |
| **Source design** | RD-V03, Sub-task V03.2 |
| **Records** | V03 (partial) |
| **Phase** | C |
| **Wave** | 3a (after TASK-DS-012) |
| **Effort** | S |
| **Description** | In `MariloDataSheet.Editing.cs`: (1) Add field `internal DataSheetSelectionState<TItem> _selection = new();`. (2) Modify `ActivateCell` to call `_selection.SetSingleCell(row, field)`. (3) Modify `OnCellClick` to check Shift modifier: plain click calls `SetSingleCell`, Shift+Click extends range by setting `_selection.ExtentRow`/`ExtentField`. (4) Add `GetSelectedCells()` helper returning `List<(TItem Row, string Field)>` for all cells in the rectangular anchor-extent region. |
| **Files owned** | `src/Marilo.Components/DataGrid/MariloDataSheet.Editing.cs` |
| **Acceptance criteria** | `_selection` field exists. `ActivateCell` sets single-cell selection. `GetSelectedCells()` returns correct rectangular region. Shift+Click extends range. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. |

---

### TASK-DS-014: Keyboard Range Extension (V03.3 + V07.4)

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-014` |
| **Source design** | RD-V03, Sub-task V03.3 |
| **Records** | V03 (partial), V07.4 |
| **Phase** | C |
| **Wave** | 3a (after TASK-DS-013) |
| **Effort** | S |
| **Description** | In `HandleKeyDown`: (1) Shift+Arrow extends `_selection.ExtentRow`/`ExtentField` instead of moving active cell. (2) Ctrl+A (V07.4) selects all: anchor = first row/first column, extent = last row/last column. (3) Plain Arrow calls `_selection.ClearRange()` then moves active cell. |
| **Files owned** | `src/Marilo.Components/DataGrid/MariloDataSheet.Editing.cs` |
| **Acceptance criteria** | Shift+Arrow extends range. Ctrl+A selects all cells. Plain arrow clears range and moves. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. |

---

### TASK-DS-015: Range-Scoped Operations (V03.4)

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-015` |
| **Source design** | RD-V03, Sub-task V03.4 |
| **Records** | V03 (partial) |
| **Phase** | C |
| **Wave** | 3b (after TASK-DS-014) |
| **Effort** | S |
| **Description** | Update existing operations to use `GetSelectedCells()`: (1) Fill Down (Ctrl+D) iterates selected cells in active column, fills from anchor row value. (2) Delete iterates selected cells, clears each editable non-computed cell. (3) Copy (Ctrl+C) builds TSV from selected cells (pass bounds to JS interop). Paste (Ctrl+V) unchanged (already anchors at active cell). |
| **Files owned** | `src/Marilo.Components/DataGrid/MariloDataSheet.Editing.cs` |
| **Acceptance criteria** | Fill Down, Delete, and Copy operate on rectangular selection range. Single-cell operations unchanged when no range. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. |

---

### TASK-DS-016: Selection Highlight Rendering (V03.5)

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-016` |
| **Source design** | RD-V03, Sub-task V03.5 |
| **Records** | V03 (partial) |
| **Phase** | C |
| **Wave** | 3b (after TASK-DS-013) |
| **Effort** | XS |
| **Description** | In `MariloDataSheet.Rendering.cs`: (1) Add `IsInSelectedRange(row, field)` helper. (2) Add CSS class `mar-datasheet__cell--selected` when cell is in range. Provider SCSS is gated on UD-01 (Phase D), but the class emission must land now. |
| **Files owned** | `src/Marilo.Components/DataGrid/MariloDataSheet.Rendering.cs` |
| **Acceptance criteria** | Cells within the anchor-extent range receive `mar-datasheet__cell--selected` class. Cells outside do not. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. |

---

### TASK-DS-017: Shift+Click in Rendering (V03.6)

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-017` |
| **Source design** | RD-V03, Sub-task V03.6 |
| **Records** | V03 (partial) |
| **Phase** | C |
| **Wave** | 3a (concurrent with TASK-DS-013) |
| **Effort** | XS |
| **Description** | In `MariloDataSheet.Rendering.cs`: change `onclick` handler to pass `MouseEventArgs` so `ShiftKey` is available. Update `OnCellClick` signature to `OnCellClick(TItem row, string field, bool shiftKey)`. This enables TASK-DS-013's Shift+Click logic. |
| **Files owned** | `src/Marilo.Components/DataGrid/MariloDataSheet.Rendering.cs` |
| **Acceptance criteria** | `onclick` handler passes `MouseEventArgs`. `OnCellClick` receives `shiftKey` boolean. Existing single-click behavior unchanged when `shiftKey=false`. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. |

---

### TASK-DS-018: Range Selection Tests (V03.7)

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-018` |
| **Source design** | RD-V03, Sub-task V03.7 |
| **Records** | V03 (partial) |
| **Phase** | C |
| **Wave** | 3c (after TASK-DS-015 and TASK-DS-016) |
| **Effort** | S-M |
| **Description** | Full bUnit test suite for V03: (1) Shift+Click creates range (anchor stays, extent moves). (2) Shift+Arrow extends range. (3) Ctrl+A selects all. (4) Plain click clears range. (5) Delete on range clears all editable cells. (6) Fill Down on range fills column within range. Spec `selection-and-ranges.md` already describes target behavior -- no spec changes. |
| **Files owned** | bUnit test files for MariloDataSheet |
| **Acceptance criteria** | All 6 test scenarios pass. Range selection, keyboard extension, and range-scoped operations verified. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. `dotnet test` scoped to DataSheet -- 0 failures. |

---

### TASK-DS-019: Frozen Column (Source Half)

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-019` |
| **Source design** | RD-VP07-SRC |
| **Records** | VP-datasheet-07 (source half) |
| **Phase** | C |
| **Wave** | 3 (parallel with V03 tasks -- disjoint files) |
| **Effort** | S |
| **Description** | (1) Add `[Parameter] public bool Frozen { get; set; }` to `MariloDataSheetColumn.razor` after `Width` (~line 40). (2) In `MariloDataSheet.Rendering.cs`: emit `mar-datasheet__header-cell--frozen` and `mar-datasheet__cell--frozen` CSS classes for frozen columns. Emit inline `position:sticky; left:{cumulativeLeft}px;` with left offset calculated by summing preceding frozen column widths. Add validation warning if non-frozen column precedes a frozen one. (3) Verify spec `columns-and-schema.md` parameter table includes `Frozen`. (4) bUnit tests: frozen column emits correct classes and inline sticky styles. |
| **Files owned** | `src/Marilo.Components/DataGrid/MariloDataSheetColumn.razor`, `src/Marilo.Components/DataGrid/MariloDataSheet.Rendering.cs`, `docs/component-specs/datasheet/columns-and-schema.md`, bUnit test file |
| **Acceptance criteria** | `Frozen` parameter exists on column. Frozen columns emit sticky classes and inline styles. bUnit tests pass. SCSS styling deferred to Phase D. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. `dotnet test` scoped to DataSheet -- 0 failures. |

---

## Wave 4 -- Phase E: Low-Priority Cleanup (4 lanes, 10 records)

All Wave 4 tasks are spec/demo-only changes with zero source risk. Parallel-eligible.

---

### TASK-DS-020: Batch Spec Wording Fixes

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-020` |
| **Source design** | RD-SPEC-FIXES |
| **Records** | SA-07, SA-11, SA-12, SA-14, SA-15 |
| **Phase** | E |
| **Wave** | 4 |
| **Effort** | S |
| **Description** | 5 spec wording corrections where source behavior is intentional: (1) SA-07: `bulk-paste-and-clipboard.md:91` -- change "current culture" to "InvariantCulture". (2) SA-11: `editing-and-validation.md:139` -- rewrite to describe required-check short-circuit. (3) SA-12: `editing-and-validation.md:193` -- drop "invalid-only rows" clause. (4) SA-14: `columns-and-schema.md:118` -- drop "or zero" clause. (5) SA-15: `columns-and-schema.md:231` -- drop `default(DateTime)` rejection clause. |
| **Files owned** | `docs/component-specs/datasheet/bulk-paste-and-clipboard.md`, `docs/component-specs/datasheet/editing-and-validation.md`, `docs/component-specs/datasheet/columns-and-schema.md` |
| **Acceptance criteria** | All 5 spec lines updated to match actual source behavior. No source changes. |
| **Build verification** | N/A (spec-only changes). |

---

### TASK-DS-021: Demo-Only Additions (EU-04, EU-08)

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-021` |
| **Source design** | RD-DEMO-P2 |
| **Records** | EU-04, EU-08 |
| **Phase** | E |
| **Wave** | 4 |
| **Effort** | XS |
| **Description** | (1) EU-04: Add Delete-key scenario to `Keyboard-and-Accessibility.razor` -- grid with pre-populated data, user presses Delete to clear cell value to type default. Single-cell only (multi-cell Delete requires V03). (2) EU-08: Add `CellTemplate` scenario -- column with custom rendering (colored badge/icon), demonstrating `DataSheetCellContext<TItem>` properties (`Item`, `Field`, `Value`, `IsEditing`, `IsDirty`, `ValidationError`) and coexistence with editing mode. |
| **Files owned** | Demo pages (Keyboard-and-Accessibility.razor, CellTemplate demo) |
| **Acceptance criteria** | Both demos build, render, and are interactive. EU-04 shows single-cell Delete. EU-08 shows CellTemplate with context properties. |
| **Build verification** | `dotnet build Marilo.slnx` exit 0. Demo pages compile. |

---

### TASK-DS-022: Fill-Down Editable Filter (Partial)

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-022` |
| **Source design** | RD-SA06 |
| **Records** | SA-06 |
| **Phase** | E |
| **Wave** | 4 |
| **Effort** | XS |
| **Description** | Partial fix (full fix deferred to after V03). (1) Soften spec wording at `selection-and-ranges.md:88` to reflect current row-level Fill Down behavior, noting rectangular range scoping will arrive with V03. (2) Soften any demo wording in `Keyboard-and-Accessibility.razor` that implies range-aware fill-down. No source changes. |
| **Files owned** | `docs/component-specs/datasheet/selection-and-ranges.md`, demo page |
| **Acceptance criteria** | Spec and demo wording accurately describe current row-level Fill Down. No source changes. |
| **Build verification** | N/A (spec/demo wording only). |

---

### TASK-DS-023: P3 Polish

| Field | Value |
|---|---|
| **Task ID** | `TASK-DS-023` |
| **Source design** | RD-P3-POLISH |
| **Records** | SRC-01, NM-01 |
| **Phase** | E |
| **Wave** | 4 |
| **Effort** | XS |
| **Description** | (1) SRC-01: `virtualization-and-performance.md:77` -- change "viewport-calculated number of skeleton rows" to "fixed number of skeleton rows (currently 5)". (2) NM-01: `overview.md:122-123` -- clarify that Class/Style are inherited via `AdditionalAttributes` from `MariloComponentBase`, not direct `[Parameter]` properties. |
| **Files owned** | `docs/component-specs/datasheet/virtualization-and-performance.md`, `docs/component-specs/datasheet/overview.md` |
| **Acceptance criteria** | Both spec lines updated. Wording matches actual source. |
| **Build verification** | N/A (spec-only changes). |

---

## Wave Dependency Map

```
Wave 1 (Phase A) -- 8 tasks, all parallel
  TASK-DS-001  WS-01          gap-plan         XS
  TASK-DS-002  SA-01          source+tests     XS
  TASK-DS-003  UD-02+EU-01    spec+demo        S
  TASK-DS-004  SA-08+EU-03    source+tests+demo S
  TASK-DS-005  SA-13+EU-05    source+spec+demo+tests S-M
  TASK-DS-006  SA-03          source+tests     XS
  TASK-DS-007  SA-04          source+tests     XS
  TASK-DS-008  EU-02          demo             XS

Wave 2 (Phase B) -- 3 tasks, all parallel
  TASK-DS-009  SA-02          spec             XS
  TASK-DS-010  SA-05          spec             XS
  TASK-DS-011  SA-09          source+tests     S

Wave 3 (Phase C) -- 8 tasks, dependency chain within V03

  Sub-wave 3a (parallel-eligible):
    TASK-DS-012  V03.1 State Model         source        S
      |
      v
    TASK-DS-017  V03.6 Shift+Click Render  source        XS   (parallel with 012)
      |
      v
    TASK-DS-013  V03.2 Integrate State     source        S    (needs 012 + 017)
      |
      v
    TASK-DS-014  V03.3 Keyboard Extension  source        S    (needs 013)

  Sub-wave 3b (needs 3a):
    TASK-DS-015  V03.4 Range Operations    source        S    (needs 014)
    TASK-DS-016  V03.5 Selection Highlight  source        XS   (needs 013)

  Sub-wave 3c (needs 3b):
    TASK-DS-018  V03.7 Tests               tests         S-M  (needs 015 + 016)

  Independent (parallel with all V03 sub-waves):
    TASK-DS-019  VP-07 Frozen Column       source+tests  S

Wave 4 (Phase E) -- 4 tasks, all parallel
  TASK-DS-020  SA-07+11+12+14+15  spec            S
  TASK-DS-021  EU-04+EU-08        demo             XS
  TASK-DS-022  SA-06              spec+demo        XS
  TASK-DS-023  SRC-01+NM-01       spec             XS
```

### Wave Boundaries

- **Wave 1 -> Wave 2:** No hard dependency. Wave 2 can start as soon as orchestrator confirms pre-approvals (already done tick 12). Parallel with Wave 1 tail if file ownership is disjoint.
- **Wave 2 -> Wave 3:** No hard dependency. Wave 3 can start independently. TASK-DS-011 (SA-09 double-click) touches `Rendering.cs` and `Editing.cs` -- if Wave 3 V03 tasks also touch those files, schedule TASK-DS-011 before V03 sub-wave 3a to avoid file conflicts.
- **Wave 3 -> Wave 4:** No hard dependency. Wave 4 is spec/demo cleanup and can run anytime files are available.
- **Wave 4 -> Phase D:** Phase D (skipped, UD-01 blocked) is independent of Waves 1-4. It activates when the orchestrator completes UD-01.

---

## Coverage Verification

### All 29 designed records mapped to tasks

| Task ID | Records | Count |
|---|---|---:|
| TASK-DS-001 | WS-01 | 1 |
| TASK-DS-002 | SA-01 | 1 |
| TASK-DS-003 | UD-02, EU-01 | 2 |
| TASK-DS-004 | SA-08, EU-03 | 2 |
| TASK-DS-005 | SA-13, EU-05 | 2 |
| TASK-DS-006 | SA-03 | 1 |
| TASK-DS-007 | SA-04 | 1 |
| TASK-DS-008 | EU-02 | 1 |
| TASK-DS-009 | SA-02 | 1 |
| TASK-DS-010 | SA-05 | 1 |
| TASK-DS-011 | SA-09 | 1 |
| TASK-DS-012 | V03 (partial) | -- |
| TASK-DS-013 | V03 (partial) | -- |
| TASK-DS-014 | V03 (partial), V07.4 | -- |
| TASK-DS-015 | V03 (partial) | -- |
| TASK-DS-016 | V03 (partial) | -- |
| TASK-DS-017 | V03 (partial) | -- |
| TASK-DS-018 | V03 (partial) | -- |
| TASK-DS-019 | VP-07 (source) | 1 |
| TASK-DS-020 | SA-07, SA-11, SA-12, SA-14, SA-15 | 5 |
| TASK-DS-021 | EU-04, EU-08 | 2 |
| TASK-DS-022 | SA-06 | 1 |
| TASK-DS-023 | SRC-01, NM-01 | 2 |
| **V03+V07.4 combined** | V03, V07.4 | **2** |
| **Total** | | **29** |

### Skipped (Phase D, UD-01 blocked)

10 records: VP-datasheet-01 through VP-datasheet-12 (excluding VP-07 source half which is TASK-DS-019).

### Retired

SA-10: Duplicate of SA-07. Merged during S02 dedup.

### Full accounting

29 designed + 10 Phase D skipped = 39 - 1 retired = 38 actionable. All accounted for.

---

## Task Summary

| Wave | Phase | Tasks | Records | Effort range |
|---|---|---:|---:|---|
| 1 | A | 8 | 12 (unique, excl. shared V03) | XS -- S-M |
| 2 | B | 3 | 3 | XS -- S |
| 3 | C | 8 | 3 (V03+V07.4+VP-07src) | XS -- S-M |
| 4 | E | 4 | 10 | XS -- S |
| **Total** | | **23** | **29** (covers 28 unique records, V03 spans 7 tasks) | |

---

## Checkpoint

**STOP -- end of Stage 04 remediation-plan.**

- 23 atomic tasks defined across 4 waves (Phases A, B, C, E).
- V03 decomposed into 7 ordered sub-tasks (TASK-DS-012 through TASK-DS-018) with internal dependency chain.
- Phase D (10 VP SCSS records) skipped per UD-01 block.
- Every task has: ID, description, files_owned, acceptance criteria, build verification, wave, effort.
- Wave dependency map shows parallelism opportunities and file-conflict avoidance.
- Full coverage verification: 29 designed records mapped, 10 Phase D skipped, 1 retired = 38 actionable total.
