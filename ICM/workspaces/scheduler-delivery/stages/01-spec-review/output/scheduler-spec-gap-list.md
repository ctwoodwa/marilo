# Scheduler Spec Gap List

**Component:** MariloScheduler
**Purpose:** Track spec-vs-source gaps discovered during ICM stage `01-spec-review` for the Scheduler delivery pipeline.
**Entry point:** `.claude/orchestration/_orchestrator/inbox/w-scheduler-delivery.md`

Gap codes:
- **SA** = Spec-Ahead (spec describes surface area the source does not implement)
- **SRC** = Source-Ahead (source exposes behavior the spec does not document)
- **NM** = Name/Shape Mismatch (both sides exist but diverge in API shape, type, or naming)

---

## 2026-04-11 orchestrator wave 1 (subagent dispatch)

### Headline

**HEADLINE FINDING — SOURCE IS STUB-LEVEL.** `MariloScheduler.razor` is a single 181-line razor file with no `.razor.cs` companion, exposing **8 parameters** (`CurrentDate`, `View`, `Appointments`, `StartHour`, `EndHour`, `OnAppointmentClick`, `OnDateClick`, `OnAppointmentCreate`) and **3 inline views** (Day/Week/Month) backed by a locally-defined `SchedulerAppointment` DTO. The spec surface area (25 markdown files covering overview, data-bind, navigation, recurrence, resources, resource-grouping, toolbar, events, manual-operations, refresh-data, 7 view types, 4 templates, 3 editing topics, and WAI-ARIA accessibility) describes a full Telerik-equivalent Scheduler with generic `TItem` data binding, field mapping, child-content configuration (`<SchedulerViews>`, `<SchedulerResources>`, `<SchedulerSettings>`, `<SchedulerToolBar>`), CRUD events, recurrence handling, popup editing, and observable data. **Effectively every topic in the spec is a spec-ahead gap.** This headline shapes Wave 3 visual-parity expectations: there is no visual parity work possible until a real implementation exists — Wave 3 should be re-framed as "Wave 3 blocked on net-new implementation of Scheduler source."

Because the source is stub-level relative to the 10+ topic spec surface, gap entries below are consolidated by topic rather than enumerated parameter-by-parameter; doing one entry per missing parameter would produce hundreds of near-identical SA rows without added signal. Each topic entry flags the single root cause: the component surface the spec assumes does not exist.

### Spec-Ahead Gaps (SA)

**SA-SCHED-001 — Generic `TItem` data binding and `Data` parameter**
- Spec: `docs/component-specs/scheduler/data-bind.md`, `overview.md`
- Spec expects: `<MariloScheduler TItem="..." Data="@Items">` with field-mapping parameters (`IdField`, `TitleField`, `DescriptionField`, `StartField`, `EndField`, `IsAllDayField`, `RecurrenceRuleField`, `RecurrenceExceptionsField`, `RecurrenceIdField`), auto-detection of default property names (`Id`, `Title`, `Description`, `Start`, `End`, `IsAllDay`), built-in validation of title/start/end, and custom-model support.
- Source state: Source has a non-generic `IEnumerable<SchedulerAppointment>` `Appointments` parameter bound to a single hard-coded DTO. No generics, no field mapping, no validation.
- Gap: spec-ahead — entire data-binding contract.

**SA-SCHED-002 — Two-way `@bind-Date` / `@bind-View` naming mismatch**
- Spec: `overview.md`, `navigation.md`
- Spec expects: `@bind-Date` and `@bind-View` two-way binding on `Date` and `View` parameters.
- Source state: Source exposes `CurrentDate` / `CurrentDateChanged` and `View` / `ViewChanged`. `View` aligns; `CurrentDate` vs. `Date` is a name mismatch (see NM-SCHED-001).

