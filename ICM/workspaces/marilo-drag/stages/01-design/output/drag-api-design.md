# marilo-drag.ts API Design

## Module Entry Point
`src/Marilo.Components/wwwroot/js/marilo-drag.ts`

## TypeScript API

### Types

```typescript
interface DragOptions {
  /** Constrain drag movement to an axis. 'both' = free movement. */
  axis: 'x' | 'y' | 'both';
  /** Snap to pixel grid. null = free positioning. */
  snapToGrid: number | null;
  /** Prevent dragging outside the parent element bounds. */
  constrainToParent: boolean;
  /** Minimum distance in pixels before drag begins (prevents accidental drags). */
  threshold: number;
  /** CSS cursor to apply during drag. */
  cursor: string;
}

interface DragEventData {
  /** Current X position relative to parent. */
  x: number;
  /** Current Y position relative to parent. */
  y: number;
  /** Delta X from drag start. */
  deltaX: number;
  /** Delta Y from drag start. */
  deltaY: number;
  /** The element ID being dragged. */
  elementId: string;
}
```

### Exported Functions

```typescript
/**
 * Initialize drag behavior on an element.
 * Attaches pointerdown/pointermove/pointerup listeners.
 * Uses pointer capture for reliable tracking.
 */
export function initDrag(
  elementId: string,
  options: DragOptions,
  dotNetRef: DotNetObjectReference
): void;

/**
 * Remove drag behavior and clean up listeners.
 */
export function disposeDrag(elementId: string): void;
```

### .NET Callbacks (invoked via DotNetObjectReference)
- `OnDragStart(DragEventData data)` — called when drag threshold is exceeded
- `OnDragMove(DragEventData data)` — called on each pointer move during drag
- `OnDragEnd(DragEventData data)` — called on pointer up

### Implementation Notes
- Uses `pointerdown`/`pointermove`/`pointerup` (not mouse events) for touch support
- Calls `element.setPointerCapture(e.pointerId)` on drag start
- Releases capture on drag end
- Tracks `_activeDrags: Map<string, DragState>` for concurrent drags
- ESM module pattern: `export { initDrag, disposeDrag }`

## .NET Interop Interface

```csharp
namespace Marilo.Components.Services;

public class DragOptions
{
    public string Axis { get; set; } = "both";
    public int? SnapToGrid { get; set; }
    public bool ConstrainToParent { get; set; }
    public int Threshold { get; set; } = 3;
    public string Cursor { get; set; } = "grabbing";
}

public class DragEventData
{
    public double X { get; set; }
    public double Y { get; set; }
    public double DeltaX { get; set; }
    public double DeltaY { get; set; }
    public string ElementId { get; set; } = "";
}

/// <summary>
/// JS interop service for drag operations. Shared across components.
/// </summary>
public interface IJSInteropDragService : IAsyncDisposable
{
    Task InitDragAsync(string elementId, DragOptions options, 
        DotNetObjectReference<IDragHandler> handler);
    Task DisposeDragAsync(string elementId);
}

/// <summary>
/// Interface that components implement to receive drag callbacks.
/// </summary>
public interface IDragHandler
{
    [JSInvokable] Task OnDragStart(DragEventData data);
    [JSInvokable] Task OnDragMove(DragEventData data);
    [JSInvokable] Task OnDragEnd(DragEventData data);
}
```

## Consumer Contracts

### MariloGantt
| Feature | DragOptions | Notes |
|---------|------------|-------|
| Timeline bar drag-move | axis: 'x', constrainToParent: true | Move task start/end dates |
| Timeline bar resize (left) | axis: 'x', cursor: 'w-resize' | Change task start date |
| Timeline bar resize (right) | axis: 'x', cursor: 'e-resize' | Change task end date |
| Column reorder | axis: 'x', constrainToParent: true | Reorder tree list columns |
| Column resize | axis: 'x', cursor: 'col-resize' | Resize tree list columns |

### MariloWindow (future consolidation)
| Feature | DragOptions |
|---------|------------|
| Window drag | axis: 'both' |
| Window resize | axis: 'both', cursor varies by edge |

### MariloSplitter (future consolidation)
| Feature | DragOptions |
|---------|------------|
| Pane resize | axis: dependent on orientation |

## Gaps Unblocked by This Module
- Column reorder (Gantt)
- Column resize (Gantt)
- Timeline bar drag-move (Gantt)
- Timeline bar resize (Gantt)
- Drag-specific screen reader announcements (requires drag events)
