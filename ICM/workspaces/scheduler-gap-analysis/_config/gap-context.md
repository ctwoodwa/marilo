# Gap Context -- MariloScheduler

## Target Component

| Field | Value |
|-------|-------|
| TARGET_COMPONENT | MariloScheduler |
| Solution | Marilo.Components |
| Framework | .NET 10 / Blazor |
| Category | Scheduling |

## Artifact Paths

| Field | Value |
|-------|-------|
| TARGET_PROJECT_PATH | src/Marilo.Components/ |
| TEST_PROJECT_PATH | tests/Marilo.Components.Tests/ |
| OWNER_WORKSPACE | scheduler-delivery |

## Gap Source

- **GAP_SOURCE:** `assess` (fresh intake)
- **Source file:** `stages/01-intake/output/gap-scheduler-inventory.md`
- **Pre-prioritization research:** `stages/01-intake/output/pre-prioritization-research.md` (2026-04-10 — resolves decisions #2 and #4 with codebase evidence: 0 runtime consumers of old parameter names, and built-in enum-based edit pipeline is the established grid-family convention)
- **Source description:** Assessed current MariloScheduler source against the full spec and demo:
  - Component: `src/Marilo.Components/DataDisplay/MariloScheduler.razor` (181 lines)
  - Spec: `docs/component-specs/scheduler/` (25 markdown files across 6 sub-areas)
  - Demo: `samples/Marilo.Demo/Pages/Components/Scheduler/Overview.razor`

## Resolution Scope

- **Total gaps:** 32
- **By severity:** 5 Critical / 13 High / 9 Medium / 5 Low
- **Scope classification:** `systematic` (cross-cutting: DataBinding, Views, Editing, Recurrence, Resources, Templates, Events, Navigation/Toolbar, Accessibility)
- **Active phase:** 01-intake complete; awaiting Stage 02 prioritization
- **Critical path:** GAP-SCHEDULER-001 (generic `TItem` rewrite) → GAP-SCHEDULER-004 (child-tag architecture) → everything else
- **Execution model recommendation:** Phased subagent-driven rewrite mirroring the MariloGantt full-rewrite precedent (phases A–J documented in inventory)

## Resolution Tracking

- **01-intake:** ✅ Complete — 2026-04-10 (assess mode)
- **02-prioritize:** Not started
- **03-resolution-design:** Not started
- **04-remediation-plan:** Not started
- **05-implement:** Not started
- **06-validate:** Not started

## Open Human Decisions (before Stage 02 can proceed)

**Evidence-resolved (2026-04-10 via `pre-prioritization-research.md` — pending human ratification):**

- ~~2. Backward-compat horizon for `[Obsolete]` aliases~~ → **Recommend: zero horizon, break cleanly.** Evidence: 0 external runtime consumers pass `Appointments=`, `CurrentDate=`, `StartHour=`, or `EndHour=` anywhere in the repo. Demo passes no data parameters at all. Unit tests reference nothing. Visual-parity test just needs a one-command Playwright re-baseline. No [Obsolete] phase worth the ceremony.
- ~~4. Editing popup ownership~~ → **Recommend: built-in `SchedulerEditMode` enum + `SchedulerEditPopup` sub-component.** Evidence: every grid-family component in Marilo (DataGrid, DataSheet, FileManager, Gantt) uses a built-in enum-based edit mode. Zero precedent for consumer-provided `EditTemplate`. The alternative would make Scheduler the odd component out.

**Still open (require genuine human/external judgment):**

- **Decision #1 — Branch strategy:** generic rewrite on `workInProgress` or dedicated branch (cf. `gantt-rewrite`)?
- **Decision #3 — RRULE library:** approve `Ical.Net` (MIT) as the recurrence parser, or defer recurrence entirely? (Requires external NuGet vetting — license, API surface, bundle size.)
- **Decision #5 — Timeline view + Resources coupling:** deliver Timeline view in Phase C (standalone stub) or gate on Phase D (Resources) for a meaningful first demo?

## Test Coverage Rollup

| Batch | Tests | Passing |
|-------|-------|---------|
| (none yet) | 0 | -- |

## Constraints

- No Telerik dependencies
- License: MIT / Apache-2.0 / BSD only
- Must inherit from MariloComponentBase
- Must use CssProvider pattern (no hardcoded CSS classes)