**SA-SCHED-003 — `<SchedulerViews>` child configuration tag + view type components**
- Spec: `overview.md`, `views/overview.md`, `views/day.md`, `views/week.md`, `views/month.md`, `views/multiday.md`, `views/timeline.md`, `views/agenda.md`
- Spec expects: A `<SchedulerViews>` child tag containing `<SchedulerDayView>`, `<SchedulerWeekView>`, `<SchedulerMultiDayView>`, `<SchedulerMonthView>`, `<SchedulerTimelineView>`, `<SchedulerAgendaView>` components, each accepting per-view `StartTime` / `EndTime` / `NumberOfDays` / slot parameters.
- Source state: Source renders views inline via a hardcoded `if/else` on a `SchedulerView` enum (Day/Week/Month only). No child-content configuration, no MultiDay, no Timeline, no Agenda. `StartHour` / `EndHour` are flat `int` parameters on the parent, not per-view `DateTime` ranges.
- Gap: spec-ahead — entire view sub-component system.

**SA-SCHED-004 — Navigation: Today button, calendar picker, day-header click, business-hours toggle**
- Spec: `navigation.md`
- Spec expects: "Today", "Previous", "Next", calendar picker popup, clickable day headers (navigate to day view), business-hours toggle.
- Source state: Only Previous/Next buttons exist. No Today button, no calendar picker, no day-header navigation, no business-hours toggle.
- Gap: spec-ahead.

**SA-SCHED-005 — Recurrence (rule/exceptions/recurrenceId + recurrence editor components)**
- Spec: `recurrence.md`
- Spec expects: RFC5545 recurrence rules, `RecurrenceExceptions`, `RecurrenceId`, series/occurrence `EditMode` on `OnEdit`/`OnUpdate`/`OnDelete`, standalone `MariloRecurrenceFrequencyEditor`, `MariloRecurrenceIntervalEditor`, `MariloRecurrenceEditor`, `MariloRecurrenceEndEditor`, `RecurrenceRule.Parse()` / `ToString()`, Marilo.Recurrence namespace.
- Source state: None of the above exists. No recurrence fields on the DTO, no expansion logic, no recurrence editor components, no `Marilo.Recurrence` namespace.
- Gap: spec-ahead — entire recurrence feature set.

**SA-SCHED-006 — Resources (`<SchedulerResources>`, `<SchedulerResource>`, color mapping)**
- Spec: `resources.md`
- Spec expects: `<SchedulerResources>` child tag containing one or more `<SchedulerResource>` entries with `Field`, `Title`, `Data`, `TextField`, `ValueField`, `ColorField` parameters. First-matched-resource color rule. Resource dropdowns in the edit form.
- Source state: Source supports only a per-appointment ad-hoc `Color` string. No resource concept, no `<SchedulerResources>` tag, no resource-driven edit-form dropdowns.
- Gap: spec-ahead.

**SA-SCHED-007 — Resource grouping (`SchedulerGroupSettings`, horizontal/vertical orientation)**
- Spec: `resource-grouping.md`
- Spec expects: `<SchedulerGroupSettings Resources="..." Orientation="Horizontal|Vertical" />` inside `<SchedulerSettings>`, repeated view tables per resource group, cross-resource drag between groups.
- Source state: No grouping concept exists. No `SchedulerSettings`, no `SchedulerGroupSettings`.
- Gap: spec-ahead.

**SA-SCHED-008 — Toolbar (`<SchedulerToolBar>`, built-in + custom tools, spacers)**
- Spec: `toolbar.md`
- Spec expects: Configurable `<SchedulerToolBar>` tag with `SchedulerToolBarNavigationTool`, `SchedulerToolBarCalendarTool`, `SchedulerToolBarViewsTool`, `SchedulerToolBarSpacerTool`, and `SchedulerToolBarCustomTool` (RenderFragment) children. Reordering, removal, and custom tools supported.
- Source state: Source has a flat hard-coded header `<div class="mar-scheduler__header">` with 5 buttons (prev/next/day/week/month). No toolbar abstraction.
- Gap: spec-ahead — whole toolbar framework.

**SA-SCHED-009 — CUD events and edit lifecycle (`OnCreate`, `OnEdit`, `OnUpdate`, `OnDelete`, `OnCancel`, `OnModelInit`)**
- Spec: `events.md`, `editing/edit-appointments.md`
- Spec expects: `AllowCreate`, `AllowUpdate`, `AllowDelete` flags; `OnCreate`/`OnEdit`/`OnUpdate`/`OnDelete`/`OnCancel` event callbacks with strongly-typed event args (`SchedulerCreateEventArgs`, `SchedulerUpdateEventArgs`, `SchedulerDeleteEventArgs`, `SchedulerEditEventArgs` with `IsCancelled`, `EditMode`); `OnModelInit` factory for non-parameterless constructors.
- Source state: Only `OnAppointmentClick` (no args type), `OnDateClick`, and `OnAppointmentCreate` (raised nowhere in current template). No edit/update/delete lifecycle, no `AllowCreate`/`AllowUpdate`/`AllowDelete`, no event args types.
- Gap: spec-ahead — entire edit lifecycle.

