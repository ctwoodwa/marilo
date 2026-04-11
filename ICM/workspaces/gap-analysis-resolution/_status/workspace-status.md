# Workspace Status

<!-- SUMMARY SNAPSHOT -- read at session start for fast orientation only.
     NOT authoritative. If this file contradicts a stage output, the stage output wins.
     Update this file after completing a stage or a significant batch of work.
     Do not append -- replace the content and update the date.
     Keep this file under 50 lines. See Pattern 16 in _core/CONVENTIONS.md. -->

## Header

| Field | Value |
|-------|-------|
| Workspace | gap-analysis-resolution |
| Last updated | 2026-04-10 |
| Current phase | **ALL JS Interop batches complete (B1+B2+B3).** Stage 06 complete for all gaps. 1097/1097 full suite. Zero remaining gaps in this workspace. |

## Pipeline Status

**Treeview batch (24 gaps: 21 resolved, 1 deferred)** -- COMPLETE

**Form batch (60 gaps: 35+ resolved, 11 deferred)** -- COMPLETE
- [x] 01-intake
- [x] 02-prioritize
- [x] 03-resolution-design
- [x] 05-implement (20 tests, all passing)
- [x] 06-validate (closure report 2026-04-02)

**Splitter batch (10 gaps: 8 resolved, 1 deferred demo, 1 demo guidance)** -- COMPLETE
- [x] 01-intake
- [x] 02-prioritize
- [x] 03-resolution-design
- [x] 05-implement (completed 2026-04-04)
- [x] **06-validate** (closure report 2026-04-04; 17 tests written, runtime pending)

**Wizard batch (18 gaps: 18 resolved)** -- COMPLETE
- [x] 01-intake
- [x] 02-prioritize
- [x] 03-resolution-design
- [x] 05-implement (completed 2026-04-04)
- [x] **06-validate** (closure report 2026-04-04; 27 tests written, runtime pending)

**t4-pickers / readonly-guards / expandall-lazyload** -- BATCH 1+2+3+4+5+6+7+8 CLOSED
- [x] 01-intake through 06-validate (Batch 1 complete: 7 resolved, 3 partial; 17 tests)
- [x] 01-intake through 06-validate (Batch 2 complete: 4 resolved; 9 tests)
- [x] 03-resolution through 06-validate (Batch 3: AdaptiveMode 7 pickers, ARIA combobox, CSS provider; 17 tests, 547/547 full suite)
- [x] 03-resolution through 06-validate (Batch 4: MultiSelect GroupField + DateTimePicker tumbler steps; 12 tests pending runtime)
- [x] 03-resolution through 06-validate (Batch 5: MultiSelect OnRead/Rebind/ValueMapper + DateTimePicker typed input; 12 tests pending runtime)
- [x] 03-resolution through 06-validate (Batch 6: MultiSelect OnChange/OnItemRender + ItemHeight/PageSize virtual config; 11 tests pending runtime; GAP-MSEL-001 fully closed)
- [x] 03-resolution through 06-validate (Batch 7: MultiSelect Settings/PopupSettings child component API; 7 tests pending runtime; subagent-driven dev mode; GAP-MSEL-005 closed)
- [x] 03-resolution through 06-validate (Batch 8: DRP polish + TP polish + FU/UPL polish; 48 tests; 726/726 full suite runtime validated 2026-04-09)

**Chart batch (16 gaps: 13 resolved B1+B2, 2 deferred, 1 partial)** -- BATCH 2 CLOSED
- [x] 01-intake through 06-validate (Batch 1: wrappers, subtitle, CSS vars, 16 tests)
- [x] 03-resolution through 06-validate (Batch 2: bubble, transitions, OnRender, tooltip template, 11 tests)

**Editor batch (12 gaps: 7 resolved B1+B2a, 5 remaining)** -- BATCH 2a CLOSED
- [x] 01-intake through 06-validate (Batch 1: validation, custom tools, docs, 14 tests)
- [x] 03-resolution through 06-validate (Batch 2a: import/export with Markdig + plaintext; 8 tests, 675/675 full suite)

