# Coverage Summary

## Snapshot
- Date: 2026-04-04T18:00:00Z
- Branch: claude/marilo-gap-resolution-LDyRw
- Scope touched this run: DataGrid Phase 1+2, Splitter, Wizard, T4 Pickers B2, Chart B1, Editor B1
- Plan source: `src/Marilo.Components/GAP_ANALYSIS_RESOLUTION_PLAN.md`

## Component Status
| Area | Open | In Progress | Completed | Blocked | Stage Focus | Tests Written | Notes |
|------|------|-------------|-----------|---------|-------------|---------------|-------|
| TreeView | 0 | 0 | 21 | 1 (virtualization deferred) | 06-validate | 67/67 | Full closure report complete |
| DataGrid | ~35-50 | 0 | 15 (Ph1+Ph2) | 1 (typed expand args deferred) | **06-validate** | 37 | Phase 1+2 closed (15/71); Phase 3+ pending |
| DataSheet | 0 | 0 | 0 | 1 (architecture decision) | — | 0 | Blocked: MariloSpreadsheet vs MariloDataSheet |
| Forms | ~60 | 0 | 22+12+4+4 | 6 deferred | 03-resolution | 20/20 | Resolution design done; awaiting implementation |
| T4 Pickers | ~28 | 0 | 11 | 3 partially resolved | 06-validate (B2) | 26 (17+9) | Batch 1+2 closed; Batch 3 pending |
| Splitter | 0 | 0 | 8 resolved | 1 demo deferred | **06-validate** | 17 | Stage 06 closure report complete; runtime test pending |
| Wizard | 0 | 0 | 18 resolved | 0 | **06-validate** | 27 | Stage 06 closure report complete; runtime test pending |
| Chart | 2 remaining | 0 | 13 (B1+B2) | 2 deferred | **06-validate (B2)** | 27 | Batch 1+2 closed; 4 pre-existing, 9 implemented; drilldown+demos deferred |
| Editor | ~6 remaining | 0 | 6 (B1) | 0 | **06-validate (B1)** | 14 | Batch 1 closed; 2 already resolved, 4 implemented |
| FileManager | ~20-30 | 0 | 0 | 0 | 01-intake | 0 | Intake complete; awaiting prioritization |
| Scheduler | ~25-40 | 0 | 0 | 0 | 01-intake | 0 | Recommend dedicated CDW |
| Gantt | ~30-50 | 0 | 0 | 0 | 01-intake | 0 | Recommend dedicated CDW |
| TreeList | ~35-55 | 0 | 0 | 0 | 01-intake | 0 | Recommend dedicated CDW |
| Diagram | ~15-25 | 0 | 0 | 1 (no source) | — | 0 | Architecture decision needed |
| DockManager | ~15-25 | 0 | 0 | 1 (no source) | — | 0 | Architecture decision needed |
| Map | ~15-25 | 0 | 0 | 1 (no source) | — | 0 | Architecture decision needed |
| PivotGrid | ~15-25 | 0 | 0 | 1 (no source) | — | 0 | Architecture decision needed |

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

## Recent Movement
- DataGrid Phase 2: 6 important gaps resolved (validation, composite filters, auto-gen attributes, aggregates, export lifecycle, CancellationToken)
- DataGrid Phase 1: 9 pure C# gaps resolved (SortMode, Editable, ConfirmDelete, SetStateAsync, AddFilter/ClearFilters, pager, DisplayFormat, Groupable, ExpandedItems)
- DataGrid: 37 total bUnit tests (4 original + 18 Ph1 + 15 Ph2)
- Chart Batch 1: Stage 06 closure (8/8 resolved — 3 pre-existing, 5 implemented; 16 bUnit tests)
- Editor Batch 1: Stage 06 closure (6/6 resolved — 2 pre-existing, 4 implemented; 14 bUnit tests)
- T4 Pickers Batch 2: Stage 06 closure (4/4 resolved; 9 bUnit tests)
- Splitter: Stage 06 closure (8/10 resolved; 17 bUnit tests)
- Wizard: Stage 06 closure (18/18 resolved; 27 bUnit tests)

## Active Blockers
- TreeView Delivery: Demo scope approval needed (human decision)
- DataSheet: Architecture decision (MariloSpreadsheet vs MariloDataSheet)
- No-source components (Diagram, DockManager, Map, PivotGrid): Build/integrate/defer decision needed
- .NET SDK not available: Cannot run `dotnet test` to verify test pass/fail
