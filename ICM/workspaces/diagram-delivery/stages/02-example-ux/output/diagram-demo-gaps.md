# MariloDiagram -- Example UX Audit (Stage 02)

**Date:** 2026-04-12
**Worker:** w-diagram-delivery
**Demo page:** `samples/Marilo.Demo/Pages/Components/Diagram/Overview.razor`

---

## Current Demo Coverage

The demo page (`Overview.razor`) contains **3 sections**:

| # | Section | What It Shows | Spec Area Covered |
|---|---|---|---|
| 1 | Basic Usage | 5-node flowchart with `OnNodeClick` | Flat-list API, basic rendering |
| 2 | Shapes | Rectangle + ellipse shapes | 2 of 26+ shape types |
| 3 | Custom Node Template | `NodeTemplate` RenderFragment | Source-ahead feature (not in spec) |

**Note:** The demo uses the source API (`Nodes`, `Edges`, `OnNodeClick`), NOT the spec API (`<DiagramShapes>`, `<DiagramShape>`, `OnShapeClick`). This is consistent with the current source but diverges from the spec.

---

## Missing Demo Scenarios

### Critical (features prominent in spec, no demo coverage)

| # | Scenario | Spec Reference | Notes |
|---|---|---|---|
| D1 | Declarative child-tag usage | overview.md | Spec's primary usage pattern is not demonstrated at all |
| D2 | Layout types (Tree, Layered, Force) | layouts.md | Layout is the most visual differentiator; no demo |
| D3 | Layout subtypes (Down, Up, Left, Right, MindMap, Radial, TipOver) | layouts.md | 8 tree subtypes + 4 layered subtypes = 0 demos |
| D4 | Data binding with descriptors | data-bind.md | Entire spec page has no demo coverage |
| D5 | JSON save/load | overview.md | Key enterprise feature; no demo |

### High (important UX features, no demo)

| # | Scenario | Spec Reference | Notes |
|---|---|---|---|
| D6 | Connection types (Cascading vs. Polyline) | connections.md | No demo showing routed connections |
| D7 | Connection caps (ArrowEnd, FilledCircle, None) | connections.md | No demo showing cap configuration |
| D8 | Zoom and pan | overview.md | Interactive feature; no demo |
| D9 | Selection (single + marquee) | overview.md | Interactive feature; no demo |
| D10 | All shape types gallery | shapes.md | Spec has a 26-shape gallery example; no demo |
| D11 | Shape styling (fill, hover, stroke) | shapes.md | No styling customization demo |
| D12 | Connection styling | connections.md | No connection styling demo |

### Medium (useful demos, lower priority)

| # | Scenario | Spec Reference | Notes |
|---|---|---|---|
| D13 | Shape editability (drag, connect, remove) | shapes.md | No editing interaction demo |
| D14 | Connection editability | connections.md | No editing interaction demo |
| D15 | Shape tooltips | shapes.md | No tooltip demo |
| D16 | Connection tooltips | connections.md | No tooltip demo |
| D17 | Connection text and labels | connections.md | No labeled connections demo |
| D18 | Layout grid settings (multiple components) | layouts.md | No multi-subgraph demo |
| D19 | `OnConnectionClick` event | events.md | Only `OnNodeClick` is demonstrated |
| D20 | Connection points (polyline waypoints) | connections.md | No intermediate point demo |

### Low (advanced features)

| # | Scenario | Spec Reference | Notes |
|---|---|---|---|
| D21 | Visual functions (shape) | shapes.md | JS interop; advanced |
| D22 | Visual functions (connection) | connections.md | JS interop; advanced |
| D23 | Image shapes | shapes.md | Specific shape type |
| D24 | Custom path shapes | shapes.md | SVG path-based |
| D25 | Connectors (hover dots) | shapes.md | Interaction detail |
| D26 | Selection handles styling | connections.md | Deep customization |

---

## Demo Quality Assessment

**Existing demos:** The 3 current demo sections are functional and follow the `DemoSection` pattern correctly. Code display strings are provided. The `HandleNodeClick` interaction works and shows a selection indicator.

**Gaps:**
- No demo demonstrates ANY spec-defined API (all use source-ahead flat-list API)
- No interactive demos (zoom, pan, drag, select)
- No layout algorithm demos (the most visually impactful feature)
- Shape coverage is 2/26+ types
- Connection coverage is basic lines only (no caps, types, or styling)

**Overall demo coverage vs. spec: ~5%**

---

## Recommendations

1. **Immediate:** Add a layouts demo (Tree with subtypes selector) -- this is the most visually impressive feature and shows the component's value proposition.
2. **When source catches up:** Add a shape types gallery matching the spec's 26-shape example.
3. **When source catches up:** Add data binding and JSON save/load demos for enterprise scenarios.
4. **When source catches up:** Add zoom/pan and selection interaction demos.
5. **Naming alignment:** Current demos use `OnNodeClick` -- when spec/source align, update to final naming.
