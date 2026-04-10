# Capture Matrix -- MariloPivotGrid

Defines theme/mode/state combinations for PivotGrid visual parity review. PivotGrid is a dense, header-heavy component requiring coverage across nested headers, data cells, grouping, and the field chooser panel.

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
| Wide | 1600px | Deep header nesting and column density |

## PivotGrid State Inventory

Each state below is a capture point per theme/mode combination.

### Headers and Data Cells

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Default view | PivotGrid at rest with sample data, row and column headers visible | Yes |
| 2 | Row headers | Nested row header hierarchy at rest | Yes |
| 3 | Column headers | Nested column header hierarchy at rest | Yes |
| 4 | Data cells | Aggregated value cells with number formatting | Yes |
| 5 | Aggregation typography | Totals/subtotals rows with distinct typography treatment | Yes |

### Grouping States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 6 | Expanded group | Row or column group in expanded state, children visible | Yes |
| 7 | Collapsed group | Row or column group in collapsed state | Yes |
| 8 | Group expand/collapse icon | Expand/collapse chevron or icon at rest and hover | Yes |

### Field Chooser

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 9 | Field chooser idle | Field chooser panel/dialog at rest with available fields | Yes |
| 10 | Field chooser drag | Field being dragged to a zone (if applicable) | Partial |

### Edge States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 11 | Empty state | PivotGrid with no data or no fields configured | Yes |
| 12 | Loading state | Skeleton or spinner while data loads | Partial |

**Total state/scenario items:** 12
**Total capture points:** 12 states x 6 theme/modes = 72 (minus N/A for unsupported states)

## PivotGrid-Specific Gap Categories

When scoring PivotGrid captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Header nesting indentation | Pixels per nesting level in row and column headers |
| Data cell alignment | Right-alignment of numeric values, padding consistency |
| Aggregation typography | Font weight and color distinction for totals/subtotals vs. data rows |
| Group expand/collapse icons | Icon size, alignment within header cell, hover treatment |
| Field chooser chrome | Panel border, zone backgrounds, drag affordance styling |
| Header border contrast | Grid lines between header cells in light and dark modes |
| Dark mode cell surfaces | Data cell and header background tints in dark mode |

## Capture Priority

For first-pass review, prioritize in this order:
1. Default view + row headers + column headers (structural baseline)
2. Data cells + aggregation typography (data density quality)
3. Expanded group + collapsed group (grouping state treatment)
4. Group expand/collapse icon (icon precision)
5. Field chooser idle (panel chrome quality)
6. Empty state + loading + field chooser drag (edge cases)
