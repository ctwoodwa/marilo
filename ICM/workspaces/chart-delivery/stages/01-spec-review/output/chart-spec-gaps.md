# MariloChart -- Stage 01 Spec Review: Gap List

**Audit date:** 2026-04-10
**Source files:**
- `src/Marilo.Components/Charts/MariloChart.razor`
- `src/Marilo.Components/Charts/MariloChartSeries.razor`
- `src/Marilo.Components/Charts/ChartTooltip.razor`
- `src/Marilo.Components/Charts/ChartLegend.razor`
- `src/Marilo.Components/Charts/ChartTitle.razor`
- `src/Marilo.Components/Charts/ChartSubtitle.razor`
- `src/Marilo.Components/Charts/ChartCategoryAxis.razor`
- `src/Marilo.Components/Charts/ChartValueAxis.razor`
- `src/Marilo.Components/Charts/ChartCategoryAxes.razor`
- `src/Marilo.Components/Charts/ChartSeriesItems.razor`
- `src/Marilo.Components/Charts/ChartDataPoint.cs`
- `src/Marilo.Components/Charts/ChartEventArgs.cs`
- `src/Marilo.Components/Charts/MariloStockChart.razor`
- `src/Marilo.Core/Enums/ChartSeriesType.cs`

**Spec files:** `docs/component-specs/chart/` (37 .md files)

**Source parameter count:** 43 (across all chart components)
**Spec parameter count:** 80+ (estimated from spec references)
**Total gaps:** 52

| Gap type | Count |
|----------|-------|
| Undocumented | 7 |
| Spec-ahead | 38 |
| Mismatch | 7 |

---

## Source Inventory

### MariloChart Parameters

| Parameter | Type | Default | Notes |
|-----------|------|---------|-------|
| `Width` | `string` | `"100%"` | |
| `Height` | `string` | `"300px"` | |
| `Title` | `string?` | `null` | Shorthand; ChartTitle child takes precedence |
| `ChildContent` | `RenderFragment?` | `null` | |
| `Transitions` | `bool?` | `null` | Null = theme default (true) |
| `Palette` | `string[]?` | `null` | Falls back to 8 default colors |
| `ShowLegend` | `bool` | `true` | Used when ChartLegend child not provided |
| `ShowTooltips` | `bool` | `true` | Used when ChartTooltip child not provided |

### MariloChart Events

| Event | Type | Notes |
|-------|------|-------|
| `OnSeriesClick` | `EventCallback<ChartSeriesClickEventArgs>` | |
| `OnClick` | `EventCallback<ChartClickEventArgs>` | Legacy alias |
| `OnLegendItemClick` | `EventCallback<ChartLegendItemClickEventArgs>` | |
| `OnRender` | `EventCallback<ChartRenderEventArgs>` | |

### MariloChart Methods

| Method | Notes |
|--------|-------|
| `Refresh()` | Forces re-render |

### MariloChartSeries Parameters

| Parameter | Type | Default | Notes |
|-----------|------|---------|-------|
| `Name` | `string` | `"Series"` | |
| `Data` | `IEnumerable<object>?` | `null` | |
| `Field` | `string` | `""` | |
| `CategoryField` | `string` | `""` | |
| `XField` | `string?` | `null` | Scatter/bubble |
| `YField` | `string?` | `null` | Scatter/bubble |
| `SizeField` | `string?` | `null` | Bubble |
| `Type` | `ChartSeriesType` | `Line` | |
| `Color` | `string?` | `null` | |
| `Visible` | `bool` | `true` | |
| `Gap` | `double?` | `null` | Bar/column spacing |
| `Spacing` | `double?` | `null` | Bar group spacing |

### ChartTooltip Parameters

| Parameter | Type | Default |
|-----------|------|---------|
| `Visible` | `bool` | `true` |
| `Background` | `string?` | `null` |
| `Color` | `string?` | `null` |
| `Format` | `string?` | `null` |
| `Template` | `RenderFragment<ChartTooltipContext>?` | `null` |
| `Shared` | `bool` | `false` |

