# Priority Sequence: Phase 1

**Date:** 2026-04-05
**Scope:** batch

## Sequencing Rationale

Test coverage gaps (GAP-TEST-*) are higher priority than P3 demo gaps because tests protect against regressions. Within demo gaps, parameters that affect visible behaviour are prioritized over passthrough styling parameters.

## Sequence

| Order | ID | Description | Effort |
|-------|----|-------------|--------|
| 1 | GAP-TEST-001 | Expand bUnit parameter coverage | Medium |
| 2 | GAP-TEST-002 | Add scenario planning tests | Medium |
| 3 | GAP-P3-009 | CanExecuteAction demo (enables context menu enable/disable) | Small |
| 4 | GAP-P3-006 | OnAllocationOverridden demo (scenario editing) | Small |
| 5 | GAP-P3-001 | ShowCriticalPath demo | Small |
| 6 | GAP-P3-004 | EnableLoaderContainer demo | Small |
| 7 | GAP-P3-012 | BaselineDateFormat demo | Small |
| 8 | GAP-P3-005 | VisibleEnd demo | Small |
| 9 | GAP-P3-010 | VisibleStartChanged demo | Small |
| 10 | GAP-P3-007 | OnScenarioStatusChanged demo | Small |
| 11 | GAP-P3-008 | OnScenarioPromoted demo | Small |
| 12 | GAP-P3-011 | ActiveSetIdChanged demo (already partially covered) | Small |
| 13 | GAP-P3-002 | Width demo | Trivial |
| 14 | GAP-P3-003 | Class demo | Trivial |
