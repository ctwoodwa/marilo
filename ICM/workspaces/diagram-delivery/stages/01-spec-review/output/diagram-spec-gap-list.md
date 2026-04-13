# Stage 01 — Spec Review: MariloDiagram

**Audit date:** 2026-04-11
**Source file:** `src/Marilo.Components/DataDisplay/MariloDiagram.razor`
**Model file:** `src/Marilo.Core/Models/DiagramModels.cs`
**Spec directory:** `docs/component-specs/diagram/`

---

## Summary

The component source (`MariloDiagram.razor`) is an early-prototype SVG renderer whose API is completely different from the spec. The spec describes a rich Blazor component library modeled after the Telerik/Kendo Diagram (declarative child-tag architecture, built-in layouts, connection types, cap types, data binding via descriptor classes, event system, etc.). The source implements a minimal home-grown node/edge renderer using flat `List<DiagramNode>` / `List<DiagramEdge>` parameters.

**No spec parameter, event, or child-tag is implemented in source.** Every spec item is therefore classified as **spec-ahead**. Every source parameter is classified as **undocumented**.

---

## Category 1: Undocumented (in source but not in spec)

### SPEC-diagram-001

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-001 |
| Feature area | data-bind |
| Parameter/event | `Nodes` (`List<DiagramNode>`) |
| Gap type | undocumented |
| Source location | `src/Marilo.Components/DataDisplay/MariloDiagram.razor:93` |
| Spec location | missing |
| Description | Source accepts a flat `List<DiagramNode>` parameter. Spec describes data binding via `ShapesData` (`List<DiagramShapeDescriptor>`). The types and parameter names are completely different. |
| Priority | P1 |
| Priority rationale | Primary data-entry API; every consumer must use it. |
| Suggested resolution | Replace with `ShapesData`/`DiagramShapeDescriptor` per spec, or add spec section documenting the current `Nodes` approach and migration path. |

---

### SPEC-diagram-002

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-002 |
| Feature area | data-bind |
| Parameter/event | `Edges` (`List<DiagramEdge>`) |
| Gap type | undocumented |
| Source location | `src/Marilo.Components/DataDisplay/MariloDiagram.razor:96` |
| Spec location | missing |
| Description | Source uses `List<DiagramEdge>` with a `Text` label field. Spec defines `ConnectionsData` (`List<DiagramConnectionDescriptor>`). Parameter name and type mismatch. |
| Priority | P1 |
| Priority rationale | Required to define connections; every diagram needs it. |
| Suggested resolution | Align with spec `ConnectionsData`/`DiagramConnectionDescriptor` or document `Edges` as the current interim API. |

---

### SPEC-diagram-003

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-003 |
| Feature area | overview |
| Parameter/event | `Zoomable` (bool) |
| Gap type | undocumented |
| Source location | `src/Marilo.Components/DataDisplay/MariloDiagram.razor:105` |
| Spec location | missing |
| Description | Source exposes a simple boolean `Zoomable` toggle. The spec describes four distinct zoom parameters (`Zoom`, `ZoomRate`, `MaxZoom`, `MinZoom`) on `MariloDiagram` with default values. The spec API is a superset of the source API. |
| Priority | P2 |
| Priority rationale | Feature exists in both but with different shapes; confusing to developers comparing source to docs. |
| Suggested resolution | Expand source to match spec's zoom parameters, or add a spec note about the current boolean shortcut. |

---

### SPEC-diagram-004

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-004 |
| Feature area | overview |
| Parameter/event | `Pannable` (bool) |
| Gap type | undocumented |
| Source location | `src/Marilo.Components/DataDisplay/MariloDiagram.razor:108` |
| Spec location | missing |
| Description | Source has a flat `Pannable` boolean. Spec defines `<DiagramPannable Enabled="..." Key="..." />` child tag with key binding support. |
| Priority | P2 |
| Priority rationale | Feature exists in both but shapes differ; the key-binding option is entirely missing from source. |
| Suggested resolution | Implement `<DiagramPannable>` child tag per spec, or document current boolean shortcut. |

