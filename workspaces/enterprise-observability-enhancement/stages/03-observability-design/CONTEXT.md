# Stage 03 - Observability Design

Define the target observability model: traces, metrics, logs, error envelopes, and JSON profile.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Previous stage | ../02-standards-gap-analysis/output/obs-[slug]-standards-gap-analysis.md | Full file | Prioritized gaps to address |
| Enterprise context | ../../references/layer-3-defaults-index.md | Context section | Cross-cutting principles |
| Logical architecture | ../../references/layer-3-defaults-index.md | Architecture section (logical-architecture) | Service boundaries and component map |
| Observability defaults | ../../references/layer-3-defaults-index.md | Architecture section (observability) | Target standard to design toward |

## Process

1. Review prioritized gaps and select which to address in this enhancement.
2. Design target structured logging: format, required fields, correlation, log levels per scenario.
3. Design target distributed tracing: span naming, propagation headers, sampling strategy.
4. Design target metrics: counters, histograms, gauges, naming conventions, and labels.
5. Design error response shape per ADR-0019 (problem details, error codes, status mapping).
6. Design JSON serialization profile per ADR-0017 where it affects telemetry payloads.
7. Define dashboard and alert specifications for the target observability model.
8. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Observability design specification | output/obs-[slug]-observability-design.md | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 4 | Target tracing and metrics design | Confirm feasibility and naming conventions |
| 7 | Dashboard and alert specifications | Approve monitoring additions |

## Audit

| Check | Pass Condition |
|---|---|
| Gap coverage | All prioritized gaps from Stage 02 have a design response |
| Logging design | Structured logging format and required fields are specified |
| Tracing design | Span naming, propagation, and sampling are defined |
| Metrics design | Required counters, histograms, and naming conventions are defined |
| Error shape | Error responses follow ADR-0019 problem details standard |
| Output naming | Artifact matches `output/obs-[slug]-observability-design.md` |
