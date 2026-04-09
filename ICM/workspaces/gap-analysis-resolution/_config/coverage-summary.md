# Coverage Summary

## Snapshot
- Date: 2026-04-08T00:00:00Z
- Branch: workInProgress
- Scope touched this run: T4 Pickers Batches 4–7 (MariloMultiSelect Batches 4–7 closing every actionable medium+ gap; MariloDateTimePicker Batches 4–5 tumbler-steps and typed-input). Batch 7 executed via subagent-driven development with two-stage review (spec compliance + code quality + fix-and-re-review loop).
- Plan source: `src/Marilo.Components/GAP_ANALYSIS_RESOLUTION_PLAN.md`

## Component Status
| Area | Open | In Progress | Completed | Blocked | Stage Focus | Tests Written | Notes |
|------|------|-------------|-----------|---------|-------------|---------------|-------|
| TreeView | 0 | 0 | 21 | 1 (virtualization deferred) | 06-validate | 67/67 | Full closure report complete |
| DataGrid | ~33-48 | 0 | 17 (Ph1+Ph2+Ph3) | 3 (expand args, frozen cols, drag-drop) | **06-validate** | 47 | Phase 1+2+3 closed (17/71); 2 JS deferred |
| DataSheet | 0 | 0 | 0 | 1 (architecture decision) | — | 0 | Blocked: MariloSpreadsheet vs MariloDataSheet |
| Forms | ~60 | 0 | 22+12+4+4 | 6 deferred | 03-resolution | 20/20 | Resolution design done; awaiting implementation |
| T4 Pickers | ~6 | 0 | 30 (B1+B2+B3+B4+B5+B6+B7) | 1 partial (MSEL-007 ScrollMode deferred) + 1 won't fix (MSEL-008 naming) | **06-validate (B7)** | 85 (17+9+17+12+12+11+7) | Batch 1–7 closed; **MariloMultiSelect feature-complete** for medium+ gaps; **runtime validated 2026-04-09: 667/667 full suite** |
| Splitter | 0 | 0 | 8 resolved | 1 demo deferred | **06-validate** | 17 | Stage 06 closure report complete; runtime test pending |
| Wizard | 0 | 0 | 18 resolved | 0 | **06-validate** | 27 | Stage 06 closure report complete; runtime test pending |
| Chart | 2 remaining | 0 | 13 (B1+B2) | 2 deferred | **06-validate (B2)** | 27 | Batch 1+2 closed; 4 pre-existing, 9 implemented; drilldown+demos deferred |
| Editor | ~5 remaining | 0 | 7 (B1+B2a) | 0 | **06-validate (B2a)** | 22 (14+8) | Batch 1+2a closed; B2a adds Markdig import/export; 675/675 runtime validated |
| FileManager | ~20-30 | 0 | 0 | 0 | 01-intake | 0 | Intake complete; awaiting prioritization |
| Scheduler | ~25-40 | 0 | 0 | 0 | 01-intake | 0 | Recommend dedicated CDW |
| Gantt | 0 | 0 | 20 | 0 | **06-validate** | 31 | Full rewrite complete; 20/20 gaps resolved; subagent-driven dev |
| TreeList | ~35-55 | 0 | 0 | 0 | 01-intake | 0 | Recommend dedicated CDW |
| Diagram | ~15-25 | 0 | 0 | 1 (no source) | — | 0 | Architecture decision needed |
| DockManager | ~15-25 | 0 | 0 | 1 (no source) | — | 0 | Architecture decision needed |
| Map | ~15-25 | 0 | 0 | 1 (no source) | — | 0 | Architecture decision needed |
| PivotGrid | ~15-25 | 0 | 0 | 1 (no source) | — | 0 | Architecture decision needed |
| ResizableContainer | 0 | 0 | 0 | 0 | — (new component) | 27 | Built via component-builder; dedicated CDW + gap workspace |

