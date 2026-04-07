# Data API Builder Integration Guide

How to set up Data API Builder (DAB) as the data access layer for Blazor applications migrated from .NET Framework. Use during Stage 02 (architecture) and Stage 03 (data layer migration).

## When to Use DAB vs. EF Core

| Scenario | Use DAB | Use EF Core | Notes |
|----------|---------|-------------|-------|
| Simple CRUD (list, get, create, update, delete) | Yes | No | DAB generates REST + GraphQL automatically |
| Filtering, sorting, pagination | Yes | No | DAB supports OData-style `$filter`, `$orderby`, `$first` |
| Complex business logic with transactions | No | Yes | DAB has no transaction support |
| Multi-table joins with custom projection | No | Yes | Use EF Core LINQ for complex queries |
| Stored procedures (simple execution) | Yes | Maybe | DAB exposes stored procs as entities |
| Stored procedures (complex result mapping) | No | Yes | Use `FromSqlRaw` for complex result sets |
| Reporting queries (aggregations, CTEs) | No | Yes | DAB has limited aggregation support |
| Bulk operations (insert/update many rows) | No | Yes | DAB handles single-entity operations |

## DAB Configuration Structure

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
      "cors": {
        "origins": ["http://localhost:5000"],
        "allow-credentials": true
      },
      "authentication": {
        "provider": "StaticWebApps"
      }
    }
  },
  "entities": {}
}
```

## Mapping EF6 Entities to DAB

For each EF6 entity (DbSet), decide: DAB entity or EF Core DbSet.

```
EF6 DbContext                    DAB config
─────────────                    ──────────
DbSet<Customer> Customers   -->  "Customer": { "source": { "object": "dbo.Customers", "type": "table" } }
DbSet<Order> Orders          -->  "Order": { "source": { "object": "dbo.Orders", "type": "table" } }
DbSet<OrderDetail> Details   -->  Keep in EF Core (complex joins, transactions)
```

## Mapping ADO.NET Queries to DAB

| ADO.NET Pattern | DAB Equivalent |
|----------------|----------------|
| `SELECT * FROM Table` | `GET /api/table-name` |
| `SELECT * FROM Table WHERE Id = @id` | `GET /api/table-name/Id/@id` |
| `SELECT * FROM Table WHERE Status = @s` | `GET /api/table-name?$filter=Status eq '@s'` |
| `SELECT * FROM Table ORDER BY Name` | `GET /api/table-name?$orderby=Name` |
| `INSERT INTO Table ...` | `POST /api/table-name` with JSON body |
| `UPDATE Table SET ... WHERE Id = @id` | `PATCH /api/table-name/Id/@id` with JSON body |
| `DELETE FROM Table WHERE Id = @id` | `DELETE /api/table-name/Id/@id` |
| `EXEC usp_GetReport @Start, @End` | `POST /api/run-report` (stored-proc entity) |

## DAB Entity with Relationships

```json
"Customer": {
  "source": { "object": "dbo.Customers", "type": "table" },
  "rest": { "enabled": true, "path": "/customers" },
  "graphql": {
    "enabled": true,
    "type": { "singular": "Customer", "plural": "Customers" }
  },
  "permissions": [
    { "role": "anonymous", "actions": ["read"] },
    { "role": "authenticated", "actions": ["create", "read", "update", "delete"] }
  ],
  "relationships": {
    "Orders": {
      "cardinality": "many",
      "target.entity": "Order",
      "source.fields": ["Id"],
      "target.fields": ["CustomerId"]
    }
  }
}
```

## Calling DAB from Blazor Components

### Service interface pattern

```csharp
public interface IDabApiService
{
    Task<List<T>> GetAllAsync<T>(string entityPath, string? filter = null);
    Task<T?> GetByIdAsync<T>(string entityPath, object id);
    Task<T> CreateAsync<T>(string entityPath, T entity);
    Task<T> UpdateAsync<T>(string entityPath, object id, T entity);
    Task DeleteAsync(string entityPath, object id);
}
```

### Registration in Program.cs

```csharp
builder.Services.AddHttpClient<IDabApiService, DabApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["DabBaseUrl"]!);
});
```

### Usage in a Blazor component

```razor
@inject IDabApiService DabApi

<TelerikGrid Data="@customers" Pageable="true" Sortable="true">
    <GridColumns>
        <GridColumn Field="@nameof(Customer.Name)" />
        <GridColumn Field="@nameof(Customer.Email)" />
    </GridColumns>
</TelerikGrid>

@code {
    private List<Customer> customers = new();
    protected override async Task OnInitializedAsync()
        => customers = await DabApi.GetAllAsync<Customer>("/customers");
}
```

## Running DAB Locally

```bash
# Install the CLI
dotnet tool install -g Microsoft.DataApiBuilder

# Initialize a new config
dab init --database-type mssql --connection-string "@env('DATABASE_CONNECTION_STRING')"

# Add an entity
dab add Customer --source dbo.Customers --permissions "anonymous:read"

# Start DAB
dab start
```

## Permissions Mapping

Map existing .NET Framework authorization to DAB roles:

| Framework Auth | DAB Permission |
|---------------|----------------|
| `[AllowAnonymous]` | `"role": "anonymous"` |
| `[Authorize]` | `"role": "authenticated"` |
| `[Authorize(Roles = "Admin")]` | `"role": "admin"` (custom role in DAB auth config) |
| `[Authorize(Roles = "ReadOnly")]` | Custom role with `["read"]` action only |

## DAB + Aspire Integration

When using .NET Aspire as the orchestrator:

```csharp
// In AppHost Program.cs
var dab = builder.AddDataAPIBuilder("dab")
    .WithReference(sqlServer);

var blazorApp = builder.AddProject<Projects.BlazorApp>("blazor")
    .WithReference(dab);
```
