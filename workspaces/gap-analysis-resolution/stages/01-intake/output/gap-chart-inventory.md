# Gap Inventory: MariloChart

> Imported: 2026-04-03
> Analysis mode: Reconstructed (code exists before gap analysis)
> Total gaps: ~18 (6 Critical, 7 High, 5 Medium)

---

## Component Inventory

| Attribute | Value |
|-----------|-------|
| **Source files** | `MariloChart.razor` (844 lines), `MariloChartSeries.razor` (150 lines) |
| **Code-behind partials** | None |
| **Public parameters (Chart)** | 11 (Width, Height, Title, ChildContent, Transitions, Palette, ShowLegend, ShowTooltips, OnSeriesClick, OnClick, OnLegendItemClick) |
| **Public parameters (Series)** | 12 |
| **Tests** | Yes -- `ChartTests.cs` (122 lines, 5 test methods) |
| **Demos** | No demo pages found |
| **Spec** | `docs/component-specs/chart/overview.md` |

---

## Gap Summary

The spec describes a rich charting API with child component composition (ChartTitle, ChartSubtitle, ChartLegend, ChartTooltip, ChartCategoryAxis, ChartValueAxis, ChartSeriesItems, ChartSeries), multiple chart types, drilldown, responsive Refresh(), ResetDrilldownLevel(), CSS variable theming, and data binding via Field/CategoryField. The implementation covers basic rendering with SVG, series registration, tooltips, title, legend, and a few chart types. Major gaps exist in the child component API surface, advanced chart types, drilldown, and programmatic methods.

### GAP-CHART-001: Missing Refresh() method

**Area:** MariloChart
**Severity:** Critical
**Theme:** missing-public-method
**Source:** chart/overview.md -- Chart Reference and Methods

**Target behavior:** `Refresh()` method to programmatically re-render the chart, essential for responsive layouts.
**Current behavior:** No public Refresh method found in the 844-line source.
**Impact:** Charts in percentage-sized containers cannot be redrawn after resize.
**Recommended direction:** Add public Refresh() method that triggers StateHasChanged and re-measures via JS interop.
**Status:** Open

---

### GAP-CHART-002: Missing ResetDrilldownLevel() method

**Area:** MariloChart
**Severity:** High
**Theme:** missing-public-method
**Source:** chart/overview.md -- Chart Reference and Methods

**Target behavior:** `ResetDrilldownLevel()` to programmatically reset chart drilldown.
**Current behavior:** No drilldown support implemented.
**Impact:** Drilldown charting scenario is entirely absent.
**Recommended direction:** Implement drilldown state management and the reset method.
**Status:** Open

---

### GAP-CHART-003: Missing ChartSubtitle child component

**Area:** MariloChart
**Severity:** High
**Theme:** missing-child-component
**Source:** chart/overview.md -- Title and Subtitle section

**Target behavior:** `ChartSubtitle` child component with Text and Position parameters, nested inside ChartTitle.
**Current behavior:** ChartTitle child is registered internally but ChartSubtitle does not exist.
**Impact:** Cannot display secondary titles as shown in spec examples.
**Recommended direction:** Create ChartSubtitle component that registers with ChartTitle.
**Status:** Open

---

### GAP-CHART-004: Missing Class parameter

**Area:** MariloChart
**Severity:** Critical
**Theme:** missing-css-class-param
**Source:** chart/overview.md -- Chart Parameters table, CSS Variables section

**Target behavior:** `Class` parameter renders a custom CSS class on the chart container div for CSS variable scoping.
**Current behavior:** No dedicated Class parameter (base class may provide one, but spec explicitly lists it).
**Impact:** Cannot scope CSS variable overrides to individual chart instances.
**Recommended direction:** Verify MariloComponentBase provides Class; if not, add it.
**Status:** Open

---

### GAP-CHART-005: Missing ChartSeriesItems wrapper component

**Area:** MariloChart
**Severity:** Critical
**Theme:** missing-child-component
**Source:** chart/overview.md -- code examples show `<ChartSeriesItems>` wrapper

**Target behavior:** `ChartSeriesItems` wrapper tag contains multiple `ChartSeries` instances.
**Current behavior:** Series are registered directly via CascadingValue without a wrapper.
**Impact:** API shape differs from spec; consumer code would not match documented examples.
**Recommended direction:** Add ChartSeriesItems as a pass-through wrapper or accept direct children as valid alternate API.
**Status:** Open

---

### GAP-CHART-006: Missing ChartCategoryAxes/ChartCategoryAxis child components

**Area:** MariloChart
**Severity:** Critical
**Theme:** missing-child-component
**Source:** chart/overview.md -- Creating Blazor Chart step 3, code examples

**Target behavior:** `ChartCategoryAxes` > `ChartCategoryAxis` with `Categories` parameter for bulk X-axis configuration.
**Current behavior:** Internal `_categoryAxis` field exists but the public child component API surface is unclear.
**Impact:** Cannot configure category axis via the documented tag-based API.
**Recommended direction:** Expose ChartCategoryAxes/ChartCategoryAxis as public child components.
**Status:** Open

---

