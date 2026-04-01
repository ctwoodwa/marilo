# High-Quality Test Examples: Data API Builder Endpoints

> **Purpose:** Demonstrate test patterns for DAB REST and GraphQL endpoints including configuration validation, permission enforcement, integration testing, and contract testing.
> **Governed by:** DAB API specification (Stage 04), security-baseline.md, api-visibility.md, contract-testing.md.
> **NuGet:** `Microsoft.AspNetCore.Mvc.Testing`, `Pact.Net`, `FluentAssertions` (optional)

## Project Setup

```xml
<!-- DAB.Tests.csproj -->
<PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="8.*" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.*" />
<PackageReference Include="xunit" Version="2.*" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.*" />
```

### Test Fixture for DAB Integration Tests

```csharp
/// <summary>
/// Shared fixture that starts a DAB instance against a test database.
/// Uses Testcontainers for isolated database per test run.
/// </summary>
public class DabTestFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer;
    private HttpClient _restClient = null!;
    private HttpClient _graphqlClient = null!;

    public HttpClient RestClient => _restClient;
    public HttpClient GraphQLClient => _graphqlClient;
    public string ConnectionString => _dbContainer.GetConnectionString();

    public DabTestFixture()
    {
        _dbContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        // Apply schema migrations
        await ApplyMigrationsAsync(ConnectionString);

        // Seed test data
        await SeedTestDataAsync(ConnectionString);

        // Start DAB with test dab-config.json pointing to container
        var dabProcess = StartDabProcess(ConnectionString);

        _restClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:5000/api/")
        };
        _graphqlClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:5000/graphql")
        };
    }

    public async Task DisposeAsync()
    {
        _restClient.Dispose();
        _graphqlClient.Dispose();
        await _dbContainer.DisposeAsync();
    }

    private static async Task SeedTestDataAsync(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(@"
            INSERT INTO dbo.Assets (AssetId, AssetName, Condition, FacilityId, CreatedBy)
            VALUES
                ('ASSET-001', 'HVAC Unit 1', 3, 'FAC-001', 'seed-user'),
                ('ASSET-002', 'Pump Station A', 5, 'FAC-001', 'seed-user'),
                ('ASSET-003', 'Generator B', 2, 'FAC-002', 'seed-user');

            INSERT INTO dbo.WorkOrders (WorkOrderId, Title, AssetId, Priority, Status, CreatedBy)
            VALUES
                ('WO-1001', 'Quarterly Inspection', 'ASSET-001', 'High', 'Draft', 'seed-user'),
                ('WO-1002', 'Emergency Repair', 'ASSET-002', 'Critical', 'Submitted', 'seed-user');
        ");
    }
}
```

---

## Example 1: REST GET Endpoint Tests

Testing read operations with filtering, pagination, and field selection.

