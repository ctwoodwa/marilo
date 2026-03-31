# State vs Service Decision and Test Checklist

> **Driven by:** ADR-0024 (State vs Service Decision Criteria and Testing Guidance), ADR-0011 (State Management Architecture).
> **Purpose:** Ensure correct boundary selection and comprehensive test coverage at the chosen seams.

## Step 1: Boundary Decision

For each feature or behavior, confirm which boundary applies using ADR-0024 decision criteria:

### Choose State if:

- [ ] Behavior is UI/session-scoped.
- [ ] Behavior is view-specific (single component or page).
- [ ] Logic is primarily derivation (computed from existing state).
- [ ] No external service calls or side effects required.
- [ ] Data does not need to survive beyond the session.

### Choose Service if:

- [ ] Behavior spans multiple views or components.
- [ ] Logic involves orchestration or complex business rules.
- [ ] External integrations are required (APIs, message brokers, databases).
- [ ] Behavior needs retry logic, compensation, or transactional guarantees.
- [ ] Multiple consumers need the same capability.

### Document the Decision

- [ ] Boundary choice recorded in ADR or feature document.
- [ ] Rationale for choice documented (why State and not Service, or vice versa).
- [ ] Edge cases where boundary is ambiguous are noted with resolution.

---

## Step 2: State Testing Requirements

If **State** was chosen:

### State Transitions and Derivations

- [ ] Unit tests for each state action handler:
  - Given initial state + action → assert new state.
  - Test all state transitions, including error/edge states.
- [ ] Derivation tests:
  - Given state → assert computed/derived values are correct.
  - Test derivations with boundary inputs (null, empty, max values).

### RouteState / Navigation (if RouteState is involved, per ADR-0007)

- [ ] Tests for RouteState navigation behaviors:
  - Assert `RouteState.ChangeRouteAction` dispatches correctly.
  - Verify navigation state updates predictably.
  - Test deep-link and back-navigation scenarios.

### UI-Scoped Cache (if caching is involved)

- [ ] Cache key composition tests:
  - Assert cache keys are deterministic and collision-free.
- [ ] Cache invalidation tests:
  - Assert cache clears on expected state changes.

---

## Step 3: Service Testing Requirements

If **Service** was chosen:

### Orchestration Unit Tests

- [ ] Unit tests with ports/adapters mocked:
  - Assert correct orchestration flow (call order, branching).
  - Test retry logic and failure paths.
  - Test input validation and guard conditions.
  - Verify correct port methods called with expected parameters.

### Integration Tests

- [ ] Integration tests with real CQRS handlers (no UI):
  - Assert end-to-end flow through service → handler → data store.
  - Verify transactional correctness (commit/rollback).
  - Test with realistic data volumes where applicable.

### Cross-Module Effects

- [ ] If service publishes events:
  - Assert correct MediatR notifications are published.
  - Verify event payload completeness.
- [ ] If service consumes events:
  - Test handler idempotency.
  - Test error handling for malformed or unexpected events.

---

## Step 4: Coverage Verification

- [ ] All tests pass in CI pipeline.
- [ ] Coverage for new/changed code meets project threshold.
- [ ] No untested state transitions or service branches remain.
- [ ] Test names clearly describe the behavior being verified.
