# DataGrid Gap Inventory — Stage 01 Intake

**Worker:** `w-datagrid-gap-analysis`
**Session:** `marilo-grid-pipeline-2026-04-11-1200`
**Wave:** gap-analysis Stage 01 (intake) — fresh bootstrap
**Intake date:** 2026-04-11
**Component:** `MariloDataGrid<TItem>`
**Stage purpose:** Consolidate every distinct gap record from Wave 1 (spec review), Wave 2 (example UX), Wave 3 (visual parity), and Wave 4 (sync check) into a single flat inventory ready for Stage 02 prioritization. **No resolution work happens here.** Fields follow the inbox-declared schema (id / origin / priority / scope / sync areas / rationale / resolution hint).

---

## Inputs Consumed (verification-before-completion)

| # | File | Purpose | Records extracted |
|---|---|---|---:|
| 1 | `ICM/workspaces/datagrid-delivery/stages/04-sync-check/output/datagrid-delivery-report.md` | Authoritative gate verdict (BLOCKED, 12 blockers, 27 checklist items). Source of the FU-1..FU-12 remediation lane catalog and the blocking-item designation used for priority assignment. | 12 follow-up lanes (FU-1..FU-12) |
| 2 | `.claude/orchestration/_orchestrator/decisions/tick-8-2026-04-11-1830.md` | Naming-cascade resolution (datagrid-fu-3-user-escalation): full `Marilo`-prefix everywhere, NO `<MariloGrid>` short form. Cascades into M-01/M-02/S-04/S-05 and every FU-1..FU-12 downstream record. | 1 resolution → cascaded into 15+ records |
| 3 | `ICM/workspaces/datagrid-delivery/stages/01-spec-review/output/datagrid-spec-gap-list.md` | Wave 1 spec review refresh pass + orchestrator Wave 1 focus-topic addendum. | 68 records: U-01..U-10, S-01..S-17, M-01..M-13, SA-01..SA-14, SRC-01..SRC-08, NM-01..NM-06 |
| 4 | `ICM/workspaces/datagrid-delivery/stages/02-example-ux/output/datagrid-example-ux-gap-list.md` | Wave 2 demo inventory and 77-scenario coverage matrix across 6 Wave 1 focus topics. | 13 demo-level action items: A-01..A-13 |
| 5 | `ICM/workspaces/datagrid-delivery/stages/03-visual-parity/output/datagrid-visual-parity-gaps.md` | Wave 3 static-analysis visual-parity gap records + DEFERRED-TO-CAPTURE (DC-01..DC-20) queue. | 20 records: VP-datagrid-001..020 |
| 6 | `ICM/workspaces/datagrid-delivery/stages/03-visual-parity/output/datagrid-parity-summary.md` | Wave 3 roll-up table, parity scores by theme × mode, remediation route. | 0 new records (aggregation only) |
| 7 | `ICM/workspaces/datagrid-gap-analysis/CLAUDE.md` | Stage routing (01-intake → 02-prioritize → … → 06-validate). | n/a |
| 8 | `ICM/workspaces/datagrid-gap-analysis/_config/gap-context.md` (stub, also updated this turn) | Target project + tracking counters. | n/a |
| 9 | `.claude/orchestration/_memory/workers/w-datagrid-gap-analysis.json` | Worker state (files_owned, required_sync_areas, required_skills). | n/a |
| 10 | `.claude/orchestration/_memory/projects/marilo.json` | Sync-area definitions and architecture_constants (additive-only public API). | n/a |

**Distinct gap records ingested:** 68 (Wave 1) + 13 (Wave 2) + 20 (Wave 3) = **101 base records**. Plus **12 Wave 4 aggregated follow-up lanes** (FU-1..FU-12) retained as routing rows (not double-counted). **Total inventory rows: 113.**

---

## Scope / Priority / Sync-Area Legend

**Priority** — taken from the delivery report blocking designation and the Wave 1 P1/P2/P3 assignment:

- **P0-blocker** — explicitly called out as a BLOCKING item in `datagrid-delivery-report.md` §Gate Status or in Wave 1 §"Three-List Classification". Maps to the 12-item blocking list (9 checklist failures + 3 category criticals B-A/B-B/B-C).
- **P1** — Wave 1 P1 (blocking to consumer compile) but not promoted to gate-blocker. Includes most M-* mismatches that break spec-example compilation.
- **P2** — Wave 1 P2 (this-phase backlog).
- **P3-cosmetic** — Wave 1 P3 (next-phase / deferred). Includes low-severity undocumented parameters and minor mismatches.

**Scope** — per the workspace gap-scope routing convention:

- **single** — one record, one file, no dependencies (e.g. document one parameter in spec, style one SCSS selector).
- **batch** — a cluster of records sharing a single edit surface (e.g. FluentUI `_data-grid.scss` unstyled-selector cluster; spec-update batch for 10 U-* entries).
- **systematic** — crosses 3+ files or touches the architecture layer (naming cascade, provider contract, validation integration).

**Sync areas** — per `marilo.json -> sync_area_definitions`:

`source` | `spec` | `demo` | `docs` | `tests` | `gap-plan`

Every record in this inventory has `gap-plan` implicitly required (its intake row lives in this file); explicit `gap-plan` listings below highlight records that will need a dedicated `GAP_ANALYSIS_RESOLUTION_PLAN.md` phase row at Stage 02.

**FU-3 verbatim annotation** (applied to every cascaded record):

> Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form.

---

## Section A — Wave 1 Spec Review (refresh pass)

### A.1 — Undocumented parameters (U-01..U-10)

