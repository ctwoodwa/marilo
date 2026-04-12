# DataSheet Resolution Designs -- Stage 03

**Worker:** `w-datasheet-gap-analysis`
**Session:** `marilo-grid-pipeline-2026-04-11-1200`
**Stage:** `03-resolution-design` (checkpoint -- STOP before remediation plan)
**Date:** 2026-04-12
**Component:** MariloDataSheet
**Input:** `stages/02-prioritize/output/datasheet-priority-lanes.md` (14 lanes + 9 VP sub-lanes), `stages/01-intake/output/gap-inventory.md` (38 actionable post-dedup)

---

## Scope

Designs resolutions for **Phases A, B, C, and E** (17 lanes, 29 records).

**Phase D is SKIPPED** -- 9 VP SCSS sub-lanes + VP-datasheet-12 blocked on UD-01 (`IDataSheetTheme` contract, orchestrator-only).

---

## Phase A Resolutions (8 lanes, 22 records)

### RD-WS01: lane-ws-01 -- Workspace Coverage Audit

| Field | Value |
|---|---|
| **Record** | WS-01 |
| **Resolution type** | Gap-plan artifact |
| **Change nature** | New file |
| **Files changed** | `ICM/workspaces/datasheet-gap-analysis/_config/coverage-summary.md` |
| **Breaking** | No |
| **Effort** | XS |
| **Dependencies** | None |

**Design:**
Populate `coverage-summary.md` with a per-parameter test-coverage table. Structure:

```markdown
# DataSheet Coverage Summary

## Parameter Coverage

| Parameter | bUnit test? | Demo scenario? | Spec documented? | Notes |
|---|---|---|---|---|
| Data | Yes | Yes | Yes | |
| KeyField | Yes | Yes | Yes | |
| IsSaving | Partial | Yes | Yes | Missing: paste-during-save guard (SA-08) |
| AllowAddRow | Yes | Yes | Yes | |
| AllowDeleteRow | Yes | Yes | Yes | |
| AllowBulkPaste | Yes | Yes | Yes | |
| EnableVirtualization | No | Partial | Partial | Missing: 5k-row demo (UD-02) |
... (all 16 parameters from MariloDataSheet.razor.cs)

## Event Coverage
| Event | bUnit test? | Demo scenario? | Spec documented? |
...

## Keyboard Coverage
| Shortcut | bUnit test? | Demo scenario? | Spec documented? |
...
```

Derive from cross-referencing `MariloDataSheet.razor.cs` parameters, `MariloDataSheet.Editing.cs` HandleKeyDown branches, and existing test/demo files. Mark each cell Yes/No/Partial with notes referencing gap IDs where coverage is missing.

**Verification:** File exists and every public parameter, event, and keyboard shortcut from the source has a row.

---

### RD-SA01: lane-sa-01 -- Grid Root tabindex

| Field | Value |
|---|---|
| **Record** | SA-01 |
| **Resolution type** | Source fix + spec alignment + test |
| **Change nature** | Attribute addition |
| **Files changed** | `src/Marilo.Components/DataGrid/MariloDataSheet.razor` (line 6-14), bUnit test file |
| **Breaking** | No |
| **Effort** | XS |
| **Dependencies** | None (unblocks VP-datasheet-12 downstream) |

**Design:**
Add `tabindex="0"` to the grid root `<div>` at `MariloDataSheet.razor:6`:

```diff
- <div id="@_gridId"
-      class="@CombineClasses(CssProvider.DataSheetClass(IsLoading))"
+ <div id="@_gridId"
+      class="@CombineClasses(CssProvider.DataSheetClass(IsLoading))"
+      tabindex="0"
```

Position: after `class`, before `style`. Matches spec `keyboard-and-accessibility.md:74` which documents `tabindex="0"` on the grid root.

**Test:** bUnit test asserting root `<div role="grid">` has `tabindex="0"` attribute. Pattern: `cut.Find("[role='grid']").GetAttribute("tabindex").Should().Be("0");`

**Spec:** No spec change needed -- spec already documents this. The source is catching up.

**Verification:** `dotnet build` succeeds. bUnit test passes. Grid root markup contains `tabindex="0"`.

---

### RD-UD02-EU01: lane-ud02-eu01 -- Virtualization Threshold

| Field | Value |
|---|---|
| **Records** | UD-02, EU-01 |
| **Resolution type** | Spec edit + demo addition |
| **Change nature** | Documentation + new demo scenario |
| **Files changed** | `docs/component-specs/datasheet/virtualization-and-performance.md`, demo page (new `Virtualization.razor` or addition to `BulkOperations.razor`) |
| **Breaking** | No |
| **Effort** | S |
| **Dependencies** | None |

**Design:**

1. **Spec edit** (`virtualization-and-performance.md`): Add to the "Recommended Thresholds" section:
   ```
   > 10,000 rows is supported with EnableVirtualization=true; see Phase B roadmap.
   ```
   Add a note that the demo caps at 5,000 rows due to WASM host constraints (Phase B JS interop not yet implemented).