---

### SPEC-diagram-005

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-005 |
| Feature area | events |
| Parameter/event | `OnNodeClick` (`EventCallback<DiagramNode>`) |
| Gap type | mismatch |
| Source location | `src/Marilo.Components/DataDisplay/MariloDiagram.razor:111` |
| Spec location | `docs/component-specs/diagram/events.md` — `OnShapeClick` section |
| Description | Source event is `OnNodeClick` and delivers a `DiagramNode` object. Spec event is `OnShapeClick` and delivers `DiagramShapeClickEventArgs` with an `Id` property. Name mismatch (`Node` vs `Shape`) and argument type mismatch. |
| Priority | P1 |
| Priority rationale | Public event API; any consumer code referencing `OnNodeClick` will break when aligned to spec. |
| Suggested resolution | Rename to `OnShapeClick`, change argument type to `DiagramShapeClickEventArgs`. |

---

### SPEC-diagram-006

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-006 |
| Feature area | events |
| Parameter/event | `OnEdgeClick` (`EventCallback<DiagramEdge>`) |
| Gap type | mismatch |
| Source location | `src/Marilo.Components/DataDisplay/MariloDiagram.razor:114` |
| Spec location | `docs/component-specs/diagram/events.md` — `OnConnectionClick` section |
| Description | Source event is `OnEdgeClick` delivering `DiagramEdge`. Spec event is `OnConnectionClick` delivering `DiagramConnectionClickEventArgs` with `FromId`, `ToId`, `FromX`, `FromY`, `ToX`, `ToY`. Name mismatch (`Edge` vs `Connection`) and argument type mismatch. |
| Priority | P1 |
| Priority rationale | Public event API; same breaking-change risk as `OnNodeClick`. |
| Suggested resolution | Rename to `OnConnectionClick`, change argument type to `DiagramConnectionClickEventArgs`. |

---

### SPEC-diagram-007

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-007 |
| Feature area | shapes |
| Parameter/event | `NodeTemplate` (`RenderFragment<DiagramNode>`) |
| Gap type | undocumented |
| Source location | `src/Marilo.Components/DataDisplay/MariloDiagram.razor:117` |
| Spec location | missing |
| Description | Source provides a `NodeTemplate` RenderFragment for custom node rendering. The spec handles this use case through the Shape visual function (`Visual` parameter of `<DiagramShapeDefaults>` / `<DiagramShape>`) which calls a JavaScript function. No equivalent Blazor template parameter is described in the spec. |
| Priority | P2 |
| Priority rationale | Useful API that exists in source with no spec coverage; developer would not know to use it. |
| Suggested resolution | Either add a spec section for `NodeTemplate` as a Blazor-idiomatic alternative to the JS visual function, or replace with the spec-defined `Visual` JavaScript approach. |

---

### SPEC-diagram-008

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-008 |
| Feature area | data-bind |
| Parameter/event | `DiagramNode` model class |
| Gap type | undocumented |
| Source location | `src/Marilo.Core/Models/DiagramModels.cs:6` |
| Spec location | missing |
| Description | `DiagramNode` (with properties `Id`, `Text`, `X`, `Y`, `Width`, `Height`, `Shape`) has no spec equivalent. Spec uses `DiagramShapeDescriptor` for data binding and declarative `<DiagramShape>` for markup. The `Shape` property is a raw string (`"rectangle"`, `"ellipse"`) instead of the `DiagramShapeType` enum. |
| Priority | P1 |
| Priority rationale | Public model class; changing it is a breaking API change. |
| Suggested resolution | Replace or alias to `DiagramShapeDescriptor`, add `DiagramShapeType` enum. |

---

