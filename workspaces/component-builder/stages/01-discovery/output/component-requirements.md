# Component Requirements: MariloResizableContainer

## Component Identity

| Field | Value |
|-------|-------|
| Name | MariloResizableContainer |
| Category | Layout |
| Complexity | Medium-High (JS interop + pointer events + ResizeObserver) |
| Slug | resizable-container |
| JS Interop | Yes (pointer drag, ResizeObserver, keyboard resize, optional persistence) |
| Reference patterns | MariloSplitter (drag handles, pointer interop), MariloDrawer (layout container) |

## Purpose

A reusable wrapper/container component that allows end users to resize its child content via drag handles. Designed as a **container primitive** so that complex hosted components (AllocationScheduler, DataGrid, Scheduler, Gantt, Charts) remain focused on rendering while the container owns all resize logic.

This is NOT a multi-pane splitter. It wraps a single content area with optional resize handles on edges/corners.

## Primary Use Cases

| # | Scenario | Description |
|---|----------|-------------|
| UC-1 | Grid host | Wrap a MariloDataGrid so users can resize the grid area; grid reflows via normal layout |
| UC-2 | Scheduler host | Wrap an AllocationScheduler; container handles drag resize, scheduler responds to size change |
| UC-3 | Chart host | Wrap a chart that redraws on observed size change via OnObservedSizeChanged |
| UC-4 | Dashboard tile | Resizable tile in a dashboard layout with persisted dimensions |
| UC-5 | Editor panel | Resizable code/text editor panel with min/max constraints |
| UC-6 | Simple content | Basic resizable content area (text, images, previews) |
| UC-7 | Edge options | Right-only, bottom-only, corner, or all-edges resizing |
| UC-8 | Keyboard resize | Accessible resize via arrow keys on focused handle |

## Visual States

| State | Description |
|-------|-------------|
| Default | Container at initial width/height, handle visible but unobtrusive |
| Hover (handle) | Handle highlights on hover, cursor changes to resize direction |
| Active (dragging) | Handle in active state, optional ghost outline, text selection disabled |
| Focused (handle) | Visible focus ring on handle for keyboard users |
| Disabled | Resize disabled, handle hidden or inert, container at current size |
| Constrained | Container at min or max boundary |
| Ghost outline | Semi-transparent outline shows target size during drag (when UseGhostOutline=true) |

## Interactive Behavior

| Action | Trigger | Result |
|--------|---------|--------|
| Drag resize | pointerdown + pointermove on handle | Container resizes live; OnResizing fires |
| Resize start | pointerdown on handle | OnResizeStart fires |
| Resize end | pointerup | OnResizeEnd fires with final dimensions |
| Keyboard resize | Arrow keys on focused handle | Resize by small increment (~4px) |
| Keyboard resize (large) | Shift+Arrow on focused handle | Resize by larger increment (~20px) |
| Size observation | ResizeObserver detects change | OnObservedSizeChanged fires |
| Programmatic resize | SetSizeAsync() called | Container updates to specified dimensions |
| Reset | ResetSizeAsync() called | Container returns to initial Width/Height |
| Focus handle | FocusHandleAsync() or Tab to handle | Handle receives visible focus |

## Composition

- Accepts `RenderFragment ChildContent` for arbitrary child content
- No child component registration needed (standalone wrapper)
- No CascadingValue pattern required
- Does not participate in parent-child component trees

## Accessibility

| Aspect | Requirement |
|--------|-------------|
| Handle element | `<button>` element (implicit button role) |
| ARIA label | Configurable via HandleAriaLabel; default "Resize" |
| Keyboard: Arrow keys | Resize by small step (~4px) |
| Keyboard: Shift+Arrow | Resize by larger step (~20px) |
| Focus | Handle is focusable; visible focus ring using Marilo theme tokens |
| Focus trap | Must NOT trap focus |
| Reduced motion | Respect prefers-reduced-motion for transitions |
| Screen reader | Handle announces its purpose via aria-label |

## Data Binding

- Width and Height as string parameters (CSS values)
- Two-way binding via WidthChanged and HeightChanged EventCallbacks
- No complex model binding needed

## Theme Considerations

| Property | Varies between providers? |
|----------|--------------------------|
| Handle appearance | Yes — FluentUI uses subtle/accent tokens; Bootstrap uses border/shadow |
| Handle size | Slight variation (FluentUI more compact) |
| Focus ring | Provider-specific focus indicators |
| Cursor styles | Same across providers (CSS standard cursors) |
| Border/shadow | Provider-specific container border treatment |
| Ghost outline | Provider-specific outline color/style |

## Research References

- **Blazor Blueprint Resizable**: Panel groups, nested IDE layouts, min/max constraints, draggable handles
- **interact.js**: Strong resize-event model, edge/corner concepts
- **Simple Blazor resizable div**: JS interop with pointer events pattern

## Scope Boundaries

### In scope
- Single-container drag resize with configurable edges/corners
- Pointer event-based resize with JS interop
- ResizeObserver for size change notification
- Min/max constraints
- Keyboard resize
- Optional ghost outline during drag
- Optional size persistence via browser storage
- FluentUI and Bootstrap theming
- Accessible handle with focus and keyboard support

### Out of scope
- Multi-pane/split-layout resizing (use MariloSplitter or future ResizablePanelGroup)
- IDE-style adjacent panel resize
- Virtualized content orchestration beyond container observation
- Drag-to-reorder or drag-and-drop
- Native CSS resize: both as primary solution (too limited for Marilo theming/events)
