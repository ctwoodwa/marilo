# Implementation Log: MariloChart Batch 2 — Events & Polish

**Scope:** batch
**Phase:** Batch 2 (Medium-severity gaps)
**Status:** Complete
**Date:** 2026-04-04

## Summary

Resolved 5 Chart gaps (GAP-CHART-009, 011, 012, 013, 014). GAP-CHART-011 was already resolved (parameter names align with spec). 4 gaps required code changes.

## Tasks Completed

| Task | File(s) Modified | Status | Notes |
|------|-----------------|--------|-------|
| Add Bubble rendering case | `src/Marilo.Components/Charts/MariloChart.razor` | ✅ Complete | New `case ChartSeriesType.Bubble:` with `RenderBubbleSeries` method |
| RenderBubbleSeries method | `src/Marilo.Components/Charts/MariloChart.razor` | ✅ Complete | Circles with radius scaled from BubbleSize, fill-opacity 0.6, ARIA labels |
| Transitions bool → bool? | `src/Marilo.Components/Charts/MariloChart.razor` | ✅ Complete | Changed parameter type; null = theme default |
| Add OnRender event | `src/Marilo.Components/Charts/MariloChart.razor` | ✅ Complete | EventCallback<ChartRenderEventArgs> fires in OnAfterRenderAsync |
| ChartRenderEventArgs class | `src/Marilo.Components/Charts/ChartEventArgs.cs` | ✅ Complete | Width, Height, SeriesCount, TotalDataPoints |
| ChartTooltipContext class | `src/Marilo.Components/Charts/ChartEventArgs.cs` | ✅ Complete | SeriesName, Category, Value, FormattedValue, Color, DataItem, Percentage |
| ChartTooltip Template param | `src/Marilo.Components/Charts/ChartTooltip.razor` | ✅ Complete | RenderFragment<ChartTooltipContext>? |
| ChartTooltip Shared param | `src/Marilo.Components/Charts/ChartTooltip.razor` | ✅ Complete | bool parameter for shared tooltip mode |
| _tooltipContext field | `src/Marilo.Components/Charts/MariloChart.razor` | ✅ Complete | Populated in ShowTooltip/ShowPieTooltip, cleared in HideTooltip |
| Template rendering in tooltip div | `src/Marilo.Components/Charts/MariloChart.razor` | ✅ Complete | Checks _chartTooltip.Template before falling back to _tooltipContent |
| Verify data binding alignment | — | ✅ Already resolved | Field, CategoryField, Data, Name, Type, XField, YField, SizeField all match spec |

## Tests

| Test file | Test name | Covers |
|-----------|-----------|--------|
| `tests/Marilo.Tests.Unit/Charts/ChartBatch2Tests.cs` | `Bubble_Series_Renders_Circles` | Bubble rendering (3 circles) |
| | `Bubble_Series_HasAria_Labels` | Bubble ARIA labels with size |
| | `Bubble_Series_CirclesHave_FillOpacity` | Bubble visual distinction |
| | `Transitions_Defaults_To_Null` | bool? default |
| | `Transitions_Can_Be_Set_False` | Explicit false |
| | `OnRender_EventCallback_Accepted` | OnRender fires with SeriesCount + TotalDataPoints |
| | `ChartRenderEventArgs_HasDimensions` | Width/Height in event args |
| | `ChartTooltip_Template_Parameter_Accepted` | Template RenderFragment accepted |
| | `ChartTooltip_Shared_Parameter_Accepted` | Shared parameter accepted |
| | `ChartTooltipContext_Has_Expected_Properties` | Context model properties |
| | `ChartSeries_HasExpected_DataBinding_Parameters` | Data binding param alignment verification |

**Total new tests:** 11 bUnit tests
**Combined Chart total:** 27 bUnit tests (16 Batch 1 + 11 Batch 2)

## Files Changed

| File | Change Type | Reason |
|------|-------------|--------|
| `src/Marilo.Components/Charts/MariloChart.razor` | edit | Bubble rendering, Transitions nullable, OnRender event, tooltip template support |
| `src/Marilo.Components/Charts/ChartEventArgs.cs` | edit | ChartRenderEventArgs, ChartTooltipContext classes |
| `src/Marilo.Components/Charts/ChartTooltip.razor` | edit | Template and Shared parameters |
| `tests/Marilo.Tests.Unit/Charts/ChartBatch2Tests.cs` | new | 11 bUnit tests |

## Deviations from Resolution Record

- **OnAxisRender:** Deferred per resolution design — would require intercepting inline axis rendering pipeline.
- **Shared tooltip rendering:** `Shared` parameter is accepted but the multi-series shared tooltip rendering requires additional work in the tooltip show logic to gather all series values at a category index. The parameter is wired; full rendering deferred to a follow-up.
