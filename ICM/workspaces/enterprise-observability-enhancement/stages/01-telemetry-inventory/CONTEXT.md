# Stage 01 - Telemetry Inventory

Capture the current state of logging, tracing, metrics, and dashboards for the target service.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Service code | Target project source code | Logging, tracing, metrics instrumentation | Baseline of current telemetry |
| Current dashboards | Monitoring platform (Grafana, App Insights, etc.) | Existing dashboards and alerts | Understand what is already observable |
| Observability defaults | ../../references/layer-3-defaults-index.md | Architecture section (observability) | Know what to inventory against |

## Process

1. Scan service code for logging frameworks, trace instrumentation, and metrics collection.
2. Inventory existing dashboards, alerts, and health check endpoints.
3. Document log formats, structured vs unstructured, and current log levels.
4. Document trace propagation: which calls are traced, correlation IDs, span naming.
5. Document metrics: which counters, histograms, gauges exist and where they report.
6. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Current telemetry inventory | output/obs-[slug]-current-telemetry.md | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 2 | Dashboard and alert inventory | Confirm completeness of monitoring landscape |
| 5 | Full telemetry summary | Approve as accurate baseline |

## Audit

| Check | Pass Condition |
|---|---|
| Coverage | Inventory covers logging, tracing, metrics, dashboards, and alerts |
| Evidence quality | Findings reference concrete code paths, config files, or dashboard links |
| Format documentation | Log format (structured/unstructured) is explicitly noted |
| Output naming | Artifact matches `output/obs-[slug]-current-telemetry.md` |
