# Resolution Design: Phase 1

**Date:** 2026-04-05

## GAP-TEST-001: Parameter Coverage Tests

Add targeted test methods for parameters not yet asserted. Group by category:

| Test | Parameters Covered | Assertion |
|------|-------------------|-----------|
| ValueMode_Currency_Shows_Currency_Symbol | ValueMode=Currency | Markup contains currency indicator |
| ShowTargets_Renders_Target_Values | ShowTargets, Targets | Markup contains target value element |
| ShowDeltas_Renders_Delta_Values | ShowDeltas, DeltaDisplayMode | Markup contains delta element |
| SelectionMode_None_Prevents_Selection_Class | SelectionMode=None | No aria-selected="true" in cells |
| Height_Applied_To_Root | Height | Root style contains height value |
| Width_Applied_To_Root | Width | Root style contains width value |
| Custom_Class_Applied_To_Root | Class | Root element has custom class |
| EnableLoaderContainer_False_No_Loader | EnableLoaderContainer=false | No loader class in markup |
| EnableContextMenu_False_No_Menu | EnableContextMenu=false | No context menu class in markup |
| ShowCriticalPath_Adds_CriticalPath_Class | ShowCriticalPath=true | Markup contains critical-path class |

## GAP-TEST-002: Scenario Planning Tests

| Test | Assertion |
|------|-----------|
| AllocationSets_Renders_Scenario_Strip | Scenario strip toolbar rendered with set chips |
| ScenarioOverrides_Applies_Override_Values | Override value shown instead of baseline value |
| ShowBaselineDiff_Renders_Ghost_Bars | Baseline diff class present in markup |

## P3 Demo Gaps

Add one new demo page: `AdvancedFeatures.razor` covering all 12 remaining P3 gaps. Scenarios:

1. **Critical Path & Loader** — ShowCriticalPath toggle, EnableLoaderContainer toggle, Width/Class binding
2. **Scenario Lifecycle Events** — OnAllocationOverridden, OnScenarioStatusChanged, OnScenarioPromoted with event output
3. **Context Menu Gating** — CanExecuteAction to conditionally disable commands
4. **Explicit Date Range** — VisibleEnd binding, VisibleStartChanged two-way callback, BaselineDateFormat
