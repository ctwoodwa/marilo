# Stage 04 - EF Core Setup

Scaffold the EF Core DbContext, entity configurations, and initial migration for the ad-hoc
API endpoints. EF Core owns schema migrations for the entire database — DAB reads from this schema.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Schema definition | `../02-schema-design/output/[slug]-schema.sql` | All tables | Entity configuration source |
| EF Core entity reference | `../02-schema-design/output/[slug]-ef-entity-reference.md` | All entities | Column mappings and relationship config |
| Endpoint map | `../../_config/endpoint-map.md` | Ad-hoc endpoint summary | Scopes which entities the API DbContext needs |
| Migration config | `../../_config/migration-context.md` | DbContext name, project path, migrations folder | Output destinations |
| EF Core patterns | `../../shared/ef-core-patterns.md` | Configuration patterns | Authoring guide |

## Process

1. Define C# entity classes for all tables (even those exposed only by DAB — EF Core owns the full schema).
2. Create the DbContext class. Register only the DbSets needed by ad-hoc endpoints; include all entities in `OnModelCreating` for migration completeness.
3. Write `IEntityTypeConfiguration<T>` classes for each entity: table name, column types, nullable constraints, FKs, and indexes.
4. Add the connection string to `appsettings.json` (dev) using an environment variable reference pattern.
5. Register the DbContext in the DI container with appropriate lifetime (Scoped).
6. Generate the initial EF Core migration: `dotnet ef migrations add InitialCreate`.
7. Create a `IDbSeeder` (or equivalent) to migrate seed data from the in-memory store to the database.
8. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Entity classes | `{{API_PROJECT_PATH}}/Data/Entities/` | C# source files |
| DbContext class | `{{API_PROJECT_PATH}}/Data/` | C# source file |
| Entity configurations | `{{API_PROJECT_PATH}}/Data/Configurations/` | C# source files |
| Initial migration | `{{MIGRATIONS_FOLDER}}/` | EF Core migration files |
| Seed script | `{{API_PROJECT_PATH}}/Data/Seeding/` | C# source file |
| EF Core setup summary | `output/[slug]-ef-setup.md` | What was generated, migration commands, DI registration snippet |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 3 | Entity configurations (before migration) | Approve column types and constraints |
| 6 | Migration file content | Confirm the generated SQL matches Stage 02 schema |
| 7 | Seed data mapping | Verify in-memory seed data maps correctly to relational rows |

## Audit

| Check | Pass Condition |
|---|---|
| All tables have entity classes | Every table in the schema has a C# entity class |
| DbContext compiles | No unresolved references in DbContext or entity configs |
| Migration generated | `InitialCreate` migration exists and is not empty |
| Migration SQL matches schema | Generated migration SQL aligns with Stage 02 schema definition |
| Seed data complete | All in-memory seed collections have a seeder equivalent |
| Connection string is a reference | No literal connection string in any committed file |
