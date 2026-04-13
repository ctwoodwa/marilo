# PivotGrid Spec Gaps — Stage 01 Spec Review

**Date:** 2026-04-12
**Spec files audited:** overview.md, data-binding.md, configurator.md, templates.md, accessibility/wai-aria-support.md
**Source file:** src/Marilo.Components/DataGrid/MariloPivotGrid.razor
**Models file:** src/Marilo.Core/Models/PivotGridModels.cs

---

## Summary

The spec describes a rich, multi-component PivotGrid ecosystem (MariloPivotGrid, MariloPivotGridConfigurator, MariloPivotGridConfiguratorButton, MariloPivotGridContainer) with XMLA support, templates, filtering, sorting, and accessibility. The current source implementation is a minimal single-component scaffold that covers only basic local data pivoting. The gap between spec and source is substantial.

---

## Spec-Ahead Gaps (Spec defines, source does NOT implement)

| ID | Feature Area | Spec Reference | Gap Description | Severity |
|----|-------------|----------------|-----------------|----------|
| SG-01 | Sub-components | overview.md §Components | `MariloPivotGridConfigurator`, `MariloPivotGridConfiguratorButton`, and `MariloPivotGridContainer` are defined in spec but do not exist in source | Critical |
| SG-02 | Data binding: XMLA | data-binding.md §XMLA | XMLA data provider type, `PivotGridSettings`, `PivotGridXmlaDataProviderSettings`, `PivotGridXmlaDataProviderCredentials` are spec-only. Source has no XMLA support | Critical |
| SG-03 | Data binding: DataProviderType enum | overview.md §Grid Parameters | Spec defines `PivotGridDataProviderType` enum (Local, Xmla). Source uses no such enum; always assumes local data | High |
| SG-04 | Row/Column/Measure child components | overview.md §Creating | Spec uses `<PivotGridColumns>`, `<PivotGridRows>`, `<PivotGridMeasures>` with child `<PivotGridColumn>`, `<PivotGridRow>`, `<PivotGridMeasure>` tags. Source uses `List<PivotGridField>` parameters instead | Critical |
| SG-05 | Measure: Aggregate per-measure | overview.md §Row, Column and Measure Parameters | Spec puts `Aggregate` on individual `PivotGridMeasure` tags. Source has a single `AggregateFunction` parameter on the grid | High |
| SG-06 | Measure: Format parameter | overview.md §Row, Column and Measure Parameters | Spec defines `Format` on PivotGridMeasure (e.g. `"{0:C2}"`). Source has no format support | Medium |
| SG-07 | HeaderClass parameter | overview.md §Row, Column and Measure Parameters | Spec defines `HeaderClass` on rows/columns/measures. Source has no header class support | Medium |
| SG-08 | Class parameter | overview.md §Grid Parameters | Spec defines `Class` on grid, configurator, button, container. Source inherits from MariloComponentBase (may get Class via AdditionalAttributes) but does not explicitly declare it | Low |
| SG-09 | ColumnHeadersWidth | overview.md §Grid Parameters | Spec defines `ColumnHeadersWidth`. Source does not implement | Medium |
| SG-10 | RowHeadersWidth | overview.md §Grid Parameters | Spec defines `RowHeadersWidth`. Source does not implement | Medium |
| SG-11 | EnableLoaderContainer | overview.md §Grid Parameters | Spec defines `EnableLoaderContainer` on grid and configurator. Source does not implement | Medium |
| SG-12 | LoadOnDemand | overview.md §Grid Parameters | Spec defines `LoadOnDemand` parameter. Source does not implement | Medium |
| SG-13 | Rebind method | overview.md §PivotGrid Reference and Methods | Spec documents `Rebind()` method. Source does not expose it | Medium |
| SG-14 | Templates | templates.md | `ColumnHeaderTemplate`, `DataCellTemplate`, `RowHeaderTemplate` with typed template contexts (`PivotGridColumnHeaderTemplateContext`, `PivotGridDataCellTemplateContext`, `PivotGridRowHeaderTemplateContext`). None exist in source | High |
| SG-15 | Configurator behavior | configurator.md | Fields TreeView, drag-and-drop between sections, Apply/Cancel buttons, chip-based columns/rows/values sections. None exist in source | Critical |
| SG-16 | Accessibility / WAI-ARIA | accessibility/wai-aria-support.md | Spec defines comprehensive ARIA roles, attributes, and keyboard navigation for grid and configurator. Source has no ARIA attributes | High |
| SG-17 | Aggregate type enum | overview.md §Row, Column and Measure Parameters | Spec uses `PivotGridAggregateType` (on PivotGridMeasure). Source uses `PivotGridAggregateFunction` (on grid level). Enum name mismatch | Medium |
| SG-18 | Generic TItem | overview.md §Grid Parameters | Spec defines `TItem` type parameter on `MariloPivotGrid<TItem>`. Source component is non-generic, accepts `IEnumerable<object>` | High |

## Source-Ahead Gaps (Source implements, spec does NOT describe)

| ID | Feature | Source Location | Gap Description | Severity |
|----|---------|----------------|-----------------|----------|
| XG-01 | Sortable parameter | MariloPivotGrid.razor L73 | `Sortable` bool parameter sorts row/column keys alphabetically. Not documented in spec | Low |
| XG-02 | Filterable parameter | MariloPivotGrid.razor L76 | `Filterable` bool parameter is declared but has no implementation. Not in spec | Low |
| XG-03 | BEM class structure | MariloPivotGrid.razor | Uses `mar-pivotgrid`, `mar-pivotgrid__empty`, `mar-pivotgrid__scroll`, `mar-pivotgrid__table`, `mar-pivotgrid__corner`, `mar-pivotgrid__col-header`, `mar-pivotgrid__row-header`, `mar-pivotgrid__cell`. Spec references `k-pivotgrid` (Kendo-style) classes | Medium |
| XG-04 | PivotGridField model | PivotGridModels.cs | `PivotGridField` class with Name/Title used as List parameter. Spec uses child component approach instead | Medium |

## API Mismatches

| ID | Area | Spec API | Source API | Impact |
|----|------|----------|------------|--------|
| AM-01 | Data input | `Data` as `IEnumerable<TItem>` with `<PivotGridColumns>/<PivotGridRows>/<PivotGridMeasures>` child tags | `Data` as `IEnumerable<object>` with `RowFields`/`ColumnFields`/`MeasureFields` as `List<PivotGridField>` | Breaking: completely different API shape |
| AM-02 | Aggregate | Per-measure `Aggregate` parameter (PivotGridAggregateType enum) | Grid-level `AggregateFunction` parameter (PivotGridAggregateFunction enum) | Breaking: different granularity and enum name |
| AM-03 | CSS classes | `k-pivotgrid`, `k-pivotgrid-configurator`, etc. (Kendo-derived) | `mar-pivotgrid`, `mar-pivotgrid__*` (BEM) | Non-breaking but affects visual parity and theming |

---

## Conclusion

The PivotGrid source is an early scaffold that demonstrates basic pivot table rendering with local data. The spec describes a fully-featured enterprise component with XMLA support, a configurator UI, templates, accessibility, and a child-component-based API. **18 spec-ahead gaps** and **4 source-ahead gaps** were identified. The API shape between spec and source is fundamentally different (child components vs. list parameters), which constitutes the most significant alignment issue.
