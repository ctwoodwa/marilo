# Microsoft DAB Reference

Quick reference for authoring `dab-config.json` in the ASP-to-Blazor migration context. DAB auto-generates REST and GraphQL endpoints for standard CRUD operations. For full docs see the [DAB documentation](https://learn.microsoft.com/en-us/azure/data-api-builder/overview).

## When to Use DAB vs Custom API

| Use DAB | Use Custom API |
|---------|---------------|
| Single-table CRUD (list, get, create, update, delete) | Multi-step business logic |
| Filtered listing pages | Complex joins or aggregations |
| Paginated data grids (TelerikGrid) | Solver pipelines or batch operations |
| Simple stored procedure calls | COM object replacements with logic |
| Read-only lookup tables | Custom auth or session logic beyond role-based permissions |

## Configuration File Structure

```json
{
  "$schema": "https://github.com/Azure/data-api-builder/releases/download/v1.x.x/dab.draft.schema.json",
  "data-source": {
    "database-type": "mssql",
    "connection-string": "@env('DATABASE_CONNECTION_STRING')"
  },
  "runtime": {
    "rest": { "enabled": true, "path": "/api" },
    "graphql": { "enabled": true, "path": "/graphql", "allow-introspection": true },
    "host": {
      "mode": "development",
      "cors": { "origins": ["http://localhost:5000"], "allow-credentials": false }
    }
  },
  "entities": {}
}
```

Always use `@env('VAR_NAME')` for connection strings. Never use literal values.

## Entity Definition

```json
"Product": {
  "source": { "object": "dbo.Products", "type": "table" },
  "rest": { "enabled": true, "path": "/products" },
  "graphql": {
    "enabled": true,
    "type": { "singular": "Product", "plural": "Products" }
  },
  "permissions": [
    { "role": "anonymous", "actions": ["read"] },
    { "role": "authenticated", "actions": ["create", "read", "update", "delete"] }
  ]
}
```

## Relationships

```json
"Order": {
  "relationships": {
    "OrderItems": {
      "cardinality": "many",
      "target.entity": "OrderItem",
      "source.fields": ["Id"],
      "target.fields": ["OrderId"]
    }
  }
}
```

Cardinality options: `"one"` or `"many"`.

## Column Mappings

Use when DB column names differ from the desired API field names:

```json
"Product": {
  "mappings": {
    "product_name": "name",
    "created_at_utc": "createdAt",
    "is_active": "active"
  }
}
```

## Permissions per Role

| Action | REST Equivalent | Notes |
|--------|----------------|-------|
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

For complex operations that already exist as stored procedures:

```json
"RunReport": {
  "source": {
    "object": "dbo.usp_RunReport",
    "type": "stored-procedure",
    "parameters": { "StartDate": "2024-01-01", "EndDate": "2024-12-31" }
  },
  "rest": { "methods": ["POST"] },
  "graphql": { "operation": "mutation" },
  "permissions": [{ "role": "authenticated", "actions": ["execute"] }]
}
```

## REST Endpoint Conventions

| Operation | Method | Path |
|-----------|--------|------|
| List all | GET | `/api/entity-name` |
| Get by PK | GET | `/api/entity-name/PK_VALUE` |
| Create | POST | `/api/entity-name` |
| Update | PATCH | `/api/entity-name/PK_VALUE` |
| Replace | PUT | `/api/entity-name/PK_VALUE` |
| Delete | DELETE | `/api/entity-name/PK_VALUE` |
| Filter | GET | `/api/entity-name?$filter=Field eq 'value'` |
| Order | GET | `/api/entity-name?$orderby=Field desc` |
| Paginate | GET | `/api/entity-name?$first=10&$after=cursor` |

## Calling DAB from Blazor Components

Use `HttpClient` registered in `Program.cs` to call DAB endpoints:

```csharp
// Register a named HttpClient for DAB in Program.cs
builder.Services.AddHttpClient("DAB", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/api/");
});

// In a Blazor component or service
@inject IHttpClientFactory HttpClientFactory

var client = HttpClientFactory.CreateClient("DAB");
var products = await client.GetFromJsonAsync<List<Product>>("/api/products?$orderby=Name");

// Create
await client.PostAsJsonAsync("/api/products", newProduct);

// Update
await client.PatchAsJsonAsync($"/api/products/{id}", updatedFields);

// Delete
await client.DeleteAsync($"/api/products/{id}");
```

For TelerikGrid with server-side operations, use the OnRead event to call DAB with `$filter`, `$orderby`, and `$first`/`$after` parameters built from the grid state.

## Host Modes

| Mode | Use For |
|------|---------|
| `development` | Local dev with introspection and detailed errors |
| `production` | Deployed environments with introspection disabled |

## Running DAB Locally

```bash
# Install the CLI
dotnet tool install -g Microsoft.DataApiBuilder

# Initialize a new config
dab init --database-type mssql --connection-string "@env('DATABASE_CONNECTION_STRING')"

# Add an entity
dab add Product --source dbo.Products --permissions "anonymous:read"

# Start DAB
dab start
```
