# Resolution Design — GAP-DATASHEET-V03: Cell Range Selection

**Gap ID:** GAP-DATASHEET-V03
**Component:** `MariloDataSheet<TItem>`
**Stage:** 03-resolution-design
**Written:** 2026-04-10
**Author:** Gap resolution loop (Stage 01b verification surfaced this)
**Upstream:** `../../../../datasheet-delivery/stages/01-spec-review/output/datasheet-spec-gaps-verified-2026-04-10.md` (V03 section)
**Spec reference:** `docs/component-specs/datasheet/selection-and-ranges.md`
**Source baseline:** `src/Marilo.Components/DataGrid/MariloDataSheet*` (1,324 lines across 7 partials)

## Problem Statement

The spec defines a **two-tier selection model** for `MariloDataSheet<TItem>`:

1. **Active cell** — exactly one cell has focus; acts as the anchor for range operations.
2. **Rectangular range** — multi-cell selection created by Shift+Click, click-drag, Shift+Arrow, or Ctrl+A.

Copy, Paste, Fill Down, and Delete all operate on the range when one exists; otherwise they fall back to the active cell.

The current source implements **only the active cell** and **row-level selection via checkboxes** (`HashSet<TItem> _selectedRows`). There is no rectangular range state, no shift-click handling, no click-drag, no range-aware copy/paste/fill/delete, and no public `DataSheetSelection<TItem>` model. This is the single largest gap surfaced by the Stage 01b verification and is not fixable as a targeted patch — it requires a cohesive design.

## Target Behavior

### Selection states the component must track

| Concept | Shape | When set | When cleared |
|---|---|---|---|
| Active cell | `(TItem row, string field)` | Click, ActivateCell, arrow key nav, paste anchor | Never cleared while grid has focus (always exactly one) |
| Range anchor | `(TItem row, string field)` | Mousedown, Shift+Click origin lock, arrow start | Any single-cell click without shift |
| Range extent | `(TItem row, string field)` | Mousemove during drag, Shift+Click target, Shift+Arrow target | Any single-cell click without shift |
| Range bounds | computed min/max of anchor + extent | Derived | Derived |

**Invariant:** Active cell is always present. Range is optional. When range is absent, operations target the active cell. When range is present, active cell equals the most recently moved endpoint (anchor or extent depending on interaction).

### Input handling

| Input | Action |
|---|---|
| Click cell | Set active + anchor = clicked cell, clear extent |
| Shift+Click cell | Keep anchor, set extent = clicked cell, set active = clicked cell |
| Mousedown on cell | Set anchor = cell, enter drag mode |
| Mousemove during drag | Update extent = cell under pointer |
| Mouseup | Exit drag mode, keep range |
| Arrow key (no shift) | Move active cell by 1, clear range |
| Shift+Arrow | Move extent by 1 (or initialize anchor=active if no range), set active = extent |
| Ctrl+A | Set anchor = (first row, first col), extent = (last row, last col), active = extent |
| Escape (no edit mode) | Clear range, active cell unchanged |
| Any non-navigation key | Clear range on commit; active cell remains |

### Operations on the range

| Operation | Active-cell-only behavior (today) | Range-aware behavior (target) |
|---|---|---|
| Copy (Ctrl+C) | Copy active cell value to clipboard | Copy all cells in range bounds as TSV (tab-separated rows, newline-separated) |
| Paste (Ctrl+V) | Paste TSV anchored at active cell | Paste TSV anchored at range anchor (if range) or active cell (if not) — existing paste already supports this; minimal change |
| Fill Down (Ctrl+D) | Copy active cell value down to all selected rows | Copy top row of range down through range rows for each column in range |
| Delete | Clear active cell | Clear every editable cell in range |
| Cell visual highlight | Active class on active cell | Active class on active cell + `--in-range` class on every cell in range bounds |

### Public API additions