### ChartLegend Parameters

| Parameter | Type | Default |
|-----------|------|---------|
| `Visible` | `bool` | `true` |
| `Position` | `ChartPosition` | `Bottom` |

### ChartTitle Parameters

| Parameter | Type | Default |
|-----------|------|---------|
| `Text` | `string?` | `null` |
| `Description` | `string?` | `null` |
| `Position` | `ChartPosition` | `Top` |
| `ChildContent` | `RenderFragment?` | `null` |

### ChartSubtitle Parameters

| Parameter | Type | Default |
|-----------|------|---------|
| `Text` | `string?` | `null` |
| `Position` | `ChartPosition` | `Bottom` |

### ChartCategoryAxis Parameters

| Parameter | Type | Default |
|-----------|------|---------|
| `Categories` | `string[]?` | `null` |
| `Name` | `string?` | `null` |
| `Title` | `string?` | `null` |
| `Color` | `string?` | `null` |

### ChartValueAxis Parameters

| Parameter | Type | Default |
|-----------|------|---------|
| `Name` | `string?` | `null` |
| `Title` | `string?` | `null` |
| `Min` | `double?` | `null` |
| `Max` | `double?` | `null` |
| `Color` | `string?` | `null` |

### ChartSeriesType Enum

| Value | Implemented |
|-------|------------|
| `Line` | Yes |
| `Bar` | Yes |
| `Column` | Yes |
| `Area` | Yes |
| `Pie` | Yes |
| `Donut` | Yes |
| `Scatter` | Yes |
| `ScatterLine` | Yes |
| `Bubble` | Yes |

---

## Gap Records

### A. Undocumented (in source, not in spec)

#### GAP-U01: `MariloChart.ShowLegend` parameter
| | Source | Spec |
|--|--------|------|
| Exists | Yes | No |
| Type | `bool` (default `true`) | N/A |
**Priority:** P3 (next phase)
**Notes:** Convenience parameter that provides a default legend without requiring a `ChartLegend` child. Spec only documents the `ChartLegend` child component approach.

#### GAP-U02: `MariloChart.ShowTooltips` parameter
| | Source | Spec |
|--|--------|------|
| Exists | Yes | No |
| Type | `bool` (default `true`) | N/A |
**Priority:** P3 (next phase)
**Notes:** Convenience parameter for default tooltip behavior without `ChartTooltip` child.

#### GAP-U03: `MariloChart.Palette` parameter
| | Source | Spec |
|--|--------|------|
| Exists | Yes | No |
| Type | `string[]?` | N/A |
**Priority:** P2 (this phase)
**Notes:** Allows custom color palette override. Spec documents CSS variable theming but not a programmatic Palette parameter.

#### GAP-U04: `MariloChart.OnClick` legacy event
| | Source | Spec |
|--|--------|------|
| Exists | Yes | No |
| Type | `EventCallback<ChartClickEventArgs>` | N/A |
**Priority:** P3 (next phase)
**Notes:** Legacy alias for `OnSeriesClick`. Should be documented or deprecated in spec.

#### GAP-U05: `MariloChart.OnRender` event
| | Source | Spec |
|--|--------|------|
| Exists | Yes | No |
| Type | `EventCallback<ChartRenderEventArgs>` | N/A |
**Priority:** P2 (this phase)
**Notes:** Fires after chart render with chart dimensions and data summary. Not documented in spec events page.

#### GAP-U06: `MariloChartSeries.Gap` and `Spacing` parameters
| | Source | Spec |
|--|--------|------|
| Exists | Yes | No |
| Type | `double?` each | N/A |
**Priority:** P3 (next phase)
**Notes:** Bar/column gap and spacing controls present in source but not referenced in any spec doc.

