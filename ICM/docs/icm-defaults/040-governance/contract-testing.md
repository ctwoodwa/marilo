# Contract Testing: Consumer-Driven Contracts

> **Role in ICM:** Layer 3 - governance defaults for versioning, contracts, compatibility, and interface lifecycle controls.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: contract-testing | docs/icm-defaults/040-governance/contract-testing.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: Architecture Team  
> Last updated: 2026-01-22

## Consumer-Driven Contract Testing (Pact Pattern)

**Consumer defines expectations as tests; Provider must honor them.**

### Consumer Test Example

```csharp
// Asset.Web (consumer) defines contract for Asset.Service (provider)
[Fact]
public async Task GetAsset_ReturnsAssetWithRequiredFields()
{
    var pact = new PactBuilder()
        .ServiceConsumer("Asset.Web")
        .HasPactWith("Asset.Service");
    
    pact.UponReceiving("a request for asset asset-123")
        .Given("asset asset-123 exists")
        .WithRequest(HttpMethod.Get, "/api/v1/assets/asset-123")
        .WillRespondWith(new ProviderServiceResponse
        {
            Status = 200,
            Headers = new Dictionary<string, object> { { "Content-Type", "application/json" } },
            Body = new
            {
                assetId = "asset-123",
                assetName = "HVAC Unit 1",
                condition = 3,
                location = new { facilityId = "facility-1" }
            }
        });
    
    await pact.VerifyAsync(async ctx =>
    {
        var client = new AssetApiClient(ctx.MockServerUri);
        var asset = await client.GetAssetAsync("asset-123");
        
        Assert.Equal("asset-123", asset.AssetId);
        Assert.Equal(3, asset.Condition);
    });
}
```

### Provider Verification

```csharp
// Asset.Service (provider) runs consumer contracts in CI/CD
[Fact]
public void VerifyConsumerContracts()
{
    var config = new PactVerifierConfig
    {
        ProviderVersion = "1.2.3",
        PactBrokerUri = new Uri("https://pact-broker.{{ PRODUCT_NAME }}.local"),
        ProviderName = "Asset.Service"
    };
    
    new PactVerifier(config)
        .ServiceProvider("Asset.Service", _serviceUri)
        .HonoursPactWith("Asset.Web")
        .PactUri("../pacts/Asset.Web-Asset.Service.json")
        .Verify();
}
```

---

## CI/CD Enforcement

**Breaking changes must fail the build:**

- Consumer publishes contracts to Pact Broker
- Provider verifies contracts before deployment
- Failing contract test = blocked deployment
- Forces provider to maintain backward compatibility or version bump

---

## Contract Registry Integration

Contracts published to central registry:

```csharp
// Automatic contract publishing in service startup
public static class ContractRegistration
{
    public static IApplicationBuilder UseContractRegistry(this IApplicationBuilder app)
    {
        var apiSpec = app.GenerateOpenApiSpecification();
        
        // Publish to contract registry
        var registry = app.Services.GetRequiredService<IContractRegistry>();
        registry.PublishAsync(new ServiceContract
        {
            ServiceName = "Asset.Service",
            Version = "v1",
            Specification = apiSpec,
            Type = ContractType.OpenAPI
        });
        
        return app;
    }
}
```

---

## Related Documentation

- [API Versioning](api-versioning.md)
- [Event Schema Versioning](event-schema-versioning.md)
- [Service Contract Registry](service-contract-registry.md)
- [Breaking Change Detection](breaking-change-detection.md)
