# Channel Configuration & Governance

> **Role in ICM:** Layer 3 - release-channel defaults for progression, isolation, deployment flow, and environment policy.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: channel-governance-config | docs/icm-defaults/060-release-channels/channel-governance-config.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Active  
> Owner: Platform Architecture Team  
> Last updated: 2026-01-22

## Overview

Each release × environment combination requires specific configuration for data management, synchronization, security, and availability. This document defines governance policies and configuration strategies.

---

## Dual-Channel Configuration

Each release × environment combination has independent configuration:

```csharp
public class DualChannelConfiguration : IChannelScoped
{
    public Guid TenantId { get; set; }
    public ReleaseChannel ReleaseChannel { get; set; }
    public EnvironmentChannel EnvironmentChannel { get; set; }
    
    // Data Management
    public int DataRetentionDays { get; set; }
    public bool AutoArchiveDeprecated { get; set; }
    
    // Synchronization
    public TimeSpan SyncInterval { get; set; }
    public bool EnableDeltaSync { get; set; }
    
    // Security & Privacy
    public bool MaskPIIInNonProduction { get; set; }
    public bool RequireApprovalForPromotion { get; set; }
    public bool EnableEncryption { get; set; }
    
    // Availability
    public int TargetAvailabilityPercentage { get; set; }
    public int MaxConcurrentUsers { get; set; }
}
```

---

## Configuration by Channel Combination

### Alpha × Development

```csharp
var alphaDev = new DualChannelConfiguration
{
    ReleaseChannel = ReleaseChannel.Alpha,
    EnvironmentChannel = EnvironmentChannel.Development,
    
    // Data: Minimal retention
    DataRetentionDays = 30,
    AutoArchiveDeprecated = false,
    
    // Sync: Frequent updates
    SyncInterval = TimeSpan.FromMinutes(5),
    EnableDeltaSync = false,
    
    // Security: Not required (no real data)
    MaskPIIInNonProduction = true,
    RequireApprovalForPromotion = false,
    EnableEncryption = false,
    
    // Availability: Not critical
    TargetAvailabilityPercentage = 80,
    MaxConcurrentUsers = 100
};
```

### Beta × Testing

```csharp
var betaTest = new DualChannelConfiguration
{
    ReleaseChannel = ReleaseChannel.Beta,
    EnvironmentChannel = EnvironmentChannel.Testing,
    
    // Data: Test data retention
    DataRetentionDays = 90,
    AutoArchiveDeprecated = true,
    
    // Sync: Regular updates
    SyncInterval = TimeSpan.FromMinutes(30),
    EnableDeltaSync = true,
    
    // Security: Moderate
    MaskPIIInNonProduction = true,
    RequireApprovalForPromotion = false,
    EnableEncryption = true,
    
    // Availability: Good
    TargetAvailabilityPercentage = 95,
    MaxConcurrentUsers = 500
};
```

### Beta × UAT

```csharp
var betaUAT = new DualChannelConfiguration
{
    ReleaseChannel = ReleaseChannel.Beta,
    EnvironmentChannel = EnvironmentChannel.UAT,
    
    // Data: User acceptance testing data
    DataRetentionDays = 180,
    AutoArchiveDeprecated = false,
    
    // Sync: Regular updates
    SyncInterval = TimeSpan.FromHours(1),
    EnableDeltaSync = true,
    
    // Security: Masked PII, approval required
    MaskPIIInNonProduction = true,
    RequireApprovalForPromotion = true,
    EnableEncryption = true,
    
    // Availability: High
    TargetAvailabilityPercentage = 99,
    MaxConcurrentUsers = 1000
};
```

### Stable × Production

```csharp
var stableProd = new DualChannelConfiguration
{
    ReleaseChannel = ReleaseChannel.Stable,
    EnvironmentChannel = EnvironmentChannel.Production,
    
    // Data: Permanent retention (compliance)
    DataRetentionDays = 2555,  // 7 years
    AutoArchiveDeprecated = true,
    
    // Sync: Regular but controlled
    SyncInterval = TimeSpan.FromHours(4),
    EnableDeltaSync = true,
    
    // Security: Maximum
    MaskPIIInNonProduction = false,
    RequireApprovalForPromotion = true,
    EnableEncryption = true,
    
    // Availability: Maximum SLA
    TargetAvailabilityPercentage = 99,
    MaxConcurrentUsers = 10000
};
```

### LTS × Support

```csharp
var ltsSupport = new DualChannelConfiguration
{
    ReleaseChannel = ReleaseChannel.LTS,
    EnvironmentChannel = EnvironmentChannel.Support,
    
    // Data: Permanent retention
    DataRetentionDays = 2555,  // 7 years
    AutoArchiveDeprecated = false,
    
    // Sync: Less frequent (stable)
    SyncInterval = TimeSpan.FromHours(8),
    EnableDeltaSync = true,
    
    // Security: Maximum
    MaskPIIInNonProduction = false,
    RequireApprovalForPromotion = true,
    EnableEncryption = true,
    
    // Availability: Maximum (for enterprise)
    TargetAvailabilityPercentage = 99,
    MaxConcurrentUsers = 5000
};
```

