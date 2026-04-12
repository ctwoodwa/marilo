# Gap-Analysis-Resolution: Consolidated Remediation Plan

> **Status:** Stage 04 output — updated 2026-04-11 (rev 2: delivery-gate findings incorporated)
> **Source:** All 41 Stage 03 resolution files under `stages/03-resolution-design/output/`; DataGrid delivery report (`datagrid-delivery/stages/04-sync-check/output/datagrid-delivery-report.md`); DataSheet delivery report (`datasheet-delivery/stages/04-sync-check/output/datasheet-delivery-report.md`)
> **Scope:** Cross-cutting gaps across all Marilo components (Navigation/TreeView, DataGrid, Chart, Editor, Forms, Pickers, Layout, Utilities) **plus delivery-pipeline blockers for DataGrid and DataSheet**
> **Stage routing:** `systematic` — this plan covers gaps across multiple disjoint areas; the full pipeline applies.

---

## Executive Summary

| Metric | Value |
|--------|-------|
| Total resolution records | 100+ individual resolutions across 41 batch files |
| Distinct pattern families | 9 |
| Remediation phases | 5 (Foundation → Pilot → Rollout → Enforcement → Delivery Gate) |
| Tasks in Foundation | 8 |
| Tasks in Pilot | 14 |
| Tasks in Rollout | 28 |
| Tasks in Enforcement | 6 |
| Tasks in Delivery Gate | 18 |
| **Total tasks** | **74** |
| Components affected | 20+ |
| Already-implemented (no code change) | ~35 resolutions (retroactive records / pre-existing) |
| Pending implementation (requires code changes) | ~45 resolutions |
| Delivery-gate blockers (DataGrid) | 12 (9 checklist + 3 category-critical) |
| Delivery-gate blockers (DataSheet) | 6 remaining post-Wave-5 (dominant: UD-01 theming architecture + V03 range selection) |

### Implementation Status at Plan Date

Most TreeView gaps (Phases 1-3) are **already implemented** — Stage 03 records are retroactive. The pending work is concentrated in DataGrid advanced features, Chart polish, Editor enhancements, and T4 Picker completions. All T4 Picker batches 1-8c are **already implemented** (Stage 06 closed). DataGrid phases 1-3, Chart batch 1-2, Editor batch 1 and 2a are **already implemented**. The remaining open items are: DataGrid frozen columns, DataGrid row drag-drop, Editor adaptive toolbar, Editor table/image resize, DropZone JS interop, and ThemeProvider wrapper.

Delivery pipeline work (Phase 5) is net-new work surfaced by the DataGrid and DataSheet CDW passes (Waves 1–5, completed 2026-04-11). These tasks address spec drift, missing demo scenarios, visual-parity SCSS gaps, and one orchestrator-required naming decision. They do not overlap with Phases 1–4 (which are source-behavior tasks); Phase 5 is spec, demo, SCSS, and provider work only.

---

## Pattern Families

Nine distinct cross-cutting patterns emerge from the Stage 03 resolutions. Each maps to a pilot component and a rollout set.

| # | Pattern Family | Description | Pilot Component |
|---|---------------|-------------|-----------------|
| PF-1 | **Shared Core Models** | Shared event args / enums in `Marilo.Core` | `PopupEventArgs` → all pickers |
| PF-2 | **ReadOnly/Disabled Guards** | Two-parameter access-state guards (Disabled blocks all; ReadOnly allows navigation) | `MariloTreeView` |
| PF-3 | **Pass-Through Wrapper Components** | Transparent `ChildContent` wrappers for spec-compatible tag nesting | `MariloSplitterPanes` |
| PF-4 | **CSS Provider Delegation** | Components delegate visual classes to `IMariloCssProvider`; inline styles for layout | `MariloStack` |
| PF-5 | **CascadingValue Child Registration** | Parent cascades `this`; children register in `OnInitialized`, unregister in `Dispose` | `MariloWizard` / `MultiSelectSettings` |
| PF-6 | **Filter/Predicate API** | Public filter methods with ancestor-preserving logic | `MariloTreeView.FilterFunc` |
| PF-7 | **LazyLoad + ExpandAll Extension** | Opt-in `includeUnloaded` with depth + cancellation | `MariloTreeView.ExpandAllAsync` |
| PF-8 | **Format Converter Interface** | `IEditorFormatConverter` for pluggable import/export | `MariloEditor` |
| PF-9 | **JS Interop Module Pattern** | ES module + C# service + `IAsyncDisposable` teardown | DropZone / Editor JS interop |

---

## Phase 0: Prerequisites (Already Done — Verify Before Starting)

Before any phase begins, verify the following are in place:

| Check | How to Verify |
|-------|---------------|
| `dotnet build Marilo.slnx` exits 0 | Run from solution root |
| All Stage 06 closure reports exist for "already implemented" batches | Check `stages/06-validate/output/gap-*-closure-report.md` |
| `Marilo.Core.Models.PopupEventArgs` exists | Grep for `PopupEventArgs` in `src/Marilo.Core/` |
| `MariloStack` uses `Orientation` (not `Direction`) | Grep for `[Parameter] public StackDirection Orientation` |
| T4 picker tests all pass (1097+/1097+) | `dotnet test --filter "Category=Pickers"` or full suite |

---

## Phase 1: Foundation

**Purpose:** Establish or verify shared infrastructure that all subsequent phases depend on. No component-specific behavior changes here — only shared types, base patterns, and infrastructure services.

**Entry criteria:** `dotnet build Marilo.slnx` exits 0. No pending merge conflicts in `Marilo.Core`.

**Exit criteria:** All Foundation tasks pass their done criteria. `dotnet build` remains green. No breaking changes to existing tests.

**Estimated scope:** 8 tasks across 4 files in `Marilo.Core` + 2 service files.

**Rollback:** All Foundation tasks are purely additive (new types, new methods). Roll back by reverting the individual file. No existing behavior changes.

