# Capture Matrix -- MariloDataSheet

Defines theme/mode/state combinations for MariloDataSheet visual parity review. DataSheet is a true spreadsheet component with dense cell-grid states requiring careful coverage.

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
| Narrow | 768px | Frozen column / scrollbar behavior |

## DataSheet State Inventory

Each state below is a capture point per theme/mode combination.

### Primary States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Cell grid default | Sheet at rest with sample data, no interaction | N/A |
| 2 | Selected cell | Single cell selected, highlight and border visible | N/A |
| 3 | Cell editing | Active cell in edit mode with inline input | N/A |

### Secondary States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 4 | Column/row headers | Header row and column at rest | N/A |
| 5 | Frozen rows/columns | Frozen area separator and sticky headers visible | N/A |
| 6 | Cell range selection | Multi-cell range highlighted | N/A |

### Edge States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 7 | Formula bar | Formula bar visible and active | N/A |
| 8 | Sheet tabs | Tab strip with multiple sheets | N/A |
| 9 | Empty sheet | Sheet with no data, grid lines only | N/A |

**Total state/scenario items:** 9
**Total capture points:** 9 states x 6 theme/modes = 54 (all N/A for Telerik ref — internal baseline only)

## DataSheet-Specific Gap Categories

When scoring DataSheet captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Cell border weight | Grid line thickness, color, and contrast in light/dark |
| Header background | Column/row header fill color vs. cell background |
| Selection highlight | Selected cell border color, fill, and range tint |
| Editing input chrome | Input border, caret, background in edit mode |
| Frozen separator | Visual separator line weight and color at freeze boundary |
| Sheet tab styling | Active/inactive tab background, border, typography |
| Scrollbar | Scrollbar visibility and styling in light/dark |
| Focus treatment | Keyboard focus on cells and controls |

## Capture Priority

For first-pass review, prioritize in this order:
1. Cell grid default + column/row headers (structural baseline)
2. Selected cell + cell editing (primary interaction states)
3. Cell range selection (multi-select visual)
4. Frozen rows/columns (layout boundary quality)
5. Sheet tabs (chrome quality)
6. Formula bar + empty sheet
