# Persistence CQRS Test Checklist

> **Driven by:** ADR-0006 (Persistence CQRS Consolidation Pattern), ADR-0013 (Data Access Technology Selection).
> **Purpose:** Ensure CQRS handlers are correctly implemented and thoroughly tested.

## Step 1: Pattern Compliance

For each CQRS operation, confirm structural compliance:

### File Organization

- [ ] Request + handler co-located in single file.
- [ ] Naming follows convention: `Get[Entity]`, `List[Entities]`, `Create[Entity]`, `Change[Entity]`, `Delete[Entity]`.
- [ ] Request class implements correct MediatR interface (`IRequest<T>`).
- [ ] Handler class implements `IRequestHandler<TRequest, TResponse>`.

### Dapper-First Pattern (per ADR-0013)

- [ ] Uses Dapper for data access (not EF Core for queries).
- [ ] Calls stored procedures (not inline SQL for production queries).
- [ ] Required parameters included:
  - [ ] `LogUser` — audit trail user identifier.
  - [ ] `LogAppName` — calling application identifier.
  - [ ] `Identifier` — entity identifier (where applicable).

### Stored Procedure Convention

- [ ] Procedure name follows `GetStoredProcedure` + suffix pattern.
- [ ] Parameters mapped correctly from request to stored procedure.
- [ ] Return type mapped to domain/DTO model.

---

## Step 2: Unit Tests

For each CQRS handler:

### Parameter Building

- [ ] Test that handler builds correct parameters from request properties.
- [ ] Test that `LogUser`, `LogAppName`, and `Identifier` are always included.
- [ ] Test parameter mapping for optional/nullable fields.

### Stored Procedure Invocation

- [ ] Test that handler calls the expected stored procedure name.
- [ ] Test that the correct Dapper method is used (`QueryAsync`, `ExecuteAsync`, etc.).

### Result Mapping

- [ ] Test that results are mapped correctly to response DTOs.
- [ ] Test handling of empty result sets (return empty list, not null).
- [ ] Test handling of null/missing fields in database results.

### Error Handling

- [ ] Test behavior when stored procedure throws (SQL exception handling).
- [ ] Test behavior with invalid parameters (validation before DB call).

---

## Step 3: Integration Tests

For each CQRS handler (or critical subset):

### Database Integration

- [ ] Test against real database (test DB or container).
- [ ] Verify result mapping with actual stored procedure output.
- [ ] Verify transactional correctness:
  - [ ] Create operations insert correctly.
  - [ ] Change operations update expected rows.
  - [ ] Delete operations remove or soft-delete correctly.
- [ ] Test with edge-case data (empty strings, max-length values, special characters).

### Concurrency and Isolation

- [ ] Test behavior under concurrent access where applicable.
- [ ] Verify optimistic concurrency checks if implemented.

---

## Step 4: Observability Verification

- [ ] `OperationOutcome` logging is triggered for each handler execution.
- [ ] Structured log entries include operation name, duration, and result status.
- [ ] Optionally: assert log entries exist in test output for critical operations.
- [ ] Error scenarios produce structured error logs with correlation IDs.

---

## Step 5: Coverage Verification

- [ ] All CQRS handlers have corresponding unit tests.
- [ ] Critical handlers (Create, Change, Delete) have integration tests.
- [ ] Coverage for handler code meets project threshold.
- [ ] Test names describe the specific CQRS operation and scenario.