### Foundation Tasks

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| F-01 | FOUND-POPUP-ARGS | `src/Marilo.Core/Models/PopupEventArgs.cs` | Verify `PopupEventArgs` exists with `IsCancelled` property; create if missing | File exists; `PopupEventArgs.IsCancelled` is a `bool` property; builds without error |
| F-02 | FOUND-FORM-ENUMS | `src/Marilo.Core/Enums/FormEnums.cs` | Verify `FormOrientation`, `FormValidationMessageType`, `FormButtonsLayout` enums exist; create if missing | All 3 enums exist in `Marilo.Core.Enums`; `dotnet build` green |
| F-03 | FOUND-FILTER-ENUMS | `src/Marilo.Core/Enums/ComponentEnums.cs` | Verify `AdaptiveMode` (None, Auto) enum exists; create if missing | `AdaptiveMode` enum resolvable from `Marilo.Core.Enums` |
| F-04 | FOUND-GRID-ENUMS | `src/Marilo.Core/Enums/GridEnums.cs` | Verify `GridSortMode`, `GridSelectionUnit`, `GridColumnFrozenPosition`, `GridRowDropPosition` enums exist | All 4 enums present; `GridSortMode` has Single/Multiple values |
| F-05 | FOUND-COMPOSITE-FILTER | `src/Marilo.Core/Data/CompositeFilterDescriptor.cs` | Verify `CompositeFilterDescriptor` + `FilterCompositionOperator` exist; create if missing | Both types exist in `Marilo.Core.Data`; `GridState.CompositeFilterDescriptors` property compiles |
| F-06 | FOUND-DROPZONE-SVC | `src/Marilo.Components/Interop/IDropZoneService.cs` + `DropZoneService.cs` | Verify `IDropZoneService` / `DropZoneService` exist with `RegisterAsync`/`UnregisterAsync`; create if missing | Interface and implementation exist; registered in DI via `AddMariloInteropServices()` |
| F-07 | FOUND-EDITOR-CONVERTER | `src/Marilo.Components/Editors/IEditorFormatConverter.cs` | Verify `IEditorFormatConverter` interface with `Format`, `ToHtml`, `FromHtml` exists; create if missing | Interface exists; `MariloMarkdownConverter` (Markdig-backed) implementation registered in DI |
| F-08 | FOUND-JS-DROPZONE | `wwwroot/js/marilo-dropzone.js` | Verify ES module with `registerDropZone` / `unregisterDropZone` exports exists; create if missing | File exists with both exports; called correctly by `DropZoneService` |

---

## Phase 2: Pilot

**Purpose:** Apply each pattern family to one representative component, establishing a verified reference implementation before rolling out to remaining components. Pilots are chosen for minimal blast radius and maximum pattern clarity.

**Entry criteria:** All Phase 1 tasks complete (Foundation tasks done or confirmed pre-existing). `dotnet build` and full test suite pass.

**Exit criteria:** Each pilot task has passing bUnit tests. `dotnet build` green. Pattern reviewed and approved before Rollout begins.

**Estimated scope:** 14 tasks across ~12 component files + test files.

**Rollback:** Each pilot task is scoped to a single component. Revert the component file + its test file. No shared infrastructure modified.

### Pilot Tasks

#### PF-2 Pilot: ReadOnly/Disabled Guards — `MariloTreeView` (DONE; verify)

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| P-01 | PILOT-RO-DRAGDROP | `MariloTreeView.razor.cs` line ~480, ~692 | Verify ReadOnly guards on `HandleDrop()` and drag handler attachment; add `if (ReadOnly) return;` at top of `HandleDrop` and `EnableDragDrop && !Disabled && !ReadOnly` guard if missing | SC-1 and SC-2 from RES-readonly-guards pass in bUnit |
| P-02 | PILOT-RO-EXPAND | `MariloTreeView.razor.cs` line ~501, `MariloTreeItem.razor` line ~16 | Verify ExpandOnClick guard includes `!ReadOnly`; verify toggle button `disabled` attr checks `ReadOnly` | SC-3, SC-4, SC-8 from RES-readonly-guards pass |

#### PF-3 Pilot: Pass-Through Wrapper — `MariloSplitterPanes` (DONE; verify)

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| P-03 | PILOT-WRAPPER-SPLITTER | `src/Marilo.Components/Layout/MariloSplitterPanes.razor` | Verify file exists; renders `@ChildContent` only; no logic | Component exists; panes register correctly through wrapper; backward-compatible direct-child usage still works |

#### PF-5 Pilot: CascadingValue Child Registration — `MariloWizard` (DONE; verify)

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| P-04 | PILOT-CASCADE-WIZARD | `MariloWizard.razor` | Verify `<CascadingValue Value="this" IsFixed="true">@ChildContent</CascadingValue>` wraps child content; verify `Value` parameter (`@bind-Value`) replaces `ActiveStepIndex` | `_steps.Count` matches WizardStep children count; step labels render in stepper; active step content renders |

#### PF-7 Pilot: LazyLoad + ExpandAll Extension — `MariloTreeView` (DONE; verify)

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| P-05 | PILOT-EXPANDALL-LAZY | `MariloTreeView.razor.cs` | Verify `ExpandAllAsync(bool includeUnloaded, int maxDepth, CancellationToken)` signature | SC-1 (backward compat), SC-2 (lazy load fires), SC-4 (maxDepth respected), SC-5 (cancellation) pass in bUnit |

#### PF-8 Pilot: Format Converter Interface — `MariloEditor` (DONE; verify)

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| P-06 | PILOT-EDITOR-IMPORT | `MariloEditor.razor` / `.razor.cs` | Verify `ImportAsync(string format, string content)` and `ExportAsync(string format)` public methods exist; verify Markdig converter registered | Both methods callable; `ImportAsync("markdown", ...)` roundtrips through Markdig to HTML; 8 bUnit tests pass |

#### PF-9 Pilot: JS Interop Module — DropZone (OPEN)

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| P-07 | PILOT-DROPZONE-FU | `MariloFileUpload.razor` + `.razor.cs` | Wire `IDropZoneService`; call `RegisterAsync` in `OnAfterRenderAsync(firstRender)` when `DropZoneId` is set; call `UnregisterAsync` in `DisposeAsync` | Dropping a file onto an external element with the configured ID triggers file selection; bUnit interop test confirms registration call; `DisposeAsync` cleans up |
| P-08 | PILOT-DROPZONE-UPL | `MariloUpload.razor` + `.razor.cs` | Same wiring as P-07 (MariloUpload variant) | Same criteria as P-07 for MariloUpload |