#### GAP-U07: `MariloStockChart` component
| | Source | Spec |
|--|--------|------|
| Exists | Yes | No |
| Type | Full component with `Data`, `Width`, `Height`, `BullColor`, `BearColor`, `ShowNavigator`, `ShowVolume` | N/A |
**Priority:** P2 (this phase)
**Notes:** Entire component undocumented. Has candlestick rendering, navigator, and volume support.

---

### B. Spec-Ahead (in spec, not in source)

#### GAP-S01: `MariloChart.RenderAs` parameter
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (rendering-modes.md) |
| Type | N/A | `RenderingMode` enum (SVG/Canvas) |
**Priority:** P3 (next phase)
**Notes:** Source always renders SVG. Spec documents Canvas rendering mode option.

#### GAP-S02: `MariloChart.Class` parameter
| | Source | Spec |
|--|--------|------|
| Exists | Inherited from `MariloComponentBase` | Yes (overview.md) |
| Type | Via base class | `string` |
**Priority:** P3 (next phase)
**Notes:** Likely inherited from base class but worth verifying it functions correctly per spec.

#### GAP-S03: `OnAxisLabelClick` event
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (events.md) |
| Type | N/A | `EventCallback<ChartAxisLabelClickEventArgs>` |
**Priority:** P2 (this phase)
**Notes:** Spec documents full event args with `AxisName`, `Index`, `Text`, `Value`. Source has no axis label click handling.

#### GAP-S04: `OnDrilldown` event
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (events.md, drilldown.md) |
| Type | N/A | `EventCallback<ChartDrilldownEventArgs>` |
**Priority:** P3 (next phase)
**Notes:** Entire drilldown feature not implemented. Spec has full article.

#### GAP-S05: `ResetDrilldownLevel` method
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (overview.md) |
| Type | N/A | Method on chart reference |
**Priority:** P3 (next phase)
**Notes:** Part of unimplemented drilldown feature.

#### GAP-S06: `ChartSeries.Axis` parameter (multi-axis binding)
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (multiple-axes.md) |
| Type | N/A | `string` |
**Priority:** P2 (this phase)
**Notes:** Spec documents binding series to named value axes.

#### GAP-S07: `ChartSeries.CategoryAxis` parameter
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (multiple-axes.md) |
| Type | N/A | `string` |
**Priority:** P2 (this phase)
**Notes:** Spec documents binding series to named category axes.

#### GAP-S08: `ChartSeries.Aggregate` parameter
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (date-axis.md, multiple-axes.md) |
| Type | N/A | `ChartSeriesAggregate` enum |
**Priority:** P3 (next phase)
**Notes:** Data aggregation for date axes.

#### GAP-S09: `ChartSeries.DrilldownField` parameter
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (drilldown.md) |
| Type | N/A | `string` |
**Priority:** P3 (next phase)
**Notes:** Part of unimplemented drilldown feature.

#### GAP-S10: `ChartSeries.ColorField` parameter
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (types/pie.md) |
| Type | N/A | `string` |
**Priority:** P2 (this phase)
**Notes:** Spec shows per-data-point color binding for pie charts.

#### GAP-S11: `ChartSeries.Style` parameter
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (multiple-axes.md) |
| Type | N/A | `ChartSeriesStyle` enum (e.g., `Smooth`) |
**Priority:** P3 (next phase)
**Notes:** Smooth line interpolation for scatter line charts.

#### GAP-S12: `ChartSeriesStack` child component
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (stacked-series.md) |
| Type | N/A | Component with `Enabled`, `Group`, `Type` parameters |
**Priority:** P2 (this phase)
**Notes:** Full stacking feature (simple, named, 100%) not implemented.

#### GAP-S13: `ChartSeriesTooltip` child component
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (tooltip/overview.md) |
| Type | N/A | Per-series tooltip with `Visible`, `Background`, `Color`, `Template` |
**Priority:** P2 (this phase)
**Notes:** Source only has chart-level tooltip. Spec documents per-series tooltip configuration.

