# Scheduler Example UX Gap List

**Component:** MariloScheduler
**Stage:** 02-example-ux
**Purpose:** Track demo-vs-spec coverage gaps discovered during ICM stage `02-example-ux` for the Scheduler delivery pipeline.
**Wave 1 reference:** `ICM/workspaces/scheduler-delivery/stages/01-spec-review/output/scheduler-spec-gap-list.md`

Coverage codes:
- **COV** = Covered (demo exercises the spec topic against real source API)
- **PAR** = Partial (demo exists but only exercises a stub-subset of the spec topic)
- **BLK** = Missing — blocked-by-source (spec topic requires source API that does not exist)
- **DEM** = Missing — demo only (source supports it but no demo wired)

---

## 2026-04-11 orchestrator wave 2 (subagent dispatch)

### Headline

**HEADLINE FINDING — DEMO SURFACE IS A SINGLE 16-LINE FILE.** The entire Scheduler demo footprint under `samples/Marilo.Demo/Pages/**` is exactly one file: `samples/Marilo.Demo/Pages/Components/Scheduler/Overview.razor`. That file renders a single `<MariloScheduler Style="height:500px;" />` with **no `Appointments` data**, **no view switching**, **no event handlers**, and **no parameter exercises** beyond inline style. It is below "stub demo" — it is effectively a placeholder confirming the component compiles and renders an empty chrome. Of the 25 Scheduler spec topics, **exactly zero** have a purposeful demo. The single existing demo is a Partial at best against `overview.md` and irrelevant to every other topic.

The demo gap map is therefore almost entirely `BLK` (blocked-by-source) because Wave 1 confirmed the source itself lacks the APIs those topics would need to demo against. A few entries are `DEM` (demo-only) — specifically the 3 existing hardcoded views (Day/Week/Month) and the hardcoded 5-button toolbar — because the source DOES support those at a stub level but the demo does not exercise them at all.

Because the demo surface is a single file, the remediation at Wave 4 is net-new demo authoring, not demo updates. This does not block this inventory — it is the inventory's conclusion.

### Demo inventory

| # | File | LOC | Exercises | Notes |
|---|------|-----|-----------|-------|
| 1 | `samples/Marilo.Demo/Pages/Components/Scheduler/Overview.razor` | 16 | `<MariloScheduler Style="..." />` with no other parameters | Does not pass `Appointments`, does not switch views, does not handle events, does not toggle StartHour/EndHour. Just confirms the component renders. |

**Related but out-of-scope:** `samples/Marilo.Demo/Pages/Components/AllocationScheduler/AllocationSchedulerDemo.razor` references `MariloAllocationScheduler`, not `MariloScheduler`. That is a separate component with its own delivery workspace (`allocation-scheduler-delivery`) and is not counted toward Scheduler coverage.

**Total Scheduler-specific demo files:** 1.
**Total Scheduler demo LOC:** 16.
**Total scenario coverage:** 1 trivial render scenario.

### Coverage matrix (25 spec topics)

