# Enterprise API Change Workspace

Brownfield workflow for safely changing APIs and events under interface governance and release channel controls.

## Folder Map

```
enterprise-api-change/
|- CLAUDE.md                      (you are here)
|- CONTEXT.md                     (workspace routing and rules)
|- references/
|  |- layer-3-defaults-index.md   (read-only links to docs/icm-defaults)
|- stages/
|  |- 01-change-request-analysis/ (understand current API/event state and change goal)
|  |- 02-contract-design/         (design new/changed contracts under governance)
|  |- 03-implementation-impact/   (assess services, tests, and registry impact)
|  |- 04-rollout-plan/            (plan phased channel rollout and deprecation)
|  |- 05-review-and-validation/   (produce PR checklist and validation report)
|- output/                        (workspace-level consolidated artifacts)
```

## Task Routing

| Task Type | Go To |
|---|---|
| Understand current API/event state and change goal | `stages/01-change-request-analysis/CONTEXT.md` |
| Design contract changes under governance | `stages/02-contract-design/CONTEXT.md` |
| Assess implementation and testing impact | `stages/03-implementation-impact/CONTEXT.md` |
| Plan channel-based rollout and deprecation | `stages/04-rollout-plan/CONTEXT.md` |
| Produce review checklist and validation | `stages/05-review-and-validation/CONTEXT.md` |

## Triggers

| Keyword | Action |
|---|---|
| `setup` | Confirm API/event scope and target slug, then start Stage 01 |
| `status` | Scan `stages/*/output/` and summarize progress by stage |

## Global Constraints

- Treat all references in `references/layer-3-defaults-index.md` as canonical and read-only.
- This workspace does not modify product code; it produces analysis, design, and planning artifacts.
- Stage outputs use the naming convention `api-[slug]-*.md`.
- Run stages in order from 01 to 05 unless a stage explicitly states a valid shortcut.
