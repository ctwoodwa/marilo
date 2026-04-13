# Gap Prioritization: MariloPivotGrid — Stage 02

> **Component:** MariloPivotGrid
> **Date:** 2026-04-12
> **Input:** stage-01/pivotgrid-gap-inventory.md (33 gaps: 4 Critical, 10 High, 15 Medium, 3 Low)
> **Architecture decision:** Child tags win (human-approved)

---

## Wave Strategy

Four waves, sequenced by dependency order. Each wave is a self-contained deliverable that leaves the component in a working state.

---

## Wave 1: API Shape Refactor (Foundation)

**Goal:** Transform MariloPivotGrid from flat `List<PivotGridField>` parameters to child-tag registration using the DataGrid CascadingValue pattern. Make the component generic. This is the foundation every subsequent wave depends on.

**Pattern to follow:** `MariloDataGrid<TItem>` + `MariloGridColumn<TItem>` — parent provides `CascadingValue`, children call `RegisterXxx()` in `OnInitialized`.

| Gap ID | Description | Severity | Implementation Notes |
|--------|-------------|----------|---------------------|
| G-01 | Child-tag API refactor | Critical | Create `PivotGridRows` (RenderFragment), `PivotGridColumns` (RenderFragment), `PivotGridMeasures` (RenderFragment) as RenderFragment parameters on the grid. Create `PivotGridRow`, `PivotGridColumn`, `PivotGridMeasure` as child components with `[CascadingParameter] MariloPivotGrid<TItem> ParentGrid`. Each calls `ParentGrid.RegisterRow/Column/Measure(this)` on init. Remove `RowFields`/`ColumnFields`/`MeasureFields` list parameters. |
| G-02 | Generic TItem | Critical | Add `@typeparam TItem`. Change `Data` to `IEnumerable<TItem>`. Replace reflection-based `GetPropertyValue`/`GetNumericValue` with compiled expressions or `Func<TItem, object>` field selectors on child components. |
| G-03 | Aggregate per-measure | High | Move `Aggregate` property to `PivotGridMeasure`. Create `PivotGridAggregateType` enum (rename from `PivotGridAggregateFunction`). Remove grid-level `AggregateFunction` parameter. Update `BuildPivot()` to iterate registered measures and aggregate each independently. |
| G-04 | PivotGridField model removal | Medium | Delete `PivotGridField` class from `PivotGridModels.cs`. Keep renamed enum. State now lives on child component instances. |
| G-18 | DataProviderType enum | High | Create `PivotGridDataProviderType` enum (Local, Xmla). Add `DataProviderType` parameter to grid, default `Local`. XMLA implementation deferred to Wave 3 feature gaps; enum presence enables the API surface now. |

**Files touched:**
- `src/Marilo.Components/DataGrid/MariloPivotGrid.razor` — major refactor
- `src/Marilo.Core/Models/PivotGridModels.cs` — rename enum, delete PivotGridField, add DataProviderType enum
- New files: `PivotGridRow.razor`, `PivotGridColumn.razor`, `PivotGridMeasure.razor` (in `src/Marilo.Components/DataGrid/`)
- `samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor` — update to child-tag API

**Sync areas:** source, models, demo, spec (update CSS class refs from `k-` to `mar-`)

**Gate:** `dotnet build` passes. Demo page renders a pivot table using child-tag syntax. Old `List<PivotGridField>` API no longer compiles.

**Estimated scope:** ~300 LOC new/changed across 5-6 files.

---

## Wave 2: Missing Sub-Components (Configurator, Button, Container)

**Goal:** Add the three companion components that make PivotGrid a configurable tool. The Configurator is the largest single feature in the PivotGrid ecosystem.

**Depends on:** Wave 1 (child-tag registration pattern must exist so the Configurator can read and modify registered fields).

