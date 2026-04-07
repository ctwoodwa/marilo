# Batch Intake: 12 Complex Components

**Date:** 2026-04-03
**Scope:** Chart, Diagram, DockManager, Editor, FileManager, Gantt, Map, PivotGrid, Scheduler, Splitter, TreeList, Wizard
**Exclusions:** DataGrid and DataSheet (have dedicated delivery flows)

---

## Maturity Summary

| # | Component | Source? | Lines | Params | Tests? | Demo? | Spec Files | Analysis Mode | Estimated Gaps |
|---|-----------|---------|-------|--------|--------|-------|------------|---------------|---------------|
| 1 | MariloChart | Yes (2 files) | 994 | 23 | Yes (5 tests) | No | 37 | Reconstructed | ~16 |
| 2 | MariloDiagram | **No** | 0 | 0 | No | Yes (placeholder) | 6 | Standard | ~55 |
| 3 | MariloDockManager | **No** | 0 | 0 | No | Yes (placeholder) | 6 | Standard | ~47 |
| 4 | MariloEditor | Yes (1 file) | 670 | 18 | Yes (7 tests) | No | 18 | Reconstructed | ~12 |
| 5 | MariloFileManager | Yes (1 file) | 170 | 12 | No | Yes | 15 | Reconstructed | 22-28 |
| 6 | MariloGantt | Yes (1 file) | 95 | 6 | No | No | 44 | Reconstructed | ~20+ |
| 7 | MariloMap | **No** | 0 | 0 | No | Yes (placeholder) | 7 | Standard | ~35 |
| 8 | MariloPivotGrid | **No** | 0 | 0 | No | Yes (placeholder) | 5 | Standard | ~39 |
| 9 | MariloScheduler | Yes (1 file) | 182 | 10 | No | Yes | 25 | Reconstructed | 25-30 |
| 10 | MariloSplitter | Yes (2 files) | 568 | 22 | No | No | 7 | Reconstructed | ~10 |
| 11 | MariloTreeList | Yes (1 file) | 199 | 6 | No | Yes | 55 | Reconstructed | 40-50 |
| 12 | MariloWizard | Yes (1+1) | 125 | 9 | No | Yes | 9 | Reconstructed | 15-18 |

---

## Per-Component Intake

### 1. MariloChart
**Status:** Implemented (994 lines across 2 files, 23 params, 5 tests, no demo pages)
**Source:** `/workspaces/Marilo/src/Marilo.Components/Charts/MariloChart.razor` (844 lines) + `MariloChartSeries.razor` (150 lines)
**Implemented:** SVG rendering, tooltips, legend, series registration
**Missing (Critical):** Child components (ChartSeriesItems, ChartCategoryAxes, ChartSubtitle), Refresh method, drilldown
**Missing (Important):** CSS variable theming, ResetDrilldownLevel, axis configuration depth
**Intake record:** `stages/01-intake/output/gap-chart-inventory.md`
**Severity estimate:** 6 Critical, 5 Important, 5 Nice-to-have (~16 total)
**Recommendation:** Reconstructed mode. Substantial code exists.

### 2. MariloDiagram
**Status:** No source code (greenfield)
**Source:** None found
**Spec depth:** 6 files (shapes with 10+ types, connections with 2 types, 3 layout algorithms, data binding, zoom/pan/select, 2 events)
**Known gaps:** ~55 total (18 Critical, 22 Important, 15 Nice-to-have)
**Intake record:** `workspaces/diagram-gap-analysis/output/stage-01/gap-diagram-intake-inventory.md`
**Recommendation:** Standard mode. **Merits dedicated delivery workspace** — complexity rivals DataGrid. Requires heavy JS interop for rendering engine.

### 3. MariloDockManager
**Status:** No source code (greenfield)
**Source:** None found
**Spec depth:** 6 files (3 pane types with ~35 pane-level params, floating panes, state management, 10 events, WAI-ARIA delegation)
**Known gaps:** ~47 total (15 Critical, 20 Important, 12 Nice-to-have)
**Intake record:** `workspaces/dockmanager-gap-analysis/output/stage-01/gap-dockmanager-intake-inventory.md`
**Recommendation:** Standard mode. **Merits dedicated delivery workspace** — drag-and-dock complexity, multi-component composition.

### 4. MariloEditor
**Status:** Implemented (670 lines, 18 params, 7 tests, no demo pages)
**Source:** `/workspaces/Marilo/src/Marilo.Components/Editors/MariloEditor.razor`
**Implemented:** WYSIWYG/Source/Preview modes, toolbar, debounced value binding, HTML sanitization
**Missing (Critical):** ProseMirror schema/plugin customization, ExecuteAsync method, import/export
**Missing (Important):** Adaptive toolbar, table/image resizing
**Intake record:** `stages/01-intake/output/gap-editor-inventory.md`
**Severity estimate:** 3 Critical, 5 Important, 4 Nice-to-have (~12 total)
**Recommendation:** Reconstructed mode.

