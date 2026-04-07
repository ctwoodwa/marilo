# Stage 05 - Review and Validation

Produce a PR-ready checklist and validation report for the API/event change.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Previous stage | ../04-rollout-plan/output/api-[slug]-rollout-plan.md | Full file | Rollout plan to validate against |
| Contract testing | ../../references/layer-3-defaults-index.md | Interface Governance section (contract-testing) | Test requirements checklist |
| Breaking change detection | ../../references/layer-3-defaults-index.md | Interface Governance section (breaking-change-detection) | Detection requirements checklist |
| Observability defaults | ../../references/layer-3-defaults-index.md | Architecture section (observability) | Monitoring requirements for new contracts |

## Process

1. Compile PR review checklist covering: contract tests, OpenAPI/schema diffs, headers, versioning, deprecation notices.
2. Verify that breaking-change detection checks are in place.
3. Define monitoring and alerting additions for new or changed contracts.
4. List documentation updates required (API docs, consumer guides, ADRs).
5. Produce final validation summary with pass/fail per checklist item.
6. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Review checklist and validation report | output/api-[slug]-review-checklist.md | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 1 | Draft PR checklist | Confirm completeness of review criteria |
| 5 | Final validation summary | Approve for PR submission |

## Audit

| Check | Pass Condition |
|---|---|
| Checklist completeness | Covers contract tests, schema diffs, versioning, deprecation, docs |
| Detection validation | Breaking-change detection checks are confirmed in place |
| Monitoring plan | New contracts have defined alerting and dashboard additions |
| Documentation | Required doc updates are listed with ownership |
| Output naming | Artifact matches `output/api-[slug]-review-checklist.md` |