2. **Demo page**: Create `Virtualization.razor` (or extend BulkOperations scenario E) with:
   - Row-count toggle: 100 / 1,000 / 5,000
   - `EnableVirtualization="true"` for 1k and 5k variants
   - Performance note in demo comments: "10k rows are tested but not demoed; see spec threshold note."
   - Do NOT add a 10k option (per UD-02 resolution)

**Auto-closes:** VP-datasheet-D03 deferral (effective WONTFIX at capture level -- no 10k capture will be made).

**Verification:** Demo builds. Spec contains the verbatim threshold text. No 10k demo option exists.

---

### RD-SA08-EU03: lane-sa08-eu03 -- Paste-during-save Guard

| Field | Value |
|---|---|
| **Records** | SA-08, EU-03 |
| **Resolution type** | Source fix + test + demo |
| **Change nature** | Early-return guard |
| **Files changed** | `src/Marilo.Components/DataGrid/MariloDataSheet.Editing.cs` (~line 428), bUnit test file, demo page |
| **Breaking** | No |
| **Effort** | S |
| **Dependencies** | None (SA-08 source fix must land before EU-03 demo is truthful -- internal ordering) |

**Design:**

1. **Source fix** -- Add `IsSaving` guard to `PasteFromClipboard` in `MariloDataSheet.Editing.cs`:

   ```csharp
   [JSInvokable]
   public async Task PasteFromClipboard(string tsvData)
   {
       if (!AllowBulkPaste || _activeCellRow is null || _activeCellField is null) return;
       if (IsSaving) return;  // SA-08: paste disabled during save
       // ... rest unchanged
   }
   ```

   Single line addition after existing guard check (line 428). The `IsSaving` parameter is already wired on the component; this just honors it in the paste path.

2. **bUnit test:** Set `IsSaving = true` on the component, invoke `PasteFromClipboard("foo\tbar")`, assert no cells changed (dirty count unchanged, no CommitCellEdit calls).

3. **Demo (EU-03):** Add scenario to BulkOperations or Clipboard demo:
   - Button sets `IsSaving = true` on a timer (simulates server save)
   - User attempts Ctrl+V during the saving window
   - Visual feedback: "Paste blocked -- save in progress" (leveraging existing `_ariaAnnouncement`)
   - Timer completes, paste is re-enabled

**Verification:** `dotnet build` + `dotnet test` pass. bUnit test confirms paste is a no-op when `IsSaving = true`.

---

### RD-SA13-EU05: lane-sa13-eu05 -- Missing aria-live Announcements

| Field | Value |
|---|---|
| **Records** | SA-13, EU-05 |
| **Resolution type** | Source fix + spec alignment + test + demo |
| **Change nature** | New announcements in SaveAllAsync |
| **Files changed** | `src/Marilo.Components/DataGrid/MariloDataSheet.Data.cs` (SaveAllAsync method), bUnit test file, demo page |
| **Breaking** | No |
| **Effort** | S-M |
| **Dependencies** | None (SA-13 source fix must land before EU-05 demo -- internal ordering) |

**Design:**

Three missing `aria-live` announcements per spec `keyboard-and-accessibility.md:148-154`:

1. **"Saving changes..."** -- Start-of-save announcement. Add after the re-entrancy guard and before Step 1 (validate):

   ```csharp
   // After _isSaving = true;
   _ariaAnnouncement = "Saving changes.";
   StateHasChanged();
   ```

   Location: `MariloDataSheet.Data.cs`, inside `SaveAllAsync`, after line ~266 (`_isSaving = true;`).

2. **"Save failed. {N} validation errors."** -- After Step 3 (validation block). Modify the existing blocked announcement:

   ```csharp
   // Step 3: Block if invalid
   if (!isValid)
   {
       var errorCount = _dirtyRows.Values.Sum(e => e.ValidationErrors.Count);
       _ariaAnnouncement = errorCount == 1
           ? "Save failed. 1 validation error."
           : $"Save failed. {errorCount} validation errors.";
       StateHasChanged();
       return;
   }
   ```

   This replaces the current `"Save blocked: fix validation errors first."` with the spec-prescribed wording that includes the error count.

3. **"{N} cells have errors"** -- After ValidateAllAsync when errors are found but before the save-block return. This can be combined with announcement #2 since they trigger at the same point. If spec wants them as separate announcements, use: `$"Save failed. {errorCount} validation errors. {cellErrorCount} cells have errors."`. Given the single `_ariaAnnouncement` slot, combine into one announcement.

**Exception path:** Add a catch-block announcement:

   ```csharp
   catch (Exception)
   {
       _ariaAnnouncement = "Save failed. An error occurred.";
       // ... existing rollback logic
   }
   ```

**bUnit tests:**
- Assert `_ariaAnnouncement == "Saving changes."` after SaveAllAsync begins (use `_savedStateDurationMs = 0` + async timing)
- Assert save-blocked message includes error count when validation fails
- Assert success message on happy path (already exists: `"Changes saved successfully."`)
- Assert catch-path announcement on exception