### 5. MariloFileManager
**Status:** Implemented (170 lines, 12 parameters, single .razor, no code-behind, no tests)
**Source:** `/workspaces/Marilo/src/Marilo.Components/Forms/Inputs/MariloFileManager.razor`
**Spec depth:** 15 files (navigation, views, toolbar, context menu, search)
**Implemented:** Grid/List views, folder tree sidebar, nav up/into, file selection, create/delete callbacks, size formatting
**Missing (Critical):** Generic data binding, upload/download, OnRead event, parameter naming mismatches (`CurrentPath` vs `Path`)
**Missing (Important):** Context menu, preview pane, breadcrumb nav, toolbar, rename, OnModelInit, Height/Width/Class
**Severity estimate:** 6-8 Critical, 9-12 Important, 5-8 Nice-to-have (~22-28 total)
**Recommendation:** Reconstructed mode. Complex component (file browser UX).

### 6. MariloGantt
**Status:** Minimal scaffold (95 lines, 6 params, no tests, no demo pages)
**Source:** `/workspaces/Marilo/src/Marilo.Components/DataDisplay/MariloGantt.razor`
**Implemented:** Flat task list, day-only timeline, static bars, two events — barely functional
**Missing (Critical):** Generic data binding, hierarchical tree, timeline views, CRUD operations, drag editing
**Missing (Important):** Sorting, filtering, templates, toolbar, dependencies, Rebind
**Intake record:** `stages/01-intake/output/gap-gantt-inventory.md`
**Severity estimate:** 8 Critical, 7 Important, 5 Nice-to-have (~20 gaps, but spec has 44 files so full audit will likely reveal more)
**Recommendation:** Reconstructed mode. **Merits dedicated delivery workspace** given spec depth and minimal scaffold.

### 7. MariloMap
**Status:** No source code (greenfield)
**Source:** None found
**Spec depth:** 7 files (4 layer types: Tile/Marker/Shape-GeoJSON/Bubble, 12 core params, 5 events, CSP-compliant JS templates, marker customization)
**Known gaps:** ~35 total (12 Critical, 14 Important, 9 Nice-to-have)
**Intake record:** `workspaces/map-gap-analysis/output/stage-01/gap-map-intake-inventory.md`
**Recommendation:** Standard mode. **Merits dedicated delivery workspace** — tile rendering engine, GeoJSON parsing. Smallest of the no-source group.

### 8. MariloPivotGrid
**Status:** No source code (greenfield)
**Source:** None found
**Spec depth:** 5 files (4-component system: Grid/Configurator/ConfiguratorButton/Container, 2 data providers: Local + XMLA/OLAP, configurator with TreeView + drag-reorder, 3 cell templates)
**Known gaps:** ~39 total (14 Critical, 16 Important, 9 Nice-to-have)
**Intake record:** `workspaces/pivotgrid-gap-analysis/output/stage-01/gap-pivotgrid-intake-inventory.md`
**Recommendation:** Standard mode. **Merits dedicated delivery workspace** — aggregation engine, dual-provider architecture.

### 9. MariloScheduler
**Status:** Implemented (182 lines, 10 parameters, single .razor, no code-behind, no tests)
**Source:** `/workspaces/Marilo/src/Marilo.Components/DataDisplay/MariloScheduler.razor`
**Spec depth:** 25 files (views, events, appointments, resources, templates)
**Implemented:** 3 views (Day/Week/Month), basic navigation, appointment rendering with color, click callbacks
**Missing (Critical):** Editing/CRUD, generic data binding (hardcoded `SchedulerAppointment`), recurrence, resource grouping
**Missing (Important):** Timeline/Agenda/Multiday views, templates, loader, Height/Width, keyboard nav
**Severity estimate:** 8 Critical, 10 Important, 7-12 Nice-to-have (~25-30 total)
**Recommendation:** Reconstructed mode. **Merits dedicated delivery workspace** given complexity.

### 10. MariloSplitter
**Status:** Implemented (568 lines across 2 files, 22 params, no tests, no demo pages)
**Source:** `/workspaces/Marilo/src/Marilo.Components/Layout/MariloSplitter.razor` (500 lines) + `MariloSplitterPane.razor` (68 lines)
**Implemented:** Pane registration, drag resize, collapse toggle, keyboard resize, ARIA attributes
**Missing (Critical):** SplitterPanes wrapper tag, GetState/SetState methods, SplitterOrientation enum
**Missing (Important):** Per-pane Min/Max constraints
**Intake record:** `stages/01-intake/output/gap-splitter-inventory.md`
**Severity estimate:** 3 Critical, 4 Important, 3 Nice-to-have (~10 total)
**Recommendation:** Reconstructed mode. Mostly complete — fewest gaps in batch.

