# Enterprise Test Coverage - Workspace Context

Task routing for systematically increasing and improving test coverage for an existing service.

## Task Routing

| Task Type | Go To Stage | Description |
|---|---|---|
| Inventory current tests and coverage | `stages/01-test-inventory/` | Capture tests by type and areas covered |
| Identify coverage gaps | `stages/02-coverage-gap-analysis/` | Find gaps by risk area and interface |
| Design test strategy and targets | `stages/03-test-strategy-design/` | Define unit/integration/contract/e2e mix and targets |
| Plan test implementation tasks | `stages/04-test-implementation-plan/` | Create tasks per team and phase |
| Verify and report coverage results | `stages/05-verification-and-reporting/` | Produce before/after coverage report |

## Reference Materials

| Resource | Location | Contains |
|---|---|---|
| Canonical Layer-3 defaults | `references/layer-3-defaults-index.md` | Read-only links to `docs/icm-defaults` guidance |
| Stage outputs | `stages/*/output/` | Test coverage analysis and planning artifacts |
| Workspace-level summaries | `output/` | Optional consolidated artifacts across services |

## Rules

1. Treat every document linked from `references/layer-3-defaults-index.md` as canonical and read-only.
2. Run stages in order from 01 to 05 and carry outputs forward as listed in each stage input table.
3. Use `test-[slug]-*.md` output names, where `slug` is a stable service or project identifier.
4. Keep findings evidence-based by citing concrete test files, coverage reports, and code paths.
5. Test strategy must align with contract testing (090c) and interface governance defaults.