#### DataGrid Foundation Pilots (DONE; verify)

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| P-09 | PILOT-DG-SORTMODE | `MariloDataGrid.razor.cs`, `GridEnums.cs` | Verify `SortMode` parameter + `GridSortMode.Single/Multiple` enum + single-sort logic in `OnHeaderClick` | 18 Phase 1 bUnit tests pass; `SortMode.Single` prevents multi-sort |
| P-10 | PILOT-DG-FILTER-API | `MariloDataGrid.Data.cs` | Verify `AddFilter`, `ClearFilters`, `AddCompositeFilter`, `ClearCompositeFilters` public methods | Methods callable; composite OR/AND filters work with `CompositeFilterDescriptor` |
| P-11 | PILOT-THEME-WRAPPER | `MariloThemeProvider.razor` | Verify wrapper `<div>` with `GenerateThemeStyles()`, `data-marilo-theme`, `dir` attributes; verify `SetTheme` async method | ThemeProvider emits wrapper div; CSS variables present on element; dark mode toggles `data-marilo-theme="dark"` |

#### Chart Pass-Through Wrappers Pilot (DONE; verify)

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| P-12 | PILOT-CHART-WRAPPER | `ChartSeriesItems.razor`, `ChartCategoryAxes.razor` | Verify both pass-through wrapper files exist | Both components render children identically to direct nesting; bUnit test verifies wrapper |
| P-13 | PILOT-CHART-SUBTITLE | `ChartSubtitle.razor`, `ChartTitle.razor` | Verify `ChartSubtitle` registers with `ChartTitle` via CascadingValue; verify subtitle renders below main title | Subtitle text appears in SVG output; `Position` parameter controls placement |
| P-14 | PILOT-CHART-BUBBLE | `MariloChart.razor` | Verify `case ChartSeriesType.Bubble:` rendering path exists in the cartesian rendering switch | Bubble series renders circles with scaled radius from `BubbleSize`; no silent fallthrough |

---

## Phase 3: Rollout

**Purpose:** Apply verified patterns from Phase 2 systematically to all remaining components and gaps. Tasks are grouped by pattern family to minimize context switching.

**Entry criteria:** All Phase 2 pilots reviewed and approved. `dotnet build` green. Full test suite pass at pilot completion.

**Exit criteria:** All rollout tasks done. Test suite expanded to cover new behavior. `dotnet build` green. Demo pages updated.

**Estimated scope:** 28 tasks across ~25+ files.

**Rollback:** Rollout tasks are grouped by component. Roll back one component at a time by reverting its files. Pattern pilots remain in place.

### PF-3 Rollout: Remaining Pass-Through Wrappers

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| R-01 | ROLL-WRAPPER-WIZARD | `MariloWizardSteps.razor` (new) | Verify or create `MariloWizardSteps` pass-through wrapper (same pattern as `MariloSplitterPanes`) | `<MariloWizardSteps>` wraps step children; existing direct-child usage still works |
| R-02 | ROLL-WRAPPER-CHART | Already done (P-12/P-13) | — | Verified in Pilot |

### PF-4 Rollout: CSS Provider + Layout Parameters

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| R-03 | ROLL-CSS-DRP | `MariloDateRangePicker.razor` | Verify `DateRangePickerClass()` CSS provider method called; verify `Size`, `Rounded`, `FillMode` appearance parameters wired | Parameters accepted; CSS provider called; 23 bUnit tests pass |
| R-04 | ROLL-CSS-DTP | `MariloDateTimePicker.razor` | Verify `DateTimePickerClass()` CSS provider method + `ValidateOn` parameter | Provider method called; `ValidateOn` parameter present |
| R-05 | ROLL-CSS-TP | `MariloTimePicker.razor` | Verify `TimePicker` CSS provider method + `InputMode` + `ValidateOn` + `OnChange-on-blur` | 13 bUnit tests pass; CSS provider delegated correctly |
| R-06 | ROLL-CSS-FU | `MariloFileUpload.razor` | Verify `FileUpload` CSS provider delegation for drop-zone styling | CSS provider method called; `FileUploadTemplateContext` wrapper type used |
| R-07 | ROLL-GRID-CSS | `MariloGridLayout.razor` | Verify dual-mode rendering (flex vs CSS Grid); `Columns`/`Rows`/`ColumnSpacing`/`RowSpacing`/`Width`/`HorizontalAlign`/`VerticalAlign` parameters wired | CSS Grid mode activates when Columns/Rows set; flex mode unchanged; 7 criteria in RES-GRID-001 all pass |

### PF-5 Rollout: CascadingValue Child Registration — Remaining Components

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| R-08 | ROLL-CASCADE-MSEL-SETTINGS | `MultiSelectSettings.razor`, `MultiSelectPopupSettings.razor`, `IMultiSelectSettingsSink.cs` | Verify both child components exist and register via `IMultiSelectSettingsSink` interface; verify `MariloMultiSelect` implements sink | Child component settings applied on parent; 7 bUnit tests pass |
| R-09 | ROLL-CASCADE-CHART-SUBTITLE | Already covered in P-13 | — | Verified in Pilot |

