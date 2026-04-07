# Data Layer Migration

Replace ADO/ADODB and COM data access with a hybrid approach: DAB auto-generates REST and GraphQL endpoints for standard CRUD, while EF Core powers a custom API service for complex business logic.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Architecture | `../02-architecture-design/output/{{PROJECT_SLUG}}-architecture.md` | "Endpoint Classification" and "Project Structure" sections | Know which endpoints are DAB-eligible vs custom API |
| Inventory | `../01-inventory-assessment/output/{{PROJECT_SLUG}}-page-inventory.md` | Full file | Find all pages with database access |
| Pattern reference | `../../shared/asp-to-blazor-patterns.md` | "Data Access" section | Map ADO patterns to DAB or EF Core equivalents |
| DAB reference | `../../shared/dab-reference.md` | Full file | DAB config syntax, entity definitions, permissions |

## Process

1. Read the architecture decisions (especially the endpoint classification) and inventory.
2. Extract every inline SQL statement and ADO connection string from the inventory.
3. Group SQL by table: identify all tables, columns, and relationships.
4. Design C# entity models for each table, using {{DATABASE_TYPE}} conventions.
5. For each DAB-eligible endpoint from the classification:
   - Define the entity in `dab-config.json` with source table, REST path, and GraphQL type.
   - Configure permissions per role (anonymous, authenticated, admin as needed).
   - Add relationships and column mappings where DB names differ from API names.
6. For custom API endpoints (business logic, multi-step operations, complex aggregations):
   - Design EF Core DbContext with DbSet properties and relationship configuration.
   - Plan EF Core migrations for schema ownership.
   - Design repository or service methods with parameterized queries.
7. Design the service layer: one service per domain area, injected via DI.
8. Map each COM object (`Server.CreateObject`) to its .NET replacement service.
9. For each `On Error Resume Next` block around data access, design explicit error handling.
10. Produce the data migration plan and DAB config artifact.

## Audit

| Check | Pass Condition |
|-------|---------------|
| No inline SQL | Every SQL statement maps to a DAB entity or parameterized EF Core query |
| All tables modeled | Every table has a C# entity and is in either DAB config or DbContext |
| DAB entities complete | Every DAB-eligible endpoint has an entity with permissions defined |
| COM objects replaced | Every `Server.CreateObject` has a named .NET service replacement |
| Connection strings secure | DAB uses `@env('VAR_NAME')`; EF Core uses `appsettings.json` config |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Data migration plan | `output/{{PROJECT_SLUG}}-data-plan.md` | Sections: Entity Models, DAB Entities, DbContext/Repositories, Service Layer, COM Replacements, Error Handling |
| DAB config | `output/{{PROJECT_SLUG}}-dab-config.json` | Valid `dab-config.json` with all DAB-eligible entities defined |

<!-- Target: keep this file under 80 lines. -->
