# MariloGantt Resolution Status

## Pass 1 — Spec alignment (complete)
Spec files in `docs/component-specs/gantt/` aligned with source. ~60 gaps closed via documentation.

## Pass 2 — E1–E8 (complete, 648 tests)

| ID | Description | Status | Files |
|----|-------------|--------|-------|
| E1 | Milestone rendering (◆ for zero-duration tasks) | ✅ Complete | MariloGantt.razor |
| E2 | Summary task auto-calculation (parent dates/percent from children) | ✅ Complete | MariloGantt.razor.cs, GanttNode.cs |
| E3 | GanttState Phase 2 (EditItem, EditField wired to state events) | ✅ Complete | GanttState.cs, MariloGantt.razor.cs |
| E4 | Hierarchical data binding (ItemsField, HasChildrenField) | ✅ Complete | GanttFieldAccessor.cs, MariloGantt.razor.cs |
| E5 | In-cell edit mode (click cell, Tab/Enter/Escape) | ✅ Complete | MariloGantt.razor, MariloGantt.razor.cs |
| E6 | Filter menu (popup per column header with funnel icon) | ✅ Complete | MariloGantt.razor, MariloGantt.razor.cs, GanttState.cs |
| E7 | Gantt SCSS for both providers + prefers-reduced-motion + forced-colors | ✅ Complete | SCSS files |
| E8 | Spec updates for all 7 features | ✅ Complete | docs/component-specs/gantt/ |

## Pass 3 — E9–E17 (complete, 700 tests)

| ID | Description | Status | Files |
|----|-------------|--------|-------|
| E9 | GanttState.OriginalEditItem clone (IGanttCloneable + JSON fallback) | ✅ Complete | IGanttCloneable.cs, GanttCloneHelper.cs, MariloGantt.razor.cs |
| E10 | GanttDependencies component model (stub, no SVG rendering) | ✅ Complete | GanttDependency.cs, GanttDependencyType.cs, GanttDependencyEventArgs.cs, MariloGanttDependencies.razor, MariloGantt.razor.cs |
| E11 | Screen reader announcements (non-drag) | ✅ Complete | MariloGantt.razor, MariloGantt.razor.cs, _gantt.scss, _bridge-gantt.scss |
| E12 | Filter checkbox list (Drawer-hosted, no popup anchoring) | ✅ Complete | GanttState.cs (GanttColumnFilterType enum), GanttColumn.razor, MariloGantt.razor, MariloGantt.razor.cs |
| E13 | marilo-drag.ts CDW workspace + API design (design only) | ✅ Complete | ICM/workspaces/marilo-drag/ (full workspace scaffold + API design doc) |
| E14 | MariloPopup primitive (stub, no full anchor tracking) | ✅ Complete | Overlays/MariloPopup.razor, MariloPopup.razor.cs, PopupPlacement.cs, docs/component-specs/popup/overview.md |
| E15 | Popup edit mode (using MariloPopup stub) | ✅ Complete | GanttState.cs (Popup enum value), MariloGantt.razor, MariloGantt.razor.cs |
| E16 | Filter checkbox list via MariloPopup | ✅ Complete | GanttState.cs (GanttFilterPopupMode enum), MariloGantt.razor, MariloGantt.razor.cs |
| E17 | Column chooser using MariloPopup | ✅ Complete | GanttState.cs (VisibleColumns), MariloGantt.razor, MariloGantt.razor.cs |

## Deferred to Pass 4+
- Column reorder (JS drag interop required)
- Column resize (JS drag interop required)
- Timeline bar drag-move (JS drag interop required)
- Timeline bar resize (JS drag interop required)
- GanttDependencies SVG rendering
- RangeSnapTo / zooming
- Full anchor-tracked popup (Floating UI)
- Drag-specific screen reader announcements
