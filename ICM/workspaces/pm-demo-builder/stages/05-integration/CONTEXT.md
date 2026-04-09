# Stage 05: Integration

Wire services, register DI, seed data, and connect cross-cutting concerns.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../04-page-build/output/build-manifest.md` | Full file | Know what was built |
| Previous stage | `../03-domain-modeling/output/domain-models.md` | Service interfaces | What to register |
| Reference | `../../shared/implementation-guardrails.md` | "Serilog + OTEL + DAB" | Stack conventions |

## Process

1. Read the build manifest — identify all new services that need DI registration.
2. Create mock/in-memory implementations for each new service interface (following the `InMemoryUserNotificationService` pattern).
3. Register services in `Marilo.PmDemo/Program.cs` (scoped, alongside existing registrations).
4. Add seed data to each mock service — realistic PM demo scenarios.
5. If DAB entities are in scope:
   a. Add EF Core entity configurations.
   b. Create migration via `dotnet ef migrations add`.
   c. Add DAB entity config for GraphQL exposure.
6. Wire SignalR hub events if real-time updates are needed for new features.
7. Wire Wolverine handlers if domain events trigger cross-feature updates (e.g., deficiency → risk escalation).
8. Build the solution. Fix any compile errors.
9. Run existing tests. Fix any regressions.
10. Write the integration manifest to output.

## Audit

| Check | Pass Condition |
|-------|---------------|
| Clean build | `dotnet build` succeeds with 0 errors |
| Tests pass | All existing tests still pass |
| DI complete | Every injected service resolves at runtime |
| Seed data present | Navigating to new pages shows realistic demo data |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Integration manifest | `output/integration-manifest.md` | Table: service, registration, seed data summary, DAB entities |
