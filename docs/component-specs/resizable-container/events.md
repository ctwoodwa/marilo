---
title: ResizableContainer Events
page_title: MariloResizableContainer Events
description: Event handlers for the ResizableContainer component.
slug: resizable-container-events
tags: resizable-container, events
published: true
position: 3
---

# Events

## OnResizeStart

Fires when the user begins a drag resize (pointer down on handle).

```razor
<MariloResizableContainer OnResizeStart="@OnStart">
    <p>Content</p>
</MariloResizableContainer>

@code {
    private Task OnStart(MariloResizeEventArgs args)
    {
        Console.WriteLine($"Resize started: {args.WidthPixels}x{args.HeightPixels}");
        return Task.CompletedTask;
    }
}
```

## OnResizing

Fires on each frame during a drag resize. Use for live feedback.

## OnResizeEnd

Fires when the drag resize completes (pointer up).

```razor
<MariloResizableContainer OnResizeEnd="@OnEnd">
    <p>Content</p>
</MariloResizableContainer>

@code {
    private Task OnEnd(MariloResizeEventArgs args)
    {
        Console.WriteLine($"Final size: {args.Width} x {args.Height}");
        Console.WriteLine($"Active edge: {args.ActiveEdge}");
        return Task.CompletedTask;
    }
}
```

## OnObservedSizeChanged

Fires when the ResizeObserver detects any size change on the container — including changes from CSS, layout shifts, and window resize, not only user drag.

```razor
<MariloResizableContainer ObserveSizeChanges="true"
                          OnObservedSizeChanged="@OnSizeChanged">
    <MariloChart Data="@_data" />
</MariloResizableContainer>

@code {
    private Task OnSizeChanged(MariloObservedSizeChangedEventArgs args)
    {
        // Redraw chart based on new size
        Console.WriteLine($"Observed: {args.WidthPixels}x{args.HeightPixels}");
        return Task.CompletedTask;
    }
}
```

## MariloResizeEventArgs

| Property | Type | Description |
|----------|------|-------------|
| Width | string | Width as CSS value (e.g., "400px") |
| Height | string | Height as CSS value |
| WidthPixels | double | Width in pixels |
| HeightPixels | double | Height in pixels |
| ActiveEdge | MariloResizeEdges | The edge/corner being dragged |
| IsUserInitiated | bool | True for user actions (drag, keyboard) |

## MariloObservedSizeChangedEventArgs

| Property | Type | Description |
|----------|------|-------------|
| Width | string | Observed width as CSS value |
| Height | string | Observed height as CSS value |
| WidthPixels | double | Observed width in pixels |
| HeightPixels | double | Observed height in pixels |
