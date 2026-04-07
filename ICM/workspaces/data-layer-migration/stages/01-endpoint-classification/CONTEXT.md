# Stage 01 - Endpoint Classification

Read every endpoint in the API service and classify each as DAB-eligible, ad-hoc, or remove.
This classification drives the entire migration — schema design, DAB config, and EF Core scope
are all determined by the decisions made here.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Migration config | `../../_config/migration-context.md` | API project path, in-memory store path | Defines what to scan |
| In-memory store source | `{{IN_MEMORY_STORE_PATH}}` | All collections and seed data | Reveals the full data model |
| API endpoint files | `{{API_PROJECT_PATH}}/Endpoints/` | All endpoint definitions | Full current API surface |

## Classification Rules

Apply these rules in order. The first matching rule wins.

| Rule | Classification |
|---|---|
| Endpoint is a GET, POST, PUT, DELETE on a single entity type with no business logic | `dab` |
| Endpoint chains two or more domain operations (e.g., prioritize → schedule) | `ad-hoc` |
| Endpoint executes a solver, calculator, or projection | `ad-hoc` |
| Endpoint manages authentication, sessions, or tokens | `ad-hoc` |
| Endpoint reads data + applies filtering/sorting/pagination only | `dab` |
| Endpoint writes data + validates/transforms before persisting | `dab+ad-hoc` |
| Endpoint is duplicated by another or unused | `remove` |

## Process

1. Read `_config/migration-context.md` and confirm `{{API_PROJECT_PATH}}` and `{{IN_MEMORY_STORE_PATH}}` are filled.
2. List all in-memory collections. For each, note the entity name, fields, and relationships.
3. List all API endpoints. For each, record: HTTP method, route, what data it reads/writes, and any business logic present.
4. Apply the classification rules to each endpoint. Flag any ambiguous cases for human review.
5. Populate `_config/endpoint-map.md` — classification table, DAB entity summary, and ad-hoc summary.
6. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| In-memory model inventory | `output/[slug]-inmemory-model.md` | Table: collection, entity fields, relationships |
| Classified endpoint list | `_config/endpoint-map.md` | Filled classification table (all three sections) |
| Classification summary | `output/[slug]-classification-summary.md` | Counts by decision, ambiguous cases flagged |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 2 | In-memory model inventory | Confirm the model is complete before classifying endpoints |
| 4 | Ambiguous endpoints with proposed classification | Override or approve each case |
| 5 | Full endpoint-map.md | Sign off before Stage 02 begins |

## Audit

| Check | Pass Condition |
|---|---|
| Every endpoint classified | No endpoint in the API has an empty Decision column |
| DAB entity summary populated | All `dab` endpoints have a corresponding entity row |
| Ad-hoc summary populated | All `ad-hoc` endpoints have a business logic summary |
| Ambiguous cases resolved | No open flags remain after checkpoint |
