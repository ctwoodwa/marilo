# API Design: MariloResizableContainer

## Parameters

### Content

| Parameter | Type | Default | Required | Description |
|-----------|------|---------|----------|-------------|
| ChildContent | RenderFragment? | null | No | Content to render inside the resizable container |

### Sizing

| Parameter | Type | Default | Required | Description |
|-----------|------|---------|----------|-------------|
| Width | string | "100%" | No | Current width as CSS value |
| Height | string | "320px" | No | Current height as CSS value |
| MinWidth | string? | null | No | Minimum width constraint (CSS value) |
| MinHeight | string? | null | No | Minimum height constraint (CSS value) |
| MaxWidth | string? | null | No | Maximum width constraint (CSS value) |
| MaxHeight | string? | null | No | Maximum height constraint (CSS value) |

### Behavior

| Parameter | Type | Default | Required | Description |
|-----------|------|---------|----------|-------------|
| Enabled | bool | true | No | Whether resizing is enabled |
| ShowHandle | bool | true | No | Whether to render the resize handle(s) |
| ResizeEdges | MariloResizeEdges | BottomRight | No | Which edges/corners have resize handles |
| ObserveSizeChanges | bool | true | No | Use ResizeObserver to detect size changes |
| PersistSize | bool | false | No | Persist resized dimensions in browser storage |
| PersistKey | string? | null | No | Storage key for persisted dimensions |
| UseGhostOutline | bool | false | No | Show ghost outline during drag instead of live resize |
| ClampToParent | bool | false | No | Constrain resize within parent element bounds |
| DisableTextSelection | bool | true | No | Disable text selection while dragging |
| KeyboardResizeEnabled | bool | true | No | Allow keyboard arrow-key resizing |

### Handle / UX

| Parameter | Type | Default | Required | Description |
|-----------|------|---------|----------|-------------|
| HandleAriaLabel | string? | null | No | Accessible label for the resize handle (defaults to "Resize") |
| HandleClass | string? | null | No | Additional CSS class for handle element |
| HandleStyle | string? | null | No | Additional inline style for handle element |

### Inherited from MariloComponentBase

| Parameter | Type | Description |
|-----------|------|-------------|
| Class | string? | Consumer-supplied CSS class to append |
| Style | string? | Consumer-supplied inline style to append |
| AdditionalAttributes | Dictionary<string, object>? | Unmatched HTML attributes |

## Events

| Event | Type | When |
|-------|------|------|
| OnResizeStart | EventCallback\<MariloResizeEventArgs\> | Pointer down on handle begins a drag |
| OnResizing | EventCallback\<MariloResizeEventArgs\> | Each frame during drag |
| OnResizeEnd | EventCallback\<MariloResizeEventArgs\> | Pointer up ends drag |
| OnObservedSizeChanged | EventCallback\<MariloObservedSizeChangedEventArgs\> | ResizeObserver detects size change (any cause) |
| WidthChanged | EventCallback\<string\> | Two-way binding callback for Width |
| HeightChanged | EventCallback\<string\> | Two-way binding callback for Height |

## Enumerations

### MariloResizeEdges

File: `src/Marilo.Core/Enums/ResizableContainerEnums.cs`

```csharp
[Flags]
public enum MariloResizeEdges
{
    None = 0,
    Right = 1,
    Bottom = 2,
    Left = 4,
    Top = 8,
    BottomRight = Bottom | Right,
    TopLeft = Top | Left,
    TopRight = Top | Right,
    BottomLeft = Bottom | Left,
    All = Top | Bottom | Left | Right
}
```

### MariloResizeAxis

```csharp
public enum MariloResizeAxis
{
    Horizontal,
    Vertical,
    Both
}
```

## EventArgs Models

### MariloResizeEventArgs

File: `src/Marilo.Core/Models/MariloResizeEventArgs.cs`

```csharp
public sealed class MariloResizeEventArgs
{
    public string Width { get; init; } = default!;
    public string Height { get; init; } = default!;
    public double WidthPixels { get; init; }
    public double HeightPixels { get; init; }
    public MariloResizeEdges ActiveEdge { get; init; }
    public bool IsUserInitiated { get; init; }
}
```

### MariloObservedSizeChangedEventArgs

File: `src/Marilo.Core/Models/MariloObservedSizeChangedEventArgs.cs`

```csharp
public sealed class MariloObservedSizeChangedEventArgs
{
    public string Width { get; init; } = default!;
    public string Height { get; init; } = default!;
    public double WidthPixels { get; init; }
    public double HeightPixels { get; init; }
}
```

## CSS Provider Methods

Add to `IMariloCssProvider.cs`:

```csharp
// ── ResizableContainer ─────────────────────────────────────────────
string ResizableContainerClass(bool isResizing, bool isDisabled);
string ResizableContainerContentClass();
string ResizableContainerHandleClass(MariloResizeEdges edge, bool isActive, bool isFocused);
```

### CSS Class Naming (BEM with mar- prefix)

| Method | Primary classes |
|--------|----------------|
| ResizableContainerClass | `mar-resizable-container` `--resizing` `--disabled` |
| ResizableContainerContentClass | `mar-resizable-container__content` |
| ResizableContainerHandleClass | `mar-resizable-container__handle` `--right` `--bottom` `--bottom-right` `--left` `--top` `--top-left` `--top-right` `--bottom-left` `--active` `--focused` |

## Public Methods (via @ref)

| Method | Return | Description |
|--------|--------|-------------|
| SetSizeAsync(string width, string height) | Task | Programmatically set container dimensions |
| ResetSizeAsync() | Task | Reset to initial Width/Height values |
| FocusHandleAsync() | Task | Focus the primary resize handle |

## Rendering Structure

```razor
<div class="@_containerClass"
     style="@_containerStyle"
     @ref="_containerRef"
     @attributes="AdditionalAttributes">

    <div class="@_contentClass">
        @ChildContent
    </div>

    @foreach (var handle in _activeHandles)
    {
        <button type="button"
                class="@GetHandleClass(handle)"
                style="@HandleStyle"
                aria-label="@(HandleAriaLabel ?? "Resize")"
                @ref="@_handleRefs[handle]" />
    }

    @if (_isDragging && UseGhostOutline)
    {
        <div class="mar-resizable-container__ghost"
             style="@_ghostStyle" />
    }
</div>
```

## JS Interop Module

File: `src/Marilo.Components/wwwroot/js/resizable-container.js`

Responsibilities:
- Attach pointer event listeners to handle elements (pointerdown/pointermove/pointerup)
- Compute live width/height changes based on active edge
- Apply min/max constraints (parse CSS values to pixels)
- Optionally clamp to parent bounds
- Optionally disable text selection during drag (user-select: none on body)
- Fire callbacks to Blazor (.NET ref) during start/move/end
- Set up ResizeObserver on root element
- Forward keyboard events (arrow keys) for handle resize
- Optionally persist size to localStorage by key
- Restore persisted size on init
- Clean up all listeners and observers on dispose
