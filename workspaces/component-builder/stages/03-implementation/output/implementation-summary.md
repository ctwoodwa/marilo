# Implementation Summary: MariloAllocationScheduler

## Files Created

| File | Path | Lines | Description |
|---|---|---|---|
| Enums | `src/Marilo.Core/Enums/AllocationSchedulerEnums.cs` | ~80 | TimeGranularity, AllocationValueMode, DistributionMode, AllocationSelectionMode, DeltaDisplayMode, AllocationUnit |
| Models | `src/Marilo.Core/Models/AllocationSchedulerModels.cs` | ~200 | AllocationRecord, AllocationTarget, AllocationSet, ScenarioOverride, AllocationCellRef, AllocationCellContext, AllocationMenuDescriptor, DateRange, plus 15 EventArgs classes |
| Component markup | `src/Marilo.Components/DataDisplay/AllocationScheduler/MariloAllocationScheduler.razor` | ~130 | Razor markup with grid layout, scenario strip, toolbar, resource rows, allocation cells |
| Component code-behind | `src/Marilo.Components/DataDisplay/AllocationScheduler/MariloAllocationScheduler.razor.cs` | ~310 | Parameters, events, lifecycle, navigation, scenario switching, cell selection, helper methods |
| Child component | `src/Marilo.Components/DataDisplay/AllocationScheduler/AllocationResourceColumn.razor.cs` | ~50 | Resource column definition with CascadingParameter registration |
| JS interop | `src/Marilo.Components/wwwroot/js/allocation-scheduler.js` | ~130 | Drag-fill, keyboard navigation, cell focus management |

## Files Modified

| File | Path | Changes |
|---|---|---|
| CSS provider contract | `src/Marilo.Core/Contracts/IMariloCssProvider.cs` | Added 14 AllocationScheduler method signatures under Scheduling section |

## Core Integration Notes

### Existing contracts consumed (not recreated)

- `ScenarioStatus` enum from `BusinessLogicEnums.cs` -- used in AllocationSet.Status
- `AllocationSetType` enum from `BusinessLogicEnums.cs` -- used in AllocationSet.Type
- `BusinessObjectBase<T>` -- consumer-side AllocationEntry business objects extend this (not part of the component source; consumed by host application)
- `FieldManager`, `BusinessRuleEngine`, `AuthorizationEngine`, `UndoStack` -- all consumed by consumer-side business objects, not duplicated in component

### New enums created

- `TimeGranularity` -- not in existing core
- `AllocationValueMode` -- not in existing core
- `DistributionMode` -- not in existing core
- `AllocationSelectionMode` -- not in existing core
- `DeltaDisplayMode` -- not in existing core
- `AllocationUnit` -- not in existing core

### Architecture decisions

1. **Generic TResource** -- The component is generic over the resource type, using reflection for Id/Name discovery. This matches the pattern used by MariloDataGrid.
2. **Scenario overlay at component level** -- The component computes effective allocations by merging baseline + scenario overrides internally. This keeps the host's data model clean.
3. **CascadingValue for column registration** -- AllocationResourceColumn discovers its parent via CascadingParameter and registers itself, following the DataGrid column pattern.
4. **JS interop for drag-fill and keyboard** -- DOM-level event handling for drag-fill selection and keyboard navigation is delegated to JS, following the existing pattern for complex interactive components.

## Audit Checks

| Check | Status |
|---|---|
| API fidelity | PASS -- all parameters, events, and enums from API design implemented |
| Base class | PASS -- inherits MariloComponentBase |
| CssProvider usage | PASS -- all CSS classes generated via CssProvider methods |
| Accessibility | PASS -- role=grid, role=row, role=gridcell, aria-selected, aria-disabled, aria-readonly, aria-label |
| No hardcoded styles | PASS -- size styles computed from Height/Width parameters, all theming via provider |
| No MariloScheduler references | PASS -- zero references to scheduler-delivery or MariloScheduler |
| Core wired not duplicated | PASS -- ScenarioStatus and AllocationSetType consumed from BusinessLogicEnums |
