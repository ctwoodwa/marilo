# Stage 02 - Standards Gap Analysis

Compare current telemetry against observability, error handling, and JSON serialization standards.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Previous stage | ../01-telemetry-inventory/output/obs-[slug]-current-telemetry.md | Full file | Baseline of current telemetry |
| Observability defaults | ../../references/layer-3-defaults-index.md | Architecture section (observability) | Target observability standard |
| Error handling ADR | ADR-0019 (error handling and problem details) | Full ADR | Target error shape and problem details standard |
| JSON serialization ADR | ADR-0017 (JSON serialization standards) | Full ADR | Target JSON options and formatting |
| Enterprise context | ../../references/layer-3-defaults-index.md | Context section | Cross-cutting observability principles |

## Process

1. Compare current logging against observability defaults: structured format, required fields, log levels.
2. Compare current tracing against defaults: span naming, propagation, sampling, correlation.
3. Compare current metrics against defaults: naming conventions, required counters/histograms.
4. Compare error responses against ADR-0019: problem details shape, error codes, status mapping.
5. Compare JSON serialization against ADR-0017: casing, date formats, null handling.
6. Classify each area as `Aligned`, `Partial`, `Not`, or `NA`.
7. Prioritize top gaps by risk and impact.
8. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Standards gap analysis | output/obs-[slug]-standards-gap-analysis.md | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 3 | Telemetry alignment matrix | Confirm interpretation of observability standard |
| 7 | Prioritized gap list | Approve priorities for design stage |

## Audit

| Check | Pass Condition |
|---|---|
| Required structure | Output contains per-area tables and prioritized gap list |
| Table fields | Each row includes Area, Current State, Enterprise Standard, Status, Notes |
| Status fidelity | Each area uses only `Aligned`, `Partial`, `Not`, or `NA` |
| ADR coverage | Error handling (ADR-0019) and JSON (ADR-0017) are explicitly assessed |
| Output naming | Artifact matches `output/obs-[slug]-standards-gap-analysis.md` |
