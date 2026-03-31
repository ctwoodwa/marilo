# Stage 02 - Coverage Gap Analysis

Identify test coverage gaps by risk area and interface, prioritized by business impact.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Previous stage | ../01-test-inventory/output/test-[slug]-current-inventory.md | Full file | Baseline test inventory |
| Domain governance | ../../references/layer-3-defaults-index.md | Domain section | Identify high-value business logic to test |
| Architecture defaults | ../../references/layer-3-defaults-index.md | Architecture section | Service boundaries and integration points to cover |
| Contract testing | ../../references/layer-3-defaults-index.md | Interface Governance section (contract-testing) | Contract test requirements |
| Data architecture | ../../references/layer-3-defaults-index.md | Architecture section (data-architecture) | Data access paths to verify |

## Process

1. Map current coverage to architecture components: domain logic, API handlers, integrations, data access.
2. Identify domain logic with high business value and low/no test coverage.
3. Identify API and event interfaces lacking contract tests per governance defaults.
4. Identify integration points (external services, databases, message brokers) without integration tests.
5. Identify UI workflows without end-to-end coverage for critical paths.
6. Classify each gap by risk: `Critical`, `High`, `Medium`, `Low`.
7. Produce prioritized gap list ordered by risk and business impact.
8. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Coverage gap analysis | output/test-[slug]-coverage-gap-analysis.md | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 3 | Contract test gap summary | Confirm interface test expectations |
| 7 | Prioritized gap list | Approve risk classifications and priorities |

## Audit

| Check | Pass Condition |
|---|---|
| Architecture mapping | Gaps are mapped to specific components and layers |
| Domain coverage | High-value business logic gaps are identified |
| Contract gaps | Missing contract tests are listed per governance defaults |
| Risk classification | Each gap uses `Critical`, `High`, `Medium`, or `Low` |
| Output naming | Artifact matches `output/test-[slug]-coverage-gap-analysis.md` |
