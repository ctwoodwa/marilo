# Enterprise Observability Enhancement - Workspace Context

Task routing for improving observability, error handling, and telemetry in an existing service.

## Task Routing

| Task Type | Go To Stage | Description |
|---|---|---|
| Inventory current telemetry | `stages/01-telemetry-inventory/` | Capture current logging, tracing, metrics, and dashboards |
| Compare to observability standards | `stages/02-standards-gap-analysis/` | Identify where telemetry diverges from defaults |
| Design target observability model | `stages/03-observability-design/` | Define target traces, metrics, logs, and error shapes |
| Plan instrumentation tasks | `stages/04-instrumentation-plan/` | Create actionable tasks with ownership and rollout |
| Verify and tune instrumentation | `stages/05-verification-and-tuning/` | Validate instrumentation and tune thresholds |

## Reference Materials

| Resource | Location | Contains |
|---|---|---|
| Canonical Layer-3 defaults | `references/layer-3-defaults-index.md` | Read-only links to `docs/icm-defaults` guidance |
| Stage outputs | `stages/*/output/` | Observability analysis and planning artifacts |
| Workspace-level summaries | `output/` | Optional consolidated artifacts across services |

## Rules

1. Treat every document linked from `references/layer-3-defaults-index.md` as canonical and read-only.
2. Run stages in order from 01 to 05 and carry outputs forward as listed in each stage input table.
3. Use `obs-[slug]-*.md` output names, where `slug` is a stable service or project identifier.
4. Keep findings evidence-based by citing concrete code paths, log samples, or dashboard links where possible.
5. Observability changes must align with error handling (ADR-0019) and JSON serialization (ADR-0017) standards.
