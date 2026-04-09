# Data Isolation: Dual-Channel Enforcement

> **Role in ICM:** Layer 3 - release-channel defaults for progression, isolation, deployment flow, and environment policy.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: data-isolation | docs/icm-defaults/060-release-channels/data-isolation.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Active  
> Owner: Platform Architecture Team  
> Last updated: 2026-01-22

## Overview

**Data isolation is the foundation of the dual-channel architecture.** Every piece of data is scoped to **both** a Release Channel and an Environment Channel. Operations cannot cross channel boundaries on either dimension.

---

## Core Principle

**Channels are NOT just queries; they are identity.** Data belongs to a specific channel combination and cannot be accessed from other combinations.

---

## Repository-Level Enforcement

Every repository enforces isolation on **both** dimensions:

```csharp
public abstract class DualChannelScopedRepository<T> where T : IChannelScoped
{
    protected IQueryable<T> GetBaseQuery(
        Guid tenantId,
        ReleaseChannel releaseChannel,
        EnvironmentChannel environmentChannel)
    {
        return _context.Set<T>()
            .Where(e => e.TenantId == tenantId)
            .Where(e => e.ReleaseChannel == releaseChannel)      // ALWAYS FILTERED
            .Where(e => e.EnvironmentChannel == environmentChannel);  // ALWAYS FILTERED
    }
    
    protected void ValidateChannels(T entity)
    {
        // Prevent accidental channel changes
        if (entity.Id != Guid.Empty)
        {
            var existing = _context.Set<T>().Find(entity.Id);
            if (existing?.ReleaseChannel != entity.ReleaseChannel)
                throw new InvalidOperationException(
                    $"Cannot change release channel from {existing.ReleaseChannel} to {entity.ReleaseChannel}");
            
            if (existing?.EnvironmentChannel != entity.EnvironmentChannel)
                throw new InvalidOperationException(
                    $"Cannot change environment channel from {existing.EnvironmentChannel} to {entity.EnvironmentChannel}");
        }
    }
}
```

### Key Rules

1. **Every query must include both channel filters** (enforced in base class)
2. **Channel changes are forbidden** (validated on update)
3. **Cross-channel operations throw exceptions** (fail fast)
4. **No null channel values** (required on all entities)

---

## Cross-Channel Operation Prevention

Operations cannot cross channels on either dimension:

```csharp
[Fact]
public async Task CrossReleaseChannel_ThrowsException()
{
    var alphaForm = await CreateFormInChannels(
        ReleaseChannel.Alpha, EnvironmentChannel.Development);
    
    // Attempt to query Alpha form from Stable context
    await Assert.ThrowsAsync<ChannelMismatchException>(
        () => _formService.GetInstanceAsync(
            alphaForm.Id,
            ReleaseChannel.Stable,        // Different release ❌
            EnvironmentChannel.Development));
    
    // Error: "Cannot access Release=Alpha from context Release=Stable"
}

[Fact]
public async Task CrossEnvironmentChannel_ThrowsException()
{
    var devForm = await CreateFormInChannels(
        ReleaseChannel.Stable, EnvironmentChannel.Development);
    
    // Attempt to query Development form from Production context
    await Assert.ThrowsAsync<ChannelMismatchException>(
        () => _formService.GetInstanceAsync(
            devForm.Id,
            ReleaseChannel.Stable,              // Same release ✓
            EnvironmentChannel.Production));    // Different environment ❌
    
    // Error: "Cannot access Environment=Development from context Environment=Production"
}

[Fact]
public async Task BothChannelsMismatched_ThrowsException()
{
    var betaTestForm = await CreateFormInChannels(
        ReleaseChannel.Beta, EnvironmentChannel.Testing);
    
    // Attempt to access with both channels wrong
    await Assert.ThrowsAsync<ChannelMismatchException>(
        () => _formService.GetInstanceAsync(
            betaTestForm.Id,
            ReleaseChannel.Stable,
            EnvironmentChannel.Production));
}
```

---

## Data Flow Isolation Architecture

Data strictly respects channel boundaries on both dimensions:

```
Release Dimension:
├─ Alpha Data (internal, breaking changes OK)
│  └─ Alpha → Beta (validated)
│
├─ Beta Data (features complete, potential issues)
│  └─ Beta → Stable (tested)
│
├─ Stable Data (production-proven, reliable)
│  └─ Stable → LTS (for enterprises)
│
└─ LTS Data (long-term stability)

Environment Dimension:
├─ Dev Data (developers, synthetic)
│  └─ Dev → Test (tested)
│
├─ Test Data (QA, edge cases)
│  └─ Test → UAT (approved)
│
├─ UAT Data (business scenarios, masked PII)
│  ├─ → Production (approved)
│  ├─ → Training (parallel, for user prep)
│  └─ → Support (parallel, for issue diagnosis)
│
├─ Production Data (real customers, sensitive)
│  └─ ↔ Support (bidirectional for troubleshooting)
│
└─ Support Data (isolated reproduction)

CRITICAL: No data flows ACROSS channel boundaries. Ever.
```

---

## Mobile App Synchronization

Mobile apps download only content for their subscribed channels:

