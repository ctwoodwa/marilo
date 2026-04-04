# Coverage Summary

## Snapshot
- Date: 2026-04-04T18:00:00Z
- Branch: claude/marilo-gap-resolution-LDyRw
- Scope touched this run: Splitter, Wizard (Stage 05 implementation)
- Plan source: `src/Marilo.Components/GAP_ANALYSIS_RESOLUTION_PLAN.md`

## Component Status
| Area | Open | In Progress | Completed | Blocked | Stage Focus | Tests Written | Notes |
|------|------|-------------|-----------|---------|-------------|---------------|-------|
| TreeView | 0 | 0 | 21 | 1 (virtualization deferred) | 06-validate | 67/67 | Full closure report complete |
| DataGrid | ~35-50 | 0 | 0 | 0 | 01-intake | 0 | Per-feature checklist ready for CDW |
| DataSheet | 0 | 0 | 0 | 1 (architecture decision) | — | 0 | Blocked: MariloSpreadsheet vs MariloDataSheet |
| Forms | ~60 | 0 | 22+12+4+4 | 6 deferred | 03-resolution | 20/20 | Resolution design done; awaiting implementation |
| T4 Pickers | ~28 | 0 | 7 | 3 partially resolved | 06-validate (B1) | 17/17 | Batch 1 closed; Batch 2-3 pending |
| Splitter | 0 | 0 | 9 (4 pre-resolved + 5 implemented) | 1 demo deferred | **05-implement** | 17 | Stage 05 complete this run; awaiting Stage 06 |
| Wizard | 0 | 0 | 18 | 0 | **05-implement** | 30 | Stage 05 complete this run; all 14 resolutions implemented |
| Chart | ~20-30 | 0 | 0 | 0 | 01-intake | 0 | Intake complete; awaiting prioritization |
| Editor | ~15-25 | 0 | 0 | 0 | 01-intake | 0 | Intake complete; awaiting prioritization |
| FileManager | ~20-30 | 0 | 0 | 0 | 01-intake | 0 | Intake complete; awaiting prioritization |
| Scheduler | ~25-40 | 0 | 0 | 0 | 01-intake | 0 | Recommend dedicated CDW |
| Gantt | ~30-50 | 0 | 0 | 0 | 01-intake | 0 | Recommend dedicated CDW |
| TreeList | ~35-55 | 0 | 0 | 0 | 01-intake | 0 | Recommend dedicated CDW |
| Diagram | ~15-25 | 0 | 0 | 1 (no source) | — | 0 | Architecture decision needed |
| DockManager | ~15-25 | 0 | 0 | 1 (no source) | — | 0 | Architecture decision needed |
| Map | ~15-25 | 0 | 0 | 1 (no source) | — | 0 | Architecture decision needed |
| PivotGrid | ~15-25 | 0 | 0 | 1 (no source) | — | 0 | Architecture decision needed |

## Stage Output Index
| Item | Stage 05 Implementation | Stage 06 Closure | Last Updated |
|------|-------------------------|------------------|--------------|
| Splitter | `stages/05-implement/output/gap-splitter-implementation-log.md` | — | 2026-04-04 |
| Wizard | `stages/05-implement/output/gap-wizard-implementation-log.md` | — | 2026-04-04 |

## Recent Movement
- Splitter: Stage 05 implementation completed (SplitterOrientation enum, MariloSplitterPanes wrapper, 17 bUnit tests)
- Wizard: Stage 05 implementation completed (CascadingValue fix, Value/ValueChanged rename, 14 resolutions, 30 bUnit tests)
- Both components ready for Stage 06 validation

## Active Blockers
- TreeView Delivery: Demo scope approval needed (human decision)
- DataSheet: Architecture decision (MariloSpreadsheet vs MariloDataSheet)
- No-source components (Diagram, DockManager, Map, PivotGrid): Build/integrate/defer decision needed
- .NET SDK not available: Cannot run `dotnet test` to verify test pass/fail
