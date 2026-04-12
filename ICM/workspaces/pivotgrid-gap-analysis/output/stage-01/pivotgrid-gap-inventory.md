# Gap Inventory: MariloPivotGrid — Stage 01 Intake (Revised)

> **Component:** MariloPivotGrid
> **Date:** 2026-04-12
> **Intake mode:** Import from delivery audit (pivotgrid-delivery stages 01-04)
> **Architecture decision:** Child component tags win (human-approved). Refactor from flat `List<PivotGridField>` to `<PivotGridRows>/<PivotGridColumns>/<PivotGridMeasures>` child-tag registration following the DataGrid CascadingValue pattern.
> **Prior intake:** 2026-04-03 (greenfield). This revision updates for the scaffold that now exists.

---

## 1. Current Source Status

Source exists as an **early scaffold** with basic local-data pivoting:

| File | Status |
|------|--------|
| `src/Marilo.Components/DataGrid/MariloPivotGrid.razor` | Exists — 170 LOC, single non-generic component |
| `src/Marilo.Core/Models/PivotGridModels.cs` | Exists — `PivotGridField` class + `PivotGridAggregateFunction` enum |
| `samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor` | Exists — basic demo matching source API |

**What the scaffold implements:**
- `IEnumerable<object>` data input (non-generic)
- `RowFields`, `ColumnFields`, `MeasureFields` as `List<PivotGridField>` parameters
- Grid-level `AggregateFunction` (single aggregate for all measures)
- Basic HTML table rendering with BEM classes (`mar-pivotgrid__*`)
- `Height`, `Width`, `Sortable`, `Filterable` (declared but non-functional) parameters
- Simple in-memory pivot via reflection-based property access
- Sum, Count, Average, Min, Max aggregation

**What the scaffold does NOT implement:**
- Generic `TItem` type parameter
- Child-tag API (`PivotGridColumn`, `PivotGridRow`, `PivotGridMeasure`)
- Sub-components (Configurator, ConfiguratorButton, Container)
- Templates (ColumnHeaderTemplate, DataCellTemplate, RowHeaderTemplate)
- XMLA data provider
- Format, HeaderClass, ColumnHeadersWidth, RowHeadersWidth, EnableLoaderContainer, LoadOnDemand
- Rebind() method
- ARIA attributes / keyboard navigation
- SCSS in any provider

---

## 2. Gap Inventory

All gaps imported from the delivery audit (stages 01-04), reframed with the child-tag architecture decision applied. The API shape mismatch (formerly the top blocker) is now a **resolved decision** — implementation direction is clear.

### 2.1 API Shape Gaps (DECISION RESOLVED — child tags win)

| ID | Gap | Severity | Source Ref | Resolution Direction |
|----|-----|----------|------------|---------------------|
| G-01 | **API shape refactor**: Source uses `List<PivotGridField>` params; spec requires `<PivotGridRows>/<PivotGridColumns>/<PivotGridMeasures>` child tags with `<PivotGridRow>/<PivotGridColumn>/<PivotGridMeasure>` children | Critical | AM-01, SG-04, C-01 | Follow DataGrid pattern: parent provides `CascadingValue<MariloPivotGrid<TItem>>`, child components call `RegisterRow/RegisterColumn/RegisterMeasure` in `OnInitialized`. Remove `RowFields`/`ColumnFields`/`MeasureFields` list parameters. |
| G-02 | **Generic TItem**: Source is non-generic (`IEnumerable<object>`); spec requires `MariloPivotGrid<TItem>` with strongly-typed `Data` parameter | Critical | SG-18, C-07 | Add `@typeparam TItem` to component. Change `Data` to `IEnumerable<TItem>`. Replace reflection-based property access with expression-based field binding. |
| G-03 | **Aggregate per-measure**: Source has grid-level `AggregateFunction`; spec puts `Aggregate` on individual `PivotGridMeasure` tags using `PivotGridAggregateType` enum | High | SG-05, AM-02, H-01 | Move aggregate to `PivotGridMeasure` component. Rename enum from `PivotGridAggregateFunction` to `PivotGridAggregateType`. Remove grid-level `AggregateFunction` parameter. |
| G-04 | **PivotGridField model removal**: `PivotGridField` class in `PivotGridModels.cs` is the flat-API model; child-tag pattern makes it obsolete | Medium | XG-04 | Delete `PivotGridField`. Keep `PivotGridAggregateType` (renamed). Row/Column/Measure state lives on the child components themselves. |

### 2.2 Missing Sub-Components

| ID | Gap | Severity | Source Ref | Notes |
|----|-----|----------|------------|-------|
| G-05 | **MariloPivotGridConfigurator**: Full configurator UI (fields TreeView, chip-based sections, drag-and-drop, sort/filter, Apply/Cancel) | Critical | SG-01, SG-15, C-02 | Largest single feature. Depends on TreeView and drag-and-drop infrastructure. Parameters: Class, EnableLoaderContainer. |
| G-06 | **MariloPivotGridConfiguratorButton**: Toggle button for configurator visibility | Medium | SG-01, C-02 | Small component. Parameter: Class. |
| G-07 | **MariloPivotGridContainer**: Wrapper for grid + configurator + button | Medium | SG-01, C-02 | RenderFragment container providing CascadingValue context for sibling coordination. Parameter: Class. |