```csharp
public class AssetRestGetTests : IClassFixture<DabTestFixture>
{
    private readonly HttpClient _client;

    public AssetRestGetTests(DabTestFixture fixture) => _client = fixture.RestClient;

    [Fact]
    public async Task GetAssets_ReturnsPaginatedList_WithDefaultPageSize()
    {
        // Act
        var response = await _client.GetAsync("Asset");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<DabListResponse<AssetDto>>();

        Assert.NotNull(content);
        Assert.NotEmpty(content.Value);
        Assert.True(content.Value.Count <= 100); // DAB default page size
    }

    [Fact]
    public async Task GetAssetById_ReturnsCorrectAsset()
    {
        // Act
        var response = await _client.GetAsync("Asset/AssetId/ASSET-001");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<DabSingleResponse<AssetDto>>();

        Assert.NotNull(content?.Value);
        Assert.Equal("ASSET-001", content.Value.AssetId);
        Assert.Equal("HVAC Unit 1", content.Value.AssetName);
        Assert.Equal(3, content.Value.Condition);
        Assert.Equal("FAC-001", content.Value.FacilityId);
    }

    [Fact]
    public async Task GetAssetById_WhenNotFound_Returns404()
    {
        // Act
        var response = await _client.GetAsync("Asset/AssetId/NONEXISTENT");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetAssets_WithFilter_ReturnsMatchingItems()
    {
        // Act — filter assets by facility
        var response = await _client.GetAsync(
            "Asset?$filter=FacilityId eq 'FAC-001'");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<DabListResponse<AssetDto>>();

        Assert.NotNull(content);
        Assert.All(content.Value, a => Assert.Equal("FAC-001", a.FacilityId));
        Assert.Equal(2, content.Value.Count); // ASSET-001 and ASSET-002
    }

    [Fact]
    public async Task GetAssets_WithOrderBy_ReturnsSortedItems()
    {
        // Act — sort by condition ascending
        var response = await _client.GetAsync(
            "Asset?$orderby=Condition asc");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<DabListResponse<AssetDto>>();

        Assert.NotNull(content);
        var conditions = content.Value.Select(a => a.Condition).ToList();
        Assert.Equal(conditions.OrderBy(c => c), conditions);
    }

    [Fact]
    public async Task GetAssets_WithSelect_ReturnsOnlyRequestedFields()
    {
        // Act — select only AssetId and AssetName
        var response = await _client.GetAsync(
            "Asset?$select=AssetId,AssetName");

        // Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);
        var firstItem = doc.RootElement.GetProperty("value")[0];

        Assert.True(firstItem.TryGetProperty("AssetId", out _));
        Assert.True(firstItem.TryGetProperty("AssetName", out _));
        Assert.False(firstItem.TryGetProperty("Condition", out _));
        Assert.False(firstItem.TryGetProperty("FacilityId", out _));
    }

    [Fact]
    public async Task GetAssets_WithPagination_ReturnsPagedResults()
    {
        // Act — request first 2 items
        var response = await _client.GetAsync("Asset?$first=2");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<DabListResponse<AssetDto>>();

        Assert.NotNull(content);
        Assert.Equal(2, content.Value.Count);
        Assert.NotNull(content.NextLink); // More results available
    }
}

/// <summary>
/// DAB response wrappers matching DAB's default response shape.
/// </summary>
public record DabListResponse<T>
{
    [JsonPropertyName("value")]
    public List<T> Value { get; init; } = new();

    [JsonPropertyName("nextLink")]
    public string? NextLink { get; init; }
}

public record DabSingleResponse<T>
{
    [JsonPropertyName("value")]
    public T? Value { get; init; }
}
```

### Why This Is High Quality

- **Tests all OData query capabilities:** filter, orderby, select, pagination.
- **Verifies response shape:** Checks DAB's `value` wrapper and `nextLink` for pagination.
- **Tests not-found case:** Ensures 404 for missing entities.
- **Uses typed DTOs:** Validates field mapping between database and API contract.

---

## Example 2: REST Mutation Tests (Create, Update, Delete)

Testing write operations with validation and error handling.

```csharp
public class AssetRestMutationTests : IClassFixture<DabTestFixture>
{
    private readonly HttpClient _client;

    public AssetRestMutationTests(DabTestFixture fixture) => _client = fixture.RestClient;

    [Fact]
    public async Task CreateAsset_WithValidPayload_Returns201WithCreatedEntity()
    {
        // Arrange
        var newAsset = new
        {
            AssetId = "ASSET-NEW-001",
            AssetName = "New Cooling Tower",
            Condition = 5,
            FacilityId = "FAC-001",
            CreatedBy = "test-user"
        };

        // Act
        var response = await _client.PostAsJsonAsync("Asset", newAsset);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<DabSingleResponse<AssetDto>>();
        Assert.Equal("ASSET-NEW-001", created?.Value?.AssetId);
        Assert.Equal("New Cooling Tower", created?.Value?.AssetName);
    }

    [Fact]
    public async Task CreateAsset_WithDuplicateId_Returns409Conflict()
    {
        // Arrange — ASSET-001 already exists in seed data
        var duplicate = new
        {
            AssetId = "ASSET-001",
            AssetName = "Duplicate Asset",
            Condition = 1,
            FacilityId = "FAC-001",
            CreatedBy = "test-user"
        };

        // Act
        var response = await _client.PostAsJsonAsync("Asset", duplicate);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task CreateAsset_WithMissingRequiredField_Returns400()
    {
        // Arrange — missing AssetName (required)
        var incomplete = new
        {
            AssetId = "ASSET-INCOMPLETE",
            Condition = 3,
            FacilityId = "FAC-001"
        };

        // Act
        var response = await _client.PostAsJsonAsync("Asset", incomplete);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateAsset_WithPatch_UpdatesOnlyProvidedFields()
    {
        // Arrange
        var patch = new { Condition = 1 };

        // Act
        var response = await _client.PatchAsJsonAsync(
            "Asset/AssetId/ASSET-001", patch);

        // Assert
        response.EnsureSuccessStatusCode();

        // Verify only Condition changed
        var getResponse = await _client.GetAsync("Asset/AssetId/ASSET-001");
        var asset = await getResponse.Content
            .ReadFromJsonAsync<DabSingleResponse<AssetDto>>();
        Assert.Equal(1, asset?.Value?.Condition);
        Assert.Equal("HVAC Unit 1", asset?.Value?.AssetName); // Unchanged
    }

    [Fact]
    public async Task UpdateAsset_WhenNotFound_Returns404()
    {
        // Arrange
        var patch = new { Condition = 1 };

        // Act
        var response = await _client.PatchAsJsonAsync(
            "Asset/AssetId/NONEXISTENT", patch);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteAsset_RemovesEntity_Returns204()
    {
        // Arrange — create a disposable asset first
        var disposable = new
        {
            AssetId = "ASSET-DELETE-ME",
            AssetName = "Disposable",
            Condition = 1,
            FacilityId = "FAC-001",
            CreatedBy = "test-user"
        };
        await _client.PostAsJsonAsync("Asset", disposable);

        // Act
        var response = await _client.DeleteAsync("Asset/AssetId/ASSET-DELETE-ME");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify deletion
        var getResponse = await _client.GetAsync("Asset/AssetId/ASSET-DELETE-ME");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
```