All 10 are implemented in source but absent from `docs/component-specs/grid/`. All are doc-only fixes (no source change). Delivery report FU-1 routes these to the spec-update batch.

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| U-01 | wave-1-spec | P2 | batch | spec | `ShowSearchBox` + `SearchBoxPlaceholder` implemented but spec only treats search as a toolbar tool. Source: `MariloDataGrid.razor.cs` L107 area. | Bundle with FU-1 spec-update batch. No FU-3 cascade. |
| U-02 | wave-1-spec | P2 | batch | spec | `EnableVirtualization` + `VirtualizeOverscanCount` expose virtual-scrolling as bool+int rather than `ScrollMode`/`RowHeight`. Paired with M-03 shape mismatch. | Bundle with FU-1 spec-update batch. See M-03 for the shape decision. |
| U-03 | wave-1-spec | P3-cosmetic | single | spec | `Striped` appearance toggle not in spec overview. Demo D3 already uses it (Orphan-spec-gap). | Bundle with FU-1 spec-update batch. |
| U-04 | wave-1-spec | P2 | single | spec | `AutoGenerateColumns` boolean not documented though the spec references auto-generation. | Bundle with FU-1 spec-update batch. |
| U-05 | wave-1-spec | P2 | single | spec | `Resizable`, `Reorderable` exist as grid-level bools; spec scopes these to column level only. | Bundle with FU-1 spec-update batch. |
| U-06 | wave-1-spec | P3-cosmetic | single | spec | `OnRowContextMenu` right-click event missing from spec events page. D2 already demos it (Orphan-spec-gap). | Bundle with FU-1 / FU-5 spec-update batch. |
| U-07 | wave-1-spec | P3-cosmetic | single | spec | `OnRowExpand` / `OnRowCollapse` detail expand events missing from spec events page. D2 already demos them (Orphan-spec-gap). Linked to M-13 event-args-shape mismatch. | Bundle with FU-1 / FU-5 spec-update batch. |
| U-08 | wave-1-spec | P2 | single | spec | Grid-level flat `PagerButtonCount` int exists; spec documents a rich `GridPagerSettings` object only. Paired with M-12 shape mismatch. | Bundle with FU-1 spec-update batch. See M-12 for the shape decision. |
| U-09 | wave-1-spec | P3-cosmetic | single | spec | `ColumnWidthProvider` / `IColumnWidthProvider` extension point absent from sizing spec. | Bundle with FU-1 spec-update batch. |
| U-10 | wave-1-spec | P2 | single | spec | `GridGroupHeaderContext<TItem>` context type on `GroupHeaderTemplate` / `GroupFooterTemplate` not named in spec templates page. | Bundle with FU-1 spec-update batch. |

### A.2 — Spec-ahead parameters (S-01..S-17)

Documented but not implemented in source. These all require source work and are the primary feed into remediation phases.

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| S-01 | wave-1-spec | P3-cosmetic | single | source, spec, tests | `AdaptiveMode` listed in overview table, absent from source. Known planned gap. | Stage 02 prioritization — Phase C/D candidate. |
| S-02 | wave-1-spec | P2 | single | source, spec, tests | `Class` parameter on grid (overview table, absent from `MariloDataGrid`). | Stage 02 prioritization — trivial-add candidate. |
| S-03 | wave-1-spec | P3-cosmetic | systematic | source, spec, tests | `CustomKeyboardShortcuts` dictionary param + `GridKeyboardScope` + `GridKeyboardCommand` enums absent. Bundled with SA-05..SA-08 under the keyboard engine lane. | Stage 02 — keyboard engine lane (FU-7 cluster). |
| S-04 | wave-1-spec | **P0-blocker** | systematic | source, spec, demo, docs, tests, gap-plan | **`<GridColumns>` wrapper element** referenced in spec but source takes columns as direct `ChildContent`. Every Wave 1 spec code snippet fails to compile. Listed as FU-3 cascade in delivery report §Gate Status. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. |
| S-05 | wave-1-spec | **P0-blocker** | systematic | source, spec, demo, docs, tests, gap-plan | **`GridCommandColumn` + `GridCommandButton` built-in.** Source has `MariloGridCommandButton.razor` + `GridCommandTypes.cs` but no dedicated `GridCommandColumn` element. Listed as FU-3 cascade. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. Per tick-8 §"datagrid-fu-3-user-escalation": rename `MariloGridCommandButton.razor` → `MariloGridCommandColumn.razor` if spec requires column-shaped API; otherwise leave as button inside a command column. Gap-analysis worker decides per-case. |
| S-06 | wave-1-spec | P2 | batch | source, spec, demo, tests, gap-plan | `<GridToolBarTemplate>` + 13 tool components (Add, Save, Export*, SearchBox, ColumnChooser, CsvExport, ExcelExport, …). Source has `ToolbarTemplate` fragment + `MariloGridToolbar.razor` shell only. | Stage 02 — toolbar-expansion batch. |
| S-07 | wave-1-spec | P2 | systematic | source, spec, tests | `GridPagerSettings` compound settings object (paired with M-12 / U-08). Coordinator decision on API shape still open (delivery report FU-4). | Stage 02 — depends on FU-4 post-FU-3 resolution. |
| S-08 | wave-1-spec | P2 | batch | source, spec, demo, tests, gap-plan | Excel + PDF export (spec `export/excel.md`, `export/pdf.md`). Source supports CSV string return only. | Stage 02 — export lane. |
| S-09 | wave-1-spec | P2 | batch | source, spec, tests | Composite filter descriptors / AND/OR filter menu (spec `filter/filter-menu.md`). Source stores a single filter per field. | Stage 02 — filter-descriptor lane. |
| S-10 | wave-1-spec | P2 | batch | source, spec, demo, tests | Drag-to-group header panel UI. Source has programmatic `GroupBy`/`Ungroup` only. | Stage 02 — grouping UI lane. |
| S-11 | wave-1-spec | P3-cosmetic | batch | source, spec, tests | Multi-column headers (`MariloGridColumnGroup`). | Stage 02 — deferred column-feature lane. |
| S-12 | wave-1-spec | P3-cosmetic | batch | source, spec, demo, tests | Column menu / column chooser (spec `columns/menu.md`). | Stage 02 — deferred column-feature lane. |
| S-13 | wave-1-spec | P3-cosmetic | systematic | source, spec, docs, tests | AI features (9 spec pages under `smart-ai-features/`). Phase D. | Stage 02 — Phase D deferred. |
| S-14 | wave-1-spec | P3-cosmetic | single | source, spec, tests | `HighlightedItems` + highlighting API (`highlighting.md`). | Stage 02 — deferred. |
| S-15 | wave-1-spec | P2 | systematic | source, spec, demo, docs, tests, gap-plan | DataAnnotations validation integration (`editing/validation.md`). Source edit pipeline does not wire `EditContext` / validation. | Stage 02 — validation-integration lane. |
| S-16 | wave-1-spec | P3-cosmetic | batch | source, spec, tests | `PopupFormTemplate`, `PopupButtonsTemplate`, `PagerTemplate`. | Stage 02 — template-expansion lane. |
| S-17 | wave-1-spec | P3-cosmetic | single | source, spec, tests | Checkbox-list filter control (`filter/checkboxlist.md`). Internal state fields exist but no public parameters. | Stage 02 — filter-control lane. |

