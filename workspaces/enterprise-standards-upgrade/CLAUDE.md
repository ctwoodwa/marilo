# Enterprise Standards Upgrade Workspace

Brownfield workflow for assessing an existing repository against enterprise standards and producing a practical upgrade path.

## Folder Map

```
enterprise-standards-upgrade/
|- CLAUDE.md                      (you are here)
|- CONTEXT.md                     (workspace routing and rules)
|- references/
|  |- layer-3-defaults-index.md   (read-only links to docs/icm-defaults)
|- stages/
|  |- 01-audit-current/           (capture current architecture and practices)
|  |- 02-gap-analysis/            (compare to standards and identify gaps)
|  |- 03-evolution-design/        (decide what to adopt and how)
|  |- 04-migration-plan/          (build phased upgrade plan)
|  |- 05-rollout-checklist/       (track execution and adoption status)
|- output/                        (workspace-level consolidated artifacts)
```

## Task Routing

| Task Type                          | Go To                                    |
| ---------------------------------- | ---------------------------------------- |
| Capture current repo state         | `stages/01-audit-current/CONTEXT.md`     |
| Compare with enterprise defaults   | `stages/02-gap-analysis/CONTEXT.md`      |
| Decide adoption scope and strategy | `stages/03-evolution-design/CONTEXT.md`  |
| Build implementation rollout plan  | `stages/04-migration-plan/CONTEXT.md`    |
| Track execution and readiness      | `stages/05-rollout-checklist/CONTEXT.md` |

## Triggers

| Keyword  | Action                                                     |
| -------- | ---------------------------------------------------------- |
| `setup`  | Confirm repo metadata and target slug, then start Stage 01 |
| `status` | Scan `stages/*/output/` and summarize progress by stage    |

## Global Constraints

- Treat all references in `references/layer-3-defaults-index.md` as canonical and read-only.
- This workspace does not scaffold or modify product code; it produces analysis and planning artifacts.
- Stage outputs use the naming convention `[slug]-*.md`.
- Run stages in order from 01 to 05 unless a stage explicitly states a valid shortcut.
