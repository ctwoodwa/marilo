# API Design: MariloAllocationScheduler

## Parameters

### MariloAllocationScheduler (Root Component)

| Parameter | Type | Default | Required | Description |
|---|---|---|---|---|
| Resources | `IEnumerable<TResource>` | -- | Yes | The collection of schedulable entities displayed in the resource grid. |
| Allocations | `IEnumerable<AllocationRecord>` | -- | Yes | The collection of allocation records. Each ties a resource, task, time bucket, and value. |
| Targets | `IEnumerable<AllocationTarget>` | `null` | No | Optional desired-total records for delta analysis. |
| AuthoritativeLevel | `TimeGranularity` | -- | Yes | The one granularity at which cells are editable. Coarser levels are read-only rollups. |
| ViewGrain | `TimeGranularity` | same as AuthoritativeLevel | No | Current display granularity. Supports two-way binding. |
| VisibleStart | `DateTime` | `DateTime.Today` | No | Start of the visible date range. Supports two-way binding. |
| VisibleEnd | `DateTime` | derived | No | End of the visible date range. Supports two-way binding. |
| DefaultRangeLength | `int` | `3` | No | Number of units for the default visible range. |
| DefaultRangeUnit | `TimeGranularity` | `Month` | No | Unit for the default visible range length. |
| ValueMode | `AllocationValueMode` | `Hours` | No | Whether cells display hours or currency values. |
| ShowTargets | `bool` | `false` | No | Renders target overlay values alongside actuals. |
| ShowDeltas | `bool` | `false` | No | Renders variance between actuals and targets. |
| DeltaDisplayMode | `DeltaDisplayMode` | `Value` | No | Show deltas as absolute values, percentage, or status icons. |
| AllowDragFill | `bool` | `true` | No | Enables click-and-drag to fill a range of cells. |
| AllowKeyboardEdit | `bool` | `true` | No | Enables Tab/arrow keyboard editing model. |
| AllowBulkEdit | `bool` | `true` | No | Enables range selection and bulk value application. |
| EnableContextMenu | `bool` | `true` | No | Shows the right-click context menu. |
| ContextMenuItems | `IEnumerable<AllocationMenuDescriptor>` | `null` | No | Custom commands appended to the built-in context menu. |
| SelectionMode | `AllocationSelectionMode` | `Range` | No | None, Cell, or Range. |
| DefaultDistributionMode | `DistributionMode` | `EvenSpread` | No | Default policy for distributing higher-level values to sub-buckets. |
| AllowZoomEdit | `bool` | `false` | No | Opt-in: allows direct editing at zoom levels above AuthoritativeLevel. |
| Height | `string` | -- | No | Height CSS value for the root element. |
| Width | `string` | -- | No | Width CSS value for the root element. |
| EnableLoaderContainer | `bool` | `true` | No | Shows loading animation for operations over 600ms. |
| AllocationSets | `IEnumerable<AllocationSet>` | `null` | No | Baselines and scenarios for scenario planning. Null = single-plan mode. |
| ScenarioOverrides | `IEnumerable<ScenarioOverride>` | `null` | No | Delta records for all scenarios. |
| ActiveSetId | `Guid` | baseline id | No | SetId of the currently displayed set. Two-way bindable. |
| CompareSetId | `Guid?` | `null` | No | Enables diff overlay mode against this set. |
| ShowBaselineDiff | `bool` | `false` | No | Renders baseline ghost bars behind active scenario bars. |
| BaselineDateFormat | `string` | `null` | No | Format string for auto-generated baseline labels. |
| ShowComparisonPanel | `bool` | `false` | No | Toggles the scenario comparison panel. |
| ShowCriticalPath | `bool` | `false` | No | Highlights per-scenario critical path. |

### Inherited from MariloComponentBase

| Parameter | Type | Description |
|---|---|---|
| Class | `string?` | Consumer-supplied CSS class to append |
| Style | `string?` | Consumer-supplied inline style to append |
| AdditionalAttributes | `Dictionary<string, object>?` | Unmatched HTML attributes |