```csharp
// New model (Marilo.Core.Models.DataSheet)
public sealed class DataSheetSelection<TItem>
{
    public (TItem Row, string Field) ActiveCell { get; init; }
    public DataSheetRange<TItem>? Range { get; init; }
}

public sealed class DataSheetRange<TItem>
{
    public (TItem Row, string Field) Anchor { get; init; }
    public (TItem Row, string Field) Extent { get; init; }

    // Derived bounds (computed lazily or eagerly in ctor)
    public int TopRowIndex { get; init; }
    public int BottomRowIndex { get; init; }
    public int LeftColumnIndex { get; init; }
    public int RightColumnIndex { get; init; }
    public bool IsSingleCell => TopRowIndex == BottomRowIndex && LeftColumnIndex == RightColumnIndex;
    public int CellCount => (BottomRowIndex - TopRowIndex + 1) * (RightColumnIndex - LeftColumnIndex + 1);
}

// New event args
public sealed class DataSheetSelectionChangedArgs<TItem>
{
    public DataSheetSelection<TItem>? Previous { get; init; }
    public DataSheetSelection<TItem> Current { get; init; } = default!;
    public SelectionChangeReason Reason { get; init; }
}

public enum SelectionChangeReason
{
    Click,
    ShiftClick,
    Drag,
    Keyboard,
    SelectAll,
    Programmatic
}

// New parameter on MariloDataSheet<TItem>
[Parameter]
public EventCallback<DataSheetSelectionChangedArgs<TItem>> OnSelectionChanged { get; set; }

// New read-only property
public DataSheetSelection<TItem>? CurrentSelection { get; private set; }

// New public methods
public Task SelectRangeAsync(TItem anchorRow, string anchorField, TItem extentRow, string extentField);
public Task ClearSelectionAsync(); // sets range to null, keeps active cell
public Task SelectAllAsync();
```

Row-level checkbox selection (`_selectedRows`) remains separate and unchanged — it drives `BulkDeleteAsync` and select-all row operations and is orthogonal to cell ranges.

## Implementation Approach

### Phase A — state model (no UX behavior change)

1. Introduce `DataSheetSelection<TItem>`, `DataSheetRange<TItem>`, `DataSheetSelectionChangedArgs<TItem>`, `SelectionChangeReason` in `src/Marilo.Core/Models/DataSheet/`.
2. Add `_rangeAnchor`, `_rangeExtent` fields alongside existing `_activeCellRow`, `_activeCellField` in `MariloDataSheet.razor.cs`.
3. Add helper `ComputeRangeBounds()` that walks `_displayRows` / `_columns` to produce numeric indices from `(TItem, string)` pairs. Add `BuildCurrentSelection()` that snapshots state into the public model.
4. Raise `OnSelectionChanged` from a central `UpdateSelectionAsync(reason)` method. Every interaction will route through this.
5. Expose `CurrentSelection` read-only property.
6. Add public `SelectRangeAsync`, `ClearSelectionAsync`, `SelectAllAsync`.
7. **No rendering or input changes yet.** Ship as a dead state layer — existing behavior unchanged.
8. Tests: 6 tests covering state transitions and programmatic `SelectRangeAsync`/`ClearSelectionAsync`/`SelectAllAsync`.

### Phase B — keyboard range extension

1. Add a `shift` flag to `HandleKeyDown` (JS passes `event.shiftKey`).
2. In arrow-key branches, when `shift && _rangeAnchor == null`, initialize `_rangeAnchor = (_activeCellRow, _activeCellField)` then move extent by one cell.
3. When `shift && _rangeAnchor != null`, just move extent.
4. When no shift, clear anchor/extent (existing behavior).
5. Add `Ctrl+A` branch: set anchor to `(_displayRows[0], _columns[0].Field)`, extent to `(_displayRows[^1], _columns[^1].Field)`, active to extent.
6. Route all changes through `UpdateSelectionAsync`.
7. Tests: 6 tests covering shift+arrow extend/reduce/wrap, Ctrl+A bounds, non-shift arrow clears.

### Phase C — mouse drag range creation

