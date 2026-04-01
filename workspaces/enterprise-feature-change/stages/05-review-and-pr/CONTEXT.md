# Stage 05 - Review and PR

Prepare review artifacts and PR messaging from approved design and plan.

## Inputs

| Source                | File/Location                                                          | Section/Scope                                               | Why                                                 |
| --------------------- | ---------------------------------------------------------------------- | ----------------------------------------------------------- | --------------------------------------------------- |
| Previous stage output | ../04-implementation-plan/output/feature-[slug]-implementation-plan.md | Full file                                                   | Base PR narrative on approved execution plan        |
| Design reference      | ../03-design-spec/output/feature-[slug]-design-spec.md                 | Full file                                                   | Include contract and versioning decisions in review |
| Governance checks     | ../../../../docs/icm-defaults/040-governance/*.md                      | Contract testing, breaking changes, deprecation, versioning | Build required validation checklist                 |
| Release checks        | ../../../../docs/icm-defaults/060-release-channels/*.md                | Testing gates and channel promotion rules                   | Ensure rollout readiness criteria are present       |

## Process

1. Draft PR description with context, scope, and linked artifacts.
2. Generate checklist for contract tests, API diffs, performance checks, and channel gates.
3. List ADR updates or new ADR candidates needed for long-lived decisions.
4. Save outputs.

## Outputs

| Artifact         | Location                          | Format   |
| ---------------- | --------------------------------- | -------- |
| PR draft package | output/feature-[slug]-pr-draft.md | Markdown |
