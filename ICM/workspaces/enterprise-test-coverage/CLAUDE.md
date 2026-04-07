# Enterprise Test Coverage Workspace

Brownfield workflow for systematically increasing and improving test coverage for an existing service.

## Folder Map

```
enterprise-test-coverage/
|- CLAUDE.md                      (you are here)
|- CONTEXT.md                     (workspace routing and rules)
|- references/
|  |- layer-3-defaults-index.md   (read-only links to docs/icm-defaults)
|- stages/
|  |- 01-test-inventory/          (capture current tests and coverage)
|  |- 02-coverage-gap-analysis/   (identify gaps by risk area and interface)
|  |- 03-test-strategy-design/    (define test mix and target coverage)
|  |- 04-test-implementation-plan/(plan test tasks per team and phase)
|  |- 05-verification-and-reporting/ (verify coverage and report results)
|- output/                        (workspace-level consolidated artifacts)
```

## Task Routing

| Task Type | Go To |
|---|---|
| Inventory current tests and coverage | `stages/01-test-inventory/CONTEXT.md` |
| Identify coverage gaps | `stages/02-coverage-gap-analysis/CONTEXT.md` |
| Design test strategy and targets | `stages/03-test-strategy-design/CONTEXT.md` |
| Plan test implementation tasks | `stages/04-test-implementation-plan/CONTEXT.md` |
| Verify and report coverage results | `stages/05-verification-and-reporting/CONTEXT.md` |

## Triggers

| Keyword | Action |
|---|---|
| `setup` | Confirm service scope and target slug, then start Stage 01 |
| `status` | Scan `stages/*/output/` and summarize progress by stage |

## Global Constraints

- Treat all references in `references/layer-3-defaults-index.md` as canonical and read-only.
- This workspace does not modify product code; it produces analysis, strategy, and planning artifacts.
- Stage outputs use the naming convention `test-[slug]-*.md`.
- Run stages in order from 01 to 05 unless a stage explicitly states a valid shortcut.
