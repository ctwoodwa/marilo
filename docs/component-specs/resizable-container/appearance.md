---
title: ResizableContainer Appearance
page_title: MariloResizableContainer Appearance
description: Appearance options and styling for the ResizableContainer component.
slug: resizable-container-appearance
tags: resizable-container, appearance, styling
published: true
position: 2
---

# Appearance

## Resize Edges

The `ResizeEdges` parameter controls which edges and corners have handles:

```razor
@* Bottom-right corner only (default) *@
<MariloResizableContainer ResizeEdges="MariloResizeEdges.BottomRight" />

@* Right edge only *@
<MariloResizableContainer ResizeEdges="MariloResizeEdges.Right" />

@* Bottom edge only *@
<MariloResizableContainer ResizeEdges="MariloResizeEdges.Bottom" />

@* All edges and corners *@
<MariloResizableContainer ResizeEdges="MariloResizeEdges.All" />
```

## MariloResizeEdges Values

| Value | Description |
|-------|-------------|
| None | No resize handles |
| Right | Right edge |
| Bottom | Bottom edge |
| BottomRight | Bottom-right corner (default) |
| Left | Left edge |
| Top | Top edge |
| TopLeft | Top-left corner |
| TopRight | Top-right corner |
| BottomLeft | Bottom-left corner |
| All | All edges and corners |

This is a `[Flags]` enum — values can be combined using bitwise OR.

## Ghost Outline

When `UseGhostOutline` is true, a dashed outline shows the target size during drag instead of live resizing:

```razor
<MariloResizableContainer UseGhostOutline="true">
    <p>Content stays stable during drag.</p>
</MariloResizableContainer>
```

## Disabled State

When `Enabled` is false, the container renders in a dimmed state with no active handles:

```razor
<MariloResizableContainer Enabled="false">
    <p>This container cannot be resized.</p>
</MariloResizableContainer>
```

## Custom Handle Styling

Use `HandleClass` and `HandleStyle` to customize handle appearance:

```razor
<MariloResizableContainer HandleClass="my-custom-handle"
                          HandleStyle="background: red;">
    <p>Custom handle appearance.</p>
</MariloResizableContainer>
```

## CSS Classes

| Class | Element |
|-------|---------|
| `mar-resizable-container` | Root container |
| `mar-resizable-container--resizing` | Root during drag |
| `mar-resizable-container--disabled` | Root when disabled |
| `mar-resizable-container__content` | Content wrapper |
| `mar-resizable-container__handle` | Handle element |
| `mar-resizable-container__handle--[edge]` | Handle position modifier |
| `mar-resizable-container__handle--active` | Handle during drag |
| `mar-resizable-container__handle--focused` | Handle with focus |
| `mar-resizable-container__ghost` | Ghost outline during drag |
