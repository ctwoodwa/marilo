# MariloGantt Spec Gap List — Stage 01 Intake

**Date:** 2026-04-09
**Source:** Marilo-gantt-rewrite worktree (canonical)
**Scope:** systematic (all 8 feature areas)
**Total unique gaps:** 107 (after deduplication across feature areas)

## Summary by Feature Area

| Feature Area | Gaps | P1 | P2 | P3 | Types |
|---|---|---|---|---|---|
| events | 4 | 1 | 2 | 1 | 1 mismatch, 2 undocumented, 1 spec-ahead |
| state | 25 | 9 | 11 | 5 | 3 mismatch, 5 undocumented, 17 spec-ahead |
| refresh-data | 11 | 2 | 6 | 3 | 1 mismatch, 8 undocumented, 2 spec-ahead |
| gantt-tree | 29 | 1 | 16 | 12 | 4 mismatch, 4 undocumented, 21 spec-ahead |
| timeline | 23 | 7 | 10 | 6 | 7 mismatch, 5 undocumented, 11 spec-ahead |
| dependencies | 19 | 6 | 8 | 5 | 1 mismatch, 4 undocumented, 14 spec-ahead |
| accessibility | 23 | 5 | 12 | 6 | 1 mismatch, 9 undocumented, 9 spec-ahead, 4 missing |
| **Totals** | **134 raw / ~107 unique** | **31** | **65** | **38** |

## Summary by Type

| Type | Count | Description |
|---|---|---|
| spec-ahead | ~65 | Documented in spec but not implemented in source |
| undocumented | ~25 | Implemented in source but not documented in spec |
| mismatch | ~17 | Both exist but differ in name, type, default, or behavior |

## Critical Findings (P1 Blockers)

### Architectural Mismatches

1. **Dependency data model (SPEC-gantt-613):** Spec documents a separate `GanttDependencies` component with `PredecessorIdField`, `SuccessorIdField`, `TypeField`, `GanttDependencyType` enum, and `GanttDependencyModel`. Source uses a simple `DependsOnField` string parameter mapping to a list of predecessor IDs. Entire dependency spec section needs rewriting.

2. **State management (SPEC-gantt-200–212):** Spec documents a full `GanttState<TItem>` container class, `OnStateInit`/`OnStateChanged` events, `GetState()`/`SetStateAsync()` methods, and 10+ state properties (FilterDescriptors, SortDescriptors, ExpandedItems, EditItem, ColumnStates, etc.). None of this exists in source. State is managed internally with no public state API.

3. **TooltipTemplate type (SPEC-gantt-507/508):** Spec defines `RenderFragment<object>` with `TooltipTemplateContext` class requiring cast. Source defines `RenderFragment<TItem>` — strongly typed, no context class. Types are fundamentally incompatible.

### Naming Mismatches

4. **TreeListWidth vs TaskListWidth (SPEC-gantt-400):** Spec uses `TreeListWidth` (string); source uses `TaskListWidth` (int, default 250). Appears in 3+ spec files.

5. **OnEdit vs OnTaskEdit (SPEC-gantt-101):** Spec documents `OnEdit`; source implements `OnTaskEdit`. Breaking naming mismatch.

6. **TooltipTemplate casing (SPEC-gantt-522):** Spec inconsistently uses `ToolTipTemplate` (capital T in Tip) in overview.md vs `TooltipTemplate` elsewhere. Source uses `TooltipTemplate`.

### Missing Timeline Interactions

7. **Bar drag-move (SPEC-gantt-501):** Spec documents drag-move for timeline bars. Not implemented.

8. **Bar drag-resize (SPEC-gantt-502):** Spec documents resize handles on bars. Not implemented.

9. **RangeSnapTo (SPEC-gantt-500):** Spec documents `GanttRangeSnapTo` enum and parameter. Not implemented.

### Missing Accessibility

10. **Skip navigation (SPEC-gantt-719):** Not implemented, not in spec, but needed per WCAG.

---

## Events Area (SPEC-gantt-100–103)

**SPEC-gantt-100** | undocumented | P2
`OnTaskClick: EventCallback<TItem>` — in source, not in spec.

**SPEC-gantt-101** | mismatch | P1
Spec: `OnEdit`. Source: `OnTaskEdit`. Rename spec to match source per Telerik-naming-is-canonical decision.

**SPEC-gantt-102** | undocumented | P2
`ViewChanged: EventCallback<GanttView>` — in source, not in spec.

**SPEC-gantt-103** | spec-ahead | P3
`TreeListWidthChanged` — in spec, not found in source API surface.

---

## State Area (SPEC-gantt-200–225)

**SPEC-gantt-200** | spec-ahead | P1
`GanttState<TItem>` class — entire state container not implemented.

**SPEC-gantt-201** | spec-ahead | P1
`OnStateInit` event — not implemented.

**SPEC-gantt-202** | spec-ahead | P1
`OnStateChanged` event — not implemented.

**SPEC-gantt-203** | spec-ahead | P1
`GetState()` public method — not implemented.

**SPEC-gantt-204** | spec-ahead | P1
`SetStateAsync()` public method — not implemented.

