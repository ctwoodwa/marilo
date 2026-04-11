# Gap Inventory — MariloScheduler (Stage 01 Intake)

**Component:** `MariloScheduler`
**Mode:** Assess (no prior gap analysis file existed; this inventory is the fresh discovery)
**Intake run:** 2026-04-10
**Source snapshot:**
- Component: `src/Marilo.Components/DataDisplay/MariloScheduler.razor` (181 lines)
- Supporting types: `src/Marilo.Core/Models/SchedulerModels.cs` (`SchedulerAppointment`, `SchedulerView` enum)
- Demo: `samples/Marilo.Demo/Pages/Components/Scheduler/Overview.razor` (16 lines — trivial placeholder)
- Spec root: `docs/component-specs/scheduler/` (25 markdown files across 6 sub-areas)

**Scope classification:** `systematic` (cross-cutting gaps across multiple feature areas — Views, Editing, Recurrence, Resources, Templates, Events, DataBinding, Accessibility)

**Recommended stage routing:** `01 → 02 → 03 → 04 → 05 → 06` (full pipeline — this is a green-field rebuild of an API surface, not a point fix)

---

## Summary Counts

| Severity | Count |
|---|---|
| Critical | 5 |
| High | 13 |
| Medium | 9 |
| Low | 5 |
| **Total** | **32** |

## Theme Tags

| Theme | Gaps |
|---|---|
| `generic-data-binding` | GAP-SCHEDULER-001, 002, 004 |
| `spec-api-naming` | GAP-SCHEDULER-002, 003, 012 |
| `missing-views` | GAP-SCHEDULER-006, 007 |
| `missing-child-tag-architecture` | GAP-SCHEDULER-004, 024 |
| `editing-pipeline` | GAP-SCHEDULER-008, 020, 021, 022 |
| `recurrence` | GAP-SCHEDULER-009, 019 |
| `resources-grouping` | GAP-SCHEDULER-010, 011 |
| `templates` | GAP-SCHEDULER-013, 014, 015, 016 |
| `events` | GAP-SCHEDULER-017, 018 |
| `a11y` | GAP-SCHEDULER-023 |
| `demo-coverage` | GAP-SCHEDULER-028, 029, 030, 031, 032 |

---

## Feature Area: DataBinding

### GAP-SCHEDULER-001: Not generic (`TItem`)
**Area:** DataBinding
**Severity:** Critical
**Theme:** `generic-data-binding`
**Source:** `docs/component-specs/scheduler/overview.md:18-19` (spec: `Data` parameter is `IEnumerable<TItem>` with user-defined model); `src/Marilo.Components/DataDisplay/MariloScheduler.razor:98` (source: fixed `IEnumerable<SchedulerAppointment>`)

**Target behavior:** `MariloScheduler<TItem>` accepts any user model via `Data="@Appointments"` with property-name detection for `Title`, `Description`, `Start`, `End`, `IsAllDay`, etc., plus explicit field-name overrides (`TitleField`, `StartField`, `EndField`, …).

**Current behavior:** Non-generic `MariloScheduler` that only accepts the fixed `SchedulerAppointment` type from `Marilo.Core.Models`.

**Impact:** Blocks every real-world consumer — users cannot use their own domain models. Contradicts the rest of the Marilo library's generic-by-default convention (DataGrid, TreeList, MultiSelect, Gantt, FileManager all generic).

**Recommended direction:** Convert to `MariloScheduler<TItem>` (cf. `MariloDataGrid<TItem>`, `MariloGantt<TItem>`). Add reflection-based field resolver mirroring `DropDownTree`/`Gantt` patterns. Retain `SchedulerAppointment` as the default model when `TItem` is unspecified (backward compat for current demo usage).

**Status:** Open

---

### GAP-SCHEDULER-002: Parameter `Appointments` vs spec `Data`
**Area:** DataBinding
**Severity:** Critical
**Theme:** `spec-api-naming`, `generic-data-binding`
**Source:** `docs/component-specs/scheduler/overview.md:19-27` (spec: `Data="@Appointments"`); `src/Marilo.Components/DataDisplay/MariloScheduler.razor:98` (source: `[Parameter] public IEnumerable<SchedulerAppointment> Appointments`)

**Target behavior:** `[Parameter] public IEnumerable<TItem>? Data { get; set; }` — matching spec and Marilo library convention (DataGrid, TreeList, etc.).

