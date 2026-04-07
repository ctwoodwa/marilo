# Test Coverage Expansion — Workspace Context

Task routing and rules for auditing, designing, and scaffolding test coverage.

## Task Routing

| Task Type | Go To Stage | Description |
|---|---|---|
| Map tested vs untested surface | `stages/01-coverage-audit/` | Produces risk-scored inventory of all testable units |
| Define the test layer strategy | `stages/02-test-strategy/` | Assigns each testable unit to unit / integration / component / e2e |
| Generate unit test scaffolds | `stages/03-unit-tests/` | Domain logic, solvers, services — no infrastructure |
| Generate Blazor component test scaffolds | `stages/04-component-tests/` | bunit tests for pages and shared components |
| Set coverage gates and CI hooks | `stages/05-coverage-gates/` | Thresholds per layer, coverage report integration, CI gate |

## Reference Materials

| Resource | Location | Contains |
|---|---|---|
| Coverage config | `_config/coverage-context.md` | Project paths, test framework, coverage targets |
| Coverage audit results | `_config/coverage-audit.md` | Filled in Stage 01 — current state |
| Agreed test strategy | `_config/test-strategy.md` | Filled in Stage 02 — layer assignments |
| Test patterns | `shared/test-patterns.md` | NUnit, bunit, Aspire.Hosting.Testing examples |
| Risk scoring guide | `shared/risk-matrix.md` | How to prioritize untested surface |
| Stage outputs | `stages/*/output/` | Artifacts produced at each stage |

## Rules

1. Run stages 01 → 05 in order. The audit and strategy must be agreed before any scaffolds are generated.
2. Unit tests have zero infrastructure dependencies — no database, no HTTP, no file system. Use value objects, fakes, or pure functions only.
3. Integration tests target real infrastructure via Aspire.Hosting.Testing — no mocking at the boundary.
4. Blazor component tests use bunit only — no Playwright or Selenium in this workspace.
5. Scaffolds provide structure and one representative passing test per class — they do not generate exhaustive cases.
6. Coverage targets are defined per layer, not as a single percentage. A low unit-coverage number is acceptable if integration tests cover the same risk.
7. Use `[slug]-*.md` naming for all stage output artifacts.
