# Stage 02 — Example UX Gap List: MariloPivotGrid
**Date:** 2026-04-11
**Demo page audited:** `samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor`
**Spec areas cross-referenced:** overview, data-binding, templates, configurator, accessibility

---

## Preflight Note: Demo Uses Stub API

The demo page targets the stub-level API (`RowFields`, `ColumnFields`, `MeasureFields`, `AggregateFunction`), not the spec-compliant child-component API (`<PivotGridRow>`, `<PivotGridColumn>`, `<PivotGridMeasure>`). Most gaps below are **Blocked-by-source** because the spec features are not implemented yet. Gaps for features that are partly in the stub but missing from the demo are marked **Missing** or **Partial**.

---

## Demo Scenario Inventory

The current demo page (`Overview.razor`) contains two sections:

| # | Section Title | Parameters Demonstrated |
|---|---------------|------------------------|
| 1 | Basic Usage (line 6–13) | `Data`, `RowFields`, `ColumnFields`, `MeasureFields`, `AggregateFunction=Sum`, `Height` |
| 2 | Aggregate Functions (line 16–33) | Same + `AggregateFunction` (interactive toggle), `Sortable=true` |

Neither section meets the full demo-scenario-format requirements:
- No parameter table per scenario.
- No spec section cross-reference.
- No code snippet panel (strings `_basicCode`/`_countCode` are provided but are already stale — they use stub API, not spec API).
- No disabled/read-only/empty/error states demonstrated.

---

## Gap Records

### Spec Area: overview — Grid Parameters

**GAP-UX-001**
**Type:** Partial
**Spec area:** overview — Grid Parameters
**Spec file:** `docs/component-specs/pivotgrid/overview.md`, lines 157–168
**Demo file:** `samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor`, lines 6–33

Coverage status per spec parameter:

| Parameter | Covered? | Notes |
|-----------|----------|-------|
| `Class` | No | Not present in any demo section; no CSS customization scenario |
| `ColumnHeadersWidth` | No | Not in source or demo |
| `Data` | Yes (partial) | Demonstrated in both sections, but uses `IEnumerable<object>` not `IEnumerable<TItem>` |
| `DataProviderType` | No | Not in source; blocked-by-source |
| `EnableLoaderContainer` | No | Not in source; blocked-by-source |
| `Height` | Yes | Used in both sections (line 12, 31) |
| `LoadOnDemand` | No | Not in source; blocked-by-source |
| `RowHeadersWidth` | No | Not in source or demo |
| `TItem` | No | Source uses `object`; blocked-by-source |
| `Width` | No | Parameter exists in source (line 70) but never used in any demo section |

**Missing scenarios to add (once source ready):**
- CSS class override scenario using `Class`
- `ColumnHeadersWidth` / `RowHeadersWidth` side-by-side dimension control demo
- `DataProviderType` toggle between Local and XMLA
- `EnableLoaderContainer` demo with a slow data source
- `LoadOnDemand` demo with XMLA
- `Width` demo — component is never constrained by width in current demo

---

**GAP-UX-002**
**Type:** Missing
**Spec area:** overview — `Width` parameter (source implemented, not demoed)
**Spec file:** `docs/component-specs/pivotgrid/overview.md`, line 169
**Demo file:** `samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor`

`Width` exists in source (`MariloPivotGrid.razor` line 70) but is never set in any demo section.

**Missing scenario:** Add a "Fixed Dimensions" demo section showing both `Width` and `Height` as explicit CSS values, with a code snippet panel.

---

**GAP-UX-003**
**Type:** Missing
**Spec area:** overview — Empty/no-data state
**Spec file:** `docs/component-specs/pivotgrid/overview.md`
**Demo file:** `samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor`

The source renders a dedicated empty-state div (line 8–11) when no rows/columns/measures are configured. No demo scenario shows this state.

**Missing scenario:** Add an "Empty State" demo section showing the PivotGrid with no `RowFields`/`ColumnFields`/`MeasureFields` configured, so users understand the default empty experience.

---

**GAP-UX-004**
**Type:** Missing
**Spec area:** overview — `Rebind()` method
**Spec file:** `docs/component-specs/pivotgrid/overview.md`, lines 215–240
**Demo file:** `samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor`

