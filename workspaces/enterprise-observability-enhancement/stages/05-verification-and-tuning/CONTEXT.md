# Stage 05 - Verification and Tuning

Verify that instrumentation meets the observability design and tune thresholds and sampling.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Previous stage | ../04-instrumentation-plan/output/obs-[slug]-instrumentation-plan.md | Full file | Planned tasks to verify completion of |
| Observability design | ../03-observability-design/output/obs-[slug]-observability-design.md | Full file | Target specifications to validate against |
| Updated telemetry | Live service dashboards, log queries, trace views | Current state after instrumentation | Evidence of implementation |

## Process

1. Verify structured logging: format compliance, required fields present, correlation IDs propagated.
2. Verify distributed tracing: spans named per convention, propagation working, sampling as designed.
3. Verify metrics: counters and histograms reporting, naming matches conventions.
4. Verify error responses: problem details shape matches ADR-0019.
5. Verify dashboards and alerts: created per specification, thresholds sensible.
6. Tune sampling rates, alert thresholds, and log levels based on observed traffic.
7. Document remaining gaps or deferred items.
8. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Verification report | output/obs-[slug]-verification-report.md | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 5 | Verification checklist results | Confirm pass/fail per area |
| 6 | Tuning recommendations | Approve threshold adjustments |

## Audit

| Check | Pass Condition |
|---|---|
| Verification completeness | All observability design areas have a pass/fail verification |
| Evidence | Verification findings include dashboard screenshots, log samples, or trace examples |
| Tuning documented | Sampling, threshold, and log-level adjustments are recorded |
| Remaining gaps | Deferred items are explicitly listed with rationale |
| Output naming | Artifact matches `output/obs-[slug]-verification-report.md` |