**Demo (EU-05):** SaveAllAsync failure + retry scenario:
- Wire `OnSaveAll` to throw on first call, succeed on second
- Show aria-live announcement text in a visible `<pre>` for demo purposes
- Demonstrate retry flow

**Verification:** `dotnet build` + `dotnet test` pass. All three announcement paths covered by bUnit tests.

---

### RD-SA03: lane-sa-03 -- AddRow ActivateCell

| Field | Value |
|---|---|
| **Record** | SA-03 |
| **Resolution type** | Source fix + test |
| **Change nature** | Method call addition |
| **Files changed** | `src/Marilo.Components/DataGrid/MariloDataSheet.razor.cs` (~line 235), bUnit test file |
| **Breaking** | No |
| **Effort** | XS |
| **Dependencies** | None |

**Design:**
Add `ActivateCell` call at the end of `AddRowAsync` to move focus to the first editable column of the new row. Per spec `bulk-operations-and-saveall.md:119`: "Active cell moves to first editable column of the new row."

```csharp
// At end of AddRowAsync, after the dirty-entry creation and DirtyFields seeding:
var firstEditableCol = _columns.FirstOrDefault(c => c.Editable && c.ColumnType != DataSheetColumnType.Computed);
if (firstEditableCol != null)
{
    ActivateCell(newItem, firstEditableCol.Field);
}
```

Insert after the `foreach` loop that seeds DirtyFields (~line 240+). The `ActivateCell` call sets `_activeCellRow` and `_activeCellField` and calls `StateHasChanged()`.

**bUnit test:** Invoke `AddRowAsync`, assert `_activeCellRow` equals `newItem` and `_activeCellField` equals the first editable column's Field name.

**Verification:** `dotnet build` + `dotnet test` pass. After AddRow, active cell is on the new row's first editable column.

---

### RD-SA04: lane-sa-04 -- Reset Clears Undo Buffer

| Field | Value |
|---|---|
| **Record** | SA-04 |
| **Resolution type** | Source fix + test |
| **Change nature** | Method call addition |
| **Files changed** | `src/Marilo.Components/DataGrid/MariloDataSheet.Data.cs` (ResetAsync method, ~line 517), bUnit test file |
| **Breaking** | No |
| **Effort** | XS |
| **Dependencies** | None |

**Design:**
Add `_undoBuffer.Clear()` to `ResetAsync` per spec `bulk-operations-and-saveall.md:162`: "The undo buffer is cleared."

```csharp
public Task ResetAsync()
{
    foreach (var entry in _dirtyRows.Values)
    {
        if (entry.IsDeleted) continue;
        RestoreEntryOrRemoveNewRow(entry);
    }

    _dirtyRows.Clear();
    _undoBuffer.Clear();  // SA-04: clear undo buffer on reset
    ClearActiveCell();
    _ariaAnnouncement = "All changes have been reset.";
    StateHasChanged();
    return Task.CompletedTask;
}
```

Insert `_undoBuffer.Clear();` after `_dirtyRows.Clear();` and before `ClearActiveCell();`. The `_undoBuffer` field is in `MariloDataSheet.Editing.cs` (line 23) but is accessible from the `Data.cs` partial since they share the same class.

**bUnit test:** Edit a cell (populates undo buffer), call `ResetAsync`, then invoke Ctrl+Z (HandleKeyDown("z", ctrl:true, shift:false)). Assert the cell value is NOT reverted to the pre-reset value (undo buffer was cleared, so Ctrl+Z is a no-op).

**Verification:** `dotnet build` + `dotnet test` pass. Ctrl+Z after Reset is a no-op.

---

### RD-EU02: lane-eu-02 -- Copy-Paste Round-Trip Demo

| Field | Value |
|---|---|
| **Record** | EU-02 |
| **Resolution type** | Demo addition |
| **Change nature** | New demo scenario |
| **Files changed** | Demo page (BulkOperations or new Clipboard demo page) |
| **Breaking** | No |
| **Effort** | XS |
| **Dependencies** | None |

**Design:**
Add a copy-paste round-trip scenario to the clipboard demo:

- Grid with formatted columns (e.g., currency `Format="@(item => item.Salary.ToString("C"))"`, date with format)
- User copies a cell range (Ctrl+C)
- Pastes back into the same or different location (Ctrl+V)
- Verifies that `data-raw-value` round-trip contract from V04.4 is exercised: the pasted value is the raw underlying value (e.g., `42000`), not the formatted display string (e.g., `$42,000.00`)
- Include visual indicator showing "Raw value preserved through round-trip"

**Verification:** Demo page builds and renders. Scenario exercises the Format + data-raw-value path.

---

## Phase B Resolutions (3 lanes, 3 records -- orchestrator pre-approved)

### RD-SA02: lane-sa-02 -- AddRow Append vs. Prepend (SPEC FIX)

