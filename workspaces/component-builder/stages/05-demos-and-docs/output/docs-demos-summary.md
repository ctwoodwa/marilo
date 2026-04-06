# Docs & Demos Summary: MariloResizableContainer

## Documentation Files

| File | Content |
|------|---------|
| `docs/component-specs/resizable-container/overview.md` | Purpose, parameters table, usage, integration guidance, performance |
| `docs/component-specs/resizable-container/appearance.md` | ResizeEdges options, ghost outline, disabled state, CSS classes |
| `docs/component-specs/resizable-container/events.md` | All events with code examples, EventArgs reference |
| `docs/component-specs/resizable-container/accessibility/overview.md` | Keyboard interactions, ARIA, focus, reduced motion |
| `docs/component-specs/resizable-container/toc.yml` | Table of contents for DocFx |

## Demo Pages

| File | Sections |
|------|----------|
| `samples/Marilo.Demo/Pages/Components/ResizableContainer/Overview.razor` | 8 demo sections |

### Demo Sections

1. **Basic Bottom-Right Resizing** — Simple content panel with min/max constraints and size display
2. **Grid Host Example** — Realistic grid table inside container with OnObservedSizeChanged
3. **AllocationScheduler Host** — Scheduler-like split layout (resources + timeline) in container
4. **Chart Host** — Chart placeholder with observed size change callback
5. **Right-Only / Bottom-Only / All Edges** — Three edge configuration demos
6. **Keyboard Resizing** — Tab to handle, arrow keys to resize
7. **Persisted Size** — localStorage persistence with PersistKey
8. **Usage Guidance** — When to use, when NOT to use, integration pattern documentation