No scenario demonstrates programmatic data refresh via `Rebind()`. The source does not yet implement `Rebind()`.

**Missing scenario (blocked-by-source for `Rebind()`):** Once `Rebind()` is added to source, add a "Refresh Data" demo where a button appends new records and calls `PivotGridRef.Rebind()`.

---

### Spec Area: overview — Row, Column, and Measure Child Components

**GAP-UX-005**
**Type:** Blocked-by-source
**Spec area:** overview — `PivotGridRow` / `PivotGridColumn` / `PivotGridMeasure` child component API
**Spec file:** `docs/component-specs/pivotgrid/overview.md`, lines 170–179
**Demo file:** `samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor`

Demo uses `RowFields="..."` / `ColumnFields="..."` / `MeasureFields="..."` list parameters. Spec defines child components with `<PivotGridRow Name="..." Title="..." HeaderClass="..." />` etc. No demo coverage for:
- `Title` on row/column/measure
- `HeaderClass` on any axis
- `Format` on `PivotGridMeasure`
- Multiple rows (multi-level row grouping)
- Multiple columns (multi-level column grouping)
- Multiple measures (multi-measure)

**Missing scenarios (all blocked until child components implemented):**
- Multi-level row and column grouping demo
- Custom `HeaderClass` styling demo
- `Format` (e.g. currency) on `PivotGridMeasure`
- Multiple `PivotGridMeasure` instances side by side

---

### Spec Area: configurator

**GAP-UX-006**
**Type:** Blocked-by-source
**Spec area:** configurator — `MariloPivotGridConfigurator`, `MariloPivotGridConfiguratorButton`, `MariloPivotGridContainer`
**Spec file:** `docs/component-specs/pivotgrid/configurator.md`
**Demo file:** `samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor`

No Configurator scenario exists in the demo. None of the required sub-components (`MariloPivotGridConfigurator`, `MariloPivotGridConfiguratorButton`, `MariloPivotGridContainer`) are implemented.

**Missing scenarios (all blocked-by-source):**
- Full Configurator demo: PivotGrid inside `<MariloPivotGridContainer>` with `<MariloPivotGridConfigurator>` and `<MariloPivotGridConfiguratorButton>`
- Configurator Fields section (checking/unchecking TreeView items)
- Configurator Columns/Rows drag reorder
- Configurator sorting and filtering via chip context menu
- Apply/Cancel buttons interaction

---

### Spec Area: data-binding

**GAP-UX-007**
**Type:** Partial
**Spec area:** data-binding — Local provider
**Spec file:** `docs/component-specs/pivotgrid/data-binding.md`, lines 33–97
**Demo file:** `samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor`, lines 6–33

Local data binding is partially covered (data is bound in both demo sections). However:
- No demo shows `DataProviderType="PivotGridDataProviderType.Local"` explicitly (parameter missing from source).
- No demo shows programmatic data reload / `Rebind()` after data changes.
- No async data loading scenario.

---

**GAP-UX-008**
**Type:** Blocked-by-source
**Spec area:** data-binding — XMLA provider
**Spec file:** `docs/component-specs/pivotgrid/data-binding.md`, lines 100–167
**Demo file:** `samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor`

XMLA data binding has no demo coverage. `DataProviderType`, `PivotGridSettings`, `PivotGridXmlaDataProviderSettings`, and `PivotGridXmlaDataProviderCredentials` are not implemented.

**Missing scenarios (all blocked-by-source):**
- XMLA connection setup demo with `ServerUrl`, `Catalog`, `Cube`
- Credentials demo with `Username`, `Password`, `Domain`
- LoadOnDemand toggle for XMLA

---

### Spec Area: templates

**GAP-UX-009**
**Type:** Blocked-by-source
**Spec area:** templates — `ColumnHeaderTemplate`, `DataCellTemplate`, `RowHeaderTemplate`
**Spec file:** `docs/component-specs/pivotgrid/templates.md`
**Demo file:** `samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor`

No template scenarios exist in the demo. None of the template RenderFragments are implemented in source.

**Missing scenarios (all blocked-by-source):**
- Column header template with custom styling (e.g. colored bold header)
- Row header template with custom icon or prefix
- Data cell template with currency formatting / conditional cell coloring
- Nested template context disambiguation scenario (`Context` parameter usage)

