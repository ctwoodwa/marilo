---
title: Events
page_title: AllocationScheduler Events
description: All EventCallback parameters for the AllocationScheduler with payload descriptions and usage examples.
slug: allocation-scheduler-events
tags: marilo,blazor,allocation-scheduler,events,callbacks
published: True
position: 3
components: ["allocation-scheduler"]
---

# AllocationScheduler Events

## Editing Events

### OnCellEdited

Fires when a single cell value is committed.

```razor
<MariloAllocationScheduler ... OnCellEdited="@HandleEdit">

@code {
    private async Task HandleEdit(CellEditedArgs args)
    {
        // args.ResourceKey, args.TaskId, args.BucketStart
        // args.OldValue, args.NewValue, args.Record
        await SaveAsync(args.Record);
    }
}
```

### OnRangeEdited

Fires when a bulk range edit is committed (multiple cells at once).

| Property | Type |
|---|---|
| AffectedRecords | `IReadOnlyList<AllocationRecord>` |
| Value | `decimal` |

### OnDistributeRequested

Fires before a distribution command writes. The host can inspect proposed records, override values, or cancel.

| Property | Type |
|---|---|
| SourcePeriod | `DateRange` |
| TargetValue | `decimal` |
| TargetGranularity | `TimeGranularity` |
| Mode | `DistributionMode` |
| ProposedDistribution | `IReadOnlyList<AllocationRecord>` |
| IsCancelled | `bool` |

## Navigation Events

### OnVisibleRangeChanged

Fires when the user navigates to a different date range.

### OnSelectionChanged

Fires when the selected cell or range changes.

## Context Menu Events

### OnContextMenuAction

Fires when a built-in or custom context menu command is invoked.

### CanExecuteAction

Called before a context menu action is shown to allow enable/disable logic. Set `IsEnabled = false` to grey out the command.

## Scenario Events

### OnScenarioChanged

Fires when the user switches the active scenario.

### OnScenarioCreated

Fires when a new scenario is created from the Scenario Strip.

### OnAllocationOverridden

Fires when an edit in a scenario produces a new or updated ScenarioOverride.

### OnScenarioStatusChanged

Fires when a scenario's status transitions (e.g., Draft to Shared).

### OnScenarioPromoted

Fires when a scenario is promoted to become the new locked baseline.