| Field | Value |
|---|---|
| **Record** | SA-02 |
| **Resolution type** | Spec fix |
| **Change nature** | Wording correction |
| **Files changed** | `docs/component-specs/datasheet/bulk-operations-and-saveall.md` (~line 117) |
| **Breaking** | No |
| **Effort** | XS |
| **Dependencies** | None (orchestrator pre-approved: spec fix direction) |

**Design:**
Per orchestrator pre-approval, this is a spec fix. The source behavior (`_displayRows.Insert(0, newItem)` -- prepend) is the intended UX.

Change spec wording at `bulk-operations-and-saveall.md:117`:

```diff
- Appends the row to the end of the data collection.
+ Prepends the row at the top of the data collection (insert at index 0). New rows appear at the top of the sheet, following the common spreadsheet convention for data entry.
```

**Rationale:** Source behavior is deliberate -- `Insert(0, newItem)` places new rows at the top, matching Excel's insert-above pattern. The spec wording was an oversight.

**Verification:** Spec file updated. No source changes. No test impact.

---

### RD-SA05: lane-sa-05 -- Saving->Saved Transition (SPEC FIX)

| Field | Value |
|---|---|
| **Record** | SA-05 |
| **Resolution type** | Spec fix |
| **Change nature** | Wording clarification |
| **Files changed** | `docs/component-specs/datasheet/bulk-operations-and-saveall.md` (~line 104-107) |
| **Breaking** | No |
| **Effort** | XS |
| **Dependencies** | None (orchestrator pre-approved: spec fix, component-driven transition) |

**Design:**
Per orchestrator pre-approval, this is a spec fix. The source's component-driven `Task.Delay(_savedStateDurationMs)` transition is the intended behavior.

Update the cell-state transition table at `bulk-operations-and-saveall.md:104-107`:

```diff
- | Saving | Saved | IsSaving set to false |
+ | Saving | Saved | Component-driven: after OnSaveAll completes successfully, the component automatically transitions cells from Saving to Saved for a brief visual indicator period (~1 second), then clears to Pristine. This transition is managed internally by the component, not keyed off the consumer's IsSaving parameter. |
```

Also add a clarifying note below the table:

```markdown
> **Note:** The `IsSaving` parameter controls the toolbar spinner and Save button disabled state. The cell-level `Saving -> Saved -> Pristine` transition is managed by the component's internal save lifecycle (see `SaveAllAsync` implementation). These are independent state machines.
```

**Rationale:** The component-driven timer is a polished UX pattern. Making it consumer-keyed would break the auto-feedback loop.

**Verification:** Spec file updated. No source changes. Cell-state transition wording matches actual `SaveAllAsync` behavior.

---

### RD-SA09: lane-sa-09 -- Double-Click Edit Entry (SOURCE FIX)

| Field | Value |
|---|---|
| **Record** | SA-09 |
| **Resolution type** | Source fix + test |
| **Change nature** | New event handler |
| **Files changed** | `src/Marilo.Components/DataGrid/MariloDataSheet.Rendering.cs` (~line 122-123), bUnit test file |
| **Breaking** | No |
| **Effort** | S |
| **Dependencies** | None (orchestrator pre-approved: source fix, add ondblclick) |

**Design:**
Per orchestrator pre-approval, add `ondblclick` handler. The spec is correct at `editing-and-validation.md:50` ("Double-click on any cell"); the source is incomplete.

1. **Add `ondblclick` handler** in `MariloDataSheet.Rendering.cs`, alongside the existing `onclick` handler (~line 122-123):

   ```csharp
   // Existing click handler
   builder.AddAttribute(31, "onclick",
       EventCallback.Factory.Create<MouseEventArgs>(this, (_) => OnCellClick(cellRow, cellField)));

   // SA-09: Double-click enters edit mode directly
   builder.AddAttribute(32, "ondblclick",
       EventCallback.Factory.Create<MouseEventArgs>(this, (_) => OnCellDoubleClick(cellRow, cellField)));
   ```

   Note: Attribute sequence numbers may need adjustment if 32 is already used. Check neighboring AddAttribute calls.

2. **Add `OnCellDoubleClick` method** in `MariloDataSheet.Editing.cs`:

   ```csharp
   internal void OnCellDoubleClick(TItem row, string field)
   {
       var column = _columns.FirstOrDefault(c => c.Field == field);
       if (column is null) return;

       // Computed and non-editable cells: activate only (same as single-click)
       if (column.ColumnType == DataSheetColumnType.Computed || !column.Editable)
       {
           ActivateCell(row, field);
           return;
       }

       // Checkbox columns: toggle on single-click already; double-click is a no-op
       // (double-click would double-toggle back to original, which is confusing)
       if (column.ColumnType == DataSheetColumnType.Checkbox)
       {
           return;
       }

       // All other editable columns: enter edit mode directly
       EnterEditMode(row, field);
   }
   ```

   **UX rationale:** The existing single-click path uses a "click to activate, click again to edit" two-step. Double-click bypasses the activation step and goes straight to edit mode, matching Excel/Google Sheets behavior. Both affordances coexist: single-click activates, double-click edits. Checkbox columns are excluded because double-click would toggle twice (on-off), which is confusing.