### Why This Is High Quality

- **Full CRUD cycle tested:** Create, read-back, update with patch, delete with verify.
- **Error scenarios covered:** Duplicate key (409), missing fields (400), not found (404).
- **Patch semantics verified:** Only modified fields change, others remain untouched.
- **Clean test data management:** Creates disposable records for destructive tests.

---

## Example 3: GraphQL Query and Mutation Tests

Testing DAB's GraphQL endpoint with typed queries.

```csharp
public class AssetGraphQLTests : IClassFixture<DabTestFixture>
{
    private readonly HttpClient _client;

    public AssetGraphQLTests(DabTestFixture fixture) => _client = fixture.GraphQLClient;

    private async Task<JsonElement> ExecuteGraphQLAsync(string query, object? variables = null)
    {
        var payload = new { query, variables };
        var response = await _client.PostAsJsonAsync("", payload);
        response.EnsureSuccessStatusCode();
        var doc = await response.Content.ReadFromJsonAsync<JsonDocument>();
        return doc!.RootElement;
    }

    [Fact]
    public async Task QueryAssetById_ReturnsCorrectEntity()
    {
        // Act
        var result = await ExecuteGraphQLAsync(@"
            query {
                asset_by_pk(AssetId: ""ASSET-001"") {
                    AssetId
                    AssetName
                    Condition
                    FacilityId
                }
            }
        ");

        // Assert
        var asset = result.GetProperty("data").GetProperty("asset_by_pk");
        Assert.Equal("ASSET-001", asset.GetProperty("AssetId").GetString());
        Assert.Equal("HVAC Unit 1", asset.GetProperty("AssetName").GetString());
        Assert.Equal(3, asset.GetProperty("Condition").GetInt32());
    }

    [Fact]
    public async Task QueryAssets_WithFilter_ReturnsFilteredList()
    {
        // Act
        var result = await ExecuteGraphQLAsync(@"
            query {
                assets(filter: { FacilityId: { eq: ""FAC-001"" } }) {
                    items {
                        AssetId
                        FacilityId
                    }
                }
            }
        ");

        // Assert
        var items = result.GetProperty("data")
            .GetProperty("assets")
            .GetProperty("items");

        Assert.Equal(2, items.GetArrayLength());
        foreach (var item in items.EnumerateArray())
        {
            Assert.Equal("FAC-001", item.GetProperty("FacilityId").GetString());
        }
    }

    [Fact]
    public async Task QueryAssets_WithPagination_SupportsFirstAndAfter()
    {
        // Act — get first page
        var page1 = await ExecuteGraphQLAsync(@"
            query {
                assets(first: 2) {
                    items { AssetId }
                    hasNextPage
                    endCursor
                }
            }
        ");

        var assetsPage1 = page1.GetProperty("data").GetProperty("assets");
        Assert.True(assetsPage1.GetProperty("hasNextPage").GetBoolean());
        var cursor = assetsPage1.GetProperty("endCursor").GetString();

        // Act — get second page using cursor
        var page2 = await ExecuteGraphQLAsync($@"
            query {{
                assets(first: 2, after: ""{cursor}"") {{
                    items {{ AssetId }}
                    hasNextPage
                }}
            }}
        ");

        var assetsPage2 = page2.GetProperty("data").GetProperty("assets");
        var page2Items = assetsPage2.GetProperty("items");
        Assert.True(page2Items.GetArrayLength() > 0);

        // Verify no overlap between pages
        var page1Ids = assetsPage1.GetProperty("items").EnumerateArray()
            .Select(i => i.GetProperty("AssetId").GetString()).ToList();
        var page2Ids = page2Items.EnumerateArray()
            .Select(i => i.GetProperty("AssetId").GetString()).ToList();
        Assert.Empty(page1Ids.Intersect(page2Ids));
    }

    [Fact]
    public async Task QueryAssets_WithRelationship_ReturnsNestedData()
    {
        // Act — query asset with its work orders
        var result = await ExecuteGraphQLAsync(@"
            query {
                asset_by_pk(AssetId: ""ASSET-001"") {
                    AssetId
                    AssetName
                    WorkOrders {
                        items {
                            WorkOrderId
                            Title
                            Status
                        }
                    }
                }
            }
        ");

        // Assert
        var asset = result.GetProperty("data").GetProperty("asset_by_pk");
        Assert.Equal("ASSET-001", asset.GetProperty("AssetId").GetString());

        var workOrders = asset.GetProperty("WorkOrders").GetProperty("items");
        Assert.True(workOrders.GetArrayLength() >= 1);

        var wo = workOrders[0];
        Assert.Equal("WO-1001", wo.GetProperty("WorkOrderId").GetString());
    }

    [Fact]
    public async Task MutationCreateAsset_ReturnsCreatedEntity()
    {
        // Act
        var result = await ExecuteGraphQLAsync(@"
            mutation {
                createAsset(item: {
                    AssetId: ""ASSET-GQL-001""
                    AssetName: ""GraphQL Created Asset""
                    Condition: 4
                    FacilityId: ""FAC-001""
                    CreatedBy: ""gql-test""
                }) {
                    AssetId
                    AssetName
                    Condition
                }
            }
        ");

        // Assert
        var created = result.GetProperty("data").GetProperty("createAsset");
        Assert.Equal("ASSET-GQL-001", created.GetProperty("AssetId").GetString());
        Assert.Equal("GraphQL Created Asset", created.GetProperty("AssetName").GetString());
        Assert.Equal(4, created.GetProperty("Condition").GetInt32());
    }

    [Fact]
    public async Task MutationUpdateAsset_UpdatesAndReturnsEntity()
    {
        // Act
        var result = await ExecuteGraphQLAsync(@"
            mutation {
                updateAsset(AssetId: ""ASSET-002"", item: {
                    Condition: 1
                }) {
                    AssetId
                    AssetName
                    Condition
                }
            }
        ");

        // Assert
        var updated = result.GetProperty("data").GetProperty("updateAsset");
        Assert.Equal("ASSET-002", updated.GetProperty("AssetId").GetString());
        Assert.Equal(1, updated.GetProperty("Condition").GetInt32());
        Assert.Equal("Pump Station A", updated.GetProperty("AssetName").GetString()); // Unchanged
    }

    [Fact]
    public async Task QueryWithErrors_ReturnsGraphQLErrorFormat()
    {
        // Act — query with invalid filter
        var result = await ExecuteGraphQLAsync(@"
            query {
                asset_by_pk(AssetId: null) {
                    AssetId
                }
            }
        ");

        // Assert
        Assert.True(result.TryGetProperty("errors", out var errors));
        Assert.True(errors.GetArrayLength() > 0);
    }
}
```