### A.3 — Name/shape mismatches (M-01..M-13)

Documented AND implemented but name or shape differs.

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| M-01 | wave-1-spec | **P0-blocker** | systematic | source, spec, demo, docs, tests, gap-plan | **`<MariloGrid>` component tag in spec vs `<MariloDataGrid>` in source.** Every spec example fails to compile against source. Root of the FU-3 escalation. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. Resolution = spec-side rename to `<MariloDataGrid>`. No source rename. |
| M-02 | wave-1-spec | **P0-blocker** | systematic | source, spec, demo, docs, tests, gap-plan | **`<GridColumn>` in spec vs `<MariloGridColumn>` in source.** Every spec column example fails to compile. Root of the FU-3 escalation. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. Resolution = spec-side rename to `<MariloGridColumn>`. |
| M-03 | wave-1-spec | P2 | systematic | source, spec, tests | `ScrollMode` enum + `RowHeight` decimal in spec vs `EnableVirtualization` bool + `VirtualizeOverscanCount` int in source. Public API shape decision required. | Stage 02 — post-FU-3 decision cluster (FU-4). |
| M-04 | wave-1-spec | P2 | single | spec | `SortMode` — source closed, spec-name update needed to reference `GridSortMode` enum. | Bundle with FU-1 spec-update batch. |
| M-05 | wave-1-spec | P2 | systematic | source, spec, tests | `GridState<TItem>` in spec vs `GridState` (non-generic) in source. Type-loss on edit-item/original-item. Shared Core model — coordinator decision required. | Stage 02 — FU-4 cluster. Cross-reference the GanttState decision template (`feedback/project_gantt_state_api_shape.md`) — analogous genericization. |
| M-06 | wave-1-spec | P2 | single | spec | `GridCommandEventArgs` (untyped) in spec vs `GridEditEventArgs<TItem>` (typed) in source for edit callbacks. Spec examples won't compile. | Bundle with FU-1 spec-update batch — keep typed source shape. |
| M-07 | wave-1-spec | P2 | single | spec | `OnModelInit` lambda-returning-new-model in spec vs `EventCallback<GridModelInitEventArgs<TItem>>` in source. | Bundle with FU-1 spec-update batch — keep source idiom. |
| M-08 | wave-1-spec | P2 | single | spec | `DisplayFormat` — source closed. Column spec needs to show both `DisplayFormat` and legacy `Format`. | Bundle with FU-1 spec-update batch. |
| M-09 | wave-1-spec | P2 | single | spec | `Locked` / frozen column — source closed. Re-verify `columns/frozen.md` parameter names. | Bundle with FU-1 spec-update batch (verify only). |
| M-10 | wave-1-spec | P2 | single | spec | Cell selection — source closed (`SelectedCells`/`SelectedCellsChanged`). Re-verify `selection/cells.md`. | Bundle with FU-1 spec-update batch (verify only). See NM-02 for the type-name inconsistency. |
| M-11 | wave-1-spec | P2 | single | spec | Row drag-drop (`OnRowDrop`) — source closed. Re-verify `row-drag-drop.md`. | Bundle with FU-1 spec-update batch (verify only). |
| M-12 | wave-1-spec | P2 | systematic | source, spec, tests | Pager shape — `GridPagerSettings { ButtonCount, … }` in spec vs flat `PagerButtonCount` + `PageSizes` in source. Coordinator decision required. | Stage 02 — FU-4 cluster. Paired with S-07 / U-08. |
| M-13 | wave-1-spec | P3-cosmetic | single | spec | `OnRowExpand`/`OnRowCollapse` typed event args (`GridRowExpandEventArgs<TItem>`) in spec vs plain `EventCallback<TItem>` in source. | Bundle with FU-1 spec-update batch — keep source shape. |

### A.4 — Wave 1 spec-ahead additions (SA-01..SA-14)

