---
title: Splitter and Dual-Pane Layout
page_title: AllocationScheduler Splitter Layout
description: Specification for the draggable vertical splitter between the resource grid and timeline panes in the AllocationScheduler component, including collapse/restore behavior, keyboard accessibility, theming, and programmatic control.
slug: allocation-scheduler-splitter-layout
tags: marilo,blazor,allocation-scheduler,splitter,layout
published: True
position: 2
components: ["allocation-scheduler"]
---

# AllocationScheduler — Splitter and Dual-Pane Layout

## Overview

The `MariloAllocationScheduler` uses a dual-pane layout separated by a draggable vertical splitter. The **left pane** contains the resource grid — the `<AllocationResourceColumns>` that display resource identity, role, department, and other metadata fields. The **right pane** contains the navigable timeline surface — time-bucket columns, allocation values, and visual overlays.

The splitter is the boundary between these two regions. Users drag the splitter horizontally to redistribute space between the resource grid and the timeline. This interaction model is inspired by the **Microsoft Project Gantt Chart** view, where a vertical divider separates the left task-sheet table from the right Gantt chart. Users can drag the divider to give more room to columns or to the chart, and can drag it fully to one side to effectively hide either pane.

The splitter is a first-class interactive element: it is keyboard-operable, ARIA-annotated, and themeable. It participates in the component's focus order and fires events when the user changes its position or collapses a pane.


## Default Layout

The AllocationScheduler renders with the following default layout dimensions:

| Dimension | Default | Rationale |
| --- | --- | --- |
| Left pane width | `320px` | Accommodates a typical three-column resource grid (Name + Role + Department) without truncation. |
| Left pane minimum | `200px` | Ensures at least the primary resource name column remains readable. |
| Right pane minimum | `300px` | Guarantees at least a few time-bucket columns are visible for orientation. |
| Splitter handle visible width | `8px` | Wide enough to see and target with a mouse, narrow enough to not waste horizontal space. |
| Splitter handle pointer hit area | `24px` | Extends the clickable zone beyond the visible handle for easier targeting, especially on touch devices. |

On first render, the left-pane width is set to the value of `SplitterPosition` if bound, or `DefaultSplitterPosition` if `SplitterPosition` is `null`. The right pane fills the remaining component width minus the splitter handle width.


## Parameters

| Parameter | Type | Default | Description |
| --- | --- | --- | --- |
| `SplitterPosition` | `double?` | `null` | The left-pane width in pixels. Supports two-way binding. When `null`, `DefaultSplitterPosition` is used on first render. |
| `DefaultSplitterPosition` | `double` | `320` | The initial left-pane width in pixels used when `SplitterPosition` is `null`. |
| `SplitterPositionChanged` | `EventCallback<double>` | — | Fires when the user finishes dragging the splitter. Carries the new left-pane width in pixels. |
| `MinLeftPaneWidth` | `double` | `200` | Minimum width of the left resource-grid pane in pixels. |
| `MinRightPaneWidth` | `double` | `300` | Minimum width of the right timeline pane in pixels. |
| `AllowSplitterCollapse` | `bool` | `false` | When `true`, the user can drag the splitter past `MinRightPaneWidth` or `MinLeftPaneWidth` to fully collapse either pane. A restore affordance appears when a pane is collapsed. |
| `SplitterCssClass` | `string` | `null` | Optional CSS class added to the splitter handle element for custom styling. |


## Collapse and Restore Behavior

When `AllowSplitterCollapse` is `true`, the splitter supports full pane collapse in both directions.

### Collapsing the Timeline (Right Pane)

When the user drags the splitter past the point where the right pane would be narrower than `MinRightPaneWidth`, the timeline pane **snaps fully closed**. The left resource grid expands to fill the full component width minus a narrow restore zone.

A **restore zone** remains visible at the right edge of the component:

- The restore zone is at least **24px wide** and spans the full component height.
- It renders a visual collapse indicator (a chevron or grip icon pointing left, indicating the collapsed pane direction).
- Clicking the restore zone, or pressing **Enter** or **Space** while it is focused, re-opens the timeline pane.
- The timeline restores to the last non-collapsed `SplitterPosition`. If no prior position exists, it restores to `DefaultSplitterPosition`.
- `SplitterPositionChanged` fires with the restored width.
- `OnSplitterRestored` fires with the restored width.

### Collapsing the Resource Grid (Left Pane)

Symmetric behavior applies when the user drags the splitter past `MinLeftPaneWidth` to the left:

- The left resource grid snaps fully closed.
- A restore zone (at least 24px wide, full height) appears at the left edge of the component.
- Clicking or pressing Enter/Space on the restore zone re-opens the resource grid to its last non-collapsed position, or to `DefaultSplitterPosition`.
- `SplitterPositionChanged` and `OnSplitterRestored` fire with the restored width.

### Collapse State Tracking

When a pane collapses, `OnSplitterCollapsed` fires with a `SplitterSide` value indicating which pane was collapsed. The component internally stores the last non-collapsed position so that restore operations return to a usable state.

When `AllowSplitterCollapse` is `false` (the default), dragging is clamped to the range `[MinLeftPaneWidth, componentWidth - MinRightPaneWidth]` and collapse is not possible.


## Programmatic Control

Obtain a reference with `@ref` to call splitter methods programmatically.

| Method | Return Type | Description |
| --- | --- | --- |
| `SetSplitterPosition(double widthPx)` | `Task` | Moves the splitter to the given left-pane pixel width, clamped to `[MinLeftPaneWidth, componentWidth - MinRightPaneWidth]`. Fires `SplitterPositionChanged`. |
| `CollapseSplitter(SplitterSide side)` | `Task` | Fully collapses the specified pane. Requires `AllowSplitterCollapse` to be `true`; throws `InvalidOperationException` otherwise. Fires `OnSplitterCollapsed`. |
| `RestoreSplitter()` | `Task` | Restores the last collapsed pane to its prior non-collapsed position, or to `DefaultSplitterPosition` if no prior position exists. Fires `OnSplitterRestored` and `SplitterPositionChanged`. |


