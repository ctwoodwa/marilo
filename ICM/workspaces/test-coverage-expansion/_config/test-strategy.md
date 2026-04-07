# Test Strategy

Filled during Stage 02. Defines the agreed test layer assignment for each area.

## Layer Definitions

| Layer | Tool | Scope | Infrastructure |
|---|---|---|---|
| Unit | NUnit | Single class or function in isolation | None — fakes/value objects only |
| Integration | NUnit + Aspire.Hosting.Testing | API endpoint through real DB | SQL Server container via Aspire |
| Component | bunit | Single Blazor component or page | Mocked services via bunit |
| E2E | Aspire.Hosting.Testing | Full user journey across services | Full Aspire stack |

## Assignment Table

Filled during Stage 02. One row per testable area.

| Area | Layer | Priority | Scaffold Target |
|---|---|---|---|
| _fill during Stage 02_ | | | |

## Out of Scope

_Filled during Stage 02. Areas explicitly excluded and why._

| Area | Reason Excluded |
|---|---|
