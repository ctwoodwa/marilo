# Stage 02 - Schema Design

Design the SQL Server relational schema from the classified entity model. Produce a migration-ready
schema definition and record all significant design decisions as ADRs.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| In-memory model inventory | `../01-endpoint-classification/output/[slug]-inmemory-model.md` | All entities and relationships | Source of truth for the schema |
| Endpoint map | `../../_config/endpoint-map.md` | DAB entity summary | Confirms which entities need REST/GraphQL exposure |
| Migration config | `../../_config/migration-context.md` | Database name, schema name | Naming context |

## Process

1. For each entity in the in-memory model, propose a table definition: column names, types, nullability, and primary key strategy.
2. Identify relationships (one-to-many, many-to-many) and define foreign keys.
3. Identify columns that need indexes: foreign keys, common filter fields, sort fields exposed via DAB.
4. Design the audit pattern: created/modified timestamps and user columns on mutable entities.
5. Flag any normalization decisions (e.g., enum-as-lookup-table vs enum-as-int column) and record each as an ADR in `_config/schema-decisions.md`.
6. Produce a CREATE TABLE script and an EF Core entity configuration reference.
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Schema definition | `output/[slug]-schema.sql` | SQL CREATE TABLE statements |
| EF Core entity reference | `output/[slug]-ef-entity-reference.md` | Per-entity: table name, columns, relationships, index notes |
| Schema ADRs | `_config/schema-decisions.md` | Updated with decisions from this stage |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 3 | Proposed table definitions with relationships | Approve or amend before FK and index work |
| 5 | ADR entries for normalization and design choices | Sign off on each ADR before SQL script is finalized |

## Audit

| Check | Pass Condition |
|---|---|
| All entities covered | Every entity from the in-memory model has a table definition |
| PKs defined | Every table has a primary key strategy (identity int or UUID — document the choice) |
| FKs defined | All relationships have explicit foreign key constraints |
| Audit columns present | Created/modified timestamps on all mutable tables |
| ADRs recorded | Every non-obvious design choice has an ADR entry |
| Output naming | SQL file and entity reference match `output/[slug]-*.sql` and `output/[slug]-*.md` |
