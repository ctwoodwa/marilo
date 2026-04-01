# Enterprise API Change - Workspace Context

Task routing for safely changing APIs and events in an existing enterprise project.

## Task Routing

| Task Type | Go To Stage | Description |
|---|---|---|
| Understand current API/event state and change goal | `stages/01-change-request-analysis/` | Capture current contracts and desired outcome |
| Design contract changes under governance | `stages/02-contract-design/` | Define new/changed endpoints, events, versioning |
| Assess implementation and testing impact | `stages/03-implementation-impact/` | Map affected services, tests, and registry entries |
| Plan channel-based rollout and deprecation | `stages/04-rollout-plan/` | Define phased rollout across channels |
| Produce review checklist and validation | `stages/05-review-and-validation/` | Generate PR checklist and validation report |

## Reference Materials

| Resource | Location | Contains |
|---|---|---|
| Canonical Layer-3 defaults | `references/layer-3-defaults-index.md` | Read-only links to `docs/icm-defaults` guidance |
| Stage outputs | `stages/*/output/` | API change analysis and planning artifacts |
| Workspace-level summaries | `output/` | Optional consolidated artifacts across changes |

## Rules

1. Treat every document linked from `references/layer-3-defaults-index.md` as canonical and read-only.
2. Run stages in order from 01 to 05 and carry outputs forward as listed in each stage input table.
3. Use `api-[slug]-*.md` output names, where `slug` is a stable identifier for the change (ticket ID or API name).
4. All contract changes must be classified as breaking or non-breaking per interface governance defaults.
5. Deprecation of existing contracts must follow the deprecation process defined in governance defaults.
