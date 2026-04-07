# Stage 04 - Implementation Plan

Turn approved design into owned tasks and controlled rollout steps.

## Inputs

| Source                    | File/Location                                                           | Section/Scope                                                | Why                                          |
| ------------------------- | ----------------------------------------------------------------------- | ------------------------------------------------------------ | -------------------------------------------- |
| Previous stage output     | ../03-design-spec/output/feature-[slug]-design-spec.md                  | Full file                                                    | Convert design decisions into execution work |
| Team ownership            | ../../../../docs/icm-defaults/050-operating-model/team-ownership.md     | Full file                                                    | Assign implementation ownership              |
| Migration strategy        | ../../../../docs/icm-defaults/050-operating-model/migration-strategy.md | Full file                                                    | Plan data and behavior migration safely      |
| Release channels guidance | ../../../../docs/icm-defaults/060-release-channels/*.md                 | Channel concept, progression, migration, publishing, testing | Define rollout and rollback path             |

## Process

1. Break design into implementation tasks by service and repository.
2. Assign owners and dependencies for each task.
3. Define rollout path across channels and success criteria.
4. Define feature flags, migration steps, rollback plan, and fallbacks.
5. Save outputs.

## Outputs

| Artifact            | Location                                     | Format   |
| ------------------- | -------------------------------------------- | -------- |
| Implementation plan | output/feature-[slug]-implementation-plan.md | Markdown |