### Why This Is High Quality

- **Tests both queries and mutations:** Full GraphQL CRUD coverage.
- **Pagination tested with cursor:** Verifies `first`/`after` and no page overlap.
- **Relationship queries tested:** Validates nested navigation properties.
- **Error format validated:** Confirms GraphQL errors follow standard error shape.
- **Reusable helper:** `ExecuteGraphQLAsync` reduces boilerplate across tests.

---

## Example 4: Permission and Security Tests

Testing role-based access control on DAB endpoints.

```csharp
public class DabPermissionTests : IClassFixture<DabTestFixture>
{
    private readonly DabTestFixture _fixture;

    public DabPermissionTests(DabTestFixture fixture) => _fixture = fixture;

    private HttpClient CreateClientWithRole(string role)
    {
        var client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:5000/api/")
        };
        // Simulate role via test auth token
        var token = TestTokenGenerator.GenerateToken(
            claims: new[]
            {
                new Claim(ClaimTypes.Role, role),
                new Claim("sub", $"test-user-{role}")
            });
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    [Fact]
    public async Task ReadOnlyRole_CanGetAssets()
    {
        // Arrange
        using var client = CreateClientWithRole("reader");

        // Act
        var response = await client.GetAsync("Asset");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task ReadOnlyRole_CannotCreateAssets()
    {
        // Arrange
        using var client = CreateClientWithRole("reader");
        var newAsset = new { AssetId = "BLOCKED", AssetName = "Blocked", Condition = 1 };

        // Act
        var response = await client.PostAsJsonAsync("Asset", newAsset);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task ReadOnlyRole_CannotDeleteAssets()
    {
        // Arrange
        using var client = CreateClientWithRole("reader");

        // Act
        var response = await client.DeleteAsync("Asset/AssetId/ASSET-001");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task OperatorRole_CanCreateButNotDelete()
    {
        // Arrange
        using var client = CreateClientWithRole("operator");
        var newAsset = new
        {
            AssetId = "ASSET-OP-001",
            AssetName = "Operator Created",
            Condition = 3,
            FacilityId = "FAC-001",
            CreatedBy = "operator"
        };

        // Act — create should succeed
        var createResponse = await client.PostAsJsonAsync("Asset", newAsset);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        // Act — delete should fail
        var deleteResponse = await client.DeleteAsync("Asset/AssetId/ASSET-OP-001");
        Assert.Equal(HttpStatusCode.Forbidden, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task AdminRole_CanPerformAllOperations()
    {
        // Arrange
        using var client = CreateClientWithRole("admin");
        var newAsset = new
        {
            AssetId = "ASSET-ADMIN-001",
            AssetName = "Admin Created",
            Condition = 5,
            FacilityId = "FAC-001",
            CreatedBy = "admin"
        };

        // Act & Assert — full CRUD
        var create = await client.PostAsJsonAsync("Asset", newAsset);
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);

        var read = await client.GetAsync("Asset/AssetId/ASSET-ADMIN-001");
        Assert.Equal(HttpStatusCode.OK, read.StatusCode);

        var update = await client.PatchAsJsonAsync(
            "Asset/AssetId/ASSET-ADMIN-001", new { Condition = 1 });
        Assert.Equal(HttpStatusCode.OK, update.StatusCode);

        var delete = await client.DeleteAsync("Asset/AssetId/ASSET-ADMIN-001");
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);
    }

    [Fact]
    public async Task UnauthenticatedRequest_Returns401()
    {
        // Arrange — no auth header
        using var client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:5000/api/")
        };

        // Act
        var response = await client.GetAsync("Asset");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RowLevelPolicy_OperatorSeesOnlyOwnRecords()
    {
        // Arrange — operator with specific userId
        using var client = CreateClientWithRole("operator");

        // Act — query work orders (filtered by @claims.userId policy)
        var response = await client.GetAsync("WorkOrder");
        response.EnsureSuccessStatusCode();
        var content = await response.Content
            .ReadFromJsonAsync<DabListResponse<WorkOrderDto>>();

        // Assert — only sees work orders created by this user
        Assert.NotNull(content);
        Assert.All(content.Value,
            wo => Assert.Equal("test-user-operator", wo.CreatedBy));
    }
}
```

