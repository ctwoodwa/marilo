# Navigation and Eventing Test Checklist

> **Driven by:** ADR-0007 (Navigation State Management Pattern), ADR-0023 (Inter-Component Communication and MediatR Eventing).
> **Purpose:** Ensure navigation and eventing patterns are correctly implemented and tested.

## Part A: Navigation Testing (ADR-0007)

### Step 1: Pattern Compliance

- [ ] No direct `NavigationManager` usage in component code.
- [ ] All navigation goes through `RouteState.ChangeRouteAction`.
- [ ] Route parameters are passed via the action, not via query strings or ambient state.
- [ ] Navigation targets use defined route constants (not hardcoded strings).

### Step 2: Component Tests

For each component that triggers navigation:

- [ ] Mock MediatR and assert the correct `RouteState.ChangeRouteAction` is sent.
- [ ] Verify action parameters match expected route and data.
- [ ] Test conditional navigation (e.g., navigate only if validation passes).
- [ ] Test that navigation is not triggered for invalid/unauthorized states.

### Step 3: State Tests

For RouteState itself:

- [ ] Test that `ChangeRouteAction` updates state predictably.
- [ ] Test deep-link scenarios: given a URL, assert correct initial state.
- [ ] Test back-navigation: assert state reverts or updates correctly.
- [ ] Test route parameter extraction and validation.

### Step 4: Integration Scenarios

- [ ] Test multi-step navigation flows (wizard, multi-page form):
  - Assert forward/backward navigation maintains state consistency.
  - Assert completion triggers correct final navigation.
- [ ] Test navigation with unsaved changes:
  - Assert confirmation/guard behavior works correctly.

---

## Part B: Eventing Testing (ADR-0023)

### Step 1: Pattern Compliance

- [ ] MediatR notifications used for in-process events (not custom event bus).
- [ ] Notification handlers are small and focused (single responsibility).
- [ ] Handlers are idempotent (safe to re-execute).
- [ ] No circular notification chains (A notifies B notifies A).

### Step 2: Unit Tests for Notification Handlers

For each notification handler:

- [ ] Test with fake/mock store and sender:
  - Assert expected state updates occur.
  - Assert expected side effects (additional notifications, log entries).
- [ ] Test handler idempotency:
  - Execute handler twice with same notification, assert no double-effects.
- [ ] Test error handling:
  - Assert handler does not throw unhandled exceptions.
  - Assert failed handlers produce structured error logs.

### Step 3: Integration Tests for Cross-Module Effects

Where notifications cross module boundaries:

- [ ] Publish a notification and verify the receiving module processes it correctly.
- [ ] Verify that receiver state updates are consistent with sender expectations.
- [ ] Test ordering behavior if multiple handlers respond to the same notification.

### Step 4: Event Payload Verification

- [ ] Notification payload includes all required fields for consumers.
- [ ] Payload serialization/deserialization is tested (if events cross process boundaries).
- [ ] Sensitive data is not leaked in notification payloads.

---

## Coverage Verification

- [ ] All navigation-triggering components have tests asserting RouteState actions.
- [ ] All notification handlers have unit tests.
- [ ] Critical cross-module notification flows have integration tests.
- [ ] Tests pass in CI pipeline.
- [ ] Coverage for navigation and eventing code meets project threshold.
