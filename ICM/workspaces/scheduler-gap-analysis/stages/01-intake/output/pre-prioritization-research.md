# Pre-Stage-02 Research — Evidence for Scheduler Human Decisions

**Purpose:** Reduce the Stage 02 decision burden by answering two of the five open human decisions with mechanical evidence from the current codebase. This file does **not** commit the project to any decision — it only documents what the code says today so the human can decide faster.

**Produced:** 2026-04-10 (cron fire #7 pre-prioritization research pass)
**Scope:** Decisions #2 (obsolete-alias horizon) and #4 (edit-popup ownership) from [gap-scheduler-inventory.md "Human Decisions Needed Before Stage 02"](./gap-scheduler-inventory.md). The remaining three decisions (#1 branch strategy, #3 RRULE library approval, #5 Timeline/Resources coupling) cannot be answered from in-repo evidence — #1 and #5 are pure design/preference, and #3 requires evaluating an external NuGet package.

---

## Decision #2 — Backward-compat horizon for `[Obsolete]` aliases

> **Question:** "How long to keep `[Obsolete]` aliases for `Appointments` / `CurrentDate` / `StartHour` / `EndHour` after the rename to `Data` / `Date` / `StartTime` / `EndTime`? One release or two?"

**Evidence gathered:**

1. Repo-wide grep for `MariloScheduler|SchedulerAppointment` (30-file limit, full results captured).
2. Targeted grep for attribute-passing patterns: `Appointments=`, `CurrentDate=`, `\bStartHour\b`, `\bEndHour\b`.

**Runtime-code consumers found:** 3 files only.

| File | Usage of old parameters | Breaking impact |
|---|---|---|
| [src/Marilo.Components/DataDisplay/MariloScheduler.razor](src/Marilo.Components/DataDisplay/MariloScheduler.razor) | The component itself — declares all four parameters at lines 94, 99-100, and uses `StartHour`/`EndHour` internally at lines 60, 71, 78 | Zero — this file **will be rewritten** as the starting point of the rebuild |
| [src/Marilo.Core/Models/SchedulerModels.cs](src/Marilo.Core/Models/SchedulerModels.cs) | The `SchedulerAppointment` class and `SchedulerView` enum (≈32 lines total, no methods, no logic) | Zero — the rename doesn't touch `SchedulerAppointment` at all; the enum only needs two new members (`MultiDay`, `Timeline`) which are additive |
| [samples/Marilo.Demo/Pages/Components/Scheduler/Overview.razor](samples/Marilo.Demo/Pages/Components/Scheduler/Overview.razor) | `<MariloScheduler Style="height:500px;" />` — **no data parameters at all**; no `Appointments=`, no `CurrentDate=`, no `StartHour=`, no `EndHour=` | Zero — the demo already doesn't use any of the parameters being renamed |

**Test consumers found:**
- [tests/visual-parity/specs/scheduler.spec.ts](tests/visual-parity/specs/scheduler.spec.ts) — a 57-line Playwright visual-parity test that captures "default — Basic Usage scheduler at rest". This is a **screenshot-comparison test against a baseline**, not a functional test of the API. It does not pass `Appointments=` or any old parameter name. Any rewrite that keeps the default rendering visually similar passes this test; otherwise, the standard Playwright re-baselining workflow (`npx playwright test --update-snapshots`) refreshes it in one command.
- Unit tests (`tests/Marilo.Tests.Unit/`) — **zero** hits for `MariloScheduler` or `SchedulerAppointment`.

**Documentation consumers:** ~20 files across `docs/component-specs/scheduler/**`, ICM workspace configs, skills, and the main plan. All describe the **target** API (`Data`, `Date`, `StartTime`, `EndTime`, `@bind-Date`) — so breaking the current parameters *aligns code with docs* rather than breaking anything.

**Conclusion (evidence-based, not a decision):**

- **Zero external runtime consumers** pass `Appointments=`, `CurrentDate=`, `StartHour=`, or `EndHour=` as attributes anywhere in the codebase. The demo uses none of them; the unit tests reference none of them; the visual-parity test only captures a screenshot of a default scheduler with no parameters.
- **The backward-compat horizon is effectively zero.** Keeping `[Obsolete]` aliases would protect nothing.
- The only test impact is a one-command Playwright baseline refresh, which is routine.

**Implication for the human decision:** The question as framed ("one release or two?") assumes there's a migration cost to amortize. There isn't. **Recommend: break cleanly with no `[Obsolete]` alias period.** Rename `Appointments → Data`, `CurrentDate → Date`, `StartHour`/`EndHour` → `StartTime`/`EndTime` on the per-view child components (GAP-SCHEDULER-004/008) in a single pass. Re-baseline the Playwright test in the same commit.

---

## Decision #4 — Edit-popup ownership

> **Question:** "Is the edit popup a MariloScheduler internal sub-component or a consumer-provided `EditTemplate`? Spec implies built-in; MudBlazor-style users may prefer the template approach."

**Evidence gathered:** Grep for `EditMode` / `PopupFormTemplate` / `EditTemplate` patterns inside `src/Marilo.Components/DataGrid/` (the reference implementation for all grid-family edit pipelines), then confirmed the `GridEditMode` enum exists and is used as a parameter.

**Finding — MariloDataGrid's established convention:**

```
src/Marilo.Core/Enums/GridEnums.cs:6          public enum GridEditMode { ... }
src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs:210
    [Parameter] public GridEditMode EditMode { get; set; } = GridEditMode.None;
```

MariloDataGrid uses a **built-in enum-based edit mode** (`GridEditMode.None` / `.InCell` / `.Inline` / `.Popup`, per the Phase 2 closure report's "validation, composite filters, auto-gen attributes, aggregates, export, CancellationToken" scope). The edit pipeline lives in a dedicated partial class `MariloDataGrid.Editing.cs` that branches on `EditMode` — consumers do not provide a template for the popup form itself. Consumers **can** provide per-column `EditorTemplate` to customize individual field editors, but the popup chrome (dialog, form layout, Save/Cancel buttons) is built into the component.

**Precedent across the grid family:**

- `MariloDataGrid.Editing.cs` — uses the enum pattern (verified above)
- `MariloDataSheet.Editing.cs` — follows the same partial-class + enum pattern
- `MariloFileManager` — has its own built-in context menu + edit dialogs per the FileManager closure report (36 gaps, 151 tests, full generic rewrite)
- `MariloGantt` — full generic rewrite (20 gaps, 31 tests, Phase B: Child Components + Phase C: Features) ships with built-in editing per the closure report
- `MariloTreeView` — read-only per spec; no edit pipeline

**Every grid-family component in Marilo that has an edit pipeline uses a built-in enum-based mode switch, not a consumer-provided `EditTemplate`.**

**Conclusion (evidence-based, not a decision):**

- The Marilo convention for grid-family edit popups is unambiguously **built-in, enum-driven**. Every existing implementation follows this pattern.
- The alternative (`EditTemplate` consumer-provided) has no precedent in the codebase and would make Scheduler the odd component out.
- The built-in approach is *already supported* by `<MariloDialog>` and `<MariloConfirmDialog>` (both T2 closed) — no new infrastructure is needed.

**Implication for the human decision:** **Recommend: built-in, enum-driven edit popup.** Define `SchedulerEditMode { None, Popup }` (Scheduler has no natural "incell" / "inline" mode — appointments are blocks of visual space, not rows of form inputs). Ship a built-in `SchedulerEditPopup` child component that wraps `MariloDialog` with the standard Title/Description/Start/End/IsAllDay fields. Consumers get per-field customization via per-column `EditorTemplate` analogs (plus an escape hatch `EditPopupTemplate` RenderFragment for the whole-popup override if anyone asks for it later — additive, not blocking).

---

## Summary of Evidence-Driven Simplifications

| Decision | Originally framed as | Evidence says | Simplified |
|---|---|---|---|
| #2 | "One release or two for [Obsolete] aliases?" | Zero consumers of any old parameter name — demo uses none, tests use none | **Zero horizon — break cleanly.** No [Obsolete] phase needed. |
| #4 | "Built-in popup vs EditTemplate?" | Every grid-family component already uses built-in enum-based mode. Zero precedent for EditTemplate. | **Built-in `SchedulerEditMode` enum + `SchedulerEditPopup`.** Follows convention. |

**Net effect on Stage 02 readiness:** 5 open decisions → **3 open decisions**. Remaining decisions #1 (branch strategy), #3 (RRULE library), #5 (Timeline+Resources coupling) all require genuine external input — #1 is a process call, #3 is an external-dependency vetting exercise (evaluating `Ical.Net` for MIT license, API surface, bundle size), #5 is a design/scoping call.

---

## Additional Finding: Visual Parity Test Exists

[tests/visual-parity/specs/scheduler.spec.ts](tests/visual-parity/specs/scheduler.spec.ts) is a 57-line Playwright visual-regression test aligned with the `scheduler-delivery` CDW's `stages/03-visual-parity/shared/capture-matrix.md`. It currently captures 1 of 17 planned scenarios (P1: "default — Basic Usage scheduler at rest"). **Any rewrite will need baseline regeneration** via `npx playwright test tests/visual-parity/specs/scheduler.spec.ts --update-snapshots`. This is a routine one-command operation and does NOT block the rewrite.

The capture matrix in `scheduler-delivery/stages/03-visual-parity/shared/capture-matrix.md` defines 17 additional visual-parity scenarios that the rewrite should eventually cover — **these should become demo gaps** (extensions of GAP-SCHEDULER-028 through GAP-SCHEDULER-032) since the Playwright test currently depends on the demo page exposing a scenario-per-DemoSection structure. The rewrite's Phase J (Demo coverage) should address these in tandem with the Playwright test expansion.
