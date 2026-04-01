# TDD and Test Seams Playbook

> **Purpose:** Explain test seams by architecture layer, reference the governing ADRs, and provide practical TDD guidance for each seam.

> Status: Draft
> Owner: Architecture Team
> Last updated: 2026-03-21

## Test Seams by Architecture Layer

| Layer | Primary ADRs | Test Focus |
|-------|-------------|------------|
| **State** | ADR-0011, ADR-0005, ADR-0024 | State transitions, derivations, navigation flows |
| **Application Service** | ADR-0024, ADR-0023 | Orchestration, retries, event publishing |
| **Persistence CQRS** | ADR-0006, ADR-0013 | SQL parameters, mapping, transactions |
| **DAB API Endpoints** | DAB spec, security-baseline | REST/GraphQL responses, permissions, config validation |
| **Navigation** | ADR-0007, ADR-0011 | Route actions, observability for navigation |
| **Eventing** | ADR-0023, ADR-0011 | Notification handlers and cross-module effects |
| **UI Components** | ADR-0012, ADR-0007 | bUnit component rendering, interactions, state binding |
| **E2E / User Journeys** | ADR-0007, ADR-0024 | Playwright critical path flows, navigation, role-based access |

---

## Layer 1: State

### What to Test

- **Action handlers:** Each action that mutates state should have a test.
- **Derivations:** Computed values derived from state (selectors, projections).
- **RouteState:** Navigation actions that change the route.

### Typical Unit Tests

```
Test: "HandleAction_WhenValidInput_UpdatesStateCorrectly"
  Arrange: Create initial state with known values.
  Act:     Dispatch action with test parameters.
  Assert:  New state matches expected values.

Test: "Derivation_WhenStateChanges_RecomputesCorrectly"
  Arrange: Set state to specific values.
  Act:     Read derived/computed property.
  Assert:  Derived value matches expected calculation.
```

### TDD Approach

1. Write a test for the desired state transition (given state A + action → state B).
2. Create the action class and handler stub.
3. Implement handler logic until the test passes.
4. Add edge-case tests (null input, boundary values, error states).
5. Refactor handler while keeping all tests green.

### Integration Tests

- Test state → UI binding (component renders correct data for given state).
- Test state persistence across navigation (if state survives route changes).

---

## Layer 2: Application Service

### What to Test

- **Orchestration flow:** Correct sequence of calls to ports/adapters.
- **Branching logic:** Different paths based on input or external state.
- **Retry and compensation:** Error handling and retry behavior.
- **Event publishing:** Correct notifications dispatched after operations.

### Typical Unit Tests

```
Test: "Execute_WhenAllPortsSucceed_ReturnsSuccessAndPublishesEvent"
  Arrange: Mock ports to return success. Mock MediatR sender.
  Act:     Call service method.
  Assert:  Ports called in correct order. Notification published.

Test: "Execute_WhenPortFails_RetriesAndLogsError"
  Arrange: Mock port to fail once, then succeed.
  Act:     Call service method.
  Assert:  Port called twice. Error logged. Final result is success.
```

### TDD Approach

1. Write a test for the happy path (all dependencies succeed → expected result).
2. Create service class with constructor-injected ports.
3. Implement orchestration logic until the test passes.
4. Write failure-path tests (port throws, returns error, times out).
5. Implement error handling until failure tests pass.
6. Write event-publishing tests if applicable.

### Integration Tests

- Test service → CQRS handler → database round-trip (no mocks).
- Test service with real MediatR pipeline (handlers, behaviors).

---

## Layer 3: Persistence CQRS

### What to Test

- **Parameter building:** Handler constructs correct parameters for stored procedures.
- **Stored procedure naming:** Correct procedure name is called.
- **Result mapping:** Database results are mapped to DTOs correctly.
- **Error handling:** SQL exceptions are caught and wrapped appropriately.

### Typical Unit Tests

```
Test: "Handle_BuildsCorrectParametersForStoredProcedure"
  Arrange: Create request with known values.
  Act:     Execute handler with mocked connection.
  Assert:  Parameters include LogUser, LogAppName, Identifier, and request-specific fields.

Test: "Handle_MapsResultToResponseCorrectly"
  Arrange: Mock Dapper to return known result set.
  Act:     Execute handler.
  Assert:  Response DTO fields match result set values.
```

### TDD Approach

1. Write a test asserting correct parameter building from request.
2. Create handler stub with parameter construction.
3. Write a test asserting correct stored procedure name.
4. Implement procedure call.
5. Write result mapping test with mock data.
6. Implement mapping logic.
7. Write error-handling tests.

