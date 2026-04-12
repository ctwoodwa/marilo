# MariloDiagram -- Spec Review (Stage 01)

**Date:** 2026-04-12
**Worker:** w-diagram-delivery
**Component source:** `src/Marilo.Components/DataDisplay/MariloDiagram.razor`
**Model source:** `src/Marilo.Core/Models/DiagramModels.cs`
**Spec folder:** `docs/component-specs/diagram/` (6 files)

---

## Summary

The spec files describe a **rich, declarative, enterprise-grade Diagram component** with dozens of child tags (`DiagramShape`, `DiagramConnection`, `DiagramLayout`, etc.), multiple shape types (26+ enum members), three layout algorithms, data binding via descriptor classes, JSON save/load, visual functions, connection caps, selection handles, tooltips, editability, zoom, pan, and two click events.

The **actual source** (`MariloDiagram.razor`) is a **minimal SVG-based prototype** that accepts `List<DiagramNode>` / `List<DiagramEdge>`, supports only two shapes (rectangle, ellipse), renders static lines with a single arrow marker, and exposes `OnNodeClick` / `OnEdgeClick`. There is no layout engine, no zoom/pan, no selection, no editing, no JSON persistence, no child-tag API, and no descriptor binding.

The gap between spec and source is **massive**. The component is spec-ahead on virtually every feature area.

---

## Gap Classification

### SPEC-AHEAD (specified but not implemented)

| # | Feature Area | Spec Reference | Gap Description | Severity |
|---|---|---|---|---|
| S1 | Declarative child-tag API | overview.md | Spec uses `<DiagramShapes>`, `<DiagramShape>`, `<DiagramConnections>`, `<DiagramConnection>`, `<DiagramLayout>`, `<DiagramShapeDefaults>`, `<DiagramConnectionDefaults>`, etc. Source uses flat `List<DiagramNode>` / `List<DiagramEdge>` parameters. | Critical |
| S2 | Shape types (26+ enum) | shapes.md | Spec defines `DiagramShapeType` enum with Circle, Rectangle, Decision, Document, Database, Terminator, etc. Source supports only `"rectangle"` and `"ellipse"` string comparisons. | Critical |
| S3 | Layout engine (Tree, Layered, Force) | layouts.md | Spec defines `DiagramLayoutType` enum with Tree (8 subtypes), Layered (4 subtypes), and Force algorithms. Source has no layout engine; positions are manual only. | Critical |
| S4 | Connection types (Cascading, Polyline) | connections.md | Spec defines cascading (right-angle) and polyline connections with intermediate points. Source draws only straight lines. | High |
| S5 | Connection caps (ArrowEnd, FilledCircle, None) | connections.md | Spec provides start/end cap configuration per connection and globally. Source has a single hardcoded arrow marker. | High |
| S6 | Data binding (ShapesData, ConnectionsData) | data-bind.md | Spec defines `DiagramShapeDescriptor` / `DiagramConnectionDescriptor` binding. Source has no descriptor support. | High |
| S7 | JSON save/load | overview.md | Spec defines `SaveAsJsonAsync()` / `LoadFromJsonAsync()` methods. Source has neither. | High |
| S8 | Zoom / Pan | overview.md | Spec defines `Zoom`, `ZoomRate`, `MinZoom`, `MaxZoom` and `<DiagramPannable>` tag. Source has `Zoomable` / `Pannable` bool parameters but no implementation. | High |
| S9 | Selection (single + marquee) | overview.md | Spec defines `<DiagramSelectable>` with Multiple, Key, and Stroke. Source has no selection. | High |
| S10 | Shape editability (Connect, Drag, Remove) | shapes.md | Spec defines `<DiagramShapeEditable>` / `<DiagramShapeDefaultsEditable>`. Source has no editing. | Medium |
| S11 | Connection editability | connections.md | Spec defines `<DiagramConnectionEditable>` / `<DiagramConnectionDefaultsEditable>`. Source has no editing. | Medium |
| S12 | Connectors (5 hover dots per shape) | shapes.md | Spec describes interactive connector dots for drag-to-connect. Source has none. | Medium |
| S13 | Shape styling (fill, stroke, hover, rotation, corner radius) | shapes.md | Spec provides deep styling hierarchy. Source hardcodes `fill="#e3f2fd" stroke="#1976d2"`. | Medium |
| S14 | Connection styling (stroke, hover, selection handles) | connections.md | Spec provides deep styling hierarchy. Source hardcodes `stroke="#666"`. | Medium |
| S15 | Shape tooltips | shapes.md | Spec defines `<DiagramShapeTooltip>` with Template. Source has none. | Medium |
| S16 | Connection tooltips | connections.md | Spec defines `<DiagramConnectionTooltip>` with Template. Source has none. | Medium |
| S17 | Connection text positioning | connections.md | Spec defines Offset, Horizontal, Vertical positioning for connection labels. Source centers labels between endpoints. | Low |
| S18 | Visual functions (JS) | shapes.md, connections.md | Spec defines `Visual` parameter for custom JS rendering. Source has none. | Low |
| S19 | Layout grid settings (ComponentSpacing, Offset) | layouts.md | Spec defines `<DiagramLayoutGrid>` for multi-component spacing. Source has none. | Low |
| S20 | Image shapes | shapes.md | Spec defines `DiagramShapeType.Image` with `Source` parameter. Source has none. | Medium |
| S21 | Text shapes | shapes.md | Spec defines `DiagramShapeType.Text` (borderless). Source has none. | Low |
| S22 | Custom path shapes | shapes.md | Spec defines `Path` parameter for SVG path-based shapes. Source has none. | Low |
| S23 | Connection content (text labels) per-connection styling | connections.md | Spec defines `<DiagramConnectionContent>` with Color, FontStyle. Source has basic text only. | Low |
| S24 | Shape `DataItem` parameter | shapes.md | Spec defines generic DataItem for visual function context. Source has none. | Low |

