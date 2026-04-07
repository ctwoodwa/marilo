# Chart Gap Prioritization

> Date: 2026-04-04
> Source: gap-chart-inventory.md (16 gaps)
> Stage: 02-prioritize

## Audit Findings (Pre-Prioritization)

Code review reveals several gaps are already resolved or misidentified:

| Gap | Finding | Resolution |
|-----|---------|------------|
| GAP-CHART-001 | `Refresh()` method **exists** — calls `StateHasChanged()` | **Already resolved** — close immediately |
| GAP-CHART-004 | `Class` parameter **inherited** from `MariloComponentBase` | **Already resolved** — close immediately |
| GAP-CHART-006 | `ChartCategoryAxis.razor` **exists** in Charts/ folder | **Partially resolved** — verify wrapper (ChartCategoryAxes) |
| GAP-CHART-005 | Child components exist (ChartTitle, ChartLegend, ChartTooltip, ChartCategoryAxis, ChartValueAxis) | **Partially resolved** — ChartSeriesItems wrapper may be missing |

**Effective open gaps: 12** (4 already resolved or near-resolved)

## Priority Batches

### Batch 1: API Surface + Testing (Critical + High) — 8 gaps

| Gap | Severity | Description | Effort | Notes |
|-----|----------|-------------|--------|-------|
| GAP-CHART-001 | Critical | Refresh() method | — | **Already resolved** — exists as public method |
| GAP-CHART-004 | Critical | Class parameter | — | **Already resolved** — inherited from MariloComponentBase |
| GAP-CHART-005 | Critical | ChartSeriesItems wrapper | S | Pass-through wrapper (same pattern as SplitterPanes) |
| GAP-CHART-006 | Critical | ChartCategoryAxes wrapper | S | ChartCategoryAxis exists; add wrapper if needed |
| GAP-CHART-010 | High | Insufficient test coverage (5 tests / 994 lines) | L | bUnit tests for series types, events, child components, a11y |
| GAP-CHART-003 | High | Missing ChartSubtitle | S | Simple child component registering with ChartTitle |
| GAP-CHART-008 | High | CSS variable theming | M | Map CSS vars to Palette/color resolution |
| GAP-CHART-009 | High | Limited chart type coverage audit | M | Audit ChartSeriesType enum completeness |

### Batch 2: Events + Polish (Medium) — 5 gaps

| Gap | Severity | Description | Effort |
|-----|----------|-------------|--------|
| GAP-CHART-011 | Medium | Data binding parameter name alignment | S |
| GAP-CHART-012 | Medium | Transitions bool → bool? | S |
| GAP-CHART-013 | Medium | Missing OnRender / OnAxisRender events | M |
| GAP-CHART-014 | Medium | Tooltip customization API | M |
| GAP-CHART-015 | Medium | Legend positioning | S |

### Deferred: Drilldown

| Gap | Severity | Description | Reason |
|-----|----------|-------------|--------|
| GAP-CHART-002 | High | ResetDrilldownLevel() | Requires full drilldown state management; separate feature scope |
| GAP-CHART-007 | Critical | Demo pages | Defer to Chart delivery CDW |

## Recommended Sequence

1. **Close already-resolved gaps** (GAP-CHART-001, GAP-CHART-004) — 0 effort
2. **Batch 1 implementation** — pass-through wrappers (S), ChartSubtitle (S), expand tests (L)
3. **Batch 2 polish** — events, tooltip, legend, type alignment
4. **Drilldown** — separate scope or CDW handoff

## Dependencies

- ChartSeriesItems/ChartCategoryAxes wrappers: no dependencies (same pattern as SplitterPanes, MariloWizardSteps)
- ChartSubtitle: depends on ChartTitle registration mechanism (already exists)
- CSS variable theming: depends on understanding current color resolution pipeline
- Test expansion: should cover Batch 1 changes