### DataGrid Phase 2 Rollout

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| R-10 | ROLL-DG-VALIDATION | `MariloDataGrid.razor` (popup body) | Verify popup `EditForm` wraps with `DataAnnotationsValidator` + `ValidationSummary`; Save button is `type="submit"` | Popup form blocks save on invalid data; cancel bypasses validation; 15 Phase 2 bUnit tests pass |
| R-11 | ROLL-DG-AUTOGEN | `MariloDataGrid.razor.cs` (GenerateColumnsFromModel) | Verify `[Display]`/`[Editable]` attribute respect in column auto-generation | `[Display(AutoGenerateField=false)]` skips; `[Display(Name)]` sets title; `[Editable(false)]` sets column Editable=false |
| R-12 | ROLL-DG-AGGREGATES | `GridGroupHeaderContext.cs` | Verify `Sum`, `Average`, `Min`, `Max` methods on `GridGroupHeaderContext<TItem>` | Methods callable in `GroupHeaderTemplate`; type-safe with generic selectors |
| R-13 | ROLL-DG-EXPORT | `MariloDataGrid.razor.cs` | Verify `OnBeforeExport`/`OnAfterExport` events + `ExportAllPages` parameter | `OnBeforeExport` can cancel; `ExportAllPages=false` exports only current page |
| R-14 | ROLL-DG-CANCELTOKEN | `GridReadEventArgs.cs` | Verify `CancellationToken` property exists; verify previous CTS is cancelled on new request | Token cancellable; consumer can use in `OnRead` handler |
| R-15 | ROLL-DG-STATE | `MariloDataGrid.razor.cs` | Verify `SetStateAsync(GridState)`, `GetState().ExpandedItems` wired, `AddFilter`/`ClearFilters` | `SetStateAsync` applies all state properties and reprocesses; `ExpandedItems` populated in `GetState()` |

### DataGrid Phase 3 Rollout (Advanced)

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| R-16 | ROLL-DG-FROZEN | `MariloGridColumn.razor`, `MariloDataGrid.Rendering.cs`, `GridLayoutContract.cs` | Implement `Locked` + `FrozenPosition` parameters; sticky CSS via `position:sticky` with offsets from `FixedWidthProvider`; all 5 render zones (header, filter, body, footer, colgroup) | Columns stay sticky during horizontal scroll; `mar-datagrid-col--locked` CSS class applied; locked column falls back to 150px if no explicit width |
| R-17 | ROLL-DG-ROWDRAGDROP | `MariloDataGrid.razor`, `MariloDataGrid.Rendering.cs`, IIFE JS | Implement `RowDraggable` + `OnRowDrop` event; drag handle column; JS `initRowDrag()` extension | Row drag-and-drop fires `OnRowDrop`; `DestinationItem`, `DestinationIndex`, `DropPosition` correct; `IsCancelled` prevents drop |
| R-18 | ROLL-DG-CELL-SELECT | `MariloDataGrid.razor.cs` | Verify `SelectionUnit` parameter + `SelectedCells`/`SelectedCellsChanged` + `GridCellReference<TItem>` model (already implemented in Phase 3) | Cell selection works in Single and Multiple modes; 10 Phase 3 bUnit tests pass |
| R-19 | ROLL-DG-CHECKBOXLIST | `MariloDataGrid.razor` | Verify `GridFilterMode.CheckBoxList` enum value + popup checkbox filter (already implemented in Phase 3) | Distinct values populated; Apply creates composite OR filter; 10 Phase 3 tests pass |

### Chart Rollout

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| R-20 | ROLL-CHART-EVENTS | `MariloChart.razor` | Verify `OnRender` EventCallback + `ChartRenderEventArgs` fires after render; `Transitions` typed as `bool?` | `OnRender` fires with chart dimensions; `Transitions=null` preserves default animations |
| R-21 | ROLL-CHART-TOOLTIP | `ChartTooltip.razor` | Verify `Template` (`RenderFragment<ChartTooltipContext>?`) + `Shared` parameters + `ChartTooltipContext` model | Template renders custom content; `Shared` shows all series values at same category |
| R-22 | ROLL-CHART-CSS | `MariloChart.razor` | Verify CSS variable bridge emits `--mar-chart-series-{N}` on SVG container element | CSS variables present in rendered output; external CSS can override series colors |
| R-23 | ROLL-CHART-TESTS | `ChartTests.cs` | Expand from 5 to 15+ tests covering all RES-CHART-* resolutions | 15+ tests pass; covers series types, events, child components, subtitle, CSS vars, accessibility |

### Editor Rollout

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| R-24 | ROLL-EDITOR-VALIDATION | `MariloEditor.razor` / `.razor.cs` | Verify `ValueExpression` + `EditContext` cascading param + `NotifyFieldChanged` on debounced change | Data annotation validation works with editor in EditForm; no regression outside form |
| R-25 | ROLL-EDITOR-CUSTOM-TOOLS | `MariloEditor.razor`, `EditorCustomTool.cs` | Verify `CustomTools` parameter + `EditorCustomTool` model + toolbar rendering | Custom tools render after built-in tools; `OnClick` fires; `Template` custom tool renders |
| R-26 | ROLL-EDITOR-ADAPTIVE | `MariloEditor.razor` / `.razor.cs` | Implement `Adaptive` parameter; wire `IResizeObserverService`; `_overflowStartIndex` + overflow "More" button | When toolbar overflows, overflow button appears; hidden tools accessible via popup; no regression when `Adaptive=false` |
| R-27 | ROLL-EDITOR-TABLE-RESIZE | `MariloEditor.razor` (GetEditorScript IIFE) | Add table column/row resize + image resize drag handles to inline JS module | Drag on table column border resizes column; drag on image corner scales image; `onInput` sync fires |

### Form Rollout

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| R-28 | ROLL-FORM-VERIFY | `MariloForm.razor`, `MariloValidationMessage.razor`, `MariloValidationSummary.razor`, `MariloValidationTooltip.razor`, `MariloField.razor`, `MariloLabel.razor` | Verify all 6 RES-FORM-* resolutions implemented: EditContext, Model, submit events, 3 validation components, field/label enhancements | 20 FormTests pass; `EditContext` cascaded; `ValidationSummary` shows errors; `OnValidSubmit`/`OnInvalidSubmit` fire correctly |

---

## Phase 4: Enforcement

**Purpose:** Add guardrails, documentation fixes, and test coverage to prevent regression and close documentation-vs-code mismatches.

**Entry criteria:** All Phase 3 Rollout tasks complete. Test suite passing.

**Exit criteria:** All enforcement tasks done. No compile warnings added. Docs and spec pages updated.

**Estimated scope:** 6 tasks.

**Rollback:** Enforcement tasks are documentation and test additions only. Revert individual files. No behavior changes.

