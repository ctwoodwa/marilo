# Gap Intake Inventory: MariloPivotGrid

> Component: MariloPivotGrid
> Intake date: 2026-04-03
> Intake mode: Fresh analysis (assess mode) -- no source code exists
> Spec docs: 5 files (overview, configurator, data-binding, templates, accessibility/wai-aria)
> Demo page: Placeholder only ("Coming soon")

---

## 1. Source Code Status

**No source files exist.** A search of `/workspaces/Marilo/src/Marilo.Components/` found zero PivotGrid-related files. This is a **standard greenfield intake**.

## 2. Spec Summary

The spec documents a pivot table / multidimensional data analysis component:

- **Component system:** 4 integrated Razor components -- MariloPivotGrid, MariloPivotGridConfigurator, MariloPivotGridConfiguratorButton, MariloPivotGridContainer.
- **Grid parameters:** 10 -- Class, ColumnHeadersWidth, Data, DataProviderType, EnableLoaderContainer, Height, LoadOnDemand, RowHeadersWidth, TItem, Width.
- **Row/Column/Measure parameters:** 5 each -- Aggregate (measures only), Format (measures only), HeaderClass, Name, Title. Aggregate types via PivotGridAggregateType enum.
- **Configurator:** Fields section (TreeView with checkboxes), Columns/Rows sections (chips with drag reorder, sort, filter via context menu), Values section, Apply/Cancel buttons. Configurator params: Class, EnableLoaderContainer. Button params: Class.
- **Data binding:** 2 provider types -- Local (IEnumerable<TItem>, in-memory aggregation) and XMLA (remote OLAP cube with ServerUrl, Catalog, Cube, optional credentials with Domain/Password/Username). Load-on-demand support for XMLA.
- **Templates:** 3 templates -- ColumnHeaderTemplate, DataCellTemplate, RowHeaderTemplate, each with typed context objects.
- **Methods:** Rebind.
- **Accessibility:** Full WAI-ARIA spec with grid role, configurator dialog role, extensive aria attributes for column/row headers and data cells, Section 508 compliant, keyboard navigation, screen reader tested.

**Estimated parameter/feature count:** ~25 parameters, 3 templates, 2 data provider types, 1 method, 4 sub-components.

## 3. Demo Page Status

The demo page at `/workspaces/Marilo/samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor` is a **placeholder only** -- displays a "Coming soon" alert.

## 4. Rough Gap Count

| Feature Area | Estimated Gaps |
|---|---|
| Core grid rendering + aggregation engine | 5 |
| Local data provider (in-memory pivot) | 4 |
| XMLA data provider (remote OLAP) | 5 |
| Configurator (fields, columns, rows, values) | 6 |
| ConfiguratorButton + Container | 2 |
| Row/Column/Measure configuration | 4 |
| Templates (3 types) | 3 |
| Sorting and filtering in configurator | 3 |
| LoadOnDemand | 2 |
| Accessibility (ARIA grid + dialog) | 3 |
| Styling/theming | 2 |
| **Total** | **~39** |

## 5. Severity Breakdown Estimate

| Severity | Count | Examples |
|---|---|---|
| Critical | ~14 | Core aggregation engine, local data provider, grid rendering with row/column/data cells, basic row/column/measure config |
| Important | ~16 | Configurator (all sections), XMLA provider, templates, sorting/filtering, LoadOnDemand, Rebind method |
| Nice-to-have | ~9 | XMLA credentials, EnableLoaderContainer, HeaderClass, accessibility ARIA roles, Container Class param |

## 6. Delivery Workspace Recommendation

**YES -- merits its own delivery workspace.** The PivotGrid is a data-intensive analytical component requiring a pivot/aggregation engine, two distinct data provider backends (local + XMLA/OLAP), a multi-section configurator UI, and extensive accessibility requirements. A dedicated `pivotgrid-delivery/` workspace already exists. Scope: `systematic`.

---

**Next step:** Proceed to Stage 02 (prioritize) with the in-memory aggregation engine + local data provider as the critical foundation, then grid rendering, then configurator.
