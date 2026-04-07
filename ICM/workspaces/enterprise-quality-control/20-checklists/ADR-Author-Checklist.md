# ADR Author Checklist (System-Wide)

> **Scope:** Every system-level ADR authored within {{ PRODUCT_NAME }}.
> **Purpose:** Ensure ADRs are complete, testable, and aligned with ICM quality gates.

## Structural Completeness

- [ ] **Context** section filled: forces, constraints, business drivers documented.
- [ ] **Decision drivers** listed with relative weights or priorities.
- [ ] **Options** section has at least two alternatives with pros/cons.
- [ ] **Decision** section uses clear "We will..." language.
- [ ] **Consequences** section covers positive, negative, and neutral outcomes.
- [ ] **Implementation plan** section defines phases and tasks.
- [ ] **Success criteria** are measurable and time-bound.

## Metadata Compliance (per ADR-0018)

- [ ] YAML frontmatter present with all required fields.
- [ ] `adr_id` follows four-digit numbering in correct range.
- [ ] `status` is set accurately (`proposed`, `accepted`, `deprecated`, `superseded`).
- [ ] `tags` include relevant categories.
- [ ] `related_adrs` populated where cross-references exist.
- [ ] Filename follows `ADR-NNNN-<kebab-case>.md` convention.

## Validation and Test Strategy

- [ ] **Validation** section exists and includes:
  - [ ] Required test types identified (unit, integration, contract, performance, observability).
  - [ ] Test seams classified (State, Service, CQRS, Navigation, Events) per ADR-0024.
  - [ ] Minimal concrete scenarios to test listed (at least 3 per seam).
- [ ] Coverage target stated for new/changed code introduced by this ADR.
- [ ] TDD guidance included (which tests to write before implementation).

## Enterprise Alignment

- [ ] Decision is consistent with enterprise defaults (icm-defaults).
- [ ] Deviations from defaults are explicitly justified with rationale.
- [ ] No unresolved conflicts with existing accepted ADRs.
- [ ] Architecture team review completed (or scheduled).

## Implementation Readiness

- [ ] Implementation plan includes test work items per phase.
- [ ] Ownership assigned for implementation tasks.
- [ ] Dependencies and sequencing constraints documented.
- [ ] Rollback/migration strategy defined for breaking changes.
