# AllocationScheduler — Example UX Gap List

**Workspace:** ICM/workspaces/allocation-scheduler-delivery
**Stage:** 02-example-ux
**Related file:** `allocation-scheduler-demo-gap-list.md` (2026-04-05 snapshot — based on the pre-split single-page demo; superseded by this file for Wave 2 and later).

---

## 2026-04-11 orchestrator wave 2 (subagent dispatch)

**Worker:** `w-allocation-scheduler-delivery`
**Session:** `marilo-grid-pipeline-2026-04-11-1200`
**Spec topics audited:** 16 (overview, data-binding, selection, keyboard-navigation, editing, editing-grain, refresh-data, events, accessibility, splitter-layout, templates, theming, context-menu, scenario-planning, analysis-targets, business-objects)
**Spec line-count total:** 3,452 lines across 16 topic files.
**Demo source root:** `samples/Marilo.Demo/Pages/Components/AllocationScheduler/`

### Demo inventory (8 files)

| # | File | Route | Primary topics |
|---|------|-------|---------------|
| D1 | `AllocationSchedulerDemo.razor` | `/components/allocation-scheduler`, `/overview` | overview, editing, editing-grain (rollup), events, splitter-layout, refresh-data (navigation via @ref) |
| D2 | `AdvancedFeatures.razor` | `/advanced-features` | critical-path, loader/sizing, context-menu (CanExecuteAction), scenario lifecycle events |
| D3 | `BudgetAndTargets.razor` | `/budget-and-targets` | analysis-targets (ValueMode=Currency, Targets, ShowDeltas, DeltaDisplayMode, OnTargetChanged) |
| D4 | `ContextMenuDemo.razor` | `/context-menu` | context-menu (built-in, custom items, DefaultDistributionMode, OnDistributeRequested) |
| D5 | `NavigationAndZoom.razor` | `/navigation-and-zoom` | editing-grain (ViewGrain Day/Week/Month/Quarter, AllowZoomEdit), refresh-data (OnVisibleRangeChanged), ShowJumpToDate |
| D6 | `ScenarioPlanning.razor` | `/scenario-planning` | scenario-planning (AllocationSets, ScenarioOverrides, ActiveSetId/CompareSetId, ShowBaselineDiff, ShowComparisonPanel, OnScenarioChanged) |
| D7 | `SelectionAndEditing.razor` | `/selection-and-editing` | selection (None/Cell/Range), editing (bulk range, drag-fill, keyboard), custom context-menu, CanExecuteAction |
| D8 | `TemplatesDemo.razor` | `/templates` | templates (ResourceRowTemplate, CellTemplate, ToolbarTemplate, EmptyTemplate) |

**Delta vs 2026-04-05 `allocation-scheduler-demo-gap-list.md`:** that snapshot audited a single 6-scenario demo file and raised 19 parameter + 17 event + 11 edge-case gaps. Since then the demo was split into 8 pages and most of those gaps were closed. This Wave 2 audit re-evaluates against the 16-topic spec surface and raises only gaps that still exist today.

### Topic coverage matrix (16 topics)