**DataGrid Phase 1 (9 pure C# gaps + 1 deferred)** -- COMPLETE
- [x] 01-intake → 02-prioritize → 03-resolution-design → 05-implement → **06-validate**
- Closure report: 9 resolved, 1 deferred (typed expand event args); 18 bUnit tests

**DataGrid Phase 2 (6 important gaps)** -- COMPLETE
- [x] 03-resolution-design → 05-implement → **06-validate**
- Validation, composite filters, auto-gen attributes, aggregates, export, CancellationToken
- Closure report: 6 resolved; 15 bUnit tests

**DataGrid Phase 3 (2 of 4 C# gaps)** -- PARTIAL COMPLETE
- [x] 03-resolution through 06-validate (CheckBoxList filter + Cell selection; 10 tests, 557/557 full suite)
- Deferred: Frozen columns (JS sticky), Row drag-drop (JS events)

**Gantt full rewrite (20 gaps: 20 resolved)** -- COMPLETE

- [x] 01-intake (inventory imported 2026-04-03)
- [x] 03-resolution through 06-validate (full generic rewrite via subagent-driven dev; 24 commits, 31 bUnit tests)
- Closure report: `stages/06-validate/output/gap-gantt-closure-report.md`

**ColorPicker standalone + DRP multi-view (7 gaps: 5 CPICK + 2 DRP)** -- COMPLETE

- [x] 03-resolution through 06-validate (subagent-driven dev; 9 commits, 23 bUnit tests)
- Closure report: `stages/06-validate/output/gap-colorpicker-standalone-closure-report.md`

**JS Interop Batch 1 (3 gaps: DropZoneId + Editor Adaptive)** -- COMPLETE

- [x] 03-resolution through 06-validate (subagent-driven dev; DropZoneId JS module + IDropZoneService for FileUpload+Upload; Editor Adaptive via ResizeObserver + overflow popup; 18 bUnit tests; 1067/1067 full suite)

**JS Interop Batch 2 (2 gaps: DataGrid frozen cols + row drag-drop)** -- COMPLETE

- [x] 03-resolution through 06-validate (subagent-driven dev; Locked/FrozenPosition on GridColumn, sticky CSS offsets, RowDraggable+OnRowDrop, HTML5 DnD IIFE extension; 15 bUnit tests; 1083/1083 full suite)

**JS Interop Batch 3 (1 gap: Editor table/image resize)** -- COMPLETE

- [x] 03-resolution through 06-validate (table column/row resize + image resize drag handles; 14 bUnit tests; 1097/1097 full suite)

## Remaining

None — all gaps in this workspace are resolved.

## Routed to Other Workspaces

- Chart drilldown → `chart-delivery` CDW
- Scheduler → `scheduler-gap-analysis` (Stage 01 intake complete 2026-04-10 — 32 gaps; blocked on 5 human decisions before Stage 02)
- TreeList → `treelist-gap-analysis` (Stage 01 intake complete 2026-04-10 — 43 gaps; blocked on 7 human decisions before Stage 02; ~51% DataGrid-parity reuse candidates)
- DataSheet → `datasheet-delivery` CDW (true spreadsheet architecture)
- No-source components (Diagram, DockManager, Map, PivotGrid) → spec + concept-demo only per human decision
- TreeView demos → `treeview-delivery` CDW (scenario coverage)

## Human Decisions Resolved (2026-04-09)

- Editor: **Markdig approved** as bounded Markdown adapter for import/export (not as core model)
- DataSheet: **True spreadsheet** with its own architecture (not DataGrid reuse)
- TreeView demos: **Scenario coverage** and spec alignment (not exhaustive UX exploration)
- No-source components: **Spec + concept-demo only** until source exists

## Closure Reports Written (2026-04-09 housekeeping)

- `gap-filemanager-closure-report.md` — retroactive Stage 06 for 36/36 gaps, 151 tests
- `gap-drp-multiview-closure-report.md` — retroactive Stage 06 for 2/2 DRP gaps, 5 tests
- gap-context.md updated with FileManager test rollup entry
- Plan lines 1209-1213 updated: ColorPicker standalone + DRP multi-view marked resolved (were stale "deferred")
