# Stage 01 — Spec Review: MariloPivotGrid
**Date:** 2026-04-11
**Spec paths audited:**
- `docs/component-specs/pivotgrid/overview.md`
- `docs/component-specs/pivotgrid/data-binding.md`
- `docs/component-specs/pivotgrid/templates.md`
- `docs/component-specs/pivotgrid/configurator.md`
- `docs/component-specs/pivotgrid/accessibility/wai-aria-support.md`

**Source path:** `src/Marilo.Components/DataGrid/MariloPivotGrid.razor`
**Models path:** `src/Marilo.Core/Models/PivotGridModels.cs`

---

## Key Structural Finding

The source component (`MariloPivotGrid.razor`) is a simplified stub with a flat, list-parameter API that diverges fundamentally from the spec's child-component API. The spec describes a family of sub-components (`MariloPivotGridConfigurator`, `MariloPivotGridConfiguratorButton`, `MariloPivotGridContainer`, `PivotGridRow`, `PivotGridColumn`, `PivotGridMeasure`, and XMLA settings components). None of these sub-components exist in the source tree. The stub also uses a single global `AggregateFunction` parameter rather than the spec's per-measure `Aggregate` field.

All gap records below assume the source stub is in Phase 1 and the spec is authoritative for the target API.

---

## List 1: Undocumented (in source, not in spec)

These parameters exist in `MariloPivotGrid.razor` but have no matching entry in any spec file.

---

**ID:** SPEC-pivotgrid-001
**Type:** undocumented
**Parameter/Event:** `RowFields`
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `RowFields` (line 55) |
| Type | missing | `List<PivotGridField>` |
| Default | missing | `new()` |
| Description | missing | List of fields used for row-axis grouping; each entry has `Name` and optional `Title` |

**Recommended action:** Source API does not match spec. Spec uses child components `<PivotGridRow Name="..." />` inside `<PivotGridRows>`. Implement per-spec or document the stub-level parameter as a temporary internal API.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-002
**Type:** undocumented
**Parameter/Event:** `ColumnFields`
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `ColumnFields` (line 58) |
| Type | missing | `List<PivotGridField>` |
| Default | missing | `new()` |
| Description | missing | List of fields used for column-axis grouping |

**Recommended action:** Replace with spec-compliant child-component pattern (`<PivotGridColumn>`).
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-003
**Type:** undocumented
**Parameter/Event:** `MeasureFields`
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `MeasureFields` (line 61) |
| Type | missing | `List<PivotGridField>` |
| Default | missing | `new()` |
| Description | missing | List of measure (aggregate) fields |

**Recommended action:** Replace with spec-compliant child-component pattern (`<PivotGridMeasure>`).
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-004
**Type:** undocumented
**Parameter/Event:** `AggregateFunction`
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `AggregateFunction` (line 64) |
| Type | missing | `PivotGridAggregateFunction` (enum: Sum/Count/Average/Min/Max) |
| Default | missing | `Sum` |
| Description | missing | Single global aggregate applied to all measure fields |

**Recommended action:** Spec defines `Aggregate` as a per-measure parameter on `<PivotGridMeasure>` (type `PivotGridAggregateType`). The source's single global parameter is a mismatch with the spec's per-measure design. Rename enum to `PivotGridAggregateType` and move to the `PivotGridMeasure` child component when implementing full spec.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-005
**Type:** undocumented
**Parameter/Event:** `Sortable`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `Sortable` (line 73) |
| Type | missing | `bool` |
| Default | missing | `false` (implicit) |
| Description | missing | When true, row and column keys are sorted alphabetically before rendering |

**Recommended action:** Spec mentions sorting in the Configurator context (chip sort/filter). Document `Sortable` as a spec gap; determine whether this is a standalone parameter or configurator-driven.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-006
**Type:** undocumented
**Parameter/Event:** `Filterable`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `Filterable` (line 76) |
| Type | missing | `bool` |
| Default | missing | `false` (implicit) |
| Description | missing | Declared but not implemented — no filtering logic in `BuildPivot()` (line 95–137) |

**Recommended action:** `Filterable` is declared as a parameter but has no implementation. It is a stub placeholder. Mark as spec-ahead candidate once filtering is defined in spec.
**Delegated to:** gap-analysis-resolution intake

---

## List 2: Spec-Ahead (in spec, not in source)

These parameters, components, and features are fully specified but have no corresponding source implementation.

---

