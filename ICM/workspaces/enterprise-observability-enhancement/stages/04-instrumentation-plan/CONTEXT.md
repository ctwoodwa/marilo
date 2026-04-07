# Stage 04 - Instrumentation Plan

Translate the observability design into actionable implementation tasks with ownership and rollout.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Previous stage | ../03-observability-design/output/obs-[slug]-observability-design.md | Full file | Design decisions to operationalize |
| Team ownership | ../../references/layer-3-defaults-index.md | Operating Model section (team-ownership) | Assign tasks to appropriate teams |
| Release channels | ../../references/layer-3-defaults-index.md | Release Channels section | Channel-based rollout for instrumentation |

## Process

1. Break observability design into concrete tasks grouped by area (logging, tracing, metrics, errors, dashboards).
2. Identify code areas requiring changes: middleware, handlers, clients, configuration.
3. Define configuration changes: telemetry providers, exporters, sampling config.
4. Define dashboard and alert creation tasks with specifications.
5. Assign recommended ownership per task group.
6. Sequence tasks into phases aligned to release channels.
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Instrumentation plan | output/obs-[slug]-instrumentation-plan.md | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 2 | Per-area task inventory | Confirm scope and granularity |
| 6 | Phased rollout plan | Approve sequencing and ownership |

## Audit

| Check | Pass Condition |
|---|---|
| Task granularity | Tasks are actionable and grouped by observability area |
| Code targeting | Specific code areas and files are identified for changes |
| Configuration | Telemetry provider and exporter config changes are defined |
| Rollout clarity | Tasks are sequenced into phases aligned to release channels |
| Output naming | Artifact matches `output/obs-[slug]-instrumentation-plan.md` |