### SPEC-diagram-009

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-009 |
| Feature area | data-bind |
| Parameter/event | `DiagramEdge` model class |
| Gap type | undocumented |
| Source location | `src/Marilo.Core/Models/DiagramModels.cs:33` |
| Spec location | missing |
| Description | `DiagramEdge` (with properties `Id`, `FromId`, `ToId`, `Text`) has no spec equivalent. Spec uses `DiagramConnectionDescriptor` for data binding and declarative `<DiagramConnection>` for markup. |
| Priority | P1 |
| Priority rationale | Public model class; same breaking-change risk as `DiagramNode`. |
| Suggested resolution | Replace or alias to `DiagramConnectionDescriptor`. |

---

## Category 2: Spec-ahead (in spec but not in source)

The following entries are a representative but non-exhaustive enumeration of the major spec features absent from source. Because the source is a prototype that predates the spec, every spec-described feature is effectively spec-ahead. Items are grouped by spec area.

---

### SPEC-diagram-010

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-010 |
| Feature area | overview |
| Parameter/event | `Zoom` / `ZoomRate` / `MaxZoom` / `MinZoom` parameters |
| Gap type | spec-ahead |
| Source location | not implemented (only `Zoomable` bool at line 105) |
| Spec location | `docs/component-specs/diagram/overview.md` — Zoom section |
| Description | Spec defines four distinct zoom parameters with defaults and semantics. Source only has a boolean toggle with no zoom-level control. |
| Priority | P1 |
| Priority rationale | Core interactivity feature; blocks any demo using zoom-level control. |
| Suggested resolution | Implement zoom parameters per spec. |

---

### SPEC-diagram-011

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-011 |
| Feature area | overview |
| Parameter/event | `<DiagramPannable>` child tag with `Key` (DiagramPannableKey enum) |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/overview.md` — Pan section |
| Description | Spec requires a child tag with `Enabled` and `Key` parameters. Source only has `Pannable` bool with no key-binding support. |
| Priority | P2 |
| Priority rationale | Key binding needed to avoid conflicts with selection when both features are active. |
| Suggested resolution | Implement `<DiagramPannable>` child tag. |

---

### SPEC-diagram-012

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-012 |
| Feature area | overview |
| Parameter/event | `<DiagramSelectable>` child tag with `Multiple`, `Key`, `<DiagramSelectableStroke>` |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/overview.md` — Select section |
| Description | Spec defines a child tag for enabling selection mode with multi-select and marquee configuration. No selection feature exists in source. |
| Priority | P1 |
| Priority rationale | Selection is required for delete and drag operations; all `DiagramConnectionEditable` / `DiagramShapeEditable` features depend on it. |
| Suggested resolution | Implement `<DiagramSelectable>` child tag. |

---

### SPEC-diagram-013

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-013 |
| Feature area | layouts |
| Parameter/event | `<DiagramLayout>` child tag — `Type` (DiagramLayoutType), `Subtype` (DiagramLayoutSubtype), `HorizontalSeparation`, `VerticalSeparation`, radial/TipOver parameters |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/layouts.md` |
| Description | The entire built-in layout system (Tree, Layered, Force) is absent from source. Nodes must be positioned manually via X/Y. |
| Priority | P1 |
| Priority rationale | Automatic layout is a headline feature; most demos use it. Blocks nearly all spec demo scenarios. |
| Suggested resolution | Implement `<DiagramLayout>` child tag with at minimum Tree layout. |

---

### SPEC-diagram-014

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-014 |
| Feature area | layouts |
| Parameter/event | `<DiagramLayoutGrid>` child tag — `ComponentSpacingX`, `ComponentSpacingY`, `OffsetX`, `OffsetY`, `Width` |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/layouts.md` — Layout Grid Settings section |
| Description | Multi-subgraph (component) grid layout settings are not implemented. |
| Priority | P2 |
| Priority rationale | Required for diagrams with disconnected subgraphs. |
| Suggested resolution | Implement after core layout support. |

