# Resolution Design: Editor Adaptive Toolbar

## Scope
- **GAP-EDITOR-002**: MariloEditor — `Adaptive` parameter for responsive toolbar overflow

## Target State

When `Adaptive` is true, the editor toolbar automatically detects when tool buttons overflow the available width, hides overflowing buttons, and renders a "More" overflow button that opens a popup containing the hidden tools.

## Resolution Design

### Parameter Addition

```csharp
/// <summary>When true, the toolbar automatically overflows items into a popup menu.</summary>
[Parameter] public bool Adaptive { get; set; }
```

### C# Implementation

1. Inject `IResizeObserverService` and `IElementMeasurementService` (both already exist)
2. Add `@ref` on the toolbar container div (`_toolbarRef`)
3. Track overflow state:
   - `_overflowStartIndex` — the index at which tools start overflowing (-1 = no overflow)
   - `_showOverflowPopup` — whether the overflow popup is open
4. In `OnAfterRenderAsync`:
   - When `Adaptive` is true and not yet observing, call `IResizeObserverService.ObserveAsync(_toolbarRef, OnToolbarResized)`
   - Store the `IAsyncDisposable` handle for cleanup
5. `OnToolbarResized` callback:
   - Call `IElementMeasurementService.GetBoundingClientRectAsync(_toolbarRef)` to get toolbar width
   - Calculate cumulative button widths (each button ~40px estimated, or measure via JS)
   - Set `_overflowStartIndex` to the first tool that doesn't fit
   - `InvokeAsync(StateHasChanged)`

### Toolbar Rendering Changes

When `Adaptive` is true and `_overflowStartIndex >= 0`:
- Render tools 0.._overflowStartIndex-1 normally
- Render a "More" (⋯) button at the end
- On click, toggle `_showOverflowPopup`
- When popup is open, render remaining tools in a vertical dropdown div

### JS Enhancement

Add a `measureToolButtons` function to the editor's JS module (or a new shared module):
- Takes the toolbar element reference
- Returns an array of widths for each child button element
- This avoids guessing button widths and handles different tool label sizes

Alternatively, add this to `marilo-measurement.js` as a generic `getChildWidths(element)` function.

### Disposal

- On `DisposeAsync`, dispose the resize observer handle
- On `Adaptive` changing from true to false, dispose the observer and reset overflow state

### Tests

bUnit tests:
- Adaptive parameter accepted
- When Adaptive=false, all tools render normally (no overflow button)
- When Adaptive=true, component injects ResizeObserverService (verify via DI)
- Overflow popup toggle behavior

## Decision Rationale

- **ResizeObserver** instead of polling: efficient, native browser API, already available via `IResizeObserverService`
- **Measure-based overflow** instead of CSS-only: CSS `overflow:hidden` can't render a "More" button; programmatic measurement is needed
- **Vertical dropdown** for overflow popup: simple implementation, consistent with common toolbar patterns
- **JS measurement helper**: accurate widths without hardcoding, handles variable-length labels
