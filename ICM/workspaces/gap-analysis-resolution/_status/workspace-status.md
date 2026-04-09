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
| Last updated | 2026-04-09 |
| Current phase | **T4 Batch 8 complete (13 gaps, 48 tests).** Stage 06 complete (DataGrid Ph1+Ph2+Ph3 partial + splitter + wizard + T4 B1-B8 + chart B1+B2 + editor B1+B2a + Gantt full rewrite). 726/726 full suite. |

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

## Next Actions

1. **ColorPicker standalone components** (FlatColorPicker, ColorGradient, ColorPalette) — significant new component work, may need own batch/CDW.
2. **DateRangePicker multi-view calendar** (Year/Decade views) — large feature, separate batch.
3. Editor Batch 2 remainder: Adaptive toolbar (JS), table/image resize (JS).
4. DataGrid Phase 3 remaining: Frozen columns (JS), Row drag-drop (JS).
5. Chart: Drilldown feature (separate scope / CDW).
6. FileManager: Stage 02 prioritization ready (intake complete).

## Human Decisions Resolved (2026-04-09)

- Editor: **Markdig approved** as bounded Markdown adapter for import/export (not as core model)
- DataSheet: **True spreadsheet** with its own architecture (not DataGrid reuse)
- TreeView demos: **Scenario coverage** and spec alignment (not exhaustive UX exploration)
- No-source components: **Spec + concept-demo only** until source exists

## Blockers

- Editor adaptive toolbar + table resize: Require JS interop (ResizeObserver, drag handles).
- DataGrid frozen columns + row drag: Require JS interop.
