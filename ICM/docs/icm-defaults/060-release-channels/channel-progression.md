# Channel Progression: Release & Environment Lifecycle

> **Role in ICM:** Layer 3 - release-channel defaults for progression, isolation, deployment flow, and environment policy.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: channel-progression | docs/icm-defaults/060-release-channels/channel-progression.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Active  
> Owner: Platform Architecture Team  
> Last updated: 2026-01-22

## Overview

This document describes how artifacts progress through both Release and Environment channels during their lifecycle.

---

## Release Channel Progression Path

Features and services graduate through release channels as they mature:

```
Alpha (unstable, breaking changes)
    ↓ (validated by internal team)
Beta (features complete, may have issues)
    ↓ (tested and stabilized)
Stable (production-ready, proven)
    ↓ (long-term support commitment)
LTS (enterprise support)
```

### Release Progression Rules

- **Beta requires Alpha validation**: Internal team must validate the feature works as intended
- **Stable requires Beta testing**: Must pass comprehensive testing in Beta phase
- **LTS requires Stable production history**: Must prove stable in production for 6+ months
- **No skipping intermediate channels**: Must progress Alpha → Beta → Stable → LTS
- **Each release is independent**: v1.0 and v2.0 can have different progression timelines

---

## Environment Channel Progression Path

Code moves through environments sequentially:

```
Development (DEV)
    ↓ (code complete)
Testing/QA (TEST)
    ↓ (all tests pass)
UAT
    ↓ (business approved)
Production (PROD)
    ↓ (issues encountered)
Support (SUPPORT)
```

### Environment Progression Rules

- **Development → Testing**: Code must be complete and unit-tested
- **Testing → UAT**: All automated tests must pass
- **UAT → {Production, Training, Support}**: Business approval required (parallel deployment allowed)
- **Production ↔ Support**: Bidirectional (Production → Support for issue reproduction; Support → Production for fixes)
- **Sequential requirement**: Cannot skip environments
- **One-way except Support**: Typically flows forward; Support is accessed from Production for troubleshooting

---

## Combined Progression Example

A typical artifact lifecycle flows through **both** dimensions:

```
Alpha × Development (prototyping)
    ↓ (internal testing complete)
    
Beta × Testing (QA validation)
    ↓ (test suite passes)
    
Beta × UAT (business approval)
    ↓ (users approve)
    
Stable × Production (live deployment)
    ↓ (proving stable in production)
    
Stable × Training (user preparation, parallel with Prod)
    ↓ (used for 6+ months)
    
LTS × Support (enterprise customers, critical fixes only)
```

---

## Coordinated Promotion Service

Related artifacts move together to maintain consistency:

```csharp
public class CoordinatedPromotionService
{
    public async Task<CoordinatedPromotionResult> PromoteAsync(
        Guid artifactId,
        ReleaseChannel targetReleaseChannel,
        EnvironmentChannel targetEnvironmentChannel)
    {
        // Promotes all related artifacts together:
        // Example: Form definition + workflows + reports
        // All move to same release × environment combination
        // Ensures business logic stays synchronized
        
        var relatedArtifacts = await _artifactService.FindRelatedAsync(artifactId);
        
        foreach (var artifact in relatedArtifacts)
        {
            await PromoteSingleAsync(
                artifact.Id,
                targetReleaseChannel,
                targetEnvironmentChannel);
        }
        
        return new CoordinatedPromotionResult
        {
            ArtifactsPromoted = relatedArtifacts.Count,
            Timestamp = DateTimeOffset.UtcNow
        };
    }
}
```

---

## Release × Environment Promotion Matrix

### Valid Progression Paths

**Release Channel Progression** (unidirectional, no skipping):
```
Alpha → Beta → Stable → LTS
```

**Environment Channel Progression** (mostly sequential, Support is special):
```
Development → Testing → UAT → {Production, Training, Support}
Production ↔ Support  (bidirectional for issue diagnosis)
```

### Common Promotion Sequences

| Scenario | Release Path | Environment Path | Timeline |
|---|---|---|---|
| **New Feature** | Alpha → Beta → Stable | DEV → TEST → UAT → PROD | 4-12 weeks |
| **Bug Fix** | Stable → Stable | DEV → TEST → PROD | 1-2 weeks |
| **Enterprise Feature** | Stable → LTS | PROD → SUPPORT | 6+ months after Stable |
| **Security Patch** | - | - | Fast-track DEV → TEST → PROD (hours) |