---

### Spec Area: accessibility

**GAP-UX-010**
**Type:** Blocked-by-source
**Spec area:** accessibility — WAI-ARIA, keyboard navigation
**Spec file:** `docs/component-specs/pivotgrid/accessibility/wai-aria-support.md`
**Demo file:** `samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor`

No accessibility demo exists. The source renders a plain `<table>` with no ARIA roles (e.g. no `role=grid`, no `role=rowheader`, no `aria-expanded`).

**Missing scenarios (all blocked-by-source):**
- Accessibility demo with keyboard navigation callout
- Screen reader usage note
- `aria-label` or descriptive label on the grid

---

### Demo Format Compliance

**GAP-UX-011**
**Type:** Missing
**Spec area:** N/A — demo format compliance
**Spec file:** `ICM/workspaces/pivotgrid-delivery/stages/02-example-ux/shared/demo-scenario-format.md`
**Demo file:** `samples/Marilo.Demo/Pages/Components/PivotGrid/Overview.razor`

Both existing demo sections are missing required format elements:

| Requirement | Section 1 (Basic Usage) | Section 2 (Aggregate Functions) |
|-------------|------------------------|---------------------------------|
| Scenario title (real use case) | Partial — "Basic Usage" is generic | Partial — "Count Aggregation" is slightly more concrete |
| 1–2 sentence description | Yes | Yes |
| Live interactive component | Yes | Yes |
| User-controllable input | No | Yes (aggregate toggle buttons) |
| Code snippet panel | Stale string (stub API) | Stale string (stub API) |
| Parameter table | No | No |
| Spec section link | No | No |

**Stale code snippets:**
- `_basicCode` (line 72–78): references `RowFields`, `ColumnFields`, `MeasureFields`, `AggregateFunction` — all stub parameters not in spec.
- `_countCode` (line 80–87): same issue plus `Sortable` which is also undocumented.

Both code snippets must be rewritten once the spec-compliant API is implemented.

---

## Gap Summary

| Type | Count |
|------|-------|
| Missing (spec area has no coverage, source ready or partially ready) | 4 |
| Partial (coverage exists but incomplete) | 3 |
| Blocked-by-source (spec feature not yet implemented) | 4 |
| **Total gap records** | **11** |

---

## Spec Area Coverage Matrix

| Spec Feature Area | Demo Coverage | Gap Records | Blocker? |
|---|---|---|---|
| overview — Grid Parameters | Partial | GAP-UX-001, GAP-UX-002, GAP-UX-003, GAP-UX-004 | Mostly blocked-by-source |
| overview — Child Components (Row/Column/Measure) | Missing | GAP-UX-005 | Blocked-by-source |
| configurator | Missing | GAP-UX-006 | Blocked-by-source |
| data-binding — Local | Partial | GAP-UX-007 | Partial block |
| data-binding — XMLA | Missing | GAP-UX-008 | Blocked-by-source |
| templates | Missing | GAP-UX-009 | Blocked-by-source |
| accessibility | Missing | GAP-UX-010 | Blocked-by-source |
| demo format compliance | Non-compliant | GAP-UX-011 | Not blocked — fixable now |

---

## Recommended Next Actions

1. **Now (no source changes needed):** Fix `GAP-UX-011` — rewrite demo section titles, add parameter tables and spec links, remove stale code string constants.
2. **Phase 1 source priority:** Implement `Width`, `Rebind()`, `Class`, `ColumnHeadersWidth`, `RowHeadersWidth`, generic `TItem`, and the child component API (`PivotGridRow`/`PivotGridColumn`/`PivotGridMeasure`). This unblocks GAP-UX-001, 002, 003, 004, 005.
3. **Phase 2 source priority:** Implement `MariloPivotGridConfigurator`, `MariloPivotGridConfiguratorButton`, `MariloPivotGridContainer`. This unblocks GAP-UX-006.
4. **Phase 2 source priority:** Add template RenderFragments (`ColumnHeaderTemplate`, `DataCellTemplate`, `RowHeaderTemplate`). This unblocks GAP-UX-009.
5. **Phase 3 source priority:** Implement XMLA provider. This unblocks GAP-UX-008.
6. **Parallel with source work:** Add ARIA attributes to rendered markup. This unblocks GAP-UX-010.
