# Gap Inventory: MariloGantt

> Imported: 2026-04-03
> Analysis mode: Reconstructed (code exists before gap analysis)
> Total gaps: ~20 (8 Critical, 7 High, 5 Medium)

---

## Component Inventory

| Attribute | Value |
|-----------|-------|
| **Source files** | `MariloGantt.razor` (95 lines) |
| **Code-behind partials** | None |
| **Public parameters** | 6 (Data, TaskListWidth, DayWidth, RowHeight, OnTaskClick, OnTaskEdit) |
| **Tests** | None found |
| **Demos** | No demo pages found |
| **Spec** | `docs/component-specs/gantt/overview.md` |

---

## Gap Summary

The Gantt spec describes a full-featured project management component with a tree-list left pane (GanttColumns, hierarchical data via IdField/ParentIdField), a timeline right pane with multiple views (Day/Week/Month/Year), CRUD operations (OnUpdate/OnDelete/OnCreate), sorting, filtering, templates, toolbar, drag-to-resize timeline bars, progress tracking, dependencies, and the Rebind() method. The current implementation is a minimal 95-line scaffold: a flat task list, simple day-based timeline header, bar rendering with progress and dependencies, and two events. This is the least mature of the four components with the largest gap surface.

### GAP-GANTT-001: Missing hierarchical data binding (IdField/ParentIdField)

**Area:** MariloGantt
**Severity:** Critical
**Theme:** missing-core-feature
**Source:** gantt/overview.md -- Creating Blazor Gantt steps 2, code examples

**Target behavior:** `IdField` and `ParentIdField` parameters enable self-referencing flat data to render as a hierarchy. Data is generic `TItem`.
**Current behavior:** `Data` is typed as `IEnumerable<GanttTask>` (concrete type). Hierarchy simulated only via `Level` property for indentation.
**Impact:** Core data binding model incompatible with spec. Cannot use arbitrary model types.
**Recommended direction:** Make component generic (`MariloGantt<TItem>`), add IdField/ParentIdField, build tree from flat data.
**Status:** Open

---

### GAP-GANTT-002: Missing GanttColumns/GanttColumn child components

**Area:** MariloGantt
**Severity:** Critical
**Theme:** missing-child-component
**Source:** gantt/overview.md -- step 5, code examples

**Target behavior:** `GanttColumns` wrapper with `GanttColumn` children defining Field, visibility, format, alignment.
**Current behavior:** Task list renders only `Title` hardcoded; no column configuration.
**Impact:** Cannot display Start, End, PercentComplete or custom fields in the tree list.
**Recommended direction:** Create GanttColumn component with Field parameter and column rendering engine.
**Status:** Open

---

### GAP-GANTT-003: Missing GanttViews (Day/Week/Month/Year)

**Area:** MariloGantt
**Severity:** Critical
**Theme:** missing-child-component
**Source:** gantt/overview.md -- step 4, code examples

**Target behavior:** `GanttViews` wrapper with `GanttDayView`, `GanttWeekView`, `GanttMonthView`, `GanttYearView` children.
**Current behavior:** Only day-level timeline rendering hardcoded; no view switching.
**Impact:** Cannot zoom timeline to different granularities.
**Recommended direction:** Create view components and timeline rendering engine supporting multiple scales.
**Status:** Open

---

### GAP-GANTT-004: Missing CRUD operations (OnUpdate/OnDelete/OnCreate)

**Area:** MariloGantt
**Severity:** Critical
**Theme:** missing-events
**Source:** gantt/overview.md -- step 6, Editing section

**Target behavior:** `OnUpdate`, `OnDelete`, `OnCreate` EventCallbacks with typed event args for tree list and timeline editing.
**Current behavior:** Only `OnTaskClick` and `OnTaskEdit` events; no CRUD event args types.
**Impact:** Cannot perform inline editing, deletion, or creation of tasks.
**Recommended direction:** Add CRUD events with GanttUpdateEventArgs/GanttDeleteEventArgs/GanttCreateEventArgs.
**Status:** Open

---

### GAP-GANTT-005: Missing Width/Height parameters

