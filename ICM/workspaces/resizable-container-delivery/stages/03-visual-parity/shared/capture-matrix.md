# Capture Matrix -- MariloResizableContainer

Defines theme/mode/state combinations for MariloResizableContainer visual parity review. ResizableContainer is a simple utility component — state inventory is kept to 5-7 items.

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

## ResizableContainer State Inventory

Each state below is a capture point per theme/mode combination.

### Primary States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Container default | Container at rest with content, handles not visible or idle | N/A |
| 2 | Resize handle hover | Mouse over a resize handle | N/A |
| 3 | Resize handle active | Handle being dragged, resize in progress | N/A |

### Secondary States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 4 | Min/max constraint visible | Container at minimum or maximum size limit | N/A |

### Edge States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 5 | Corner handles | Corner drag handles if present | N/A |

**Total state/scenario items:** 5
**Total capture points:** 5 states x 6 theme/modes = 30 (all N/A for Telerik ref — internal baseline only)

## ResizableContainer-Specific Gap Categories

| Category | What to Check |
|----------|--------------|
| Handle visibility | Handle visibility at rest vs. hover — not hidden when needed |
| Handle cursor | Correct resize cursor (ew-resize, ns-resize, nwse-resize) |
| Resize border | Border or outline during active drag |
| Constraint indicator | Visual feedback when min/max limit is reached |

## Capture Priority

For first-pass review, prioritize in this order:
1. Container default + resize handle hover (baseline and primary interaction)
2. Resize handle active (drag state quality)
3. Min/max constraint visible
4. Corner handles