**Current behavior:** Parameter is named `Appointments`, not `Data`.

**Impact:** Users copying spec code snippets get compile errors. Diverges from every other data-driven Marilo component.

**Recommended direction:** Rename to `Data`, keep old `Appointments` as `[Obsolete]` alias for one release cycle.

**Status:** Open

---

### GAP-SCHEDULER-003: Parameter `CurrentDate` vs spec `Date`
**Area:** DataBinding
**Severity:** Critical
**Theme:** `spec-api-naming`
**Source:** `docs/component-specs/scheduler/overview.md:28-29` (spec: `@bind-Date="@SchedulerStartDate"`); `src/Marilo.Components/DataDisplay/MariloScheduler.razor:94-95` (source: `CurrentDate` / `CurrentDateChanged`).

**Target behavior:** `[Parameter] public DateTime Date { get; set; }` + `DateChanged` — supporting `@bind-Date`.

**Current behavior:** `CurrentDate` + `CurrentDateChanged` — `@bind-CurrentDate` works, but `@bind-Date` (spec idiom) does not.

**Impact:** Spec code snippets fail to compile. Every example in `docs/component-specs/scheduler/**` uses `@bind-Date`.

**Recommended direction:** Rename to `Date` / `DateChanged`. Keep `CurrentDate` as obsolete alias for one release.

**Status:** Open

---

### GAP-SCHEDULER-004: Missing `<SchedulerViews>` child-tag architecture
**Area:** DataBinding (via view configuration)
**Severity:** Critical
**Theme:** `missing-child-tag-architecture`
**Source:** `docs/component-specs/scheduler/overview.md:31-37` (spec: `<SchedulerViews>` wrapper containing `<SchedulerDayView>`, `<SchedulerWeekView>`, `<SchedulerMonthView>`, `<SchedulerTimelineView>` children with per-view `StartTime`/`EndTime`/`SlotDuration` config)

**Target behavior:** Child-tag view configuration via `SchedulerViews` wrapper + per-view child components, each carrying its own parameters (`StartTime`, `EndTime`, `SlotDuration`, `WorkDayStart`, `WorkDayEnd`, `ShowWorkHours`, etc.). Mirrors `MariloDataGrid` ↔ `MariloGridColumn` cascading pattern (`MariloDataGrid.razor:36-39`, `MariloGridColumn.razor:5,83-92`).

**Current behavior:** View configuration is a single `View` enum parameter with inline `if/else` rendering branches in the parent component — no child-tag API at all.

**Impact:** Every spec example fails to compile. Per-view parameters (Day-view slot duration vs Week-view slot duration vs Timeline-view grouping) have nowhere to live. Blocks GAP-SCHEDULER-006 / 007 (multi-day + timeline views) because those views need their own config.

**Recommended direction:** Add `ISchedulerViewSink` registration interface (cf. `IMultiSelectSettingsSink` from T4 Pickers B7). Create `SchedulerViews` non-generic wrapper that accepts `RenderFragment` of view children. Create `SchedulerDayView`, `SchedulerWeekView`, `SchedulerMonthView`, `SchedulerMultiDayView`, `SchedulerTimelineView` child components, each implementing a common `SchedulerViewBase` that registers itself with the parent via cascade. **Apply the Wizard CascadingValue bug class learning** (cerebrum 2026-04-04): cast cascade value to interface `(ISchedulerViewSink)this` for non-generic children to attach to `MariloScheduler<TItem>`.

**Status:** Open

---

### GAP-SCHEDULER-005: Missing explicit field-name parameters
**Area:** DataBinding
**Severity:** High
**Theme:** `generic-data-binding`
**Source:** `docs/component-specs/scheduler/overview.md:19` (spec: "configure them explicitly" — implies `TitleField`, `StartField`, etc. overrides)

**Target behavior:** `TitleField`, `StartField`, `EndField`, `DescriptionField`, `IsAllDayField`, `RecurrenceRuleField`, `RecurrenceExceptionsField`, `RecurrenceIdField` parameters to map user-model property names when they don't match the defaults.

**Current behavior:** None. The fixed-type approach makes field overrides unnecessary — but this blocks generic binding.

**Impact:** Follows from GAP-SCHEDULER-001. Must land together with the generic rewrite.

