# Stage 04 - Rollout Plan

Plan phased rollout of API/event changes across release channels with migration and deprecation schedules.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Previous stage | ../03-implementation-impact/output/api-[slug]-implementation-impact.md | Full file | Services and changes to roll out |
| Release channels | ../../references/layer-3-defaults-index.md | Release Channels section | Channel progression and governance |
| Channel progression | ../../references/layer-3-defaults-index.md | Release Channels section (channel-progression) | Validation gates between channels |
| Channel migration | ../../references/layer-3-defaults-index.md | Release Channels section (channel-migration) | Migration patterns across channels |
| Code publishing | ../../references/layer-3-defaults-index.md | Release Channels section (code-publishing-through-channels) | Publishing workflow constraints |

## Process

1. Define rollout phases aligned to release channel progression (dev, staging, canary, production).
2. For each phase, list which services deploy, which contracts activate, and validation criteria.
3. Define consumer migration window and communication plan for breaking changes.
4. Define deprecation schedule for retired contracts with sunset dates per governance.
5. Define rollback strategy per phase in case of contract failures.
6. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Rollout plan | output/api-[slug]-rollout-plan.md | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 2 | Per-phase rollout table | Confirm phase sequencing and validation gates |
| 4 | Deprecation schedule | Approve sunset dates and consumer notification |

## Audit

| Check | Pass Condition |
|---|---|
| Phase completeness | All release channels have explicit deployment and validation steps |
| Consumer migration | Breaking changes include migration window and communication plan |
| Deprecation schedule | Retired contracts have explicit sunset dates |
| Rollback strategy | Each phase has a defined rollback approach |
| Output naming | Artifact matches `output/api-[slug]-rollout-plan.md` |
