# Capture Matrix -- MariloChart

Defines theme/mode/state combinations for Chart visual parity review. Chart is a data-visualization component requiring coverage across series types, interactive states, and compositional elements.

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
| Narrow | 768px | Legend reflow and responsive axis label behavior |

## Chart State Inventory

Each state below is a capture point per theme/mode combination.

### Series Types

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 1 | Line chart | Single or multi-series line chart at rest | Yes |
| 2 | Bar chart | Vertical bar chart with category axis | Yes |
| 3 | Area chart | Area series with fill and stroke | Yes |
| 4 | Pie chart | Pie/donut chart at rest with legend | Yes |

### Chart Structure

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 5 | Axis labels | X and Y axis label typography and alignment | Yes |
| 6 | Gridlines | Background gridlines weight and color | Yes |
| 7 | Legend idle | Legend at rest, series color swatches and labels | Yes |

### Interactive States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 8 | Series hover | Mouse over a data point or bar, hover highlight | Yes |
| 9 | Tooltip visible | Data tooltip shown on hover | Yes |
| 10 | Series selected | Clicked data point or series in selected state | Yes |
| 11 | Data point markers | Point markers on line/area series | Yes |

### Edge States

| # | State/Scenario | Description | Telerik Ref |
|---|---------------|-------------|-------------|
| 12 | Empty state | Chart with no data, empty message or empty plot area | Yes |
| 13 | Loading state | Skeleton or spinner while data loads | Partial |
| 14 | Legend layout overflow | Legend items wrapping at narrow widths | Partial |

**Total state/scenario items:** 14
**Total capture points:** 14 states x 6 theme/modes = 84 (minus N/A for unsupported states)

## Chart-Specific Gap Categories

When scoring Chart captures, pay particular attention to:

| Category | What to Check |
|----------|--------------|
| Series colors | Theme-mapped series palette — matches provider color tokens |
| Gridline weight | Line width and opacity of background gridlines in light and dark |
| Axis typography | Font size, weight, and color of axis tick labels and titles |
| Tooltip chrome | Tooltip border, background, shadow, and text contrast in both modes |
| Legend layout | Spacing between swatch and label, row spacing, wrapping behavior |
| Data point markers | Marker size, shape, border, and fill color at rest and hover |
| Area fill opacity | Fill transparency consistency across series and themes |
| Dark mode surface | Plot area background and axis line colors in dark mode |

## Capture Priority

For first-pass review, prioritize in this order:
1. Line chart + bar chart (most common series types — structural baseline)
2. Axis labels + gridlines (structural quality anchors)
3. Series hover + tooltip visible (interaction quality)
4. Legend idle (compositional element)
5. Pie chart + area chart (secondary series types)
6. Series selected + data point markers (selection and marker quality)
7. Empty state + loading + legend overflow (edge cases)
