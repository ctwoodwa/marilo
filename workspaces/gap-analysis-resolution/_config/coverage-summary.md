# Coverage Summary

## Snapshot
- Date: 2026-04-04T12:00:00Z
- Branch: workInProgress
- Scope touched this run: Splitter, Wizard
- Plan source: `src/Marilo.Components/GAP_ANALYSIS_RESOLUTION_PLAN.md`

## Component Status
| Area | Open | In Progress | Completed | Blocked | Stage Focus | Tests Passing | Notes |
|------|------|-------------|-----------|---------|-------------|---------------|-------|
| TreeView | 0 | 0 | 21 | 1 (virtualization deferred) | 06-validate | 67/67 | Full closure report complete |
| DataGrid | ~35-50 | 0 | 0 | 0 | 01-intake | 0 | Per-feature checklist ready for CDW |
| DataSheet | 0 | 0 | 0 | 1 (architecture decision) | — | 0 | Blocked: MariloSpreadsheet vs MariloDataSheet |
| Forms | ~60 | 0 | 22+12+4+4 | 6 deferred | 03-resolution | 20/20 | Resolution design done; awaiting implementation |
| T4 Pickers | ~28 | 0 | 7 | 3 partially resolved | 06-validate (B1) | 17/17 | Batch 1 closed; Batch 2-3 pending |
| Splitter | 6 | 0 | 4 (pre-resolved) | 0 | **03-resolution** | 0 | Resolution design complete this run; 4 gaps already in code |
| Wizard | 18 | 0 | 0 | 0 | **03-resolution** | 0 | Resolution design complete this run; wizard non-functional (GAP-018) |
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
| Item | Stage 03 Resolution | Stage 05 Implementation | Stage 06 Closure | Last Updated |
|------|---------------------|-------------------------|------------------|--------------|
| Splitter | `stages/03-resolution-design/output/gap-splitter-resolutions.md` | — | — | 2026-04-04 |
| Wizard | `stages/03-resolution-design/output/gap-wizard-resolutions.md` | — | — | 2026-04-04 |

## Recent Movement
- Splitter: Stage 03 resolution design completed (5 resolutions + 4 pre-resolved gaps identified via code audit)
- Wizard: Stage 03 resolution design completed (14 resolutions covering all 18 gaps; critical CascadingValue bug documented)
- Both components ready for Stage 05 implementation

## Active Blockers
- TreeView Delivery: Demo scope approval needed (human decision)
- DataSheet: Architecture decision (MariloSpreadsheet vs MariloDataSheet)
- No-source components (Diagram, DockManager, Map, PivotGrid): Build/integrate/defer decision needed
