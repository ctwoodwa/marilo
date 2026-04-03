# Gap Intake Inventory: MariloDiagram

> Component: MariloDiagram
> Intake date: 2026-04-03
> Intake mode: Fresh analysis (assess mode) -- no source code exists
> Spec docs: 6 files (overview, shapes, connections, events, layouts, data-bind)
> Demo page: Placeholder only ("Coming soon")

---

## 1. Source Code Status

**No source files exist.** A search of `/workspaces/Marilo/src/Marilo.Components/` found zero Diagram-related files. This is a **standard greenfield intake** -- every spec feature is a gap by definition.

## 2. Spec Summary

The spec documents a rich graph/diagram component with:

- **Shapes:** 10+ built-in shape types (Rectangle, Circle, Triangle, Diamond, flowchart shapes, Image, Text, custom Path), with content text, fill, stroke, size, position, connectors, visual functions, and data binding via `DiagramShapeDescriptor`.
- **Connections:** 2 types (Cascading, Polyline), with start/end caps (3 cap types), connection points, text labels with positioning, tooltips, selection handles, editability controls, styling, and visual functions. Data binding via `DiagramConnectionDescriptor`.
- **Layouts:** 3 layout types (Tree with 8 subtypes, Layered with 4 subtypes, Force), plus layout grid settings for multi-component diagrams.
- **Events:** 2 events (OnConnectionClick, OnShapeClick) with typed event args.
- **Data binding:** ShapesData/ConnectionsData parameters with descriptor classes, plus JSON load/save (LoadFromJsonAsync, SaveAsJsonAsync).
- **Interaction:** Zoom (4 params), Pan (2 params), Select/multi-select (3+ params), drag-and-drop connection creation.
- **Component API:** @ref methods (LoadFromJsonAsync, SaveAsJsonAsync), Height, Width, Zoom, ZoomRate, MaxZoom, MinZoom, ConnectionDefaults, ShapeDefaults.

**Estimated parameter/feature count:** ~40 top-level parameters, ~80 nested configuration tags, 2 events, 2+ async methods.

## 3. Demo Page Status

The demo page at `/workspaces/Marilo/samples/Marilo.Demo/Pages/Components/Diagram/Overview.razor` is a **placeholder only** -- displays a "Coming soon" alert with no functional component rendering.

## 4. Rough Gap Count

Since no source code exists, every documented feature is a gap:

| Feature Area | Estimated Gaps |
|---|---|
| Core component + rendering engine | 5 |
| Shape types and configuration | 12 |
| Connection types and configuration | 10 |
| Layout engine (Tree, Layered, Force) | 8 |
| Data binding (descriptors + JSON) | 6 |
| Interaction (zoom, pan, select) | 5 |
| Events | 2 |
| Styling/theming | 4 |
| Visual functions (JS interop) | 3 |
| **Total** | **~55** |

## 5. Severity Breakdown Estimate

| Severity | Count | Examples |
|---|---|---|
| Critical | ~18 | Core rendering, shape/connection basics, at least one layout, data binding, JS interop engine |
| Important | ~22 | All layout types/subtypes, connection text/caps/handles, zoom/pan/select, JSON load/save |
| Nice-to-have | ~15 | Visual functions, tooltip templates, custom Path shapes, advanced grid layout settings |

## 6. Delivery Workspace Recommendation

**YES -- merits its own delivery workspace.** The Diagram is a complex, JS-interop-heavy component with a dedicated rendering engine, ~80 nested configuration tags, and multiple sub-components (shapes, connections, layouts). Complexity is comparable to DataGrid. A dedicated `diagram-delivery/` workspace already exists in the workspace structure. Scope: `systematic`.

---

**Next step:** Proceed to Stage 02 (prioritize) or directly to Stage 03 (resolution design) for the core rendering engine and basic shape/connection support.