**ID:** SPEC-pivotgrid-007
**Type:** spec-ahead
**Parameter/Event:** `DataProviderType`
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `DataProviderType` (overview.md line 162) | missing |
| Type | `PivotGridDataProviderType` enum (`Local`, `Xmla`) | missing |
| Default | `Local` | missing |
| Description | Selects between local flat data and XMLA/OLAP cube data | missing |

**Recommended action:** Implement `DataProviderType` parameter on `MariloPivotGrid`.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-008
**Type:** spec-ahead
**Parameter/Event:** `Class`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Class` (overview.md line 159) | missing |
| Type | `string` | missing |
| Default | missing | missing |
| Description | Custom CSS class for the root `<div class="k-pivotgrid">` element | missing |

**Recommended action:** Add `Class` parameter; merge with base class CSS in provider.
**Delegated to:** spec update only (base class likely handles via `AdditionalAttributes` or `CombineClasses`)

---

**ID:** SPEC-pivotgrid-009
**Type:** spec-ahead
**Parameter/Event:** `ColumnHeadersWidth`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `ColumnHeadersWidth` (overview.md line 160) | missing |
| Type | `string` | missing |
| Default | missing | missing |
| Description | Width of each column in any supported CSS unit | missing |

**Recommended action:** Implement `ColumnHeadersWidth` parameter.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-010
**Type:** spec-ahead
**Parameter/Event:** `EnableLoaderContainer` (on `MariloPivotGrid`)
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `EnableLoaderContainer` (overview.md line 163) | missing |
| Type | `bool` | missing |
| Default | `true` | missing |
| Description | Shows a built-in LoaderContainer during operations over 600ms | missing |

**Recommended action:** Implement `EnableLoaderContainer`.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-011
**Type:** spec-ahead
**Parameter/Event:** `LoadOnDemand`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `LoadOnDemand` (overview.md line 165) | missing |
| Type | `bool` | missing |
| Default | `true` | missing |
| Description | When true, PivotGrid requests only data for current view; applies to XMLA only | missing |

**Recommended action:** Implement `LoadOnDemand`; relevant only once `DataProviderType.Xmla` is implemented.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-012
**Type:** spec-ahead
**Parameter/Event:** `RowHeadersWidth`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `RowHeadersWidth` (overview.md line 167) | missing |
| Type | `string` | missing |
| Default | missing | missing |
| Description | Width of all row headers in any supported CSS unit | missing |

**Recommended action:** Implement `RowHeadersWidth` parameter.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-013
**Type:** spec-ahead
**Parameter/Event:** `TItem`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `TItem` (overview.md line 168) | missing |
| Type | generic `@typeparam` | missing |
| Default | `object` | missing |
| Description | PivotGrid type parameter; required when data item type can't be inferred | missing |

**Recommended action:** Convert `MariloPivotGrid` to a generic component `MariloPivotGrid<TItem>`. Current source uses `IEnumerable<object>` (line 52) — this is a mismatch.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-014
**Type:** spec-ahead
**Parameter/Event:** `Rebind()` method
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Rebind` (overview.md line 215) | missing |
| Type | `void` method | missing |
| Default | N/A | missing |
| Description | Processes component Data and refreshes UI | missing |

**Recommended action:** Add `Rebind()` public method.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-015
**Type:** spec-ahead
**Parameter/Event:** `MariloPivotGridConfigurator` component
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `MariloPivotGridConfigurator` (overview.md line 33) | missing (no file found) |
| Type | Razor component | missing |
| Default | N/A | missing |
| Description | Allows end users to add/remove rows, columns, and measures via UI | missing |

**Recommended action:** Create `MariloPivotGridConfigurator` component with `Class` and `EnableLoaderContainer` parameters.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-016
**Type:** spec-ahead
**Parameter/Event:** `MariloPivotGridConfiguratorButton` component
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `MariloPivotGridConfiguratorButton` (overview.md line 34) | missing |
| Type | Razor component | missing |
| Default | N/A | missing |
| Description | Toggles configurator visibility | missing |

**Recommended action:** Create `MariloPivotGridConfiguratorButton` component with `Class` parameter.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-017
**Type:** spec-ahead
**Parameter/Event:** `MariloPivotGridContainer` component
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `MariloPivotGridContainer` (overview.md line 35) | missing |
| Type | Razor component | missing |
| Default | N/A | missing |
| Description | Wraps PivotGrid, Configurator, and Button; required when using a Configurator | missing |

