# Stage 02 — Example UX Gap List: MariloDiagram

**Audit date:** 2026-04-11
**Demo file audited:** `samples/Marilo.Demo/Pages/Components/Diagram/Overview.razor`
**Spec directory:** `docs/component-specs/diagram/`
**Stage 01 output (source of truth for "blocked-by-source" calls):** `stages/01-spec-review/output/diagram-spec-gap-list.md`

---

## Summary

The demo page (`Overview.razor`) contains three scenarios:

1. **Basic Usage** — displays a hardcoded flowchart using `Nodes` + `Edges` + `Height` + `OnNodeClick`.
2. **Shapes** — shows rectangle vs ellipse node types via the `Shape` string property.
3. **Custom Node Template** — demonstrates the `NodeTemplate` RenderFragment.

The spec describes six feature areas: **connections**, **data-bind**, **events**, **layouts**, **overview**, and **shapes**. Almost every spec feature area is either entirely undemonstrated or blocked by missing source implementation.

The demo does not follow the required demo-section format (no parameter table, no spec anchor link, no code snippet panel that matches the current API for spec-defined parameters, no interactive toggles for most scenarios).

---

## Scenario Inventory

### Existing demo scenarios

| # | Title | Source API used | Spec area(s) covered |
|---|-------|-----------------|----------------------|
| D-01 | Basic Usage | `Nodes`, `Edges`, `Height`, `OnNodeClick` | Partial: overview (Height), events (OnNodeClick — wrong name) |
| D-02 | Shapes | `Nodes` with `Shape = "rectangle" / "ellipse"` | Partial: shapes (only 2 of 27 types, no `DiagramShapeType` enum) |
| D-03 | Custom Node Template | `NodeTemplate` RenderFragment | Not in spec (undocumented source feature) |

### Missing scenarios (cross-referenced to spec areas)

Every spec area that is not blocked by source is either missing from the demo or only partially covered. The table below lists all required scenarios from the demo-section completeness criteria and their current status.

---

## Gap Records

### DEMO-diagram-001

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-001 |
| Spec area | overview |
| Scenario | Declarative shape + connection setup (MariloDiagram with DiagramShapes / DiagramConnections child tags) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-015, SPEC-diagram-025 — `<DiagramShape>` and `<DiagramConnection>` child tags not implemented |
| Spec location | `docs/component-specs/diagram/overview.md` — Define Shapes and Connections Declaratively |
| Description | The spec's primary "getting started" pattern uses `<DiagramShapes>` / `<DiagramShape>` / `<DiagramConnections>` / `<DiagramConnection>` child tags. Source uses flat `List<DiagramNode>` / `List<DiagramEdge>` parameters. No demo scenario can demonstrate the declarative pattern until the child-tag architecture is implemented. |
| Suggested resolution | Implement `<DiagramShape>` / `<DiagramConnection>` child tags (per Stage 01). Once available, add a "Declarative Setup" demo section as the first scenario. |

---

### DEMO-diagram-002

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-002 |
| Spec area | overview |
| Scenario | Zoom control (Zoom, ZoomRate, MaxZoom, MinZoom parameters with interactive slider) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-010 — zoom parameters not implemented (only `Zoomable` bool at line 105) |
| Spec location | `docs/component-specs/diagram/overview.md` — Zoom section |
| Description | The spec defines four numeric zoom parameters. The demo should show a slider or numeric input controlling the zoom level in real time. The `Zoomable` bool exists in source but is not demonstrated, and it does not match the spec shape. |
| Suggested resolution | Implement zoom parameters. Add interactive zoom demo with a `MariloSlider` or `MariloNumericTextBox` wired to `Zoom`. |

---

### DEMO-diagram-003

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-003 |
| Spec area | overview |
| Scenario | Pan with key modifier (DiagramPannable child tag with Key toggle) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-011 — `<DiagramPannable>` not implemented |
| Spec location | `docs/component-specs/diagram/overview.md` — Pan section |
| Description | No demo for panning. The `Pannable` bool exists in source but does not match the spec tag API and is not shown in any demo scenario. |
| Suggested resolution | Implement `<DiagramPannable>`. Add a demo explaining Ctrl+drag behavior and the key-override toggle. |

---

### DEMO-diagram-004

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-004 |
| Spec area | overview |
| Scenario | Selection — single and multi-select with marquee (DiagramSelectable) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-012 — `<DiagramSelectable>` not implemented |
| Spec location | `docs/component-specs/diagram/overview.md` — Select section |
| Description | No selection feature in source or demo. |
| Suggested resolution | Implement `<DiagramSelectable>`. Add a demo with enabled/disabled toggle and a multi-select checkbox. |