#### GAP-S14: `ChartSeriesLabels` child component
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (labels-template-and-format.md) |
| Type | N/A | Component with `Visible`, `Format`, `Template` |
**Priority:** P2 (this phase)
**Notes:** Data point labels on chart series.

#### GAP-S15: `ChartSeriesLegendItem` child component
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (legend.md) |
| Type | N/A | Per-series legend item customization |
**Priority:** P3 (next phase)
**Notes:** Custom legend item markers, highlight, etc.

#### GAP-S16: `ChartLegendTitle` child component
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (legend.md) |
| Type | N/A | Component with `Text`, `Background`, `Color` |
**Priority:** P3 (next phase)
**Notes:** Legend title customization.

#### GAP-S17: `ChartLegendItem` child component
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (legend.md) |
| Type | N/A | Component with `ChartLegendItemMarkers` child |
**Priority:** P3 (next phase)
**Notes:** Legend item marker customization.

#### GAP-S18: `ChartLegendBorder` child component
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (legend.md) |
| Type | N/A | Legend border customization |
**Priority:** P3 (next phase)
**Notes:** Legend border settings.

#### GAP-S19: `ChartTooltip.SharedTemplate` render fragment
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (tooltip/shared.md) |
| Type | N/A | `RenderFragment` with multi-series context |
**Priority:** P2 (this phase)
**Notes:** Spec documents shared tooltip template with `context.Category` and `context.Points` collection. Source `Shared` parameter exists but shared rendering logic is not implemented.

#### GAP-S20: `ChartCategoryAxis.Type` parameter
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (date-axis.md) |
| Type | N/A | `ChartCategoryAxisType` enum (Date) |
**Priority:** P2 (this phase)
**Notes:** Date axis type for time-series data.

#### GAP-S21: `ChartCategoryAxis.BaseUnit` parameter
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (date-axis.md) |
| Type | N/A | `ChartCategoryAxisBaseUnit` enum |
**Priority:** P3 (next phase)
**Notes:** Date grouping (days, weeks, months, years).

#### GAP-S22: `ChartCategoryAxis.WeekStartDay` parameter
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (date-axis.md) |
| Type | N/A | `int` |
**Priority:** P3 (next phase)
**Notes:** Week start day for date axis base unit.

#### GAP-S23: `ChartCategoryAxis.AxisCrossingValue` parameter
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (multiple-axes.md) |
| Type | N/A | `object[]` |
**Priority:** P3 (next phase)
**Notes:** Multi-axis crossing point configuration.

#### GAP-S24: `ChartValueAxis.Type` parameter
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (multiple-axes.md) |
| Type | N/A | `ChartValueAxisType` enum |
**Priority:** P3 (next phase)
**Notes:** Value axis type (Numeric, etc).

#### GAP-S25: `ChartValueAxis.Visible` parameter
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (multiple-axes.md) |
| Type | N/A | `bool` |
**Priority:** P3 (next phase)
**Notes:** Axis visibility toggle.

#### GAP-S26: `ChartValueAxis.AxisCrossingValue` parameter
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (multiple-axes.md) |
| Type | N/A | `object[]` |
**Priority:** P3 (next phase)
**Notes:** Multi-axis crossing point configuration.

#### GAP-S27: `ChartValueAxisLabels` child component
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (multiple-axes.md) |
| Type | N/A | Component with `Template`, `Visible`, `Format` |
**Priority:** P3 (next phase)
**Notes:** Value axis label customization.

#### GAP-S28: `ChartValueAxisTitle` child component
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (multiple-axes.md) |
| Type | N/A | Component (source uses string `Title` param instead) |
**Priority:** P3 (next phase)
**Notes:** Source implements `Title` as a string parameter on ChartValueAxis. Spec uses a nested child component pattern.

#### GAP-S29: `ChartXAxes` / `ChartXAxis` / `ChartYAxes` / `ChartYAxis` components
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (data-bind.md, multiple-axes.md) |
| Type | N/A | Numerical chart axis components |
**Priority:** P2 (this phase)
**Notes:** Spec documents separate X/Y axis components for numerical (scatter/bubble) charts. Source reuses ChartCategoryAxis/ChartValueAxis for all chart types.

