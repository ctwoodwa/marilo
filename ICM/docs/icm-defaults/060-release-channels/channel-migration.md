# Channel Migration Guide: From Single-Environment Systems

> **Role in ICM:** Layer 3 - release-channel defaults for progression, isolation, deployment flow, and environment policy.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: channel-migration | docs/icm-defaults/060-release-channels/channel-migration.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Active  
> Owner: Platform Architecture Team  
> Last updated: 2026-01-22

## Overview

This guide describes how to migrate existing systems from single-environment architectures to the dual-channel (Release × Environment) model.

---

## Migration Approach

Migrate gradually, validating at each phase. Never attempt big-bang migration.

---

## Phase 1: Add Channel Columns

Add both channel columns with sensible defaults:

```sql
-- Add both channel columns with defaults
ALTER TABLE Artifacts 
ADD ReleaseChannel INT DEFAULT 3,  -- Default to Stable
    EnvironmentChannel INT DEFAULT 4;  -- Default to Production

-- This maintains backward compatibility
-- Old code continues to work (returns Stable × Production by default)
-- New code can use channels explicitly
```

**Validation:**
- Verify table still has all data
- Verify old queries still work
- No application code changes needed yet

---

## Phase 2: Classify Existing Data

Migrate data to appropriate channels based on context:

```sql
-- Identify and classify data by creator role
UPDATE Artifacts 
SET ReleaseChannel = 1,  -- Alpha
    EnvironmentChannel = 1  -- Development
WHERE CreatedBy IN (SELECT Id FROM Users WHERE Role = 'Developer');

-- Classify QA-created data as Beta × Testing
UPDATE Artifacts 
SET ReleaseChannel = 2,  -- Beta
    EnvironmentChannel = 2  -- Testing
WHERE CreatedBy IN (SELECT Id FROM Users WHERE Role = 'QA');

-- Classify business-approved data as Beta × UAT
UPDATE Artifacts 
SET ReleaseChannel = 2,  -- Beta
    EnvironmentChannel = 3  -- UAT
WHERE CreatedBy IN (SELECT Id FROM Users WHERE Role = 'BusinessAnalyst')
  AND IsApproved = 1;

-- Classify production-deployed data as Stable × Production
UPDATE Artifacts 
SET ReleaseChannel = 3,  -- Stable
    EnvironmentChannel = 4  -- Production
WHERE DeployedToProduction = 1;
```

**Validation:**
- Spot-check classified data
- Verify no unclassified records remain
- Create backup before proceeding

---

## Phase 3: Separate Data by Environment

For systems with multiple deployment targets, separate by environment:

```sql
-- Development environment
UPDATE Artifacts 
SET EnvironmentChannel = 1  -- Development
WHERE DeploymentTarget = 'DEV';

-- Testing environment
UPDATE Artifacts 
SET EnvironmentChannel = 2  -- Testing
WHERE DeploymentTarget = 'TEST';

-- UAT environment
UPDATE Artifacts 
SET EnvironmentChannel = 3  -- UAT
WHERE DeploymentTarget = 'UAT';

-- Production environment
UPDATE Artifacts 
SET EnvironmentChannel = 4  -- Production
WHERE DeploymentTarget = 'PROD';
```

---

## Phase 4: Add Database Constraints

Enforce channel constraints:

```sql
-- Prevent NULL channels
ALTER TABLE Artifacts
ALTER COLUMN ReleaseChannel INT NOT NULL;

ALTER TABLE Artifacts
ALTER COLUMN EnvironmentChannel INT NOT NULL;

-- Constrain valid values
ALTER TABLE Artifacts
ADD CONSTRAINT FK_ReleaseChannel 
    CHECK (ReleaseChannel IN (1, 2, 3, 4));

ALTER TABLE Artifacts
ADD CONSTRAINT FK_EnvironmentChannel 
    CHECK (EnvironmentChannel IN (1, 2, 3, 4, 5));
```

---

## Phase 5: Create Indexes

Optimize queries with channel-aware indexes:

```sql
-- Main composite index for channel-based queries
CREATE CLUSTERED INDEX IX_Artifacts_TenantChannels 
ON Artifacts(TenantId, ReleaseChannel, EnvironmentChannel);

-- Partial indexes for common patterns
CREATE INDEX IX_Stable_Prod ON Artifacts(TenantId, CreatedAt)
WHERE ReleaseChannel = 3 AND EnvironmentChannel = 4;

CREATE INDEX IX_Beta_Test ON Artifacts(TenantId, CreatedAt)
WHERE ReleaseChannel = 2 AND EnvironmentChannel = 2;

CREATE INDEX IX_Alpha_Dev ON Artifacts(TenantId, CreatedAt)
WHERE ReleaseChannel = 1 AND EnvironmentChannel = 1;
```

**Performance Impact:**
- Queries become more selective
- Index scans much faster than table scans
- Database optimizer has more options

---

## Phase 6: Update Application Code

Gradually migrate application code to use channels:

### Step 1: Create Repository Extensions

```csharp
// Extension methods for channel filtering
public static class ChannelQueryExtensions
{
    public static IQueryable<T> InChannel<T>(
        this IQueryable<T> query,
        ReleaseChannel release,
        EnvironmentChannel env)
        where T : IChannelScoped
    {
        return query
            .Where(e => e.ReleaseChannel == release)
            .Where(e => e.EnvironmentChannel == env);
    }
    
    public static IQueryable<T> InProduction<T>(this IQueryable<T> query)
        where T : IChannelScoped
    {
        return query.InChannel(ReleaseChannel.Stable, EnvironmentChannel.Production);
    }
}
```