## Stage Output Index
| Item | Stage 03 Resolution | Stage 05 Implementation | Stage 06 Closure | Last Updated |
|------|---------------------|-------------------------|------------------|--------------|
| DataGrid Ph1 | `stages/03-resolution-design/output/gap-datagrid-phase1-resolutions.md` | `stages/05-implement/output/gap-datagrid-phase1-implementation-log.md` | `stages/06-validate/output/gap-datagrid-phase1-closure-report.md` | 2026-04-04 |
| DataGrid Ph2 | `stages/03-resolution-design/output/gap-datagrid-phase2-resolutions.md` | `stages/05-implement/output/gap-datagrid-phase2-implementation-log.md` | `stages/06-validate/output/gap-datagrid-phase2-closure-report.md` | 2026-04-04 |
| Splitter | `stages/03-resolution-design/output/gap-splitter-resolutions.md` | `stages/05-implement/output/gap-splitter-implementation-log.md` | `stages/06-validate/output/gap-splitter-closure-report.md` | 2026-04-04 |
| Wizard | `stages/03-resolution-design/output/gap-wizard-resolutions.md` | `stages/05-implement/output/gap-wizard-implementation-log.md` | `stages/06-validate/output/gap-wizard-closure-report.md` | 2026-04-04 |
| T4 Pickers B2 | — | `stages/05-implement/output/gap-t4-picker-batch2-implementation-log.md` | `stages/06-validate/output/gap-t4-picker-batch2-closure-report.md` | 2026-04-04 |
| Chart B1 | — | `stages/05-implement/output/gap-chart-batch1-implementation-log.md` | `stages/06-validate/output/gap-chart-batch1-closure-report.md` | 2026-04-04 |
| Editor B1 | — | `stages/05-implement/output/gap-editor-batch1-implementation-log.md` | `stages/06-validate/output/gap-editor-batch1-closure-report.md` | 2026-04-04 |

| Chart B2 | `stages/03-resolution-design/output/gap-chart-batch2-resolutions.md` | `stages/05-implement/output/gap-chart-batch2-implementation-log.md` | `stages/06-validate/output/gap-chart-batch2-closure-report.md` | 2026-04-04 |
| DataGrid Ph1+Ph2 Closure | — | — | `stages/06-validate/output/gap-datagrid-phase1-closure-report.md`, `gap-datagrid-phase2-closure-report.md` | 2026-04-04 |
| T4 Pickers B4 | `stages/03-resolution-design/output/gap-t4-picker-batch4-resolutions.md` | `stages/05-implement/output/gap-t4-picker-batch4-implementation-log.md` | `stages/06-validate/output/gap-t4-picker-batch4-closure-report.md` | 2026-04-08 |
| T4 Pickers B5 | `stages/03-resolution-design/output/gap-t4-picker-batch5-resolutions.md` | `stages/05-implement/output/gap-t4-picker-batch5-implementation-log.md` | `stages/06-validate/output/gap-t4-picker-batch5-closure-report.md` | 2026-04-08 |
| T4 Pickers B6 | `stages/03-resolution-design/output/gap-t4-picker-batch6-resolutions.md` | `stages/05-implement/output/gap-t4-picker-batch6-implementation-log.md` | `stages/06-validate/output/gap-t4-picker-batch6-closure-report.md` | 2026-04-08 |
| T4 Pickers B7 | `stages/03-resolution-design/output/gap-t4-picker-batch7-resolutions.md` | `stages/05-implement/output/gap-t4-picker-batch7-implementation-log.md` | `stages/06-validate/output/gap-t4-picker-batch7-closure-report.md` | 2026-04-08 |
| Editor B2a | `stages/03-resolution-design/output/gap-editor-batch2-import-export-resolutions.md` | `stages/05-implement/output/gap-editor-batch2a-implementation-log.md` | `stages/06-validate/output/gap-editor-batch2a-closure-report.md` | 2026-04-09 |
| Gantt (full) | — | — (24 commits on `gantt-rewrite` branch) | `stages/06-validate/output/gap-gantt-closure-report.md` | 2026-04-09 |

## Recent Movement

