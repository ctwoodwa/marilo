# MariloGantt Gap Priorities — Stage 02

**Date:** 2026-04-09
**Input:** output/stage-01/gantt-spec-gap-list.md

## Priority Tiers

### Tier 1 — Spec Corrections (fix spec to match source)

These require NO code changes. The spec is wrong; the source is right. Fix the spec.

| # | Gap IDs | Action | Rationale |
|---|---------|--------|-----------|
| 1 | 400, 300, 215 | Rename `TreeListWidth` → `TaskListWidth` (int, default 250) across all spec files | Telerik-naming-is-canonical decision; source name is correct |
| 2 | 101 | Rename `OnEdit` → `OnTaskEdit` in events.md | Source name is canonical |
| 3 | 522 | Normalize `ToolTipTemplate` → `TooltipTemplate` in overview.md | Casing inconsistency; source uses lowercase 't' |
| 4 | 405 | Change `GanttColumn.Visible` from `bool?` to `bool` (default true) | Source type is correct |
| 5 | 514 | Change `RangeStart`/`RangeEnd` from `DateTime` to `DateTime?` | Source nullability is correct |
| 6 | 507, 508 | Update `TooltipTemplate` from `RenderFragment<object>` + `TooltipTemplateContext` to `RenderFragment<TItem>` | Source's strongly-typed approach is correct; no cast needed |
| 7 | 511 | Clarify `TaskTemplate` is bar inner content only, not full bar override | Source behavior is correct |
| 8 | 613 | Rewrite dependency data binding section: replace `GanttDependencies` component model with `DependsOnField` approach | Source architecture is simpler and correct |
| 9 | 422, 423 | Rewrite sorting/filter behavior descriptions to match source (tri-state single-column, auto-expand) | Source behavior is correct |
| 10 | 700–705 | Update timeline ARIA roles: `role="img"` + `aria-label`, not `role="treeitem"` | Source implementation is the better a11y pattern |
| 11 | 221 | Clarify `View`/`ViewChanged` is standard Blazor two-way binding pattern | No mismatch — `@bind-View` works with `View`+`ViewChanged` |
| 12 | 520 | Add per-view `SlotWidth` defaults to parameter table | Documentation gap only |

### Tier 2 — Document Undocumented Source Features

These require spec additions to cover what the source already implements. No code changes.

| # | Gap IDs | Feature to Document |
|---|---------|---------------------|
| 1 | 100, 102 | `OnTaskClick`, `ViewChanged` events |
| 2 | 301–305, 310, 428 | All 7 field mapping parameters + customization guide |
| 3 | 419, 307–309 | `OnExpand`/`OnCollapse`/`OnCreate` events |
| 4 | 420, 421 | `RowHeight`, `GanttToolBarTemplate` parameters |
| 5 | 424, 425 | `Filterable`/`Sortable` column params in bound.md table |
| 6 | 515, 518 | `DayWidth` legacy param, `GanttViews` empty fallback |
| 7 | 606 | `DependsOnField` parameter documentation |
| 8 | 706, 711, 715, 716, 717 | Accessibility: aria-sort, treegrid tabindex, chevron labels, bar labels |
| 9 | 707–710 | Keyboard navigation: edit mode, arrow expand/collapse, Home/End |
| 10 | 516, 517 | `RowHeight` and `ViewChanged` in timeline spec |
| 11 | 224 | `Rebind()` method in state spec |
| 12 | 306, 311 | OnParametersSet auto-detection, Rebind timeline recomputation |

### Tier 3 — Spec-Ahead Features (mark as Planned)

These are documented in spec but not implemented. Per decision log: spec-ahead items stay as Planned with P2/P3 priority and gap ID links.

**P2 — Should implement this phase:**

| # | Gap IDs | Feature | Notes |
|---|---------|---------|-------|
| 1 | 401 | `GanttCommandColumn` / command buttons | Needed for CRUD UX |
| 2 | 408 | `TreeListEditMode` enum (Incell/Inline/Popup) | Source has one mode; spec expects three |
| 3 | 407 | `Sortable` (gantt-level) + `SortMode.Multiple` | Source: single-column only |
| 4 | 414 | `FilterMode` enum (FilterRow/FilterMenu) | Source: filter row only |
| 5 | 503, 504 | Percent-complete bar + drag handle | Core visual feature |
| 6 | 509, 510 | Date header templates + format params on views | View customization |
| 7 | 410 | Cancellable `OnEdit` event | Pre-edit gate |
| 8 | 411 | `NewRowPosition` | New item placement |
| 9 | 412 | `EditorType` per column | Editor customization |
| 10 | 418 | Remove `Navigable` from spec (always-on in source) | Spec cleanup |
| 11 | 505, 506 | Hover delete button + popup edit from bar | Timeline interaction |

**P3 — Next phase:**

| # | Gap IDs | Feature |
|---|---------|---------|
| 1 | 200–212, 222–223 | Full `GanttState<TItem>` system (container, events, methods) |
| 2 | 600–605, 607–612, 614 | Full dependency component model (when needed) |
| 3 | 402, 403 | Column reorder + resize |
| 4 | 404, 426, 427 | Column menu + chooser + settings |
| 5 | 406 | Hierarchical data binding (`HasChildrenField`/`ItemsField`) |
| 6 | 409, 413 | Popup editing system + editor template |
| 7 | 415–417 | Filter editor type, operator, debounce |
| 8 | 500 | `RangeSnapTo` / zooming |
| 9 | 501, 502 | Timeline bar drag-move + resize |
| 10 | 512, 513 | Milestone rendering + summary auto-calc |
| 11 | 618 | Dependency validation |

### Tier 4 — Missing Features (not in spec or source)

| # | Gap IDs | Feature | Priority |
|---|---------|---------|----------|
| 1 | 719 | Skip navigation links | P1 — WCAG requirement |
| 2 | 720 | Screen reader drag announcements | P2 |
| 3 | 722 | `prefers-reduced-motion` support | P2 |
| 4 | 721 | High-contrast mode | P3 |

## Recommended Execution Sequence

1. **Tier 1** (12 spec corrections) — immediate, no code changes
2. **Tier 2** (12 documentation additions) — immediate, no code changes
3. **Tier 4 #1** (skip nav) — quick a11y win
4. **Tier 3 P2** (11 features) — implementation phase
5. **Tier 4 #2–4** (a11y enhancements) — alongside Tier 3
6. **Tier 3 P3** (11 feature groups) — next phase