| Code | Spec topic | Spec file | Coverage | Notes |
|------|-----------|-----------|----------|-------|
| EUX-SCHED-001 | Overview | `scheduler/overview.md` | **PAR** | One demo file exists but only exercises a bare `<MariloScheduler Style="..."/>` tag. Does not demonstrate any overview-level parameter from the spec's parameters table (no `Data`, `Date`, `View`, `Height`, `Width`, `Class`, etc.). |
| EUX-SCHED-002 | Data binding (generic `TItem`, field mapping) | `scheduler/data-bind.md` | **BLK** | Source has no generic `TItem`, no field mapping, no `Data` parameter. Cannot demo what does not exist. See Wave 1 SA-SCHED-001. |
| EUX-SCHED-003 | Navigation (Today, calendar picker, day-header click, business-hours toggle) | `scheduler/navigation.md` | **BLK** | Source has only Previous/Next. No Today, no calendar picker, no day-header navigation, no business-hours toggle. See Wave 1 SA-SCHED-004. |
| EUX-SCHED-004 | Recurrence (RFC5545, exceptions, recurrence editors, `Marilo.Recurrence` namespace) | `scheduler/recurrence.md` | **BLK** | No recurrence support exists in source. No recurrence editor components exist. See Wave 1 SA-SCHED-005. |
| EUX-SCHED-005 | Resources (`<SchedulerResources>`, `<SchedulerResource>`, color mapping) | `scheduler/resources.md` | **BLK** | Source has only a per-appointment ad-hoc `Color` string, no resource concept. See Wave 1 SA-SCHED-006. |
| EUX-SCHED-006 | Resource grouping (`SchedulerGroupSettings`, horizontal/vertical) | `scheduler/resource-grouping.md` | **BLK** | No grouping concept in source. See Wave 1 SA-SCHED-007. |
| EUX-SCHED-007 | Toolbar (`<SchedulerToolBar>` with built-in + custom tools) | `scheduler/toolbar.md` | **BLK** (with stub-level **DEM** opportunity) | Source has a hardcoded 5-button header but no `<SchedulerToolBar>` abstraction. The existing hardcoded buttons could be shown in a demo, but the spec's toolbar framework is entirely unimplemented. See Wave 1 SA-SCHED-008. |
| EUX-SCHED-008 | Edit lifecycle events (`OnCreate`/`OnEdit`/`OnUpdate`/`OnDelete`/`OnCancel`/`OnModelInit`, `AllowCreate`/`AllowUpdate`/`AllowDelete`) | `scheduler/events.md`, `scheduler/editing/edit-appointments.md` | **BLK** | Only `OnAppointmentClick`, `OnDateClick`, and dead `OnAppointmentCreate` exist. No lifecycle, no flags. See Wave 1 SA-SCHED-009. |
| EUX-SCHED-009 | Additional events (`OnItemClick`, `OnItemDoubleClick`, `OnItemContextMenu`, `ItemRender`, `OnCellRender`, typed `DateChanged`/`ViewChanged` args) | `scheduler/events.md` | **BLK** | Existing `CurrentDateChanged`/`ViewChanged` carry raw types, not the spec's arg classes; other events do not exist. See Wave 1 SA-SCHED-010. |
| EUX-SCHED-010 | Popup edit settings, custom edit form | `scheduler/editing/edit-popup-customization.md` | **BLK** | No `<SchedulerSettings>`, no `<SchedulerPopupEditSettings>`, no edit form. See Wave 1 SA-SCHED-011. |
| EUX-SCHED-011 | Delete-confirmation dialog | `scheduler/editing/delete-confirmation-dialog.md` | **BLK** | No edit lifecycle in source → no delete confirmation. See Wave 1 SA-SCHED-011. |
| EUX-SCHED-012 | Edit appointments (popup form + event flow) | `scheduler/editing/edit-appointments.md` | **BLK** | Same root cause as EUX-SCHED-010/011. |
| EUX-SCHED-013 | Appointment template (`RenderFragment<...>`) | `scheduler/templates/appointment.md` | **BLK** | No template RenderFragments exposed. See Wave 1 SA-SCHED-012. |
| EUX-SCHED-014 | Date-header template | `scheduler/templates/dateheader.md` | **BLK** | Same. |
| EUX-SCHED-015 | Slot template | `scheduler/templates/slot.md` | **BLK** | Same. |
| EUX-SCHED-016 | Resource-grouping header template | `scheduler/templates/resource-grouping-header.md` | **BLK** | Same, and also depends on resource grouping which is blocked. |
| EUX-SCHED-017 | Views — overview page (view framework + `<SchedulerViews>` child tag) | `scheduler/views/overview.md` | **BLK** | No `<SchedulerViews>` child-content configuration exists. View is a flat enum parameter. See Wave 1 SA-SCHED-003. |
| EUX-SCHED-018 | Day view (`<SchedulerDayView>` with per-view `StartTime`/`EndTime`) | `scheduler/views/day.md` | **BLK** for the spec shape / **DEM** for the stub shape | The stub supports a Day view via `View=SchedulerView.Day` with parent-level `StartHour`/`EndHour`. The existing demo never exercises it. Spec's per-view `StartTime`/`EndTime` on a `<SchedulerDayView>` child tag is blocked by source (NM-SCHED-002, SA-SCHED-003). A stub-level Day demo would be a **DEM** gap. |
| EUX-SCHED-019 | Week view | `scheduler/views/week.md` | **BLK** for spec shape / **DEM** for stub shape | Same reasoning as Day. Stub supports `View=SchedulerView.Week`; demo does not exercise it. |
| EUX-SCHED-020 | Month view | `scheduler/views/month.md` | **BLK** for spec shape / **DEM** for stub shape | Same reasoning. |
| EUX-SCHED-021 | MultiDay view | `scheduler/views/multiday.md` | **BLK** | Not in source at all. |
| EUX-SCHED-022 | Timeline view | `scheduler/views/timeline.md` | **BLK** | Not in source at all. |
| EUX-SCHED-023 | Agenda view | `scheduler/views/agenda.md` | **BLK** | Not in source at all. |
| EUX-SCHED-024 | Manual operations (`OnRead`, `DataSourceRequest`, `args.Data`/`args.Total`) | `scheduler/manual-operations.md` | **BLK** | No `OnRead`/`DataSourceRequest`. Spec is `published: false` — escalation pending from Wave 1 on whether this is in-scope. Not resolved this wave. See Wave 1 SA-SCHED-013. |
| EUX-SCHED-025 | Accessibility (WAI-ARIA, keyboard, SR) | `scheduler/accessibility/wai-aria-support.md` | **BLK** | No ARIA roles, no keyboard handling. See Wave 1 SA-SCHED-016. |