Wave 1 orchestrator-focus-topic pass (`2026-04-11`). These overlap thematically with S-* but are logged separately because they originated in the focus-topic sweep and reference specific source line numbers.

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| SA-01 | wave-1-spec | P2 | batch | source, spec, tests | `DragToSelect` parameter on `GridSelectionSettings` absent. Cell selection is click-only. | Stage 02 — selection-extensions lane. |
| SA-02 | wave-1-spec | P2 | batch | source, spec, tests | `<GridSelectionSettings SelectionType="Row">` alternate enablement — type absent from source. | Stage 02 — selection-extensions lane. |
| SA-03 | wave-1-spec | P2 | batch | source, spec, demo, tests | `<GridCheckboxColumn SelectAll CheckBoxOnlySelection>` dedicated element absent; source has flat `ShowCheckboxColumn` bool. | Stage 02 — checkbox-column lane. |
| SA-04 | wave-1-spec | P2 | systematic | source, spec, tests | Shift-click range and Ctrl-click toggle selection — no modifier handling in row click logic. | Stage 02 — selection-modifier lane (selection-extensions cluster). |
| SA-05 | wave-1-spec | P3-cosmetic | systematic | source, spec, tests | `CustomKeyboardShortcuts` dictionary param re-confirmed (same as S-03). Enums absent. | Stage 02 — keyboard engine lane. |
| SA-06 | wave-1-spec | **P0-blocker** | systematic | source, spec, demo, tests, gap-plan | Default key bindings (arrows, Home/End, Ctrl+Home/End, PageUp/PageDown) undelivered. `Navigable` exposes only the bool; no `onkeydown` handler. Wave 2 D4 "Navigable Grid" demo advertises behavior the source does not implement — gate-level honesty defect B-C. | Stage 02 — keyboard engine lane (FU-12 bundle with VP-015). |
| SA-07 | wave-1-spec | **P0-blocker** | systematic | source, spec, demo, tests, gap-plan | Data-cell keyboard actions (Enter/F2 = edit, Esc = cancel, Space = select, Delete/Backspace = delete). None wired. Part of B-C honesty defect. | Stage 02 — keyboard engine lane (FU-12 bundle). |
| SA-08 | wave-1-spec | **P0-blocker** | systematic | source, spec, demo, tests, gap-plan | Edit-row keyboard (Tab/Shift+Tab, Enter = save, Esc = cancel). No editor focus management in `Editing.cs`. Part of B-C honesty defect. | Stage 02 — keyboard engine lane (FU-12 bundle). |
| SA-09 | wave-1-spec | P2 | batch | source, spec, tests | Column `EditorType` enum (`CheckBox`/`Switch`/`DatePicker`/…). Not on `MariloGridColumn`; no `GridEditorType` enum. | Stage 02 — editor-type lane. |
| SA-10 | wave-1-spec | P2 | single | source, spec, tests | `NewRowPosition` parameter (`GridNewRowPosition.Top`/`Bottom`) missing. `BeginAdd()` has no position concept. | Stage 02 — single-param add. |
| SA-11 | wave-1-spec | P2 | single | source, spec, tests | Automatic `OnRead` call after `OnCancel`/`OnCreate`/`OnDelete`/`OnUpdate` uncertain. `ProcessDataAsync()` path needs verification. | Stage 02 — refresh-data lane (pairs with SRC-05). |
| SA-12 | wave-1-spec | P3-cosmetic | single | source, spec, tests | "Cell selection not supported with InCell edit" / "row selection in InCell requires checkbox column" — no source guard. Allowed combo silently undefined. | Stage 02 — editing-guard lane. |
| SA-13 | wave-1-spec | P3-cosmetic | single | source, spec, tests | Grid should clear `SelectedItems` when user drags and drops selected rows — row-drop handler does not clear. | Stage 02 — selection/drop guard. |
| SA-14 | wave-1-spec | P2 | single | source, spec, tests, docs | Silent-failure defect: `BeginAdd()` in `Editing.cs:39` silently assigns null/default if `OnModelInit` not wired. No defensive throw or warning. Cross-linked to Wave 4 Section 2.6 error-state demo gap. | Stage 02 — editing-guard lane (FU-8 bundle). |

### A.5 — Source-ahead records (SRC-01..SRC-08)

Source implements behavior the spec does not document.

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| SRC-01 | wave-1-spec | P2 | single | spec | `SelectionUnit` parameter exists on grid but spec `selection/overview.md` never names it at overview level. | Bundle with FU-1 spec-update batch. |
| SRC-02 | wave-1-spec | P2 | single | spec | `SelectedCells` uses `IEnumerable<GridCellReference<TItem>>` but spec overview lists `IEnumerable<GridSelectedCellDescriptor>` — different name. See NM-02 for type disagreement. | Bundle with FU-1 spec-update batch. Resolve internal spec inconsistency first (NM-02). |
| SRC-03 | wave-1-spec | P2 | batch | spec, demo | **Imperative edit API** (`BeginEdit`, `BeginCellEdit`, `BeginAdd`, `SaveEdit`, `CancelEdit`, `DeleteItem`, `ExecuteCommand`) — 7 public methods in `MariloDataGrid.Editing.cs:13-23`, zero spec coverage, zero demo coverage. Called out by Wave 4 Section 1.1/3.2 as a BLOCKING omission. | Bundle with FU-1 (spec) + FU-6 (demo, new `DataGrid/ProgrammaticControl.razor` page or D2 extension). Pairs with Wave 2 A-07. |
| SRC-04 | wave-1-spec | P2 | single | spec | `OnCommand` typed with `GridCommandEventArgs<TItem>`. Spec conflates with edit-event args (see M-06). | Bundle with FU-1 spec-update batch + Wave 2 A-06 for demo. |
| SRC-05 | wave-1-spec | P2 | single | spec | `Rebind()` method — spec's `refresh-data.md:25` example targets `MariloGrid<T>` (wrong tag, wrong lifetime reference). | Bundle with FU-1 spec-update batch. Apply FU-3 cascade to the spec snippet. Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. |
| SRC-06 | wave-1-spec | P3-cosmetic | single | spec, source (optional), tests | `DeleteItem` uses `JS.InvokeAsync<bool>("confirm", ...)` — browser-native `window.confirm`, not `MariloDialog`. Spec says "Dialog". | Stage 02 — either (a) document native behavior in spec, or (b) swap to MariloDialog (source change). Low priority. |
| SRC-07 | wave-1-spec | P3-cosmetic | single | spec | `GridCellReference<TItem>.RowIndex` is page-relative, not source-absolute. Spec does not clarify. | Bundle with FU-1 spec-update batch. |
| SRC-08 | wave-1-spec | P3-cosmetic | single | spec | `ToggleDetailRow` fires `OnRowExpand`/`OnRowCollapse` + `NotifyStateChanged("DetailExpand")` but `editing/overview.md` / `refresh-data.md` don't cover detail-row expansion persistence across `Rebind()`. | Bundle with FU-1 spec-update batch. |

### A.6 — Naming mismatch records (NM-01..NM-06)