---

## Promotion State Machine

### Release Channel State

```csharp
public class ReleaseChannelProgression
{
    // Valid transitions
    private static readonly Dictionary<ReleaseChannel, HashSet<ReleaseChannel>> ValidTransitions 
        = new()
    {
        [ReleaseChannel.Alpha] = new() { ReleaseChannel.Beta },
        [ReleaseChannel.Beta] = new() { ReleaseChannel.Stable },
        [ReleaseChannel.Stable] = new() { ReleaseChannel.LTS },
        [ReleaseChannel.LTS] = new()  // Terminal state
    };
    
    public bool CanPromote(ReleaseChannel from, ReleaseChannel to)
    {
        return ValidTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to);
    }
}
```

### Environment Channel State

```csharp
public class EnvironmentChannelProgression
{
    // Valid transitions (including bidirectional Production ↔ Support)
    private static readonly Dictionary<EnvironmentChannel, HashSet<EnvironmentChannel>> ValidTransitions 
        = new()
    {
        [EnvironmentChannel.Development] = new() { EnvironmentChannel.Testing },
        [EnvironmentChannel.Testing] = new() { EnvironmentChannel.UAT },
        [EnvironmentChannel.UAT] = new() 
        { 
            EnvironmentChannel.Production,
            EnvironmentChannel.Training,
            EnvironmentChannel.Support 
        },
        [EnvironmentChannel.Production] = new() { EnvironmentChannel.Support },
        [EnvironmentChannel.Support] = new() { EnvironmentChannel.Production }  // Bidirectional
    };
    
    public bool CanPromote(EnvironmentChannel from, EnvironmentChannel to)
    {
        return ValidTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to);
    }
}
```

---

## Promotion Timeline Example

### Feature: New Equipment Inspection Form

**Timeline**: January to August (8 months)

```
January (Week 1-2)
  Alpha × Development
  - Developers design form structure
  - Internal team reviews mockups
  - Basic functionality tested

January (Week 3-4) → February (Week 1)
  Beta × Testing
  - QA team validates features
  - Edge cases tested
  - Performance benchmarked

February (Week 2-4)
  Beta × UAT
  - Business analysts review form flow
  - Field managers provide feedback
  - Workflows tested end-to-end

March (Week 1-2)
  ✅ Business approval received
  
March (Week 3-4) → April
  Stable × Production
  - Rolled out to 100% of production users
  - Monitoring alerts active
  - Support team trained

April → August (6 months)
  ✅ Stable production history proven
  
August
  LTS × Support
  - Enterprise customers locked to this version
  - Receives only critical security updates
  - Feature frozen
  - 5+ year support commitment
```

---

## Rollback Strategy

If issues discovered at any stage:

```csharp
public class ChannelRollbackService
{
    // Rollback within same environment (different release)
    public async Task<RollbackResult> RollbackReleaseChannelAsync(
        Guid artifactId,
        ReleaseChannel targetReleaseChannel)
    {
        // Example: Stable → Beta rollback
        // Goes back one release channel
        // Typically used for hotfixes
    }
    
    // Rollback within same release (previous environment)
    public async Task<RollbackResult> RollbackEnvironmentChannelAsync(
        Guid artifactId,
        EnvironmentChannel targetEnvironmentChannel)
    {
        // Example: Production → UAT rollback
        // Goes back one environment channel
        // Typically for emergency fixes
    }
    
    // Both dimensions (full rollback)
    public async Task<RollbackResult> RollbackBothChannelsAsync(
        Guid artifactId,
        ReleaseChannel targetReleaseChannel,
        EnvironmentChannel targetEnvironmentChannel)
    {
        // Full rollback to previous state
        // Last resort for critical issues
    }
}
```

---

## Related Documentation

- [Release Channels Concept](release-channels-concept.md)
- [Environment Channels Concept](environment-channels-concept.md)
- [Data Isolation](data-isolation.md)
- [Configuration & Governance](channel-governance-config.md)
- [Best Practices & Testing](best-practices-and-testing.md)