### Enforcement Tasks

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| E-01 | ENFC-ICON-DOCS | `docs/component-specs/icon/` | Update `IconFlip` enum docs to include `Both`; update `IconSize` docs to include `ExtraLarge` example; correct `IconThemeColor.Error` → `IconThemeColor.Danger` | Docs reflect actual enum values; no incorrect `Error` references |
| E-02 | ENFC-EDITOR-DOCS | `MariloEditor.razor.cs` (XML docs) | Add XML doc comment on `SanitizeHtml()` and `SanitizeAttr()` documenting allowed/blocked tags and attributes; add `<remarks>` security model block on component class | All public methods have complete XML docs; security model documented |
| E-03 | ENFC-ARIA-PICKERS | `MariloDateRangePicker.razor`, `MariloDateTimePicker.razor`, `MariloTimePicker.razor` | Verify WAI-ARIA 1.2 combobox attributes: `role="combobox"`, `aria-haspopup="dialog"`, `aria-controls` when open, unique popup `id`s | Picker inputs have correct ARIA roles; `aria-controls` targets popup id when open |
| E-04 | ENFC-ADAPTIVE-MODE | All 7 T4 pickers | Verify `AdaptiveMode` parameter (None/Auto enum) accepted on all 7 components with default `None` | No behavior change; parameter accepted without error; `AdaptiveMode.Auto` logs warning until responsive infrastructure added |
| E-05 | ENFC-EDITOR-TESTS | `EditorTests.cs` | Expand from 7 to 15+ tests covering RES-EDITOR-007 (validation), RES-EDITOR-009 (custom tools), edit mode switching, disabled state, events, accessibility | 15+ editor tests pass |
| E-06 | ENFC-GAP-PLAN-UPDATE | `src/Marilo.Components/GAP_ANALYSIS_RESOLUTION_PLAN.md` | Mark all resolved gaps as closed with resolution reference; mark deferred gaps (Gap 18 virtualization, MSEL-007 ScrollMode, editor JS gaps) with deferral reason | Gap plan reflects actual implementation status; no bypass of gap-analysis notes |

---

## Phase 5: Delivery Gate Clearance

**Purpose:** Close the delivery-pipeline blockers surfaced by DataGrid CDW Waves 1–4 and DataSheet CDW Waves 1–5. These are not source-behavior gaps — they are spec drift, missing demo scenarios, visual-parity SCSS absences, and one user-decision item that requires orchestrator arbitration. Phase 5 is intentionally isolated from Phases 1–4 so it can begin in parallel once Phase 2 is underway.

**Entry criteria:** DataGrid delivery report finalized (Gate: BLOCKED, 12 items). DataSheet delivery report Wave 5 finalized (Gate: BLOCKED, 6 items remaining). `dotnet build` green (confirmed at 2026-04-11T18:00Z, exit 0).

**Exit criteria:** All 18 tasks below reach their done criteria. DataGrid and DataSheet delivery gates re-run and reach CLEAR or AMBER. No new test failures introduced.

**Estimated scope:** ~5.5 worker days total across spec-update, demo, and SCSS lanes (excluding Material provider, which is an open-ended track).

**Rollback:** All Phase 5 tasks are documentation, demo, and SCSS additions. Revert individual files. No behavioral source changes.

---

### User-Decision Required Before Starting (Orchestrator-Only)

**D-01 — DataGrid tag-name decision (FU-3):** The spec uses `<MariloGrid>` / `<GridColumn>` / `GridCommandEventArgs`; source uses `<MariloDataGrid>` / `<MariloGridColumn>` / `GridEditEventArgs<TItem>`. Every spec code snippet and the M-01..M-06 mismatch records cascade off this. **Two paths:**
- Path A: Update spec to match source (spec-side rename only, no source change, no consumer impact).
- Path B: Rename source to match spec (public API rename, touches every consumer, requires demo + spec + test update sweep).

**This must be decided by the user before DG spec-update tasks (D-03..D-05) can start.** Path A is strongly recommended (source is the reference; spec can be corrected without breaking consumers).

**D-02 — DataSheet theming architecture decision (UD-01):** `IMariloCssProvider` currently does not expose DataSheet BEM subregion methods. 21 BEM classes are hard-coded in source. **Two paths:**
- Path A: Extend `IMariloCssProvider` with 21 new DataSheet methods (orchestrator-only change per `.claude/rules/orchestration.md` Architecture-Level Changes list).
- Path B: Narrow `theming-and-css-provider.md` spec to state that BEM element classes below the container are component-internal (spec-only edit, no source change).

**This must be decided by the user before all DataSheet visual-parity lanes (DS-12..DS-15) can start.** Path B is lower risk and unblocks the SCSS foundation work without a provider contract change.

**D-03 — DataSheet `AddRowAsync` prepend vs. append (SA-02):** Spec says append-to-end; source inserts at index 0 (prepend). Choose: fix source (low risk, one line) or update spec to match source behavior. Recommend fixing source.

**D-04 — DataSheet `SA-05` saving→saved cell-state transition:** Minor behavioral wording decision. Recommend spec-side clarification to match current source output.

**D-05 — DataSheet 10k-row demo cap (UD-02):** Confirm whether `Marilo.Demo` can host a 10k-row DataSheet scenario or cap at 5k. Affects Wave 2 `EU-01` (virtualization upper threshold demo). Low-stakes scope decision.

---

### DataGrid Spec-Update Batch (worker-tractable; unblocked by D-01 Path A)

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| DG-01 | DG-SPEC-UNDOC | `docs/component-specs/grid/` (multiple pages) | Document 10 undocumented parameters (U-01..U-10): `ShowSearchBox`/`SearchBoxPlaceholder`, `EnableVirtualization`/`VirtualizeOverscanCount`, `Striped`, `AutoGenerateColumns`, grid-level `Resizable`/`Reorderable`, `OnRowContextMenu`, `OnRowExpand`/`OnRowCollapse`, `PagerButtonCount`, `ColumnWidthProvider`, `GridGroupHeaderContext<TItem>`. Also document 7 imperative API methods (SRC-03: `BeginEdit`, `BeginCellEdit`, `BeginAdd`, `SaveEdit`, `CancelEdit`, `DeleteItem`, `ExecuteCommand`). | All 10 parameters appear in at least one spec page; all 7 methods appear in spec events/api page; no `[Parameter]` omissions from spec overview |
| DG-02 | DG-SPEC-NAMES | `docs/component-specs/grid/` (all pages) | [Requires D-01 decision] Fix all stale tag names (M-01 `<MariloGrid>` → `<MariloDataGrid>`, M-02 `<GridColumn>` → `<MariloGridColumn>`) and event-args types (M-06 `GridCommandEventArgs` → `GridEditEventArgs<TItem>`, M-07 shape corrections, NM-01..NM-06 namespace slugs) in every spec code snippet. | `grep -r "MariloGrid[^Column]" docs/component-specs/grid/` finds only `MariloDataGrid`; no `<GridColumn>` in spec code snippets; parameter count in overview matches source (66+) |
| DG-03 | DG-SPEC-EVENTS | `docs/component-specs/grid/events.md` | Document `OnRowContextMenu` (U-06) and `OnRowExpand`/`OnRowCollapse` (U-07) events with signature and usage examples. | Both events appear in `events.md` with correct `EventCallback<TItem>` signatures |