## Events

| Event | Args Type | Description |
| --- | --- | --- |
| `SplitterPositionChanged` | `EventCallback<double>` | Fires when the user finishes dragging the splitter. Carries the new left-pane width in pixels. Also fires after programmatic position changes via `SetSplitterPosition` or `RestoreSplitter`. |
| `OnSplitterCollapsed` | `EventCallback<SplitterSide>` | Fires when either pane is fully collapsed via drag or programmatic `CollapseSplitter`. Carries which side was collapsed. |
| `OnSplitterRestored` | `EventCallback<double>` | Fires when a collapsed pane is restored via the restore zone or programmatic `RestoreSplitter`. Carries the restored left-pane width in pixels. |


## Keyboard and Accessibility

The splitter handle is a keyboard-operable control that meets WCAG 2.1 Level AA requirements.

### ARIA Attributes

| Attribute | Value |
| --- | --- |
| `role` | `separator` |
| `aria-orientation` | `vertical` |
| `aria-valuenow` | Current left-pane width in pixels |
| `aria-valuemin` | `MinLeftPaneWidth` value |
| `aria-valuemax` | Component width minus `MinRightPaneWidth` |
| `aria-label` | `Resize resource columns` |

### Keyboard Bindings

| Key | Action |
| --- | --- |
| **Arrow Left** | Decreases left-pane width by 16px |
| **Arrow Right** | Increases left-pane width by 16px |
| **Shift + Arrow Left** | Decreases left-pane width by 64px |
| **Shift + Arrow Right** | Increases left-pane width by 64px |
| **Home** | Snaps left pane to `MinLeftPaneWidth` |
| **End** | Snaps left pane to component width minus `MinRightPaneWidth` |
| **Enter** | Collapses or restores a pane (when `AllowSplitterCollapse` is `true`). If not collapsed, collapses the right pane. If collapsed, restores to prior position. |

All keyboard adjustments are clamped to the valid range `[MinLeftPaneWidth, componentWidth - MinRightPaneWidth]` unless `AllowSplitterCollapse` is `true`, in which case Home and End can trigger full collapse.

### Focus

- The splitter handle is included in the natural Tab order of the component.
- A visible focus ring appears when the splitter receives keyboard focus, using the Marilo `--focus-ring` CSS custom property or a 2px outline in the theme primary color.
- The focus indicator has sufficient contrast against both the splitter background and adjacent pane backgrounds.


## Theming

The splitter appearance is controlled via CSS custom properties, consistent with the AllocationScheduler theming model.

| CSS Custom Property | Default | Description |
| --- | --- | --- |
| `--allocation-scheduler-splitter-width` | `8px` | Visible width of the splitter handle. |
| `--allocation-scheduler-splitter-background` | `var(--marilo-color-border)` | Background color of the splitter handle in its rest state. |
| `--allocation-scheduler-splitter-hover-background` | `var(--marilo-color-primary)` | Background color when the pointer hovers over the splitter. |
| `--allocation-scheduler-splitter-active-background` | `var(--marilo-color-primary)` | Background color while the splitter is being dragged. |
| `--allocation-scheduler-splitter-cursor` | `col-resize` | Cursor displayed when hovering over the splitter handle. |
| `--allocation-scheduler-splitter-collapse-indicator-background` | `var(--marilo-color-subtle-background)` | Background of the collapse/restore indicator zone. |
| `--allocation-scheduler-splitter-collapse-indicator-icon-color` | `var(--marilo-color-primary)` | Color of the chevron or grip icon in the collapse/restore indicator. |

### BEM Class Names

```
mar-allocation-scheduler__splitter                           (element)
mar-allocation-scheduler__splitter--dragging                 (modifier)
mar-allocation-scheduler__splitter--focused                  (modifier)
mar-allocation-scheduler__splitter-restore                   (element)
mar-allocation-scheduler__splitter-restore--left             (modifier)
mar-allocation-scheduler__splitter-restore--right            (modifier)
mar-allocation-scheduler__pane--left                         (element)
mar-allocation-scheduler__pane--right                        (element)
mar-allocation-scheduler__pane--collapsed                    (modifier)
```


## SplitterSide Enumeration

The `SplitterSide` enum identifies which pane is the target of a collapse or restore operation.

```csharp
public enum SplitterSide
{
    Left,
    Right
}
```

This enum is used by `CollapseSplitter(SplitterSide side)` and `OnSplitterCollapsed`.


## Demo Scenarios

The following scenarios represent the primary coverage targets for splitter-related examples and tests.

1. **Drag resize** — Start with default widths (`DefaultSplitterPosition = 320`). Drag the splitter to the right to give the resource grid more space for a long Name column. Confirm that `SplitterPositionChanged` fires with the new width and that both panes re-render correctly.

2. **Collapse and restore** — Set `AllowSplitterCollapse="true"`. Drag the splitter all the way to the right to collapse the timeline pane. Confirm the restore zone is visible at the right edge. Click the restore zone and confirm the timeline re-opens to the prior width. Verify that `OnSplitterCollapsed` fires on collapse and `OnSplitterRestored` fires on restore.

3. **Programmatic control** — Add a button that calls `SetSplitterPosition(480)`. Click the button and confirm the left pane widens to 480px, revealing additional resource columns. Confirm that `SplitterPositionChanged` fires with `480`.
