# MariloGantt — Demo Gap List

**Audit date:** 2026-04-10
**Existing demo pages:** 5 pages (Overview, Views, Templates, Hierarchical, Editing)
**Current scenario count:** 5
**Target scenario count:** 8 (3 new scenarios needed)

---

## Current Coverage

| # | Page | Scenario | Parameters Covered |
|---|------|---------|-------------------|
| 1 | Overview.razor | Basic Usage | Data, IdField, ParentIdField, Height, Width, OnUpdate, OnDelete, View, GanttColumns, GanttViews |
| 2 | Views.razor | All Four Views | GanttDayView, GanttWeekView, GanttMonthView, GanttYearView, SlotWidth |
| 3 | Templates.razor | Templates | TaskTemplate, column Template, GanttToolBarTemplate |
| 4 | Hierarchical.razor | Three-Level Hierarchy | ParentId nesting, expand/collapse |
| 5 | Editing.razor | Inline Editing | OnUpdate, OnDelete, OnCreate, double-click edit |

---

## Demo Gaps (Missing Scenarios)

| # | Gap | Feature | Priority |
|---|-----|---------|----------|
| G1 | No sorting/filtering demo | Sortable, Filterable, FilterFunc | P2 |
| G2 | No dependency lines demo | MariloGanttDependencies, dependency arrows | P2 |
| G3 | No drag-to-move/resize demo | Timeline drag interaction, OnUpdate | P2 |

---

## Assessment

The Gantt demo coverage is already strong (5 pages covering core features). Only 3 gaps remain for production readiness. The existing pages were created during the gap-analysis rewrite and cover the primary API surface well.