| # | Spec topic | Classification | Backing demo(s) | Notes |
|---|------------|----------------|-----------------|-------|
| 1 | overview | **Covered** | D1 | Basic resource grid, read-only mode, AllocationResourceColumns, Height, default range/visible start all shown. |
| 2 | data-binding | **Covered** | D1, D3, D6 | Resources + Allocations + Targets + AllocationSets + ScenarioOverrides bindings all present. `VisibleEnd` (explicit end-date binding) still not demoed — minor. |
| 3 | selection | **Covered** | D7 | All three `AllocationSelectionMode` values toggled with `OnSelectionChanged`. |
| 4 | keyboard-navigation | **Partial** | D1 (AllowKeyboardEdit), D5 (AllowZoomEdit + keyboard edit), D7 (bulk keyboard) | Keyboard *editing* is demoed but the 260-line `keyboard-navigation.md` spec describes a much richer surface (Tab/Enter/Arrow/PgUp/PgDn traversal, roving tabindex, focus persistence across zoom). No demo shows a keyboard-focused walkthrough, focus indicator, or shortcut cheat-sheet overlay. |
| 5 | editing | **Covered** | D1 (Interactive Allocation section), D7 | AllowDragFill, AllowKeyboardEdit, AllowBulkEdit, OnCellEdited, OnRangeEdited all covered. |
| 6 | editing-grain | **Partial** | D1 ("Granularity Modes"/Month Rollup), D5 (ViewGrain toggle + AllowZoomEdit) | Week ↔ Month ↔ Quarter rollup and AllowZoomEdit demoed. **Day grain editing and the authoritative-level rules for sub-bucket distribution are not explicitly demonstrated.** The spec's distinction between cell-grain writes vs row-grain writes (editing-grain.md L70-120) is not surfaced as a scenario. |
| 7 | refresh-data | **Partial** | D1 ("Navigation" section via `_navScheduler` @ref) | NavigateTo / NavigateForward / NavigateBack / NavigateToToday are all demoed via component ref. **`Rebind()` and `Refresh()` public methods have NO demo.** Spec (refresh-data.md, 255 lines) treats Rebind/Refresh as first-class — this is a hard gap. |
| 8 | events | **Covered** | D1, D2, D3, D4, D6, D7 | OnCellEdited, OnRangeEdited, OnSelectionChanged, OnContextMenuAction, OnDistributeRequested, OnVisibleRangeChanged, OnTargetChanged, OnScenarioChanged, OnAllocationOverridden, OnScenarioStatusChanged, OnScenarioPromoted, ViewGrainChanged, ActiveSetIdChanged all wired. `OnShiftValues` and `OnMoveValues` still appear to be unexercised by any demo (see Wave 1 SPEC-AS-008 context). |
| 9 | accessibility | **MISSING** | — | No demo references accessibility, ARIA, screen-reader messaging, focus rings, or the `accessibility.md` spec (72 lines). Zero `aria-*`, zero `AriaLabel` references in demo sources. **Hard gap — most user-visible "invisible" surface.** |
| 10 | splitter-layout | **Covered** | D1 ("Layout & Splitter" section) | SplitterPosition (two-way), AllowSplitterCollapse, OnSplitterCollapsed, OnSplitterRestored, SetSplitterPosition / CollapseSplitter / RestoreSplitter all demoed via `_splitterScheduler` ref. Strong coverage. |
| 11 | templates | **Covered** | D8 | ResourceRowTemplate, CellTemplate, ToolbarTemplate, EmptyTemplate all shown. **HeaderTemplate / FooterTemplate / ColumnTemplate-at-scheduler-level** (if any exist in source — see Wave 1 SPEC-AS-001 re ResourceRowTemplate being undocumented) should be cross-checked against source templates enumeration; treat as Partial once Wave 1 template inventory lands. |
| 12 | theming | **MISSING** | — | Zero demo references `MariloTheme`, `ThemeVariant`, provider-swap, or dark-mode. The 71-line `theming.md` spec describes provider CSS methods (overview.md lists 14 CSS provider methods). No demo shows theme swap or per-instance theming hook. **Hard gap.** |
| 13 | context-menu | **Covered** | D2, D4, D7 | Built-in commands, custom ContextMenuItems, CanExecuteAction, OnContextMenuAction, OnDistributeRequested, DefaultDistributionMode all wired. |
| 14 | scenario-planning | **Covered** | D2, D6 | AllocationSets, ScenarioOverrides, ActiveSetId (two-way), CompareSetId, ShowBaselineDiff, ShowComparisonPanel, scenario lifecycle events (OnAllocationOverridden, OnScenarioStatusChanged, OnScenarioPromoted, OnScenarioChanged). One of the richer demo areas — well exercised relative to the 299-line spec. |
| 15 | analysis-targets | **Covered** | D3 | Targets, ShowTargets, ShowDeltas, DeltaDisplayMode (Value/Percentage/StatusIcon), OnTargetChanged — directly mirrors the 108-line `analysis-targets.md` spec. |
| 16 | business-objects | **Partial** | D1-D8 (all use `StaffResource` + `AllocationRecord`) | Every demo uses the same `StaffResource` / `AllocationRecord` pair. The 660-line `allocation-scheduler-business-objects.md` spec describes a larger object graph (Task, Project, AllocationSet, Override, Target, etc.). No demo pulls from a shared business-objects fixture or showcases more than one `TResource` binding. **Partial — functional but thin.** |

### Coverage roll-up