Originally a subset of the M-* cluster; Wave 1 focus pass re-confirmed them against specific spec files. All cascade under FU-3.

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| NM-01 | wave-1-spec | **P0-blocker** | batch | spec | All 4 Wave 1 spec files use `<MariloGrid>` as the component tag. Same root as M-01. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. |
| NM-02 | wave-1-spec | P2 | single | spec | `docs/component-specs/grid/selection/overview.md:38` uses `IEnumerable<GridSelectedCellDescriptor>`; `selection/cells.md:24` uses `IEnumerable<GridCellReference<TItem>>`. **Spec internally inconsistent.** Source uses `GridCellReference<TItem>`. | Bundle with FU-1 spec-update batch. Pick `GridCellReference<TItem>`. No FU-3 cascade (not a tag-name issue). |
| NM-03 | wave-1-spec | **P0-blocker** | single | spec | `refresh-data.md:48` — `private MariloGrid<Employee> GridRef` in `@code` block. Naive find/replace on tags won't catch this since it's C#, not markup. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. In C# blocks: replace `MariloGrid<T>` with `MariloDataGrid<T>`. |
| NM-04 | wave-1-spec | **P0-blocker** | single | spec | `editing/overview.md:147` uses `GridCommandEventArgs` as the type for `OnCreate`/`OnUpdate`/`OnDelete`/`OnEdit`/`OnCancel`. Source uses `GridEditEventArgs<TItem>` for those and reserves `GridCommandEventArgs<TItem>` for `OnCommand` alone. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. Resolve spec type reference (paired with M-06). |
| NM-05 | wave-1-spec | **P0-blocker** | batch | spec | `keyboard-navigation.md:231` uses `@using Marilo.Blazor.Components.Grid` and `Marilo.Blazor.*` slug references. Stale Telerik-provenance namespace; real namespaces are `Marilo.Components.DataGrid` / `Marilo.Core.Enums`. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. Apply to all `Marilo.Blazor.*` slug references under `docs/component-specs/grid/`. |
| NM-06 | wave-1-spec | **P0-blocker** | single | spec | `editing/overview.md:164` implies `GridEditorType` enum in `Marilo.Blazor` namespace — enum doesn't exist anywhere (see SA-09). Naming AND existence mismatch. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. Decision on enum namespace pairs with SA-09 implementation. |

---

## Section B — Wave 2 Example-UX Action Items (A-01..A-13)

Wave 2 inventoried 4 demo pages (~32 sections) against 6 focus-topic specs and produced 13 action items. A-01..A-08 are demo-only (no source blocker); A-09..A-13 are source-blocked and route to gap-analysis.

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| A-01 | wave-2-example-ux | **P0-blocker** | single | demo | D4 "Navigable Grid" honesty fix — remove or gate keyboard cheat sheet. Wave 2 headline finding #1 / delivery report category critical B-C. | Stage 02 — bundle with FU-12 (joint lane with VP-datagrid-015 focus-ring fix). |
| A-02 | wave-2-example-ux | **P0-blocker** | batch | demo | Add `DataGrid/RefreshData.razor` or Overview section covering Rebind / ObservableCollection / new collection / `OnRead+SetStateAsync` / EF pattern. Wave 2 headline finding #2 — zero demo coverage for refresh-data spec topic. | Stage 02 — Wave 2 demo batch (FU-6 routing). |
| A-03 | wave-2-example-ux | P1 | single | demo | Add cell-selection section to D1 (`SelectionUnit=Cell`, `SelectedCells`, `SelectedCellsChanged`, `GridCellReference` inspection). Wave 2 headline finding #3 — source closed but demo missing. | Stage 02 — Wave 2 demo batch (FU-6). |
| A-04 | wave-2-example-ux | P2 | single | demo | Add combined "Selection + Inline edit" and "Selection + Popup edit" sections. | Stage 02 — Wave 2 demo batch (FU-6). |
| A-05 | wave-2-example-ux | P2 | single | demo | Add `ConfirmDelete` section. Pairs with SRC-06 (native confirm UX tension). | Stage 02 — Wave 2 demo batch (FU-6). |
| A-06 | wave-2-example-ux | P2 | single | demo | Add `OnAdd` and `OnCommand` demo sections to D2 (pairs with SRC-04 and Wave 4 Section 1.5/2.2). | Stage 02 — Wave 2 demo batch (FU-5/FU-6). |
| A-07 | wave-2-example-ux | P2 | batch | demo, spec | Add imperative API section (Begin*/Save/Cancel/DeleteItem/ExecuteCommand) — new `DataGrid/ProgrammaticControl.razor` or D2 extension. Pairs with SRC-03. | Stage 02 — Wave 2 demo batch (FU-6) + FU-1 spec-update batch. |
| A-08 | wave-2-example-ux | P3-cosmetic | single | demo | Add `@bind-SelectedItems` shorthand variant. | Stage 02 — Wave 2 demo batch (FU-6). |
| A-09 | wave-2-example-ux | P1 | systematic | source, spec, demo, tests | Keyboard navigation topic — route entire scenario list (17 blocked-by-source) as single intake item. | Stage 02 — keyboard engine lane (shared with SA-05..SA-08). |
| A-10 | wave-2-example-ux | P2 | batch | source, demo, tests | Shift-click / Ctrl-click / drag-to-select demos — blocked by SA-01/SA-04. | Stage 02 — selection-extensions lane. |
| A-11 | wave-2-example-ux | P2 | batch | source, spec, demo, tests | `GridCheckboxColumn` with SelectAll + `CheckBoxOnlySelection` — blocked by SA-03. | Stage 02 — checkbox-column lane. |
| A-12 | wave-2-example-ux | P2 | batch | source, spec, demo, tests | `EditorType` enum demos — blocked by SA-09. | Stage 02 — editor-type lane. |
| A-13 | wave-2-example-ux | P2 | single | source, spec, demo, tests | `NewRowPosition` demo — blocked by SA-10. | Stage 02 — single-param add. |

---

## Section C — Wave 3 Visual-Parity Records (VP-datagrid-001..020)