### AllocationResourceColumn (Child Component)

| Parameter | Type | Default | Required | Description |
|---|---|---|---|---|
| Field | `string` | -- | Yes | Property name on TResource to display. |
| Title | `string` | -- | Yes | Column header text. |
| Width | `string` | `"auto"` | No | Column width CSS value. |
| Template | `RenderFragment<TResource>` | `null` | No | Custom cell template for this column. |
| HeaderTemplate | `RenderFragment` | `null` | No | Custom header template. |
| Sortable | `bool` | `false` | No | Enable sorting on this column. |
| Filterable | `bool` | `false` | No | Enable filtering on this column. |
| Visible | `bool` | `true` | No | Show/hide this column. |
| Pinned | `bool` | `false` | No | Pin column to left edge during horizontal scroll. |


## RenderFragment Slots

| Slot | Type | On Component | Description |
|---|---|---|---|
| AllocationResourceColumns | `RenderFragment` | MariloAllocationScheduler | Container for AllocationResourceColumn declarations |
| ToolbarTemplate | `RenderFragment` | MariloAllocationScheduler | Optional custom toolbar content |
| EmptyTemplate | `RenderFragment` | MariloAllocationScheduler | Content when no allocations are bound |
| CellTemplate | `RenderFragment<AllocationCellContext>` | MariloAllocationScheduler | Per-cell rendering customization |
| ResourceRowTemplate | `RenderFragment<TResource>` | MariloAllocationScheduler | Custom resource row header rendering |


## Events

| Event | Args Type | Description |
|---|---|---|
| OnCellEdited | `EventCallback<CellEditedArgs>` | Fires when a single cell value is committed. |
| OnRangeEdited | `EventCallback<RangeEditedArgs>` | Fires when a bulk range edit is committed. |
| OnContextMenuAction | `EventCallback<ContextMenuActionArgs>` | Fires when a context menu command is invoked. |
| OnDistributeRequested | `EventCallback<DistributeArgs>` | Fires before distribution writes. Host can override or cancel. |
| OnShiftValues | `EventCallback<ShiftValuesArgs>` | Fires on shift-forward or shift-backward confirmation. |
| OnMoveValues | `EventCallback<MoveValuesArgs>` | Fires on move-to-task or move-to-resource confirmation. |
| OnTargetChanged | `EventCallback<TargetChangedArgs>` | Fires when a desired total is set or updated. |
| OnVisibleRangeChanged | `EventCallback<VisibleRangeChangedArgs>` | Fires on date range navigation. |
| OnSelectionChanged | `EventCallback<SelectionChangedArgs>` | Fires when selected cell or range changes. |
| OnScenarioChanged | `EventCallback<ScenarioChangedArgs>` | Fires when active scenario switches. |
| OnScenarioCreated | `EventCallback<ScenarioCreatedArgs>` | Fires when a new scenario is created. |
| OnAllocationOverridden | `EventCallback<AllocationOverriddenArgs>` | Fires when an edit in a scenario produces an override. |
| OnScenarioStatusChanged | `EventCallback<ScenarioStatusChangedArgs>` | Fires when scenario status changes. |
| OnScenarioPromoted | `EventCallback<ScenarioPromotedArgs>` | Fires when a scenario is promoted to baseline. |
| CanExecuteAction | `EventCallback<CanExecuteActionArgs>` | Called before a context menu action to allow enable/disable logic. |
| ActiveSetIdChanged | `EventCallback<Guid>` | Two-way binding callback for ActiveSetId. |
| ViewGrainChanged | `EventCallback<TimeGranularity>` | Two-way binding callback for ViewGrain. |
| VisibleStartChanged | `EventCallback<DateTime>` | Two-way binding callback for VisibleStart. |


## EventArgs Classes

All in `src/Marilo.Core/Models/AllocationSchedulerModels.cs`:

### CellEditedArgs

