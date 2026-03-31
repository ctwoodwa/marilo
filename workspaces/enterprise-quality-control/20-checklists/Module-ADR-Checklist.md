# Module ADR Checklist

> **Scope:** Module-level ADRs that govern decisions within a specific module or bounded context.
> **Purpose:** Ensure module ADRs include module-specific testing strategy and align with system ADR expectations.

## All Items from ADR Author Checklist

- [ ] All items from `ADR-Author-Checklist.md` are satisfied (structural completeness, metadata, validation, alignment).

## Module-Specific Additions

### Module Context

- [ ] Module name and bounded context clearly identified.
- [ ] Module's role within the system architecture documented.
- [ ] Inter-module dependencies and communication patterns listed.
- [ ] Module-specific constraints (performance, data, security) noted.

### Module Testing Strategy

- [ ] Module-level test types defined:
  - [ ] **Unit tests:** domain logic, state handlers, service orchestration within the module.
  - [ ] **Integration tests:** module boundary interactions (API calls, event publishing, data access).
  - [ ] **Performance tests:** module-specific performance scenarios if applicable.
  - [ ] **UAT scenarios:** user-facing acceptance criteria mapped to test scenarios.
- [ ] Integration with system ADR test expectations confirmed:
  - [ ] Module tests align with boundaries chosen in related system ADRs.
  - [ ] Cross-module contract tests identified where module publishes/consumes events or APIs.

### Module Coverage Goals

- [ ] Coverage target for new module features stated (e.g., ">80% lines for new service classes").
- [ ] Critical paths within the module identified and prioritized for coverage.
- [ ] Test infrastructure requirements noted (test doubles, fixtures, containers).

### Module-Level Implementation

- [ ] Module-specific implementation phases defined.
- [ ] Test tasks included in each phase.
- [ ] Module owner and contributing teams identified.