### Integration Tests

- Run against test database or Testcontainers.
- Verify stored procedure execution returns expected results.
- Test transactional behavior (commit/rollback).

---

## Layer 3.5: DAB API Endpoints

### What to Test

- **REST responses:** GET (single, list, filter, sort, pagination), POST, PATCH, DELETE return correct status codes and shapes.
- **GraphQL queries and mutations:** Queries return correct data; mutations create, update, and delete correctly.
- **Permissions:** Each role can only access authorized operations; row-level policies filter data by claims.
- **Configuration:** dab-config.json has valid entity definitions, sources, permissions, and no anonymous writes.
- **Error handling:** Invalid requests return correct HTTP status codes and error shapes.

### Typical Unit Tests

```
Test: "DabConfig_AllEntities_HavePermissionsDefined"
  Arrange: Parse dab-config.json.
  Act:     Enumerate all entities.
  Assert:  Every entity has at least one permission entry.

Test: "DabConfig_NoEntity_AllowsAnonymousWrite"
  Arrange: Parse dab-config.json.
  Act:     Check anonymous role actions across all entities.
  Assert:  No anonymous role has create, update, or delete.
```

### Typical Integration Tests

```
Test: "GetAssetById_ReturnsCorrectEntity"
  Arrange: Seed test database with known asset.
  Act:     GET /api/Asset/AssetId/ASSET-001.
  Assert:  200 with correct fields in DAB response wrapper.

Test: "CreateAsset_WithDuplicateId_Returns409"
  Arrange: Seed test database with existing asset.
  Act:     POST /api/Asset with same ID.
  Assert:  409 Conflict.

Test: "ReadOnlyRole_CannotCreateAssets"
  Arrange: Auth token with reader role.
  Act:     POST /api/Asset.
  Assert:  403 Forbidden.
```

### TDD Approach

1. Write config validation tests asserting each entity has correct source, permissions, and field mappings.
2. Create or update dab-config.json entity definitions.
3. Write REST integration tests for GET (single, list, filtered, sorted, paginated).
4. Configure REST endpoints and verify tests pass.
5. Write GraphQL query and mutation tests.
6. Configure GraphQL and verify tests pass.
7. Write permission tests for each role × entity × operation combination.
8. Configure role permissions and row-level policies until permission tests pass.
9. Write contract tests (Pact) for each consumer of the DAB API.

### Integration Tests

- Run DAB against a Testcontainers SQL Server instance for isolation.
- Verify REST and GraphQL responses match direct SQL query results.
- Test YARP gateway proxying to DAB endpoints.
- Verify Scalar UI and GraphQL Playground load through gateway routes.
- See `50-examples/High-Quality-Test-Examples-DAB-API.md` for concrete examples.

---

## Layer 4: Navigation

### What to Test

- **Route actions:** Components dispatch correct `RouteState.ChangeRouteAction`.
- **State updates:** RouteState handler updates navigation state correctly.
- **Deep links:** URL → correct initial state.

### Typical Unit Tests

```
Test: "Component_WhenButtonClicked_DispatchesChangeRouteAction"
  Arrange: Render component with mocked MediatR.
  Act:     Simulate button click.
  Assert:  Mediator.Send called with ChangeRouteAction("expected/route", params).

Test: "RouteState_WhenChangeRouteAction_UpdatesCurrentRoute"
  Arrange: Initial RouteState with route A.
  Act:     Dispatch ChangeRouteAction to route B.
  Assert:  CurrentRoute == "B", PreviousRoute == "A".
```

### TDD Approach

1. Write a component test asserting the correct action is dispatched.
2. Create component with navigation trigger.
3. Write RouteState handler test for the action.
4. Implement handler.
5. Add deep-link and back-navigation tests.

---

## Layer 5: Eventing

### What to Test

- **Handler behavior:** Notification handlers perform correct updates.
- **Idempotency:** Re-executing handler produces no double-effects.
- **Cross-module effects:** Notifications received by other modules produce correct state changes.

### Typical Unit Tests

```
Test: "Handler_WhenNotificationReceived_UpdatesTargetState"
  Arrange: Create notification with test data. Set up fake store.
  Act:     Execute handler.
  Assert:  Store contains expected updates.

Test: "Handler_WhenExecutedTwice_IsIdempotent"
  Arrange: Create notification. Set up fake store.
  Act:     Execute handler twice.
  Assert:  Store state is same as after single execution.
```

