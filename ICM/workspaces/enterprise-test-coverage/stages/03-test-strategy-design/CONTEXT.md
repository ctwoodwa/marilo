# Stage 03 - Test Strategy Design

Define the target test mix, coverage targets, and testing approach per architecture layer.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Previous stage | ../02-coverage-gap-analysis/output/test-[slug]-coverage-gap-analysis.md | Full file | Prioritized gaps to address |
| Interface governance | ../../references/layer-3-defaults-index.md | Interface Governance section | Contract testing requirements |
| Contract testing | ../../references/layer-3-defaults-index.md | Interface Governance section (contract-testing) | Specific contract test patterns |
| Signal/reactive defaults | ../../references/layer-3-defaults-index.md | Architecture section (signal-reactive-architecture) | Testing reactive/signal patterns |
| Release channels | ../../references/layer-3-defaults-index.md | Release Channels section | Channel-aware test gating |

## Process

1. Define target test pyramid: ratio of unit, integration, contract, and e2e tests.
2. Set coverage targets per architecture layer (domain: X%, handlers: X%, integrations: X%).
3. Design contract test approach: consumer-driven vs provider, tooling selection.
4. Design integration test approach: in-memory vs Testcontainers vs shared environment.
5. Design e2e test approach: critical path selection, browser/API testing, data management.
6. Define test gating per release channel (which tests must pass before channel promotion).
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Test strategy specification | output/test-[slug]-strategy.md | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 2 | Coverage targets per layer | Confirm realism and priority alignment |
| 6 | Channel test gating rules | Approve promotion criteria |

## Audit

| Check | Pass Condition |
|---|---|
| Test pyramid | Unit/integration/contract/e2e ratios are defined |
| Coverage targets | Numeric targets are set per architecture layer |
| Contract testing | Contract test approach follows governance defaults |
| Channel gating | Test pass criteria are defined per release channel |
| Output naming | Artifact matches `output/test-[slug]-strategy.md` |