3. **bUnit test:**
   - Render a DataSheet with an editable text column
   - Double-click a cell (trigger `ondblclick` event)
   - Assert `IsCellEditing(row, field)` returns `true`
   - Verify single-click still works as before (activate, then second click edits)

**Verification:** `dotnet build` + `dotnet test` pass. Double-click enters edit mode. Single-click path unchanged.

---

## Phase C Resolutions (2 lanes, 3 records)

### RD-V03: lane-v03 -- Range Selection Model (LARGE -- decomposition)

| Field | Value |
|---|---|
| **Records** | V03, V07.4 |
| **Resolution type** | New feature implementation |
| **Change nature** | New source files + major editing changes |
| **Files changed** | Multiple -- see sub-tasks below |
| **Breaking** | No (additive) |
| **Effort** | L |
| **Dependencies** | None (but unblocks VP-datasheet-D02, SA-06 full fix, EU-04 partial) |

**Design:**
This is the largest gap in the inventory. It introduces the entire rectangular range selection model described in `docs/component-specs/datasheet/selection-and-ranges.md:37-114`.

**Current state:** Source tracks only `_activeCellRow`/`_activeCellField` (single cell) and `_selectedRows` (`HashSet<TItem>` for row-level bulk delete). No multi-cell selection exists.

**Target state:** Full rectangular range selection with anchor/extent, Shift+Click, Shift+Arrow, click-drag, Ctrl+A, and range-scoped operations (Copy/Paste/Fill Down/Delete).

**Decomposition into sub-tasks** (recommended for Stage 04 dispatch):

#### Sub-task V03.1: Selection State Model

New file: `src/Marilo.Components/DataGrid/DataSheetSelectionState.cs`

```csharp
internal class DataSheetSelectionState<TItem>
{
    /// <summary>Anchor cell (where selection started).</summary>
    public TItem? AnchorRow { get; set; }
    public string? AnchorField { get; set; }

    /// <summary>Extent cell (where selection ended / currently extends to).</summary>
    public TItem? ExtentRow { get; set; }
    public string? ExtentField { get; set; }

    /// <summary>Whether a multi-cell range is active (anchor != extent).</summary>
    public bool HasRange => AnchorRow != null && ExtentRow != null
        && !(EqualityComparer<TItem>.Default.Equals(AnchorRow, ExtentRow)
             && AnchorField == ExtentField);

    /// <summary>Clears the range, keeping only the active cell.</summary>
    public void ClearRange()
    {
        ExtentRow = default;
        ExtentField = null;
    }

    /// <summary>Sets a single-cell selection (degenerate range).</summary>
    public void SetSingleCell(TItem row, string field)
    {
        AnchorRow = row;
        AnchorField = field;
        ExtentRow = row;
        ExtentField = field;
    }
}
```

#### Sub-task V03.2: Integrate Selection State into DataSheet

In `MariloDataSheet.Editing.cs`:
- Add `internal DataSheetSelectionState<TItem> _selection = new();`
- Modify `ActivateCell` to call `_selection.SetSingleCell(row, field)` and clear any existing range
- Modify `OnCellClick` to check for Shift modifier:
  - Plain click: `_selection.SetSingleCell(row, field)` (existing behavior)
  - Shift+Click: `_selection.ExtentRow = row; _selection.ExtentField = field;` (extend range)
- Add helper: `GetSelectedCells()` returns `List<(TItem Row, string Field)>` -- all cells in the rectangular region between anchor and extent

#### Sub-task V03.3: Keyboard Range Extension

In `HandleKeyDown`:
- `Shift+Arrow`: extend `_selection.ExtentRow`/`_selection.ExtentField` instead of moving active cell
- `Ctrl+A` (V07.4): `_selection.AnchorRow = _displayRows[0]; _selection.AnchorField = _columns[0].Field; _selection.ExtentRow = _displayRows[^1]; _selection.ExtentField = _columns[^1].Field;`
- Plain Arrow: call `_selection.ClearRange()` then move active cell (existing behavior)

#### Sub-task V03.4: Range-Scoped Operations

Update existing operations to use `GetSelectedCells()` instead of single active cell:

- **Fill Down (Ctrl+D):** Iterate `GetSelectedCells()` filtered to active column, set value from anchor row
- **Delete:** Iterate `GetSelectedCells()`, clear each editable non-computed cell
- **Copy (Ctrl+C):** Build TSV from `GetSelectedCells()` (this is JS-side; need to pass selection bounds to JS interop)
- **Paste (Ctrl+V):** Already anchored at active cell; no change needed (paste fills from anchor regardless of range)

#### Sub-task V03.5: Rendering -- Selection Highlight

In `MariloDataSheet.Rendering.cs`:
- Add `IsInSelectedRange(row, field)` helper
- Add CSS class `mar-datasheet__cell--selected` when cell is in range
- Provider SCSS for the highlight is gated on UD-01 (Phase D), but the class emission must land now

#### Sub-task V03.6: Shift+Click in Rendering