```csharp
public class MobileAppConfiguration
{
    public Guid TenantId { get; set; }
    public ReleaseChannel ReleaseChannel { get; set; }      // e.g., Stable
    public EnvironmentChannel EnvironmentChannel { get; set; }  // e.g., Production
}

// Example: Production app configuration
var prodApp = new MobileAppConfiguration
{
    TenantId = _tenantId,
    ReleaseChannel = ReleaseChannel.Stable,
    EnvironmentChannel = EnvironmentChannel.Production
};

// Example: Beta tester app configuration
var betaApp = new MobileAppConfiguration
{
    TenantId = _tenantId,
    ReleaseChannel = ReleaseChannel.Beta,
    EnvironmentChannel = EnvironmentChannel.Testing
};
```

### Manifest Download

Apps download unified manifest for their subscribed channels only:

```csharp
var manifest = await _syncService.GetManifestAsync(
    tenantId,
    ReleaseChannel.Stable,
    EnvironmentChannel.Production);
    // Returns only Stable × Production manifest

// Manifest includes:
// - Form definitions for Stable × Production
// - Workflow definitions for Stable × Production
// - Report templates for Stable × Production
// - Configuration for Stable × Production
// All in one secure download
```

### Delta Sync

Apps track manifest version for efficient channel-specific updates:

```http
GET /api/sync/manifest
  ?tenantId={guid}
  &releaseChannel=Stable
  &environmentChannel=Production
  &version=abc123

// Returns only changes since last sync for this channel combo
// Benefits: Reduces bandwidth, improves security
```

---

## Database Isolation Strategy

Dual channels enable sophisticated logical and physical partitioning:

```sql
-- Composite index on both channels (mandatory)
CREATE CLUSTERED INDEX IX_Artifact_TenantChannels 
ON Artifacts(TenantId, ReleaseChannel, EnvironmentChannel);

-- Physical partitioning option 1: By environment
-- ProdDb contains only Production and Support environments
-- DevDb contains only Development and Testing environments  
-- UATDb contains only UAT environment

-- Physical partitioning option 2: By release
-- StableDb contains only Stable and LTS releases
-- AlphaBetaDb contains only Alpha and Beta releases

-- Partial indexes for common patterns
CREATE INDEX IX_Stable_Prod ON Artifacts(TenantId, CreatedAt)
WHERE ReleaseChannel = 3 AND EnvironmentChannel = 4;  -- Stable × Production
```

---

## Query Optimization

Dual-channel filters enable highly efficient queries:

```sql
-- Highly selective composite filter
SELECT * FROM FormInstances
WHERE TenantId = @tenantId
  AND ReleaseChannel = @releaseChannel       -- Indexed
  AND EnvironmentChannel = @environmentChannel  -- Indexed
  AND CreatedAt >= @startDate;
```

**Benefits:**
- Queries are highly selective (3 indexed columns)
- Database optimizer can use partial indexes
- Physical partitioning reduces table size
- No risk of cross-channel data leakage

---

## Compliance Benefits

Channel isolation supports regulatory compliance:

### Data Separation
- Development data clearly separated from Production
- Test data cannot contaminate operational records
- Training scenarios isolated from real customer data
- Support data isolated from Production data

### Traceability
- Complete audit trail of all data movements
- Records which users/systems accessed which channels
- Documents reasons for channel promotions
- Tracks feature rollout history

### Risk Management
- New features tested thoroughly before reaching Stable
- UAT approval prevents unvalidated code reaching Production
- Breaking changes confined to Alpha, never in Stable/LTS
- Enterprise customers on LTS protected from instability

### Regulatory Compliance
- PII handling varies by environment (masked in non-Prod)
- Production data meets security requirements (encrypted, backed up)
- Development data can be synthetic/test data (no regulatory concern)
- Support data isolated for issue reproduction only

---

## Anti-Patterns

### ❌ Cross-Channel Queries

```csharp
// WRONG: Querying multiple release channels
var allReleases = _repository.GetAsync(
    tenantId, 
    new[] { ReleaseChannel.Alpha, ReleaseChannel.Beta });  // ❌ NO!
```

**Why**: Creates data leakage risk, violates isolation contract

**Correct**: Execute separate queries per channel
```csharp
var alphaForms = await _repository.GetAsync(tenantId, ReleaseChannel.Alpha, env);
var betaForms = await _repository.GetAsync(tenantId, ReleaseChannel.Beta, env);
```

### ❌ Copying Data Between Channels

```csharp
// WRONG: Moving Production data to Development
var prodData = await _repo.GetAsync(tenantId, Release.Stable, Env.Production);
await _repo.SaveAsync(prodData.Select(d => new Form 
{ 
    ...d,  
    EnvironmentChannel = EnvironmentChannel.Development  // ❌ NO!
}));
```

**Why**: Violates data residency, compliance, and privacy

**Correct**: Use synthetic test data for development
```csharp
var testData = _testDataGenerator.Generate();
await _repo.SaveAsync(testData.Select(d => new Form 
{ 
    ...d, 
    ReleaseChannel = ReleaseChannel.Alpha,
    EnvironmentChannel = EnvironmentChannel.Development  // ✓ Correct
}));
```

---

## Related Documentation

- [Channel Progression](channel-progression.md)
- [Configuration & Governance](channel-governance-config.md)
- [Performance & Scaling](performance-and-scaling.md)
- [Best Practices & Testing](best-practices-and-testing.md)
