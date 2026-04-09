# Event Schema Versioning

> **Role in ICM:** Layer 3 - governance defaults for versioning, contracts, compatibility, and interface lifecycle controls.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: event-schema-versioning | docs/icm-defaults/040-governance/event-schema-versioning.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: Architecture Team  
> Last updated: 2026-01-22

## Event Metadata

All events include schema version in metadata:

```csharp
public abstract record DomainEvent
{
    public string EventType { get; init; }
    public int SchemaVersion { get; init; }  // Explicit version
    public DateTimeOffset Timestamp { get; init; }
    // ...
}

public record AssetConditionChanged : DomainEvent
{
    public AssetConditionChanged()
    {
        EventType = "AssetConditionChanged";
        SchemaVersion = 2;  // Incremented for breaking changes
    }
    
    public string AssetId { get; init; }
    public ConditionScore NewScore { get; init; }
}
```

---

## Event Evolution Rules

**Backward-compatible changes (same schema version):**
- Add optional fields
- Add new event types

**Breaking changes (increment schema version):**
- Remove fields
- Rename fields
- Change field types

---

## Multi-Version Event Handling

Consumers must handle multiple schema versions:

```csharp
public class AssetReadModelUpdater : IEventConsumer
{
    public async Task HandleAsync(DomainEvent @event, EventContext ctx)
    {
        if (@event is AssetConditionChanged acc)
        {
            switch (acc.SchemaVersion)
            {
                case 1:
                    await HandleV1Async((AssetConditionChangedV1)acc);
                    break;
                case 2:
                    await HandleV2Async(acc);
                    break;
                default:
                    _logger.LogWarning("Unknown schema version {Version}", acc.SchemaVersion);
                    break;
            }
        }
    }
}
```

---

## Event Versioning Strategy

**Append-only events:**
- Events in Event Hub are immutable history
- Old schema versions remain in stream forever
- Consumers must handle ALL schema versions they've ever seen

**Schema registry:**
- All event schemas published to central registry
- Consumers download schemas at startup
- Breaking changes detected by comparing schemas

---

## Related Documentation

- [API Versioning](api-versioning.md)
- [Contract Testing](contract-testing.md)
- [Service Contract Registry](service-contract-registry.md)
