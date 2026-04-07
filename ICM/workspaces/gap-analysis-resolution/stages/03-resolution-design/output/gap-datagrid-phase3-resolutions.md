# Resolution Records: MariloDataGrid Phase 3 — C# Achievable Gaps

> Date: 2026-04-05
> Source: `stages/02-prioritize/output/gap-datagrid-backlog.md` Phase 3
> Component: `MariloDataGrid<TItem>` — `src/Marilo.Components/DataGrid/`

---

## RES-DG-P3-02: Cell selection mode

**Resolves:** DG-P3-02
**Status:** Implemented

### Target Pattern

Add `GridSelectionUnit` enum (Row, Cell) and `SelectionUnit` parameter. When `Cell`, clicking a cell selects it (with CSS highlight), and `SelectedCellsChanged` fires with `GridCellReference<TItem>` objects. Supports Single and Multiple modes.

### Success Criteria
- [x] `GridSelectionUnit` enum with Row and Cell values
- [x] `SelectionUnit` parameter defaults to Row
- [x] `SelectedCells` / `SelectedCellsChanged` EventCallback
- [x] `GridCellReference<TItem>` model with Item, Field, Value, RowIndex
- [x] Cell click handler respects Single/Multiple selection mode

---

## RES-DG-P3-04: CheckBoxList filter mode

**Resolves:** DG-P3-04
**Status:** Implemented

### Target Pattern

Add `GridFilterMode.CheckBoxList` enum value. When active, column headers show a filter button that opens a checkbox list of distinct values extracted from `Data`. Users select/deselect values, then Apply creates a composite OR filter. Select All / None quick actions.

### Decision

Uses existing `CompositeFilterDescriptor` with `FilterCompositionOperator.Or` — no new filter infrastructure needed. `GetDistinctValues` uses reflection on `Data` (client-side only; server-side `OnRead` needs separate distinct-values endpoint).

### Success Criteria
- [x] `GridFilterMode.CheckBoxList` enum value
- [x] Filter button with funnel icon in column headers
- [x] Popup shows checkbox list of distinct values (sorted, deduplicated)
- [x] Select All / None quick actions
- [x] Apply creates composite OR filter; Clear removes it
- [x] Active filter shown via button highlight
