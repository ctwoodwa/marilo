# Performance & Scaling: Dual-Channel Optimization

> **Role in ICM:** Layer 3 - release-channel defaults for progression, isolation, deployment flow, and environment policy.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: performance-and-scaling | docs/icm-defaults/060-release-channels/performance-and-scaling.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Active  
> Owner: Platform Architecture Team  
> Last updated: 2026-01-22

## Overview

Dual channels enable sophisticated performance optimization and scalability by enabling independent resource allocation, partitioning, and optimization strategies per channel combination.

---

## Database Partitioning Strategy

### Composite Indexing

The foundation of performance is proper indexing on **both** channels:

```sql
-- Mandatory composite index on both channels
CREATE CLUSTERED INDEX IX_Artifact_TenantChannels 
ON Artifacts(TenantId, ReleaseChannel, EnvironmentChannel);

-- Improves query selectivity dramatically
-- Queries filter on all three columns, making index scans very efficient
```

### Physical Partitioning Options

#### Option 1: Partition by Environment

Separate databases reduce contention and enable environment-specific optimization:

```sql
-- Database 1: Development & Testing (non-production)
CREATE DATABASE {{ PRODUCT_NAME }}Dev
  -- Contains: Alpha/Beta × Development/Testing
  -- Size: Small (test data only)
  -- Backup: Optional
  -- Recovery: Not critical

-- Database 2: UAT (acceptance testing)
CREATE DATABASE {{ PRODUCT_NAME }}UAT
  -- Contains: Beta/Stable × UAT
  -- Size: Medium (realistic test data)
  -- Backup: Daily
  -- Recovery: Standard RTO/RPO

-- Database 3: Production & Support (high-value)
CREATE DATABASE {{ PRODUCT_NAME }}Prod
  -- Contains: Stable/LTS × Production/Support
  -- Size: Large (real customer data)
  -- Backup: Hourly + geo-redundancy
  -- Recovery: Aggressive RTO/RPO
```

**Benefits:**
- Dev/Test database can be small and fast
- Production database optimized for high throughput
- Support database optimized for point queries (issue diagnosis)
- Completely isolated data residency per environment

#### Option 2: Partition by Release

Stable/LTS data is long-lived; Alpha/Beta data is temporary:

```sql
-- Database 1: Alpha & Beta (temporary)
CREATE DATABASE {{ PRODUCT_NAME }}AlphaBeta
  -- Contains: Alpha/Beta × All environments
  -- Retention: Short-lived (months)
  -- Optimization: For rapid change

-- Database 2: Stable & LTS (permanent)
CREATE DATABASE {{ PRODUCT_NAME }}StableLTS
  -- Contains: Stable/LTS × All environments
  -- Retention: Permanent (years)
  -- Optimization: For archival and compliance
```

**Benefits:**
- Alpha/Beta database can be optimized for change (minimal indexes)
- Stable/LTS database optimized for query performance
- Different backup/retention strategies per database
- Easier to manage version lifecycle

#### Option 3: Hybrid Partitioning

Combine both strategies for maximum flexibility:

```sql
-- Environment + Release partitioning
-- Production Stable: {{ PRODUCT_NAME }}ProdStable (hot, permanent)
-- Production Support: {{ PRODUCT_NAME }}ProdSupport (point queries, permanent)
-- Production Development: {{ PRODUCT_NAME }}ProdDev (temporary, fast)
-- etc.
```

---

## Query Optimization

### Selective Filtering

Queries become highly selective with both channel filters:

```sql
-- Highly selective composite filter
SELECT * FROM FormInstances
WHERE TenantId = @tenantId                      -- 1000s of rows
  AND ReleaseChannel = @releaseChannel           -- ÷ 4 channels
  AND EnvironmentChannel = @environmentChannel   -- ÷ 5 environments
  AND CreatedAt >= @startDate;
  -- Result: Only dozens of rows scanned

-- Query plan uses clustered index seek (very efficient)
-- Scan range: highly constrained by channel filters
```

### Partial Indexes

Create partial indexes for common channel combinations:

```sql
-- Production queries (most common)
CREATE INDEX IX_Stable_Prod ON Artifacts(TenantId, CreatedAt)
WHERE ReleaseChannel = 3 AND EnvironmentChannel = 4;  -- Stable × Production

-- Test queries (frequent)
CREATE INDEX IX_Beta_Test ON Artifacts(TenantId, CreatedAt)
WHERE ReleaseChannel = 2 AND EnvironmentChannel = 2;  -- Beta × Testing

-- Development queries (frequent)
CREATE INDEX IX_Alpha_Dev ON Artifacts(TenantId, CreatedAt)
WHERE ReleaseChannel = 1 AND EnvironmentChannel = 1;  -- Alpha × Development
```

**Benefits:**
- Smaller indexes for common patterns
- Faster index seeks for common queries
- Reduced index maintenance overhead
- Lower query cost

---

## Caching Strategy

### Channel-Aware Cache Keys

Cache keys must include both channels to prevent data collisions:

```csharp
var cacheKey = $"artifacts:{tenantId}:{releaseChannel}:{environmentChannel}";
_cache.Set(cacheKey, data, GetTTLForChannel(releaseChannel, environmentChannel));

// Each channel combination has separate cache entries
// Alpha × Dev: artifacts:tenant-123:1:1
// Beta × Test: artifacts:tenant-123:2:2
// Stable × Prod: artifacts:tenant-123:3:4
// etc.
```