| Property | Type | Description |
|---|---|---|
| ResourceKey | `object` | Key of the resource row |
| TaskId | `object` | Identifier of the task |
| BucketStart | `DateTime` | Start of the time bucket |
| BucketEnd | `DateTime` | End of the time bucket |
| OldValue | `decimal` | Previous cell value |
| NewValue | `decimal` | New cell value |
| Record | `AllocationRecord` | The updated allocation record |

### RangeEditedArgs

| Property | Type | Description |
|---|---|---|
| AffectedRecords | `IReadOnlyList<AllocationRecord>` | All records modified in the range edit |
| Value | `decimal` | The value applied to the range |

### ContextMenuActionArgs

| Property | Type | Description |
|---|---|---|
| CommandName | `string` | Name of the invoked command |
| TargetCells | `IReadOnlyList<AllocationCellRef>` | Cells the command applies to |
| IsCancelled | `bool` | Set to true to cancel the action |

### DistributeArgs

| Property | Type | Description |
|---|---|---|
| SourcePeriod | `DateRange` | The higher-level period being distributed |
| TargetValue | `decimal` | The total to distribute |
| TargetGranularity | `TimeGranularity` | The sub-bucket granularity |
| Mode | `DistributionMode` | The distribution policy |
| ProposedDistribution | `IReadOnlyList<AllocationRecord>` | Proposed sub-bucket records |
| IsCancelled | `bool` | Set to true to cancel |

### ShiftValuesArgs

| Property | Type | Description |
|---|---|---|
| ResourceKey | `object` | Resource being shifted |
| TaskId | `object` | Task being shifted |
| Direction | `int` | Positive = forward, negative = backward |
| Periods | `int` | Number of periods to shift |
| AffectedRecords | `IReadOnlyList<AllocationRecord>` | Records that will be shifted |

### MoveValuesArgs

| Property | Type | Description |
|---|---|---|
| SourceResourceKey | `object` | Original resource |
| TargetResourceKey | `object` | Destination resource |
| SourceTaskId | `object` | Original task (null if moving resource-level) |
| TargetTaskId | `object` | Destination task (null if moving resource-level) |
| AffectedRecords | `IReadOnlyList<AllocationRecord>` | Records being moved |

### TargetChangedArgs

| Property | Type | Description |
|---|---|---|
| ResourceKey | `object` | Resource the target applies to |
| TaskId | `object` | Task the target applies to (null for resource-level) |
| Period | `DateRange` | Time period the target covers |
| TargetValue | `decimal` | The desired total value |

### VisibleRangeChangedArgs

| Property | Type | Description |
|---|---|---|
| NewStart | `DateTime` | New visible range start |
| NewEnd | `DateTime` | New visible range end |
| ViewGrain | `TimeGranularity` | Current view granularity |

### SelectionChangedArgs

| Property | Type | Description |
|---|---|---|
| SelectedCells | `IReadOnlyList<AllocationCellRef>` | Currently selected cells |
| SelectionMode | `AllocationSelectionMode` | Current selection mode |

### ScenarioChangedArgs

| Property | Type | Description |
|---|---|---|
| PreviousSetId | `Guid` | Previously active set |
| NewSetId | `Guid` | Newly active set |

### ScenarioCreatedArgs

| Property | Type | Description |
|---|---|---|
| NewSet | `AllocationSet` | The newly created scenario |

### AllocationOverriddenArgs

| Property | Type | Description |
|---|---|---|
| Override | `ScenarioOverride` | The new or updated override record |
| SetId | `Guid` | The scenario set this override belongs to |

### ScenarioStatusChangedArgs

| Property | Type | Description |
|---|---|---|
| SetId | `Guid` | The scenario whose status changed |
| OldStatus | `ScenarioStatus` | Previous status |
| NewStatus | `ScenarioStatus` | New status |

### ScenarioPromotedArgs

| Property | Type | Description |
|---|---|---|
| PromotedSetId | `Guid` | The scenario that was promoted |
| NewBaselineSetId | `Guid` | The new baseline set created |

### CanExecuteActionArgs

