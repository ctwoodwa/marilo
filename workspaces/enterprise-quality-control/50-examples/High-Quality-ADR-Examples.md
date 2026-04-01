# High-Quality ADR Examples

> **Purpose:** Demonstrate what a well-structured ADR looks like when it includes explicit test strategy, boundary decisions, and validation criteria.

## Example: ADR with Complete Test Strategy

The following shows the key sections of an ADR that passes all quality gate checks and includes a thorough test strategy. This is a condensed example — a real ADR would include full prose in each section.

---

```markdown
---
adr_id: "ADR-0025"
title: "Work Order Status Transition Engine"
status: "accepted"
date_proposed: "2026-02-01"
date_accepted: "2026-02-15"
decision_makers:
  - "Architecture Team"
  - "Asset Management Team Lead"
tags:
  - "domain-logic"
  - "state-management"
  - "work-orders"
supersedes: ""
superseded_by: ""
related_adrs:
  - "ADR-0024"
  - "ADR-0011"
  - "ADR-0005"
---

# ADR-0025: Work Order Status Transition Engine

## Status

Accepted

## Context

Work orders move through lifecycle states (Draft → Submitted → Approved →
In Progress → Completed → Closed). Currently, status transitions are scattered
across multiple UI components and API handlers with inconsistent validation.

**Forces:**
- Business rules differ by work order type (corrective, preventive, emergency).
- Some transitions require approval workflows.
- Status history must be auditable.
- UI must reflect real-time status changes.

## Decision Drivers

1. Consistency: all transitions must apply the same rules regardless of entry point.
2. Testability: transition rules must be unit-testable without UI or database.
3. Auditability: every transition must be recorded with who/when/why.
4. Performance: status checks must not require database round-trips for validation.

## Decision

We will implement a State-based status transition engine using the Fluxor state
pattern (per ADR-0011) with explicit guard conditions for each transition.

**Boundary classification (per ADR-0024): State**

Rationale: Status transitions are UI/session-scoped derivations that determine
what actions are available. The engine evaluates rules in-memory from the current
work order state without requiring external service calls.

## Consequences

### Positive
- All transition rules centralized and unit-testable.
- UI reactively updates based on state changes.
- Guard conditions prevent invalid transitions at the state layer.

### Negative
- More state actions and handlers to maintain.
- Team must learn the state transition pattern.

### Neutral
- Status history recording still requires a CQRS command (separate concern).

## Implementation Plan

### Phase 1: Core Engine (Sprint 14)
- Define `WorkOrderState` with status enum and transition guards.
- Implement action handlers for each valid transition.
- **Test tasks:** Unit tests for every transition (valid + invalid).

### Phase 2: Integration (Sprint 15)
- Wire state engine to UI components.
- Connect status recording via CQRS command.
- **Test tasks:** Integration tests for state → UI binding. CQRS handler tests.

### Phase 3: Type-Specific Rules (Sprint 16)
- Add work-order-type-specific guard conditions.
- **Test tasks:** Parameterized tests across all work order types.

## Validation (Test Strategy)

### Boundary: State
This is a State-boundary feature (per ADR-0024). Tests focus on state transitions
and derivations.

### Required Test Types

| Test Type | Seam | What to verify |
|-----------|------|---------------|
| Unit | State transitions | Each valid transition produces correct new state |
| Unit | Guard conditions | Invalid transitions are rejected with correct error |
| Unit | Derivations | Available actions computed correctly from current state |
| Unit | Type-specific rules | Guards vary correctly by work order type |
| Integration | State → UI | Components render correct actions for each state |
| Integration | State → CQRS | Status recording command fires on transition |

### Concrete Scenarios (minimum)

1. Draft → Submitted: valid when all required fields populated.
2. Draft → Approved: invalid (must go through Submitted first).
3. Submitted → Approved: valid only for users with approver role.
4. In Progress → Completed: valid when all tasks marked done.
5. Completed → Closed: valid after 24-hour cooling period.
6. Any → Draft: invalid once past Submitted (no regression).
7. Emergency work order: skip approval (Draft → In Progress directly).

### Coverage Target
- State action handlers: > 90% line coverage.
- Guard conditions: 100% branch coverage.
- New code overall: > 85% line coverage.

## Compliance

- [x] Reviewed by architecture team
- [x] Consistent with enterprise standards (icm-defaults)
- [x] Boundary classification per ADR-0024
- [x] Test strategy included with concrete scenarios
- [x] No unresolved conflicts with existing ADRs
```

---

## What Makes This ADR High Quality

| Quality aspect | How this example demonstrates it |
|----------------|--------------------------------|
| **Clear boundary decision** | Explicitly states "State" boundary with rationale referencing ADR-0024 |
| **Test strategy section** | Dedicated Validation section with test types, seams, and concrete scenarios |
| **Concrete scenarios** | 7+ specific scenarios covering happy paths, invalid transitions, and edge cases |
| **Coverage targets** | Numeric targets per code area (90% handlers, 100% guards, 85% overall) |
| **Phased test tasks** | Each implementation phase includes explicit test work items |
| **Traceability** | Related ADRs linked, boundary decision documented, compliance checked |

## Anti-Patterns to Avoid

| Anti-pattern | Problem | Fix |
|-------------|---------|-----|
| "Tests will be written" | No specifics, no accountability | List concrete scenarios and assign test tasks |
| Missing boundary decision | Unclear where tests should focus | Classify per ADR-0024 before implementation |
| No coverage target | No way to measure success | Set numeric targets per layer |
| Validation section empty | Quality gate 5 cannot be verified | Fill with test types, seams, and scenarios |
| All tests deferred to "later" | Tests never get written | Include test tasks in each implementation phase |