### GAP-CHART-007: No demo pages

**Area:** MariloChart
**Severity:** Critical
**Theme:** missing-demos
**Source:** samples/Marilo.Demo/Pages/Components/Chart/ (directory absent)

**Target behavior:** Demo pages showing basic, responsive, drilldown, and multi-series chart scenarios.
**Current behavior:** No demo directory or pages exist.
**Impact:** No way for developers to preview or validate chart functionality.
**Recommended direction:** Create demo pages for core chart scenarios.
**Status:** Open

---

### GAP-CHART-008: Missing CSS variable theming support

**Area:** MariloChart
**Severity:** High
**Theme:** missing-theming
**Source:** chart/overview.md -- Styling with CSS Variables section

**Target behavior:** Support `--kendo-chart-bg`, `--kendo-chart-text`, `--kendo-chart-series-N` CSS variables.
**Current behavior:** Uses hardcoded DefaultColors array; no CSS variable integration.
**Impact:** Cannot theme charts via CSS variables as documented.
**Recommended direction:** Map CSS variables to chart rendering or provide bridging parameters.
**Status:** Open

---

### GAP-CHART-009: Limited chart type coverage

**Area:** MariloChart
**Severity:** High
**Theme:** missing-feature-coverage
**Source:** chart/overview.md -- references "wide range of graph types"

**Target behavior:** Spec implies support for Column, Line, Bar, Area, Pie, Donut, Scatter, and more.
**Current behavior:** ChartSeriesType enum exists with basic types; full coverage unclear from 844 lines.
**Impact:** Users expecting specific chart types may find them unsupported.
**Recommended direction:** Audit ChartSeriesType enum and rendering switch for completeness.
**Status:** Open

---

### GAP-CHART-010: Insufficient test coverage

**Area:** MariloChart
**Severity:** High
**Theme:** low-test-coverage
**Source:** ChartTests.cs (5 tests for 994 lines of component code)

**Target behavior:** Tests covering rendering, events, tooltips, legend, multiple series types, accessibility.
**Current behavior:** 5 tests covering basic render and title display.
**Impact:** Regressions likely undetected during development.
**Recommended direction:** Expand test suite to cover events, series types, and accessibility attributes.
**Status:** Open

---

### GAP-CHART-011: Missing data binding documentation alignment

**Area:** MariloChart
**Severity:** Medium
**Theme:** api-surface-mismatch
**Source:** chart/overview.md -- references Field, CategoryField on ChartSeries

**Target behavior:** ChartSeries supports `Field`, `CategoryField`, `Data`, `Name`, `Type` per spec.
**Current behavior:** MariloChartSeries has 12 parameters; alignment with spec naming needs verification.
**Impact:** Minor if names match; confusing if they diverge.
**Recommended direction:** Audit parameter names against spec.
**Status:** Open

---

### GAP-CHART-012: Missing Transitions nullable alignment

**Area:** MariloChart
**Severity:** Medium
**Theme:** type-mismatch
**Source:** chart/overview.md -- Parameters table shows `bool?`

**Target behavior:** `Transitions` parameter typed as `bool?` (nullable).
**Current behavior:** Typed as `bool` with default `true`.
**Impact:** Cannot distinguish "not set" from "explicitly true" for theme-level animation defaults.
**Recommended direction:** Change to `bool?` if spec requires nullable semantics.
**Status:** Open

---

### GAP-CHART-013: Missing OnRender / OnAxisRender events

**Area:** MariloChart
**Severity:** Medium
**Theme:** missing-events
**Source:** chart/overview.md -- Next Steps references "Explore the Chart events"

**Target behavior:** Rich event API including render, axis, and zoom events.
**Current behavior:** Only OnSeriesClick, OnClick, OnLegendItemClick implemented.
**Impact:** Advanced customization scenarios blocked.
**Recommended direction:** Add events per spec event documentation.
**Status:** Open

---

### GAP-CHART-014: Missing tooltip customization API

**Area:** MariloChart
**Severity:** Medium
**Theme:** missing-feature
**Source:** chart/overview.md -- Next Steps references "Learn more about Chart Tooltips"

**Target behavior:** ChartTooltip child with template support, format strings, shared tooltip mode.
**Current behavior:** Basic tooltip div with JS positioning; ChartTooltip registered internally but API surface limited.
**Impact:** Cannot customize tooltip content or format.
**Recommended direction:** Expand ChartTooltip with Template, Format, Shared parameters.
**Status:** Open

---

### GAP-CHART-015: Missing legend positioning

**Area:** MariloChart
**Severity:** Medium
**Theme:** missing-feature
**Source:** chart/overview.md -- "Position of the ChartLegend"

**Target behavior:** ChartLegend child with Position parameter (Top, Bottom, Left, Right).
**Current behavior:** ChartLegend registered internally; position control unclear.
**Impact:** Cannot control legend placement.
**Recommended direction:** Add Position parameter to ChartLegend.
**Status:** Open

---

## Severity Breakdown

| Severity | Count |
|----------|-------|
| Critical | 6 |
| High | 5 |
| Medium | 5 |
| Low | 0 |
| **Total** | **16** |
