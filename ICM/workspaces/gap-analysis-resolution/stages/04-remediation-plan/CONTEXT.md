# Remediation Plan

Break approved resolutions into implementable tasks with phases, owners, dependencies, and success criteria.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../03-resolution-design/output/gap-[slug]-resolutions.md` | Full file | Approved resolution decisions |
| Backlog | `../02-prioritize/output/gap-[slug]-backlog.md` | Phase assignments | Phase structure and dependencies |
| Config | `../../_config/gap-context.md` | Full file | Project context and constraints |
| Target project | Path from gap-context.md | Project structure | Identify affected files per task |

## Process

1. Read all approved resolution records from Stage 03.
2. For each resolution, break into discrete implementation tasks. Each task must:
   - Target a single file or tightly coupled set of files.
   - Have a clear "done" definition (testable success criterion).
   - List prerequisite tasks (from this resolution or others).
3. Group tasks into rollout phases:
   - **Foundation:** Shared infrastructure, base class changes, new abstractions.
   - **Pilot:** Apply the pattern to one representative component/module as a reference implementation.
   - **Rollout:** Apply the pattern systematically to remaining components.
   - **Enforcement:** Add guardrails (analyzers, templates, docs, review checks) to prevent regression.
4. For each phase, define:
   - Entry criteria (what must be true before starting).
   - Exit criteria (what must be true to consider it complete).
   - Estimated scope (number of files/components).
5. Identify rollback strategy: what to do if a phase reveals problems.
6. Save the remediation plan to output.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| Step 3 | Phase breakdown with task counts per phase | Approve phases, adjust grouping |
| Step 5 | Rollback strategy | Approve or define alternative fallback |

## Audit

| Check | Pass Condition |
|-------|---------------|
| All resolutions covered | Every approved resolution has at least one task |
| Tasks are atomic | Each task targets a clear, bounded scope |
| Dependencies acyclic | No circular task dependencies |
| Success criteria testable | Every task's "done" definition can be verified objectively |
| Pilot identified | At least one component designated as pilot for each pattern |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Remediation plan | `output/gap-[slug]-remediation-plan.md` | Phased task list with dependencies and criteria |
