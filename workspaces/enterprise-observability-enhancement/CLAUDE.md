# Enterprise Observability Enhancement Workspace

Brownfield workflow for bringing an existing service up to enterprise observability, error handling, and JSON standards.

## Folder Map

```
enterprise-observability-enhancement/
|- CLAUDE.md                      (you are here)
|- CONTEXT.md                     (workspace routing and rules)
|- references/
|  |- layer-3-defaults-index.md   (read-only links to docs/icm-defaults)
|- stages/
|  |- 01-telemetry-inventory/     (capture current logging, tracing, metrics)
|  |- 02-standards-gap-analysis/  (compare telemetry to observability defaults)
|  |- 03-observability-design/    (design target traces, metrics, logs, error shapes)
|  |- 04-instrumentation-plan/    (plan implementation tasks and rollout)
|  |- 05-verification-and-tuning/ (verify instrumentation and tune thresholds)
|- output/                        (workspace-level consolidated artifacts)
```

## Task Routing

| Task Type | Go To |
|---|---|
| Inventory current telemetry | `stages/01-telemetry-inventory/CONTEXT.md` |
| Compare to observability standards | `stages/02-standards-gap-analysis/CONTEXT.md` |
| Design target observability model | `stages/03-observability-design/CONTEXT.md` |
| Plan instrumentation tasks | `stages/04-instrumentation-plan/CONTEXT.md` |
| Verify and tune instrumentation | `stages/05-verification-and-tuning/CONTEXT.md` |

## Triggers

| Keyword | Action |
|---|---|
| `setup` | Confirm service scope and target slug, then start Stage 01 |
| `status` | Scan `stages/*/output/` and summarize progress by stage |

## Global Constraints

- Treat all references in `references/layer-3-defaults-index.md` as canonical and read-only.
- This workspace does not modify product code; it produces analysis, design, and planning artifacts.
- Stage outputs use the naming convention `obs-[slug]-*.md`.
- Run stages in order from 01 to 05 unless a stage explicitly states a valid shortcut.
