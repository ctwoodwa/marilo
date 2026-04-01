# Data Layer Migration — Workspace Context

Task routing and rules for migrating from an in-memory data store to SQL Server
using Microsoft DAB for auto-generated endpoints and EF Core for ad-hoc API data access.

## Task Routing

| Task Type | Go To Stage | Description |
|---|---|---|
| Classify endpoints as DAB-eligible or ad-hoc | `stages/01-endpoint-classification/` | The foundational split that drives all later stages |
| Design the SQL Server schema | `stages/02-schema-design/` | Map in-memory collections to relational tables |
| Configure Microsoft DAB | `stages/03-dab-setup/` | Generate dab-config.json, entity definitions, permissions |
| Set up EF Core for ad-hoc endpoints | `stages/04-ef-core-setup/` | DbContext, entity config, migrations, connection strings |
| Wire repositories and cut over from in-memory | `stages/05-repository-wiring/` | DI swap, integration test validation |

## Reference Materials

| Resource | Location | Contains |
|---|---|---|
| Migration config | `_config/migration-context.md` | Project paths, DB name, target environments |
| Endpoint classification | `_config/endpoint-map.md` | Per-endpoint: DAB / ad-hoc / remove decision |
| Schema ADRs | `_config/schema-decisions.md` | Design choices with rationale |
| DAB config syntax | `shared/dab-reference.md` | Entity, relationship, permission patterns |
| DAB starter config | `shared/dab-config-template.json` | Editable DAB configuration template |
| EF Core patterns | `shared/ef-core-patterns.md` | DbContext, entity config, migration commands |
| Stage outputs | `stages/*/output/` | Artifacts produced at each stage |

## Rules

1. Run stages 01 → 05 in order. Stage 01 output is required before any schema work begins.
2. DAB is the default choice. An endpoint moves to ad-hoc only if it requires business logic that cannot be expressed in DAB permissions or stored procedures.
3. EF Core owns all schema migrations. DAB reads from the migrated schema but never writes migrations.
4. Record every schema decision in `_config/schema-decisions.md` as an ADR entry.
5. Preserve existing API request/response contracts. If a shape must change, flag it as a breaking change and propose a versioning strategy.
6. Use `[slug]-*.md` naming for all stage output artifacts.