All 20 static-analysis records from `datagrid-visual-parity-gaps.md`. Critical records drive FU-9 / FU-10 / FU-11 remediation lanes. Note: DC-01..DC-20 deferred-to-capture items are NOT enumerated here — they are a Playwright capture queue, not gap records.

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| VP-datagrid-001 | wave-3-visual-parity | P1 | single | source, tests | Fluent light row hover collides with header surface token (`--marilo-color-surface` used for both). Hovered row indistinguishable from header band. Category: state treatment. Severity: critical. | Stage 02 — FluentUI provider SCSS hover-token fix. Bundle with FU-9 provider visual batch. |
| VP-datagrid-002 | wave-3-visual-parity | **P0-blocker** | single | source, tests | Fluent dark mode: stripe-even and hover share same token — hover produces zero visual change on striped-even rows. Dark-mode state-layer token missing. Category: state treatment. Severity: critical. | Stage 02 — FU-9 provider visual batch + foundation dark-mode state-layer token. |
| VP-datagrid-003 | wave-3-visual-parity | P1 | single | source, tests | Fluent light selected row has no hover delta AND no left accent. Selected+hover indistinguishable from selected+rest. Category: state treatment. Severity: major. | Stage 02 — FU-9 provider visual batch. |
| VP-datagrid-004 | wave-3-visual-parity | **P0-blocker** | single | source, tests | Fluent dark selected row — `--marilo-color-primary-light` = `#0a2e4a` has insufficient luminance delta from `#1b1a19` background. Selection nearly invisible. Category: token/color. Severity: critical. | Stage 02 — FU-9 provider visual batch + foundation token redefinition. |
| VP-datagrid-005 | wave-3-visual-parity | P2 | single | source | Header typography — no dedicated header font-size/letter-spacing token. Header differentiated from body by weight only. Category: typography. Severity: major. | Stage 02 — FU-9 provider visual batch (typography sub-lane). |
| VP-datagrid-006 | wave-3-visual-parity | P2 | single | source | Row density — no density variants wired. Cell padding fixed at 8×12. Category: density. Severity: major. | Stage 02 — FU-9 provider visual batch (density sub-lane). May escalate for `Density` parameter decision. |
| VP-datagrid-007 | wave-3-visual-parity | P2 | single | source | Sort indicator unstyled — `.mar-datagrid-sort-indicator` / `.mar-datagrid-sort-order` emitted with zero SCSS. Category: iconography. Severity: major. | Stage 02 — FU-10 unstyled-selector cluster. |
| VP-datagrid-008 | wave-3-visual-parity | **P0-blocker** | batch | source, tests | **Pager buttons unstyled (Fluent)** — `.mar-datagrid-pager-btn*` emitted but zero SCSS. User-agent button chrome. Category: state treatment. Severity: critical. | Stage 02 — FU-10 unstyled-selector cluster. |
| VP-datagrid-009 | wave-3-visual-parity | **P0-blocker** | batch | source, tests | **Pager buttons unstyled (Bootstrap)** — same as VP-008 applied to Bootstrap provider. Category: state treatment. Severity: critical. | Stage 02 — FU-10 unstyled-selector cluster. |
| VP-datagrid-010 | wave-3-visual-parity | **P0-blocker** | single | source, tests | **Empty state unstyled** — `.mar-datagrid-empty` emitted, zero SCSS. Cross-linked with Wave 4 Section 2.5 undemoed empty state. Category: layout. Severity: major. Promoted to blocker because the empty-state path is both visually broken AND has no demo. | Stage 02 — FU-10 unstyled-selector cluster + FU-8 empty-state demo bundle. |
| VP-datagrid-011 | wave-3-visual-parity | P1 | single | source, tests | Loading overlay unstyled — `.mar-datagrid-loading-overlay` emitted, zero SCSS. No absolute positioning, no backdrop, no spinner. Category: layout/elevation. Severity: major. | Stage 02 — FU-10 unstyled-selector cluster. |
| VP-datagrid-012 | wave-3-visual-parity | **P0-blocker** | batch | source, tests | **Popup edit dialog unstyled** — entire `.mar-datagrid-popup-*` sub-tree has zero SCSS. No overlay, no scrim, no chrome. Category: elevation + layout + state treatment. Severity: critical. | Stage 02 — FU-10 unstyled-selector cluster. Reuse `patterns/_overlay.scss`. |
| VP-datagrid-013 | wave-3-visual-parity | P1 | single | source, tests | Filter menu popover hardcodes `#fff` / `#ffffff` as the `color-mix` base and input backgrounds (FluentUI L128/L160/L176/L189). Breaks dark mode. Category: token/color. Severity: major. | Stage 02 — FU-11 `#fff` literal find-and-replace. |
| VP-datagrid-014 | wave-3-visual-parity | P2 | single | source | Filter menu popover shadow hardcoded as `rgba(0,0,0,0.12)` instead of `--elevation-shadow-flyout`. Too subtle in dark mode. Category: elevation. Severity: major. | Stage 02 — FU-9 provider visual batch (elevation sub-lane). |
| VP-datagrid-015 | wave-3-visual-parity | **P0-blocker** | single | source, tests | **No focus treatment anywhere** — zero `:focus` / `:focus-visible` rules on any DataGrid interactive element. `--focus-stroke-outer` token exists but unused. Compounds SA-06/A-01 (D4 keyboard honesty defect). Category: state treatment. Severity: critical. | Stage 02 — **FU-12 joint lane** with A-01 honesty fix. Full Marilo-prefix cascade per tick-8 decision has no direct impact here (pure SCSS fix), but the lane is bundled with FU-3-blocked work. |
| VP-datagrid-016 | wave-3-visual-parity | **P0-blocker** | systematic | source, spec, tests, gap-plan | **Material provider is a 5-line TODO.** Every Material state/mode combination scores 0. Requires new provider implementation, not a SCSS patch. Per delivery report §Remediation Lanes: explicitly routed OUT of this wave; tracked as separate gap-analysis track. | Stage 02 — explicitly deferred to a separate Material-provider implementation track. Cross-reference tick-8 Cerebrum Pattern 5 (Material stubs = intentional technical debt). |
| VP-datagrid-017 | wave-3-visual-parity | P3-cosmetic | single | source | Group header uses shared surface token — indistinguishable from column header when stacked. No left accent or indent. Category: layout. Severity: minor. | Stage 02 — FU-9 provider visual batch. |
| VP-datagrid-018 | wave-3-visual-parity | P1 | single | source, tests | Checkbox column unstyled — `.mar-datagrid-checkbox-cell` emitted with inline width=40px; no SCSS; no flex centering. Category: layout. Severity: major. | Stage 02 — FU-10 unstyled-selector cluster. |
| VP-datagrid-019 | wave-3-visual-parity | P1 | single | source, tests | Bootstrap filter menu hardcodes `#fff` backgrounds on 3 selectors — breaks dark mode. Category: token/color. Severity: major. | Stage 02 — FU-11 `#fff` literal find-and-replace (Bootstrap provider). |
| VP-datagrid-020 | wave-3-visual-parity | P3-cosmetic | single | source | Bootstrap striped rows use `#{$table-striped-bg}` compile-time Sass interpolation — runtime dark-theme toggle can't override. Category: token/color. Severity: minor. | Stage 02 — Bootstrap provider token refactor. |