- Gantt full rewrite (2026-04-09): Complete generic rewrite of MariloGantt from 95-line scaffold to full-featured component. 20/20 gaps resolved across 5 phases (A: Foundation, B: Child Components, C: Features, D: JS Interop, E: Tests+Demos). 24 commits, 31 bUnit tests, 5 demo pages. Executed via subagent-driven development with two-stage review. Branch: `gantt-rewrite`.
- Editor Batch 2a (2026-04-09): MariloEditor import/export with Markdig (MIT) + plaintext adapters; IEditorFormatConverter interface + DI registration; 8 bUnit tests; 675/675 full suite runtime validated. Closes GAP-EDITOR-005. First third-party NuGet on Marilo.Components.
- T4 Pickers Batch 7 (2026-04-08): MariloMultiSelect MultiSelectSettings + MultiSelectPopupSettings child component API (interface-decoupled cascade, non-generic children, 5 Effective* properties, canonical CascadingValue wrap with interface cast); 7 bUnit tests; Stage 03→05→06 complete via subagent-driven dev (implementer + spec-compliance review + code-quality review + fix-and-re-review loop). Closes GAP-MSEL-005. Also fixed pre-existing Batch 6 build break in `OnChange_DoesNotFireOnExternalValueSet` (`SetParametersAndRender` → bUnit v2 `Render` rebind). MariloMultiSelect now feature-complete for all medium+ gaps.
- T4 Pickers Batch 6 (2026-04-08): MariloMultiSelect OnChange + OnItemRender (cached args, IsDisabled blocks selection) + ItemHeight/PageSize virtualization config; 11 bUnit tests; Stage 03→05→06 complete; closes GAP-MSEL-001 fully (across B1+B5+B6) and GAP-MSEL-007 ItemHeight/PageSize (ScrollMode deferred with rationale)
- T4 Pickers Batch 5 (2026-04-08): MariloMultiSelect OnRead/Rebind/ValueMapper + MariloDateTimePicker typed input parsing; 12 bUnit tests; Stage 03→05→06 complete; closes GAP-MSEL-006, GAP-DTP-003, and the OnRead portion of GAP-MSEL-001
- T4 Pickers Batch 4 (2026-04-08): MariloMultiSelect GroupField + MariloDateTimePicker tumbler step parameters; 12 bUnit tests; Stage 03→05→06 complete
- DataGrid Phase 2: 6 important gaps resolved (validation, composite filters, auto-gen attributes, aggregates, export lifecycle, CancellationToken)
- DataGrid Phase 1: 9 pure C# gaps resolved (SortMode, Editable, ConfirmDelete, SetStateAsync, AddFilter/ClearFilters, pager, DisplayFormat, Groupable, ExpandedItems)
- DataGrid: 37 total bUnit tests (4 original + 18 Ph1 + 15 Ph2)
- Chart Batch 1: Stage 06 closure (8/8 resolved — 3 pre-existing, 5 implemented; 16 bUnit tests)
- Editor Batch 1: Stage 06 closure (6/6 resolved — 2 pre-existing, 4 implemented; 14 bUnit tests)
- T4 Pickers Batch 2: Stage 06 closure (4/4 resolved; 9 bUnit tests)
- Splitter: Stage 06 closure (8/10 resolved; 17 bUnit tests)
- Wizard: Stage 06 closure (18/18 resolved; 27 bUnit tests)

## Active Blockers
- ~~TreeView Delivery: Demo scope approval needed~~ → **RESOLVED 2026-04-09:** Focus on scenario coverage and spec alignment
- ~~DataSheet: Architecture decision~~ → **RESOLVED 2026-04-09:** True spreadsheet component with its own architecture (not DataGrid reuse)
- ~~No-source components~~ → **RESOLVED 2026-04-09:** Spec + concept-demo only; no full implementation until source exists
- ~~.NET SDK not available~~ → **RESOLVED 2026-04-09:** SDK available, 667/667 tests passing
- Editor Batch 2: Markdig approved as bounded Markdown adapter for import/export (not as core model). JS interop still needed for adaptive toolbar + table/image resize.
- DataGrid Phase 3+: Frozen columns + Row drag-drop require JS interop
