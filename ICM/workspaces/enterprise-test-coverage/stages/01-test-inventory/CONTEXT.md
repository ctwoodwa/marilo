# Stage 01 - Test Inventory

Capture current tests and coverage for the target service.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Current tests | Target project test projects and test files | All test types | Baseline of existing coverage |
| Coverage reports | Target project CI output or local coverage tool | Coverage percentages by assembly/namespace | Quantify current state |
| Enterprise context | ../../references/layer-3-defaults-index.md | Context section | Anchor inventory in enterprise principles |

## Process

1. Scan test projects and classify tests by type: unit, integration, contract, end-to-end, performance.
2. List test frameworks and tools in use (xUnit, NUnit, Moq, Testcontainers, Playwright, etc.).
3. Run or collect latest coverage report and summarize coverage by assembly or namespace.
4. Identify areas with zero or minimal coverage.
5. Document test infrastructure: CI pipelines, test environments, data fixtures.
6. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Current test inventory | output/test-[slug]-current-inventory.md | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 3 | Coverage summary table | Confirm accuracy and coverage tool |
| 5 | Test infrastructure summary | Confirm completeness |

## Audit

| Check | Pass Condition |
|---|---|
| Test type classification | Tests are categorized by type (unit, integration, contract, e2e) |
| Coverage data | Quantitative coverage data is present by module/namespace |
| Zero-coverage areas | Areas with no coverage are explicitly listed |
| Infrastructure | Test frameworks, CI, and fixture approach are documented |
| Output naming | Artifact matches `output/test-[slug]-current-inventory.md` |