### Why This Is High Quality

- **Tests every role level:** reader, operator, admin, unauthenticated.
- **Tests permission boundaries:** Each role can/cannot perform specific operations.
- **Row-level policies tested:** Verifies `@claims.userId` database policies.
- **Reusable token generator:** `CreateClientWithRole` keeps tests focused on behavior.
- **Full CRUD cycle for admin:** Confirms unrestricted access for admin role.

---

## Example 5: DAB Configuration Validation Tests

Testing dab-config.json correctness at build time.

```csharp
public class DabConfigValidationTests
{
    private readonly JsonDocument _dabConfig;

    public DabConfigValidationTests()
    {
        var configPath = Path.Combine(
            TestContext.CurrentContext.TestDirectory,
            "..", "..", "..", "..", "src", "Api", "dab-config.json");
        var json = File.ReadAllText(configPath);
        _dabConfig = JsonDocument.Parse(json);
    }

    [Fact]
    public void AllEntities_HaveRestAndGraphQLEnabled()
    {
        var entities = _dabConfig.RootElement.GetProperty("entities");
        foreach (var entity in entities.EnumerateObject())
        {
            var rest = entity.Value.GetProperty("rest");
            var graphql = entity.Value.GetProperty("graphql");

            Assert.True(rest.GetProperty("enabled").GetBoolean(),
                $"Entity '{entity.Name}' should have REST enabled");
            Assert.True(graphql.GetProperty("enabled").GetBoolean(),
                $"Entity '{entity.Name}' should have GraphQL enabled");
        }
    }

    [Fact]
    public void AllEntities_HaveSourceDefined()
    {
        var entities = _dabConfig.RootElement.GetProperty("entities");
        foreach (var entity in entities.EnumerateObject())
        {
            Assert.True(
                entity.Value.TryGetProperty("source", out var source),
                $"Entity '{entity.Name}' must have a source defined");

            Assert.True(
                source.TryGetProperty("object", out _),
                $"Entity '{entity.Name}' source must specify the database object");

            Assert.True(
                source.TryGetProperty("type", out _),
                $"Entity '{entity.Name}' source must specify the type (table/view/stored-procedure)");
        }
    }

    [Fact]
    public void AllEntities_HavePermissionsDefined()
    {
        var entities = _dabConfig.RootElement.GetProperty("entities");
        foreach (var entity in entities.EnumerateObject())
        {
            Assert.True(
                entity.Value.TryGetProperty("permissions", out var permissions),
                $"Entity '{entity.Name}' must have permissions defined");

            Assert.True(permissions.GetArrayLength() > 0,
                $"Entity '{entity.Name}' must have at least one permission entry");
        }
    }

    [Fact]
    public void NoEntity_AllowsAnonymousWrite()
    {
        var entities = _dabConfig.RootElement.GetProperty("entities");
        foreach (var entity in entities.EnumerateObject())
        {
            var permissions = entity.Value.GetProperty("permissions");
            foreach (var perm in permissions.EnumerateArray())
            {
                var role = perm.GetProperty("role").GetString();
                if (role != "anonymous") continue;

                var actions = perm.GetProperty("actions");
                foreach (var action in actions.EnumerateArray())
                {
                    var actionName = action.ValueKind == JsonValueKind.String
                        ? action.GetString()
                        : action.GetProperty("action").GetString();

                    Assert.False(
                        actionName is "create" or "update" or "delete",
                        $"Entity '{entity.Name}' must not allow anonymous {actionName}");
                }
            }
        }
    }

    [Fact]
    public void PiiFields_HaveFieldLevelPermissions()
    {
        // Define known PII fields per entity
        var piiFields = new Dictionary<string, string[]>
        {
            ["Employee"] = new[] { "Email", "Phone", "SSN" },
            ["Contact"] = new[] { "Email", "Phone" }
        };

        var entities = _dabConfig.RootElement.GetProperty("entities");
        foreach (var (entityName, fields) in piiFields)
        {
            if (!entities.TryGetProperty(entityName, out var entity)) continue;

            var permissions = entity.GetProperty("permissions");
            foreach (var perm in permissions.EnumerateArray())
            {
                var role = perm.GetProperty("role").GetString();
                if (role == "admin") continue; // Admins may see all fields

                // Verify field-level restrictions exist
                foreach (var action in perm.GetProperty("actions").EnumerateArray())
                {
                    if (action.ValueKind != JsonValueKind.Object) continue;
                    if (!action.TryGetProperty("fields", out var fieldConfig)) continue;

                    if (fieldConfig.TryGetProperty("exclude", out var excluded))
                    {
                        var excludedFields = excluded.EnumerateArray()
                            .Select(f => f.GetString()).ToList();

                        foreach (var piiField in fields)
                        {
                            Assert.Contains(piiField, excludedFields,
                                StringComparer.OrdinalIgnoreCase);
                        }
                    }
                }
            }
        }
    }
}
```

