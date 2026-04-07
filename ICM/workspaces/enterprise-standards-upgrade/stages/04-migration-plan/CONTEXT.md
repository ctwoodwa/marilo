# Stage 04 - Migration Plan

Translate upgrade design decisions into a phased, owner-ready implementation plan.

## Inputs

| Source                   | File/Location                                          | Section/Scope            | Why                                            |
| ------------------------ | ------------------------------------------------------ | ------------------------ | ---------------------------------------------- |
| Previous stage           | ../03-evolution-design/output/[slug]-upgrade-design.md | Full file                | Design decisions to operationalize             |
| Operating model defaults | ../../references/layer-3-defaults-index.md             | Operating Model section  | Ownership and migration pattern guidance       |
| Release channel defaults | ../../references/layer-3-defaults-index.md             | Release Channels section | Channel-based rollout constraints and ordering |

## Process

1. Break each approved standard into concrete tasks grouped by standard area.
2. Sequence tasks into phases with prerequisites, dependencies, and risk notes.
3. Assign recommended ownership model for each task group.
4. Define release-channel rollout strategy for validation and production progression.
5. List project ADRs that must be written or updated to govern the change.
6. Save outputs.

## Outputs

| Artifact                    | Location                      | Format   |
| --------------------------- | ----------------------------- | -------- |
| Upgrade implementation plan | output/[slug]-upgrade-plan.md | Markdown |

## Checkpoints

| After Step | Agent Presents            | Human Decides                              |
| ---------- | ------------------------- | ------------------------------------------ |
| 2          | Draft phased task backlog | Confirm scope and sequencing realism       |
| 4          | Channel rollout plan      | Confirm rollout/rollback expectations      |
| 5          | Required ADR list         | Confirm governance ownership and deadlines |

## Audit

| Check                | Pass Condition                                                |
| -------------------- | ------------------------------------------------------------- |
| Task granularity     | Plan contains actionable tasks grouped by standard            |
| Rollout clarity      | Plan includes phased channel progression and validation steps |
| Governance readiness | Required ADRs are identified with purpose and ownership       |
| Output naming        | Artifact matches `output/[slug]-upgrade-plan.md`              |