**Recommended action:** Create `MariloPivotGridContainer` component with `Class` parameter and `ChildContent` RenderFragment.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-018
**Type:** spec-ahead
**Parameter/Event:** `PivotGridRow` / `PivotGridColumn` / `PivotGridMeasure` child components
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `PivotGridRow`, `PivotGridColumn`, `PivotGridMeasure` (overview.md lines 170–179) | missing |
| Type | Child Razor components | missing |
| Default | N/A | missing |
| Description | Define row, column, and measure fields via child components with `Name`, `Title`, `HeaderClass`; `PivotGridMeasure` also has `Aggregate` (PivotGridAggregateType) and `Format` | missing |

**Recommended action:** Create child components and replace `RowFields`/`ColumnFields`/`MeasureFields` list parameters.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-019
**Type:** spec-ahead
**Parameter/Event:** `PivotGridSettings` / `PivotGridXmlaDataProviderSettings` / `PivotGridXmlaDataProviderCredentials`
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | XMLA settings components (data-binding.md lines 103–130) | missing |
| Type | Child Razor components | missing |
| Default | N/A | missing |
| Description | Configure XMLA server URL, catalog, cube, and optional credentials | missing |

**Recommended action:** Implement once `DataProviderType.Xmla` is supported.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-020
**Type:** spec-ahead
**Parameter/Event:** `ColumnHeaderTemplate` / `DataCellTemplate` / `RowHeaderTemplate`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Template child components (templates.md lines 24–39) | missing |
| Type | `RenderFragment<PivotGridColumnHeaderTemplateContext>` etc. | missing |
| Default | N/A | missing |
| Description | Custom render fragments for column headers, data cells, and row headers | missing |

**Recommended action:** Add template render fragments to `MariloPivotGrid`.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-021
**Type:** spec-ahead
**Parameter/Event:** WAI-ARIA / accessibility attributes
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | ARIA roles and attributes (accessibility/wai-aria-support.md) | missing |
| Type | HTML attributes on rendered elements | missing |
| Default | N/A | missing |
| Description | `role=grid`, `role=columnheader`, `role=rowheader`, `role=gridcell`, `aria-expanded`, `aria-describedby`, `aria-owns`, `aria-colspan`, `aria-rowspan`, configurator `role=dialog` | missing |

**Recommended action:** Add ARIA attribute rendering to the PivotGrid table markup and to Configurator once implemented.
**Delegated to:** gap-analysis-resolution intake

---

## List 3: Mismatches (same concept, different name or shape)

---

**ID:** SPEC-pivotgrid-022
**Type:** mismatch
**Parameter/Event:** `AggregateFunction` vs. `Aggregate` (per-measure)
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Aggregate` (on `PivotGridMeasure`) | `AggregateFunction` (on `MariloPivotGrid`) (line 64) |
| Type | `PivotGridAggregateType` enum | `PivotGridAggregateFunction` enum |
| Default | `Sum` | `Sum` |
| Description | Per-measure aggregate type | Single global aggregate applied to all measures |

**Recommended action:** Rename source enum from `PivotGridAggregateFunction` to `PivotGridAggregateType`; move parameter from root component to `PivotGridMeasure` child component.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-pivotgrid-023
**Type:** mismatch
**Parameter/Event:** `Data` parameter type
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Data` | `Data` |
| Type | `IEnumerable<TItem>` (generic) (overview.md line 161) | `IEnumerable<object>?` (line 52) |
| Default | missing | `null` |
| Description | Data source for the pivot grid | Data source — uses reflection to access properties |

**Recommended action:** Convert to generic `IEnumerable<TItem>` once `TItem` typeparam is added.
**Delegated to:** gap-analysis-resolution intake

---

## Gap Summary

| Category | Count |
|----------|-------|
| Undocumented (source not in spec) | 6 |
| Spec-ahead (spec not in source) | 15 |
| Mismatch (name or shape divergence) | 2 |
| **Total** | **23** |

**P1 (blocking):** SPEC-pivotgrid-001, 002, 003, 004, 007, 013, 015, 016, 017, 018, 022, 023 (12 items)
**P2 (this phase):** SPEC-pivotgrid-005, 006, 008, 009, 010, 011, 012, 014, 020, 021 (10 items)
**P3 (next phase):** SPEC-pivotgrid-019 (1 item)

---

## Feature Area Status

| Feature Area | Status |
|---|---|
| accessibility | COMPLETE — no source implementation; all ARIA attributes are spec-ahead |
| configurator | COMPLETE — no Configurator source components exist; all configurator items are spec-ahead |
| data-binding | COMPLETE — `DataProviderType`, XMLA components, `LoadOnDemand` are spec-ahead; `Data` type mismatch |
| overview | COMPLETE — 12 parameters reviewed; major API shape mismatch found |
| templates | COMPLETE — no template RenderFragments in source; all three templates are spec-ahead |
