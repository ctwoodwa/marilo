# Implementation Log: JS Interop Batch 2 — DataGrid

## Scope
- DG-P3-01: Frozen/Locked Columns
- DG-P3-03: Row Drag-and-Drop Reorder

## Date: 2026-04-09

## Implementation Details

### DG-P3-01: Frozen/Locked Columns

**New files:**
- `src/Marilo.Components/DataGrid/GridColumnFrozenPosition.cs` — Enum: Start, End

**Modified files:**
- `src/Marilo.Components/DataGrid/MariloGridColumn.razor` — Added `Locked` (bool) and `FrozenPosition` parameters
- `src/Marilo.Components/DataGrid/Sizing/ColumnSizingEntry.cs` — Added Locked, FrozenPosition fields
- `src/Marilo.Components/DataGrid/Sizing/GridLayoutContract.cs` — Added FrozenOffsets, FrozenColumnIds, FrozenPositions
- `src/Marilo.Components/DataGrid/Sizing/FixedWidthProvider.cs` — Computes cumulative sticky offsets; sorts end-frozen by DOM order (descending); 150px default for non-pixel widths
- `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs` — Added GetFrozenCellStyle helper; updated ResolveLayoutContract to pass frozen info
- `src/Marilo.Components/DataGrid/MariloDataGrid.razor` — Applied sticky styles to header, filter row, footer; added mar-datagrid-col--locked class
- `src/Marilo.Components/DataGrid/MariloDataGrid.Rendering.cs` — Applied sticky styles to data rows and edit rows
- `src/Marilo.Components/DataGrid/MariloDataGrid.Interop.cs` — Skip frozen columns from reorder in JS IIFE

**Tests:** 8 bUnit tests in `MariloDataGridFrozenColumnTests.cs`

### DG-P3-03: Row Drag-and-Drop Reorder

**Modified files:**
- `src/Marilo.Components/DataGrid/GridEventArgs.cs` — Added GridRowDropEventArgs<TItem>, GridRowDropPosition enum
- `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs` — Added RowDraggable, OnRowDrop parameters; updated TotalColumnCount
- `src/Marilo.Components/DataGrid/MariloDataGrid.razor` — Drag handle col/th/td placeholders
- `src/Marilo.Components/DataGrid/MariloDataGrid.Rendering.cs` — Drag handle cell in RenderDataRow with role="gridcell", data-row-index; renumbered detail cell sequences
- `src/Marilo.Components/DataGrid/MariloDataGrid.Interop.cs` — initRowDrag() JS function with HTML5 DnD events; OnRowDropped JSInvokable callback with HasDelegate guard; stopPropagation in dragstart

**Tests:** 7 bUnit tests in `MariloDataGridRowDragTests.cs`

## Review Cycle

1. Two parallel implementer subagents
2. Spec+quality combined review found: Critical — end-frozen order assumption, missing role="gridcell", sequence collision; Important — RenderEditRow missing frozen style, missing stopPropagation, missing HasDelegate guard
3. Two parallel fix subagents resolved all issues
4. Final validation: 1083/1083 tests, build clean

## Build & Test
- Build: clean, 0 warnings, 0 errors
- Tests: 1083/1083 passing (15 new tests: 8 frozen + 7 drag-drop)