| Gap ID | Description | Severity | Implementation Notes |
|--------|-------------|----------|---------------------|
| G-07 | MariloPivotGridContainer | Medium | RenderFragment wrapper. Provides `CascadingValue` so siblings (Grid, Configurator, Button) can coordinate. Parameter: `Class`. Implement first — Configurator and Button need it. |
| G-06 | MariloPivotGridConfiguratorButton | Medium | Toggle button. Reads visibility state from Container context. Parameter: `Class`. |
| G-05 | MariloPivotGridConfigurator | Critical | Fields TreeView (all available fields with checkboxes), Columns section (chips, drag reorder, sort/filter), Rows section (chips, drag reorder, sort/filter), Values section (chips with aggregate picker), Apply/Cancel buttons. Parameters: `Class`, `EnableLoaderContainer`. Depends on TreeView component for fields list. |
| G-23 | Configurator ARIA dialog | Medium | `role="dialog"`, `aria-label`, focus trap when opened. Implement alongside G-05. |
| G-30 | Demo: configurator scenario | High | Add configurator demo to Overview.razor. |

**Files touched:**
- New: `MariloPivotGridContainer.razor`, `MariloPivotGridConfiguratorButton.razor`, `MariloPivotGridConfigurator.razor` (in `src/Marilo.Components/DataGrid/`)
- `samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor` — add configurator demo section

**Sync areas:** source, demo, spec

**Gate:** Configurator renders fields list, user can add/remove rows/columns/measures via UI, Apply button refreshes the grid. `dotnet build` passes.

**Estimated scope:** ~500-700 LOC new (Configurator is the bulk).

---

## Wave 3: Feature Gaps (Templates, Formatting, Data Binding, Methods)

**Goal:** Fill in the remaining API surface — templates, format strings, dimension parameters, LoadOnDemand, Rebind, and XMLA scaffolding.

**Depends on:** Wave 1 (child-tag API). Wave 2 (some features like EnableLoaderContainer interact with Configurator).

| Gap ID | Description | Severity | Implementation Notes |
|--------|-------------|----------|---------------------|
| G-08 | ColumnHeaderTemplate | High | `RenderFragment<PivotGridColumnHeaderTemplateContext>` on grid |
| G-09 | DataCellTemplate | High | `RenderFragment<PivotGridDataCellTemplateContext>` on grid |
| G-10 | RowHeaderTemplate | High | `RenderFragment<PivotGridRowHeaderTemplateContext>` on grid |
| G-11 | Format parameter | Medium | String format on `PivotGridMeasure`, applied in cell rendering |
| G-12 | HeaderClass parameter | Medium | CSS class on Row/Column/Measure, applied to rendered headers |
| G-13 | ColumnHeadersWidth | Medium | CSS width style on column header cells |
| G-14 | RowHeadersWidth | Medium | CSS width style on row header cells |
| G-15 | EnableLoaderContainer | Medium | Show loader during long pivot operations |
| G-16 | LoadOnDemand | Medium | Deferred calculation, primarily for XMLA |
| G-17 | Rebind() method | Medium | Public method to re-process data and refresh UI |
| G-19 | XMLA data provider | Critical | `PivotGridXmlaDataProviderSettings` + credentials model. Full XMLA/OLAP support. **Largest single feature in this wave.** |
| G-20 | Filterable implementation | Medium | Wire up the already-declared `Filterable` parameter |
| G-21 | WAI-ARIA grid role | High | `role="grid"`, `aria-colcount`, `aria-rowcount`, header roles |
| G-22 | Keyboard navigation | High | Arrow keys, Tab, Enter for cell navigation |
| G-31 | Demo: templates | High | Template examples in demo page |
| G-32 | Demo: multiple fields | Medium | Hierarchical pivoting example |
| G-33 | Demo: empty state | Low | Empty grid scenario |

**Sub-wave suggestion:** Split into 3a (Templates + Format + Rebind + dimension params), 3b (Accessibility), 3c (XMLA) to keep PRs reviewable.

**Files touched:**
- `MariloPivotGrid.razor` — template parameters, dimension props, Rebind, ARIA
- `PivotGridMeasure.razor` — Format parameter
- `PivotGridRow.razor` / `PivotGridColumn.razor` / `PivotGridMeasure.razor` — HeaderClass
- New: template context classes in `PivotGridModels.cs` or separate files
- New: XMLA provider classes
- Demo page updates

