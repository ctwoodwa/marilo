---
title: Theming
page_title: AllocationScheduler Theming
description: CSS provider methods, CSS variable list, and theming customization for the AllocationScheduler component.
slug: allocation-scheduler-theming
tags: marilo,blazor,allocation-scheduler,theming,css,fluent-ui,bootstrap
published: True
position: 7
components: ["allocation-scheduler"]
---

# AllocationScheduler Theming

## CSS Provider Methods

The AllocationScheduler uses 14 CSS provider methods. Both FluentUI and Bootstrap providers implement all methods.

| Method | Parameters | Returns |
|---|---|---|
| AllocationSchedulerClass | none | Root container class |
| AllocationSchedulerToolbarClass | none | Toolbar container |
| AllocationSchedulerResourceColumnClass | isPinned | Resource column cell |
| AllocationSchedulerTimeHeaderClass | grain | Time bucket header |
| AllocationSchedulerRowClass | isSelected, isOverAllocated | Resource row |
| AllocationSchedulerCellClass | isEditable, isSelected, isConflict, isDisabled, isDragTarget | Allocation cell |
| AllocationSchedulerCellValueClass | mode | Cell value display |
| AllocationSchedulerDeltaClass | mode, isOver, isUnder | Delta indicator |
| AllocationSchedulerScenarioStripClass | none | Scenario strip container |
| AllocationSchedulerScenarioChipClass | isActive, isLocked | Scenario chip |
| AllocationSchedulerGhostBarClass | none | Baseline ghost bar |
| AllocationSchedulerContextMenuClass | none | Context menu popup |
| AllocationSchedulerEmptyClass | none | Empty state container |
| AllocationSchedulerLoaderClass | none | Loading state container |

## BEM Class Names

All classes follow the `mar-allocation-scheduler` BEM block prefix.

```
mar-allocation-scheduler                         (block)
mar-allocation-scheduler__cell                   (element)
mar-allocation-scheduler__cell--selected         (modifier)
mar-allocation-scheduler__cell--conflict         (modifier)
mar-allocation-scheduler__row--over-allocated    (modifier)
mar-allocation-scheduler__scenario-chip--active   (modifier)
```

## FluentUI Design Tokens

| Token | Usage |
|---|---|
| `--marilo-color-primary` | Selected cell, active scenario chip, drag target |
| `--marilo-color-danger` | Over-allocated rows, conflict cells |
| `--marilo-color-warning` | Under-allocation indicators |
| `--marilo-color-success` | Currency value color |
| `--marilo-color-border` | Cell and row separators |
| `--marilo-color-surface` | Cell backgrounds |
| `--marilo-color-subtle-background` | Header and toolbar backgrounds |

## Bootstrap Mapping

| Marilo class | Bootstrap equivalent |
|---|---|
| Selected row | `table-active` |
| Over-allocated row | `table-danger` |
| Selected cell | `table-primary` |
| Conflict cell | `table-danger` |
| Disabled cell | `text-muted bg-light` |
| Scenario chip | `badge rounded-pill bg-primary`/`bg-secondary` |
| Over delta | `text-danger` |
| Under delta | `text-warning` |
