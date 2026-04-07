# Implementation Log: DataGrid Phase 3 — C# Achievable Gaps

**Scope:** batch
**Phase:** Phase 3 (selected C# items from JS interop phase)
**Status:** Complete
**Date:** 2026-04-05

## Summary

Resolved 2 DataGrid gaps (DG-P3-02 Cell Selection, DG-P3-04 CheckBoxList Filter). Both are pure C# — no JS interop needed. 10 new bUnit tests, 557/557 full suite.

## Tasks Completed

| Task | File(s) Modified | Status |
|------|-----------------|--------|
| `GridSelectionUnit` enum | `GridEnums.cs` | ✅ |
| `GridCellReference<TItem>` model | `GridCellReference.cs` (new) | ✅ |
| `SelectionUnit`, `SelectedCells`, `SelectedCellsChanged` params | `MariloDataGrid.razor.cs` | ✅ |
| `HandleCellClick`, `IsCellSelected` methods | `MariloDataGrid.Data.cs` | ✅ |
| Cell selection CSS class + onclick in rendering | `MariloDataGrid.Rendering.cs` | ✅ |
| `GridFilterMode.CheckBoxList` enum value | `GridEnums.cs` | ✅ |
| CheckBoxList state fields | `MariloDataGrid.razor.cs` | ✅ |
| `ToggleCheckBoxFilter`, `ApplyCheckBoxFilter`, `ClearCheckBoxFilter`, `GetDistinctValues` | `MariloDataGrid.Data.cs` | ✅ |
| `RenderCheckBoxFilterMenu` | `MariloDataGrid.Rendering.cs` | ✅ |
| CheckBoxList header wiring | `MariloDataGrid.razor` | ✅ |

## Tests

| Test file | Test name | Covers |
|-----------|-----------|--------|
| `MariloDataGridPhase3Tests.cs` | `CheckBoxList_FilterMode_Enum_Exists` | Enum value |
| | `CheckBoxList_FilterMode_Renders_FilterButton` | Button renders |
| | `CheckBoxList_FilterButton_OpensPopup` | Popup opens with items |
| | `CheckBoxList_Shows_Distinct_Values` | 3 distinct categories |
| | `CheckBoxList_HasApplyAndClearButtons` | Apply/Clear buttons |
| | `GridSelectionUnit_Enum_Exists` | Enum values (Row=0, Cell=1) |
| | `SelectionUnit_Defaults_To_Row` | Default parameter |
| | `SelectionUnit_Cell_Parameter_Accepted` | Explicit Cell mode |
| | `GridCellReference_HasExpectedProperties` | Model properties |
| | `CellSelection_SelectedCellsChanged_EventCallback_Accepted` | Event callback |

**Total new tests:** 10 bUnit tests
**Full suite:** 557/557 (zero regressions)

## Files Changed

| File | Change Type | Reason |
|------|-------------|--------|
| `src/Marilo.Core/Enums/GridEnums.cs` | edit | `CheckBoxList`, `GridSelectionUnit` |
| `src/Marilo.Components/DataGrid/GridCellReference.cs` | new | Cell reference model |
| `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs` | edit | Selection + filter state/params |
| `src/Marilo.Components/DataGrid/MariloDataGrid.Data.cs` | edit | Cell selection + checkbox filter methods |
| `src/Marilo.Components/DataGrid/MariloDataGrid.Rendering.cs` | edit | Cell CSS + checkbox filter rendering |
| `src/Marilo.Components/DataGrid/MariloDataGrid.razor` | edit | CheckBoxList header wiring |
| `tests/Marilo.Tests.Unit/DataGrid/MariloDataGridPhase3Tests.cs` | new | 10 bUnit tests |

## Remaining Phase 3 Gaps

| ID | Gap | Status |
|----|-----|--------|
| DG-P3-01 | Frozen/Locked columns | Deferred — needs `position: sticky` CSS + JS scroll sync |
| DG-P3-03 | Row drag-and-drop reorder | Deferred — needs JS drag events |