### DataGrid Demo Batch (worker-tractable; no user decision required)

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| DG-04 | DG-DEMO-REFRESH | `src/Marilo.Components.Shell/Pages/DataGrid/` | Create `refresh-data.md` scenario page (A-01 from Wave 2 gap list): external refresh, `OnRead` with `CancellationToken`, `AddFilter`/`ClearFilters` programmatic usage, `SetStateAsync` restore. Also add `NoDataTemplate` empty-state scenario (2.5 checklist). | `refresh-data.razor` page exists; all 4 scenarios build and render without Telerik references |
| DG-05 | DG-DEMO-KEYBOARD | `src/Marilo.Components.Shell/Pages/DataGrid/DataGrid-Navigable.razor` | Replace D4 "Navigable Grid" keyboard shortcut cheat-sheet with "Pending — keyboard navigation not yet implemented" notice, or gate the shortcut table behind a `<!-- TODO: SA-06/07/08 -->` comment. Wave 2 headline #1: demo advertises behavior that source does not implement (no `onkeydown` handler behind `Navigable=true`). | D4 demo no longer advertises unimplemented keyboard shortcuts as functional; honesty defect B-C resolved |
| DG-06 | DG-DEMO-CELL-SEL | `src/Marilo.Components.Shell/Pages/DataGrid/` | Add `cell-selection.razor` demo page covering `SelectionUnit=Cell`, `SelectedCells`, `SelectedCellsChanged`. Source is closed (Phase 3 implementation exists); demo is missing. | `cell-selection.razor` builds; uses current API names; `SelectedCells` collection updates on cell click |
| DG-07 | DG-DEMO-IMPERATIVE | `src/Marilo.Components.Shell/Pages/DataGrid/DataGrid-Overview.razor` or new page | Add imperative edit API demo scenarios for `BeginEdit()`, `SaveEdit()`, `CancelEdit()` with a button-driven workflow. | At least one demo scenario exercises each method; builds without error |

### DataGrid Visual Parity Batch (routes via datagrid-gap-analysis intake; bulk SCSS)

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| DG-08 | DG-SCSS-UNSTYLED | `src/Marilo.Providers.FluentUI/Scss/_data-grid.scss`, `src/Marilo.Providers.Bootstrap/Scss/_data-grid.scss` | Add ~200 LOC of SCSS covering the 19 unstyled `mar-datagrid-*` selectors: `.mar-datagrid-pager-btn`, `.mar-datagrid-empty`, `.mar-datagrid-loading-overlay`, `.mar-datagrid-popup-overlay`, `.mar-datagrid-sort-indicator`, `.mar-datagrid-validation-summary`, `.mar-datagrid-footer-row`, `.mar-datagrid-detail-row`, `.mar-datagrid-col--locked`, `.mar-datagrid-searchbox` (+ 9 more). Drives VP-datagrid-008..015, 018. | All 19 selectors have at least a minimal Fluent and Bootstrap rule; no CSS compilation errors; critical parity gap count drops below 4 |
| DG-09 | DG-SCSS-DARKFIX | `src/Marilo.Providers.FluentUI/Scss/_data-grid.scss`, `src/Marilo.Providers.Bootstrap/Scss/_data-grid.scss` | Replace 7 hardcoded `#fff` literals (4 FluentUI filter-menu, 3 Bootstrap filter-menu) with `var(--marilo-color-surface)` / `var(--marilo-color-background)` tokens. Closes VP-datagrid-013, 014, 019. | `grep -r "#fff" src/Marilo.Providers.*/Scss/_data-grid.scss` returns 0 matches; dark mode filter menu no longer shows white background |
| DG-10 | DG-SCSS-FOCUS | `src/Marilo.Providers.FluentUI/Scss/_data-grid.scss`, `src/Marilo.Providers.Bootstrap/Scss/_data-grid.scss` | Add focused-cell and focused-row rules using `--focus-stroke-outer` foundation token (already defined). Closes VP-datagrid-015. Bundle with D4 demo fix (DG-05) so keyboard navigation engine has a visible focus indicator when it lands. | DataGrid cell/row focus visible in Fluent and Bootstrap; token referenced rather than hardcoded |

---

### DataSheet Source Batch (worker-tractable post D-03/D-04 decisions; Wave 5 partially completed)

> **Note:** SA-01 (tabindex), SA-03 (AddRowAsync active cell), SA-04 (Reset undo buffer), SA-08 (paste guard), SA-09 (double-click edit), SA-13 (aria-live), SA-14 (numeric required), SA-15 (DateTime required) are **already implemented in Wave 5** (2026-04-11). The tasks below are what remains.

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| DS-01 | DS-SOURCE-V03 | `src/Marilo.Components/DataGrid/DataSheetSelection.cs` (new), `MariloDataSheet.razor.cs`, `MariloDataSheet.Rendering.cs` | Implement `DataSheetSelection<TItem>` model, rectangular range selection, `Ctrl+A`, range-scoped Copy/Paste/Fill Down/Delete. Wave 1 V03 (carried). Est. 3.5 days — must be a dedicated worker lane. | `DataSheetSelection<TItem>` model exists; `Ctrl+A` selects all cells; rectangular copy-paste roundtrips; Fill Down populates selected range; bUnit tests cover selection state machine |
| DS-02 | DS-SOURCE-ADDROW | `MariloDataSheet.Data.cs AddRowAsync` | Fix prepend→append order [D-03 decision]. Change `_displayRows.Insert(0, newItem)` to `_displayRows.Add(newItem)` if Path A chosen. | Row appends to end; `AddRowTests` pass |
| DS-03 | DS-SPEC-WORDING | `docs/component-specs/datasheet/` (5 files) | Spec-side wording corrections: `SA-05` cell-state transition text [D-04 decision], `SA-06` Ctrl+D behavioral wording (row-level vs range), `SA-10` DeleteAll guard wording, remaining SA-* items not completed in Wave 5. | All cited spec files match source behavior; no contradictory statements between spec and source |