**SA-SCHED-010 — Additional events: `OnItemClick`, `OnItemDoubleClick`, `OnItemContextMenu`, `ItemRender`, `OnCellRender`, `DateChanged`, `ViewChanged`**
- Spec: `events.md`
- Spec expects: Listed events with per-event argument classes.
- Source state: `ViewChanged` and `CurrentDateChanged` exist but carry raw types, not `SchedulerViewChangedEventArgs` / `SchedulerDateChangedEventArgs`. None of the others exist.
- Gap: spec-ahead.

**SA-SCHED-011 — Popup edit settings, custom edit form, delete-confirmation dialog**
- Spec: `editing/edit-popup-customization.md`, `editing/delete-confirmation-dialog.md`, `editing/edit-appointments.md`
- Spec expects: `<SchedulerSettings>` → `<SchedulerPopupEditSettings MaxHeight="..." />`; built-in popup edit form; delete-confirmation dialog; hooks for custom edit forms.
- Source state: None of these exist.
- Gap: spec-ahead.

**SA-SCHED-012 — Templates: appointment, dateheader, slot, resource-grouping-header**
- Spec: `templates/appointment.md`, `templates/dateheader.md`, `templates/slot.md`, `templates/resource-grouping-header.md`
- Spec expects: `RenderFragment<...>` templates for each surface so callers can customize appointment rendering, date header, time slot, and grouping header.
- Source state: No templates exposed. Rendering is hardcoded in the razor file.
- Gap: spec-ahead.

**SA-SCHED-013 — Manual data source operations (`OnRead`, `DataSourceRequest`, `args.Data` / `args.Total`)**
- Spec: `manual-operations.md` (note: `published: false` — still spec surface)
- Spec expects: `OnRead` event with `DataSourceRequest` carrying paging/sorting/filtering; `args.Data`/`args.Total` pattern that disables internal data handling.
- Source state: No `OnRead`, no `DataSourceRequest` integration.
- Gap: spec-ahead. (Note the spec is explicitly unpublished — escalate whether this counts as in-scope for delivery.)

**SA-SCHED-014 — Refresh: `Rebind()` and `Refresh()` methods, observable data, new-collection-reference detection**
- Spec: `refresh-data.md`, `overview.md` (Methods table)
- Spec expects: Component instance (`@ref`) exposing `Rebind()` and `Refresh()` methods; support for `ObservableCollection`; detection of new collection references.
- Source state: No public methods, no `@ref` surface, no observable-collection handling.
- Gap: spec-ahead.

**SA-SCHED-015 — Layout / dimensioning: `Height`, `Width`, `Class`, `EnableLoaderContainer`**
- Spec: `overview.md` parameters table
- Spec expects: `Height`, `Width`, `Class` string parameters and `EnableLoaderContainer` bool (loader after 600 ms).
- Source state: None of these parameters exist. `CombineClasses("mar-scheduler")` is used but there is no `Class` parameter surface for consumers.
- Gap: spec-ahead.

**SA-SCHED-016 — Accessibility / WAI-ARIA**
- Spec: `accessibility/wai-aria-support.md`
- Spec expects: Documented ARIA roles, keyboard navigation, screen-reader support.
- Source state: No ARIA roles, no keyboard navigation handling, no screen-reader affordances in the markup.
- Gap: spec-ahead.

### Name/Shape Mismatches (NM)

**NM-SCHED-001 — `Date` (spec) vs `CurrentDate` (source)**
- Spec parameter name: `Date` with `@bind-Date` and `DateChanged` event.
- Source parameter name: `CurrentDate` with `CurrentDateChanged`.
- Impact: Any consumer copy-pasting from spec will not compile. Decision required in later wave: rename source to `Date` (breaking) or update spec to `CurrentDate` (spec-change).

