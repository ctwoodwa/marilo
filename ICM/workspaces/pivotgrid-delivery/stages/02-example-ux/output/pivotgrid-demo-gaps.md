# PivotGrid Demo Gaps — Stage 02 Example UX

**Date:** 2026-04-12
**Demo file:** samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor
**Spec files:** overview.md, data-binding.md, configurator.md, templates.md, accessibility/wai-aria-support.md

---

## Summary

The demo page has two sections: "Basic Usage" and "Aggregate Functions". It demonstrates basic local data binding and aggregate function switching. Many spec-documented features have no demo coverage.

---

## Current Demo Coverage

| Section | What It Shows | Status |
|---------|--------------|--------|
| Basic Usage | PivotGrid with RowFields, ColumnFields, MeasureFields, Sum aggregation, Height | Functional |
| Aggregate Functions | Toggle buttons for Sum/Count/Average/Min/Max with Sortable=true | Functional |

## Missing Demo Scenarios

| ID | Feature Area | Spec Reference | Demo Gap Description | Priority |
|----|-------------|----------------|---------------------|----------|
| DG-01 | Configurator | configurator.md | No demo showing MariloPivotGridConfigurator, MariloPivotGridConfiguratorButton, or MariloPivotGridContainer. The configurator is a central UX feature | Critical |
| DG-02 | XMLA data binding | data-binding.md §XMLA | No demo for XMLA data provider type. Understandable since XMLA requires external OLAP setup, but at minimum a code snippet or placeholder is warranted | Medium |
| DG-03 | Templates | templates.md | No demo for ColumnHeaderTemplate, DataCellTemplate, or RowHeaderTemplate | High |
| DG-04 | Multiple measures | overview.md §Row, Column and Measure Parameters | Demo only shows a single measure field. No demo with multiple simultaneous measures | Medium |
| DG-05 | Multiple row/column fields | overview.md §Creating | Demo uses a single row field and single column field. No demo showing hierarchical (nested) row or column grouping | Medium |
| DG-06 | Format parameter | overview.md §Row, Column and Measure Parameters | No demo showing formatted measure values (e.g. currency format) | Medium |
| DG-07 | Rebind method | overview.md §PivotGrid Reference and Methods | No demo showing programmatic Rebind() with @ref | Low |
| DG-08 | LoadOnDemand | overview.md §Grid Parameters | No demo showing LoadOnDemand=false vs. true behavior | Low |
| DG-09 | ColumnHeadersWidth / RowHeadersWidth | overview.md §Grid Parameters | No demo showing width customization of headers | Low |
| DG-10 | Filtering | configurator.md §Columns and Rows | No demo for field value filtering (the Filterable parameter exists in source but is unused) | Medium |
| DG-11 | Drag-and-drop reordering | configurator.md §Buttons | No demo for reordering fields via drag | Low |
| DG-12 | EnableLoaderContainer | overview.md §Grid Parameters | No demo showing loader during long operations | Low |
| DG-13 | Empty state | MariloPivotGrid.razor | The source renders an empty-state message, but no demo explicitly shows this state | Low |
| DG-14 | Accessibility | accessibility/wai-aria-support.md | No demo or documentation showing keyboard navigation or screen reader behavior | Medium |

## API Alignment Issues in Demo

| ID | Issue | Description |
|----|-------|-------------|
| DA-01 | Demo uses source API, not spec API | Demo uses `RowFields`/`ColumnFields`/`MeasureFields` as `List<PivotGridField>` and grid-level `AggregateFunction`. Spec defines child component tags `<PivotGridRows>`/`<PivotGridColumns>`/`<PivotGridMeasures>` with per-measure `Aggregate`. Demo is aligned with current source but not with spec |
| DA-02 | Demo uses non-generic component | Demo passes `List<object>` data. Spec defines `MariloPivotGrid<TItem>` generic component |
| DA-03 | Inline button styling | Demo uses inline CSS for aggregate toggle buttons rather than Marilo components. Should use MariloButton or MariloButtonGroup when available |

---

## Conclusion

The demo covers 2 of ~14 identifiable feature scenarios. Critical gaps include the configurator, templates, and multiple row/column fields. The demo API shape matches the current source implementation but diverges from the spec's child-component-based API. The demo should be expanded significantly once the source catches up to the spec.