### TTL Strategy by Channel

Different channels need different cache lifetimes:

```csharp
private TimeSpan GetTTLForChannel(ReleaseChannel release, EnvironmentChannel env)
{
    return (release, env) switch
    {
        // Production: Stable content, longer cache
        (ReleaseChannel.Stable, EnvironmentChannel.Production) => TimeSpan.FromHours(4),
        
        // LTS/Support: Very stable, longest cache
        (ReleaseChannel.LTS, EnvironmentChannel.Support) => TimeSpan.FromHours(8),
        
        // UAT: Frequently changing during testing
        (ReleaseChannel.Beta, EnvironmentChannel.UAT) => TimeSpan.FromMinutes(30),
        
        // Testing: QA needs fresh data frequently
        (ReleaseChannel.Beta, EnvironmentChannel.Testing) => TimeSpan.FromMinutes(15),
        
        // Development: Developers need real-time data
        (ReleaseChannel.Alpha, EnvironmentChannel.Development) => TimeSpan.FromMinutes(5),
        
        _ => TimeSpan.FromHours(1)
    };
}
```

---

## Scalability Benefits

### Parallel Processing

Different channels can be processed independently without contention:

```csharp
// Alpha × Dev updates don't block Stable × Prod
Task.WaitAll(
    ProcessAlphaDevChannelAsync(),      // Async
    ProcessStableProdChannelAsync(),    // Async
    ProcessBetaTestChannelAsync()       // Async
);

// Each channel can have different:
// - Database servers
// - Cache clusters
// - Processing pipelines
// - Update frequencies
// - SLAs
```

### Resource Isolation

Each environment can have dedicated infrastructure:

```
Development Environment:
├─ Database: Single server, minimal resources
├─ Cache: Lightweight Redis instance
├─ Backup: Optional
└─ Cost: Minimal

Production Environment:
├─ Database: Multi-region replicas, optimized
├─ Cache: Cluster with failover
├─ Backup: Hourly + geo-redundancy
└─ Cost: Premium
```

### Cost Optimization

Right-size infrastructure per channel:

```csharp
public enum ChannelScale
{
    // Development: Cost-optimized
    [Infrastructure("single-vm", "shared-cache")]
    AlphaDevelopment,
    
    // Testing: Standard
    [Infrastructure("2-vms", "shared-cache")]
    BetaTesting,
    
    // Production: Premium
    [Infrastructure("multi-region", "managed-cache-cluster")]
    StableProduction,
    
    // LTS: Enterprise
    [Infrastructure("multi-region-ha", "premium-cache-cluster")]
    LTSSupport
}
```

**Cost Impact:**
- Alpha/Beta: 20% of total cost
- Stable/LTS: 80% of total cost
- Development: Minimal cost (shared resources OK)
- Production: Premium cost (high availability required)

---

## Query Performance Patterns

### Efficient Channel Scoping

```csharp
// Pattern 1: Query builder with automatic channel filtering
var query = _formRepository.InChannel(releaseChannel, environmentChannel)
    .Where(f => f.TenantId == tenantId)
    .Where(f => f.CreatedAt >= startDate)
    .AsNoTracking()
    .ToListAsync();

// Pattern 2: Explicit channel filtering
var results = await _context.Forms
    .Where(f => f.TenantId == tenantId)
    .Where(f => f.ReleaseChannel == releaseChannel)
    .Where(f => f.EnvironmentChannel == environmentChannel)
    .Where(f => f.CreatedAt >= startDate)
    .AsNoTracking()
    .ToListAsync();

// Both patterns generate identical SQL with tight index seeks
```

### Avoiding Full Table Scans

Channel filters prevent accidental full table scans:

```sql
-- ✓ Efficient: Uses channel-aware index
SELECT * FROM Forms
WHERE TenantId = @tenantId
  AND ReleaseChannel = @releaseChannel
  AND EnvironmentChannel = @environmentChannel
  AND CreatedAt >= @startDate;

-- ❌ Inefficient: Missing channel filter
SELECT * FROM Forms
WHERE TenantId = @tenantId
  AND CreatedAt >= @startDate;
-- Would scan millions of rows across all channels
```

---

## Monitoring & Performance Metrics

### Channel-Specific Metrics

Monitor performance per channel combination:

```csharp
public class ChannelMetrics
{
    public ReleaseChannel ReleaseChannel { get; set; }
    public EnvironmentChannel EnvironmentChannel { get; set; }
    
    // Performance
    public double AverageQueryTimeMs { get; set; }
    public int QueriesPerMinute { get; set; }
    public double CacheHitRate { get; set; }
    
    // Availability
    public double AvailabilityPercentage { get; set; }
    public int IncidentCount { get; set; }
    
    // Data
    public long DataSizeBytes { get; set; }
    public int RecordCount { get; set; }
}

// Production queries should be fast
var prodMetrics = GetMetricsFor(ReleaseChannel.Stable, EnvironmentChannel.Production);
Assert.True(prodMetrics.AverageQueryTimeMs < 200);  // p95 < 200ms

// Development queries can be slower
var devMetrics = GetMetricsFor(ReleaseChannel.Alpha, EnvironmentChannel.Development);
// No SLA required
```

---

## Related Documentation

- [Channel Progression](channel-progression.md)
- [Data Isolation](data-isolation.md)
- [Configuration & Governance](channel-governance-config.md)
- [Migration Guide](channel-migration.md)
- [Best Practices & Testing](best-practices-and-testing.md)