**SPEC-gantt-205** | spec-ahead | P1
`GanttState.EditItem` — not implemented.

**SPEC-gantt-206–209** | spec-ahead | P2
State properties: `OriginalEditItem`, `InsertedItem`, `EditField`, `ParentItem`.

**SPEC-gantt-210** | spec-ahead | P1
`GanttState.FilterDescriptors` — not implemented.

**SPEC-gantt-211** | spec-ahead | P1
`GanttState.SortDescriptors` — not implemented.

**SPEC-gantt-212** | spec-ahead | P1
`GanttState.ExpandedItems` — not implemented.

**SPEC-gantt-213–214** | spec-ahead | P2
`GanttState.TreeListWidth`, `GanttState.ColumnStates`.

**SPEC-gantt-215** | mismatch | P2
`TreeListWidth` (spec, string) vs `TaskListWidth` (source, int).

**SPEC-gantt-216–219** | spec-ahead | P3
`Sortable`, `FilterMode`, `ColumnResizable`, `ColumnReorderable` parameters.

**SPEC-gantt-220** | spec-ahead | P2
`TreeListEditMode` parameter / `GanttTreeListEditMode` enum.

**SPEC-gantt-221** | mismatch | P2
`View` binding: spec shows `@bind-View`; source uses `View` + `ViewChanged` callback.

**SPEC-gantt-222–223** | spec-ahead | P1/P2
`GanttStateEventArgs<TItem>`, `GanttColumnState` classes.

**SPEC-gantt-224** | undocumented | P2
`Rebind()` method — in source, not documented in state spec.

---

## Refresh-Data Area (SPEC-gantt-300–311)

**SPEC-gantt-300** | mismatch | P1
`TreeListWidth` (spec) vs `TaskListWidth` (source) in code examples.

**SPEC-gantt-301–304** | undocumented | P2
Field mapping parameters `TitleField`, `StartField`, `EndField`, `PercentCompleteField` not documented.

**SPEC-gantt-305** | undocumented | P1
`DependsOnField` parameter not documented.

**SPEC-gantt-306** | spec-ahead | P2
OnParametersSet automatic change detection not documented.

**SPEC-gantt-307–309** | undocumented | P3
`OnCreate`, `OnExpand`, `OnCollapse` events not in refresh-data docs.

**SPEC-gantt-310** | undocumented | P2
Field customization capability not explained.

**SPEC-gantt-311** | spec-ahead | P3
`Rebind()` timeline recomputation not documented.

---

## Gantt-Tree Area (SPEC-gantt-400–428)

**SPEC-gantt-400** | mismatch | P1
`TreeListWidth` (spec) vs `TaskListWidth` (source) — name and type mismatch.

**SPEC-gantt-401** | spec-ahead | P2
`GanttCommandColumn`/`GanttCommandButton` — not in source.

**SPEC-gantt-402–403** | spec-ahead | P3
Column reorder (`Reorderable`) and column resize (`Resizable`, `MinResizableWidth`, `MaxResizableWidth`).

**SPEC-gantt-404** | spec-ahead | P3
Column menu (`ShowColumnMenu`, `GanttColumnMenuSettings`, `GanttColumnMenuChooser`).

**SPEC-gantt-405** | mismatch | P2
`GanttColumn.Visible`: spec `bool?` (null=true), source `bool` (default true).

**SPEC-gantt-406** | spec-ahead | P3
`HasChildrenField`/`ItemsField` for hierarchical data binding.

**SPEC-gantt-407** | spec-ahead | P2
`Sortable` (gantt-level), `SortMode` enum — source has tri-state single-column only.

**SPEC-gantt-408** | spec-ahead | P2
`TreeListEditMode` / `GanttTreeListEditMode` enum (None/Incell/Inline/Popup).

**SPEC-gantt-409** | spec-ahead | P3
Popup editing system (`GanttPopupEditSettings`, `GanttPopupEditFormSettings`, `FormTemplate`).

**SPEC-gantt-410** | spec-ahead | P2
`OnEdit` event / `GanttEditEventArgs` (cancellable pre-edit).

**SPEC-gantt-411** | spec-ahead | P2
`NewRowPosition` / `GanttTreeListNewRowPosition` enum.

**SPEC-gantt-412** | spec-ahead | P2
`EditorType` / `GanttTreeListEditorType` enum per column.

**SPEC-gantt-413** | spec-ahead | P3
`EditorTemplate` on `GanttColumn`.

**SPEC-gantt-414** | spec-ahead | P2
`FilterMode` / `GanttFilterMode` enum (FilterRow, FilterMenu).

**SPEC-gantt-415–417** | spec-ahead | P3
`FilterEditorType`, `DefaultFilterOperator`, `ShowFilterCellButtons`, `FilterRowDebounceDelay`.

**SPEC-gantt-418** | spec-ahead | P2
`Navigable` parameter — source has always-on keyboard nav.

**SPEC-gantt-419** | undocumented | P2
`OnExpand`/`OnCollapse` events not in gantt-tree specs.

**SPEC-gantt-420–421** | undocumented | P2
`RowHeight` and `GanttToolBarTemplate` not formally documented.

