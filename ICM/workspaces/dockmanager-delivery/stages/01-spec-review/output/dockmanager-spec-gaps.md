# DockManager Spec Gaps — Stage 01 Spec Review

**Date:** 2026-04-12
**Worker:** w-dockmanager-delivery
**Component:** MariloDockManager + MariloDockPane

## Summary

The spec describes a full-featured dock manager with split panes, tab groups, floating panes, docking/undocking, pinning/unpinning, state management, and rich event model. The current source implementation is a minimal tabbed-panel component with basic close/pin/float callbacks. The gap between spec and source is large.

## Spec-Ahead Gaps (spec describes, source does not implement)

| # | Feature Area | Spec Reference | Gap Description | Severity |
|---|---|---|---|---|
| SA-1 | Pane hierarchy | overview.md, pane-types.md | Spec defines `DockManagerContentPane`, `DockManagerSplitPane`, `DockManagerTabGroupPane` as distinct child components with nesting rules. Source has only `MariloDockPane` (flat list, no hierarchy). | Critical |
| SA-2 | Split pane layout | pane-types.md, overview.md | Spec describes splitter-based horizontal/vertical resizable splits via `DockManagerSplitPane` with `Orientation` parameter. Source has no split pane component at all. | Critical |
| SA-3 | Tab group pane | pane-types.md, overview.md | Spec describes `DockManagerTabGroupPane` with `SelectedPaneId`, `AllowEmpty`. Source has a single flat tab strip — no nested tab groups. | Critical |
| SA-4 | Floating panes | overview.md | Spec describes `<DockManagerFloatingPanes>` container with floating window behavior (`FloatingHeight`, `FloatingLeft`, `FloatingTop`, `FloatingWidth`, `FloatingResizable`). Source has no floating window support. | Critical |
| SA-5 | Docking/undocking | docking-types.md | Spec describes global docking navigator and inner docking with drag-and-drop. Source has no drag-and-drop, no dock navigator, no `Dockable` parameter enforcement. | Critical |
| SA-6 | Pin/unpin behavior | overview.md, events.md | Spec describes `Unpinnable`, `Unpinned`, `UnpinnedSize` parameters with toolbar integration for unpinned panes. Source fires `OnPanePin` callback but has no actual pin/unpin visual state or toolbar. | Major |
| SA-7 | State management | state.md | Spec describes `DockManagerState` object with `GetState`/`SetState`/`SetStateAsync` methods plus `OnStateInit`/`OnStateChanged` events for persistence. Source has no state management at all. | Critical |
| SA-8 | Rich events | events.md | Spec defines `OnDock`, `OnUndock`, `OnPin`, `OnUnpin`, `OnPaneResize`, `VisibleChanged`, `SizeChanged`, `UnpinnedChanged`, `UnpinnedSizeChanged`, `OnStateInit`, `OnStateChanged` with cancellation support (`IsCancelled`). Source has only `OnPaneClose`, `OnPanePin`, `OnPaneFloat` (no cancellation). | Critical |
| SA-9 | Maximizable panes | overview.md | Spec describes `Maximizable` parameter on content panes. Source has no maximize support. | Moderate |
| SA-10 | AllowFloat parameter | overview.md | Spec describes `AllowFloat` parameter on content panes. Source has no float restriction logic. | Moderate |
| SA-11 | AllowEmpty parameter | overview.md | Spec describes `AllowEmpty` on split and tab group panes for empty-space behavior on child removal. Source has no equivalent. | Moderate |
| SA-12 | Orientation parameter | overview.md | Spec describes root-level `Orientation` parameter (`DockManagerPaneOrientation`). Source has no orientation control. | Major |
| SA-13 | Visible parameter | overview.md | Spec describes `Visible` parameter with two-way binding on all pane types. Source has no visibility toggle. | Major |
| SA-14 | Accessibility/ARIA | accessibility/wai-aria-support.md | Spec describes `role=application`, `aria-live=polite`, `aria-hidden` on navigator, plus references to toolbar/tabstrip/splitter/window a11y specs. Source has no ARIA attributes. | Major |
| SA-15 | HeaderText vs Title | overview.md | Spec uses `HeaderText` parameter name. Source uses `Title`. API naming mismatch. | Minor |

## Source-Ahead Gaps (source implements, spec does not describe)

| # | Feature | Source Location | Gap Description | Severity |
|---|---|---|---|---|
| SO-1 | OnPaneFloat event | MariloDockManager.razor:49 | Source exposes `OnPaneFloat` callback. Spec has no `OnPaneFloat` event — spec uses `OnUndock` for float-out. | Minor |
| SO-2 | OnPaneClose event | MariloDockManager.razor:43 | Source uses `OnPaneClose` (EventCallback<string>). Spec uses `VisibleChanged` (bool) for close behavior. Different pattern. | Minor |

## Mismatches

| # | Area | Spec Says | Source Does | Severity |
|---|---|---|---|---|
| MM-1 | Child component names | `DockManagerContentPane`, `DockManagerSplitPane`, `DockManagerTabGroupPane` | `MariloDockPane` (single flat type) | Critical |
| MM-2 | Event signatures | `DockManagerDockEventArgs`, `DockManagerUndockEventArgs` etc. with `IsCancelled` | `EventCallback<string>` (id-only, no cancellation) | Critical |
| MM-3 | Pane parameter name | `HeaderText` | `Title` | Minor |
| MM-4 | CSS class prefix | Spec references `k-dockmanager`, `k-pane-scrollable`, `k-dock-manager-splitter` | Source uses `mar-dockmanager`, `mar-dockpane` (correct Marilo BEM prefix) | Info (spec needs update to Marilo convention) |
| MM-5 | Methods | `GetState()`, `SetState()`, `Refresh()` | None exposed | Critical |

## Conclusion

The source is at **Phase 1 (tabbed-panel skeleton)**. The spec describes a **Phase 3+ full dock manager**. 15 spec-ahead gaps exist, 6 of which are Critical. The component needs a multi-phase implementation effort to reach spec parity. The current implementation covers roughly 10-15% of the specified API surface.