### 2.3 Feature Gaps

| ID | Gap | Severity | Source Ref | Notes |
|----|-----|----------|------------|-------|
| G-08 | **ColumnHeaderTemplate** | High | SG-14, C-04 | `RenderFragment<PivotGridColumnHeaderTemplateContext>` |
| G-09 | **DataCellTemplate** | High | SG-14, C-04 | `RenderFragment<PivotGridDataCellTemplateContext>` |
| G-10 | **RowHeaderTemplate** | High | SG-14, C-04 | `RenderFragment<PivotGridRowHeaderTemplateContext>` |
| G-11 | **Format parameter on PivotGridMeasure** | Medium | SG-06, H-03 | String format e.g. `"{0:C2}"` |
| G-12 | **HeaderClass parameter** on Row/Column/Measure | Medium | SG-07, M-05 | Custom CSS class on header cells |
| G-13 | **ColumnHeadersWidth** | Medium | SG-09, M-01 | CSS width for column headers |
| G-14 | **RowHeadersWidth** | Medium | SG-10, M-01 | CSS width for row headers |
| G-15 | **EnableLoaderContainer** | Medium | SG-11, M-02 | Built-in loader during long operations |
| G-16 | **LoadOnDemand** | Medium | SG-12, M-03 | Deferred data loading (primarily for XMLA) |
| G-17 | **Rebind() method** | Medium | SG-13, M-04 | Programmatic refresh |
| G-18 | **DataProviderType enum** | High | SG-03 | `PivotGridDataProviderType` with Local and Xmla members |
| G-19 | **XMLA data provider** | Critical | SG-02, C-03 | `PivotGridXmlaDataProviderSettings` + credentials. Full remote OLAP support. |
| G-20 | **Filterable implementation** | Medium | XG-02, M-08 | Parameter declared but non-functional |

### 2.4 Accessibility Gaps

| ID | Gap | Severity | Source Ref | Notes |
|----|-----|----------|------------|-------|
| G-21 | **WAI-ARIA grid role** and attributes | High | SG-16, H-02 | `role="grid"`, `aria-colcount`, `aria-rowcount`, columnheader/rowheader roles |
| G-22 | **Keyboard navigation** | High | SG-16, H-02 | Arrow keys, Tab, Enter for cell navigation |
| G-23 | **Configurator ARIA dialog** | Medium | SG-16 | `role="dialog"` on configurator, focus trap |

### 2.5 Styling / Provider Gaps

| ID | Gap | Severity | Source Ref | Notes |
|----|-----|----------|------------|-------|
| G-24 | **FluentUI SCSS** for PivotGrid | High | C-05 | 8 BEM classes need provider rules: `mar-pivotgrid`, `__empty`, `__scroll`, `__table`, `__corner`, `__col-header`, `__row-header`, `__cell` |
| G-25 | **Bootstrap SCSS** for PivotGrid | Medium | C-05 | Same 8 classes |
| G-26 | **CSS class convention**: Source uses `mar-pivotgrid__*` (BEM); spec references `k-pivotgrid` (Kendo-style) | Low | AM-03, M-06 | Non-breaking. Spec CSS class refs should be updated to `mar-` prefix to match Marilo conventions. |

### 2.6 Test Gaps

| ID | Gap | Severity | Source Ref | Notes |
|----|-----|----------|------------|-------|
| G-27 | **No unit tests** exist for PivotGrid | High | C-06 | Need bUnit tests for aggregation logic, child-tag registration, template rendering |
| G-28 | **No visual parity tests** | Medium | M-09 | Need spec + snapshots in `tests/visual-parity/` |

### 2.7 Documentation / Demo Gaps

| ID | Gap | Severity | Source Ref | Notes |
|----|-----|----------|------------|-------|
| G-29 | **Sortable parameter undocumented** in spec | Low | XG-01, L-01 | Source-ahead: add to spec or remove if child-tag refactor obsoletes it |
| G-30 | **Demo: configurator scenario** | High | H-04 | Demo page needs configurator example |
| G-31 | **Demo: templates scenario** | High | H-05 | Demo page needs template examples |
| G-32 | **Demo: multiple row/column fields** | Medium | M-07 | Show hierarchical pivoting |
| G-33 | **Demo: empty state** | Low | L-02 | Show empty grid gracefully |

---

## 3. Summary

| Category | Count | Critical | High | Medium | Low |
|----------|-------|----------|------|--------|-----|
| API Shape (decision resolved) | 4 | 2 | 1 | 1 | 0 |
| Missing Sub-Components | 3 | 1 | 0 | 2 | 0 |
| Feature Gaps | 13 | 1 | 3 | 8 | 0 |
| Accessibility | 3 | 0 | 2 | 1 | 0 |
| Styling / Provider | 3 | 0 | 1 | 1 | 1 |
| Tests | 2 | 0 | 1 | 1 | 0 |
| Docs / Demo | 5 | 0 | 2 | 1 | 2 |
| **Total** | **33** | **4** | **10** | **15** | **3** |

The architecture decision (child tags win) resolves the top blocker. The critical path is: G-01 + G-02 (API shape refactor) first, then G-05 (Configurator) as the largest remaining feature.