### 11. MariloTreeList
**Status:** Implemented (199 lines, 6 parameters, single .razor generic `TItem`, no code-behind, no tests)
**Source:** `/workspaces/Marilo/src/Marilo.Components/DataGrid/MariloTreeList.razor`
**Spec depth:** 55 files (the second largest spec — columns, editing, filtering, sorting, hierarchy, templates)
**Implemented:** Flat + hierarchical data binding, expand/collapse, column rendering with width, row click
**Missing (Critical):** Paging, sorting, filtering, editing/CRUD, selection, load-on-demand
**Missing (Important):** Column reorder/resize/freeze/checkbox/command, templates, toolbar, state, keyboard nav
**Severity estimate:** 12-15 Critical, 15-20 Important, 10-15 Nice-to-have (~40-50 total)
**Recommendation:** Reconstructed mode. **Merits dedicated delivery workspace** — essentially DataGrid+TreeView hybrid.

### 12. MariloWizard
**Status:** Implemented (125 lines, 9 parameters, single .razor + WizardStep companion, no tests)
**Source:** `/workspaces/Marilo/src/Marilo.Components/Layout/MariloWizard.razor`
**Spec depth:** 9 files (steps, navigation, validation, templates)
**Implemented:** Step registration, Previous/Next/Finish nav, configurable button text, AllowStepClick, disabled steps, ARIA
**Missing (Critical):** Naming mismatch (`ActiveStepIndex` vs spec's `Value`), WizardSteps container tag, Content render fragment, form validation integration
**Missing (Important):** Icon support, ShowPager, StepperPosition, Height/Width/Class, cancellable events
**Severity estimate:** 4 Critical, 7 Important, 4-7 Nice-to-have (~15-18 total)
**Recommendation:** Reconstructed mode. Moderate complexity.

---

## Severity Breakdown (Aggregate)

| Severity | Estimated Count | Notes |
|----------|----------------|-------|
| Critical | ~90-110 | 4 no-source components (~59 alone); core API gaps in implemented components |
| Important | ~100-140 | Event signatures, template slots, configuration params |
| Nice-to-have | ~70-100 | ARIA polish, CSS provider alignment, advanced features |
| **Total** | **~280-350** | Refined estimate across all 12 (4 no-source = ~176; 8 with source = ~100-175) |

---

## Recommended Prioritization Order

**Tier 1 — Near-complete (quick wins):**
1. MariloSplitter (8 gaps, mostly done)
2. MariloWizard (10-20 gaps, moderate complexity)

**Tier 2 — Substantial code, needs spec alignment:**
3. MariloChart (27 gaps, well-documented in plan)
4. MariloEditor (54 gaps, JS interop complexity)
5. MariloFileManager (20-30 gaps)

**Tier 3 — Large spec surface, needs dedicated workspace:**
6. MariloScheduler (25-40 gaps) — **recommend dedicated CDW**
7. MariloGantt (30-50 gaps) — **recommend dedicated CDW**
8. MariloTreeList (35-55 gaps) — **recommend dedicated CDW**

**Tier 4 — No source, architecture decisions needed:**
9. MariloDiagram (no source)
10. MariloDockManager (no source)
11. MariloMap (no source)
12. MariloPivotGrid (no source)

---

## Components Recommended for Dedicated Delivery Workspace

| Component | Reason | Spec Files | Est. Gaps |
|-----------|--------|-----------|-----------|
| MariloGantt | Tasks+dependencies+timeline, complex editing | 44 | 30-50 |
| MariloScheduler | Multiple views, appointments, resources, CRUD | 25 | 25-30 |
| MariloTreeList | DataGrid+TreeView hybrid, largest spec in batch | 55 | 40-50 |
| MariloDiagram | Shapes+connections+layouts, rendering engine | 6 | ~55 |
| MariloDockManager | Drag-and-dock, floating panes, state mgmt | 6 | ~47 |
| MariloMap | Tile engine, GeoJSON parsing, 4 layer types | 7 | ~35 |
| MariloPivotGrid | Aggregation engine, dual-provider, configurator | 5 | ~39 |

7 of the 12 components merit dedicated workspaces. The 4 no-source components already have gap-analysis workspace stubs with intake inventories written by this run. The 3 with-source components (Gantt, Scheduler, TreeList) exceed the complexity threshold (>20 spec files or >30 gaps) that warranted dedicated CDWs for TreeView, DataGrid, and DataSheet.
