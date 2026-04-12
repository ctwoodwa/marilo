# PivotGrid Visual Parity Gaps — Stage 03 Visual Parity

**Date:** 2026-04-12
**Source BEM classes:** Extracted from src/Marilo.Components/DataGrid/MariloPivotGrid.razor
**FluentUI SCSS search:** src/Marilo.Providers.FluentUI/ — no pivotgrid SCSS found
**Bootstrap SCSS search:** src/Marilo.Providers.Bootstrap/ — no pivotgrid SCSS found
**Visual parity registry:** tests/visual-parity/config/component-registry.ts — entry exists (slug: pivotgrid, rootSelector: .mar-pivotgrid)

---

## Summary

The PivotGrid component emits 8 BEM CSS classes. **Zero** of these classes have corresponding SCSS rules in any provider (FluentUI or Bootstrap). The component is completely unstyled by the provider layer. The visual parity registry has a placeholder entry but no actual parity specs or snapshots.

---

## BEM Classes in Source

| Class | Element | Purpose |
|-------|---------|---------|
| `mar-pivotgrid` | Root `<div>` | Component root container |
| `mar-pivotgrid__empty` | `<div>` | Empty state container (no data configured) |
| `mar-pivotgrid__scroll` | `<div>` | Scrollable wrapper with overflow:auto |
| `mar-pivotgrid__table` | `<table>` | Main pivot table element |
| `mar-pivotgrid__corner` | `<th>` | Top-left corner cell (shows row field titles) |
| `mar-pivotgrid__col-header` | `<th>` | Column header cells |
| `mar-pivotgrid__row-header` | `<td>` | Row header cells |
| `mar-pivotgrid__cell` | `<td>` | Data/value cells |

## SCSS Coverage by Provider

### FluentUI Provider

| Class | SCSS File | Status |
|-------|-----------|--------|
| `mar-pivotgrid` | — | MISSING |
| `mar-pivotgrid__empty` | — | MISSING |
| `mar-pivotgrid__scroll` | — | MISSING |
| `mar-pivotgrid__table` | — | MISSING |
| `mar-pivotgrid__corner` | — | MISSING |
| `mar-pivotgrid__col-header` | — | MISSING |
| `mar-pivotgrid__row-header` | — | MISSING |
| `mar-pivotgrid__cell` | — | MISSING |

**Coverage: 0/8 classes (0%)**

### Bootstrap Provider

| Class | SCSS File | Status |
|-------|-----------|--------|
| `mar-pivotgrid` | — | MISSING |
| `mar-pivotgrid__empty` | — | MISSING |
| `mar-pivotgrid__scroll` | — | MISSING |
| `mar-pivotgrid__table` | — | MISSING |
| `mar-pivotgrid__corner` | — | MISSING |
| `mar-pivotgrid__col-header` | — | MISSING |
| `mar-pivotgrid__row-header` | — | MISSING |
| `mar-pivotgrid__cell` | — | MISSING |

**Coverage: 0/8 classes (0%)**

## Spec vs. Source CSS Class Divergence

The spec references Kendo-style CSS classes (e.g. `k-pivotgrid`, `k-pivotgrid-column-headers`, `k-pivotgrid-row-headers`, `k-pivotgrid-values`, `k-pivotgrid-configurator`, etc.) throughout the accessibility documentation. The source uses BEM-style `mar-pivotgrid__*` classes. These are fundamentally different naming conventions.

| Spec Class (Kendo) | Source Class (BEM) | Status |
|--------------------|-------------------|--------|
| `.k-pivotgrid` | `.mar-pivotgrid` | Renamed |
| `.k-pivotgrid-column-headers` | (not implemented) | Missing from source |
| `.k-pivotgrid-row-headers` | (not implemented) | Missing from source |
| `.k-pivotgrid-values` | (not implemented) | Missing from source |
| `.k-pivotgrid-table` | `.mar-pivotgrid__table` | Renamed |
| `.k-pivotgrid-row` | (not a direct equivalent) | Structural difference |
| `.k-pivotgrid-empty-cell` | `.mar-pivotgrid__corner` | Different semantics |
| `.k-pivotgrid-configurator` | (not implemented) | Missing from source |
| `.k-pivotgrid-configurator-button` | (not implemented) | Missing from source |
| `.k-pivotgrid-configurator-header-text` | (not implemented) | Missing from source |
| `.k-pivotgrid-configurator-content` | (not implemented) | Missing from source |
| `.k-pivotgrid-configurator-actions` | (not implemented) | Missing from source |

## Visual Parity Test Infrastructure

| Item | Status |
|------|--------|
| component-registry.ts entry | Present (slug: pivotgrid, route: /components/PivotGrid/overview, rootSelector: .mar-pivotgrid) |
| Playwright spec file (tests/visual-parity/specs/pivotgrid.spec.ts) | NOT FOUND |
| Baseline snapshots | NOT FOUND |
| Capture matrix in CDW | NOT FOUND |

---

## Conclusion

The PivotGrid has **zero visual parity** — no provider SCSS exists for any of its 8 BEM classes. The component renders with browser-default table styling only. Additionally, the CSS class naming convention diverges between spec (Kendo `k-` prefix) and source (Marilo BEM `mar-` prefix). The visual parity test infrastructure has a registry entry but no actual spec or snapshots. Full SCSS authoring in both FluentUI and Bootstrap providers is required before any visual parity testing is meaningful.
