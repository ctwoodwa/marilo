# Quality Gates

Filled during workspace setup. Run the setup trigger to populate.

## Definition of Done

- Feature is covered by unit tests at the service/domain layer.
- Integration test covers the happy path end-to-end.
- OpenAPI/GraphQL schema is up to date.
- No compiler warnings introduced.
- Scalar and GraphQL Playground verified working in dev environment.
- OTEL traces confirmed flowing to {{OBSERVABILITY_BACKEND}}.
- Production-readiness checklist in Stage 05 passes.

## Coverage Expectations

- Domain/service layer: {{UNIT_TEST_COVERAGE_TARGET}} minimum.
- Integration tests: all public API endpoints covered.
- No untested code paths in auth or data access layers.