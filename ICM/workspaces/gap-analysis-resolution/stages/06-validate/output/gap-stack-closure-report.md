# Closure Report: GAP-stack — MariloStack Spacing, Alignment, Sizing

**Closure Status:** Resolved
**Validated:** 2026-04-02

## Criteria Verification

| Criterion | Implementation Found | Test Passing | Status |
|-----------|---------------------|-------------|--------|
| `Spacing` parameter sets CSS `gap` property | `MariloStack.razor:BuildStackStyles()` — `StyleBuilder.AddStyle("gap", Spacing!, !string.IsNullOrWhiteSpace(Spacing))` | `Spacing_SetsGapStyle` | ✅ |
| `Width` parameter sets inline `width` style | `MariloStack.razor:BuildStackStyles()` — `StyleBuilder.AddStyle("width", Width!, !string.IsNullOrWhiteSpace(Width))` | `Width_SetsWidthStyle` | ✅ |
| `Height` parameter sets inline `height` style | `MariloStack.razor:BuildStackStyles()` — `StyleBuilder.AddStyle("height", Height!, !string.IsNullOrWhiteSpace(Height))` | `Height_SetsHeightStyle` | ✅ |
| `HorizontalAlign` maps to `justify-content` on horizontal stack | `MariloStack.razor:BuildStackStyles()` — `justifyContent = Orientation == Horizontal ? HorizontalAlign : VerticalAlign` | `HorizontalAlign_Center_OnHorizontalStack_SetsJustifyContent` | ✅ |
| `VerticalAlign` maps to `align-items` on horizontal stack | `MariloStack.razor:BuildStackStyles()` — `alignItems = Orientation == Horizontal ? VerticalAlign : HorizontalAlign` | `VerticalAlign_Center_OnHorizontalStack_SetsAlignItems` | ✅ |
| `VerticalAlign` maps to `justify-content` on vertical stack (axis swap) | `MariloStack.razor:BuildStackStyles()` — axis swap when `Orientation == Vertical` | `VerticalAlign_Center_OnVerticalStack_SetsJustifyContent` | ✅ |
| `HorizontalAlign` maps to `align-items` on vertical stack (axis swap) | `MariloStack.razor:BuildStackStyles()` — axis swap when `Orientation == Vertical` | `HorizontalAlign_End_OnVerticalStack_SetsAlignItems` | ✅ |
| Default orientation is `Horizontal` | `MariloStack.razor:13` — `[Parameter] public StackDirection Orientation { get; set; } = StackDirection.Horizontal` | `DefaultOrientation_IsHorizontal` | ✅ |
| Parameter named `Orientation` (not `Direction`) | `MariloStack.razor:13` — `Orientation` parameter; old `Direction` parameter removed | `VerticalOrientation_AppliesVerticalClass` | ✅ |
| Default `Start` alignment emits no style properties | `MariloStack.razor:BuildStackStyles()` — `AddStyle` conditional guards on `!= StackAlignment.Start` | `DefaultAlignments_DoNotEmitJustifyContentOrAlignItems` | ✅ |
| Child content renders correctly | `MariloStack.razor:6` — `@ChildContent` inside `<div>` | `ChildContent_IsRendered` | ✅ |
| Solution builds with zero errors | All provider implementations updated consistently | N/A (build verification) | ✅ |
| Sample pages updated and compile | Both `Stack/Overview.razor` and `StackLayout/Overview.razor` updated | N/A (build verification) | ✅ |

## Evidence

- **Changed:**
  - `src/Marilo.Components/Layout/MariloStack.razor` — added `Orientation`, `HorizontalAlign`, `VerticalAlign`, `Spacing`, `Width`, `Height` parameters; removed `Direction` and `Alignment`; added `BuildStackStyles()` and `ToFlexValue()` helpers
  - `src/Marilo.Core/Contracts/IMariloCssProvider.cs` — `StackClass(StackDirection, StackAlignment)` simplified to `StackClass(StackDirection)`
  - `src/Marilo.Providers.Bootstrap/BootstrapCssProvider.cs` — alignment class logic removed; emits direction class only
  - `src/Marilo.Providers.FluentUI/FluentUICssProvider.cs` — alignment class logic removed; emits direction class only
  - `samples/Marilo.Demo/Pages/Components/Stack/Overview.razor` — updated to new parameter names
  - `samples/Marilo.Demo/Pages/Components/StackLayout/Overview.razor` — updated to new parameter names
- **Tests:** `tests/Marilo.Tests.Unit/Foundation/StackTests.cs` — 11 bUnit tests, all passing
- **Original gap no longer present:** Confirmed. `Spacing`, `Width`, and `Height` parameters now exist and drive inline styles. Two-axis alignment via `HorizontalAlign`/`VerticalAlign` replaces the removed single-axis `Alignment` parameter. Default orientation is `Horizontal`. The old `Direction` parameter is gone and replaced by `Orientation`.

## Enforcement Guardrails

- 11 bUnit tests in `StackTests.cs` cover every resolved criterion; any regression in `BuildStackStyles()` axis mapping, style emission, or parameter defaults will produce a test failure in CI
- `IMariloCssProvider.StackClass` interface signature is now direction-only — any provider implementation that re-introduces alignment class generation will produce a compile error or dead code that tests will not exercise
- `ToFlexValue` is exhaustive over `StackAlignment` with a `_` fallback, preventing silent failures for new enum values

## Follow-up Tasks

None.