---

### SPEC-diagram-015

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-015 |
| Feature area | shapes |
| Parameter/event | `<DiagramShape>` declarative child tag — `Id`, `Type` (DiagramShapeType enum), `Width`, `Height`, `X`, `Y`, `Path`, `Source`, `DataItem`, `CornerRadius`, `Selectable`, `Visual`, `MinHeight`, `MinWidth` |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/shapes.md` — Basics and Shape Types sections |
| Description | Source uses `DiagramNode` model objects. Spec defines a declarative `<DiagramShape>` tag and `DiagramShapeType` enum with 27 named types. The source `Shape` property accepts only raw strings `"rectangle"` and `"ellipse"`. |
| Priority | P1 |
| Priority rationale | Core API building block; all shape-specific customization depends on this tag. |
| Suggested resolution | Implement `<DiagramShape>` Razor component and `DiagramShapeType` enum. |

---

### SPEC-diagram-016

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-016 |
| Feature area | shapes |
| Parameter/event | `<DiagramShapeDefaults>` tag — `Type`, `Width`, `Height`, `CornerRadius`, `Selectable`, `Visual` |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/shapes.md` — Basics section |
| Description | No global shape defaults child tag exists in source. |
| Priority | P1 |
| Priority rationale | Required to set the default shape type used in overview spec examples. |
| Suggested resolution | Implement `<DiagramShapeDefaults>` component. |

---

### SPEC-diagram-017

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-017 |
| Feature area | shapes |
| Parameter/event | `<DiagramShapeContent>` — `Text`, `Color`, `FontSize`, `FontFamily`, `FontStyle`, `FontWeight`, `TextWrap` (DiagramShapesContentTextWrap), `RelativePadding` |
| Gap type | spec-ahead |
| Source location | not implemented (text rendering hard-coded in razor template) |
| Spec location | `docs/component-specs/diagram/shapes.md` — Basics section |
| Description | Shape text is rendered as an SVG `<text>` element using the `DiagramNode.Text` string. No child-tag control over font properties, color, or wrapping. |
| Priority | P1 |
| Priority rationale | Every shape label uses content styling. |
| Suggested resolution | Implement `<DiagramShapeContent>` child tag. |

---

### SPEC-diagram-018

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-018 |
| Feature area | shapes |
| Parameter/event | `<DiagramShapeFill>` / `<DiagramShapeDefaultsFill>` — `Color`, `Opacity` |
| Gap type | spec-ahead |
| Source location | not implemented (fill hard-coded as `#e3f2fd`) |
| Spec location | `docs/component-specs/diagram/shapes.md` — Styling section |
| Description | Shape fill color is hard-coded in the SVG template. No parameter allows overriding it. |
| Priority | P1 |
| Priority rationale | Fill customization is demonstrated in every shapes spec example. |
| Suggested resolution | Implement `<DiagramShapeFill>` child tag. |

---