### DataSheet Demo Batch (worker-tractable; Wave 2 EU items)

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| DS-04 | DS-DEMO-COPYPASTE | `src/Marilo.Components.Shell/Pages/DataSheet/BulkOperations.razor` | Add copy→paste round-trip scenario (EU-02), paste-blocked-during-save scenario (EU-03), Delete-key clears cells scenario (EU-04). | 3 new scenario blocks compile and render; no Telerik references |
| DS-05 | DS-DEMO-CELLTEMPLATE | `src/Marilo.Components.Shell/Pages/DataSheet/` | Add `CellTemplate` demo scenario (EU-08). | `CellTemplate` RenderFragment usage demonstrated; builds |
| DS-06 | DS-DEMO-EMPTY | `src/Marilo.Components.Shell/Pages/DataSheet/` | Add empty-state scenario with `Items` bound to an empty collection to exercise `mar-datasheet__empty` state. | Empty state renders; `mar-datasheet__empty` class emitted |
| DS-07 | DS-DEMO-THEMEOVERVIEW | `src/Marilo.Components.Shell/Pages/DataSheet/` | Create `theming-and-css-provider.razor` demo page (EU-06). Gated on D-02. | Demo page exists [blocked until D-02 lands] |

### DataSheet Visual Parity Batch (gated on D-02; routes via datasheet-gap-analysis intake)

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| DS-08 | DS-SCSS-FOUNDATION | `src/Marilo.Providers.FluentUI/Scss/_data-sheet.scss` (new), `src/Marilo.Providers.Bootstrap/Scss/_data-sheet.scss` (new), `src/Marilo.Providers.Material/Scss/_data-sheet.scss` (new) | [Gated on D-02 Path A or Path B] Create provider SCSS files with rules for all emitted `mar-datasheet*` selectors (VP-datasheet-01 umbrella gap). Path B (spec narrowing) allows creating the SCSS file without adding `IMariloCssProvider` methods — provider owns container class, BEM element classes styled directly. | All three `_data-sheet.scss` files exist and compile; critical parity gap count drops from 8 to <4 |
| DS-09 | DS-SCSS-FLUENT-DETAIL | `src/Marilo.Providers.FluentUI/Scss/_data-sheet.scss` | Fluent selection / editor / validation detailed rules (VP-04/05/06/09). | Selected cells, editor inputs, validation states all have Fluent-compatible token references |
| DS-10 | DS-SCSS-BOOTSTRAP | `src/Marilo.Providers.Bootstrap/Scss/_data-sheet.scss` | Bootstrap bridge rules (VP-10). | Bootstrap provider renders DataSheet with visible state differentiation |

### DataSheet Gap-Analysis Bootstrap

| # | Task ID | Target File(s) | Change | Done Criterion |
|---|---------|---------------|--------|----------------|
| DS-11 | DS-GAP-BOOTSTRAP | `ICM/workspaces/datasheet-gap-analysis/_config/coverage-summary.md` | Bootstrap `datasheet-gap-analysis` workspace from Wave 1 gap list (17+3 entries = 20 open spec gaps). Wave 5 partially bootstrapped Stage 01; complete the coverage-summary.md so checklist items 3.1/3.3/3.5/5.1 become evaluable. | `coverage-summary.md` lists all 20 Wave 1 gaps with phase assignments; no longer a stub |

---

### Phase 5 Dependencies

```
D-01 (naming decision) ──► DG-02 (spec tag-name fixes)
D-02 (theming arch) ──► DS-08, DS-09, DS-10, DS-07 (SCSS and theming demo)
D-03 (AddRow order) ──► DS-02
D-05 (10k-row cap) ──► DS-04 (EU-01 virtualization upper threshold)

DG-08 (unstyled selectors) and DG-09 (dark mode fix) are independent of all decisions.
DS-01 (range selection) is independent of all decisions — it is source work only.
DS-11 (gap-analysis bootstrap) is independent of all decisions.

DG-05 (keyboard honesty fix) must bundle with DG-10 (focus rings) — they land together.
DG-08 + DG-09 + DG-10 form a single DataGrid SCSS PR (~200-250 LOC).
DS-08 + DS-09 + DS-10 form a single DataSheet SCSS PR (new files, 150-200 LOC).
```

### Phase 5 Success Criteria

- DataGrid delivery gate re-run after DG-01..DG-10: expected result AMBER (FU-2/FU-3 spec-ahead backlog and Material provider remain; all others cleared).
- DataSheet delivery gate re-run after DS-01..DS-11 + D-02: expected result AMBER (V03 is a 3.5-day lane; until it lands, selection-and-ranges spec remains ahead of source).
- `dotnet build Marilo.slnx` exits 0 after all SCSS additions.
- No regressions in existing test suite.

---