**Sync areas:** source, models, demo, spec, accessibility

**Gate:** Templates render custom content. Format strings apply. Rebind refreshes. ARIA roles present on rendered HTML. `dotnet build` + `dotnet test` pass.

**Estimated scope:** ~600-900 LOC across templates, accessibility, and XMLA.

---

## Wave 4: SCSS Parity + Tests + Cleanup

**Goal:** Provider styling, unit tests, visual parity tests, documentation sync.

**Depends on:** Waves 1-3 (need stable rendered HTML to style and test).

| Gap ID | Description | Severity | Implementation Notes |
|--------|-------------|----------|---------------------|
| G-24 | FluentUI SCSS | High | Rules for 8+ BEM classes: `mar-pivotgrid`, `__empty`, `__scroll`, `__table`, `__corner`, `__col-header`, `__row-header`, `__cell`. Plus configurator classes added in Wave 2. |
| G-25 | Bootstrap SCSS | Medium | Same classes, Bootstrap design tokens |
| G-26 | CSS class convention cleanup | Low | Ensure spec references use `mar-` prefix consistently |
| G-27 | Unit tests (bUnit) | High | Test aggregation engine, child-tag registration, template rendering, Rebind, empty state |
| G-28 | Visual parity tests | Medium | Spec + snapshots in `tests/visual-parity/` |
| G-29 | Sortable parameter docs | Low | Document in spec or remove if superseded |

**Files touched:**
- New: SCSS files in FluentUI and Bootstrap provider folders
- New: bUnit test files in `tests/Marilo.Tests.Unit/`
- New: visual parity specs/snapshots
- Spec updates for CSS class references

**Sync areas:** SCSS, tests, spec, visual-parity

**Gate:** `dotnet build` + `dotnet test` pass. FluentUI SCSS renders styled pivot table. Visual parity baseline captured.

**Estimated scope:** ~400-500 LOC (SCSS + tests).

---

## Dependency Graph

```
Wave 1 (API Shape)
  │
  ├──> Wave 2 (Sub-Components)
  │       │
  │       └──> Wave 3 (Features)
  │               │
  │               └──> Wave 4 (SCSS + Tests)
  │
  └──> Wave 4 can partially start after Wave 1
       (basic SCSS for grid, tests for aggregation engine)
```

**Critical path:** Wave 1 -> Wave 2 -> Wave 3 -> Wave 4

**Parallelization opportunity:** SCSS for the base grid (G-24 partial) and basic aggregation tests (G-27 partial) can start after Wave 1 completes, in parallel with Wave 2.

---

## Risk Assessment

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| Child-tag registration breaks existing demo | High | Low | Demo is minimal; update in same PR as Wave 1 |
| Configurator depends on TreeView which may have its own gaps | Medium | High | Check TreeView readiness before Wave 2. If blocked, deliver Configurator shell with manual field config first. |
| XMLA provider requires external OLAP server for testing | High | Medium | Implement interface + local mock. Defer live XMLA testing to integration test phase. |
| Generic TItem refactor breaks reflection-based aggregation | High | Low | Replace with expression trees or `Func<TItem, object>` — well-established pattern in DataGrid. |

---

## Acceptance Criteria

| Wave | Acceptance |
|------|-----------|
| Wave 1 | `<PivotGridRows>/<PivotGridRow>` etc. compile and render. Old flat API removed. Component is generic. Per-measure aggregation works. |
| Wave 2 | Configurator opens/closes via button. Fields list shows available fields. User can add/remove rows/columns/measures. Apply refreshes grid. |
| Wave 3 | Templates render custom content. Format strings display formatted values. ARIA attributes present. Rebind method works. XMLA provider compiles (integration testing deferred). |
| Wave 4 | FluentUI SCSS styles the grid. bUnit tests cover core scenarios. Visual parity baseline exists. |
