# Test Summary: MariloAllocationScheduler

## Test Files

| File | Path | Tests |
|---|---|---|
| MariloAllocationSchedulerTests.cs | `tests/Marilo.Tests.Unit/AllocationScheduler/MariloAllocationSchedulerTests.cs` | 18 |

## Test Results

```
Passed!  - Failed: 0, Passed: 18, Skipped: 0, Total: 18, Duration: 404 ms
```

## Test Coverage

### Rendering (3 tests)
- Renders_Resource_Rows_For_Each_Resource -- PASS
- Renders_Allocation_Items_In_Correct_Slot_Positions -- PASS
- Renders_Empty_State_When_Allocations_Empty -- PASS

### Conflict Detection (2 tests)
- ShowConflicts_Applies_Conflict_CSS_Class_To_Overlapping_Items -- PASS
- No_Conflict_Class_When_No_Overlapping_Allocations -- PASS

### Interaction (3 tests)
- AllowDragFill_False_Does_Not_Set_Drag_Classes -- PASS
- Cells_Not_Editable_When_ViewGrain_Coarser_Than_AuthoritativeLevel -- PASS
- OnCellEdited_Fires_When_Allocation_Programmatically_Added -- PASS

### Accessibility (5 tests)
- Outer_Element_Has_Role_Grid -- PASS
- Resource_Rows_Have_Role_Row -- PASS
- Slot_Cells_Have_Role_Gridcell -- PASS
- Cells_Have_Aria_Selected_Attribute -- PASS
- Header_Cells_Have_Role_Columnheader -- PASS

### Templates (1 test)
- ResourceTemplate_Renders_Custom_Content -- PASS

### CSS Provider (3 tests)
- AllocationSchedulerClass_Called_On_Render -- PASS
- AllocationSchedulerCellClass_Called_For_Each_Cell -- PASS
- AllocationSchedulerRowClass_Called_For_Each_Row -- PASS

### Toolbar (1 test)
- Toolbar_Renders_Navigation_Buttons -- PASS

## Audit Checks

| Check | Status |
|---|---|
| Render test | PASS -- default rendering works |
| Parameter coverage | PASS -- key parameters tested (Resources, Allocations, ViewGrain, AuthoritativeLevel, AllowDragFill, ValueMode, SelectionMode) |
| Event coverage | PASS -- OnCellEdited wiring verified |
| Provider coverage | PASS -- FluentUI provider classes verified in markup |
| Tests pass | PASS -- 18/18, 0 failures |
| No Telerik dependencies | PASS -- bUnit + xUnit only |
| No MariloScheduler references | PASS |
