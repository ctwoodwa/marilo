# Data Layer Migration

Replace EF6/ADO.NET/stored procedure access with Data API Builder for CRUD and EF Core for complex domain logic, behind a clean service layer.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../02-architecture-design/output/{{PROJECT_SLUG}}-architecture.md` | "DAB Entity Map" and "Solution Structure" sections | Know the DAB vs. EF Core split and project layout |
| Inventory | `../01-inventory-assessment/output/{{PROJECT_SLUG}}-solution-inventory.md` | Full file | Find all projects with database access |
| Pattern reference | `../../shared/dotnet-fx-to-blazor-patterns.md` | Data access rows | Map EF6/ADO.NET patterns to DAB/EF Core equivalents |
| DAB guide | `../../shared/dab-integration-guide.md` | Full file | Configure DAB entities, permissions, and relationships |

## Process

1. Read the architecture decisions and inventory from previous stages.
2. Extract the database schema from the source project: EF6 DbContext/models, ADO.NET queries, stored procedures.
3. For tables assigned to DAB in the architecture:
   - Generate `dab-config.json` entity definitions with REST and GraphQL endpoints.
   - Define permissions per role based on current authorization rules.
   - Map relationships (foreign keys) to DAB relationship config.
   - Configure stored procedure entities for complex queries that DAB supports.
4. For tables assigned to EF Core:
   - Design DbContext with DbSet properties and Fluent API configuration.
   - Migrate EF6 model configurations (EntityTypeConfiguration) to EF Core equivalents.
   - Plan EF Core migrations from the existing database schema.
5. Migrate ADO.NET code:
   - Replace `SqlConnection`/`SqlCommand` with DAB HTTP calls (for CRUD) or EF Core (for complex logic).
   - Convert inline SQL to parameterized DAB filters or EF Core LINQ.
6. Design the service layer:
   - `IDabApiService` -- wraps HttpClient calls to DAB REST/GraphQL endpoints.
   - Domain services -- inject EF Core DbContext for transactions and complex business logic.
   - Register all services in `Program.cs` via built-in DI.
7. Map stored procedures: determine which become DAB stored-procedure entities vs. EF Core `FromSqlRaw`.
8. For each `ConfigurationManager.ConnectionStrings` reference, migrate to `appsettings.json` + `IConfiguration`.
9. Produce the data migration plan.

## Audit

| Check | Pass Condition |
|-------|---------------|
| No inline SQL | Every SQL statement maps to a DAB endpoint, EF Core operation, or parameterized query |
| All tables assigned | Every table is assigned to DAB, EF Core, or both with documented rationale |
| DAB config valid | `dab-config.json` has valid entity definitions with permissions |
| Connection strings secure | No hardcoded credentials; all connections use `@env()` (DAB) or `IConfiguration` (EF Core) |
| Service layer complete | Every data access call routes through an injected service interface |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Data migration plan | `output/{{PROJECT_SLUG}}-data-plan.md` | Sections: DAB Entity Map, DAB Config, EF Core Models, Service Layer, Stored Proc Strategy, Connection Config |
| DAB config template | `output/{{PROJECT_SLUG}}-dab-config.json` | Working `dab-config.json` with entity definitions from the source schema |

<!-- Target: keep this file under 80 lines. -->
