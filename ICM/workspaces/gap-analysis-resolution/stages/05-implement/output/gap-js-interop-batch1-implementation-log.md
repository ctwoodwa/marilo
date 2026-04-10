# Implementation Log: JS Interop Batch 1

## Scope
- GAP-FU-001: MariloFileUpload DropZoneId
- GAP-UPL-003: MariloUpload DropZoneId
- GAP-EDITOR-002: MariloEditor Adaptive Toolbar

## Date: 2026-04-09

## Implementation Details

### GAP-FU-001 + GAP-UPL-003: DropZoneId JS Interop

**New files created:**
- `src/Marilo.Components/wwwroot/js/marilo-dropzone.js` — ES module: `registerDropZone`, `unregisterDropZone`. Forwards dropped files to hidden InputFile via `files` setter + `change` event dispatch. Handles `dragenter`, `dragover`, `dragleave`, `drop` with `mar-dropzone--drag-over` CSS class.
- `src/Marilo.Components/Internal/Interop/IDropZoneService.cs` — Internal interface: `RegisterAsync`, `UnregisterAsync`
- `src/Marilo.Components/Internal/Interop/DropZoneService.cs` — Implementation using `IMariloJsModuleLoader` pattern

**Modified files:**
- `src/Marilo.Components/Internal/Interop/InteropServiceExtensions.cs` — Added `services.AddScoped<IDropZoneService, DropZoneService>()`
- `src/Marilo.Components/Forms/Inputs/MariloFileUpload.razor` — Added `@implements IAsyncDisposable`
- `src/Marilo.Components/Forms/Inputs/MariloFileUpload.razor.cs` — Inject IDropZoneService, OnAfterRenderAsync registration, DisposeAsync, parameter change handling
- `src/Marilo.Components/Forms/Inputs/MariloUpload.razor` — Added `@implements IAsyncDisposable`
- `src/Marilo.Components/Forms/Inputs/MariloUpload.razor.cs` — Same pattern as FileUpload

**Tests:**
- `tests/Marilo.Tests.Unit/Forms/Inputs/MariloFileUploadDropZoneTests.cs` — 5 tests (null/set DropZoneId, IAsyncDisposable, parameter change, clear to null)
- `tests/Marilo.Tests.Unit/Forms/Inputs/MariloUploadDropZoneTests.cs` — 5 tests (same)
- `tests/Marilo.Tests.Unit/Interop/SharedInteropServiceTests.cs` — 1 DI registration test added

### GAP-EDITOR-002: Editor Adaptive Toolbar

**Modified files:**
- `src/Marilo.Components/Editors/MariloEditor.razor` — Added `Adaptive` parameter, injected IResizeObserverService + IElementMeasurementService, overflow state, ToolbarItem record struct, GetAllToolbarItems/GetVisibleItems/GetOverflowItems helpers, RenderToolbarItem helper (deduplicated rendering), RecalculateOverflow with two-pass algorithm (no 40px deduction when all items fit, excludes More button from measurements), overflow popup with focusout dismissal, ResizeObserver setup with initial measurement
- `src/Marilo.Components/Internal/Interop/IElementMeasurementService.cs` — Added `GetChildWidthsAsync`
- `src/Marilo.Components/Internal/Interop/ElementMeasurementService.cs` — Implemented `GetChildWidthsAsync`
- `src/Marilo.Components/wwwroot/js/marilo-measurement.js` — Added `getChildWidths` export

**Tests:**
- `tests/Marilo.Tests.Unit/Editors/MariloEditorAdaptiveTests.cs` — 7 tests (default false, renders all tools when false/true, custom tools participate, no-overflow behavior, no-throw when disabled+adaptive)

## Review Cycle

1. Initial implementation by two parallel subagents
2. Spec compliance review found: missing re-registration tests (DropZoneId), 40px premature overflow + no initial measurement (Editor)
3. Fixes applied: 4 additional tests, overflow algorithm fix, initial measurement with null-safety
4. Code quality review found: critical measurement feedback loop, missing dragenter, no popup dismissal, duplicated rendering, inline styles
5. Final fixes: feedback loop fix (exclude More button from widths), dragenter added, focusout dismissal, RenderToolbarItem helper, CSS class for popup

## Build & Test
- Build: clean, 0 warnings, 0 errors
- Tests: 1067/1067 passing (11 new for DropZoneId, 7 new for Editor adaptive = 18 new tests)