---

### DEMO-diagram-005

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-005 |
| Spec area | overview |
| Scenario | JSON load/save — LoadFromJsonAsync / SaveAsJsonAsync methods |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-036 — JSON methods not implemented |
| Spec location | `docs/component-specs/diagram/overview.md` — Define Shapes and Connections in JSON section |
| Description | No demo for JSON state persistence. This is one of the three creation patterns in the spec overview. |
| Suggested resolution | Implement JSON methods. Add a "Save and Restore" demo with Save/Load buttons and a JSON display area, similar to the spec example. |

---

### DEMO-diagram-006

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-006 |
| Spec area | layouts |
| Scenario | Tree layout — all subtypes (Down, Up, Left, Right, MindMapHorizontal, MindMapVertical, Radial, TipOver) with interactive subtype selector |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-013 — `<DiagramLayout>` not implemented |
| Spec location | `docs/component-specs/diagram/layouts.md` — Tree Layout section |
| Description | No layout support in source. All node positions must be specified manually. No demo for any built-in layout. |
| Suggested resolution | Implement `<DiagramLayout>`. Add a layouts demo with a `MariloButtonGroup` to switch layout types and subtypes (per the spec example). |

---

### DEMO-diagram-007

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-007 |
| Spec area | layouts |
| Scenario | Layered layout — Direction subtypes (Down/Up/Left/Right) with LayerSeparation |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-013 |
| Spec location | `docs/component-specs/diagram/layouts.md` — Layered Layout section |
| Description | Same blocker as DEMO-diagram-006. |
| Suggested resolution | Covered in the same demo as Tree layout once source is available. |

---

### DEMO-diagram-008

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-008 |
| Spec area | layouts |
| Scenario | Force layout — NodeDistance and Iterations parameters |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-013 |
| Spec location | `docs/component-specs/diagram/layouts.md` — Force Layout section |
| Description | Same blocker as DEMO-diagram-006. |
| Suggested resolution | Include in the layout demo once source is available. |

---

### DEMO-diagram-009

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-009 |
| Spec area | layouts |
| Scenario | Layout grid settings (DiagramLayoutGrid — ComponentSpacingX/Y, OffsetX/Y, Width) with numeric inputs |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-014 — `<DiagramLayoutGrid>` not implemented |
| Spec location | `docs/component-specs/diagram/layouts.md` — Layout Grid Settings section |
| Description | Grid settings for multiple disconnected subgraphs are not implemented. |
| Suggested resolution | Add as a subsection of the layout demo with `MariloNumericTextBox` inputs matching the spec example. |

---