| Property | Type | Description |
|---|---|---|
| CommandName | `string` | The command being evaluated |
| TargetCells | `IReadOnlyList<AllocationCellRef>` | Target cells for the command |
| IsEnabled | `bool` | Set to false to disable the command |


## Supporting Models

### AllocationRecord

| Property | Type | Description |
|---|---|---|
| AllocationId | `Guid` | Unique identifier |
| ResourceId | `object` | Foreign key to resource |
| TaskId | `object` | Foreign key to task |
| TaskName | `string` | Display name for the task |
| BucketStart | `DateTime` | Start of the time bucket |
| BucketEnd | `DateTime` | End of the time bucket |
| Value | `decimal` | The numeric value (hours or currency) |
| Unit | `AllocationUnit` | Hours or Currency |

### AllocationTarget

| Property | Type | Description |
|---|---|---|
| TargetId | `Guid` | Unique identifier |
| ResourceId | `object` | Foreign key to resource (null for task-level) |
| TaskId | `object` | Foreign key to task (null for resource-level) |
| PeriodStart | `DateTime` | Start of the target period |
| PeriodEnd | `DateTime` | End of the target period |
| TargetValue | `decimal` | The desired total |

### AllocationSet

| Property | Type | Description |
|---|---|---|
| SetId | `Guid` | Unique identifier |
| DisplayLabel | `string` | Custom label (null = auto-generated for baselines) |
| Name | `string` | User-facing name |
| Type | `AllocationSetType` | Baseline or Scenario |
| ParentBaselineId | `Guid?` | Null for baselines |
| CreatedBy | `string` | Creator identifier |
| CreatedDate | `DateTime` | Creation timestamp |
| FinalizedDate | `DateTime?` | Set when IsLocked = true |
| IsLocked | `bool` | Whether the set is read-only |
| Status | `ScenarioStatus` | Lifecycle status |
| Description | `string` | Description text |

### ScenarioOverride

| Property | Type | Description |
|---|---|---|
| OverrideId | `Guid` | Unique identifier |
| SetId | `Guid` | Owning scenario set |
| OriginalAllocationId | `Guid?` | Null for new additions |
| Override | `AllocationRecord` | Full replacement record |
| IsDeleted | `bool` | Tombstone flag |
| OverrideReason | `string` | Reason for the override |

### AllocationCellRef

| Property | Type | Description |
|---|---|---|
| ResourceKey | `object` | Resource identifier |
| TaskId | `object` | Task identifier |
| BucketStart | `DateTime` | Cell time bucket start |
| BucketEnd | `DateTime` | Cell time bucket end |

### AllocationCellContext

| Property | Type | Description |
|---|---|---|
| Record | `AllocationRecord` | The allocation record for this cell (null if empty) |
| ResourceKey | `object` | Resource identifier |
| BucketStart | `DateTime` | Cell time bucket start |
| BucketEnd | `DateTime` | Cell time bucket end |
| IsEditable | `bool` | Whether the cell is editable at current zoom |
| IsSelected | `bool` | Whether the cell is selected |
| IsConflict | `bool` | Whether the cell has a conflict |

### AllocationMenuDescriptor

| Property | Type | Description |
|---|---|---|
| Name | `string` | Command identifier |
| Text | `string` | Display text |
| Icon | `string` | Icon name |
| IsEnabled | `bool` | Whether the command is enabled |

### DateRange

| Property | Type | Description |
|---|---|---|
| Start | `DateTime` | Range start |
| End | `DateTime` | Range end |


## Enumerations

### New enums (src/Marilo.Core/Enums/AllocationSchedulerEnums.cs)

```csharp
public enum TimeGranularity
{
    Day,
    Week,
    Month,
    Quarter,
    Year
}

public enum AllocationValueMode
{
    Hours,
    Currency
}

public enum DistributionMode
{
    EvenSpread,
    ProportionalToExisting,
    FrontLoaded,
    BackLoaded,
    WorkingDaysWeighted,
    Custom
}

public enum AllocationSelectionMode
{
    None,
    Cell,
    Range
}

public enum DeltaDisplayMode
{
    Value,
    Percentage,
    StatusIcon
}

public enum AllocationUnit
{
    Hours,
    Currency
}
```

