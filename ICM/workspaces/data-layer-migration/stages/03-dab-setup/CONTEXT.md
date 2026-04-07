# Stage 03 - DAB Setup

Produce a complete Microsoft DAB configuration file that exposes the DAB-classified entities
as REST and GraphQL endpoints, with correct permissions and relationship traversal.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Endpoint map | `../../_config/endpoint-map.md` | DAB entity summary | Defines which entities to expose and how |
| Schema definition | `../02-schema-design/output/[slug]-schema.sql` | Table and column names | Source of truth for entity field names |
| Migration config | `../../_config/migration-context.md` | DAB config path, REST base route, GraphQL endpoint | Output destination and routing |
| DAB reference | `../../shared/dab-reference.md` | Config syntax | Authoring guide |
| DAB config template | `../../shared/dab-config-template.json` | Starter structure | Starting point |

## Process

1. For each entity in the DAB entity summary, define a DAB entity block:
   - `source.object` → the SQL table name
   - `rest.path` → REST route segment (derive from entity name, lowercase-hyphenated)
   - `graphql.type` → GraphQL type name (PascalCase entity name)
   - `permissions` → at minimum: anonymous read if public, authenticated read/write for protected entities
2. Define relationships between entities using DAB `relationships` blocks. Use the FK definitions from the schema.
3. Define any column mappings where the database column name differs from the desired API field name.
4. Configure the data-source connection string reference (use an environment variable reference, not a literal value).
5. Set the host mode (`development` for local, `production` for deployed).
6. Write the complete `dab-config.json` to `{{DAB_CONFIG_PATH}}`.
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| DAB configuration file | `{{DAB_CONFIG_PATH}}/dab-config.json` | JSON |
| DAB entity index | `output/[slug]-dab-entities.md` | Table: entity, REST path, GraphQL type, permissions |
| DAB startup instructions | `output/[slug]-dab-startup.md` | How to run DAB locally and in each environment |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 1 | Entity block definitions (before relationships) | Confirm REST paths and GraphQL type names |
| 3 | Relationship definitions | Approve traversal directions and field names |
| 6 | Complete dab-config.json | Final review before writing to target path |

## Audit

| Check | Pass Condition |
|---|---|
| All DAB entities defined | Every entity in the DAB entity summary has a config block |
| REST paths are lowercase-hyphenated | No camelCase or PascalCase in REST path segments |
| GraphQL types are PascalCase | Consistent with GraphQL naming conventions |
| Permissions defined | Every entity has at least one permission rule |
| Relationships defined | All FK relationships are traversable in both directions |
| Connection string is a reference | No literal connection string in the config file |