### DEMO-diagram-010

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-010 |
| Spec area | shapes |
| Scenario | All DiagramShapeType enum values rendered (25+ types) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-015 — `DiagramShapeType` enum not implemented; `Shape` property only accepts `"rectangle"` / `"ellipse"` strings |
| Spec location | `docs/component-specs/diagram/shapes.md` — Shape Types section |
| Description | The existing "Shapes" demo (D-02) only shows two hard-coded string values. The spec enum has 27 members. The demo cannot show all spec types until the enum is implemented. |
| Suggested resolution | Implement `DiagramShapeType` enum. Replace D-02 with a grid of all shape types (per the spec's `@foreach` example). |

---

### DEMO-diagram-011

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-011 |
| Spec area | shapes |
| Scenario | Shape connectors — customize connector dots (DiagramShapeConnectorDefaults fill/stroke/hover) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-023 — connector rendering not implemented |
| Spec location | `docs/component-specs/diagram/shapes.md` — Connectors section |
| Description | No connector dots rendered in source. No demo for connector customization. |
| Suggested resolution | Implement connector rendering. Add demo showing connector styling options on hover. |

---

### DEMO-diagram-012

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-012 |
| Spec area | shapes |
| Scenario | Shape tooltips (DiagramShapeTooltip — Visible, Class, Template) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-024 — `<DiagramShapeTooltip>` not implemented |
| Spec location | `docs/component-specs/diagram/shapes.md` — Tooltips section |
| Description | No tooltip support. No demo. |
| Suggested resolution | Implement `<DiagramShapeTooltip>`. Add a demo where hovering shows a tooltip with shape metadata. |

---

### DEMO-diagram-013

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-013 |
| Spec area | shapes |
| Scenario | Shape fill and stroke styling (DiagramShapeFill Color/Opacity, DiagramShapeStroke Color/Width/DashType, CornerRadius) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-018, SPEC-diagram-019 — fill/stroke hard-coded |
| Spec location | `docs/component-specs/diagram/shapes.md` — Styling section |
| Description | No demo for shape color/border customization. Fill is always `#e3f2fd`, stroke always `#1976d2`. |
| Suggested resolution | Implement fill/stroke child tags. Add a styling demo with color pickers or preset palettes. |

---

### DEMO-diagram-014

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-014 |
| Spec area | shapes |
| Scenario | Shape editability — Connect, Drag, Remove toggles (DiagramShapeEditable / DiagramShapeDefaultsEditable) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-021 — editability not implemented |
| Spec location | `docs/component-specs/diagram/shapes.md` — Editability section |
| Description | No interactive shape editing (drag to reposition, Ctrl+drag to connect, Del to remove). No demo. |
| Suggested resolution | Implement editability. Add a demo with checkboxes toggling Connect/Drag/Remove permissions. |

---

### DEMO-diagram-015

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-015 |
| Spec area | shapes |
| Scenario | Shape visual function (JavaScript-based custom rendering) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-035 — `Visual` JS function parameter not implemented |
| Spec location | `docs/component-specs/diagram/shapes.md` — Visual Function section |
| Description | The source provides `NodeTemplate` (Blazor RenderFragment) as an undocumented alternative. Neither the spec `Visual` JS function nor a documented Blazor template approach is demoed with a proper scenario that follows the spec format. |
| Suggested resolution | After deciding whether `NodeTemplate` or JS `Visual` is the canonical approach, create a demo with a clearly labeled scenario (e.g., "Multi-line Shape with Icon") that follows the full demo-section format. |

---

### DEMO-diagram-016

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-016 |
| Spec area | connections |
| Scenario | Connection types — Cascading vs Polyline (DiagramConnectionType enum) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-025 — `<DiagramConnection>` / `Type` parameter not implemented |
| Spec location | `docs/component-specs/diagram/connections.md` — Connection Types section |
| Description | Source only renders straight SVG `<line>` elements. No connection type control. No demo. |
| Suggested resolution | Implement connection types. Add a demo with a toggle between Cascading and Polyline. |

---

### DEMO-diagram-017

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-017 |
| Spec area | connections |
| Scenario | Connection with intermediate points (DiagramConnectionPoints / DiagramConnectionPoint — X, Y) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-027 — connection points not implemented |
| Spec location | `docs/component-specs/diagram/connections.md` — Connection Points section |
| Description | No polyline routing. No demo. |
| Suggested resolution | Implement connection points. Add a demo showing a polyline with manually placed intermediate waypoints. |

---

### DEMO-diagram-018

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-018 |
| Spec area | connections |
| Scenario | Cap types — ArrowEnd, FilledCircle, None per connection end (DiagramConnectionsStartCapType / EndCapType enums) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-028 — cap type enums not implemented |
| Spec location | `docs/component-specs/diagram/connections.md` — Cap Types section |
| Description | Arrow head is hard-coded. No demo for configurable cap types. |
| Suggested resolution | Implement cap type enums. Add a demo with a grid comparing start/end cap combinations. |

---

### DEMO-diagram-019

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-019 |
| Spec area | connections |
| Scenario | Connection text with positioning (DiagramConnectionContent Text/Color/Offset, DiagramConnectionContentPosition Horizontal/Vertical) |
| Status | Partial |
| Source reference | `DiagramEdge.Text` renders a mid-point label (line 41–45 of `MariloDiagram.razor`) but `Color`, `Offset`, and positioning enums are absent |
| Spec location | `docs/component-specs/diagram/connections.md` — Connection Text section |
| Description | The existing Basic Usage demo (D-01) shows `Text = "Yes"` and `Text = "No"` labels on edges, but does not demonstrate positioning, color, or offset — and the code uses the `DiagramEdge` model, not the spec `<DiagramConnectionContent>` tag. No dedicated scenario exists for this feature. |
| Suggested resolution | Once `<DiagramConnectionContent>` is implemented, replace the edge text in D-01 with a dedicated "Connection Labels" scenario that includes position/color/offset controls. |

---

### DEMO-diagram-020

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-020 |
| Spec area | connections |
| Scenario | Connection tooltips (DiagramConnectionTooltip — Visible, Class, Template) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-030 — `<DiagramConnectionTooltip>` not implemented |
| Spec location | `docs/component-specs/diagram/connections.md` — Tooltips section |
| Description | No connection tooltip support. No demo. |
| Suggested resolution | Implement alongside shape tooltip. Add a demo showing a tooltip on hover. |

---

### DEMO-diagram-021

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-021 |
| Spec area | connections |
| Scenario | Connection selection handles (DiagramConnectionSelectionHandles — Height, Width, Fill, Stroke) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-031 — selection handles not implemented |
| Spec location | `docs/component-specs/diagram/connections.md` — Selection Handles section |
| Description | No visual selection handle rendering. No demo. |
| Suggested resolution | Implement after selection. Add to the selection demo. |

---

### DEMO-diagram-022

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-022 |
| Spec area | connections |
| Scenario | Connection editability — Drag, Remove toggles (DiagramConnectionEditable / DiagramConnectionDefaultsEditable) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-032 — connection editability not implemented |
| Spec location | `docs/component-specs/diagram/connections.md` — Editability section |
| Description | No interactive connection editing (drag to re-route, Del to remove). No demo. |
| Suggested resolution | Implement editability. Combine with shape editability demo (DEMO-diagram-014). |

---

### DEMO-diagram-023

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-023 |
| Spec area | connections |
| Scenario | Connection stroke styling (DiagramConnectionStroke Color/Width/DashType; DiagramConnectionHover stroke) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-033 — connection stroke hard-coded |
| Spec location | `docs/component-specs/diagram/connections.md` — Styling section |
| Description | No connection stroke customization. No demo. |
| Suggested resolution | Implement `<DiagramConnectionStroke>`. Add a styling demo. |

---

### DEMO-diagram-024

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-024 |
| Spec area | connections |
| Scenario | Connection visual function (JavaScript-based custom rendering for connections) |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-034 — `Visual` parameter for connection content not implemented |
| Spec location | `docs/component-specs/diagram/connections.md` — Visual Function section |
| Description | No JS visual function support for connections. No demo. |
| Suggested resolution | Implement after standard connection API is stable. |

---

### DEMO-diagram-025

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-025 |
| Spec area | data-bind |
| Scenario | Data binding via ShapesData / ConnectionsData (DiagramShapeDescriptor / DiagramConnectionDescriptor) — mapping from custom models |
| Status | Blocked-by-source |
| Source reference | SPEC-diagram-037 — descriptor classes not implemented |
| Spec location | `docs/component-specs/diagram/data-bind.md` |
| Description | No descriptor-based data binding. The source `Nodes`/`Edges` approach is structurally similar but uses different types and parameter names. |
| Suggested resolution | Implement descriptor classes. Replace or alias `Nodes`/`Edges`. Add a "Data Binding" demo section with a realistic model mapping example. |

---

### DEMO-diagram-026

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-026 |
| Spec area | events |
| Scenario | OnShapeClick — display clicked shape details |
| Status | Partial |
| Source reference | `OnNodeClick` event exists at line 111 of `MariloDiagram.razor`; event name and argument type do not match spec (`OnShapeClick` / `DiagramShapeClickEventArgs`) |
| Spec location | `docs/component-specs/diagram/events.md` — OnShapeClick section |
| Description | The Basic Usage demo (D-01) does show `OnNodeClick` in action and displays node details below the diagram. However, the event name, argument type, and demo section format (missing parameter table, spec link) do not meet spec requirements. |
| Suggested resolution | Rename `OnNodeClick` → `OnShapeClick`, update argument type, create a dedicated "Events" demo section with both `OnShapeClick` and `OnConnectionClick`, plus a visible event log area. |

---

### DEMO-diagram-027

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-027 |
| Spec area | events |
| Scenario | OnConnectionClick — display clicked connection details including coordinate-based connections |
| Status | Missing |
| Source reference | `OnEdgeClick` event exists in source (line 114) but is not used in any demo scenario |
| Spec location | `docs/component-specs/diagram/events.md` — OnConnectionClick section |
| Description | The `OnEdgeClick` event is declared in source but never demonstrated. No demo scenario shows clicking on a connection or displays connection event args. |
| Suggested resolution | Add to the Events demo section (DEMO-diagram-026 above). Show both shape-linked and coordinate-linked connections being clicked. |

---

### DEMO-diagram-028

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-028 |
| Spec area | all |
| Scenario | Disabled state demonstration |
| Status | Missing |
| Source reference | No disabled parameter exists in source or spec |
| Spec location | N/A — required by demo-section completeness criteria |
| Description | The demo format requires a "Disabled state" scenario for every component. The spec and source do not define a `Disabled` parameter. |
| Suggested resolution | Clarify with spec/product team whether `Disabled` applies to `MariloDiagram`. If yes, add parameter to spec and source. If no, document the decision and exclude from demo checklist. |

---

### DEMO-diagram-029

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-029 |
| Spec area | all |
| Scenario | Empty/no-data state demonstration |
| Status | Missing |
| Source reference | Source renders an empty SVG `<svg>` canvas when `Nodes` is empty — no special empty-state UI |
| Spec location | Required by demo-section completeness criteria |
| Description | No demo scenario shows the component with an empty node list and explains the empty-state behavior. |
| Suggested resolution | Add an "Empty Diagram" scenario that shows the blank canvas and describes when developers would encounter this state (e.g., before data loads). |

---

### DEMO-diagram-030

| Field | Value |
|-------|-------|
| ID | DEMO-diagram-030 |
| Spec area | all |
| Scenario | Format compliance — parameter tables, spec anchor links, interactive controls, code snippet panels |
| Status | Missing |
| Source reference | N/A — all three existing demo sections (D-01, D-02, D-03) |
| Spec location | `stages/02-example-ux/shared/demo-scenario-format.md` |
| Description | None of the three existing demo sections includes: (a) a parameter table listing active parameters, (b) a link/anchor to the corresponding spec section, or (c) a user-controllable input that changes behavior in real time. The code snippets use the undocumented `Nodes`/`Edges` API rather than the spec-defined parameter names. |
| Suggested resolution | After source API is aligned to spec, rewrite all three demo sections to comply with the demo-section format: add parameter tables, spec anchor links, and interactive controls for each scenario. |

---

## Scenario Coverage Summary

| Spec area | Scenarios required | Covered (fully) | Partial | Missing | Blocked-by-source |
|---|---|---|---|---|---|
| overview | 5 | 0 | 1 (Height/Width) | 0 | 4 (declarative, zoom, pan, JSON) |
| layouts | 4 | 0 | 0 | 0 | 4 |
| shapes | 6 | 0 | 0 | 1 (empty state) | 5 |
| connections | 9 | 0 | 1 (edge text) | 1 (empty state) | 7 |
| data-bind | 1 | 0 | 0 | 0 | 1 |
| events | 2 | 0 | 1 (OnNodeClick) | 1 (OnEdgeClick unused) | 0 |
| cross-cutting | 3 | 0 | 0 | 3 (disabled, empty, format) | 0 |
| **Total** | **30** | **0** | **3** | **6** | **21** |

---

## Recommended Demo Page Structure (post-source alignment)

Once the source is brought into alignment with the spec (see Stage 01 gap list), the demo page should be reorganized as follows:

```
Overview.razor
  PageSection: "Overview"
    DemoSection: "Declarative Setup"           ← replaces current Basic Usage
    DemoSection: "Data Binding"               ← new (ShapesData / ConnectionsData)
    DemoSection: "JSON Load and Save"         ← new (LoadFromJsonAsync / SaveAsJsonAsync)
  PageSection: "Layouts"
    DemoSection: "Tree Layout"
    DemoSection: "Layered Layout"
    DemoSection: "Force Layout"
    DemoSection: "Layout Grid (Multi-subgraph)"
  PageSection: "Shapes"
    DemoSection: "Shape Types"                ← replace current "Shapes" section
    DemoSection: "Shape Styling"              ← fill, stroke, corner radius
    DemoSection: "Shape Connectors"
    DemoSection: "Shape Tooltips"
    DemoSection: "Shape Editability"
    DemoSection: "Custom Shape Visual Function"
  PageSection: "Connections"
    DemoSection: "Connection Types"           ← Cascading vs Polyline
    DemoSection: "Connection Labels"          ← replaces edge text in Basic Usage
    DemoSection: "Cap Types"
    DemoSection: "Connection Styling"
    DemoSection: "Connection Tooltips"
    DemoSection: "Connection Editability"
    DemoSection: "Connection Visual Function"
  PageSection: "Interactivity"
    DemoSection: "Zoom"
    DemoSection: "Pan"
    DemoSection: "Selection"
    DemoSection: "Events"                     ← OnShapeClick + OnConnectionClick
  PageSection: "States"
    DemoSection: "Empty Diagram"
```