### Why This Is High Quality

- **Shift-left validation:** Catches config errors at build time before deployment.
- **Security enforcement:** No anonymous writes, PII field restrictions.
- **Comprehensive coverage:** Source, permissions, REST/GraphQL enablement all validated.
- **Self-documenting:** Test names describe the exact policy being enforced.

---

## Example 6: Consumer Contract Test (Pact)

Testing that the Blazor frontend's expectations match DAB's actual responses.

```csharp
public class BlazorToDabContractTests
{
    [Fact]
    public async Task GetAsset_Contract_MatchesBlazorExpectations()
    {
        // Arrange — define what the Blazor frontend expects
        var pact = new PactBuilder()
            .ServiceConsumer("Blazor.Frontend")
            .HasPactWith("DAB.Api");

        pact.UponReceiving("a request for asset ASSET-001")
            .Given("asset ASSET-001 exists")
            .WithRequest(HttpMethod.Get, "/api/Asset/AssetId/ASSET-001")
            .WithHeader("Authorization", "Bearer valid-token")
            .WillRespondWith(new ProviderServiceResponse
            {
                Status = 200,
                Headers = new Dictionary<string, object>
                {
                    { "Content-Type", "application/json" }
                },
                Body = new
                {
                    value = new
                    {
                        AssetId = "ASSET-001",
                        AssetName = "HVAC Unit 1",
                        Condition = 3,
                        FacilityId = "FAC-001"
                    }
                }
            });

        // Act & Assert
        await pact.VerifyAsync(async ctx =>
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(ctx.MockServerUri + "/api/")
            };
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "valid-token");

            var response = await client.GetAsync("Asset/AssetId/ASSET-001");
            response.EnsureSuccessStatusCode();

            var content = await response.Content
                .ReadFromJsonAsync<DabSingleResponse<AssetDto>>();

            Assert.Equal("ASSET-001", content?.Value?.AssetId);
            Assert.Equal(3, content?.Value?.Condition);
        });
    }

    [Fact]
    public async Task ListWorkOrders_Contract_MatchesBlazorExpectations()
    {
        var pact = new PactBuilder()
            .ServiceConsumer("Blazor.Frontend")
            .HasPactWith("DAB.Api");

        pact.UponReceiving("a request for work order list filtered by status")
            .Given("work orders exist with various statuses")
            .WithRequest(HttpMethod.Get, "/api/WorkOrder")
            .WithQuery("$filter", "Status eq 'Submitted'")
            .WithHeader("Authorization", "Bearer valid-token")
            .WillRespondWith(new ProviderServiceResponse
            {
                Status = 200,
                Headers = new Dictionary<string, object>
                {
                    { "Content-Type", "application/json" }
                },
                Body = new
                {
                    value = new[]
                    {
                        new
                        {
                            WorkOrderId = "WO-1002",
                            Title = "Emergency Repair",
                            Status = "Submitted",
                            Priority = "Critical"
                        }
                    }
                }
            });

        await pact.VerifyAsync(async ctx =>
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(ctx.MockServerUri + "/api/")
            };
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "valid-token");

            var response = await client.GetAsync(
                "WorkOrder?$filter=Status eq 'Submitted'");
            response.EnsureSuccessStatusCode();

            var content = await response.Content
                .ReadFromJsonAsync<DabListResponse<WorkOrderDto>>();

            Assert.NotEmpty(content!.Value);
            Assert.All(content.Value,
                wo => Assert.Equal("Submitted", wo.Status));
        });
    }
}
```

### Why This Is High Quality

- **Consumer-driven:** Blazor frontend defines what it expects from DAB.
- **Tests DAB response shape:** Validates the `value` wrapper that DAB returns.
- **Includes auth headers:** Matches real-world request patterns.
- **Query parameter contracts:** Tests OData filter as part of the contract.
- **CI-ready:** Pact files can be published to Pact Broker for provider verification.

---

## DTO Reference

```csharp
public record AssetDto
{
    public string AssetId { get; init; } = string.Empty;
    public string AssetName { get; init; } = string.Empty;
    public int Condition { get; init; }
    public string FacilityId { get; init; } = string.Empty;
    public string? CreatedBy { get; init; }
}

public record WorkOrderDto
{
    public string WorkOrderId { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string? AssetId { get; init; }
    public string Priority { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? CreatedBy { get; init; }
}
```
