# DockManager Gap Inventory -- Stage 01 (Intake)

**Date:** 2026-04-12
**Worker:** w-dockmanager-gap-analysis
**Component:** MariloDockManager
**Source:** Delivery audit (`dockmanager-spec-gaps.md`, `dockmanager-delivery-report.md`)
**Architecture Decision:** Full spec commitment (human-approved). All gaps are real implementation targets.

## Current State

The source is a **minimal tabbed-panel skeleton** (`MariloDockManager.razor` + `MariloDockPane.razor`) covering ~10-15% of the specified API surface. It provides:
- A flat list of `MariloDockPane` children rendered as tabs
- Simple `OnPaneClose`, `OnPanePin`, `OnPaneFloat` string callbacks
- Basic `Height`/`Width` parameters
- `Closable` parameter on panes
- CascadingValue-based pane registration

The spec describes a full dock manager with nested pane hierarchy, split panes, tab groups, floating windows, drag-and-drop docking, state management, pin/unpin behavior, and a rich typed event model.

---

## Gap Inventory

### Critical Gaps

| ID | Description | Spec Reference | Type | Complexity | Notes |
|----|-------------|---------------|------|-----------|-------|
| GAP-01 | **Pane hierarchy model missing.** Spec defines `DockManagerContentPane`, `DockManagerSplitPane`, `DockManagerTabGroupPane` as distinct child components with nesting rules. Source has only flat `MariloDockPane`. | overview.md, pane-types.md | Spec-Ahead | XL | Foundation for everything. Requires new component classes, tree data structure, nesting validation. |
| GAP-02 | **Split pane component missing.** Spec describes `DockManagerSplitPane` with `Orientation`, `Size`, `AllowEmpty`, splitter-based horizontal/vertical resizable splits. Source has no split pane at all. | pane-types.md, overview.md | Spec-Ahead | XL | Requires splitter interop, resize handles, orientation logic. Depends on GAP-01. |
| GAP-03 | **Tab group pane component missing.** Spec describes `DockManagerTabGroupPane` with `SelectedPaneId`, `AllowEmpty`. Source has a single flat tab strip with no nested tab groups. | pane-types.md, overview.md | Spec-Ahead | L | Tab strip within pane hierarchy. Depends on GAP-01. |
| GAP-04 | **Floating panes missing.** Spec describes `<DockManagerFloatingPanes>` container with floating window behavior (`FloatingHeight`, `FloatingLeft`, `FloatingTop`, `FloatingWidth`, `FloatingResizable`). Source has no floating window support. | overview.md | Spec-Ahead | XL | Requires JS interop for window management, drag positioning, resize. |
| GAP-05 | **Drag-and-drop docking missing.** Spec describes global docking navigator and inner docking with drag-and-drop. Source has no drag-and-drop, no dock navigator, no `Dockable` parameter enforcement. | docking-types.md | Spec-Ahead | XL | Heaviest JS interop requirement. Global + inner dock navigators, hit testing, drop zones. |
| GAP-06 | **State management missing.** Spec describes `DockManagerState` object with `GetState`/`SetState`/`SetStateAsync` methods plus `OnStateInit`/`OnStateChanged` events for layout persistence. Source has no state management. | state.md | Spec-Ahead | L | Requires serializable state model reflecting full pane tree. Depends on GAP-01. |
| GAP-07 | **Event model mismatch.** Spec defines typed event args (`DockManagerDockEventArgs`, `DockManagerUndockEventArgs`, `DockManagerPinEventArgs`, `DockManagerUnpinEventArgs`, `DockManagerPaneResizeEventArgs`) with `IsCancelled` support. Source has only `EventCallback<string>` (id-only, no cancellation). | events.md | Mismatch | L | New event arg classes, cancellation pattern, replace existing callbacks. |
| GAP-08 | **Child component naming mismatch.** Spec uses `DockManagerContentPane`, `DockManagerSplitPane`, `DockManagerTabGroupPane`. Source uses `MariloDockPane` (single flat type). | overview.md, pane-types.md | Mismatch | M | Breaking rename; resolved naturally when GAP-01 is implemented. |
| GAP-09 | **Methods missing.** Spec defines `GetState()`, `SetState()`, `SetStateAsync()`, `Refresh()` public methods. Source exposes none. | overview.md, state.md | Mismatch | M | Tied to GAP-06 (state management). `Refresh` is simpler (StateHasChanged wrapper). |

### Major Gaps