**Area:** MariloGantt
**Severity:** Critical
**Theme:** missing-parameter
**Source:** gantt/overview.md -- step 3

**Target behavior:** `Width` and `Height` parameters for component sizing.
**Current behavior:** No Width/Height parameters; relies on container sizing.
**Impact:** Cannot declaratively size the Gantt chart.
**Recommended direction:** Add Width/Height string parameters applied to container style.
**Status:** Open

---

### GAP-GANTT-006: Missing Rebind() method

**Area:** MariloGantt
**Severity:** Critical
**Theme:** missing-public-method
**Source:** gantt/overview.md -- Gantt Reference and Methods

**Target behavior:** `Rebind()` method to programmatically refresh data.
**Current behavior:** No public methods.
**Impact:** Cannot trigger data refresh after external data changes.
**Recommended direction:** Add Rebind() method that re-evaluates the Data collection.
**Status:** Open

---

### GAP-GANTT-007: Missing sorting support

**Area:** MariloGantt
**Severity:** High
**Theme:** missing-feature
**Source:** gantt/overview.md -- Sorting section

**Target behavior:** Automatic sorting of tree list columns.
**Current behavior:** No sorting implementation.
**Impact:** Cannot reorder tasks by field values.
**Recommended direction:** Add sortable column headers with sort state management.
**Status:** Open

---

### GAP-GANTT-008: Missing filtering support

**Area:** MariloGantt
**Severity:** High
**Theme:** missing-feature
**Source:** gantt/overview.md -- Filtering section

**Target behavior:** Automatic filtering of tree list data.
**Current behavior:** No filtering implementation.
**Impact:** Cannot filter tasks in large datasets.
**Recommended direction:** Add filter row or filter menu to GanttColumn.
**Status:** Open

---

### GAP-GANTT-009: Missing timeline templates

**Area:** MariloGantt
**Severity:** High
**Theme:** missing-feature
**Source:** gantt/overview.md -- Templates section

**Target behavior:** Templates for customizing timeline bar rendering.
**Current behavior:** Hardcoded bar rendering with basic progress fill.
**Impact:** Cannot customize task bar appearance.
**Recommended direction:** Add TaskTemplate RenderFragment parameter.
**Status:** Open

---

### GAP-GANTT-010: Missing toolbar

**Area:** MariloGantt
**Severity:** High
**Theme:** missing-feature
**Source:** gantt/overview.md -- Toolbar section

**Target behavior:** Dedicated toolbar for user actions (add task, view switching, etc.).
**Current behavior:** No toolbar.
**Impact:** No UI for common Gantt actions.
**Recommended direction:** Add GanttToolbar component or ToolbarTemplate parameter.
**Status:** Open

---

### GAP-GANTT-011: Missing timeline editing (drag resize/move)

**Area:** MariloGantt
**Severity:** Critical
**Theme:** missing-feature
**Source:** gantt/overview.md -- Editing section, Timeline editing

**Target behavior:** Drag to resize task bars (change duration) and drag to move (change dates).
**Current behavior:** Bars are static; click event only.
**Impact:** Core interactive timeline editing missing.
**Recommended direction:** Add drag handlers for bar resize and move with JS interop.
**Status:** Open

---

### GAP-GANTT-012: Missing tree list editing

**Area:** MariloGantt
**Severity:** High
**Theme:** missing-feature
**Source:** gantt/overview.md -- Editing section, TreeList editing

**Target behavior:** Inline editing of task properties in tree list cells.
**Current behavior:** No inline editing capability.
**Impact:** Cannot edit task metadata directly in the grid.
**Recommended direction:** Add cell editing mode with edit templates.
**Status:** Open

---

### GAP-GANTT-013: Missing dependency rendering with SVG markers

**Area:** MariloGantt
**Severity:** High
**Theme:** incomplete-feature
**Source:** gantt/overview.md -- Dependencies section; source uses `marker-end="url(#arrow)"`

**Target behavior:** Proper SVG arrow markers and dependency line routing between tasks.
**Current behavior:** SVG lines reference `#arrow` marker that is never defined; lines drawn as simple straight lines.
**Impact:** Dependency arrows do not render correctly (missing arrowhead marker).
**Recommended direction:** Add SVG `<defs>` with arrow marker; improve line routing.
**Status:** Open