#### GAP-S30: `ChartPannable` child component
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (pan-and-zoom/pan.md) |
| Type | N/A | Component with `Enabled` and key config |
**Priority:** P3 (next phase)
**Notes:** Pan/zoom features not implemented.

#### GAP-S31: `ChartZoomable` child component
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (pan-and-zoom/zoom.md) |
| Type | N/A | Component for zoom configuration |
**Priority:** P3 (next phase)
**Notes:** Pan/zoom features not implemented.

#### GAP-S32: `ChartPlotBands` / `PlotBand` child components
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (plot-bands.md) |
| Type | N/A | Components with `From`, `To`, `Color`, `Opacity` |
**Priority:** P3 (next phase)
**Notes:** Background highlighting ranges not implemented.

#### GAP-S33: Trendline series support
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (trendlines.md) |
| Type | N/A | Linear, Moving Average, Exponential, Logarithmic, Power, Polynomial |
**Priority:** P3 (next phase)
**Notes:** Full trendline feature not implemented.

#### GAP-S34: `ChartSettings` / `NoDataTemplate` child component
| | Source | Spec |
|--|--------|------|
| Exists | No (hardcoded "No data available") | Yes (templates.md) |
| Type | N/A | `RenderFragment` for custom no-data content |
**Priority:** P3 (next phase)
**Notes:** Source renders a hardcoded "No data available" div. Spec allows customization via `NoDataTemplate`.

#### GAP-S35: Accessibility aria label templates
| | Source | Spec |
|--|--------|------|
| Exists | Partial | Yes (labels-template-and-format.md) |
| Type | Hardcoded aria-label strings | Configurable templates |
**Priority:** P3 (next phase)
**Notes:** Source has fixed aria-labels. Spec documents customizable aria label templates.

#### GAP-S36: Additional chart series types
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (types/ folder) |
| Type | N/A | Candlestick, OHLC, RangeArea, RangeBar, RangeColumn, RadarArea, RadarColumn, RadarLine, Waterfall, Heatmap |
**Priority:** P2 (this phase) for Candlestick/OHLC; P3 for others
**Notes:** Source enum has 9 types. Spec documents 19+ types. MariloStockChart partially covers Candlestick but as a separate component, not as a ChartSeriesType.

#### GAP-S37: `ChartSeries.XAxis` and `YAxis` parameters
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (multiple-axes.md) |
| Type | N/A | `string` |
**Priority:** P3 (next phase)
**Notes:** Named axis binding for numerical charts.

#### GAP-S38: `ChartCategoryAxisTitle` child component
| | Source | Spec |
|--|--------|------|
| Exists | No | Yes (multiple-axes.md) |
| Type | N/A | Component (source uses string `Title` param instead) |
**Priority:** P3 (next phase)
**Notes:** Source implements `Title` as a string parameter. Spec uses nested child component pattern.

---

### C. Mismatch (both exist but differ)

#### GAP-M01: `ChartLegend.Position` type
| | Source | Spec |
|--|--------|------|
| Type | `ChartPosition` enum | `ChartLegendPosition` enum referenced in spec |
**Priority:** P2 (this phase)
**Notes:** Source uses a shared `ChartPosition` enum (Top/Bottom/Left/Right). Spec references `ChartLegendPosition.Right`, `ChartLegendPosition.Bottom`. Either the enum should be renamed or spec examples should be updated to match source.

#### GAP-M02: `ChartTitle.Position` type
| | Source | Spec |
|--|--------|------|
| Type | `ChartPosition` enum | `ChartTitlePosition` enum referenced in spec |
**Priority:** P2 (this phase)
**Notes:** Same issue as M01. Spec references `ChartTitlePosition.Top` and `ChartTitlePosition.Bottom` but source uses shared `ChartPosition` enum.