| ID | Description | Spec Reference | Type | Complexity | Notes |
|----|-------------|---------------|------|-----------|-------|
| GAP-10 | **Pin/unpin behavior missing.** Spec describes `Unpinnable`, `Unpinned`, `UnpinnedSize` parameters with toolbar integration for unpinned panes. Source fires `OnPanePin` callback but has no actual pin/unpin visual state or toolbar. | overview.md, events.md | Spec-Ahead | L | Requires toolbar component, slide-out animation, unpinned pane rendering area. |
| GAP-11 | **Root orientation parameter missing.** Spec describes root-level `Orientation` parameter (`DockManagerPaneOrientation`). Source has no orientation control. | overview.md | Spec-Ahead | S | Enum + root splitter direction. Depends on GAP-02. |
| GAP-12 | **Visible parameter with two-way binding missing.** Spec describes `Visible` parameter on all pane types with two-way binding and `VisibleChanged` event for close behavior. Source has no visibility toggle. | overview.md, events.md | Spec-Ahead | M | Two-way binding pattern per pane type. Replaces current close-by-removal approach. |
| GAP-13 | **Accessibility/ARIA missing.** Spec describes `role=application`, `aria-live=polite`, `aria-hidden` on navigator, plus references to toolbar/tabstrip/splitter/window a11y specs. Source has no ARIA attributes. | accessibility/wai-aria-support.md | Spec-Ahead | M | ARIA attributes, keyboard navigation, screen reader support across all sub-components. |
| GAP-14 | **SCSS parity missing (both providers).** 0 of 9 BEM classes styled in either FluentUI or Bootstrap provider. Zero visual parity. | delivery-report (VP-ALL) | Visual Parity | L | Full SCSS authoring for both providers across all BEM classes for dock manager, split pane, tab group, floating window, toolbar, navigator. |

### Moderate Gaps

| ID | Description | Spec Reference | Type | Complexity | Notes |
|----|-------------|---------------|------|-----------|-------|
| GAP-15 | **Maximizable parameter missing.** Spec describes `Maximizable` parameter on content panes. Source has no maximize support. | overview.md | Spec-Ahead | M | Toggle max/restore, overlay or expand within parent, button in header. |
| GAP-16 | **AllowFloat parameter missing.** Spec describes `AllowFloat` parameter on content panes to control whether a pane can be dragged to float. Source has no float restriction logic. | overview.md | Spec-Ahead | S | Boolean gate on undock/float action. Depends on GAP-04/GAP-05. |
| GAP-17 | **AllowEmpty parameter missing.** Spec describes `AllowEmpty` on split and tab group panes for empty-space behavior when child panes are removed. Source has no equivalent. | overview.md | Spec-Ahead | S | Conditional render of empty placeholder vs. collapse. Depends on GAP-02/GAP-03. |

### Minor Gaps

| ID | Description | Spec Reference | Type | Complexity | Notes |
|----|-------------|---------------|------|-----------|-------|
| GAP-18 | **HeaderText vs Title naming mismatch.** Spec uses `HeaderText` parameter name. Source uses `Title`. | overview.md | Mismatch | S | Rename parameter; breaking change on current API. |
| GAP-19 | **OnPaneFloat event not in spec.** Source exposes `OnPaneFloat` callback. Spec uses `OnUndock` for float-out behavior. | events.md | Source-Ahead | S | Remove or alias when GAP-07 event model is implemented. |
| GAP-20 | **OnPaneClose event pattern differs.** Source uses `OnPaneClose` (`EventCallback<string>`). Spec uses `VisibleChanged` (`bool`) for close behavior. | events.md | Source-Ahead | S | Replaced by GAP-12 (Visible two-way binding). |
| GAP-21 | **CSS class prefix in spec uses k-prefix.** Spec references `k-dockmanager`, `k-pane-scrollable`, `k-dock-manager-splitter`. Source correctly uses `mar-` prefix. Spec needs update. | overview.md, accessibility | Info | S | Spec doc update only. No source change needed. |
| GAP-22 | **Dockable parameter missing.** Spec describes `Dockable` bool on content panes to control whether other panes can dock to/over it. Source has no such parameter. | docking-types.md, overview.md | Spec-Ahead | S | Boolean gate on dock-target behavior. Depends on GAP-05. |

---

## Summary

| Severity | Count | IDs |
|----------|-------|-----|
| Critical | 9 | GAP-01 through GAP-09 |
| Major | 5 | GAP-10 through GAP-14 |
| Moderate | 3 | GAP-15 through GAP-17 |
| Minor | 5 | GAP-18 through GAP-22 |
| **Total** | **22** | |

### Complexity Distribution

| Complexity | Count | IDs |
|-----------|-------|-----|
| XL | 4 | GAP-01, GAP-02, GAP-04, GAP-05 |
| L | 5 | GAP-03, GAP-06, GAP-07, GAP-10, GAP-14 |
| M | 5 | GAP-08, GAP-09, GAP-12, GAP-13, GAP-15 |
| S | 8 | GAP-11, GAP-16, GAP-17, GAP-18, GAP-19, GAP-20, GAP-21, GAP-22 |

### Dependency Chain

```
GAP-01 (Pane Hierarchy) ──> GAP-02 (Split Pane) ──> GAP-11 (Orientation)
                        ──> GAP-03 (Tab Group)
                        ──> GAP-06 (State Mgmt) ──> GAP-09 (Methods)
                        ──> GAP-07 (Event Model)
                        ──> GAP-08 (Naming) [resolved by GAP-01]
                        ──> GAP-12 (Visible)
                        ──> GAP-10 (Pin/Unpin)

GAP-04 (Floating Panes) ──> GAP-16 (AllowFloat)
GAP-05 (Drag & Drop)    ──> GAP-22 (Dockable)
GAP-02/03               ──> GAP-17 (AllowEmpty)
GAP-07                  ──> GAP-19 (OnPaneFloat removal)
GAP-12                  ──> GAP-20 (OnPaneClose removal)
GAP-14 (SCSS)           ──> depends on all structural gaps being defined
```