### Step 2: Update Entity Constructors

```csharp
public class Artifact : IChannelScoped
{
    // Constructor automatically sets channels (migration)
    public Artifact(string name, ReleaseChannel? releaseChannel = null, EnvironmentChannel? envChannel = null)
    {
        Name = name;
        ReleaseChannel = releaseChannel ?? ReleaseChannel.Stable;      // Default
        EnvironmentChannel = envChannel ?? EnvironmentChannel.Production;  // Default
    }
}

// Usage: Old code still works with defaults
var artifact = new Artifact("MyForm");  // Gets Stable × Production

// New code can specify channels explicitly
var betaTestArtifact = new Artifact("MyForm", ReleaseChannel.Beta, EnvironmentChannel.Testing);
```

### Step 3: Update Queries Incrementally

```csharp
// Before: Query without channel filter (gets all channels)
var artifacts = await _context.Artifacts
    .Where(a => a.TenantId == tenantId)
    .ToListAsync();

// After: Query with explicit channel context
var artifacts = await _context.Artifacts
    .InChannel(releaseChannel, environmentChannel)
    .Where(a => a.TenantId == tenantId)
    .ToListAsync();
```

---

## Phase 7: Enforce in Application Code

Add validation to prevent cross-channel operations:

```csharp
public class DualChannelValidator
{
    public void ValidateOperation(
        IChannelScoped entity,
        ReleaseChannel contextRelease,
        EnvironmentChannel contextEnv)
    {
        if (entity.ReleaseChannel != contextRelease)
            throw new ChannelMismatchException(
                $"Release channel mismatch: entity is {entity.ReleaseChannel}, context is {contextRelease}");
        
        if (entity.EnvironmentChannel != contextEnv)
            throw new ChannelMismatchException(
                $"Environment channel mismatch: entity is {entity.EnvironmentChannel}, context is {contextEnv}");
    }
}

// Usage in repositories
public async Task<T?> GetByIdAsync(Guid id, ReleaseChannel release, EnvironmentChannel env)
{
    var entity = await _context.Set<T>().FindAsync(id);
    
    if (entity != null)
        _validator.ValidateOperation(entity, release, env);
    
    return entity;
}
```

---

## Phase 8: Remove Defaults, Make Required

After code migration complete, remove defaults:

```sql
-- Remove defaults (columns now explicitly required)
ALTER TABLE Artifacts 
ALTER COLUMN ReleaseChannel INT NOT NULL;

ALTER TABLE Artifacts
ALTER COLUMN EnvironmentChannel INT NOT NULL;

-- All creation code now explicitly specifies channels
-- Fail fast if missing channels
```

---

## Migration Timeline

Typical timeline for large system:

```
Month 1: Phases 1-2 (add columns, classify data)
Month 2: Phase 3 (separate by environment)
Month 3: Phases 4-5 (constraints and indexes)
Month 4-6: Phase 6 (incrementally migrate code)
Month 7: Phase 7 (enforce validation)
Month 8: Phase 8 (remove defaults)
```

---

## Rollback Strategy

If issues encountered:

```sql
-- Rollback column addition (if needed very early)
ALTER TABLE Artifacts
DROP COLUMN ReleaseChannel, EnvironmentChannel;

-- Or: Restore defaults for safety
ALTER TABLE Artifacts
ALTER COLUMN ReleaseChannel INT DEFAULT 3;

ALTER TABLE Artifacts
ALTER COLUMN EnvironmentChannel INT DEFAULT 4;
```

**Important**: Have a tested rollback plan before starting Phase 6+

---

## Validation Checklist

After each phase:

- [ ] Data still exists (no data loss)
- [ ] Old queries still work (backward compatibility)
- [ ] No application errors
- [ ] Performance metrics stable or improved
- [ ] Database integrity checks pass

---

## Performance Considerations

### Query Performance During Migration

- Queries without channel filters will scan more rows (temporary)
- Add indexes early (Phase 5) even before all code migrates
- Monitor query performance; add hints if needed

### Data Volume

Large tables (millions of rows) may need:
- Batched migration in phases (by date or partition)
- Off-peak hours for expensive updates
- Table locks for consistency

---

## Common Issues & Solutions

### Issue: NULL Channel Values

**Problem:** Old code creates entities without setting channels

**Solution:** 
```csharp
// Use constructor default or explicit validation
public class Artifact
{
    public Artifact() 
    {
        ReleaseChannel = ReleaseChannel.Stable;
        EnvironmentChannel = EnvironmentChannel.Production;
    }
}
```

### Issue: Cross-Channel Queries

**Problem:** Code queries across multiple channels unintentionally

**Solution:**
```csharp
// Always provide channel context
public async Task<T?> GetAsync(Guid id, ReleaseChannel release, EnvironmentChannel env)
{
    // Validates channel mismatch
    return await _repo.GetAsync(id, release, env);
}
```

### Issue: Performance Degradation

**Problem:** Queries slower after channel columns added

**Solution:**
- Ensure composite index created on (TenantId, ReleaseChannel, EnvironmentChannel)
- Check query plans for index usage
- Consider partial indexes for hot queries

---

## Related Documentation

- [Channel Progression](channel-progression.md)
- [Data Isolation](data-isolation.md)
- [Configuration & Governance](channel-governance-config.md)
- [Performance & Scaling](performance-and-scaling.md)
- [Best Practices & Testing](best-practices-and-testing.md)
