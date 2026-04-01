# Stage 05 - Rollout Checklist

Track standards adoption progress and maintain evidence that the project is converging on enterprise standard.

## Inputs

| Source                      | File/Location                                      | Section/Scope      | Why                               |
| --------------------------- | -------------------------------------------------- | ------------------ | --------------------------------- |
| Previous stage              | ../04-migration-plan/output/[slug]-upgrade-plan.md | Full file          | Planned tasks and phases to track |
| Optional execution evidence | Project board, CI status, ADRs, PR links           | Relevant snapshots | Validate status and outcomes      |

## Process

1. Convert migration plan tasks into checklist rows by standards area.
2. Assign each item a status: `Not started`, `In progress`, `Adopted`, or `Not applicable`.
3. Attach evidence links for each item where available (PRs, ADRs, issues, dashboards).
4. Highlight blockers, risks, and aging items needing intervention.
5. Save outputs.

## Outputs

| Artifact                 | Location                        | Format   |
| ------------------------ | ------------------------------- | -------- |
| Upgrade execution status | output/[slug]-upgrade-status.md | Markdown |

## Checkpoints

| After Step | Agent Presents                       | Human Decides                       |
| ---------- | ------------------------------------ | ----------------------------------- |
| 2          | Initial checklist with status values | Confirm status rubric and ownership |
| 4          | Blockers and risk summary            | Confirm escalation actions          |

## Audit

| Check                  | Pass Condition                                                       |
| ---------------------- | -------------------------------------------------------------------- |
| Status completeness    | All tracked standards areas have a current status                    |
| Evidence traceability  | Adopted and In progress items include evidence links where available |
| Operational visibility | Blockers and risk items are explicit with suggested action           |
| Output naming          | Artifact matches `output/[slug]-upgrade-status.md`                   |