#### GAP-M03: `ChartSubtitle.Position` type
| | Source | Spec |
|--|--------|------|
| Type | `ChartPosition` enum | `ChartSubtitlePosition` enum referenced in spec |
**Priority:** P2 (this phase)
**Notes:** Same pattern. Spec references `ChartSubtitlePosition.Bottom`.

#### GAP-M04: `ChartSeriesClickEventArgs.Category` type
| | Source | Spec |
|--|--------|------|
| Type | `string?` | `object` (castable to DateTime, string, int) |
**Priority:** P2 (this phase)
**Notes:** Source stores Category as `string?`. Spec documents it as `object` that consumers cast to their data type (DateTime, string, int). This limits date axis support.

#### GAP-M05: `ChartLegendItemClickEventArgs.PointIndex` type
| | Source | Spec |
|--|--------|------|
| Type | `int` | `int?` |
**Priority:** P3 (next phase)
**Notes:** Spec says PointIndex is nullable (applies only to pie/donut). Source declares it as non-nullable `int`.

#### GAP-M06: `ChartSeries` naming convention
| | Source | Spec |
|--|--------|------|
| Component name | `MariloChartSeries` | `ChartSeries` |
**Priority:** P2 (this phase)
**Notes:** Spec examples use `<ChartSeries>` tag name. Source component is `MariloChartSeries.razor`. If the component is registered via `@using` with a different tag name, this may work. Verify tag name resolution matches spec examples.

#### GAP-M07: Axis title implementation pattern
| | Source | Spec |
|--|--------|------|
| Pattern | String parameter `Title` on axis component | Child component `<ChartValueAxisTitle>` / `<ChartCategoryAxisTitle>` |
**Priority:** P2 (this phase)
**Notes:** Source uses `[Parameter] public string? Title` on ChartCategoryAxis and ChartValueAxis. Spec uses nested child components like `<ChartValueAxisTitle Text="...">`. Both achieve the same result but the API shape differs.

---

## Summary by Priority

### P1 -- Blocking (0 gaps)
No blocking gaps identified.

### P2 -- This Phase (17 gaps)
| ID | Type | Description |
|----|------|-------------|
| GAP-U03 | Undocumented | `Palette` parameter |
| GAP-U05 | Undocumented | `OnRender` event |
| GAP-S03 | Spec-ahead | `OnAxisLabelClick` event |
| GAP-S06 | Spec-ahead | `ChartSeries.Axis` multi-axis binding |
| GAP-S07 | Spec-ahead | `ChartSeries.CategoryAxis` multi-axis binding |
| GAP-S10 | Spec-ahead | `ChartSeries.ColorField` per-point colors |
| GAP-S12 | Spec-ahead | `ChartSeriesStack` stacking feature |
| GAP-S13 | Spec-ahead | `ChartSeriesTooltip` per-series tooltip |
| GAP-S14 | Spec-ahead | `ChartSeriesLabels` data point labels |
| GAP-S19 | Spec-ahead | `ChartTooltip.SharedTemplate` |
| GAP-S20 | Spec-ahead | `ChartCategoryAxis.Type` (Date axis) |
| GAP-S29 | Spec-ahead | `ChartXAxes`/`ChartYAxes` numerical axis components |
| GAP-S36 | Spec-ahead | Additional chart types (Candlestick, OHLC priority) |
| GAP-M01 | Mismatch | `ChartLegend.Position` enum name |
| GAP-M02 | Mismatch | `ChartTitle.Position` enum name |
| GAP-M04 | Mismatch | `Category` type (string vs object) |
| GAP-M06 | Mismatch | `ChartSeries` tag name |
| GAP-M07 | Mismatch | Axis title API pattern |

### P3 -- Next Phase (35 gaps)
All remaining gaps listed above (GAP-U01, U02, U04, U06, U07, S01, S02, S04, S05, S08, S09, S11, S15-S18, S21-S28, S30-S35, S37, S38, M03, M05).
