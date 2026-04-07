# Risk Matrix

How to score untested surface by risk level, and how to assign each risk level to a test layer.

## Risk Scoring Rules

Apply the first matching rule. Risk is cumulative — if multiple rules match, take the highest.

| Rule | Risk Level |
|---|---|
| Class implements a mathematical algorithm or scoring function | High |
| Class executes a multi-step business process (e.g., solver pipeline) | High |
| Class is called on every API request (middleware, auth handlers) | High |
| Class writes to or reads from a database | Medium |
| Class coordinates between two or more services or repositories | Medium |
| Class validates user input at an API boundary | Medium |
| Class generates output consumed by external systems (reports, exports) | Medium |
| Blazor page or component that handles user data entry or submission | Medium |
| Class is a simple read-only DTO or value object | Low |
| Blazor page that only renders read-only data | Low |
| Class is a thin wrapper with no logic (pass-through) | Low |
| Class is auto-generated or scaffolded | Low — defer |

## Layer Assignment by Risk

| Risk Level | Recommended Layer | Rationale |
|---|---|---|
| High (algorithm / solver) | Unit | Algorithms need precise, fast, exhaustive case coverage |
| High (middleware / auth) | Integration | Auth requires real HTTP pipeline to be meaningful |
| Medium (data access) | Integration | Requires a real database; unit tests with mocks are brittle |
| Medium (coordination) | Integration or Unit | Unit if logic is separable; integration if DB/HTTP involved |
| Medium (validation) | Unit | Pure logic, no infrastructure |
| Medium (Blazor + data entry) | Component (bunit) | Test UI interactions without a browser |
| Low (read-only Blazor) | Component (bunit) | Render test only; low investment |
| Low (DTO / value object) | Unit (optional) | Only if non-trivial construction or validation |
| Low — defer | None this sprint | Document and revisit |

## Deferral Criteria

Defer to a later iteration if the test would require:
- Significant test infrastructure not yet in place (e.g., DAB mock, external OAuth)
- Access to a third-party system with no test double available
- More than 2 days of test setup work for a single low-risk class

Always document the deferral reason in `_config/test-strategy.md`.

## Comet-Specific Risk Classifications

Pre-classified based on what is known about the domain:

| Area | Risk | Notes |
|---|---|---|
| `InvestmentModeler.Prioritization` | High | Weighted scoring algorithm |
| `InvestmentModeler.Scheduling` (GreedyScheduler) | High | Constraint-based allocation algorithm |
| `InvestmentModeler.Projection` | High | Multi-year condition modeling |
| `InvestmentModeler.Solver` | High | Outer-loop optimization |
| `Parsons.Comet.Gateway` auth handlers | High | Every request passes through these |
| API endpoint handlers (ad-hoc) | Medium | Business logic at the boundary |
| Blazor InvestmentModeler pages | Medium | User data entry; drives solver inputs |
| Blazor RackStack pages | Medium | User data entry |
| Blazor Admin pages | Medium | Admin data management |
| DTOs and records | Low | No logic |
| Layout components | Low | Render-only |
