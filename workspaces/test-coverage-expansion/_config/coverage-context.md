# Coverage Context

Filled during `setup`. Replace all `{{PLACEHOLDERS}}` before running stages.

## Identity

| Field | Value |
|---|---|
| Project name | `{{PROJECT_NAME}}` |
| Project slug | `{{PROJECT_SLUG}}` |
| Solution path | `{{SOLUTION_PATH}}` |
| Test project path | `{{TEST_PROJECT_PATH}}` |

## Test Framework

| Field | Value |
|---|---|
| Unit/integration framework | `{{TEST_FRAMEWORK}}` |
| Blazor component framework | `{{BLAZOR_TEST_FRAMEWORK}}` |
| Coverage tool | `{{COVERAGE_TOOL}}` |
| Coverage report format | `{{COVERAGE_REPORT_FORMAT}}` |

> Defaults: NUnit 4, bunit, Coverlet, Cobertura XML.

## Coverage Targets

| Layer | Target % | Rationale |
|---|---|---|
| Unit (domain / solvers) | `{{UNIT_COVERAGE_TARGET}}` | High — core business logic |
| Integration (API endpoints) | `{{INTEGRATION_COVERAGE_TARGET}}` | Medium — boundary validation |
| Component (Blazor pages) | `{{COMPONENT_COVERAGE_TARGET}}` | Medium — UI interaction paths |
| E2E (full pipeline) | `{{E2E_COVERAGE_TARGET}}` | Low — key user journeys only |

> Suggested starting targets: unit 80%, integration 70%, component 60%, e2e key journeys only.

## Source Projects

| Project | Type | Priority |
|---|---|---|
| _fill during Stage 01_ | | |

## Notes

`{{COVERAGE_NOTES}}`
