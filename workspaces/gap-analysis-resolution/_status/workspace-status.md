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
| Last updated | 2026-04-05 |
| Current phase | Stage 06 complete (DataGrid Ph1+Ph2 + splitter + wizard + T4 B1-B3 + chart B1+B2 + editor B1) |

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

**t4-pickers / readonly-guards / expandall-lazyload** -- BATCH 1+2+3 CLOSED
- [x] 01-intake through 06-validate (Batch 1 complete: 7 resolved, 3 partial; 17 tests)
- [x] 01-intake through 06-validate (Batch 2 complete: 4 resolved; 9 tests)
- [x] 03-resolution through 06-validate (Batch 3: AdaptiveMode 7 pickers, ARIA combobox, CSS provider; 17 tests, 547/547 full suite)

**Chart batch (16 gaps: 13 resolved B1+B2, 2 deferred, 1 partial)** -- BATCH 2 CLOSED
- [x] 01-intake through 06-validate (Batch 1: wrappers, subtitle, CSS vars, 16 tests)
- [x] 03-resolution through 06-validate (Batch 2: bubble, transitions, OnRender, tooltip template, 11 tests)

**Editor batch (12 gaps: 6 resolved B1, 6 remaining)** -- BATCH 1 CLOSED
- [x] 01-intake through 06-validate (Batch 1: validation, custom tools, docs, 14 tests)

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

## Next Actions

1. Editor Batch 2: Adaptive toolbar (JS), table/image resize (JS), import/export (needs Markdig decision).
2. T4 Pickers remaining: GroupField (MultiSelect), DateTimePickerSteps.
3. DataGrid Phase 3 remaining: Frozen columns (JS), Row drag-drop (JS).
4. Chart: Drilldown feature (separate scope / CDW).

## Blockers

- Editor import/export: Needs decision on Markdown library dependency (Markdig vs custom).
- Editor adaptive toolbar + table resize: Require JS interop (ResizeObserver, drag handles).
- DataGrid frozen columns + row drag: Require JS interop.
