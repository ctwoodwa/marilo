# Service Contract Registry

> **Role in ICM:** Layer 3 - governance defaults for versioning, contracts, compatibility, and interface lifecycle controls.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: service-contract-registry | docs/icm-defaults/040-governance/service-contract-registry.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: Architecture Team  
> Last updated: 2026-01-22

## Central Registry Structure

Central registry for all service interfaces:

```
contract-registry/
├── services/
│   ├── asset-service/
│   │   ├── openapi-v1.yaml
│   │   ├── openapi-v2.yaml
│   │   ├── events/
│   │   │   ├── AssetConditionChanged-v1.json
│   │   │   └── AssetConditionChanged-v2.json
│   │   └── consumers/
│   │       ├── Asset.Web-pact.json
│   │       └── WorkOrder.Service-pact.json
│   ├── work-order-service/
│   │   ├── openapi-v1.yaml
│   │   └── events/
│   │       └── WorkOrderCreated-v1.json
├── platform/
│   ├── messaging-abstractions.yaml
│   └── identity-abstractions.yaml
└── README.md
```

---

## Registry Contents

**For each service:**
- OpenAPI specification (REST APIs)
- Event schema definitions (AsyncAPI)
- gRPC proto files (if using gRPC)
- Consumer contracts (Pact files)

---

## Publishing Workflow

```csharp
// Automatic OpenAPI publishing in service startup
public static class OpenApiRegistration
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

## Discovery

Services and clients discover contracts from registry:

```csharp
public class ServiceDiscovery
{
    public async Task<OpenApiDocument> GetServiceSpecAsync(string serviceName, string version)
    {
        return await _registry.GetSpecAsync(serviceName, version);
    }
    
    public async Task<IEnumerable<ServiceContract>> GetAllServicesAsync()
    {
        return await _registry.ListServicesAsync();
    }
}
```

---

## Related Documentation

- [API Versioning](api-versioning.md)
- [Contract Testing](contract-testing.md)
- [Breaking Change Detection](breaking-change-detection.md)
