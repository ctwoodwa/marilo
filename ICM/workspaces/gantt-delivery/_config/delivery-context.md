# Delivery Context -- MariloGantt

## Component Identity

| Field | Value |
|-------|-------|
| Component name | MariloGantt |
| Component slug | gantt |
| Complexity tier | Complex (CDW warranted) |
| Active phase | Phase 1 (starting fresh) |

## Artifact Paths

| Field | Value |
|-------|-------|
| API spec | /workspaces/Marilo/docs/component-specs/gantt/ |
| Example UX | /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/Gantt/ |
| Component source | /workspaces/Marilo/src/Marilo.Components/DataDisplay/ |
| Test files | UNKNOWN |
| Gap workspace | /workspaces/Marilo/workspaces/gantt-gap-analysis |

## Spec State

| Field | Value |
|-------|-------|
| Spec version | unversioned |
| Last spec audit | 2026-04-11 |
| Open spec gaps | 33 (CDW stage-01 output); ~107 unique in gantt-gap-analysis output/stage-01/ |

## Example UX State

| Field | Value |
|-------|-------|
| Demo page(s) | Gantt/Overview.razor, Views.razor, Templates.razor, Hierarchical.razor, Editing.razor, Features.razor |
| Last demo audit | 2026-04-11 (Wave 2) |
| Open demo gaps | 8 (EUX-01..EUX-08; EUX-04 and EUX-05 are Tracked-Out-of-Session) |

## Visual Parity State

| Field | Value |
|-------|-------|
| Themes to cover | Fluent, Bootstrap, Material |
| Modes to cover | Light, Dark |
| Last parity audit | 2026-04-11 (Wave 3, static analysis) |
| Parity gaps | 16 direct (VP-gantt-01..16) + 2 Deferred (VP-gantt-17, VP-gantt-18); 3 Critical, 7 Major, 4 Minor |

## Delivery Gate

| Field | Value |
|-------|-------|
| Last sync check | 2026-04-11 (Wave 4) |
| Gate status | **BLOCKED** |
| Blocking items | 7 (VP-gantt-01, VP-gantt-02, VP-gantt-03, VP-gantt-16; NM-01/NM-02 + SA-01/02 GanttState API; SA-06 + SRC-01..06 spec under-population; EUX demo set) |
| Tracked-Out-of-Session | 2 (VP-gantt-17 / EUX-04 GetState/SetStateAsync; VP-gantt-18 / EUX-05 TaskListWidthChanged — both queued to gantt-gap-analysis) |
| Pipeline status | CLOSED — remediation flows through gantt-gap-analysis workspace |

## Gap Workspace Link

| Field | Value |
|-------|-------|
| Latest closure reports | /workspaces/Marilo/workspaces/gantt-gap-analysis/stages/06-validate/output/ |
| Coverage summary | /workspaces/Marilo/workspaces/gantt-gap-analysis/_config/coverage-summary.md |

## Spec Feature Areas

| Feature Area | Spec Path | Status |
|---|---|---|
| accessibility | docs/component-specs/gantt/accessibility/ | PENDING |
| dependencies | docs/component-specs/gantt/dependencies/ | PENDING |
| gantt-tree | docs/component-specs/gantt/gantt-tree/ | PENDING |
| timeline | docs/component-specs/gantt/timeline/ | PENDING |
| events | docs/component-specs/gantt/events.md | PENDING |
| overview | docs/component-specs/gantt/overview.md | PENDING |
| refresh-data | docs/component-specs/gantt/refresh-data.md | PENDING |
| state | docs/component-specs/gantt/state.md | PENDING |

Stage 01 processes one feature area at a time.
Update status to IN PROGRESS / COMPLETE per area as work proceeds.
