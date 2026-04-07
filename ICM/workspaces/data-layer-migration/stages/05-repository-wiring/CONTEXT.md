# Stage 05 - Repository Wiring

Define repository interfaces, implement them using EF Core and Dapper, register them in DI,
and replace the in-memory store with the real data layer. Validate with integration tests.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Endpoint map | `../../_config/endpoint-map.md` | Ad-hoc endpoint summary | Defines which repositories are needed |
| EF Core setup summary | `../04-ef-core-setup/output/[slug]-ef-setup.md` | DbContext and entity list | What's available for repository implementations |
| In-memory store source | `{{IN_MEMORY_STORE_PATH}}` | Current implementation | Replace method-by-method |
| Migration config | `../../_config/migration-context.md` | API project path | Output destinations |

## Process

1. Define a repository interface per aggregate root or entity group accessed by ad-hoc endpoints. Keep interfaces minimal — only the methods the endpoints actually call.
2. Implement each interface using EF Core. For bulk read operations flagged in the ad-hoc summary as performance-sensitive, use Dapper instead and note the decision.
3. Register all repositories in the DI container (Scoped lifetime, registered against their interface).
4. Update each ad-hoc endpoint handler to inject the repository interface instead of `InMemoryDataStore`.
5. Remove `InMemoryDataStore` from the DI registration. Do not delete the source file until integration tests pass.
6. Run integration tests using Aspire.Hosting.Testing with a real SQL Server container. Fix any failures.
7. Delete `InMemoryDataStore.cs` once all tests are green.
8. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Repository interfaces | `{{API_PROJECT_PATH}}/Data/Repositories/` | C# source files |
| Repository implementations | `{{API_PROJECT_PATH}}/Data/Repositories/` | C# source files |
| DI registration snippet | `output/[slug]-di-registration.md` | Code snippet for `Program.cs` |
| Integration test results | `output/[slug]-integration-test-results.md` | Pass/fail summary with failure details |
| Migration complete confirmation | `output/[slug]-migration-complete.md` | Checklist confirming all steps done |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 2 | Repository interfaces and implementations (first two) | Approve pattern before applying to remaining repositories |
| 6 | Integration test results | Review failures before deleting in-memory store |

## Audit

| Check | Pass Condition |
|---|---|
| All ad-hoc endpoints use repositories | No remaining `InMemoryDataStore` references in endpoint handlers |
| Repository interfaces are minimal | No method on an interface that isn't called by at least one endpoint |
| All integration tests pass | Zero failures against the real SQL Server container |
| InMemoryDataStore deleted | The file no longer exists in the project after final green test run |
| No hardcoded connection strings | All database access uses DI-injected `DbContext` or `IDbConnection` |