---

## Feature Flag Configuration by Release

| Release | Feature Flags | New Features | Performance | Breaking Changes |
|---|---|---|---|---|
| **Alpha** | All experimental | Bleeding edge | Minimal optimization | Expected, encouraged |
| **Beta** | Most features | Recent features, likely stable | Good optimization | Rare, announced |
| **Stable** | Core features only | Proven features only | Optimized | Never, migration paths |
| **LTS** | Minimal, critical only | None | Highly optimized | Never |

### Example: Experimental Feature Flags

```csharp
public class FeatureConfiguration : IChannelScoped
{
    public Guid TenantId { get; set; }
    public ReleaseChannel ReleaseChannel { get; set; }
    public EnvironmentChannel EnvironmentChannel { get; set; }
    public string FeatureName { get; set; }
    public bool IsEnabled { get; set; }
    public DateTimeOffset? EnabledSince { get; set; }
    public DateTimeOffset? DisabledAt { get; set; }
}

// Example: Advanced workflow features in Beta, general in Stable
if (config.IsEnabled && 
    config.ReleaseChannel == ReleaseChannel.Stable &&
    config.EnvironmentChannel == EnvironmentChannel.Production)
{
    // Enable advanced features for general users
}
```

---

## Audit & Compliance Logging

Track all channel-related operations on both dimensions:

```csharp
public class DualChannelAuditEvent : IChannelScoped
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public ReleaseChannel ReleaseChannel { get; set; }
    public EnvironmentChannel EnvironmentChannel { get; set; }
    
    public ChannelEventType EventType { get; set; }
    public string EntityType { get; set; }
    public Guid EntityId { get; set; }
    public Guid PerformedBy { get; set; }
    public DateTimeOffset PerformedAt { get; set; }
    public string? Reason { get; set; }
    public Dictionary<string, object>? Changes { get; set; }
}

public enum ChannelEventType
{
    Created,          // Artifact created
    Updated,          // Artifact modified
    Published,        // Released to next channel
    Promoted,         // Moved between channels
    Deprecated,       // Marked for removal
    Archived,         // Removed from active use
    Subscribed,       // User subscribed to channel
    Unsubscribed      // User unsubscribed from channel
}
```

### Audit Checklist

**Every channel operation must log:**
- ✅ What artifact was changed
- ✅ Which channels were involved (Release + Environment)
- ✅ Who performed the action (user/system)
- ✅ When it occurred (timestamp)
- ✅ Why (reason/approval reference)
- ✅ What changed (old/new values for significant fields)

---

## Promotion Approval Workflow

### Approval Requirements by Channel

```
Alpha → Beta:
  ✓ Requires: Internal team lead review
  ✓ Evidence: Internal validation report
  ✓ Timeline: 2-3 business days

Beta → Stable:
  ✓ Requires: QA sign-off + Architecture approval
  ✓ Evidence: Test results, release notes
  ✓ Timeline: 5-7 business days

Stable → LTS:
  ✓ Requires: Architecture team approval
  ✓ Evidence: 6+ months production history
  ✓ Timeline: 1-2 weeks

Environment Progression → Production:
  ✓ Requires: Business approval (change request)
  ✓ Evidence: UAT sign-off, deployment plan
  ✓ Timeline: depends on change control process
```

---

## Caching Strategy by Channel

Different channels have different caching needs:

```csharp
// Cache key includes both channels
var cacheKey = $"artifacts:{tenantId}:{releaseChannel}:{environmentChannel}";

// Different TTL strategies per channel combination
private TimeSpan GetTTLForChannel(ReleaseChannel release, EnvironmentChannel env)
{
    return (release, env) switch
    {
        // Production: Longer cache (stable content)
        (ReleaseChannel.Stable, EnvironmentChannel.Production) => TimeSpan.FromHours(4),
        
        // UAT: Medium cache (frequently changing during testing)
        (ReleaseChannel.Beta, EnvironmentChannel.UAT) => TimeSpan.FromMinutes(30),
        
        // Testing: Short cache (QA needs fresh data)
        (ReleaseChannel.Beta, EnvironmentChannel.Testing) => TimeSpan.FromMinutes(15),
        
        // Development: Minimal cache (developers need real-time)
        (ReleaseChannel.Alpha, EnvironmentChannel.Development) => TimeSpan.FromMinutes(5),
        
        // LTS/Support: Minimal changes, longer cache
        (ReleaseChannel.LTS, EnvironmentChannel.Support) => TimeSpan.FromHours(8),
        
        // Default
        _ => TimeSpan.FromHours(1)
    };
}

_cache.Set(cacheKey, data, GetTTLForChannel(releaseChannel, environmentChannel));
```

---

## Related Documentation

- [Channel Progression](channel-progression.md)
- [Data Isolation](data-isolation.md)
- [Performance & Scaling](performance-and-scaling.md)
- [Best Practices & Testing](best-practices-and-testing.md)