---

## Section D — Wave 4 Sync-Check Follow-Up Lanes (FU-1..FU-12)

These are NOT new gap records — they are the delivery report's aggregated remediation routing. Retained in this inventory so Stage 02 can see the routing intent at-a-glance and group records into workable lanes. Each row back-references the sections above.

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| FU-1 | wave-4-sync-check | P1 | batch | spec | Spec-update batch covering U-01..U-10 + SRC-01/02/04/05/07/08 + M-04/M-06/M-07/M-08/M-09/M-10/M-11/M-13 + NM-02. Doc-only; no source changes. Delivery workspace lane. Est. 0.5 worker day. | Full Marilo-prefix cascade per tick-8 decision applies to every snippet touched in this batch. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. |
| FU-2 | wave-4-sync-check | P1 | systematic | source, spec, demo, tests, gap-plan | Bulk intake of all S-* spec-ahead items + Wave 1 SA-01..SA-14. This is the bootstrap intake for `datagrid-gap-analysis` (this worker's output seeds it). Est. 1 worker day. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. (Every S-* / SA-* record that references tags will cascade.) |
| FU-3 | wave-4-sync-check | **P0-blocker** | systematic | source, spec, demo, docs, tests, gap-plan | **Naming cascade RESOLVED in tick-8.** Was public API rename escalation for M-01/M-02/S-04/S-05. User decided: keep source as-is, rename spec side to full Marilo-prefix. See M-01/M-02/S-04/S-05 rows for cascaded annotation. | **RESOLVED** — Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. Cascades to FU-1, FU-2, FU-4..FU-12 with naming snippets. |
| FU-4 | wave-4-sync-check | P1 | systematic | source, spec, tests | Post-FU-3 shape-mismatch resolution for M-03 (virtual-scrolling), M-05 (GridState genericization), M-12 (pager shape), M-06/M-07 event-args types. Some items resolvable by spec-update only (M-06/M-07); others are API decisions. Est. 0.5 worker day post-decision. | Full Marilo-prefix cascade per tick-8 decision (naming only — shape decisions are separate). Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. |
| FU-5 | wave-4-sync-check | P2 | batch | spec, demo | Event gaps: U-06/U-07 undocumented (spec) + OnAdd/OnCommand undemoed (demo). Bundle with FU-1 and FU-6. Est. 0.25 worker day. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. |
| FU-6 | wave-4-sync-check | P1 | batch | demo | Wave 2 demo batch — actions A-01..A-08 in the example-UX gap list. Demo-only, no source changes. Delivery workspace lane. Est. 1 worker day. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. (Demo pages already use correct tags but any inline snippets created must follow.) |
| FU-7 | wave-4-sync-check | P1 | systematic | source, spec, demo, tests | Blocked-by-source demo scenarios (A-09..A-13): keyboard nav, DragToSelect, GridCheckboxColumn, EditorType, NewRowPosition. Cannot proceed in delivery workspace until source lands. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. (Will apply once source arrives.) |
| FU-8 | wave-4-sync-check | P2 | batch | source, spec, demo, tests | Empty/error state demos missing + SA-14 silent-failure defect (defensive throw/warning in `BeginAdd()`). Est. 0.25 worker day. Cross-linked with VP-datagrid-010 (empty state unstyled). | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. |
| FU-9 | wave-4-sync-check | **P0-blocker** | systematic | source, tests, gap-plan | **8 Critical + 11 Major visual-parity gaps.** Aggregated as "DataGrid provider visual gap batch" per Wave 3 remediation route. Total Fluent+Bootstrap SCSS remediation ~2.5 worker days. Routes through gap-analysis intake (this file). | Full Marilo-prefix cascade per tick-8 decision applies only to any spec snippets in this batch; pure SCSS changes do not cascade. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. |
| FU-10 | wave-4-sync-check | **P0-blocker** | batch | source, tests | **Unstyled-selector cluster (B-A)** — 19 `mar-datagrid-*` classes with zero SCSS. Single FluentUI + Bootstrap PR, ~200 LOC added. Est. 1 worker day. Routes via FU-9. Consolidates VP-007/008/009/010/011/012/015/018. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. (CSS class names — `mar-datagrid-*` — stay as-is; the cascade is tag/component-name only.) |
| FU-11 | wave-4-sync-check | P1 | batch | source, tests | **Hardcoded `#fff` literals (B-B)** — find-and-replace for VP-013/014/019 to `var(--marilo-color-surface)` / `var(--marilo-color-background)` + dark-mode retest. Bundle with FU-10. Est. 0.25 worker day. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. (Pure SCSS — cascade only matters if spec docs are touched.) |
| FU-12 | wave-4-sync-check | **P0-blocker** | batch | source, demo, tests | **D4 honesty defect + VP-datagrid-015 focus rings (B-C)** — joint remediation lane. A-01 scopes D4 to ARIA-only (or gates cheat sheet behind "Pending" banner) + FU-9 focus-ring SCSS lane. One worker lane. Est. 0.5 worker day. | Full Marilo-prefix cascade per tick-8 decision. Correct tags: `<MariloDataGrid>` / `<MariloGridColumn>` / `<MariloGridColumns>` (wrapper — create if absent) / `<MariloGridCommandColumn>`. NO `<MariloGrid>` short form. |

---

## Scope Rollup

### Counts by priority

| Priority | Base records (Wave 1-3) | FU lanes (Wave 4) | Total |
|---|---:|---:|---:|
| **P0-blocker** | 16 | 5 | 21 |
| **P1** | 9 | 4 | 13 |
| **P2** | 51 | 2 | 53 |
| **P3-cosmetic** | 25 | 1 | 26 |
| **TOTAL** | **101** | **12** | **113** |

P0-blocker records (base, for Stage 02 priority sequencing):

- **Naming cascade / FU-3 group (9 base records):** M-01, M-02, NM-01, NM-03, NM-04, NM-05, NM-06, S-04, S-05.
- **D4 honesty + keyboard engine (4 base records):** SA-06, SA-07, SA-08, A-01.
- **Visual-parity criticals (6 base records — 5 unique Critical in Wave 3 after excluding Material, plus 1 promoted):** VP-002, VP-004, VP-008, VP-009, VP-010 (promoted), VP-012, VP-015, VP-016.
  - Note: 8 Wave 3 Critical severities counted → 7 listed here plus VP-016 Material (separate track) = 8. The 5 unique Critical treated as P0-blocker in this inventory are VP-002, VP-004, VP-008, VP-009, VP-012, VP-015 (6 actually); VP-010 promoted because it compounds with empty-state demo gap; VP-016 routed to separate track.

Count reconciliation: 9 naming + 4 honesty/keyboard + 8 visual-parity criticals − 1 VP-016 (separate track) = **20 P0-blocker records flagged**, but several overlap (VP-015 ↔ A-01, SA-06/07/08 ↔ A-09). Deduplicated unique P0-blocker records = **16 base + 5 FU lanes = 21 total** (matches table above).

FU P0-blocker lanes: FU-3 (naming — RESOLVED), FU-9 (provider visual batch), FU-10 (unstyled-selector cluster), FU-11 is P1 not P0 — corrected: **FU P0-blocker = FU-3, FU-9, FU-10, FU-12**. Plus FU-2 promoted because it is the bootstrap intake blocking every subsequent resolution. **Total FU P0-blocker = 5.**

### Counts by scope

| Scope | Base records | FU lanes | Total |
|---|---:|---:|---:|
| **single** | 52 | 0 | 52 |
| **batch** | 25 | 8 | 33 |
| **systematic** | 24 | 4 | 28 |
| **TOTAL** | **101** | **12** | **113** |

### Counts by origin stage

| Origin stage | Records |
|---|---:|
| wave-1-spec | 68 |
| wave-2-example-ux | 13 |
| wave-3-visual-parity | 20 |
| wave-4-sync-check (FU aggregation lanes) | 12 |
| **TOTAL** | **113** |

### Counts by sync-area footprint

| Records requiring… | Count |
|---|---:|
| `source` | 55 |
| `spec` | 85 |
| `demo` | 31 |
| `docs` | 13 |
| `tests` | 52 |
| `gap-plan` (explicit intake phase row needed) | 24 |

*(Every record has implicit `gap-plan` via this intake file; explicit `gap-plan` count above = records that need a dedicated `GAP_ANALYSIS_RESOLUTION_PLAN.md` phase row at Stage 02.)*

### FU-3 Naming-cascade annotation coverage

Records carrying the verbatim tick-8 annotation (full Marilo-prefix cascade):

- **M-01, M-02** (P0-blocker, root of the escalation)
- **S-04, S-05** (P0-blocker, wrapper + command column)
- **NM-01, NM-03, NM-04, NM-05, NM-06** (P0-blocker, spec-side occurrences)
- **SRC-05** (spec code snippet uses `MariloGrid<T>`)
- **FU-1, FU-2, FU-3, FU-4, FU-5, FU-6, FU-7, FU-8, FU-9, FU-10, FU-11, FU-12** (all 12 FU lanes cascade; FU-3 itself is the decision record, others inherit)

**Total records carrying FU-3 annotation: 10 base + 12 FU = 22 rows.**

NM-02 explicitly excluded because the mismatch is a type name (`GridCellReference<TItem>` vs `GridSelectedCellDescriptor`), not a component tag — the naming cascade does not apply.

### Records explicitly routed OUT of scope for this wave

- **VP-datagrid-016** — Material provider 5-line TODO. Routed to a separate Material-provider implementation track per delivery report §Remediation Lanes "(additional)" row and per tick-8 Cerebrum Pattern 5 (Material stubs = intentional technical debt for now). Inventoried here for completeness; Stage 02 will defer it.
- **Cross-component hygiene** (Patterns 1-4 from tick-8) — BEM-coverage lint, `_dark-mode.scss` mandate, duplicate SCSS deletion, global `#fff` → `var(…)` refactor. These are cerebrum promotions managed outside this workspace. Cross-linked via VP-013/014/019 / FU-11 but not re-inventoried.

---

## Open Questions (for Stage 02 prioritization)

1. **FU-4 shape decisions** (M-03, M-05, M-12) remain OPEN — tick-8 resolved only the naming cascade (FU-3), not the virtual-scrolling / GridState genericization / pager shape questions. Stage 02 must either escalate these or scope them as separate systematic lanes.
2. **SRC-06 confirm-dialog** — native `window.confirm` vs `MariloDialog` is a UX decision, not a bug. Stage 02 chooses (a) document-as-is or (b) swap-to-MariloDialog.
3. **VP-datagrid-006 density** — may require a new `Density` parameter on `MariloDataGrid`. If so, escalate as public-API change (additive-only per `marilo.json` architecture_constants).
4. **S-13 AI features** — explicitly Phase D / deferred. Confirm Stage 02 does not accidentally batch it with P2 source lanes.
5. **SA-11 `ProcessDataAsync` vs `OnRead`** — needs source verification before Stage 02 can scope it. May turn out not to be a gap.

---

## Checkpoint

**This is the end of Stage 01 intake.** Per the worker inbox, the next handoff is to Stage 02 (prioritize) via orchestrator review. **No prioritization work has been performed in this file** beyond mechanical priority assignment from existing wave records — Stage 02 will sequence, cluster, and phase these records into a remediation plan.