**SPEC-gantt-422** | mismatch | P2
Sorting behavior description conflates sorting and filtering.

**SPEC-gantt-423** | mismatch | P2
Filter auto-expand behavior not documented; empty spec sections.

**SPEC-gantt-424–425** | undocumented | P2
`Filterable` and `Sortable` column parameters not in `columns/bound.md` table.

**SPEC-gantt-426–427** | spec-ahead | P3
`GanttSettings`/`GanttColumnMenuSettings` and column chooser template.

**SPEC-gantt-428** | undocumented | P2
`DependsOnField` not documented in data-binding spec.

---

## Timeline Area (SPEC-gantt-500–522)

**SPEC-gantt-500** | spec-ahead | P1
`RangeSnapTo` / `GanttRangeSnapTo` enum — not in source.

**SPEC-gantt-501** | spec-ahead | P1
Bar drag-move — not implemented.

**SPEC-gantt-502** | spec-ahead | P1
Bar drag-resize — not implemented.

**SPEC-gantt-503–506** | spec-ahead | P2
Percent-complete drag, percent-complete bar, hover delete button, popup edit from bar.

**SPEC-gantt-507–508** | spec-ahead | P1
`TooltipTemplate` context type mismatch and `TooltipTemplateContext` class.

**SPEC-gantt-509–510** | spec-ahead | P2
Date header templates and date format parameters on view components.

**SPEC-gantt-511** | mismatch | P1
`TaskTemplate` scope: spec implies full bar control, source is inner content only.

**SPEC-gantt-512–513** | spec-ahead | P3
Milestone rendering, summary task auto-calculation.

**SPEC-gantt-514** | mismatch | P2
`RangeStart`/`RangeEnd` nullability: spec `DateTime`, source `DateTime?`.

**SPEC-gantt-515** | undocumented | P3
`DayWidth` legacy parameter not in spec.

**SPEC-gantt-516–518** | undocumented | P2
`RowHeight`, `ViewChanged`, `GanttViews` empty fallback.

**SPEC-gantt-519** | undocumented | P3
Dependency SVG overlay not in timeline spec.

**SPEC-gantt-520–521** | mismatch | P3
Per-view `SlotWidth` defaults missing; bar min width (4px) undocumented.

**SPEC-gantt-522** | mismatch | P1
`ToolTipTemplate` vs `TooltipTemplate` casing inconsistency.

---

## Dependencies Area (SPEC-gantt-600–618)

**SPEC-gantt-600–605** | spec-ahead | P1
Entire `GanttDependenciesSettings`/`GanttDependencies` component hierarchy, `PredecessorIdField`, `SuccessorIdField`, `TypeField`, `GanttDependencyType` enum, `GanttDependencyModel` class.

**SPEC-gantt-606** | undocumented | P1
`DependsOnField` parameter — in source, not documented.

**SPEC-gantt-607–608** | spec-ahead | P2
`Data` and `IdField` on `GanttDependencies` component.

**SPEC-gantt-609–612** | spec-ahead | P2
Dependency CRUD events: `OnCreate`/`OnDelete` + event args classes.

**SPEC-gantt-613** | mismatch | P1
Dependency binding architecture fundamentally different.

**SPEC-gantt-614** | spec-ahead | P2
Constraint-type-aware rendering.

**SPEC-gantt-615–617** | undocumented | P3
`DependencyLine` record, `ComputeDependencyLines()`, SVG rendering.

**SPEC-gantt-618** | spec-ahead | P3
Dependency validation rules.

---

## Accessibility Area (SPEC-gantt-700–722)

**SPEC-gantt-700–705** | mixed | P2–P3
Timeline role mismatches: spec says `role="tree"` + `role="treeitem"`, source uses `role="img"` with aria-label.

**SPEC-gantt-706** | undocumented | P1
`aria-sort` on column headers — implemented, not in spec.

**SPEC-gantt-707–710** | undocumented | P2
Row `aria-expanded` null state, edit mode keyboard, ArrowRight/Left expand/collapse, Home/End keys.

**SPEC-gantt-711** | undocumented | P1
Treegrid `tabindex="0"` + roving focus — implemented, not documented.

**SPEC-gantt-712** | mismatch | P2
`role="application"` documented in spec, not in source.

**SPEC-gantt-713–714** | spec-ahead | P3
`aria-hidden` on progress element and task actions.

**SPEC-gantt-715** | undocumented | P1
Chevron `aria-label` pattern — implemented, not in spec.

**SPEC-gantt-716** | undocumented | P1
Timeline bar `aria-label` format — implemented, not in spec.

**SPEC-gantt-717** | undocumented | P2
Treegrid `aria-label="Task list"` — implemented, not in spec.

**SPEC-gantt-718** | spec-ahead | P3
No-columns fallback accessibility structure.

**SPEC-gantt-719** | missing | P1
Skip navigation links — not implemented, not in spec.

**SPEC-gantt-720** | missing | P2
Screen reader drag announcements.

**SPEC-gantt-721** | missing | P3
High-contrast mode support.

**SPEC-gantt-722** | missing | P2
Reduced-motion support.