**NM-SCHED-002 — `StartTime` / `EndTime` per-view `DateTime` (spec) vs `StartHour` / `EndHour` flat `int` (source)**
- Spec: `SchedulerDayView StartTime="..." EndTime="..."` with only the time portion of a `DateTime` being significant; values per-view.
- Source: `StartHour` / `EndHour` as `int` hour-of-day, parent-level only, shared across all views.
- Impact: Shape mismatch — parent-level coarse-grained vs. per-view DateTime-based. Significant design-reconciliation decision needed (architecture-level — escalate at design time, not now).

**NM-SCHED-003 — `SchedulerAppointment` local DTO (source) vs generic `TItem` appointment model (spec)**
- Spec: Consumer-defined model via `TItem`, with field mapping.
- Source: Hardcoded `SchedulerAppointment` class (location unconfirmed — likely `src/Marilo.Components/DataDisplay/SchedulerAppointment.cs` or inline; not inspected this wave).
- Impact: Architecturally different component shape (closed-world DTO vs. open generic). This is a **public-API / architecture-level concern** — flag for orchestrator; worker cannot resolve.

### Source-Ahead Gaps (SRC)

**SRC-SCHED-001 — `OnAppointmentCreate` event parameter is exposed but never raised**
- Source: `[Parameter] public EventCallback<SchedulerAppointment> OnAppointmentCreate { get; set; }` is declared but no invocation site exists in the current razor template.
- Spec: `OnCreate` is documented but with different args type; spec does not describe the current `OnAppointmentCreate` shape.
- Impact: Dead parameter. Either wire it or remove it. Flag only — do not fix this wave.

**SRC-SCHED-002 — `OnAppointmentClick` / `OnDateClick` exposed without matching spec entry**
- Source: Carries raw `SchedulerAppointment` and `DateTime` payloads.
- Spec: Spec describes `OnItemClick` (with event args) but not `OnAppointmentClick` with raw payloads. These are two different naming conventions.
- Gap: source-ahead on naming; functionally overlaps with spec's `OnItemClick`.

### Meta findings (not gap-coded)

1. **Source file topology:** Only one scheduler source file exists in `src/Marilo.Components/DataDisplay/` (the `.razor`). No `.razor.cs` companion, no partial-class file, no helpers folder. The `SchedulerAppointment` and `SchedulerView` types referenced by the razor are defined elsewhere (not inspected this wave) — worth confirming location in Wave 2.
2. **Spec files discovered beyond the original 10 enumerated in the task:** the scheduler spec folder contains 25 files, not 10. The extras are: `editing/delete-confirmation-dialog.md`, `editing/edit-appointments.md`, `editing/edit-popup-customization.md`, `templates/appointment.md`, `templates/dateheader.md`, `templates/resource-grouping-header.md`, `templates/slot.md`, `views/agenda.md`, `views/day.md`, `views/month.md`, `views/multiday.md`, `views/overview.md`, `views/timeline.md`, `views/week.md`, and `accessibility/wai-aria-support.md`. Worth flagging to the orchestrator: the inbox's "~10 files" estimate understated real spec surface by 2.5×. The visual-parity bar is correspondingly larger.
3. **`manual-operations.md` is `published: false`** in its front matter — treat as draft spec; orchestrator should decide whether it is in-scope for delivery.
4. **No sync-area touches needed this wave.** Worker's declared `required_sync_areas` is `["spec"]`, but this audit does not modify any spec — it only reports gaps. No source, test, demo, doc, or gap-plan edits are required or performed.

### Wave 1 verdict

Source implementation does NOT match spec surface area. Source is a **stub-level calendar prototype** (~181 lines, 3 views, 8 parameters, no edit lifecycle, no recurrence, no resources, no templates, no toolbar, no accessibility) against a spec surface that assumes a full Telerik-parity Scheduler. **Wave 3 visual-parity work is not possible against this source as-is** — Wave 3 must either (a) be deferred pending a net-new Scheduler implementation phase, or (b) be re-scoped to "visual parity of the 3 existing views (Day/Week/Month) at the `mar-scheduler__*` BEM level, acknowledging that all other spec features are blocked." This is an orchestrator decision.
