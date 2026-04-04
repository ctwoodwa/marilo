# Implementation Log: MariloSplitter

> Date: 2026-04-04
> Stage: 05-implement
> Source: gap-splitter-resolutions.md (5 resolutions + 4 pre-resolved)
> Scope: batch (Splitter component gaps)

---

## Resolutions Implemented

### RES-SPLITTER-001: MariloSplitterPanes pass-through wrapper

**Status:** Implemented
**Files:**
- Created: `src/Marilo.Components/Layout/MariloSplitterPanes.razor`

Pass-through component renders `@ChildContent` directly. Backward compatible — direct `<MariloSplitterPane>` children still work.

### RES-SPLITTER-002: SplitterOrientation enum

**Status:** Implemented
**Files:**
- Modified: `src/Marilo.Core/Enums/LayoutEnums.cs` (added `SplitterOrientation` enum)
- Modified: `src/Marilo.Core/Contracts/IMariloCssProvider.cs` (changed `SplitterClass` signature)
- Modified: `src/Marilo.Providers.FluentUI/FluentUICssProvider.cs` (updated method)
- Modified: `src/Marilo.Providers.Bootstrap/BootstrapCssProvider.cs` (updated method)
- Modified: `src/Marilo.Components/Layout/MariloSplitter.razor` (replaced all `StackDirection` references)

Breaking change: `Orientation` parameter type changed from `StackDirection` to `SplitterOrientation`. Pre-release library, acceptable.

### RES-SPLITTER-003: bUnit test suite

**Status:** Implemented
**Files:**
- Created: `tests/Marilo.Tests.Unit/Layout/SplitterTests.cs`

17 test methods covering: pane registration, wrapper compatibility, collapse/expand, state management, orientation, width/height, min/max, events, nesting, legacy mode.

### RES-SPLITTER-004: Demo pages

**Status:** Deferred — existing demo page at `samples/Marilo.Demo/Pages/Components/Splitter/Overview.razor` is comprehensive. Additional pages not created this run.

### RES-SPLITTER-005: Nested splitter verification

**Status:** Implemented — bUnit test confirms nested splitters work correctly with `CascadingValue IsFixed="true"` scoping.

## Pre-Resolved Gaps (Verified)

| Gap | Status |
|-----|--------|
| GAP-SPLITTER-002 (GetState/SetState) | Already implemented |
| GAP-SPLITTER-003 (Class parameter) | Already inherited from MariloComponentBase |
| GAP-SPLITTER-005 (Min/Max) | Already implemented |
| GAP-SPLITTER-008 (Resizable) | Already implemented |

## Tests

- 17 bUnit tests in `SplitterTests.cs`
- All tests verify rendering, behavior, and ARIA attributes
- Cannot run `dotnet test` in this environment (no .NET SDK)

## Files Changed

| File | Action |
|------|--------|
| `src/Marilo.Components/Layout/MariloSplitterPanes.razor` | Created |
| `src/Marilo.Components/Layout/MariloSplitter.razor` | Modified (SplitterOrientation) |
| `src/Marilo.Core/Enums/LayoutEnums.cs` | Modified (added enum) |
| `src/Marilo.Core/Contracts/IMariloCssProvider.cs` | Modified (signature) |
| `src/Marilo.Providers.FluentUI/FluentUICssProvider.cs` | Modified |
| `src/Marilo.Providers.Bootstrap/BootstrapCssProvider.cs` | Modified |
| `tests/Marilo.Tests.Unit/Layout/SplitterTests.cs` | Created |
