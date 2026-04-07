# DAB API Endpoint Test Checklist

> **Driven by:** DAB API specification (Stage 04), security-baseline.md, api-visibility.md, observability-guide.md.
> **Purpose:** Ensure Data API Builder endpoints are correctly configured, secured, and thoroughly tested across REST and GraphQL surfaces.

## Step 1: Configuration Compliance

Confirm dab-config.json structural correctness for each entity:

### Entity Definition

- [ ] Entity name follows naming convention (`PascalCase` singular).
- [ ] Source maps to correct database object (table, view, or stored procedure).
- [ ] Source type is explicitly set (`table`, `view`, or `stored-procedure`).
- [ ] REST and GraphQL paths are defined and follow URL conventions.
- [ ] REST route uses kebab-case plural (`/work-orders`).
- [ ] GraphQL type name matches entity name (`WorkOrder`).

### Field Mapping

- [ ] All exposed fields are explicitly listed (no implicit wildcard exposure).
- [ ] PII fields are excluded or marked with appropriate access policies.
- [ ] Computed/read-only fields are not included in create/update operations.
- [ ] Field aliases match API contract names when database column names differ.

### Relationships

- [ ] Foreign key relationships are defined for navigation properties.
- [ ] Relationship cardinality is correct (`one`, `many`).
- [ ] Target entity and linking fields are accurate.
- [ ] Nested query depth is bounded to prevent excessive joins.

---

## Step 2: Permission and Security Tests

For each entity, verify role-based access:

### Anonymous Access

- [ ] Entities that allow anonymous access are explicitly and intentionally configured.
- [ ] No entity exposes write operations (create, update, delete) to anonymous role.
- [ ] Anonymous access is disabled in production unless documented in an ADR.

### Authenticated Role Access

- [ ] Each role maps to the correct CRUD operations (`read`, `create`, `update`, `delete`).
- [ ] Field-level permissions restrict sensitive columns per role.
- [ ] Database policies (row-level filters) are applied where required.
- [ ] `@claims.userId` or equivalent token claims are used in policy filters.

### Environment-Specific Permissions

- [ ] Dev environment has broader access for developer experience.
- [ ] Test environment enforces SSO with read-only role per api-visibility.md.
- [ ] Prod environment uses least-privilege per security-baseline.md.
- [ ] Permission differences between environments are documented.

---

## Step 3: REST Endpoint Tests

For each entity exposed via REST:

### GET (Read) Operations

- [ ] `GET /api/{entity}` returns paginated list with correct default page size.
- [ ] `GET /api/{entity}/{id}` returns single item with all expected fields.
- [ ] `GET /api/{entity}?$filter=...` supports OData filter expressions.
- [ ] `GET /api/{entity}?$orderby=...` supports sorting.
- [ ] `GET /api/{entity}?$select=...` returns only requested fields.
- [ ] `GET /api/{entity}?$first=N&$after=cursor` pagination works correctly.
- [ ] Response includes `nextLink` when more results exist.
- [ ] 404 returned for non-existent entity ID.
- [ ] 401 returned when auth token is missing (authenticated entities).
- [ ] 403 returned when role lacks read permission.

### POST (Create) Operations

- [ ] `POST /api/{entity}` creates a new record with valid payload.
- [ ] Response returns 201 with created entity including server-generated fields (ID, timestamps).
- [ ] 400 returned for missing required fields.
- [ ] 400 returned for invalid field types or values.
- [ ] 409 returned for duplicate key violations.
- [ ] 403 returned when role lacks create permission.

### PUT/PATCH (Update) Operations

- [ ] `PUT /api/{entity}/{id}` or `PATCH /api/{entity}/{id}` updates the record.
- [ ] Only fields included in the payload are modified (PATCH behavior).
- [ ] Read-only fields in the payload are ignored or rejected.
- [ ] 404 returned for non-existent entity ID.
- [ ] 403 returned when role lacks update permission.
- [ ] Row-level policy prevents updating records owned by other users.

### DELETE Operations

- [ ] `DELETE /api/{entity}/{id}` removes or soft-deletes the record.
- [ ] 404 returned for non-existent entity ID.
- [ ] 403 returned when role lacks delete permission.
- [ ] Row-level policy prevents deleting records owned by other users.
- [ ] Cascade behavior is correct (dependent records handled).

---

## Step 4: GraphQL Endpoint Tests

