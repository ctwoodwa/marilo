# Capture Matrix -- MariloDockManager

Defines theme/mode/state combinations for MariloDockManager visual parity review. DockManager is a layout-orchestration component with panel, tab, and drag/drop states requiring targeted coverage.

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
| Narrow | 768px | Panel overflow / split layout behavior |

## DockManager State Inventory

Each state below is a capture point per theme/mode combination.

### Primary States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Docked panel | Panel docked in layout at rest | N/A |
| 2 | Panel header | Panel header bar with title and controls | N/A |
| 3 | Tab strip | Tab strip with multiple panel tabs | N/A |

### Secondary States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 4 | Floating panel | Panel in floating/undocked state | N/A |
| 5 | Split layout | Two panels arranged in split view | N/A |
| 6 | Close/minimize buttons | Panel header action buttons at rest and hover | N/A |

### Edge States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 7 | Drag preview | Panel drag ghost while repositioning | N/A |
| 8 | Drop indicator | Drop zone highlight during drag | N/A |
| 9 | Empty dock zone | Dock zone with no panels | N/A |

**Total state/scenario items:** 9
**Total capture points:** 9 states x 6 theme/modes = 54 (all N/A for Telerik ref — internal baseline only)

## DockManager-Specific Gap Categories

When scoring DockManager captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Panel header height | Header bar height and vertical padding consistency |
| Tab strip styling | Active/inactive tab background, border, and typography |
| Drag preview opacity | Ghost panel opacity and border during drag |
| Drop zone indicator | Drop target highlight color and border weight |
| Splitter between panels | Splitter handle width, color, and hover state |
| Button icon sizing | Close/minimize icon size and alignment in header |
| Floating panel elevation | Shadow depth and border for floating panels |
| Focus treatment | Keyboard focus on panel headers and tab controls |

## Capture Priority

For first-pass review, prioritize in this order:
1. Docked panel + panel header (structural baseline)
2. Tab strip (tab chrome quality)
3. Split layout + close/minimize buttons (layout and controls)
4. Floating panel (elevation and border)
5. Drop indicator + drag preview (interaction state quality)
6. Empty dock zone
