# Stage 04 - Test Implementation Plan

Translate the test strategy into actionable tasks with team ownership and phased rollout.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Previous stage | ../03-test-strategy-design/output/test-[slug]-strategy.md | Full file | Strategy decisions to operationalize |
| Team ownership | ../../references/layer-3-defaults-index.md | Operating Model section (team-ownership) | Assign tasks to appropriate teams |
| Migration strategy | ../../references/layer-3-defaults-index.md | Operating Model section (migration-strategy) | Incremental rollout patterns |

## Process

1. Break strategy into concrete test authoring tasks grouped by test type and target component.
2. Define test infrastructure tasks: new test projects, fixture libraries, CI pipeline updates.
3. Sequence tasks into phases: Phase 1 (critical gaps), Phase 2 (high gaps), Phase 3 (medium/low).
4. Assign recommended ownership per task group based on team-ownership model.
5. Define CI pipeline integration: where new tests run, gating thresholds, reporting.
6. Estimate coverage improvement per phase.
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Test implementation plan | output/test-[slug]-implementation-plan.md | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 3 | Phased task backlog | Confirm scope and sequencing |
| 5 | CI pipeline integration plan | Approve gating thresholds and reporting |

## Audit

| Check | Pass Condition |
|---|---|
| Task granularity | Tasks are specific enough to assign and estimate |
| Infrastructure | Test project and CI changes are explicitly defined |
| Phase sequencing | Critical gaps are addressed in Phase 1 |
| Ownership | Tasks have recommended team ownership |
| Output naming | Artifact matches `output/test-[slug]-implementation-plan.md` |
