# Capture Matrix -- MariloAllocationScheduler

Defines theme/mode/state combinations for AllocationScheduler visual parity review. AllocationScheduler is a layout-intensive, state-rich component requiring comprehensive coverage across resource, timeline, and cell dimensions.

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
| Wide | 1600px | Timeline density at larger widths |

## AllocationScheduler State Inventory

Each state below is a capture point per theme/mode combination.

### Layout Structure

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Default view | Scheduler at rest with resource panel, timeline, and sample allocations | Yes |
| 2 | Resource panel | Left resource column showing resource names and avatars/icons | Yes |
| 3 | Timeline header | Grouped timeline header row (day/hour tiers) | Yes |
| 4 | Splitter at rest | Splitter bar between resource panel and timeline pane | Yes |

### Bucket Cells

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 5 | Default bucket cell | Allocation bucket cell at rest | Yes |
| 6 | Occupied cell | Bucket cell with an allocation block rendered | Yes |
| 7 | Disabled cell | Cell in disabled/locked state (no allocation allowed) | Yes |
| 8 | Conflict indicator | Cell or allocation showing a conflict state | Partial |
| 9 | Current period highlight | Today/current period column or cell highlighted | Yes |

### Interaction States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 10 | Cell hover | Mouse over a bucket cell | Yes |
| 11 | Cell editing | Cell in edit mode with inline input | Yes |
| 12 | Drag-fill in progress | Allocation being extended by drag | Partial |
| 13 | Selected allocation | An allocation block in selected state | Yes |

### Edge States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 14 | Empty resource | Resource row with no allocations | Yes |
| 15 | Loading state | Skeleton or spinner while data loads | Partial |

**Total state/scenario items:** 15
**Total capture points:** 15 states x 6 theme/modes = 90 (minus N/A for unsupported states)

## AllocationScheduler-Specific Gap Categories

When scoring AllocationScheduler captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Splitter alignment | Splitter bar visibility, drag handle sizing, alignment between panels |
| Bucket cell density | Cell height, padding, border weight in timeline grid |
| Resource column width | Default width, text truncation, avatar/icon sizing |
| Timeline header typography | Font weight, size, tier separator lines, grouping label alignment |
| Cell editing chrome | Input sizing, confirm/cancel buttons, focus ring within cell |
| Drag-fill indicators | Ghost preview, fill handle sizing, color during drag |
| Dark mode tints | color-mix base color — must use var(--marilo-color-surface) not hardcoded #ffffff |
| Conflict indicator styling | Color, icon, and overlay treatment for conflicting allocations |
| Current period highlight | Background tint contrast in light and dark modes |

## Capture Priority

For first-pass review, prioritize in this order:
1. Default view + resource panel (structural baseline)
2. Timeline header + splitter (layout anchor)
3. Occupied cell + cell hover (interaction density)
4. Current period highlight + disabled cell (state treatment)
5. Cell editing + selected allocation (edit-mode chrome)
6. Conflict indicator + drag-fill (complex interaction states)
7. Empty resource + loading state (edge cases)