### TDD Approach

1. Write a test asserting the handler updates state correctly.
2. Create notification class and handler stub.
3. Implement handler logic.
4. Write idempotency test.
5. Adjust handler to be idempotent if needed.
6. Write cross-module integration test if applicable.

---

## Layer 6: UI Components (bUnit)

### What to Test

- **Rendering from state:** Component displays correct data for a given state.
- **User interactions → actions:** Clicks, inputs, and selections dispatch correct state actions via MediatR.
- **Conditional rendering:** Elements appear/disappear based on state flags (permissions, status, loading).
- **Validation:** Client-side validation prevents invalid submissions.

### Typical Unit Tests (bUnit)

```
Test: "Component_RendersTitle_FromState"
  Arrange: Set up IState<T> mock with known values.
  Act:     RenderComponent<MyComponent>().
  Assert:  Find("[data-testid='title']").TextContent matches expected.

Test: "SubmitButton_DispatchesAction_WhenFormValid"
  Arrange: Render component, fill inputs.
  Act:     Find("[data-testid='submit']").Click().
  Assert:  Mediator.Verify Send was called with expected action.

Test: "SubmitButton_ShowsErrors_WhenFormInvalid"
  Arrange: Render component, leave required fields empty.
  Act:     Find("[data-testid='submit']").Click().
  Assert:  Validation message visible, Mediator.Send never called.
```

### TDD Approach

1. Write a test asserting the component renders expected elements from state.
2. Create the component with `data-testid` attributes on interactive elements.
3. Write a test asserting a user interaction dispatches the correct MediatR action.
4. Wire up the interaction handler.
5. Write validation tests to confirm invalid input is caught client-side.
6. Add conditional rendering tests (loading, empty, error states).

### Key Conventions

- Use `data-testid` attributes for all selectors (resilient to CSS refactors).
- Inject `IState<T>` and `IMediator` via bUnit's `Services.AddSingleton(mock.Object)`.
- For Telerik components, register `Services.AddTelerikBlazor()` in the test context.
- See `50-examples/High-Quality-Test-Examples-bUnit.md` for concrete examples.

---

## Layer 7: E2E User Journeys (Playwright)

### What to Test

- **Critical user journeys:** Full flows from login through task completion.
- **Navigation and deep links:** URLs resolve to correct pages, back/forward works.
- **Role-based access:** Different roles see different UI elements and capabilities.
- **Grid interactions:** Sorting, filtering, paging on Telerik grids.
- **Responsive layout:** Mobile/tablet viewports render correctly.

### Typical E2E Tests (Playwright)

```
Test: "CreateWorkOrder_FullFlow"
  Arrange: Login as operator.
  Act:     Navigate → fill form → submit.
  Assert:  Success toast shown, redirected to detail, status is "Draft".

Test: "ApproveWorkOrder_AsApprover"
  Arrange: Login as approver, navigate to submitted work order.
  Act:     Click approve → confirm dialog.
  Assert:  Status changes to "Approved", approve button disappears.

Test: "DeepLink_LoadsCorrectData"
  Arrange: Login.
  Act:     Navigate directly to /work-orders/WO-1234.
  Assert:  Page shows correct work order data.
```

### TDD Approach

1. Define the critical user journey as a sequence of steps.
2. Write the Playwright test describing the expected flow.
3. Run it — it will fail on missing routes, elements, or behaviors.
4. Implement the feature (using bUnit tests at the component level as you go).
5. Re-run the Playwright test to confirm the full journey works end-to-end.

### Key Conventions

- Use `Page.GetByTestId("x")` as the primary locator strategy.
- Use `Expect(locator).ToBeVisibleAsync()` / `ToHaveTextAsync()` for assertions.
- Categorize tests: `[Category("smoke")]` for PR gates, `[Category("full")]` for nightly.
- Keep E2E tests focused on journeys, not unit-level behavior (that belongs in bUnit/xUnit).
- Run against a real deployed instance (not in-process) for true E2E coverage.
- See `50-examples/High-Quality-Test-Examples-Playwright.md` for concrete examples.

---

## General TDD Principles

1. **Red:** Write a failing test that describes the desired behavior.
2. **Green:** Write the minimum code to make it pass.
3. **Refactor:** Improve code quality while keeping tests green.
4. **One assertion per test:** Each test verifies one behavior.
5. **Descriptive names:** Test names describe the scenario and expected outcome.
6. **Arrange-Act-Assert:** Consistent test structure across all layers.