In `MariloDataSheet.Rendering.cs`:
- Change `onclick` handler to pass `MouseEventArgs` so `ShiftKey` is available
- `OnCellClick` signature changes to `OnCellClick(TItem row, string field, bool shiftKey)`

#### Sub-task V03.7: Tests

- bUnit: Shift+Click creates range (anchor stays, extent moves)
- bUnit: Shift+Arrow extends range
- bUnit: Ctrl+A selects all
- bUnit: Plain click clears range
- bUnit: Delete on range clears all editable cells in range
- bUnit: Fill Down on range fills column within range

**Spec alignment:** Spec `selection-and-ranges.md` already describes the target behavior. No spec changes needed -- source is catching up.

**Note:** Click-and-drag (spec line 65) requires JS interop for mousedown/mouseup tracking. This may be deferred to a follow-up if the JS interop layer is not ready. Shift+Click and Shift+Arrow cover the core use cases.

**Verification:** All sub-task tests pass. Range selection visually highlights cells (class emitted). Operations scope to range.

---

### RD-VP07-SRC: lane-vp07-source -- Frozen Column (source half)

| Field | Value |
|---|---|
| **Record** | VP-datasheet-07 (source half) |
| **Resolution type** | Source addition + spec alignment + test |
| **Change nature** | New parameter + CSS class emission |
| **Files changed** | `src/Marilo.Components/DataGrid/MariloDataSheetColumn.razor`, `src/Marilo.Components/DataGrid/MariloDataSheet.Rendering.cs`, bUnit test file |
| **Breaking** | No (additive -- new parameter) |
| **Effort** | S |
| **Dependencies** | None (SCSS half is Phase D, gated on UD-01) |

**Design:**

1. **New parameter** on `MariloDataSheetColumn.razor`:

   ```csharp
   /// <summary>Freezes the column so it stays visible during horizontal scroll. Uses position:sticky.</summary>
   [Parameter] public bool Frozen { get; set; }
   ```

   Add after `Width` parameter (~line 40).

2. **Rendering changes** in `MariloDataSheet.Rendering.cs`:

   For header cells (`<th>`):
   ```csharp
   var frozenClass = column.Frozen ? "mar-datasheet__header-cell--frozen" : null;
   // Add frozenClass to the class attribute
   ```

   For body cells (`<td>`):
   ```csharp
   var frozenClass = column.Frozen ? "mar-datasheet__cell--frozen" : null;
   // Add frozenClass to the class attribute
   ```

   For frozen columns, also emit inline `position:sticky; left:{cumulativeLeft}px;` where `cumulativeLeft` is calculated by summing widths of preceding frozen columns. This ensures multiple frozen columns stack correctly.

   **Left offset calculation:**
   ```csharp
   var frozenColumns = _columns.Where(c => c.Frozen).ToList();
   var frozenLeftOffset = new Dictionary<string, string>();
   double left = 0;
   if (AllowDeleteRow) left += 40; // select header width
   foreach (var fc in _columns)
   {
       if (fc.Frozen)
       {
           frozenLeftOffset[fc.Field] = $"{left}px";
       }
       // Parse width or use default
       left += ParseColumnWidth(fc);
   }
   ```

   Note: This requires frozen columns to be contiguous and leftmost. Add validation warning if a non-frozen column precedes a frozen one.

3. **Spec alignment:** `docs/component-specs/datasheet/columns-and-schema.md` already lists `Frozen` as in-scope. Verify the parameter table includes it; if not, add a row.

4. **bUnit tests:**
   - Column with `Frozen="true"` emits `mar-datasheet__cell--frozen` class
   - Header cell emits `mar-datasheet__header-cell--frozen` class
   - Inline `position:sticky` style is present on frozen cells

**Verification:** `dotnet build` + `dotnet test` pass. Frozen columns emit sticky classes. SCSS styling deferred to Phase D.

---

## Phase E Resolutions (4 lanes, 10 records)

### RD-SPEC-FIXES: lane-spec-fixes -- Batch Spec Wording Fixes

| Field | Value |
|---|---|
| **Records** | SA-07, SA-11, SA-12, SA-14, SA-15 |
| **Resolution type** | Spec fixes (all 5) |
| **Change nature** | Wording corrections |
| **Files changed** | `docs/component-specs/datasheet/bulk-paste-and-clipboard.md`, `docs/component-specs/datasheet/editing-and-validation.md`, `docs/component-specs/datasheet/columns-and-schema.md` |
| **Breaking** | No |
| **Effort** | S |
| **Dependencies** | None |

**Per-record designs:**

#### SA-07: Date coercion culture mismatch

File: `bulk-paste-and-clipboard.md:91`

```diff
- DateTime.TryParse with the current culture
+ DateTime.TryParse with InvariantCulture (matches the invariant round-trip format used by data-raw-value)
```

**Rationale:** Source `Editing.cs:569-574 TryParseDateCell` uses `CultureInfo.InvariantCulture`. This is deliberate for V04.4 round-trip fidelity.

