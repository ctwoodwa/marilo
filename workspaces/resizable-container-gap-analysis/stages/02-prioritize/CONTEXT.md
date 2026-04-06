# Stage 02 -- Prioritize

Score gaps on impact and effort, sequence by dependencies, produce a resolution backlog ordered for execution.

## Purpose

Transform the raw gap inventory into an actionable, sequenced backlog. Every gap gets a priority score and phase assignment so downstream stages know what to resolve first.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../01-intake/output/gap-resizable-container-inventory.md` | Full file | Normalized gap records to prioritize |
| Config | `../../_config/gap-context.md` | Resolution Scope section | Scope and constraints |
| Priority framework | `../../shared/priority-framework.md` | Full file | P1-P4 scoring criteria and sequencing rules |
| Component source | `src/Marilo.Components/Layout/ResizableContainer/` | Relevant files | Understand dependencies between gaps |

## Process

1. Read the gap inventory from Stage 01 output.
2. For each gap, score using the priority framework dimensions: risk, user impact, architectural importance, and effort. Record scores.
3. Assign each gap to a priority level: P1 (Blocking), P2 (This phase), P3 (Next phase), P4 (Backlog).
4. Identify dependencies between gaps. A gap is a dependency if resolving gap B requires gap A to be resolved first.
5. Build a dependency graph. Flag circular dependencies for human resolution.
6. Sequence gaps into resolution phases:
   - **Phase 1 (Foundation):** Cross-cutting gaps, shared infrastructure changes.
   - **Phase 2 (Core):** P1 and P2 component-level gaps with no unresolved dependencies.
   - **Phase 3 (Expansion):** Remaining P2 and P3 gaps.
   - **Phase 4 (Polish):** P4 gaps, cosmetic alignment.
7. Save the prioritized backlog to `output/`.

## Audit Checklist

| Check | Pass Condition |
|-------|---------------|
| All gaps scored | Every gap in the inventory has scores on all four dimensions |
| No orphaned dependencies | Every dependency target exists in the inventory |
| Phases are acyclic | No phase depends on a later phase |
| P1 gaps in Phase 1 or 2 | No blocking gap deferred to Phase 3 or 4 |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Resolution backlog | `output/gap-resizable-container-backlog.md` | Prioritized, phased gap list with scores and dependencies |
