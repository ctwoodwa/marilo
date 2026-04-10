# Capture Matrix -- MariloMap

Defines theme/mode/state combinations for MariloMap visual parity review. Map is a canvas-heavy component with distinct layers requiring targeted state coverage.

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
| Narrow | 768px | Control positioning / legend overflow |

## Map State Inventory

Each state below is a capture point per theme/mode combination.

### Primary States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Map canvas default | Map at rest with tile layer loaded, no user interaction | Yes |
| 2 | Marker default | Standard marker rendered on map at rest | Yes |
| 3 | Marker hover | Mouse over a map marker | Yes |
| 4 | Marker selected | Marker in selected/active state | Yes |

### Secondary States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 5 | Zoom controls | Zoom in/out buttons at rest and hover | Yes |
| 6 | Tooltip | Marker tooltip open and visible | Yes |
| 7 | Legend | Map legend component at rest | Yes |

### Edge States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 8 | Bubble layer | Bubble/proportional symbol layer rendered | Yes |
| 9 | Empty map | Map canvas with no markers or data layers | N/A |
| 10 | Navigation controls | Pan/compass controls at rest | Yes |

**Total state/scenario items:** 10
**Total capture points:** 10 states x 6 theme/modes = 60 (minus N/A for unsupported states)

## Map-Specific Gap Categories

When scoring Map captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Marker sizing | Marker icon size relative to Telerik reference |
| Tooltip chrome | Tooltip border, shadow, background color in light/dark |
| Control button styling | Zoom/nav button shape, background, border, hover state |
| Legend layout | Legend spacing, label typography, icon alignment |
| Bubble opacity | Bubble layer fill opacity and stroke weight |
| Tile contrast | Map tile visibility against control overlays in dark mode |
| Focus treatment | Keyboard focus on zoom controls and interactive markers |

## Capture Priority

For first-pass review, prioritize in this order:
1. Map canvas default + marker default (structural baseline)
2. Marker hover + marker selected (interaction states)
3. Zoom controls (control styling quality)
4. Tooltip (popup chrome quality)
5. Legend (layout and typography)
6. Bubble layer + navigation controls
7. Empty map