#### SA-11: Validation short-circuit description

File: `editing-and-validation.md:139`

```diff
- Both the required check and the custom Validate function can produce errors, but only one error message is displayed (the required error takes priority if both fail).
+ The required check runs first. If it fails, the custom Validate function is not called (short-circuit). Only one error message is displayed per cell per commit: either the required error or the custom validation error, never both simultaneously.
```

**Rationale:** Source `Data.cs:166-192 RunColumnValidation` returns immediately on required failure and never invokes `column.Validate`. The phrase "both can produce errors" implies both run.

#### SA-12: Dirty count includes invalid-and-dirty rows

File: `editing-and-validation.md:193`

```diff
- The dirty count indicator does not include invalid-only rows in the count.
+ The dirty count indicator includes all rows with dirty fields, including rows that also have validation errors. There is no "invalid-only" state distinct from dirty — a row with validation errors that has been edited is counted as dirty.
```

**Rationale:** Source `razor:31,168` counts `_dirtyRows.Count(kv => !kv.Value.IsDeleted && kv.Value.DirtyFields.Count > 0)`. Since invalid commits also populate DirtyFields, invalid-AND-dirty rows ARE counted. No separate "invalid-only" pathway exists.

#### SA-14: Number Required -- null only, not zero

File: `columns-and-schema.md:118`

```diff
- Rejects null or zero when Required is set (zero rejection applies only to non-nullable types where default is 0).
+ Rejects null when Required is set. Zero (0) is treated as a valid value for number columns. For non-nullable numeric types, only an explicit null (boxing) triggers the required error.
```

**Rationale:** Source `RunColumnValidation` only checks `value is null`. A `decimal` `0m` passes. Rejecting zero would be a surprising behavior change for callers who legitimately enter zero.

#### SA-15: Date Required -- null only, not default(DateTime)

File: `columns-and-schema.md:231`

```diff
- Rejects null or default(DateTime) (DateTime.MinValue) when Required is set.
+ Rejects null when Required is set. DateTime.MinValue (default(DateTime)) is treated as a valid value. To reject default dates, use a custom Validate function.
```

**Rationale:** Mirror SA-14 resolution. Source only rejects null. Adding `default(DateTime)` rejection would require an `IsNumeric && default(T).Equals(value)` branch -- a source change with surprising behavior for callers.

**Verification:** All 5 spec files updated. Wording matches actual source behavior. No source changes.

---

### RD-DEMO-P2: lane-demo-p2 -- Demo-Only Additions

| Field | Value |
|---|---|
| **Records** | EU-04, EU-08 |
| **Resolution type** | Demo additions |
| **Change nature** | New demo scenarios |
| **Files changed** | Demo page(s) |
| **Breaking** | No |
| **Effort** | XS |
| **Dependencies** | EU-04 partially gated on V03 for Ctrl+A, but Delete-key portion is independent |

**Per-record designs:**

#### EU-04: Delete-key scenario

Add interactive Delete-key scenario to `Keyboard-and-Accessibility.razor`:
- Grid with pre-populated data
- User selects a cell and presses Delete
- Cell value clears to type default (`""` for text, `0` for number, etc.)
- Currently listed in `_keyboard[]` table but not interactive
- **Note:** Full multi-cell Delete (select range, press Delete to clear all) requires V03. This demo covers single-cell Delete, which works today.

#### EU-08: CellTemplate scenario

Add `CellTemplate` custom-rendering scenario to the demo:
- Column with `CellTemplate` rendering a colored badge or icon based on cell value
- Demonstrates `DataSheetCellContext<TItem>` properties: `Item`, `Field`, `Value`, `IsEditing`, `IsDirty`, `ValidationError`
- Shows how CellTemplate coexists with editing (template renders in read mode, standard editor in edit mode)

**Verification:** Demo pages build. Both scenarios render and are interactive.

---

### RD-SA06: lane-sa-06 -- Fill-Down Editable Filter (partial)

| Field | Value |
|---|---|
| **Record** | SA-06 |
| **Resolution type** | Spec/demo wording softening (partial fix now; full fix deferred to V03) |
| **Change nature** | Wording update |
| **Files changed** | `docs/component-specs/datasheet/selection-and-ranges.md` (~line 88), demo page |
| **Breaking** | No |
| **Effort** | XS |
| **Dependencies** | Full behavioral fix depends on V03 (range selection model) |

**Design:**

**Immediate (partial):** Soften spec and demo wording to reflect current behavior:

Spec `selection-and-ranges.md:88`:
```diff
- Copies the value of the active cell (top row of the selection) down to all cells in the same column within the current selection range. Only editable, non-computed cells are filled.
+ Copies the value of the active cell down to all cells in the same column for all selected rows. Currently operates on row-level selection (_selectedRows); rectangular range scoping will be added with the range selection model (V03). Only editable columns are targeted; computed columns are skipped.
```

Demo `Keyboard-and-Accessibility.razor:100`: Soften any wording that implies range-aware fill-down to reflect the current row-level behavior.

