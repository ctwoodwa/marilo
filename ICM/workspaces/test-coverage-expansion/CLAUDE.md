# Test Coverage Expansion Workspace

Audit existing test coverage, design a layered test strategy, and generate test scaffolds
to bring a .NET solution to a defined coverage target. Covers unit tests, Blazor component
tests (bunit), API integration tests, and end-to-end pipeline tests via Aspire.Hosting.Testing.

## Folder Map

```
test-coverage-expansion/
├── CLAUDE.md                          (you are here)
├── CONTEXT.md                         (workspace routing and rules)
├── _config/
│   ├── coverage-context.md            (project paths, test framework, coverage targets)
│   ├── coverage-audit.md              (filled in Stage 01 — current state inventory)
│   └── test-strategy.md               (filled in Stage 02 — agreed test layer assignments)
├── stages/
│   ├── 01-coverage-audit/             (map tested vs untested surface, score by risk)
│   ├── 02-test-strategy/              (define unit/integration/component/e2e split)
│   ├── 03-unit-tests/                 (scaffolds for domain/solver/service unit tests)
│   ├── 04-component-tests/            (bunit scaffolds for Blazor pages and components)
│   └── 05-coverage-gates/             (thresholds, CI integration, definition of done)
├── shared/
│   ├── test-patterns.md               (NUnit, bunit, and Aspire.Hosting.Testing patterns)
│   └── risk-matrix.md                 (how to score untested surface by risk level)
├── setup/
│   └── questionnaire.md               (onboarding questions)
└── output/                            (workspace-level summaries)
```

## Task Routing

| Task Type | Go To |
|---|---|
| First-time setup | `setup/questionnaire.md` |
| Audit current test coverage | `stages/01-coverage-audit/CONTEXT.md` |
| Design the test strategy | `stages/02-test-strategy/CONTEXT.md` |
| Generate unit test scaffolds | `stages/03-unit-tests/CONTEXT.md` |
| Generate Blazor component tests | `stages/04-component-tests/CONTEXT.md` |
| Set coverage gates and CI integration | `stages/05-coverage-gates/CONTEXT.md` |

## Triggers

| Keyword | Action |
|---|---|
| `setup` | Read `setup/questionnaire.md` and populate `_config/` |
| `status` | Scan `stages/*/output/` and show pipeline completion |
| `audit` | Jump to Stage 01 immediately |
| `scaffold` | Jump to Stage 03; requires Stage 02 output or filled `_config/test-strategy.md` |

## Global Constraints

- Scaffolds contain the structure and a representative example test — they do not generate exhaustive test cases.
- Unit tests must not take infrastructure dependencies (no DB, no HTTP). Use fakes or value objects only.
- Integration tests use Aspire.Hosting.Testing with real containers — no mocking the database.
- Blazor component tests use bunit — no Selenium or browser-based testing.
- Coverage targets are set per layer (unit, integration, component), not as a single global number.