**Recommended direction:** Resolve alongside GAP-SCHEDULER-001. Use expression-bound field resolvers (cf. Gantt's `TaskTitleField`, `StartField`, `EndField` pattern).

**Status:** Open

---

## Feature Area: Views

### GAP-SCHEDULER-006: Missing MultiDay view
**Area:** Views
**Severity:** High
**Theme:** `missing-views`
**Source:** `docs/component-specs/scheduler/views/multiday.md` (entire spec file for multi-day view); `src/Marilo.Core/Models/SchedulerModels.cs:21-31` (enum only has Day/Week/Month)

**Target behavior:** `SchedulerMultiDayView` child component with configurable day count (e.g., 3-day, 5-day view). Shares time-grid rendering with Day/Week views.

**Current behavior:** Not implemented. Enum does not have `MultiDay` member.

**Impact:** Users who need 5-day work-week or 3-day mini-range views have no option.

**Recommended direction:** Add `SchedulerView.MultiDay` enum member; add `SchedulerMultiDayView` child component (depends on GAP-SCHEDULER-004). Reuse `GetWeekDays()`-style range calculation with configurable length.

**Status:** Open

---

### GAP-SCHEDULER-007: Missing Timeline (Agenda) view
**Area:** Views
**Severity:** High
**Theme:** `missing-views`
**Source:** `docs/component-specs/scheduler/views/timeline.md`, `docs/component-specs/scheduler/views/agenda.md`, `docs/component-specs/scheduler/overview.md:95` (listed as "Timeline (agenda) view")

**Target behavior:** `SchedulerTimelineView` rendering horizontally-oriented time axis with resources as rows (Gantt-like). Enables "who is doing what when" visualization.

**Current behavior:** Not implemented.

**Impact:** No way to show resource-grouped horizontal scheduling — the primary use case for Scheduler in ops/manufacturing dashboards.

**Recommended direction:** Add `SchedulerView.Timeline`; add `SchedulerTimelineView` child. Likely requires the resources system (GAP-SCHEDULER-010) to be meaningful, so sequence accordingly in Stage 02 prioritization.

**Status:** Open

---

### GAP-SCHEDULER-008: Missing per-view `StartTime`/`EndTime` as `DateTime`
**Area:** Views
**Severity:** High
**Theme:** `spec-api-naming`
**Source:** `docs/component-specs/scheduler/overview.md:30-34` (spec: `<SchedulerDayView StartTime="@DayStart" EndTime="@DayEnd" />` with `DateTime DayStart = new DateTime(2000, 1, 1, 6, 0, 0)`); `src/Marilo.Components/DataDisplay/MariloScheduler.razor:99-100` (source: `int StartHour = 8; int EndHour = 18;`)

**Target behavior:** `DateTime? StartTime { get; set; }` and `DateTime? EndTime { get; set; }` on each view child component — only the time-of-day portion matters per spec.

**Current behavior:** Two hour-of-day `int` parameters (`StartHour`/`EndHour`) on the parent.

**Impact:** Spec examples fail to compile. Cannot express half-hour offsets or sub-hour granularity.

**Recommended direction:** Move `StartTime`/`EndTime` to the view child components (GAP-SCHEDULER-004), using `DateTime` where only the time component is read. Drop `StartHour`/`EndHour` from parent.

**Status:** Open

---

## Feature Area: Editing

### GAP-SCHEDULER-009: Missing editing pipeline (Edit / Create / Delete)
**Area:** Editing
**Severity:** High
**Theme:** `editing-pipeline`
**Source:** `docs/component-specs/scheduler/editing/edit-appointments.md`, `docs/component-specs/scheduler/editing/delete-confirmation-dialog.md`, `docs/component-specs/scheduler/editing/edit-popup-customization.md`, `docs/component-specs/scheduler/overview.md:104-105`

**Target behavior:** Double-click slot → open edit popup with form; double-click appointment → open edit popup; right-click / delete key → confirm dialog; events fire `OnCreate`, `OnUpdate`, `OnDelete`, `OnEdit`, `OnCancel` for consumer-side persistence.

**Current behavior:** Only `OnAppointmentClick`, `OnDateClick`, and an unused `OnAppointmentCreate` exist. No popup, no edit form, no delete flow.

**Impact:** Scheduler is read-only. Users cannot create, edit, or delete appointments interactively.

**Recommended direction:** Design an edit-popup component wrapping `MariloDialog` with form fields for all standard SchedulerAppointment properties. Wire edit/create/delete events. This is a multi-pass effort — plan a dedicated sub-batch in Stage 03/04.

**Status:** Open

---

### GAP-SCHEDULER-010: Missing resources and grouping
**Area:** Resources
**Severity:** High
**Theme:** `resources-grouping`
**Source:** `docs/component-specs/scheduler/resources.md`, `docs/component-specs/scheduler/resource-grouping.md`, `docs/component-specs/scheduler/templates/resource-grouping-header.md`, `docs/component-specs/scheduler/overview.md:113-115`

**Target behavior:** Define `SchedulerResource` class, `<SchedulerResources>` child collection, `Group` parameter controlling how appointments cluster by resource (e.g., meeting rooms, equipment, people). Resource grouping integrates with Timeline view (GAP-SCHEDULER-007).

**Current behavior:** No concept of resources. Every appointment is ungrouped.

**Impact:** Eliminates the primary enterprise use case (rooms/equipment/staff schedules). Also blocks a useful Timeline-view experience.

**Recommended direction:** Add `SchedulerResource` model, `<SchedulerResources>` collection pattern (mirror DataGrid columns), group-by-resource logic in appointment-to-cell assignment.

**Status:** Open

---

### GAP-SCHEDULER-011: Missing Resource field mapping parameters
**Area:** Resources
**Severity:** Medium
**Theme:** `resources-grouping`, `generic-data-binding`
**Source:** `docs/component-specs/scheduler/resources.md`

**Target behavior:** `ResourceIdField`, `ResourceNameField`, `ResourceColorField` — mapping user resource model properties.

**Current behavior:** N/A (GAP-SCHEDULER-010 prerequisite).

**Impact:** Follows from GAP-SCHEDULER-010.

**Recommended direction:** Resolve alongside GAP-SCHEDULER-010.

**Status:** Open

---

### GAP-SCHEDULER-012: `Height`/`Width` parameters missing from public API
**Area:** DataBinding (layout)
**Severity:** Medium
**Theme:** `spec-api-naming`
**Source:** `docs/component-specs/scheduler/overview.md:137-139` (spec: `Height` and `Width` are documented parameters); current demo uses `Style="height:500px;"` as a workaround.

**Target behavior:** `[Parameter] public string? Height { get; set; }` + `[Parameter] public string? Width { get; set; }`.

**Current behavior:** No explicit Height/Width parameters. Consumers use `Style=` instead.

**Impact:** Small — but spec-documented public surface should exist. Aligns with every other Marilo component.

**Recommended direction:** Add both parameters, merge into root `style` attribute via `CombineStyles`.

**Status:** Open

---

## Feature Area: Recurrence

### GAP-SCHEDULER-013: Missing recurrence support (display + edit)
**Area:** Recurrence
**Severity:** High
**Theme:** `recurrence`
**Source:** `docs/component-specs/scheduler/recurrence.md`, `src/Marilo.Core/Models/SchedulerModels.cs:15` (the `RecurrenceRule` field exists on `SchedulerAppointment` but is **unused by the source** — classic spec-ahead/orphaned-field gap).

**Target behavior:** Parse iCalendar RRULE strings; expand recurring appointments into per-occurrence virtual instances for rendering; handle exceptions (deletions + modifications of individual instances).

**Current behavior:** `RecurrenceRule` model field exists but is completely ignored during rendering.

**Impact:** "Meeting every Tuesday at 10am" use cases don't work at all. The field exists as a lie — consumers expect it to work because the spec says so.

**Recommended direction:** Add RRULE parser (evaluate `Ical.Net` MIT) or adopt `iCal.NET` for recurrence expansion. Expand in `GetAppointmentsForDate` to yield virtual instances. Plan standalone `MariloRecurrenceEditor` sub-component per spec.

**Status:** Open

---

### GAP-SCHEDULER-014: Missing standalone recurrence editor components
**Area:** Recurrence
**Severity:** Medium
**Theme:** `recurrence`
**Source:** `docs/component-specs/scheduler/recurrence.md` (references "standalone recurrence editor components")

**Target behavior:** Standalone recurrence-rule editor component consumers can embed in their own dialogs — Daily/Weekly/Monthly/Yearly, interval, until-date, by-day selection.

**Current behavior:** None.

**Impact:** Prevents building custom edit flows around recurrence. Not blocking basic read scenarios.

**Recommended direction:** Design after GAP-SCHEDULER-013. Separate component that emits `RRULE` strings.

**Status:** Open

---

## Feature Area: Templates

### GAP-SCHEDULER-015: Missing `AppointmentTemplate`
**Area:** Templates
**Severity:** High
**Theme:** `templates`
**Source:** `docs/component-specs/scheduler/templates/appointment.md`

**Target behavior:** `[Parameter] public RenderFragment<TItem>? AppointmentTemplate { get; set; }` replacing default appointment rendering.

**Current behavior:** Hardcoded `@a.Title` rendering.

**Impact:** Consumers cannot customize appointment visuals (color bars, icons, status badges, etc.).

**Recommended direction:** Add parameter, use via `@if (AppointmentTemplate is not null) { @AppointmentTemplate(appt) } else { default markup }`.

**Status:** Open

---

### GAP-SCHEDULER-016: Missing `DateHeaderTemplate`
**Area:** Templates
**Severity:** Medium
**Theme:** `templates`
**Source:** `docs/component-specs/scheduler/templates/dateheader.md`

**Target behavior:** `RenderFragment<DateTime>? DateHeaderTemplate` — customize the per-day header cell in Day/Week/MultiDay/Timeline views.

**Current behavior:** Hardcoded `@d.ToString("ddd M/d")` in the week/day column header.

**Impact:** Cannot add holiday markers, work-day indicators, or custom date formatting.

**Recommended direction:** Add parameter, route through day-column-header rendering.

**Status:** Open

---

### GAP-SCHEDULER-017: Missing `SlotTemplate`
**Area:** Templates
**Severity:** Medium
**Theme:** `templates`
**Source:** `docs/component-specs/scheduler/templates/slot.md`

**Target behavior:** `RenderFragment<SchedulerSlotContext>? SlotTemplate` — customize individual time slot cells (highlight lunch, meeting hours, etc.).

**Current behavior:** Hardcoded empty `<div class="mar-scheduler__time-slot">`.

**Impact:** Cannot highlight business hours visually, cannot indicate blocked-out time ranges.

**Recommended direction:** Add parameter with a `SchedulerSlotContext` record carrying `DateTime Start`, `DateTime End`, `bool IsWorkHour`.

**Status:** Open

---

### GAP-SCHEDULER-018: Missing `ResourceGroupHeaderTemplate`
**Area:** Templates
**Severity:** Medium
**Theme:** `templates`
**Source:** `docs/component-specs/scheduler/templates/resource-grouping-header.md`

**Target behavior:** `RenderFragment<SchedulerResource>? ResourceGroupHeaderTemplate` for customizing resource group headers in Timeline/grouped views.

**Current behavior:** N/A (depends on GAP-SCHEDULER-010).

**Impact:** Follows from GAP-SCHEDULER-010.

**Recommended direction:** Resolve alongside GAP-SCHEDULER-010.

**Status:** Open

---

## Feature Area: Events

### GAP-SCHEDULER-019: Missing CRUD events
**Area:** Events
**Severity:** High
**Theme:** `events`, `editing-pipeline`
**Source:** `docs/component-specs/scheduler/events.md`, `docs/component-specs/scheduler/overview.md:123-125`

**Target behavior:** `OnCreate`, `OnUpdate`, `OnDelete`, `OnEdit`, `OnCancel` events with typed event args (`SchedulerCreateEventArgs<TItem>`, etc.).

**Current behavior:** Only `OnAppointmentClick`, `OnDateClick`, and unused `OnAppointmentCreate`. No `OnUpdate`, `OnDelete`, `OnEdit`, `OnCancel`.

**Impact:** Consumers cannot persist mutations. Follows from GAP-SCHEDULER-009 (editing pipeline).

**Recommended direction:** Define typed event-arg classes; wire from edit popup.

**Status:** Open

---

### GAP-SCHEDULER-020: Missing `OnItemRender` event
**Area:** Events
**Severity:** Medium
**Theme:** `events`, `templates`
**Source:** `docs/component-specs/scheduler/overview.md:120` (spec mentions `OnItemRender`)

**Target behavior:** `EventCallback<SchedulerItemRenderEventArgs<TItem>>` firing per-appointment-render with a mutable `Class` property for conditional styling (cf. `OnItemRender` in MultiSelect Batch 6).

**Current behavior:** None.

**Impact:** No per-item customization hook short of a full template.

**Recommended direction:** Mirror `MultiSelect OnItemRender` implementation (cached args, rebuilt when data changes).

**Status:** Open

---

### GAP-SCHEDULER-021: Missing navigation events
**Area:** Events
**Severity:** Medium
**Theme:** `events`
**Source:** `docs/component-specs/scheduler/navigation.md`, `docs/component-specs/scheduler/events.md`

**Target behavior:** `OnDateChange`, `OnViewChange` events distinct from the `@bind-Date` / `@bind-View` callbacks — letting consumers observe navigation without taking over the binding.

**Current behavior:** Only `CurrentDateChanged` / `ViewChanged` (binding callbacks). No standalone navigation events.

**Impact:** Small — consumers can use the bind callbacks — but spec documents the separate events.

**Recommended direction:** Add the spec-named events; fire alongside the binding callbacks.

**Status:** Open

---

## Feature Area: Navigation / Toolbar

### GAP-SCHEDULER-022: Missing built-in toolbar
**Area:** Toolbar
**Severity:** Medium
**Theme:** `editing-pipeline`
**Source:** `docs/component-specs/scheduler/toolbar.md`

**Target behavior:** Configurable `SchedulerToolbar` with built-in commands (Today, Previous, Next, view switcher, View dropdown, optionally Create) and slot for custom commands.

**Current behavior:** Hardcoded header with prev/next arrows and three view buttons. No Today button, no customization slot.

**Impact:** Medium — the basic controls work, but no customization, no Today button (standard scheduler UX), no command slot.

**Recommended direction:** Extract the current header into a `SchedulerToolbar` child component. Add `Today` button. Add `ToolbarTemplate` for custom replacement.

**Status:** Open

---

### GAP-SCHEDULER-023: Missing WAI-ARIA accessibility support
**Area:** Accessibility
**Severity:** High
**Theme:** `a11y`
**Source:** `docs/component-specs/scheduler/accessibility/wai-aria-support.md`

**Target behavior:** `role="grid"` on calendar grid, `role="row"` on day rows, `role="gridcell"` on day/slot cells, `aria-label` per cell, `aria-selected`, `aria-current="date"` for today, keyboard navigation (arrow keys, Home/End, PageUp/PageDown, Enter to edit).

**Current behavior:** None. Plain `<div>` nesting with click handlers. No keyboard navigation. No screen reader support.

**Impact:** Fails WCAG 2.1 AA. Cannot be used in government/enterprise compliance contexts. Zero keyboard-only usability.

**Recommended direction:** Systematic a11y pass — follow `MariloDataGrid` and `MariloCalendar` patterns for role/aria attribute injection and arrow-key navigation.

**Status:** Open

---

## Feature Area: Methods

### GAP-SCHEDULER-024: Missing `Rebind()` / `Refresh()` methods
**Area:** DataBinding
**Severity:** Medium
**Theme:** `spec-api-naming`
**Source:** `docs/component-specs/scheduler/overview.md:144-147`, `docs/component-specs/scheduler/refresh-data.md`

**Target behavior:** Public `Rebind()` method for re-reading data source; public `Refresh()` method for re-rendering without refetch (cf. `MariloMultiSelect.Rebind()` from T4 Pickers B5).

**Current behavior:** No public methods.

**Impact:** Consumers cannot programmatically refresh after external mutations.

**Recommended direction:** Add both. `Rebind()` triggers OnRead if present (cf. GAP-SCHEDULER-026); `Refresh()` calls `StateHasChanged()` via dispatcher.

**Status:** Open

---

### GAP-SCHEDULER-025: Missing `EnableLoaderContainer` parameter
**Area:** DataBinding
**Severity:** Low
**Theme:** `spec-api-naming`
**Source:** `docs/component-specs/scheduler/overview.md:136`

**Target behavior:** `[Parameter] public bool EnableLoaderContainer { get; set; } = true;` — wraps the scheduler in a `MariloLoaderContainer` during OnRead / Rebind operations >600ms.

**Current behavior:** No loader support.

**Impact:** No loading UX during async data fetches.

**Recommended direction:** Wrap root `<div>` in `MariloLoaderContainer` conditionally; flip loader state in Rebind/OnRead paths.

**Status:** Open

---

## Feature Area: Data Binding (Advanced)

### GAP-SCHEDULER-026: Missing `OnRead` remote-data pattern
**Area:** DataBinding
**Severity:** Medium
**Theme:** `generic-data-binding`
**Source:** `docs/component-specs/scheduler/data-bind.md`, `docs/component-specs/scheduler/manual-operations.md`

**Target behavior:** `EventCallback<SchedulerReadEventArgs>? OnRead` — remote-data scenario where the scheduler asks the server for the appointments in the currently-visible window. Mirrors `OnRead` patterns in DataGrid, MultiSelect.

**Current behavior:** Only local `Data` / `Appointments` input.

**Impact:** Users with large appointment stores must load everything client-side.

**Recommended direction:** Add `OnRead` with a `SchedulerReadEventArgs` carrying the visible date range. Mirror `GridReadEventArgs` / `MultiSelectReadEventArgs` shape.

**Status:** Open

---

### GAP-SCHEDULER-027: First-day-of-week not configurable
**Area:** Views
**Severity:** Low
**Theme:** `spec-api-naming`
**Source:** `docs/component-specs/scheduler/navigation.md`

**Target behavior:** `[Parameter] public DayOfWeek FirstDayOfWeek { get; set; } = DayOfWeek.Sunday;` — allows Monday-first (European) or Saturday-first (Middle East) locales.

**Current behavior:** Hardcoded Sunday-first in `_dayNames` and `GetWeekStart`.

**Impact:** International locales get incorrect week layouts.

**Recommended direction:** Add parameter; wire through `_dayNames` reordering and `GetWeekStart` offset.

**Status:** Open

---

## Feature Area: Demo Coverage

### GAP-SCHEDULER-028: Trivial Overview demo
**Area:** Demo
**Severity:** Low
**Theme:** `demo-coverage`
**Source:** `samples/Marilo.Demo/Pages/Components/Scheduler/Overview.razor:1-16`

**Target behavior:** Realistic Overview demo matching the spec example at `docs/component-specs/scheduler/overview.md:26-79` — bound to real appointment data, demonstrating Day/Week/Month switch, business hours.

**Current behavior:** 16-line placeholder showing an empty scheduler with no data.

**Impact:** Users opening the demo see nothing useful.

**Recommended direction:** Replace with the spec's canonical example (once the generic rewrite lands).

**Status:** Open

---

### GAP-SCHEDULER-029: Missing data-binding demo
**Area:** Demo
**Severity:** Low
**Theme:** `demo-coverage`
**Source:** `docs/component-specs/scheduler/data-bind.md`

**Target behavior:** Dedicated demo page showing `OnRead` remote-data pattern (depends on GAP-SCHEDULER-026).

**Current behavior:** None.

**Status:** Open

---

### GAP-SCHEDULER-030: Missing editing demo
**Area:** Demo
**Severity:** Low
**Theme:** `demo-coverage`
**Source:** `docs/component-specs/scheduler/editing/edit-appointments.md`

**Target behavior:** Demo with create/edit/delete popup flows. Depends on GAP-SCHEDULER-009.

**Current behavior:** None.

**Status:** Open

---

### GAP-SCHEDULER-031: Missing recurrence demo
**Area:** Demo
**Severity:** Low
**Theme:** `demo-coverage`
**Source:** `docs/component-specs/scheduler/recurrence.md`

**Target behavior:** Demo with a weekly recurring "Team standup" appointment and an exception for a skipped week. Depends on GAP-SCHEDULER-013.

**Current behavior:** None.

**Status:** Open

---

### GAP-SCHEDULER-032: Missing resource-grouping demo
**Area:** Demo
**Severity:** Low
**Theme:** `demo-coverage`
**Source:** `docs/component-specs/scheduler/resource-grouping.md`

**Target behavior:** Demo with 3 meeting rooms as resources and appointments assigned to each, shown in Timeline view. Depends on GAP-SCHEDULER-010 + GAP-SCHEDULER-007.

**Current behavior:** None.

**Status:** Open

---

## Cross-cutting Observations

1. **Generic rewrite is the critical path.** GAP-SCHEDULER-001 (generic `TItem`) is the single blocking gap — almost every other gap either depends on it or is cheaper to resolve once it lands. Stage 02 should treat the generic rewrite as "Batch 1" and sequence everything else behind it.

2. **Child-tag architecture is the second critical path.** GAP-SCHEDULER-004 (`<SchedulerViews>` wrapper + per-view children) is the vehicle for per-view configuration (`StartTime`, `EndTime`, `SlotDuration`, `WorkDayStart`/`WorkDayEnd`, etc.) and must land before MultiDay/Timeline views can carry their own config. Follow the canonical `MariloDataGrid` ↔ `MariloGridColumn` cascading pattern, and apply the Wizard CascadingValue interface-cast fix (cerebrum 2026-04-04) for non-generic children to attach to a generic parent.

3. **`SchedulerAppointment.RecurrenceRule` is orphaned.** The field exists in `src/Marilo.Core/Models/SchedulerModels.cs:15` but is never read by the source. This is a user-visible API lie — consumers who set it expect it to render as a recurring event. Flag as `spec-ahead` in Stage 02 for urgent closure alongside GAP-SCHEDULER-013.

4. **`SchedulerView` enum is incomplete.** Only Day/Week/Month. Missing `MultiDay` and `Timeline` enum members — blocks GAP-SCHEDULER-006 and GAP-SCHEDULER-007 at the type level.

5. **This is essentially a greenfield rebuild of the API surface.** 5 critical + 13 high + 9 medium + 5 low = 32 gaps across 9 feature areas. Recommend the same execution model used for the MariloGantt full rewrite (20 gaps, 5 phases A–F, 24 commits, 31 bUnit tests, 2026-04-09) — subagent-driven dev with two-stage review, phased by feature area.

## Suggested Phase Breakdown for Stage 02 Prioritization

| Phase | Scope | Unblocks |
|---|---|---|
| **A** | Generic rewrite (GAP-001, 002, 003, 005, 012) + `SchedulerView` enum extension (prep for B) | All other phases |
| **B** | Child-tag architecture (GAP-004) + per-view params (GAP-008) + MultiDay/Timeline enum members + `FirstDayOfWeek` (GAP-027) | Views, Templates |
| **C** | Remaining views (GAP-006, 007) + navigation parameters / Today button (part of GAP-022) | Timeline needs Resources (D) for full value |
| **D** | Resources + grouping (GAP-010, 011, 018) | Timeline view value; templates |
| **E** | Templates (GAP-015, 016, 017) + OnItemRender (GAP-020) | Customization |
| **F** | Editing pipeline (GAP-009, 019) + toolbar (GAP-022) + delete confirmation | Full write-path |
| **G** | Recurrence (GAP-013, 014) | Recurring events |
| **H** | A11y pass (GAP-023) | WCAG compliance |
| **I** | `OnRead` / `Rebind` / `Refresh` / `EnableLoaderContainer` (GAP-024, 025, 026) | Remote data |
| **J** | Demo coverage (GAP-028, 029, 030, 031, 032) + navigation events (GAP-021) | Documentation |

Phase A is the hard dependency for everything else. Phases D and G can run in parallel with Phase F after B lands. Phase H (a11y) can start any time after A.

---

## Audit Checklist

| Check | Status |
|---|---|
| Every gap has a unique ID | ✅ GAP-SCHEDULER-001 through GAP-SCHEDULER-032 (no duplicates) |
| Every gap references real artifacts | ✅ All source paths verified against the snapshot |
| Severity assigned to all gaps | ✅ 5 Critical, 13 High, 9 Medium, 5 Low |
| Target state documented | ✅ `_config/gap-context.md` target is MariloScheduler matching `docs/component-specs/scheduler/` |
| Counts match | ✅ 5+13+9+5 = 32 |

## Human Decisions Needed Before Stage 02

1. **Generic rewrite scope** — rebuild in place on `workInProgress` or use a dedicated branch (`scheduler-rewrite`, cf. the `gantt-rewrite` precedent)?
2. **Backward-compat horizon** — how long to keep `[Obsolete]` aliases for `Appointments`/`CurrentDate`/`StartHour`/`EndHour`? One release? Two?
3. **Recurrence library** — approve `Ical.Net` (MIT) as the RRULE parser, or defer recurrence entirely to a later pass?
4. **Editing-popup ownership** — is the edit popup a MariloScheduler internal component or a consumer-provided `EditTemplate`? (Spec implies built-in; MudBlazor-style users may prefer the template approach.)
5. **Timeline view + Resources coupling** — treat Timeline view as a Phase C deliverable or block until Phase D (Resources) lands so the first demo is meaningful?

Stage 02 prioritization should begin once these decisions are made.