### Existing enums (already in BusinessLogicEnums.cs -- do not recreate)

- `ScenarioStatus` (Draft, Shared, Approved, Promoted, Rejected)
- `AllocationSetType` (Baseline, Scenario)


## CSS Provider Methods

Add to `IMariloCssProvider.cs` under the `// -- Scheduling` section:

```csharp
// -- AllocationScheduler
string AllocationSchedulerClass();
string AllocationSchedulerToolbarClass();
string AllocationSchedulerResourceColumnClass(bool isPinned);
string AllocationSchedulerTimeHeaderClass(TimeGranularity grain);
string AllocationSchedulerRowClass(bool isSelected, bool isOverAllocated);
string AllocationSchedulerCellClass(bool isEditable, bool isSelected, bool isConflict, bool isDisabled, bool isDragTarget);
string AllocationSchedulerCellValueClass(AllocationValueMode mode);
string AllocationSchedulerDeltaClass(DeltaDisplayMode mode, bool isOver, bool isUnder);
string AllocationSchedulerScenarioStripClass();
string AllocationSchedulerScenarioChipClass(bool isActive, bool isLocked);
string AllocationSchedulerGhostBarClass();
string AllocationSchedulerContextMenuClass();
string AllocationSchedulerEmptyClass();
string AllocationSchedulerLoaderClass();
```

### CSS Class Naming (BEM with mar- prefix)

| Method | Primary classes |
|---|---|
| AllocationSchedulerClass | `mar-allocation-scheduler` |
| AllocationSchedulerToolbarClass | `mar-allocation-scheduler__toolbar` |
| AllocationSchedulerResourceColumnClass | `mar-allocation-scheduler__resource-col` `--pinned` |
| AllocationSchedulerTimeHeaderClass | `mar-allocation-scheduler__time-header` `--[grain]` |
| AllocationSchedulerRowClass | `mar-allocation-scheduler__row` `--selected` `--over-allocated` |
| AllocationSchedulerCellClass | `mar-allocation-scheduler__cell` `--editable` `--selected` `--conflict` `--disabled` `--drag-target` |
| AllocationSchedulerCellValueClass | `mar-allocation-scheduler__cell-value` `--hours` `--currency` |
| AllocationSchedulerDeltaClass | `mar-allocation-scheduler__delta` `--over` `--under` |
| AllocationSchedulerScenarioStripClass | `mar-allocation-scheduler__scenario-strip` |
| AllocationSchedulerScenarioChipClass | `mar-allocation-scheduler__scenario-chip` `--active` `--locked` |
| AllocationSchedulerGhostBarClass | `mar-allocation-scheduler__ghost-bar` |
| AllocationSchedulerContextMenuClass | `mar-allocation-scheduler__context-menu` |
| AllocationSchedulerEmptyClass | `mar-allocation-scheduler__empty` |
| AllocationSchedulerLoaderClass | `mar-allocation-scheduler__loader` |


## Public Methods (via @ref)

| Method | Return | Description |
|---|---|---|
| Rebind() | `Task` | Re-reads Resources and Allocations |
| Refresh() | `Task` | Re-renders without re-reading data |
| NavigateTo(DateTime date) | `Task` | Moves visible range so date is in view |
| NavigateForward() | `Task` | Advances by one ViewGrain unit |
| NavigateBack() | `Task` | Moves back by one ViewGrain unit |
| NavigateToToday() | `Task` | Centers on today |
| GetSelectedCells() | `IReadOnlyList<AllocationCellRef>` | Returns current selection |
| ClearSelection() | `Task` | Clears cell selection |


## CascadingValue Relationships

- `MariloAllocationScheduler` provides itself as a `CascadingValue` to child `AllocationResourceColumn` components
- `AllocationResourceColumn` uses `[CascadingParameter]` to discover and register with its parent