**Deferred (full fix):** After V03 lands, update Fill Down in `HandleKeyDown` to use `GetSelectedCells()` filtered to the active column within the rectangular range, and filter out `!column.Editable || column.ColumnType == DataSheetColumnType.Computed`. Then update spec back to the target wording.

**Verification:** Spec and demo wording accurately describe current behavior. No source changes.

---

### RD-P3-POLISH: lane-p3-polish -- P3 Polish

| Field | Value |
|---|---|
| **Records** | SRC-01, NM-01 |
| **Resolution type** | Spec wording tweaks |
| **Change nature** | Minor wording corrections |
| **Files changed** | `docs/component-specs/datasheet/virtualization-and-performance.md`, `docs/component-specs/datasheet/overview.md` |
| **Breaking** | No |
| **Effort** | XS |
| **Dependencies** | None |

**Per-record designs:**

#### SRC-01: Skeleton row count description

File: `virtualization-and-performance.md:77`

```diff
- Renders a viewport-calculated number of skeleton rows to fill the visible area.
+ Renders a fixed number of skeleton rows (currently 5) as a loading placeholder. The count is not viewport-calculated.
```

**Rationale:** Source `MariloDataSheet.razor:80` uses `for (var s = 0; s < 5; s++)` -- a hard-coded 5-row skeleton. Spec overstates the sophistication.

#### NM-01: Class and Style parameter documentation

File: `overview.md:122-123`

```diff
- | Class | string? | null | CSS class applied to the root element. |
- | Style | string? | null | Inline style applied to the root element. |
+ | Class | string? | null | _(Inherited from MariloComponentBase via AdditionalAttributes)_ CSS class applied to the root element. Not a direct [Parameter] on MariloDataSheet. |
+ | Style | string? | null | _(Inherited from MariloComponentBase via AdditionalAttributes)_ Inline style applied to the root element. Not a direct [Parameter] on MariloDataSheet. |
```

**Rationale:** Source does not expose `Class` or `Style` as `[Parameter]`; they flow through `AdditionalAttributes` from `MariloComponentBase`. Spec overstates public surface.

**Verification:** Spec files updated. Wording matches actual source.

---

## Coverage Verification

### All Phase A/B/C/E records accounted for

| Phase | Lane | Records | Resolution ID | Status |
|---|---|---|---|---|
| A | lane-ws-01 | WS-01 | RD-WS01 | Designed |
| A | lane-sa-01 | SA-01 | RD-SA01 | Designed |
| A | lane-ud02-eu01 | UD-02, EU-01 | RD-UD02-EU01 | Designed |
| A | lane-sa08-eu03 | SA-08, EU-03 | RD-SA08-EU03 | Designed |
| A | lane-sa13-eu05 | SA-13, EU-05 | RD-SA13-EU05 | Designed |
| A | lane-sa-03 | SA-03 | RD-SA03 | Designed |
| A | lane-sa-04 | SA-04 | RD-SA04 | Designed |
| A | lane-eu-02 | EU-02 | RD-EU02 | Designed |
| B | lane-sa-02 | SA-02 | RD-SA02 | Designed (spec fix, pre-approved) |
| B | lane-sa-05 | SA-05 | RD-SA05 | Designed (spec fix, pre-approved) |
| B | lane-sa-09 | SA-09 | RD-SA09 | Designed (source fix, pre-approved) |
| C | lane-v03 | V03, V07.4 | RD-V03 | Designed (decomposed into 7 sub-tasks) |
| C | lane-vp07-source | VP-07 (src) | RD-VP07-SRC | Designed |
| E | lane-spec-fixes | SA-07, SA-11, SA-12, SA-14, SA-15 | RD-SPEC-FIXES | Designed (5 spec fixes) |
| E | lane-demo-p2 | EU-04, EU-08 | RD-DEMO-P2 | Designed |
| E | lane-sa-06 | SA-06 | RD-SA06 | Designed (partial, full deferred to V03) |
| E | lane-p3-polish | SRC-01, NM-01 | RD-P3-POLISH | Designed |

**Total records with resolutions:** 29 (of 38 actionable)
**Phase D skipped:** 9 VP SCSS sub-lanes + VP-datasheet-12 = 10 records (blocked on UD-01)
**Retired:** SA-10 (dedup into SA-07)
**Coverage:** 29 designed + 10 Phase D skipped = 39 - 1 retired = 38 actionable. All accounted for.

---

## Checkpoint

**STOP -- end of Stage 03 resolution-design.**

- 17 resolution designs produced for Phases A/B/C/E.
- Phase D (9 VP SCSS sub-lanes + VP-12) skipped per orchestrator instruction (blocked on UD-01).
- V03 (range selection) decomposed into 7 sub-tasks for Stage 04 dispatch planning.
- All 3 Phase B items use orchestrator pre-approved directions (SA-02=spec, SA-05=spec, SA-09=source).
- Every non-skipped record has a concrete resolution with file paths, change nature, and verification criteria.
- No source changes made in this stage -- designs only.
