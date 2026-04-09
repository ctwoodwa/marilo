# API Versioning Strategy

> **Role in ICM:** Layer 3 - governance defaults for versioning, contracts, compatibility, and interface lifecycle controls.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: api-versioning | docs/icm-defaults/040-governance/api-versioning.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: Architecture Team  
> Last updated: 2026-01-22

## Semantic Versioning for APIs

**Format:** `/api/v{major}/{resource}`

**Version rules:**
- **Major version** change: breaking changes (incompatible request/response)
- **Minor version** NOT exposed: backward-compatible additions (new optional fields)
- **Patch version** NOT exposed: bug fixes with no schema changes

**Examples:**
- `/api/v1/assets` - Version 1 of Asset API
- `/api/v2/assets` - Version 2 (breaking changes from v1)
- `/api/v1/work-orders` - Different resource, same major version

---

## Version Support Policy

**N-1 support rule:**
- Current version (N) + previous version (N-1) supported
- Minimum 6 months overlap before deprecating N-1
- Deprecation warnings added 3 months before removal

**Example timeline:**
- Jan 2026: v2 released, v1 still supported
- Apr 2026: v1 deprecation warnings added (HTTP header + logs)
- Jul 2026: v1 removed (6 months after v2 release)

---

## Breaking vs. Non-Breaking Changes

### Breaking Change Examples

**Breaking (requires new major version):**
```json
// v1 response
{
  "assetId": "string",
  "condition": 3  // number
}

// v2 response (BREAKING: type changed)
{
  "assetId": "string",
  "condition": "Good"  // string enum
}
```

### Non-Breaking Change Examples

**Non-breaking (same version, add optional fields):**
```json
// Original
{
  "assetId": "string",
  "condition": 3
}

// Enhanced (NON-BREAKING: new optional field)
{
  "assetId": "string",
  "condition": 3,
  "conditionDescription": "Good"  // new optional field
}
```

---

## Implementation

### API Versioning in Code

```csharp
[ApiVersion("1.0", Deprecated = true)]
[ApiController]
[Route("api/v{version:apiVersion}/assets")]
public class AssetV1Controller : ControllerBase
{
    [HttpGet("{id}")]
    [Obsolete("Use /api/v2/assets/{id} instead")]
    public async Task<AssetV1Dto> GetAsset(string id)
    {
        // v1 implementation
    }
}

[ApiVersion("2.0")]
[ApiController]
[Route("api/v{version:apiVersion}/assets")]
public class AssetV2Controller : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<AssetV2Dto> GetAsset(string id)
    {
        // v2 implementation
    }
}
```

### Client Compatibility

```csharp
public class AssetApiClient
{
    private readonly string _baseUrl;
    
    // Support multiple versions
    public async Task<Asset> GetAssetAsync(string id, ApiVersion version = ApiVersion.V2)
    {
        var endpoint = $"{_baseUrl}/api/{version.Value}/assets/{id}";
        return await _http.GetJsonAsync<Asset>(endpoint);
    }
}

public enum ApiVersion
{
    V1,  // Legacy (deprecated)
    V2   // Current
}
```

---

## Related Documentation

- [Event Schema Versioning](event-schema-versioning.md)
- [Contract Testing](contract-testing.md)
- [Breaking Change Detection](breaking-change-detection.md)
- [Deprecation Process](deprecation-process.md)
