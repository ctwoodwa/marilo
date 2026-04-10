# MariloChart — Demo Gap List

**Audit date:** 2026-04-10
**Existing demo page:** `samples/Marilo.Demo/Pages/Components/Chart/Chart/Overview.razor`
**Current scenario count:** 12 (updated from 2)
**Target scenario count:** 12

---

## Demo Scenarios

| # | Section | Chart Types / Features Covered |
|---|---------|-------------------------------|
| 1 | Line Chart | ChartSeriesType.Line, multi-series, Color |
| 2 | Column Chart | ChartSeriesType.Column, multi-series |
| 3 | Area Chart | ChartSeriesType.Area |
| 4 | Bar Chart | ChartSeriesType.Bar (horizontal) |
| 5 | Pie Chart | ChartSeriesType.Pie |
| 6 | Donut Chart | ChartSeriesType.Donut |
| 7 | Scatter Chart | ChartSeriesType.Scatter, XField, YField |
| 8 | Bubble Chart | ChartSeriesType.Bubble, SizeField |
| 9 | Title & Subtitle | ChartTitle, ChartSubtitle child components |
| 10 | Legend & Tooltips | ShowLegend, ShowTooltips toggles |
| 11 | Series Visibility | Visible parameter per series |
| 12 | Click Events | OnSeriesClick, ChartSeriesClickEventArgs |

---

## Coverage Assessment

All implemented chart types and key interactive features are now demonstrated. Remaining spec-ahead features (stacking, drilldown, pan/zoom, trendlines, plot bands, additional chart types) are not yet implemented and therefore not demoed.
