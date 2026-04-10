# Capture Matrix -- MariloDataGrid

Defines theme/mode/state combinations for DataGrid visual parity review. DataGrid is a dense, state-rich component requiring comprehensive coverage.

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
| Narrow | 768px | Column overflow / responsive behavior |

## DataGrid State Inventory

Each state below is a capture point per theme/mode combination.

### Structure and Data Display

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Default grid | Grid at rest with sample data, no interactions | Yes |
| 2 | Header row | Column headers with sort indicators idle | Yes |
| 3 | Empty state | Grid with no data rows, empty message visible | Yes |
| 4 | Loading state | Skeleton or spinner while data loads | Yes |

### Row Interactions

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 5 | Row hover | Mouse over a data row | Yes |
| 6 | Selected row | Single row selection active | Yes |
| 7 | Checkbox selection | Checkbox column with selected/unselected rows | Yes |

### Sorting

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 8 | Sorted ascending | Column sorted ascending, indicator visible | Yes |
| 9 | Sorted descending | Column sorted descending, indicator visible | Yes |

### Filtering

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 10 | Filter row | Inline filter row visible with input fields | Yes |
| 11 | Filter menu / popover | Column filter popup open | Yes |

### Grouping

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 12 | Grouped state | Data grouped by one column, group headers visible | Yes |

### Editing

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 13 | Inline edit row | Row in edit mode with input fields | Yes |
| 14 | Popup edit dialog | Edit form in overlay/popup (if supported) | Yes |

### Pager

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 15 | Pager idle | Pager at rest, page numbers visible | Yes |
| 16 | Pager hover/active/focus | Pager button in hover, active, and focus states | Yes |

### Advanced

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 17 | Toolbar / command area | Toolbar with action buttons if present | Yes |
| 18 | Virtualization / dense scroll | Large dataset with virtual scrolling if supported | Partial |

**Total state/scenario items:** 18
**Total capture points:** 18 states x 6 theme/modes = 108 (minus N/A for unsupported states)

## DataGrid-Specific Gap Categories

When scoring DataGrid captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Density | Row height, cell padding, compact vs. comfortable spacing |
| Row height | Consistent row height across data/edit/group rows |
| Header typography | Font weight, size, color, case treatment in column headers |
| Border contrast | Grid lines, header borders, cell borders in light and dark |
| State-layer colors | Hover, selected, and active row background tints |
| Checkbox alignment | Vertical centering of checkbox within cell |
| Pager compactness | Button sizing, spacing, page-number density |
| Icon sizing/alignment | Sort arrows, filter icons, expand/collapse chevrons |
| Popup chrome | Filter menu borders, shadows, background in light/dark |
| Focus treatment | Keyboard focus rings on cells, pager buttons, filter inputs |

## Capture Priority

For first-pass review, prioritize in this order:
1. Default grid + header row (structural baseline)
2. Row hover + selected row (interaction states)
3. Sorted ascending/descending (indicator quality)
4. Filter row + filter menu (complex UI overlay)
5. Pager states (density and alignment)
6. Grouped state + editing states
7. Empty/loading/toolbar/virtualization