- **Covered:** 10 topics (overview, data-binding, selection, editing, events, splitter-layout, templates, context-menu, scenario-planning, analysis-targets)
- **Partial:** 4 topics (keyboard-navigation, editing-grain, refresh-data, business-objects)
- **Missing:** 2 topics (accessibility, theming)
- **Orphan:** 0 demos (every file in the folder maps cleanly to at least one spec topic)
- **Blocked-by-source:** 0 (all Wave 1 SPEC-AS gaps are spec/doc only — no source rework required to demo the covered surface)

### Top 3 findings (priority order)

**F1 — accessibility topic has ZERO demo coverage (P1).**
The 72-line `accessibility.md` spec describes roving tabindex, ARIA roles, SR announcements for cell/range edits, and focus-persistence rules. None of this is visible in any of the 8 demo pages. Even a single scenario "Keyboard-first walkthrough with ARIA live-region log" would lift this from Missing to Covered. Recommended new demo file: `AccessibilityDemo.razor` with (a) a focused keyboard walkthrough (b) an SR-simulated log of edit events (c) a high-contrast toggle.

**F2 — theming topic has ZERO demo coverage (P1).**
Marilo's core value prop is the provider abstraction, and AllocationScheduler's `theming.md` spec + `overview.md` public-method table enumerate 14 CSS provider methods. No demo swaps providers, flips dark mode, or exposes per-instance theming. Recommended new demo file: `ThemingDemo.razor` showing provider swap + dark/light toggle + a custom CSS class override.

**F3 — `refresh-data` surface has navigation covered but `Rebind()` / `Refresh()` NOT covered (P2).**
NavigateTo / NavigateForward / NavigateBack / NavigateToToday are demoed via `_navScheduler` @ref in D1. But the two reload-style methods (`Rebind`, `Refresh`) — which the 255-line `refresh-data.md` spec treats as first-class and which map to the spec gap SPEC-AS-003 return-type mismatch — have no demo scenario. A "Reload from server" button in an existing demo or a small new scenario would close this.

### Secondary gaps (P3, not in top 3)

- **G1 keyboard-navigation cheat-sheet:** no demo visualises the full traversal grammar (Tab across resource cols → Enter into grid → Arrow keys across buckets → PgUp/PgDn across rows). Partial only.
- **G2 editing-grain (day + sub-bucket distribution):** no demo at `AuthoritativeLevel=Day` showing how day edits roll up into weekly totals.
- **G3 business-objects breadth:** every demo uses the same `StaffResource` + `AllocationRecord` fixture. No scenario showcases Task/Project aggregation or a second `TResource` type.
- **G4 `OnShiftValues` / `OnMoveValues` events:** not wired into any demo. Mentioned in Wave 1 gap list; still unresolved at the UX layer.
- **G5 `VisibleEnd` parameter:** explicit end-date binding shown nowhere; all demos use `VisibleStart` + `DefaultRangeUnit`. Cosmetic.

### Recommended Wave-3 actions

1. **Add `AccessibilityDemo.razor`** — P1, closes F1. New file, no source changes required.
2. **Add `ThemingDemo.razor`** — P1, closes F2. Requires reading the 14 CSS provider methods list and wiring a provider-swap toggle. No source changes.
3. **Extend `AllocationSchedulerDemo.razor` section 6 (or D5)** with a "Rebind / Refresh" scenario — P2, closes F3. Single file edit, owned by Wave-3 UX worker.
4. **Extend `NavigationAndZoom.razor`** with a Day-grain editing + sub-bucket rollup scenario — P2, closes G2.
5. **Cross-check Wave 1 SPEC-AS-001 once spec side lands:** `ResourceRowTemplate` now IS demoed (D8), so once the spec is updated the undocumented-slot gap is also closed at the demo-sync layer — the two waves should confirm this when they meet at Wave 4 sync-check.

### Constraints / sync-area notes

- Sync area for this stage: `demo` only. No source, spec, or test edits made.
- No orphan demos found. Every razor file under `Pages/Components/AllocationScheduler/` maps to one or more spec topics.
- No architecture-level concerns. All recommended follow-ups are additive new-file or append-to-existing-file demo work that fits inside the standard `files_owned` shape for a future worker.
- No ownership conflicts with the other grid-pipeline workers (datagrid, gantt, scheduler, datasheet) — AllocationScheduler demo folder is disjoint.

---
