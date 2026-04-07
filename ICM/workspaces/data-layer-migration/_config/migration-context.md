# Migration Context

Filled during `setup`. Replace all `{{PLACEHOLDERS}}` before running stages.

## Identity

| Field | Value |
|---|---|
| Project name | `{{PROJECT_NAME}}` |
| Project slug | `{{PROJECT_SLUG}}` |
| Solution path | `{{SOLUTION_PATH}}` |
| API project path | `{{API_PROJECT_PATH}}` |

## Database

| Field | Value |
|---|---|
| Database name | `{{DATABASE_NAME}}` |
| SQL Server instance (dev) | `{{SQL_SERVER_DEV}}` |
| SQL Server instance (test) | `{{SQL_SERVER_TEST}}` |
| SQL Server instance (prod) | `{{SQL_SERVER_PROD}}` |
| Schema name | `{{SCHEMA_NAME}}` |

> Default schema: `dbo`. Override if the project uses a custom schema.

## In-Memory Store

| Field | Value |
|---|---|
| In-memory store class | `{{IN_MEMORY_STORE_CLASS}}` |
| In-memory store path | `{{IN_MEMORY_STORE_PATH}}` |
| Number of collections | `{{COLLECTION_COUNT}}` |

## DAB

| Field | Value |
|---|---|
| DAB config output path | `{{DAB_CONFIG_PATH}}` |
| DAB REST base route | `{{DAB_REST_BASE_ROUTE}}` |
| DAB GraphQL endpoint | `{{DAB_GRAPHQL_ENDPOINT}}` |
| DAB host URL (dev) | `{{DAB_HOST_URL_DEV}}` |

> Defaults: REST base route `/api`, GraphQL endpoint `/graphql`.

## EF Core

| Field | Value |
|---|---|
| DbContext name | `{{DB_CONTEXT_NAME}}` |
| DbContext project path | `{{DB_CONTEXT_PROJECT_PATH}}` |
| Migrations folder | `{{MIGRATIONS_FOLDER}}` |

## Notes

`{{MIGRATION_NOTES}}`