---

### GAP-GANTT-014: No test coverage

**Area:** MariloGantt
**Severity:** Critical
**Theme:** missing-tests
**Source:** No test files found in tests/

**Target behavior:** Tests covering data binding, hierarchy, views, CRUD events, bar rendering.
**Current behavior:** Zero tests.
**Impact:** All functionality untested.
**Recommended direction:** Create GanttTests.cs with basic rendering and data binding tests.
**Status:** Open

---

### GAP-GANTT-015: No demo pages

**Area:** MariloGantt
**Severity:** High
**Theme:** missing-demos
**Source:** samples/Marilo.Demo/Pages/Components/Gantt/ (directory absent)

**Target behavior:** Demo pages showing basic Gantt, hierarchical data, CRUD, view switching.
**Current behavior:** No demo directory or pages exist.
**Impact:** No way to preview or validate Gantt functionality.
**Recommended direction:** Create demo pages for core scenarios.
**Status:** Open

---

### GAP-GANTT-016: Missing scrolling synchronization

**Area:** MariloGantt
**Severity:** Medium
**Theme:** missing-feature
**Source:** gantt/overview.md -- Scrolling section

**Target behavior:** Synchronized vertical scrolling between tree list and timeline; horizontal scroll on timeline.
**Current behavior:** Basic `overflow-x: auto` on timeline; no synchronized vertical scroll.
**Impact:** Tree list and timeline can scroll out of alignment.
**Recommended direction:** Add JS interop scroll synchronization.
**Status:** Open

---

### GAP-GANTT-017: Missing Gantt Tree component architecture

**Area:** MariloGantt
**Severity:** Medium
**Theme:** architecture
**Source:** gantt/overview.md -- Gantt Tree section

**Target behavior:** The tree list is described as a separate architectural concept with its own data binding.
**Current behavior:** Simple foreach loop over flat task list with Level-based indentation.
**Impact:** No proper tree expand/collapse, no hierarchical data management.
**Recommended direction:** Integrate TreeList-like component or build tree data structure.
**Status:** Open

---

### GAP-GANTT-018: Missing GanttTask model completeness

**Area:** MariloGantt
**Severity:** Medium
**Theme:** model-gaps
**Source:** Source references GanttTask with Id, Title, Start, End, PercentComplete, IsMilestone, Color, Level, DependsOn

**Target behavior:** Generic TItem model with field name parameters (IdField, ParentIdField, TitleField, etc.).
**Current behavior:** Concrete GanttTask type with fixed property names.
**Impact:** Cannot use custom model types; locked to GanttTask shape.
**Recommended direction:** Adopt generic TItem with reflection-based or expression-based field access.
**Status:** Open

---

### GAP-GANTT-019: Missing accessibility attributes

**Area:** MariloGantt
**Severity:** Medium
**Theme:** missing-accessibility
**Source:** General accessibility expectations

**Target behavior:** ARIA roles, labels, and keyboard navigation for tree list and timeline.
**Current behavior:** No role, aria-label, or keyboard navigation attributes.
**Impact:** Component not accessible to screen readers or keyboard-only users.
**Recommended direction:** Add role="treegrid" to tree, role="row"/role="gridcell" to rows, keyboard navigation.
**Status:** Open

---

### GAP-GANTT-020: Missing IAsyncDisposable and JS interop

**Area:** MariloGantt
**Severity:** Medium
**Theme:** architecture
**Source:** Other components implement IAsyncDisposable with JS interop

**Target behavior:** JS interop for drag operations, scroll sync, resize. IAsyncDisposable for cleanup.
**Current behavior:** No JS interop; no IAsyncDisposable. Pure Blazor rendering only.
**Impact:** Cannot implement interactive features (drag, scroll sync) without JS interop.
**Recommended direction:** Add IJSRuntime injection and JS module for interactive features.
**Status:** Open

---

## Severity Breakdown

| Severity | Count |
|----------|-------|
| Critical | 8 |
| High | 7 |
| Medium | 5 |
| Low | 0 |
| **Total** | **20** |