### SOURCE-AHEAD (implemented but not in spec)

| # | Feature | Source Location | Gap Description |
|---|---|---|---|
| A1 | `NodeTemplate` RenderFragment | MariloDiagram.razor L117 | Source supports `<NodeTemplate>` for custom node rendering via `foreignObject`. Not mentioned in spec. |
| A2 | `OnNodeClick` / `OnEdgeClick` naming | MariloDiagram.razor L111-114 | Source uses `OnNodeClick(DiagramNode)` / `OnEdgeClick(DiagramEdge)`. Spec uses `OnShapeClick(DiagramShapeClickEventArgs)` / `OnConnectionClick(DiagramConnectionClickEventArgs)`. Different names AND different event arg types. |
| A3 | `DiagramNode` / `DiagramEdge` models | DiagramModels.cs | Source uses simple POCO models. Spec uses complex descriptor classes and child-tag hierarchy. |

### MISMATCHES (both sides exist but disagree)

| # | Feature | Spec Says | Source Does | Impact |
|---|---|---|---|---|
| M1 | Event names | `OnShapeClick` / `OnConnectionClick` | `OnNodeClick` / `OnEdgeClick` | API naming conflict -- must align before public release |
| M2 | Event argument types | `DiagramShapeClickEventArgs` / `DiagramConnectionClickEventArgs` | `DiagramNode` / `DiagramEdge` | Breaking API difference |
| M3 | Data model approach | Declarative child tags + descriptor binding | Flat list parameters | Fundamental architecture disagreement |
| M4 | Default height | `"600px"` (spec overview.md) | `"500px"` (source L102) | Minor default value mismatch |

---

## Overall Assessment

**Spec coverage vs. source: ~5-10%**

The source is a working proof-of-concept that demonstrates basic SVG node-and-edge rendering. The spec describes a full-featured diagramming component with a Kendo-inspired architecture. Closing these gaps requires:

1. **Architecture decision**: Choose between the current flat-list API (source) and the declarative child-tag API (spec), or support both.
2. **Layout engine**: Implement Tree, Layered, and Force layout algorithms (most complex gap).
3. **Shape system**: Implement the full `DiagramShapeType` enum with SVG path definitions for all 26+ shapes.
4. **Interaction layer**: Implement zoom, pan, selection, drag, connect, and remove.
5. **Styling system**: Implement the hierarchical defaults + per-item override pattern.

This is a Phase 1 component that needs significant development to match its spec.