1. In `.Rendering.cs`, add `@onmousedown`, `@onmousemove`, `@onmouseup` handlers on each `<td>`. Add a `bool _isRangeDragging` state field.
2. `OnCellMouseDown(row, field, shiftKey)`: if shift, keep existing anchor and set extent; else set anchor = extent = cell, set `_isRangeDragging = true`.
3. `OnCellMouseEnter(row, field)`: if `_isRangeDragging`, update extent only (don't commit).
4. `OnCellMouseUp(row, field)`: set `_isRangeDragging = false`. Keep range.
5. Global `onmouseup` listener via JS interop to handle release outside the grid (prevents stuck-drag state).
6. Tests: 4 tests covering drag-create, shift-click-extend, drag-then-release-outside.

### Phase D — range visual highlight

1. Add CSS provider method `DataSheetCellClass` parameter — extend signature to include `bool isInRange`. **Breaking change to provider interface.** Update all 3 providers.
2. In `.Rendering.cs` cell loop, compute `isInRange = CurrentSelection?.Range?.Contains(rowIdx, colIdx) == true` for each cell and pass to the provider.
3. Providers emit `mar-datasheet__cell--in-range` (FluentUI/Material) or `table-info` (Bootstrap).
4. Add SCSS rule in `Marilo.Providers.FluentUI` for subtle blue background on in-range cells.
5. Tests: 3 tests covering CSS class emission at range edges, single-cell range, no-range.

### Phase E — range-aware operations

1. **Copy**: JS side reads `data-raw-value` for each cell in range bounds (from Phase F3 data attribute fix — V04 gap #4) and builds TSV. Range is passed from C# via a new JSInvokable method `GetRangeAsTsv()`.
2. **Fill Down**: Change `FillDown` in `.Editing.cs` to operate on `CurrentSelection?.Range` when present. For each column in range, copy top-row value down to each lower row in range.
3. **Delete**: Change Delete-key branch in `HandleKeyDown` to iterate range bounds when range exists, clearing every editable, non-computed cell.
4. **Paste**: No change needed — existing logic anchors at active cell, which will equal the range anchor in the new model.
5. Tests: 6 tests covering copy-range-tsv, fill-down-in-range, delete-range, paste-range-anchor.

### Phase F — demo + docs

1. Update `samples/Marilo.Demo/Pages/Components/DataSheet/Overview.razor` to demonstrate range selection visually: add a label showing `CurrentSelection` cell count and a "Clear selection" button that calls `ClearSelectionAsync()`.
2. Update spec file `selection-and-ranges.md` to reflect the finalized API surface if it diverges from the current draft.
3. Tests: 2 tests covering the demo page render and selection-label binding.

## Total Effort

- **Phase A:** 0.5 day — state model
- **Phase B:** 0.5 day — keyboard
- **Phase C:** 0.5 day — mouse
- **Phase D:** 0.5 day — rendering (breaking provider change)
- **Phase E:** 1 day — operations rewire
- **Phase F:** 0.5 day — demo + tests polish

**Aggregate:** ~3.5 days of focused work. Estimated **27 bUnit tests** across the 6 phases.

## Risks

1. **Breaking provider signature** (Phase D) — `DataSheetCellClass` signature changes. All 3 providers update in one commit. No external provider implementations known.
2. **Mouse-outside-grid release** (Phase C) — drag state can get stuck if mouseup happens outside the grid. Global JS listener required.
3. **Virtualization interplay** (Phase C/D) — range extent can reference a virtualized row that is not currently rendered. `ComputeRangeBounds` must work on the full `_displayRows` list, not the visible subset. This is already the case today for `_activeCellRow`.
4. **Accessibility** (V07) — ensure screen readers announce range changes via `aria-live`. Tie into the V07.9 `_ariaAnnouncement` fix: on range change, announce e.g. "Selected 3 by 4 cell range".

## Open Design Questions (non-blocking)

1. Does the spec want `Ctrl+Click` for non-contiguous cell selection? **Current call:** No — too complex for the copy/paste/fill semantics. If needed later, store `HashSet<(TItem, string)>` alongside the rectangular range.
2. Should `SelectionChanged` fire during drag (every mousemove) or only on mouseup? **Current call:** Only on mouseup, to avoid event storms. Drag state is internal.
3. Should there be a max range size cap for performance? **Current call:** No cap in Phase A–F. Add later if measurements show problems.

## Dependencies on Other Gaps

- **V04.4** (Copy honors Format via `data-raw-value`) is a prerequisite for Phase E copy-range-as-TSV. Implement V04.4 first or together.
- **V07.9** (`aria-live` population) is where range-change announcements get wired.
- **V07.4** Ctrl+A keyboard hook is covered by Phase B — no duplicate fix needed.

## Handoff

- **Stage 04 remediation plan:** not required — this doc is the plan. Skip to Stage 05.
- **Stage 05 implementer subagent brief:** use Phases A–F in order; dispatch one implementer per phase or bundle A+B+C+D in one agent and E+F in another. Require bUnit tests with each phase.
- **Stage 06 validation:** run full suite; smoke-check demo page in Marilo.Demo; confirm provider builds across FluentUI, Material, Bootstrap.

## Status

**Design complete — ready for implementation.** Deferred from the current loop iteration because total effort (~3.5 days, 27 tests, breaking provider change) exceeds the available budget. Picked up by a future loop iteration or a dedicated implementer session.
