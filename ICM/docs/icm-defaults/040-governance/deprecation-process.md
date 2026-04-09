# Deprecation Process

> **Role in ICM:** Layer 3 - governance defaults for versioning, contracts, compatibility, and interface lifecycle controls.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: deprecation-process | docs/icm-defaults/040-governance/deprecation-process.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: Architecture Team  
> Last updated: 2026-01-22

## Deprecation Lifecycle

### Step 1: Announce Deprecation

**Add deprecation metadata in code:**

```csharp
[ApiVersion("1.0", Deprecated = true)]
[ApiController]
[Route("api/v{version:apiVersion}/assets")]
public class AssetV1Controller : ControllerBase
{
    [HttpGet("{id}")]
    [Obsolete("Use /api/v2/assets/{id} instead. This endpoint will be removed on 2026-07-01.")]
    public async Task<AssetV1Dto> GetAsset(string id)
    {
        Response.Headers.Add("X-API-Deprecated", "true");
        Response.Headers.Add("X-API-Sunset", "2026-07-01");
        Response.Headers.Add("X-API-Replacement", "/api/v2/assets/{id}");
        
        // Implementation...
    }
}
```

**HTTP Headers:**
- `X-API-Deprecated: true` - Indicates this API is deprecated
- `X-API-Sunset: 2026-07-01` - When it will be removed
- `X-API-Replacement: /api/v2/assets/{id}` - What to use instead

---

### Step 2: Log Deprecation Warnings

**Application-level deprecation logging:**

```csharp
public class DeprecationLoggingMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/v1"))
        {
            _logger.LogWarning(
                "Deprecated API called: {Path} by {ClientId}",
                context.Request.Path,
                context.User.FindFirst("client_id")?.Value);
        }
        
        await _next(context);
    }
}
```

---

### Step 3: Migration Period

**Minimum 6 months overlap for Stable/Production changes:**

- Month 1-3: Deprecation warnings active
- Month 3-6: Consumers migrate to new API
- Month 6: Old API removed

**Timeline example:**
- Jan 2026: v2 released, v1 marked deprecated
- Jan-Mar 2026: Deprecation warnings logged
- Apr 2026: Send emails to all v1 API consumers
- May 2026: Last month before removal
- Jun 2026: v1 API removed

---

### Step 4: Final Removal

**Remove deprecated code only after migration period:**

```csharp
// Remove v1 controller
// [Obsolete] AssetV1Controller deleted

// Keep v2 controller
[ApiVersion("2.0")]
[ApiController]
[Route("api/v{version:apiVersion}/assets")]
public class AssetV2Controller : ControllerBase
{
    // This is now the canonical implementation
}
```

---

## Event Deprecation

**Events follow similar pattern:**

```csharp
[Obsolete("Use AssetConditionChangedV2 instead. This event will stop being published on 2026-07-01")]
public record AssetConditionChanged : DomainEvent
{
    public int SchemaVersion => 1;  // Frozen at v1
}

public record AssetConditionChangedV2 : DomainEvent
{
    public int SchemaVersion => 2;  // New version
}
```

**Multi-version publishing:**
```csharp
// Publish both versions during migration period
await _events.PublishAsync(oldEvent);      // v1 (deprecated)
await _events.PublishAsync(newEvent);      // v2 (current)

// After migration period, publish only v2
```

---

## Deprecation for Internal APIs

**Faster deprecation for internal APIs (within-team):**

- Alpha/Beta can have breaking changes immediately
- Stable requires 3-month deprecation minimum
- LTS requires 6-month deprecation minimum

---

## Communication Strategy

**Before deprecation:**
1. Announce in release notes (3 months before)
2. Post in team Slack channels
3. Email API consumers
4. Create ticket in issue tracker

**During deprecation:**
1. Log warnings (visible in monitoring)
2. Return HTTP warning headers
3. Monthly email reminders
4. Dashboard showing v1 usage %

**At removal:**
1. Final email notification
2. Update documentation
3. Remove deprecated code
4. Announce in release notes

---

## Best Practices

✅ **DO:**
- Announce deprecation at least 6 months before removal
- Support multiple versions concurrently
- Log deprecation warnings for monitoring
- Email consumers when deprecating their APIs
- Provide migration guides
- Publish removal date clearly

❌ **DON'T:**
- Remove deprecated code without warning period
- Surprise consumers with breaking changes
- Remove old versions without supporting new ones
- Forget to update documentation
- Skip the communication step

---

## Related Documentation

- [API Versioning](api-versioning.md)
- [Event Schema Versioning](event-schema-versioning.md)
- [Breaking Change Detection](breaking-change-detection.md)
