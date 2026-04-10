# Capture Matrix -- MariloDiagram

Defines theme/mode/state combinations for Diagram visual parity review. Diagram is a canvas-based component with a rich node/connector state surface — node boundary rendering, connector routing chrome, and canvas chrome are the primary visual differentiators.

## Theme/Mode Matrix

| Theme | Light | Dark |
|-------|-------|------|
| Fluent | Required | Required |
| Bootstrap | Required | Required |
| Material | Required | Required |

**Total theme/mode combinations:** 6

## Viewport Matrix

| Viewport | Width | Use Case |
|----------|-------|----------|
| Desktop | 1280px | Primary review viewport |
| Wide | 1920px | Canvas chrome and minimap at full extent |

## Diagram State Inventory

Each state below is a capture point per theme/mode combination.

### Node States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Node default | Node at rest — border, fill, shadow, and label | Yes |
| 2 | Node selected | Node with selection handles active | Yes |
| 3 | Node hover | Mouse over a node — hover ring or border change | Yes |
| 4 | Text label only | Node with no shape chrome — label as primary element | Yes |

### Connector States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 5 | Connector line | Line between nodes — stroke weight, color, routing | Yes |
| 6 | Connector endpoint | Arrowhead or endpoint marker at line termination | Yes |

### Canvas and Chrome

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 7 | Canvas background | Canvas area fill — grid dots/lines or flat color | Yes |
| 8 | Empty canvas | Canvas with no nodes — background and chrome only | Yes |
| 9 | Zoom controls | Zoom in/out/reset buttons — size, position, styling | Yes |
| 10 | Minimap | Minimap overlay — border, background, viewport indicator | Yes |

### Group and Container

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 11 | Group container | Group bounding box — border, fill, label treatment | Yes |
| 12 | Group container selected | Group with selection handles | Yes |
| 13 | Node selected with group | Node selected inside a group container | Partial |

**Total state/scenario items:** 13
**Total capture points:** 13 states x 6 theme/modes = 78 (minus N/A for unsupported states)

## Diagram-Specific Gap Categories

When scoring Diagram captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Node border weight | Stroke weight and color of node outline in default and selected states |
| Connector routing | Line routing style (straight, orthogonal, curved) and stroke quality |
| Selection handles | Size, color, and position of resize/move handles on selected nodes |
| Canvas grid | Dot or line grid visibility and color against canvas background |
| Zoom chrome | Button size, icon, border radius, and position of zoom controls |
| Group styling | Group border dashing, fill opacity, and label placement |
| Connector endpoint | Arrowhead style, size, and fill color |
| Minimap styling | Minimap border, background tint, and viewport rect color |
| Node shadow/elevation | Drop shadow presence and softness on default nodes |

## Capture Priority

For first-pass review, prioritize in this order:
1. Node default + node selected (core node visual baseline)
2. Node hover + connector line (primary interaction and connection states)
3. Canvas background + zoom controls (canvas chrome quality)
4. Group container + connector endpoint (structural chrome)
5. Minimap + text label only (auxiliary chrome)
6. Empty canvas + group container selected + node selected with group (edge scenarios)