**Bonus spec-surface entries not in the 25 above but flagged in Wave 1:**

| Code | Spec topic | Spec file | Coverage | Notes |
|------|-----------|-----------|----------|-------|
| EUX-SCHED-026 | Refresh: `Rebind()`/`Refresh()` methods, `@ref`, observable data | `scheduler/refresh-data.md` + overview Methods table | **BLK** | No `@ref`-able methods exist on the stub. See Wave 1 SA-SCHED-014. |
| EUX-SCHED-027 | Layout/dimensioning (`Height`, `Width`, `Class`, `EnableLoaderContainer`) | `scheduler/overview.md` parameters table | **PAR** | Existing demo passes `Style="height:500px;"` — which is inline style, not the spec's `Height` parameter. Spec parameters `Height`/`Width`/`Class`/`EnableLoaderContainer` do not exist on source (SA-SCHED-015), so the demo reaches the closest approximation (inline `Style`). Partial credit only. |

### Totals

- **25 primary spec topics.**
- **COV (Covered):** 0
- **PAR (Partial):** 1 (EUX-SCHED-001 Overview — only because the demo renders *something* under the overview page)
- **BLK (blocked by source):** 23 (EUX-SCHED-002 through 025 except 018/019/020 which are BLK-or-DEM dual-classified)
- **DEM (demo only):** 0 strict demo-only gaps, but **3 stub-level DEM opportunities** against the existing 3 hardcoded views (Day/Week/Month, EUX-SCHED-018/019/020) that the source already supports and the demo fails to exercise. These are the only topics where a worker could add demo-only value without needing source changes first.
- **Also:** EUX-SCHED-007 Toolbar is BLK for the spec framework but has a stub-level DEM opportunity if someone wanted to show the hardcoded 5-button header in action with view switching.

### Top 3 findings

1. **Scheduler demo surface is a single 16-line placeholder.** Every other Marilo component in the repo has at least one non-trivial demo with real data binding; Scheduler has a tag-only render. The Wave 1 headline was "source is stub" — the Wave 2 headline is "demo is sub-stub." Both halves of the delivery pipeline (source and demo) need net-new work before visual parity is meaningful.
2. **Only 3 demo-only (DEM) opportunities exist and all three are stub-level.** A worker can add non-blocked demo value TODAY for: (a) Day view with `Appointments` data, (b) Week view with `Appointments` data, (c) Month view with `Appointments` data — all against the existing hardcoded `SchedulerView` enum and local `SchedulerAppointment` DTO. Every other spec topic requires source-level work first (`TItem`, `<SchedulerViews>`, resources, toolbar framework, templates, edit lifecycle, recurrence, accessibility). This is the entire Wave 4 demo-only backlog: 3 scenarios.
3. **Visual parity (Wave 3) has nothing to compare.** With one demo file rendering an empty calendar chrome, there is no meaningful visual parity surface. Wave 3's re-framing (flagged in Wave 1 for user decision) is confirmed by Wave 2: visual parity should either (a) defer until net-new source+demo work lands, or (b) re-scope to "parity of the 3 stub views against FluentUI Calendar/Scheduler at the BEM level," but option (b) still requires the 3 DEM demos above to exist first or there is literally nothing populated to screenshot.

### Cross-reference against Wave 1 gaps

Every EUX entry marked **BLK** above maps 1:1 to a Wave 1 SA-SCHED-### gap. There are no NEW gap classes introduced by Wave 2 — Wave 2 merely confirms the demo side of each Wave 1 source-ahead gap is also empty. Wave 1 NM-SCHED-001, NM-SCHED-003, the Wave 3 scope reframe, and the `manual-operations.md` published-false question are **not** re-touched this wave (per scope).

### Sync-area impact

- `required_sync_areas` for this worker is `["demo"]`.
- This wave only writes an audit document; no demo source files are edited.
- When Wave 4 acts on the 3 DEM demo opportunities above, it must also sync: the 3 demo pages themselves, their code snippets, and (if any navigation index exists) the demo shell. That is a Wave 4 concern, not this wave's.

### Wave 2 verdict

Demo coverage against the 25 Scheduler spec topics is **0 Covered / 1 Partial / 23 Blocked / 3 stub-level demo-only opportunities**. The single existing demo file (`Overview.razor`, 16 LOC) does not exercise any non-trivial parameter. The source stub's 3 hardcoded views are unused by the demo. This closes the 02-example-ux inventory. Recommendation for Wave 4 demo-side backlog: author Day/Week/Month demos against the existing `SchedulerAppointment` DTO to populate the 3 DEM slots; everything else waits on source-level work under `scheduler-gap-analysis`.
