# Resolution Design: DropZoneId JS Interop

## Scope
- **GAP-FU-001**: MariloFileUpload — DropZoneId parameter declared but inert
- **GAP-UPL-003**: MariloUpload — DropZoneId parameter declared but inert

## Target State

When `DropZoneId` is set to a DOM element's id, files dragged onto that external element are forwarded to the component's hidden `<input type="file">`, triggering the same selection flow as direct drop/browse.

## Resolution Design

### JS Module: `marilo-dropzone.js`

New ES module in `wwwroot/js/` following the shared interop pattern.

**Exports:**
- `registerDropZone(dropZoneElementId, fileInputElementId)` → `number` (handle ID)
  - Finds both elements by ID
  - Attaches `dragover`, `dragleave`, `drop` listeners to the external element
  - On drop: sets `inputElement.files = event.dataTransfer.files` and dispatches a `change` event (bubbles) to trigger Blazor's InputFile handler
  - Adds/removes `mar-dropzone--drag-over` CSS class during drag
  - Returns a handle ID for cleanup
- `unregisterDropZone(handleId)` → `void`
  - Removes listeners and cleanup

### C# Service: `IDropZoneService` / `DropZoneService`

Internal service following the `IMariloJsModuleLoader` pattern (like `ResizeObserverService`).

```csharp
internal interface IDropZoneService
{
    ValueTask<int> RegisterAsync(string dropZoneElementId, string fileInputElementId, CancellationToken ct = default);
    ValueTask UnregisterAsync(int handleId, CancellationToken ct = default);
}
```

Registered as scoped in `InteropServiceExtensions.AddMariloInteropServices()`.

### Component Wiring

Both `MariloFileUpload` and `MariloUpload`:
1. Inject `IDropZoneService`
2. Implement `IAsyncDisposable`
3. In `OnAfterRenderAsync(firstRender)`: when `DropZoneId` is set, call `RegisterAsync(DropZoneId, InputId)`
4. Track the handle ID; on re-render if `DropZoneId` changed, unregister old + register new
5. In `DisposeAsync`: unregister if active

### Tests

bUnit tests verifying:
- DropZoneId parameter renders without error
- Component implements IAsyncDisposable
- Service registration in DI

## Decision Rationale

- **Shared JS module** rather than inline per-component: avoids duplicating logic, follows existing interop patterns
- **Forward to InputFile via `files` setter + `change` event**: cleanest integration with Blazor's `InputFile` — no need for separate `[JSInvokable]` callbacks
- **Service pattern**: consistent with existing `IResizeObserverService`, `IDragService` etc.
