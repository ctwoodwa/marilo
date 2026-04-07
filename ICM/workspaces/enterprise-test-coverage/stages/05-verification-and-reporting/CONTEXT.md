# Stage 05 - Verification and Reporting

Verify coverage improvements and produce a before/after report with remaining gaps.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Previous stage | ../04-test-implementation-plan/output/test-[slug]-implementation-plan.md | Full file | Planned tasks to verify completion of |
| Test strategy | ../03-test-strategy-design/output/test-[slug]-strategy.md | Coverage targets | Targets to measure against |
| New coverage reports | Target project CI output or local coverage tool | Updated coverage data | Evidence of improvement |

## Process

1. Collect updated coverage reports after test implementation.
2. Compare before vs after coverage by assembly/namespace against strategy targets.
3. Verify contract tests are in place per governance requirements.
4. Verify CI pipeline gating is active and thresholds are enforced.
5. Identify remaining gaps and classify as deferred, accepted risk, or next phase.
6. Produce final coverage summary with recommendations.
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Coverage verification report | output/test-[slug]-coverage-report.md | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 2 | Before/after coverage comparison | Confirm improvement meets expectations |
| 5 | Remaining gap classification | Approve deferred items and accepted risks |

## Audit

| Check | Pass Condition |
|---|---|
| Before/after data | Quantitative coverage comparison is present |
| Target comparison | Results are measured against strategy targets |
| Contract verification | Contract test presence is confirmed per governance |
| CI verification | Pipeline gating is active with correct thresholds |
| Remaining gaps | Deferred items are explicitly listed with rationale |
| Output naming | Artifact matches `output/test-[slug]-coverage-report.md` |