```
Phase 1 (Foundation)
  └── must complete before Phase 2 (Pilot) can start
      F-01 (PopupEventArgs) ──► P-07/P-08 (DropZone pilots)
      F-06 (DropZoneService) ──► P-07/P-08
      F-07 (EditorConverter) ──► P-06 (Editor import/export pilot)
      F-04 (GridEnums) ──► P-09/P-10 (DataGrid pilots)
      F-05 (CompositeFilter) ──► P-10 (Filter API pilot)
      
Phase 2 (Pilot)
  └── must complete and be reviewed before Phase 3 (Rollout)
      P-03 (SplitterPanes) ──► R-01 (WizardSteps wrapper)
      P-04 (Wizard CascadingValue) ──► R-08 (MultiSelectSettings cascade)
      P-09 (DataGrid SortMode) ──► R-10..R-15 (DataGrid Phase 2 rollout)
      R-10..R-15 ──► R-16..R-19 (DataGrid Phase 3 rollout)
      P-12/P-13 (Chart wrappers) ──► R-20..R-23 (Chart rollout)
      P-06 (Editor import) ──► R-24..R-27 (Editor rollout)

Phase 3 (Rollout)
  └── must complete before Phase 4 (Enforcement)
      R-28 (Form verify) ──► E-06 (Gap plan update)
      R-23 (Chart tests) ──► E-06
      R-27 (Editor table resize) ──► E-05 (Editor tests expansion)
```

### Critical Path

`F-06 (DropZone service) → P-07/P-08 (DropZone pilots) → R-06 (FileUpload CSS)` — this is the only chain where Foundation blocks a non-DataGrid Rollout item.

`R-16 (Frozen columns) → R-17 (Row drag-drop)` — both require DataGrid rendering changes and should be done sequentially to avoid merge conflicts in `MariloDataGrid.Rendering.cs`.

---

## Rollback Strategy Per Phase

### Phase 1 Rollback
All Phase 1 tasks add new files or new types to existing files. If a Foundation task introduces a build break:
1. Revert the specific new file via `git checkout -- <file>`.
2. Downstream phases cannot start until the break is fixed. No data migration involved.

### Phase 2 Rollback
Pilots are single-component changes. If a pilot fails:
1. Revert the component `.razor`/`.razor.cs` files and its test file.
2. Foundation stays in place — Foundation changes are reusable for a retried pilot.
3. If the pilot reveals a Foundation design flaw (e.g., `PopupEventArgs` missing a needed property), fix Foundation before retrying.

### Phase 3 Rollback (per sub-group)
- **DataGrid frozen columns (R-16):** Revert `MariloGridColumn.razor`, `MariloDataGrid.Rendering.cs`, `GridLayoutContract.cs`. The IIFE JS for frozen is inline — revert that JS block. Other DataGrid functionality unaffected.
- **DataGrid row drag-drop (R-17):** Revert JS additions to the IIFE and `RenderDataRow`. Other DataGrid functionality unaffected.
- **Editor adaptive toolbar (R-26):** Remove `Adaptive` parameter and resize observer wiring. Editor works without adaptive mode.
- **Editor table/image resize (R-27):** Remove JS additions from `GetEditorScript()`. Editor base functionality unaffected.
- **Chart rollout (R-20..R-23):** Each chart task is additive. Revert individual component file.

### Phase 4 Rollback
Documentation-only and test-only tasks. Revert individual doc files or test files. No behavior change.

---

## Deferred Items (Out of Scope for This Plan)

The following items were evaluated in Stage 03 but explicitly deferred. They are excluded from all phases above.

| Item | Component | Reason for Deferral |
|------|-----------|---------------------|
| GAP-18 (Virtualization) | MariloTreeView | Requires significant architectural work; no timeline |
| GAP-MSEL-007 (ScrollMode virtual scroll config) | MariloMultiSelect | Blazor `<Virtualize>` lacks required scroll position primitive |
| RES-DG-010 (Typed expand/collapse event args) | MariloDataGrid | Breaking change to `EventCallback<TItem>` signature; deferred to next breaking-change cycle |
| GAP-EDITOR-002 (Adaptive toolbar) | MariloEditor | Partially scoped (R-26), but full ResizeObserver infrastructure is complex |
| GAP-CHART-013 (OnAxisRender) | MariloChart | Requires intercepting inline axis rendering pipeline; deferred after OnRender is confirmed useful |

---

## Summary Checklist

Use this to track overall progress:

- [ ] **Phase 1 complete** — All 8 Foundation tasks done; `dotnet build` green
- [ ] **Phase 2 complete** — All 14 Pilot tasks done; bUnit pilots pass; patterns reviewed
- [ ] **Phase 3 complete** — All 28 Rollout tasks done; full test suite pass; demos updated
- [ ] **Phase 4 complete** — All 6 Enforcement tasks done; docs corrected; gap plan updated
- [ ] **Phase 5 pre-work** — D-01 (DataGrid naming), D-02 (DataSheet theming), D-03 (AddRow), D-04 (SA-05), D-05 (10k-row) decisions recorded by orchestrator
- [ ] **Phase 5 DataGrid spec + demo** — DG-01..DG-07 done; delivery report FU-1/FU-5/FU-6/FU-8 cleared
- [ ] **Phase 5 DataGrid SCSS** — DG-08..DG-10 done; unstyled selectors, dark mode `#fff` literals, focus rings resolved
- [ ] **Phase 5 DataSheet source** — DS-01 (range selection 3.5-day lane), DS-02 (AddRow order post-D-03) done
- [ ] **Phase 5 DataSheet spec + demo** — DS-03..DS-07 done; Wave 2 EU items closed; theming demo deployed (post-D-02)
- [ ] **Phase 5 DataSheet SCSS** — DS-08..DS-10 done (post-D-02); all three `_data-sheet.scss` files exist
- [ ] **Phase 5 gap-analysis bootstrap** — DS-11 done; `datasheet-gap-analysis` coverage-summary no longer a stub
- [ ] **Final gate** — `dotnet build Marilo.slnx` exits 0; `dotnet test` 0 failures; gap closure reports exist for all newly implemented gaps; DataGrid and DataSheet delivery gates reach AMBER or CLEAR

---

*Plan generated from 41 Stage 03 resolution files; updated rev 2 to incorporate DataGrid CDW (12 blockers, FU-1..FU-12) and DataSheet CDW (6 remaining blockers, post-Wave-5). All resolution IDs (RES-*) are traceable to `stages/03-resolution-design/output/`. Delivery-gate findings traceable to `ICM/workspaces/datagrid-delivery/stages/04-sync-check/output/datagrid-delivery-report.md` and `ICM/workspaces/datasheet-delivery/stages/04-sync-check/output/datasheet-delivery-report.md`. Closure evidence expected in `stages/06-validate/output/gap-*-closure-report.md`.*
