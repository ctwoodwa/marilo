# ICM Quality Gates

> **Role in ICM:** Quality control reference — defines lifecycle gates that every ADR-governed feature must pass through, with explicit testing hooks at each gate.

> Status: Draft
> Owner: Architecture Team
> Last updated: 2026-03-21

## Purpose

Quality gates enforce that every feature flowing through the ICM process produces testable artifacts. Each gate has a **decision check** (does the artifact exist and meet standards?) and a **test hook** (what tests must be identified, planned, or verified at this point?).

---

## Gate 1: Problem Framing

**When:** Before design work begins.

**Decision check:**
- [ ] Decision context and problem statement exist (system ADR or module ADR template).
- [ ] Affected modules and boundaries are identified.
- [ ] Success criteria are defined in measurable terms.

**Test hook:**
- [ ] Identify "where will we test this?" — classify by seam: State, Service, CQRS, Navigation, Events.
- [ ] List the primary behavior(s) the feature must exhibit.

---

## Gate 2: Options & Drivers

**When:** After options analysis, before decision.

**Decision check:**
- [ ] At least two options documented with pros/cons.
- [ ] Decision drivers listed and weighted.
- [ ] Alignment with enterprise defaults (icm-defaults) confirmed.

**Test hook:**
- [ ] For the preferred option, list primary behaviors and edge cases that must be covered.
- [ ] Identify test types needed: unit, integration, contract, e2e, performance.

---

## Gate 3: Decision & Boundaries

**When:** After architectural decision is made.

**Decision check:**
- [ ] ADR decision section completed with clear "We will..." statement.
- [ ] Boundaries explicitly chosen per ADR-0024 (State vs Service vs CQRS).
- [ ] Domain primitives and business logic governance reviewed where applicable.

**Test hook — map each boundary to test types:**

| Boundary | Required tests |
|----------|---------------|
| **State** | State-transition tests, derivation tests, RouteState navigation tests |
| **Service** | Orchestration unit tests (ports/adapters mocked), integration tests with real handlers |
| **CQRS** | Query/command handler unit tests, DB integration tests, parameter validation |
| **Navigation** | Component tests asserting RouteState actions, state tests for navigation commands |
| **Eventing** | Notification handler unit tests, cross-module integration tests |

---

## Gate 4: Implementation Plan

**When:** Before development starts.

**Decision check:**
- [ ] Phases and tasks documented in ADR implementation plan section.
- [ ] Dependencies and sequencing defined.
- [ ] Ownership assigned per task group.

**Test hook:**
- [ ] Each phase includes explicit test work items (new tests, refactors, coverage targets).
- [ ] "Test Task" sub-issues created in tracking system for each seam.
- [ ] Coverage targets set for new/changed code in this feature.

---

## Gate 5: Validation

**When:** Before marking feature as done.

**Decision check:**
- [ ] "Test Strategy and TDD Guidance" section of ADR followed.
- [ ] All implementation plan tasks completed.
- [ ] ADR status updated to "accepted" (or remains "proposed" with documented blockers).

**Test hook:**
- [ ] Tests exist at every chosen seam before marking ADR "accepted" or story "done".
- [ ] CI pipeline passes with coverage thresholds met for changed code.
- [ ] Coverage report shows improvement or maintenance in affected modules.
- [ ] Contract tests verified where interface changes were made.

---

## Gate Summary

| Gate | Decision artifact | Test output |
|------|------------------|-------------|
| 1. Problem Framing | Problem statement + affected modules | Seam classification |
| 2. Options & Drivers | Options analysis + drivers | Behavior list + test types |
| 3. Decision & Boundaries | ADR decision + boundary selection | Boundary-to-test mapping |
| 4. Implementation Plan | Phased task list | Test work items per phase |
| 5. Validation | Completed implementation | Tests passing at all seams |
