# Capture Matrix -- MariloSplitter

Defines theme/mode/state combinations for Splitter visual parity review. Splitter is a structural layout component with a small but precise state surface — handle interactions and pane boundary rendering are the primary visual differentiators.

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
| Narrow | 768px | Pane constraint behavior / compact layout |

## Splitter State Inventory

Each state below is a capture point per theme/mode combination.

### Orientation and Layout

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Horizontal split default | Two panes side-by-side at rest, handle visible | Yes |
| 2 | Vertical split default | Two panes stacked at rest, handle visible | Yes |

### Handle Interactions

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 3 | Resize handle idle | Handle at rest, no user interaction | Yes |
| 4 | Resize handle hover | Mouse over the resize handle | Yes |
| 5 | Resize handle active | Handle being dragged, drag in progress | Yes |

### Pane States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 6 | Collapsed pane | One pane collapsed to minimum, collapse button visible | Yes |
| 7 | Min/max constraint hit | Pane at its minimum or maximum size limit | Yes |

### Advanced Layouts

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 8 | Nested splitters | Splitter containing a child splitter, both handles visible | Yes |
| 9 | Three-pane layout | Three panes with two handles | Partial |
| 10 | Collapse button styling | Collapse/expand toggle button at rest and hover | Yes |

**Total state/scenario items:** 10
**Total capture points:** 10 states x 6 theme/modes = 60 (minus N/A for unsupported states)

## Splitter-Specific Gap Categories

When scoring Splitter captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Handle width/height | Physical size of the drag handle bar in horizontal vs. vertical orientation |
| Handle hover color | Background color change on handle hover — must be distinct but not jarring |
| Handle active color | Color during active drag — should have clear pressed/active treatment |
| Collapse button styling | Size, icon, border, and color of the collapse toggle button |
| Pane border weight | Border or divider line weight between panes at rest |
| Resize cursor | `col-resize` or `row-resize` cursor applies on handle hover |
| Pane background | Pane content area background — should not bleed into handle zone |
| Constraint visual | Any visual indication when a pane reaches min/max size |

## Capture Priority

For first-pass review, prioritize in this order:
1. Horizontal split default + handle idle (structural baseline)
2. Resize handle hover + resize handle active (core interaction states)
3. Collapsed pane + collapse button (collapse treatment quality)
4. Vertical split default (orientation variant)
5. Nested splitters (complexity layer)
6. Min/max constraint hit + three-pane layout