### SPEC-diagram-019

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-019 |
| Feature area | shapes |
| Parameter/event | `<DiagramShapeStroke>` / `<DiagramShapeDefaultsStroke>` — `Color`, `Width`, `DashType` (DashType enum) |
| Gap type | spec-ahead |
| Source location | not implemented (stroke hard-coded) |
| Spec location | `docs/component-specs/diagram/shapes.md` — Styling section |
| Description | Shape stroke is hard-coded blue (#1976d2, width 2). No parameter for customization. |
| Priority | P2 |
| Priority rationale | Visual customization; not required for functional correctness. |
| Suggested resolution | Implement `<DiagramShapeStroke>` child tag. |

---

### SPEC-diagram-020

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-020 |
| Feature area | shapes |
| Parameter/event | `<DiagramShapeHover>` / `<DiagramShapeDefaultsHover>` |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/shapes.md` — Styling section |
| Description | No hover state customization exists. |
| Priority | P3 |
| Priority rationale | Visual polish; not blocking. |
| Suggested resolution | Implement after fill/stroke. |

---

### SPEC-diagram-021

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-021 |
| Feature area | shapes |
| Parameter/event | `<DiagramShapeEditable>` / `<DiagramShapeDefaultsEditable>` — `Connect`, `Drag`, `Remove` |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/shapes.md` — Editability section |
| Description | No interactive editing (connect, drag, delete) for shapes is implemented. |
| Priority | P1 |
| Priority rationale | Interactivity is a core diagram feature; required for runtime use. |
| Suggested resolution | Implement editability after selection. |

---

### SPEC-diagram-022

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-022 |
| Feature area | shapes |
| Parameter/event | `<DiagramShapeRotation>` / `<DiagramShapeDefaultsRotation>` — `Angle` |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/shapes.md` — Styling section (example at line 389) |
| Description | Shape rotation is not supported. |
| Priority | P3 |
| Priority rationale | Uncommon; not required for standard diagrams. |
| Suggested resolution | Low-priority implementation. |

---

### SPEC-diagram-023

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-023 |
| Feature area | shapes |
| Parameter/event | `<DiagramShapeConnectorDefaults>` / `<DiagramShapeDefaultsConnectorDefaults>` — `Width`, `Height`, fill/stroke/hover |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/shapes.md` — Connectors section |
| Description | Connector dots (the 5 drag handles on shape boundaries) are entirely absent from source. |
| Priority | P1 |
| Priority rationale | Required for interactive connection creation at runtime. |
| Suggested resolution | Implement connector rendering and interaction as part of editability work. |

---

### SPEC-diagram-024

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-024 |
| Feature area | shapes |
| Parameter/event | `<DiagramShapeTooltip>` — `Visible`, `Class`, `Template` |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/shapes.md` — Tooltips section |
| Description | No tooltip support for shapes. |
| Priority | P2 |
| Priority rationale | Useful for dense diagrams; not blocking core flow. |
| Suggested resolution | Implement after core shape API. |

---

### SPEC-diagram-025

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-025 |
| Feature area | connections |
| Parameter/event | `<DiagramConnection>` declarative tag — `FromId`, `ToId`, `Type` (DiagramConnectionType), `Selectable`, `DataItem`, and nested `<DiagramConnectionFrom>` / `<DiagramConnectionTo>` (X/Y) |
| Gap type | spec-ahead |
| Source location | not implemented (source uses `DiagramEdge` model objects) |
| Spec location | `docs/component-specs/diagram/connections.md` — Basics section |
| Description | No declarative connection child tag exists. `DiagramEdge` provides only `FromId`/`ToId`/`Text`. Connection type (Cascading vs Polyline), coordinate-based connections, and DataItem are all missing. |
| Priority | P1 |
| Priority rationale | Core API component. |
| Suggested resolution | Implement `<DiagramConnection>` Razor component. |

---

### SPEC-diagram-026

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-026 |
| Feature area | connections |
| Parameter/event | `<DiagramConnectionDefaults>` — `Type`, `Selectable` and all nested defaults |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/connections.md` — Connection Types section |
| Description | No connection defaults child tag exists. All connection styling defaults are hard-coded (`stroke="#666"`, `stroke-width="2"`, arrow marker). |
| Priority | P1 |
| Priority rationale | Required for applying consistent connection styling. |
| Suggested resolution | Implement `<DiagramConnectionDefaults>`. |

---

### SPEC-diagram-027

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-027 |
| Feature area | connections |
| Parameter/event | `<DiagramConnectionPoints>` / `<DiagramConnectionPoint>` — X, Y (polyline routing) |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/connections.md` — Connection Points section |
| Description | Connections are always rendered as straight SVG `<line>` elements. No polyline routing with intermediate points. |
| Priority | P2 |
| Priority rationale | Needed for non-trivial diagrams with overlapping connections. |
| Suggested resolution | Implement after connection type support. |

---

### SPEC-diagram-028

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-028 |
| Feature area | connections |
| Parameter/event | Cap types — `<DiagramConnectionStartCap>` / `<DiagramConnectionEndCap>` / defaults variants; `DiagramConnectionsStartCapType` / `DiagramConnectionsEndCapType` enums |
| Gap type | spec-ahead |
| Source location | not implemented (arrow head hard-coded via SVG `<marker>`) |
| Spec location | `docs/component-specs/diagram/connections.md` — Cap Types section |
| Description | The hard-coded SVG marker only provides one arrow cap style. The spec defines three cap types (`ArrowEnd`, `FilledCircle`, `None`) each configurable per connection end. |
| Priority | P2 |
| Priority rationale | Affects directed vs undirected diagram semantics. |
| Suggested resolution | Implement cap type enum and configurable rendering. |

---

### SPEC-diagram-029

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-029 |
| Feature area | connections |
| Parameter/event | `<DiagramConnectionContent>` — `Text`, `Color`, `Offset`, `FontStyle`; `<DiagramConnectionContentPosition>` — `Horizontal`, `Vertical` |
| Gap type | spec-ahead |
| Source location | partial — `DiagramEdge.Text` renders a label at line midpoint (line 41–45) |
| Spec location | `docs/component-specs/diagram/connections.md` — Connection Text section |
| Description | Source renders edge text at the line midpoint as a plain SVG text element. Spec provides configurable color, offset, horizontal/vertical positioning enums. The positioning API is entirely absent from source. |
| Priority | P2 |
| Priority rationale | Text exists but positioning control does not. |
| Suggested resolution | Implement `<DiagramConnectionContent>` with positioning child tag. |

---

### SPEC-diagram-030

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-030 |
| Feature area | connections |
| Parameter/event | `<DiagramConnectionTooltip>` — `Visible`, `Class`, `Template` |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/connections.md` — Tooltips section |
| Description | No tooltip support for connections. |
| Priority | P2 |
| Priority rationale | Same tier as shape tooltip. |
| Suggested resolution | Implement alongside shape tooltip. |

---

### SPEC-diagram-031

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-031 |
| Feature area | connections |
| Parameter/event | Selection handles — `<DiagramConnectionSelection>` / `<DiagramConnectionDefaultsSelection>` / `<DiagramConnectionSelectionHandles>` fill/stroke |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/connections.md` — Selection Handles section |
| Description | No visual selection handle rendering for connections. |
| Priority | P2 |
| Priority rationale | Required once selection feature is implemented. |
| Suggested resolution | Implement as part of selection work. |

---

### SPEC-diagram-032

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-032 |
| Feature area | connections |
| Parameter/event | `<DiagramConnectionEditable>` / `<DiagramConnectionDefaultsEditable>` — `Drag`, `Remove`, `Enabled` |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/connections.md` — Editability section |
| Description | Connection drag/remove interactivity is not implemented. |
| Priority | P1 |
| Priority rationale | Part of the interactive editing feature set. |
| Suggested resolution | Implement as part of editability work. |

---

### SPEC-diagram-033

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-033 |
| Feature area | connections |
| Parameter/event | `<DiagramConnectionStroke>` / hover stroke — `Color`, `Width`, `DashType` (DashType enum) |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/connections.md` — Styling section |
| Description | Connection stroke is hard-coded (`stroke="#666"`, `stroke-width="2"`). No per-connection stroke styling. |
| Priority | P2 |
| Priority rationale | Visual customization. |
| Suggested resolution | Implement `<DiagramConnectionStroke>`. |

---

### SPEC-diagram-034

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-034 |
| Feature area | connections |
| Parameter/event | Connection `Visual` function parameter (`<DiagramConnectionDefaultsContent Visual="...">`) |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/connections.md` — Visual Function section |
| Description | JavaScript-based visual function for connection content is not implemented. |
| Priority | P3 |
| Priority rationale | Advanced/rare scenario. |
| Suggested resolution | Implement after standard connection API is stable. |

---

### SPEC-diagram-035

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-035 |
| Feature area | shapes |
| Parameter/event | Shape `Visual` function parameter (`<DiagramShapeDefaults Visual="...">` / `<DiagramShape Visual="...">`) |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/shapes.md` — Visual Function section |
| Description | JavaScript-based visual function for shape rendering is not implemented. The source provides a Blazor `NodeTemplate` RenderFragment instead, which is not documented in the spec. |
| Priority | P3 |
| Priority rationale | Advanced/rare scenario. |
| Suggested resolution | Implement after standard shape API is stable; decide whether `NodeTemplate` should coexist or be replaced. |

---

### SPEC-diagram-036

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-036 |
| Feature area | overview |
| Parameter/event | `LoadFromJsonAsync(string json)` / `SaveAsJsonAsync()` methods |
| Gap type | spec-ahead |
| Source location | not implemented (no `@ref`-accessible methods) |
| Spec location | `docs/component-specs/diagram/overview.md` — Define Shapes and Connections in JSON section |
| Description | JSON import/export methods are not available on the component instance. |
| Priority | P1 |
| Priority rationale | Required for persistence scenarios and the JSON-based initialization pattern. |
| Suggested resolution | Implement JS interop-backed `LoadFromJsonAsync` / `SaveAsJsonAsync` methods. |

---

### SPEC-diagram-037

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-037 |
| Feature area | data-bind |
| Parameter/event | `ShapesData` (`List<DiagramShapeDescriptor>`) / `ConnectionsData` (`List<DiagramConnectionDescriptor>`) |
| Gap type | spec-ahead |
| Source location | not implemented (source uses `Nodes`/`Edges` instead) |
| Spec location | `docs/component-specs/diagram/data-bind.md` |
| Description | The spec data-binding API uses `DiagramShapeDescriptor` and `DiagramConnectionDescriptor` with nested descriptor classes for fill, stroke, content. None of these exist in source. |
| Priority | P1 |
| Priority rationale | The spec data-binding approach is the alternative to declarative child tags; both are primary APIs. |
| Suggested resolution | Implement descriptor classes and replace/alias `Nodes`/`Edges` parameters. |

---

### SPEC-diagram-038

| Field | Value |
|-------|-------|
| ID | SPEC-diagram-038 |
| Feature area | connections |
| Parameter/event | `FromConnector` (`DiagramConnectionsFromConnector` enum) on `<DiagramConnection>` |
| Gap type | spec-ahead |
| Source location | not implemented |
| Spec location | `docs/component-specs/diagram/connections.md` — Example section (line 355) |
| Description | The connector-side anchoring parameter for connections is not implemented. |
| Priority | P2 |
| Priority rationale | Required for specific connector-to-connector routing. |
| Suggested resolution | Implement as part of `<DiagramConnection>` child tag work. |

---

## Category 3: Mismatches (both exist but differ)

The mismatches above are covered inline in Category 1 (SPEC-diagram-005, SPEC-diagram-006, SPEC-diagram-003, SPEC-diagram-004) since the source and spec both address the same feature but with different types or names. SPEC-diagram-029 is also a partial mismatch (text label exists in both, but positioning is absent).

---

## Gap Counts

| Category | Count |
|----------|-------|
| Undocumented (source not in spec) | 9 (001–009) |
| Spec-ahead (spec not in source) | 29 (010–038) |
| Mismatch (both exist, differ) | 5 (003, 004, 005, 006, 029 — cross-listed) |
| **Total distinct records** | **38** |

---

## Delivery Context Update

| Feature Area | Status |
|---|---|
| connections | COMPLETE |
| data-bind | COMPLETE |
| events | COMPLETE |
| layouts | COMPLETE |
| overview | COMPLETE |
| shapes | COMPLETE |
