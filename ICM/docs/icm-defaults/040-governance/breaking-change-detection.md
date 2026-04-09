# Breaking Change Detection

> **Role in ICM:** Layer 3 - governance defaults for versioning, contracts, compatibility, and interface lifecycle controls.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: breaking-change-detection | docs/icm-defaults/040-governance/breaking-change-detection.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: Architecture Team  
> Last updated: 2026-01-22

## Automated Detection in CI/CD

Breaking changes are caught before deployment:

```yaml
# .github/workflows/contract-check.yml
name: Contract Verification

on: [pull_request]

jobs:
  verify-contracts:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      
      - name: Generate OpenAPI spec
        run: dotnet run --project src/Asset.Service -- generate-openapi > current-spec.yaml
      
      - name: Download previous spec from registry
        run: curl https://registry.{{ PRODUCT_NAME }}.local/asset-service/v1/openapi.yaml > previous-spec.yaml
      
      - name: Detect breaking changes
        uses: oasdiff/oasdiff-action@v0.0.15
        with:
          base: previous-spec.yaml
          revision: current-spec.yaml
          fail-on-diff: breaking
      
      - name: Verify consumer contracts
        run: dotnet test --filter Category=ContractTest
```

---

## Breaking Change Detection Tool Output

```
Breaking changes detected:
- [BREAKING] Removed field: /assets/{id} response.location.buildingId
- [BREAKING] Changed type: /assets/{id} response.condition from integer to string
- [NON-BREAKING] Added optional field: /assets/{id} response.metadata

Build failed. Breaking changes require major version bump.
```

---

## What Triggers Build Failure

**API Changes:**
- Removing fields or endpoints
- Changing field types
- Adding required fields (without defaults)
- Renaming endpoints or fields

**Event Changes:**
- Removing fields from events
- Changing field types
- Renaming event properties

---

## What Doesn't Trigger Failure

**Non-breaking changes (safe):**
- Adding new optional fields
- Adding new endpoints
- Adding new events
- Adding optional parameters

---

## Prevention Strategies

**1. Add deprecated metadata early:**
```csharp
[Obsolete("Use GetAssetV2 instead. This method will be removed on 2026-07-01")]
public async Task<AssetV1Dto> GetAsset(string id)
{
    // Implementation
}
```

**2. Introduce new endpoint before removing old one:**
```csharp
// v2 endpoint (new)
public async Task<AssetV2Dto> GetAssetV2(string id)

// v1 endpoint (deprecated, still works)
public async Task<AssetV1Dto> GetAsset(string id)
```

**3. Use version negotiation:**
```csharp
[ApiVersion("1.0", Deprecated = true)]
[ApiVersion("2.0")]
public async Task<Asset> GetAsset(string id, ApiVersion version = ApiVersion.V2)
```

---

## Related Documentation

- [API Versioning](api-versioning.md)
- [Event Schema Versioning](event-schema-versioning.md)
- [Service Contract Registry](service-contract-registry.md)
- [Deprecation Process](deprecation-process.md)
