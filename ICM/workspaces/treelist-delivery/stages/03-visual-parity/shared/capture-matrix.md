# Capture Matrix -- MariloTreeList

Defines theme/mode/state combinations for TreeList visual parity review. TreeList is a hierarchical grid component requiring coverage across tree indentation, expand/collapse, row interactions, editing, sorting, filtering, and paging.

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
| Narrow | 768px | Pager compactness and column overflow |

## TreeList State Inventory

Each state below is a capture point per theme/mode combination.

### Structure and Hierarchy

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Default view | TreeList at rest with hierarchical sample data | Yes |
| 2 | Header row | Column headers with sort indicators idle | Yes |
| 3 | Hierarchical rows | Multi-level tree rows with indentation | Yes |
| 4 | Expanded row | Parent row expanded, children visible | Yes |
| 5 | Collapsed row | Parent row collapsed | Yes |

### Row Interactions

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 6 | Row hover | Mouse over a data row | Yes |
| 7 | Selected row | Single row selection active | Yes |
| 8 | Checkbox selection | Checkbox column with selected/unselected rows | Yes |

### Sorting and Filtering

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 9 | Sorted column | Column sorted ascending or descending, indicator visible | Yes |
| 10 | Filter row | Inline filter row visible with input fields | Yes |

### Editing

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 11 | Inline edit row | Row in edit mode with input fields | Yes |

### Pager

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 12 | Pager idle | Pager at rest, page numbers visible | Yes |
| 13 | Pager hover/active | Pager button in hover and active states | Yes |

### Edge States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 14 | Empty state | TreeList with no data rows | Yes |
| 15 | Loading state | Skeleton or spinner while data loads | Partial |

**Total state/scenario items:** 15
**Total capture points:** 15 states x 6 theme/modes = 90 (minus N/A for unsupported states)

## TreeList-Specific Gap Categories

When scoring TreeList captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Indentation per level | Pixels per nesting level — consistency across hierarchy depth |
| Expand icon alignment | Vertical centering and horizontal position of expand/collapse chevron |
| Row density | Row height and cell padding — compact vs. comfortable vs. Telerik reference |
| Header typography | Font weight, size, color, and case treatment in column headers |
| Filter/sort indicators | Sort arrow sizing and filter icon visibility |
| Pager compactness | Button sizing, spacing, and page-number density |
| Checkbox alignment | Vertical centering of checkbox within tree cell |
| Dark mode row tints | Hover and selected row background tints using surface tokens |

## Capture Priority

For first-pass review, prioritize in this order:
1. Default view + header row (structural baseline)
2. Hierarchical rows + expanded/collapsed (tree indentation quality)
3. Row hover + selected row (interaction states)
4. Sorted column + filter row (indicator quality)
5. Checkbox selection + pager idle (secondary elements)
6. Inline edit row + pager hover (edit and pager quality)
7. Empty state + loading (edge cases)
