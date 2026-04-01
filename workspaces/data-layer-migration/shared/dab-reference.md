# Microsoft DAB Reference

Quick reference for authoring `dab-config.json`. For full documentation see the
[DAB documentation](https://learn.microsoft.com/en-us/azure/data-api-builder/overview).

## Configuration File Structure

```json
{
  "$schema": "https://github.com/Azure/data-api-builder/releases/download/v1.x.x/dab.draft.schema.json",
  "data-source": {
    "database-type": "mssql",
    "connection-string": "@env('DATABASE_CONNECTION_STRING')"
  },
  "runtime": {
    "rest": {
      "enabled": true,
      "path": "/api"
    },
    "graphql": {
      "enabled": true,
      "path": "/graphql",
      "allow-introspection": true
    },
    "host": {
      "mode": "development",
      "cors": {
        "origins": ["http://localhost:5000"],
        "allow-credentials": false
      }
    }
  },
  "entities": {}
}
```

## Entity Definition

```json
"entities": {
  "EntityName": {
    "source": {
      "object": "dbo.TableName",
      "type": "table"
    },
    "rest": {
      "enabled": true,
      "path": "/entity-name-plural"
    },
    "graphql": {
      "enabled": true,
      "type": {
        "singular": "EntityName",
        "plural": "EntityNames"
      }
    },
    "permissions": [
      {
        "role": "anonymous",
        "actions": ["read"]
      },
      {
        "role": "authenticated",
        "actions": ["create", "read", "update", "delete"]
      }
    ]
  }
}
```

## Relationships

```json
"EntityName": {
  "relationships": {
    "RelatedEntities": {
      "cardinality": "many",
      "target.entity": "RelatedEntityName",
      "source.fields": ["Id"],
      "target.fields": ["EntityNameId"]
    }
  }
}
```

Cardinality options: `"one"` or `"many"`.

## Column Mappings

Use when DB column names differ from the desired API field names:

```json
"EntityName": {
  "mappings": {
    "db_column_name": "apiFieldName",
    "created_at_utc": "createdAt"
  }
}
```

## Permissions — Role Actions

| Action | REST equivalent | Notes |
|---|---|---|
| `read` | GET | Single item and list |
| `create` | POST | |
| `update` | PATCH / PUT | |
| `delete` | DELETE | |
| `execute` | POST (stored procedure) | Stored proc entities only |
| `*` | All of the above | |

## Field-Level Security

Restrict which fields a role can read or write:

```json
"permissions": [
  {
    "role": "authenticated",
    "actions": [
      {
        "action": "read",
        "fields": {
          "include": ["Id", "Name", "Status"],
          "exclude": ["InternalNotes"]
        }
      }
    ]
  }
]
```

## Stored Procedure Entity

```json
"RunReport": {
  "source": {
    "object": "dbo.usp_RunReport",
    "type": "stored-procedure",
    "parameters": {
      "StartDate": "2024-01-01",
      "EndDate": "2024-12-31"
    }
  },
  "rest": { "methods": ["POST"] },
  "graphql": { "operation": "mutation" },
  "permissions": [{ "role": "authenticated", "actions": ["execute"] }]
}
```

## Connection String — Environment Variable Reference

Always use `@env('VAR_NAME')` — never a literal value:

```json
"connection-string": "@env('DATABASE_CONNECTION_STRING')"
```

## Host Modes

| Mode | Use For |
|---|---|
| `development` | Local dev — enables introspection, detailed errors |
| `production` | Deployed environments — disables introspection |

## Running DAB Locally

```bash
# Install the CLI
dotnet tool install -g Microsoft.DataApiBuilder

# Initialize a new config
dab init --database-type mssql --connection-string "@env('DATABASE_CONNECTION_STRING')"

# Add an entity
dab add EntityName --source dbo.TableName --permissions "anonymous:read"

# Start DAB
dab start
```

## REST Endpoint Conventions

| Operation | Method | Path |
|---|---|---|
| List all | GET | `/api/entity-name` |
| Get by PK | GET | `/api/entity-name/PK_VALUE` |
| Create | POST | `/api/entity-name` |
| Update | PATCH | `/api/entity-name/PK_VALUE` |
| Replace | PUT | `/api/entity-name/PK_VALUE` |
| Delete | DELETE | `/api/entity-name/PK_VALUE` |
| Filter | GET | `/api/entity-name?$filter=Field eq 'value'` |
| Order | GET | `/api/entity-name?$orderby=Field desc` |
| Paginate | GET | `/api/entity-name?$first=10&$after=cursor` |