For each entity exposed via GraphQL:

### Query Operations

- [ ] `query { entityById(id: "...") { fields } }` returns correct entity.
- [ ] `query { entities(filter: {...}) { items { fields } } }` returns filtered list.
- [ ] `query { entities(orderBy: {...}) { items { fields } } }` returns sorted list.
- [ ] `query { entities(first: N, after: "cursor") { items { fields } hasNextPage endCursor } }` supports pagination.
- [ ] Nested relationship queries return correct related data.
- [ ] Query depth is limited to prevent resource exhaustion.
- [ ] Null/not-found returns null with no errors for nullable queries.
- [ ] Introspection is disabled in production unless explicitly enabled.

### Mutation Operations

- [ ] `mutation { createEntity(item: {...}) { fields } }` creates and returns new record.
- [ ] `mutation { updateEntity(id: "...", item: {...}) { fields } }` updates and returns record.
- [ ] `mutation { deleteEntity(id: "...") { fields } }` deletes and returns removed record.
- [ ] Input validation errors return GraphQL error format with field-level detail.
- [ ] Permission errors return appropriate GraphQL error codes.

### Schema Validation

- [ ] GraphQL schema matches entity definitions in dab-config.json.
- [ ] Required fields are marked non-nullable in schema.
- [ ] Enum types are correctly mapped from database constraints.
- [ ] Relationship fields resolve correctly in schema.

---

## Step 5: Integration and Contract Tests

### DAB-to-Database Integration

- [ ] Each entity's REST GET returns data matching direct SQL query results.
- [ ] Create via REST followed by direct SQL verify confirms data persisted.
- [ ] Update via REST followed by direct SQL verify confirms data changed.
- [ ] Delete via REST followed by direct SQL verify confirms data removed.
- [ ] Stored procedure entities execute correctly and return expected results.
- [ ] Test with edge-case data (empty strings, max-length values, special characters, Unicode).

### Consumer Contract Tests

- [ ] Pact contracts defined for each consumer of DAB REST endpoints.
- [ ] Pact contracts defined for each consumer of DAB GraphQL endpoints.
- [ ] Provider verification runs against DAB instance with test data.
- [ ] Contract changes trigger consumer notification via Pact Broker.
- [ ] Breaking changes fail the provider build.

### Gateway Integration

- [ ] YARP gateway correctly proxies to DAB REST endpoints.
- [ ] YARP gateway correctly proxies to DAB GraphQL endpoint.
- [ ] Auth token propagation through gateway to DAB works correctly.
- [ ] Health check endpoint responds through gateway.
- [ ] Scalar UI loads through gateway route `/scalar/{service}`.
- [ ] GraphQL Playground loads through gateway route `/graphql/{service}/ui`.

---

## Step 6: Observability Verification

Per observability-guide.md requirements:

- [ ] DAB emits traces for HTTP inbound requests.
- [ ] DAB emits traces for database queries.
- [ ] Request count metrics are available.
- [ ] Query duration metrics are available.
- [ ] Error rate metrics are available.
- [ ] Structured logs include TraceId and SpanId.
- [ ] W3C TraceContext headers are propagated from gateway.
- [ ] Error responses produce structured error logs with correlation IDs.
- [ ] PII fields identified in DAB spec do not appear in logs or traces.

---

## Step 7: Performance and Resilience

### Performance Baseline

- [ ] Response time for single-entity GET is under acceptable threshold.
- [ ] Response time for paginated list GET is under acceptable threshold.
- [ ] Response time for filtered/sorted queries is under acceptable threshold.
- [ ] GraphQL nested queries with relationships complete within timeout.
- [ ] Concurrent request handling meets expected throughput.

### Error Handling

- [ ] Database connection failure returns 503 with retry-after header.
- [ ] Malformed request body returns 400 with descriptive error.
- [ ] Request timeout returns 504.
- [ ] DAB restarts cleanly after transient database outage.

---

## Step 8: Coverage Verification

- [ ] Every entity in dab-config.json has corresponding REST endpoint tests.
- [ ] Every entity with GraphQL enabled has corresponding GraphQL tests.
- [ ] Permission tests exist for every role × entity × operation combination.
- [ ] Integration tests cover create-read-update-delete round-trips.
- [ ] Contract tests cover all consumer-facing API surfaces.
- [ ] Test names describe the entity, operation, and scenario.
