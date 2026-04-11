# Gap Analysis Resolution Plan (UPF)

**Plan Confidence Level:** High (retrospective — all phases completed)
**Domain:** Software Development
**Status:** Completed (as of 2026-04-10)
**Conversion:** Retrospective wrapper over [GAP_ANALYSIS_RESOLUTION_PLAN.md](GAP_ANALYSIS_RESOLUTION_PLAN.md) — original preserved as execution log
**UPF Version:** 1.2 (see [.claude/rules/universal-planning.md](../../.claude/rules/universal-planning.md))

> **About this document.** This is a Universal Planning Framework shell wrapped around the completed Marilo gap-analysis resolution effort. The authoritative history — per-component status, batch trackers, audit findings, handoffs — lives in the original [GAP_ANALYSIS_RESOLUTION_PLAN.md](GAP_ANALYSIS_RESOLUTION_PLAN.md) and is not duplicated here. This UPF wrapper exists so the plan has the CORE sections, Gates, FAILED conditions, Reference Library, and Hardening Log that UPF Grade B/A requires, and so future gap-closure efforts inherit a UPF-native template.

---

## End State

Every one of the 87 components originally evaluated in [GAP_ANALYSIS_INDEX.md](GAP_ANALYSIS_INDEX.md), plus the 24 additional components surfaced during the 2026-04-10 Prototype API Completion Pass, has a prototype-ready public API surface, at least one working demo in the Marilo demo registry, bUnit coverage for its documented parameters and events, and a closure status recorded in the [resolution tracking table](GAP_ANALYSIS_RESOLUTION_PLAN.md#component-resolution-tracking-table). No "coming soon" / "TBD" / "placeholder" strings remain in the demo UX. Open gaps that remain are explicitly deferred with rationale (e.g., Map tile rendering, Spreadsheet formula engine, Diagram interactive editing) and tracked outside this plan via the CDW workspaces. Further component-level work routes through the ICM/CDW pipeline, not through this plan.

---

## CORE: Context & Why

The initial Marilo Blazor component library had 87 components with 600+ documented spec/implementation gaps spanning missing parameters, broken data binding, absent sub-components, missing accessibility, and placeholder demos. Shipping a provider-first Blazor component library requires a compile-first public API surface across the whole set before per-component polish makes sense, because consumers cannot adopt a library where half the components don't exist or silently no-op. This plan exists to resolve those gaps in dependency order — primitives first, complex components second, standard components third, polish last — so downstream composition always has a working base to build on.

### Behavior Description

From a consumer's perspective: a developer adding Marilo to a .NET 10 Blazor app can reference any component in the demo registry, wire the documented parameters and events, receive the expected visual output from their chosen provider (FluentUI, Bootstrap, Material), bind forms through `EditContext` and get validation feedback, and switch themes at runtime without breakage. Demos in the sample app show every component working with interactive controls, not placeholders.

---

## CORE: Success Criteria

**Measurable outcomes:**

1. **API surface completeness** — 100% of components in `ComponentRegistry.cs` (119 entries) compile with their documented public API. *(Met: 2026-04-10 Prototype API Completion Pass closed the final 39 placeholder pages.)*
2. **Demo parity** — 0 "coming soon" / "TBD" / "placeholder" strings in the demo site. *(Met: placeholder scan returns zero after the API Completion Pass.)*
3. **Test coverage** — every T1/T2/T3/T4 resolved component has at least one bUnit test asserting documented parameters, events, and rendering; complex components (DataGrid, Editor, AllocationScheduler, Gantt, TreeView, pickers) have multi-test suites. *(Met: see [Test Coverage Status](GAP_ANALYSIS_RESOLUTION_PLAN.md#test-coverage-status) in the original.)*
4. **Dependency ordering respected** — no component in Phase N uses infrastructure from Phase N+1 (verified by build order and phase gate reviews).
5. **Deferred work is explicit** — every gap not closed is recorded with a reason (spec-only, external dependency, behavior-parity follow-up) and a routing decision (CDW workspace, future batch, won't fix).

### FAILED Conditions

This plan is abandoned / replanned if ANY of the following hold:

- **FAILED-1:** Core infrastructure (ThemeProvider, Form/Validation, Layout) cannot reach stable public API after Phase 1 → downstream phases are unsafe to run; must replan with a different component model. *(Did not trigger.)*
- **FAILED-2:** No viable OSS-compatible rich-text engine exists under MIT/Apache-2.0/BSD → Editor gap is unclosable; must mark Editor as "won't fix" and document rationale. *(Did not trigger — Tiptap/ProseMirror wrapped successfully; later superseded by in-house WYSIWYG path per batch history.)*
- **FAILED-3:** Gap count in any completed tier grows instead of shrinking across two consecutive review checkpoints → indicates spec drift faster than resolution; halt and revisit spec stability. *(Did not trigger.)*
- **FAILED-4:** Breaking changes to public API surface exceed 2 per completed tier → consumer trust eroded; halt and stabilize. *(Did not trigger.)*
- **FAILED-5 (timeout):** Any tier remains open > 90 days without a tracked batch landing → indicates ownership gap; escalate or abandon tier. *(Did not trigger.)*

### Acceptance Criteria (GWT)

- **Given** a developer consumes a T1 primitive (e.g., `MariloForm`), **When** they bind it to a model via `EditContext` and wire `OnSubmit`/`OnValidSubmit`/`OnInvalidSubmit`, **Then** submission events fire in the expected order and validation state flows through `MariloValidationMessage` / `MariloValidationSummary`.
- **Given** a T2 complex component (e.g., `MariloDataGrid`), **When** they wire server-side paging via `OnRead`, **Then** the grid renders within one frame of `Data`/`Total` arriving and pager state stays consistent across resize.
- **Given** any T3/T4 form input, **When** it is placed inside a `MariloForm` + `MariloField`, **Then** the floating label, validation styling, and disabled state respond to `EditContext` without per-component wiring.
- **Given** a provider switch at runtime (Fluent ↔ Bootstrap ↔ Material), **When** the page reloads (intentional — see `ProviderSwitcher`), **Then** every component in the demo renders with the target provider's visuals and no hard-coded Fluent tokens leak through.

### NOT-Scope

- Visual design refinement beyond provider-native defaults (belongs to provider-specific design passes).
- Advanced behavior parity with commercial libraries (Telerik, Syncfusion, DevExpress) — deferred to CDW workspaces.
- Map tile rendering, Spreadsheet formula engine, PdfViewer PDF.js wiring, Diagram drag/edit, Sankey layout optimization, PivotGrid OLAP drill-down, DockManager true dock zones, Chat real-time transport, AI provider wiring, SmartPaste clipboard parsing, SpeechToText Web Speech API wiring — all explicitly deferred as behavior-parity follow-ups.
- Virtualization for TreeView (GAP deferred).
- Public API stability guarantees beyond "additive changes only" (see CLAUDE.md).

---

## CORE: Assumptions & Validation

| Assumption | Validate By | Impact If Wrong |
|---|---|---|
| The 87-component inventory in `GAP_ANALYSIS_INDEX.md` is exhaustive for the prototype scope. | Cross-check against `ComponentRegistry.cs` (119 entries) before each phase starts. | **Partial invalidation in Apr 2026:** the registry revealed 32 extra components not in the original 87; closed via the 2026-04-10 API Completion Pass. Rest of plan held. |
| Gaps can be resolved in dependency order without cyclic dependencies between tiers. | Build the dependency graph from `component-mapping.json` and verify it is a DAG before Phase 1. | Cycles force replanning; held true for T1→T2→T3→T4. |
| OSS-compatible libraries exist for every hard external dependency (rich-text, HTML sanitization, anchor positioning, input masking). | Check license + activity of each candidate in Stage 0 external-research table (Phase 2/4 tables). | If any falls through, the gap becomes unclosable; see FAILED-2. Held true for all selected libraries. |
| bUnit v2 + `MariloTestBase` is adequate for behavioral parity testing. | Run a probe test suite on DataGrid (most complex) before expanding to every component. | If inadequate, testing strategy needs a new layer (Playwright, visual regression). Partially held — Playwright was added later for visual parity, but bUnit remained the primary behavioral layer. |
| Provider-first architecture lets every component ship a Fluent UI baseline and have Bootstrap/Material follow without rework. | For each component, verify SCSS provider files exist and visually match the Fluent baseline at review checkpoint. | If wrong, the provider-first thesis fails; would have required a rewrite. Held true. |
| AI subagent delegation is viable for batch-shaped work (pickers, T3 single-pass components). | Run one subagent batch end-to-end (Batch 7 T4 pickers, 2026-04-08) and compare output quality to direct implementation. | If subagent output is unreliable, serial implementation is the fallback. Held — subagent batches 7–8 landed successfully. |
| The CLAUDE.md "never delete gap notes" rule does not block closure, only deletion. | Confirm by keeping per-component closure entries in the tracking table. | Held — closures recorded as status updates, not deletions. |

---

## CORE: Phases

Four phases sized by scope (files + components + tests), not hours. Review checkpoint every two phases per UPF Software Development domain convention. All phases are **Complete** as of 2026-04-10.

### Phase 1 — Critical Primitives and Foundation (T1) — ✅ Complete

**Scope:** ThemeProvider, Form, Validation (+3 sub-components), Field, Label, Icon, Layout primitives (Grid, Stack, Container, Row, Column, Divider).

**Deliverable:** CSS-variable token system, dark mode toggle, `EditContext`-integrated form pipeline, floating-label/validation-state infrastructure, layout primitives composable by downstream components.

**Gate (binary):**
- `MariloThemeProvider` emits `--marilo-color-*` tokens and flips on `[data-marilo-theme="dark"]` — verified by DOM assertion test. **PASS**
- `MariloForm` binds to a model and fires `OnSubmit` / `OnValidSubmit` / `OnInvalidSubmit` in bUnit integration test. **PASS**
- `MariloValidationMessage` + `MariloValidationSummary` + `MariloValidationTooltip` render field errors from `EditContext`. **PASS**
- Layout primitives render with Grid/Stack children and reserve the documented spacing. **PASS**

**Review:** Spec/source/demo alignment review. **PASS** (recorded in original plan tracking table).

**[Phase 1–2 Review Checkpoint]:** Cross-reference check — new T2 code follows the Phase 1 patterns (SCSS provider split, `EditContext` wiring, dispatcher-safe public state APIs). **PASS.**

---

### Phase 2 — Complex Data and Interaction Components (T2) — ✅ Complete

**Scope:** DataGrid (+GridColumn, GridToolbar), Editor, Chart (+ChartSeries), Window, Dialog, ConfirmDialog, Popover, Drawer, List, TreeView, Menu, ContextMenu, Accordion, Splitter, Panel, Stepper, Pagination.

**Deliverable:** Hierarchical data binding pattern shared across tree/menu/accordion, JS interop infrastructure for drag/resize/positioning, rich-text editing via in-house contenteditable (Tiptap/ProseMirror evaluated; in-house path won), SVG chart rendering with 12+ chart types, overlay positioning service shared by Window/Dialog/Popover.

**Gate (binary):**
- `MariloDataGrid` supports paging (client + `OnRead`), sorting, filtering, column templates, three edit modes, virtual scrolling via `Virtualize`, public `GridState` API. **PASS**
- `MariloEditor` performs WYSIWYG editing via contenteditable + execCommand, paste cleanup, HtmlSanitizer XSS sanitization, adaptive toolbar, table/image resize. **PASS**
- `MariloChart` renders bar/line/column/pie + tooltips + legend + axis config + a11y. **PASS**
- `MariloWindow` drags, resizes, minimizes, maximizes, restores with two-way position/size binding. **PASS**
- TreeView/Menu/ContextMenu/Accordion share a single hierarchical data-binding model and pass per-component bUnit tests. **PASS**

**Review:** Architecture review on shared infrastructure (overlay service, JS interop module, hierarchical binding). **PASS.**

---

### Phase 3 — Standard Components, Single-Pass Resolution (T3) — ✅ Complete

**Scope:** 42 standard components — buttons (Button, ButtonGroup, Chip, ChipSet, SplitButton, Fab, ToggleButton, IconButton, SegmentedControl), data display (Avatar, Badge, Card family, Carousel, ListItem, ListView, Tooltip), feedback (Alert, AlertStrip, Callout, ProgressBar, Skeleton, Toast), form inputs (Autocomplete, Checkbox, ComboBox, DatePicker, DropDownList, NumericInput, Radio, TextField, TextArea, Select, Switch, Slider), layout/navigation (AccordionItem, AppBar, TabStrip, Step, Breadcrumb, Toolbar).

**Deliverable:** Every standard component has documented parameters, events, ARIA roles, keyboard navigation, and `EditContext` integration where applicable; parameter naming is consistent across the set.

**Gate (binary):**
- Every T3 component passes a bUnit test asserting documented parameters + events + rendering. **PASS**
- No T3 component introduces new overlay/positioning infrastructure (all reuse T2). **PASS**
- Consistent naming audit: `Enabled`/`Disabled` conventions applied across set. **PASS**

**Review:** Cross-component naming + a11y audit. **PASS.**

**[Phase 3–4 Review Checkpoint]:** A11y spot-check + SCSS provider parity review before T4 polish. **PASS.**

---

### Phase 4 — UX Polish, Performance, Accessibility, Edge Cases (T4) — ✅ Complete

**Scope:** ColorPicker (+FlatColorPicker, ColorGradient, ColorPalette, ColorPickerViews), DateRangePicker, DateTimePicker, TimePicker, FileUpload, Upload, MaskedInput, MultiSelect, RangeSlider, Rating, SearchBox, plus 15 remaining minor-gap components (DataBanner, DataToast, ProgressCircle, Snackbar, SnackbarHost, Spinner, BreadcrumbItem, EnvironmentBadge, MenuItem, ToolbarButton/Group/Separator/ToggleButton, TreeItem, TimeRangeSelector, TabStripTab).

**Deliverable:** Advanced picker UIs (HSV canvas, dual-calendar, tumbler), chunked file upload with drop-zone JS interop via `IDropZoneService`, WCAG 2.1 AA audit results, generic-parent + non-generic-children cascade pattern proven (MultiSelect → `IMultiSelectSettingsSink`).

**Gate (binary):**
- All advanced pickers render + typed-input parse + keyboard nav + bUnit coverage. **PASS**
- File upload chunked path works with drop-zone JS interop wired. **PASS**
- Every T4 component is either RESOLVED or explicitly deferred with documented reason in the tracking table. **PASS**

**Review:** Final closure review — tracking table audited, deferred items routed to CDW workspaces. **PASS.**

---

### Post-Plan Supplement — Prototype API Completion Pass (2026-04-10)

Not originally a phase of this plan. Triggered by discovery that `ComponentRegistry.cs` contained 32 additional components beyond the 87 originally inventoried, all shipping as "coming soon" demo stubs.

**Scope:** 24 new prototype component stubs, 2 new enum files, 8 new model files, 39 placeholder demo pages rewritten.

**Gate:** Post-pass placeholder scan returns zero "coming soon" / "TBD" / "placeholder" strings. **PASS.**

**[REPLANNED: inventory discovery]** — Added as a scope supplement rather than a full replan because the 4-phase structure still held; this was additive closure, not structural change.

---

## CORE: Verification

### Automated

- `dotnet build` clean across solution.
- `dotnet test` — full bUnit + unit suite green. Target coverage: per-component API assertions + integration tests for `EditContext`, overlay, drag/resize, virtualization, clipboard, drag-fill.
- Visual parity via Playwright harness at [tests/visual-parity/](../../tests/visual-parity/) — Chromium desktop 1280×900, baselines per `{component}/{theme-mode}/{viewport}/{scenario}.png`.
- Placeholder string scan in demo site (zero tolerance).
- Component registry audit — every registry entry has a resolved status in the tracking table.

### Manual

- Per-component spec/source/demo alignment review (recorded in `docs/component-specs/<slug>/`).
- Demo sample app smoke test per provider (FluentUI, Bootstrap, Material), light + dark themes.
- A11y spot-check per component family (WCAG 2.1 AA target).
- Architecture review at phase 1–2 and 3–4 checkpoints (shared infrastructure drift).

### Ongoing Observability

- Component status in the [tracking table](GAP_ANALYSIS_RESOLUTION_PLAN.md#component-resolution-tracking-table) updated per batch.
- CDW/ICM workspace stages tracked via `workspace-status` skill — `.claude/rules/openwolf.md` + `.wolf/anatomy.md` maintained on every file change.
- Follow-up gap intake via `gap-analysis-resolution` skill stages `01-intake` through `06-validate`.
- Bug log at `.wolf/buglog.json` captures regressions with root cause + fix + tags.

---

## CONDITIONAL: Dependencies & Blockers

| Type | Item | Status at close | Fallback |
|---|---|---|---|
| External library | Tiptap / ProseMirror (MIT) | Evaluated, not adopted | In-house contenteditable + execCommand |
| External library | HtmlSanitizer (MIT) | Adopted | N/A |
| External library | Floating UI (MIT) | Adopted for popover positioning | N/A |
| External library | IMask.js (MIT) | Adopted for MaskedInput | N/A |
| Framework feature | Blazor `Virtualize` | Adopted for DataGrid rows, MultiSelect | N/A — no other built-in option |
| Framework feature | `EditContext` / `DataAnnotationsValidator` | Adopted across all form inputs | N/A — framework primitive |
| Internal | `IDropZoneService` for drop-zone JS interop | Delivered | N/A |
| Internal | Shared overlay/positioning service | Delivered | N/A |

---

## CONDITIONAL: Rollback / Undo Strategy

- All work landed through reviewable commits on branch `workInProgress` → merge PRs to `main`. Rollback = git revert per-PR.
- No schema migrations, no external service changes, no data migration — plan is fully reversible at the VCS layer.
- Provider contracts (`IMariloProvider`, `IMariloCssProvider`) remained additive throughout; no breaking changes to consumers required.
- Point of no return: none — consumers do not yet depend on Marilo at production scale during this plan's lifetime.

---

## CONDITIONAL: Risk Assessment

| Risk | Likelihood | Impact | Mitigation | Detection | Actual outcome |
|---|---|---|---|---|---|
| OSS rich-text engine fails license or maintenance check | Medium | High | Evaluated multiple candidates; kept in-house fallback | Stage 0 Check 0.3 + 0.9 | Mitigation activated — in-house path chosen |
| DataGrid complexity exceeds a single phase | High | High | Split DataGrid across multiple batches within Phase 2 | Gate: paging + sorting + filtering each independently tested | Mitigation worked; DataGrid delivery shipped across multiple batches with batch-level closure |
| bUnit v2 API surprises (vs v1 SetParametersAndRender) | Medium | Medium | Test infra refactored to v2 Render API early | Build fail on `SetParametersAndRender` references | Occurred + fixed; logged to cerebrum |
| CascadingValue pattern fragility for generic components | Medium | High | Adopted `IXxxSettingsSink` interface decouple pattern | MariloWizard bug class as reference detection | Mitigation proven via MariloMultiSelect `IMultiSelectSettingsSink` |
| Theme token drift between light and dark modes | Medium | Medium | Co-locate `:root` + `[data-marilo-theme="dark"]` in `_colors.scss` | Visual parity baselines per mode | Mitigation worked; a few component-local dark patches still required |
| Subagent batch quality below direct implementation | Medium | Medium | Spot-check first batch, expand only if quality holds | Per-batch closure review | Batch 7 proved viable; batches 7–8 delivered successfully |

---

## CONDITIONAL: Delegation & Team Strategy

- **Direct implementation:** T1 primitives, T2 shared infrastructure (overlay, JS interop, DataGrid core), T2 Editor, T2 DataGrid feature passes.
- **Subagent delegation (Batch 7–8 pattern, 2026-04-08 / 2026-04-09):** Repetitive T4 picker work split across subagent batches with explicit input contracts (gap list, target files, existing patterns to mirror) and output contracts (source change + bUnit test + tracking-table update).
- **Workspace routing (post-closure):** Further work routes through CDW/ICM workspaces (see `.claude/skills/*-delivery/` and `*-gap-analysis/` stages). This plan does not accept new gaps.

**Interface contract for subagent batches:**
- Input: gap ID list, component path, existing sibling component to mirror, test template.
- Output: source files (Razor + code-behind + `.cs` partials), SCSS provider files for FluentUI + Bootstrap, bUnit test file, tracking table row update, per-component batch closure entry in this file's history.
- Verification: spot-check + bUnit run + visual parity baseline refresh.

---

## CONDITIONAL: Reference Library

> UPF mandates this section for Software domain plans with 3+ phases.

| Source | Version/Date | What it informed | Link |
|---|---|---|---|
| ASP.NET Core Blazor — EditContext / DataAnnotationsValidator | .NET 10 docs | Form/Validation pipeline | https://learn.microsoft.com/aspnet/core/blazor/forms/ |
| ASP.NET Core Blazor — `<Virtualize>` | .NET 10 docs | DataGrid row virtualization, MultiSelect | https://learn.microsoft.com/aspnet/core/blazor/components/virtualization |
| MudBlazor (MIT) | 2025 | CSS-variable theme pattern reference (no code copied) | https://mudblazor.com/ |
| Radzen.Blazor (MIT) | 2025 | Parameter-naming convention reference | https://www.radzen.com/blazor-components/ |
| Blazorise (Apache-2.0) | 2025 | Provider-first architecture reference | https://blazorise.com/ |
| Tiptap / ProseMirror (MIT) | 2025 | Rich-text engine evaluation — in-house path chosen | https://tiptap.dev/ |
| HtmlSanitizer (MIT) | 2025 | Editor XSS sanitization | https://github.com/mganss/HtmlSanitizer |
| Floating UI (MIT) | 2025 | Anchor-based popover positioning | https://floating-ui.com/ |
| IMask.js (MIT) | 2025 | MaskedInput client-side mask enforcement | https://imask.js.org/ |
| bUnit v2 | 2025 | Component testing — `Render(parameters => …)` rebind API | https://bunit.dev/ |
| WCAG 2.1 AA | W3C | Accessibility target for all components | https://www.w3.org/TR/WCAG21/ |

License audit: all adopted libraries are MIT or Apache-2.0, compatible with project's Unlicense release (see CLAUDE.md "Licensing and Third-Party Usage").

---

## CONDITIONAL: Post-Completion Plan

- **Monitor:** CDW/ICM workspace completion across components; behavior-parity follow-ups tracked per workspace.
- **Maintain:** Tracking table in original file kept as historical record; NOT updated with new gaps (new gaps route through CDW).
- **If fails:** Regressions flow into `.wolf/buglog.json` and CDW `gap-analysis-resolution` stages. Spec drift flows into `*-delivery` workspace spec-review stage.
- **Cerebrum marker:** "Gap-analysis-resolution workspace is COMPLETE (as of 2026-04-10)" — see `.wolf/cerebrum.md`. Any revival of this plan must update that marker first.

---

## CONDITIONAL: Learning & Knowledge Capture

The original [GAP_ANALYSIS_RESOLUTION_PLAN.md](GAP_ANALYSIS_RESOLUTION_PLAN.md) carries the full execution log. Key sections worth citing:

- **[T4 Component Audit — Detailed Findings](GAP_ANALYSIS_RESOLUTION_PLAN.md#7-t4-component-audit--detailed-findings-2026-04-02)** — cross-component gap patterns and per-component audit results.
- **[T4 Picker Batches 1–8](GAP_ANALYSIS_RESOLUTION_PLAN.md#t4-picker-batch-1--implementation-tracking)** — the subagent-driven batch workflow that proved subagent delegation viable for repetitive gap closure.
- **[Phase 2.5 — Post-Reconstruction Fixes](GAP_ANALYSIS_RESOLUTION_PLAN.md#phase-25--post-reconstruction-fixes)** — `GAP-readonly-guards` and `GAP-expandall-lazyload` closures.
- **[DataGrid Header Alignment Fix](GAP_ANALYSIS_RESOLUTION_PLAN.md#datagrid-header-alignment-fix-2026-04-04)** — canonical example of the width-alignment architecture referenced by AllocationScheduler and TreeList.
- **[Icon System Upgrade](GAP_ANALYSIS_RESOLUTION_PLAN.md#icon-system-upgrade)** — Tabler Icons default + multi-provider icon architecture.
- **[Post–Step 07 Routing](GAP_ANALYSIS_RESOLUTION_PLAN.md#8-poststep-07-routing-from-executive-report-2026--04-03)** — routing table for executive-report findings.

Cross-cutting patterns extracted to `.wolf/cerebrum.md`:
- Generic-parent + non-generic-children cascade via `IXxxSettingsSink` (fixes MariloWizard bug class).
- Dispatcher-safe public state APIs (`InvokeAsync(StateHasChanged)`).
- Per-item render callback cached args pattern.
- DataGrid width-alignment architecture → reused by AllocationScheduler, TreeList.
- Bootstrap dark-mode `[data-marilo-theme="dark"], [data-bs-theme="dark"]` patch pattern.

---

## CONDITIONAL: Completion Gate

Verified at plan closure (2026-04-10):

- ☑ **Registration** — every resolved component registered in `ComponentRegistry.cs` and `component-mapping.json`.
- ☑ **Connections** — cross-references present: spec → source → demo → tests → tracking table.
- ☑ **Documentation** — specs under `docs/component-specs/<slug>/` populated; SCSS provider contract documented in `.wolf/cerebrum.md`.
- ☑ **Orphan detection** — placeholder scan returns zero; no unreferenced component stubs.
- ☑ **Consistency** — parameter naming audited; enum namespaces migrated to `Marilo.Core.Enums`; DataGrid table semantics standardized.

---

## Stage 0 Discovery Summary

Reconstructed retrospectively from the original plan content.

- **0.1 Existing Work Audit:** Prior Marilo component tree + 87-component `GAP_ANALYSIS_INDEX.md` existed; 32 extra components discovered in registry only on 2026-04-10 (partial Stage 0 miss — see Hardening Log).
- **0.3 Official Docs Check:** ASP.NET Core Blazor docs consulted for `EditContext`, `<Virtualize>`, component lifecycle.
- **0.7 Feasibility:** Provider-first architecture proven viable by MudBlazor + Radzen + Blazorise precedent.
- **0.8 ROI:** Must-do — library cannot ship with half-broken components.
- **0.9 AHA Effect:** Considered forking an existing OSS Blazor library vs. continuing in-house. Decision: continue in-house because provider-first abstraction is unique and no existing library offers it; OSS libraries used as *pattern references only*, not code copies.
- **0.11 Constraint Discovery:** Unlicense release → all dependencies must be MIT / Apache-2.0 / BSD / public domain. Enforced in Reference Library license audit.
- **0.12 People Risk:** Solo project, no approval chain.

---

## Hardening Log

This plan was retrospectively wrapped into UPF on 2026-04-11, after all four phases closed. The Stage 1.5 Autonomous Hardening pass below is applied *to the wrapper*, not to the historical execution (which has already happened and cannot be mutated).

| Perspective | Finding | Action |
|---|---|---|
| **Outside Observer** | End State was implicit across the original plan but not stated in one paragraph. | Added explicit End State section at the top of this wrapper. |
| **Pessimistic Risk Assessor** | FAILED conditions were absent from the original plan (UPF anti-pattern #11, Zombie Project). | Added FAILED-1 through FAILED-5 retrospectively; all marked "did not trigger" based on actual history. |
| **Pedantic Lawyer** | Original gates contained words like "implement", "resolved" without binary observable criteria. | Rewrote phase gates as binary pass/fail checks tied to observable artifacts (test pass, DOM assertion, registry audit, scan result). |
| **Skeptical Implementer** | Original plan had no explicit Assumptions & Validation section. | Reconstructed from embedded "External Research" rows and dependency notes. |
| **The Manager** | Original plan ran >10h effort without a formal Resume Protocol (UPF anti-pattern #6). *Not fixable retrospectively — plan is closed.* | Noted; future gap plans to include Resume Protocol from Phase 1. |
| **Devil's Advocate** | 2026-04-10 discovery of 32 additional components in the registry means Stage 0 Existing Work Audit (Check 0.1) was incomplete at plan start. | Logged as `[REPLANNED: inventory discovery]` on the supplementary Prototype API Completion Pass. |

**Structural fixes applied:** Added End State, FAILED conditions, Assumptions table, Gate rewrites, Reference Library, Completion Gate, Stage 0 summary.

**Cannot mutate (historical):** Phases, component scope, Stage 0 discoveries, dependency ordering — all locked by execution history.

**[Stage 1.5 Note:]** Future gap-analysis plans should be born UPF-native: include Resume Protocol from day one (UPF anti-pattern #6 detection), run Stage 0 Check 0.1 (Existing Work Audit) against both `GAP_ANALYSIS_INDEX.md` *and* `ComponentRegistry.cs` to avoid the 32-component inventory gap that caused the 2026-04-10 supplementary pass. A UPF template for the next plan is recommended — see the `gap-analysis-resolution` skill workspace for where it should live.

---

## Anti-Pattern Self-Audit

| # | Pattern | Status |
|---|---|---|
| 1 | Unvalidated Assumptions | ✅ Fixed — Assumptions table with VALIDATE BY + IMPACT IF WRONG |
| 2 | All-or-Nothing Phases | ✅ Scoped phases with observable gates |
| 3 | Vague Success | ✅ Measurable success criteria + FAILED conditions |
| 4 | No Rollback | ✅ Rollback section (git revert, no schema) |
| 5 | Plan Ends at Deploy | ✅ Post-Completion Plan present |
| 6 | No Resume Protocol | ⚠️ Historical gap — flagged for future plans |
| 7 | Parallel Without Contracts | ✅ Subagent batch interface contract documented |
| 8 | Blind Delegation Trust | ✅ Spot-check + bUnit + visual parity verification |
| 9 | Skipping Stage 0 | ⚠️ Partial — reconstructed Stage 0 summary; original Existing Work Audit missed 32 components (logged) |
| 10 | First Idea = Final | ✅ AHA Effect noted — fork vs in-house considered |
| 11 | Zombie Project | ✅ FAILED conditions + timeout added retrospectively |
| 12 | Timeline Fantasy | ✅ Scope-based phases per UPF coding-domain rule; no hours |
| 13 | Confidence Without Evidence | ✅ Historical outcomes cited per risk |
| 14 | Wrong Level of Detail | ✅ Details live in original file + cerebrum, not here |
| 15 | Premature Precision | ✅ No uncited numbers in this wrapper |
| 16 | Hallucinated Effort Estimate | ✅ No effort estimates |
| 17 | Delegation Without Context Transfer | ✅ Subagent interface contract specified |
| 18 | Unverifiable Gates | ✅ Binary observable gates |
| 19 | Missing Tool Fallback | ✅ OSS library fallbacks listed |
| 20 | Discovery Amnesia | ✅ Stage 0 summary reconstructed; every finding addressed |
| 21 | Assumed Facts | ✅ All facts sourced to original file or `.wolf/cerebrum.md` |

**Grade:** B (Solid) — all 5 CORE + 8 CONDITIONAL sections, FAILED conditions defined, Confidence Level assigned, Cold Start Test passes (plan is self-contained with pointers to the execution log). Grade A is not achievable retrospectively because Resume Protocol was not in place during execution (anti-pattern #6).

---

## Replanning Triggers

This plan is closed; these triggers apply only if it is revived:

- A new component is added to `ComponentRegistry.cs` that is not in the tracking table.
- A deferred behavior-parity follow-up is promoted to must-do (e.g., Map tile rendering becomes a shipping blocker).
- A breaking change to a provider contract is proposed (would require a new plan, not a replan of this one).
- CDW/ICM workspace pipeline cannot absorb new gaps, forcing re-centralization of gap tracking.

If any fires: start a new UPF plan from scratch rather than replanning this one.
