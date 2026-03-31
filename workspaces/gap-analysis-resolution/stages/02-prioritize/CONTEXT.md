# Prioritize

Score gaps on impact and effort, sequence by dependencies, produce a resolution backlog ordered for execution.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../01-intake/output/gap-[slug]-inventory.md` | Full file | Normalized gap records to prioritize |
| Config | `../../_config/gap-context.md` | "Resolution Scope" section | Scope and constraints |
| Priority framework | `../../shared/priority-framework.md` | Full file | Scoring criteria and sequencing rules |
| Target project | Path from gap-context.md | Relevant source files | Understand dependencies between gaps |

## Process

1. Read the gap inventory from Stage 01 output.
2. For each gap, score using the priority framework dimensions: risk, user impact, architectural importance, and effort. Record scores.
3. Assign each gap to a priority band: **Critical** (must fix before any release), **Important** (fix in current cycle), **Nice-to-have** (fix when convenient).
4. Identify dependencies between gaps. A gap is a dependency if resolving gap B requires gap A to be resolved first (e.g., base class changes before component-level changes).
5. Build a dependency graph. Flag circular dependencies for human resolution.
6. Sequence gaps into resolution phases:
   - **Phase 1 (Foundation):** Cross-cutting gaps, base class changes, shared infrastructure.
   - **Phase 2 (Core):** Critical and important gaps with no unresolved dependencies.
   - **Phase 3 (Expansion):** Remaining important gaps.
   - **Phase 4 (Polish):** Nice-to-have gaps.
7. Save the prioritized backlog to output.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| Step 3 | Priority band assignments with rationale | Adjust priorities, override bands |
| Step 6 | Sequenced phases with dependency graph | Approve phase plan, reorder if needed |

## Audit

| Check | Pass Condition |
|-------|---------------|
| All gaps scored | Every gap in the inventory has scores on all four dimensions |
| No orphaned dependencies | Every dependency target exists in the inventory |
| Phases are acyclic | No phase depends on a later phase |
| Critical gaps in Phase 1 or 2 | No critical gap deferred to Phase 3 or 4 |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Resolution backlog | `output/gap-[slug]-backlog.md` | Prioritized, phased gap list with scores and dependencies |
