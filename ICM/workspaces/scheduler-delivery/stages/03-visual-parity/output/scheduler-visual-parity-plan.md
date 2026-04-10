# Visual Parity Plan -- MariloScheduler

## Component

MariloScheduler

## Reference Strategy

**Telerik Scheduler parity.** Telerik's Blazor Scheduler provides the visual reference baseline for calendar views (day, week, month, timeline), appointment rendering, popup editing, time indicators, and drag/drop interactions. Marilo targets visual quality equivalence in calendar grid density, appointment treatment, and view-switcher styling.

## Prioritized Scenarios (First Pass)

| Priority | Scenario | Why First |
|----------|----------|-----------|
| P1 | Week view default | Most commonly seen calendar layout |
| P1 | Day view default | Core single-day calendar |
| P1 | Appointment card default | Primary content element |
| P1 | Appointment hover/selected | Core interaction states |
| P2 | Current time indicator | Temporal anchor, high visibility |
| P2 | Work-hours region | Visual distinction critical for usability |
| P2 | All-day area | Layout complexity above time grid |
| P2 | Overlapping appointments | Dense scenario quality |
| P3 | Popup editor | Overlay chrome and form quality |
| P3 | View-switcher buttons | Navigation tab styling |
| P3 | Empty timeslot | Baseline grid appearance |
| P3 | Month view | Alternative layout (if supported) |
| P4 | Timeline view | Horizontal layout (if supported) |
| P4 | Drag preview / resize affordance | Interaction visual feedback |
| P4 | Disabled/read-only slot | Edge case styling |

## First-Pass Review Order

1. **Fluent Light** — primary design target, most mature provider
2. **Fluent Dark** — validates dark-mode token coverage for time grid and appointments
3. **Bootstrap Light** — validates bridge token mapping for calendar-specific elements
4. **Bootstrap Dark** — validates dark-mode patches for grid borders and appointment colors
5. **Material Light** — newest provider, likely most gaps in calendar rendering
6. **Material Dark** — highest gap density expected

## Known Gap Categories to Watch

Based on Scheduler-specific concerns:
- Calendar grid border weight and contrast in dark mode
- Appointment background color opacity and text readability
- Time label font size relative to grid density
- Current-time indicator line weight and color prominence
- Work-hours vs non-work-hours background differentiation in dark mode
- Popup editor shadow and background treatment across themes
- View-switcher active state indicator consistency

## Known Unknowns

- Material provider runtime not yet implemented — Material captures may be blocked
- Which views (day, week, month, timeline) are currently implemented in Marilo
- Appointment drag/drop and resize visual affordances — may not be implemented yet
- Popup editor form layout and styling completeness
- Multi-day appointment spanning behavior in week/month views

## Blockers

- Material runtime provider not yet implemented (SCSS-only scaffold as of 2026-04-10)
- Scheduler component maturity unknown — may need Stage 01 (spec review) first

## Next Steps After Plan

1. Verify which Scheduler views and features are currently implemented
2. Confirm Stage 02 output exists or create baseline demo scenarios
3. Set up Playwright capture scripts for Scheduler screenshot collection
4. Execute first-pass Fluent Light review across P1 scenarios
5. Document gaps and iterate through remaining themes/modes
