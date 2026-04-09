# Best Practices & Testing: Dual-Channel Architecture

> **Role in ICM:** Layer 3 - release-channel defaults for progression, isolation, deployment flow, and environment policy.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: best-practices-and-testing | docs/icm-defaults/060-release-channels/best-practices-and-testing.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Active  
> Owner: Platform Architecture Team  
> Last updated: 2026-01-22

## Overview

This document outlines best practices for working within dual-channel architecture and comprehensive testing strategies to ensure channel isolation and correctness.

---

## Best Practices

### Release Channel Management

#### ✅ DO

- **Follow strict progression**: Alpha → Beta → Stable → LTS
- **Test thoroughly before graduating** to next release channel
- **Mark releases as LTS** only after proven 6+ months Stable production history
- **Use feature flags** to control Alpha/Beta features in Stable release
- **Document breaking changes** with migration guides when bumping release channel
- **Support N-1 versions** during migration period (e.g., support both v1 and v2 for 6 months)

#### ❌ DON'T

- **Skip intermediate channels**: Never jump from Alpha directly to Stable
- **Break backward compatibility** in Stable or LTS (use major version bump)
- **Deploy Alpha/Beta to production users** without explicit opt-in
- **Remove deprecated endpoints** before completing migration period
- **Mix release channels** in same query
- **Forget deprecation warnings** before removing features

### Environment Channel Management

#### ✅ DO

- **Always flow through Development → Testing → UAT** sequentially
- **Deploy to Production, Training, and Support in parallel** after UAT approval
- **Use UAT environment exactly mirroring Production** to catch issues early
- **Isolate Support environment from Production** to avoid cross-contamination
- **Keep Development data synthetic/test only** (never real customer data)
- **Archive old Support environment data** after issues resolved

#### ❌ DON'T

- **Deploy to Production without Testing → UAT progression**
- **Skip UAT validation before Production deployment**
- **Mix Development/Test artifacts with Production**
- **Copy Production data into Development** (use masking/anonymization)
- **Deploy directly to Production** (always through dev pipeline)
- **Use same configuration for all channel combinations**

### Artifact Management

#### ✅ DO

- **Always include both channels in queries and operations**
- **Use coordinated promotion for related artifacts** (forms + workflows + reports)
- **Configure different retention policies per channel combination**
- **Test artifact combinations in UAT before Production deployment**
- **Version all artifacts** with both Release and Environment metadata
- **Document channel constraints** in schema/documentation

#### ❌ DON'T

- **Allow channel changes after artifact creation**
- **Query across release channels**
- **Query across environment channels**
- **Forget to set channels when creating artifacts**
- **Use defaults blindly** (always specify channels explicitly)

### Deployment Strategy

#### ✅ DO

- **Create test plans per environment channel before promotion**
- **Monitor carefully** after each channel progression
- **Have rollback plan** ready before deployment
- **Document promotions** with reasons and timestamps
- **Test approval workflows** for Production deployments
- **Validate data integrity** before each migration

#### ❌ DON'T

- **Never skip channel progression stages**
- **Never promote directly to Stable** (always through Beta)
- **Never deploy to Production without UAT approval**
- **Never make breaking changes in Stable** (use major version)

### Data Management

#### ✅ DO

- **Mask PII in non-Production environments** (remove/anonymize customer data)
- **Keep Development data synthetic/test only**
- **Use realistic test data in UAT** (masked customer data)
- **Archive old Support environment data** after issues resolved
- **Enforce data isolation** at repository level
- **Audit all channel promotions**

#### ❌ DON'T

- **Mix channel data in reports**
- **Copy Production data into Development**
- **Contaminate UAT with Developer artifacts**
- **Use real customer data in test environments**
- **Leave Support data indefinitely** (archive when issue resolved)

---

## Testing Strategy

### 1. Channel Isolation Tests

Test that isolation is enforced on **both** dimensions:

```csharp
[Theory]
[InlineData(ReleaseChannel.Alpha, EnvironmentChannel.Development)]
[InlineData(ReleaseChannel.Beta, EnvironmentChannel.Testing)]
[InlineData(ReleaseChannel.Stable, EnvironmentChannel.Production)]
[InlineData(ReleaseChannel.LTS, EnvironmentChannel.Support)]
public async Task Repository_RespectsBothChannelBoundaries(
    ReleaseChannel releaseChannel, 
    EnvironmentChannel environmentChannel)
{
    // Create artifact in specific channel combination
    var artifact = await CreateArtifactInChannels(releaseChannel, environmentChannel);
    
    // Verify other release channels cannot see it
    foreach (var otherRelease in GetOtherReleaseChannels(releaseChannel))
    {
        var artifacts = await _repository.GetAsync(
            _tenantId, otherRelease, environmentChannel);
        
        Assert.DoesNotContain(artifacts, a => a.Id == artifact.Id);
    }
    
    // Verify other environment channels cannot see it
    foreach (var otherEnv in GetOtherEnvironmentChannels(environmentChannel))
    {
        var artifacts = await _repository.GetAsync(
            _tenantId, releaseChannel, otherEnv);
        
        Assert.DoesNotContain(artifacts, a => a.Id == artifact.Id);
    }
}
```

### 2. Cross-Channel Operation Tests

Verify that cross-channel operations fail:

```csharp
[Fact]
public async Task CrossReleaseChannelQuery_Throws()
{
    var alphaArtifact = await CreateArtifactInChannels(
        ReleaseChannel.Alpha, EnvironmentChannel.Development);
    
    // Attempt cross-release-channel query
    await Assert.ThrowsAsync<ChannelMismatchException>(
        () => _repository.GetAsync(
            _tenantId, 
            ReleaseChannel.Beta,  // Different release
            EnvironmentChannel.Development));
}

[Fact]
public async Task CrossEnvironmentChannelQuery_Throws()
{
    var devArtifact = await CreateArtifactInChannels(
        ReleaseChannel.Stable, EnvironmentChannel.Development);
    
    // Attempt cross-environment-channel query
    await Assert.ThrowsAsync<ChannelMismatchException>(
        () => _repository.GetAsync(
            _tenantId, 
            ReleaseChannel.Stable,
            EnvironmentChannel.Production));  // Different environment
}

[Fact]
public async Task BothChannelsMismatched_Throws()
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

### 3. Progression Tests

Verify valid channel progressions work:

```csharp
[Theory]
[InlineData(ReleaseChannel.Alpha, ReleaseChannel.Beta)]
[InlineData(ReleaseChannel.Beta, ReleaseChannel.Stable)]
[InlineData(ReleaseChannel.Stable, ReleaseChannel.LTS)]
public async Task ReleaseChannelProgression_Validates(
    ReleaseChannel fromChannel, 
    ReleaseChannel toChannel)
{
    // Can promote from lower to higher release channel
    var artifact = await CreateArtifactInChannels(
        fromChannel, EnvironmentChannel.Development);
    
    var promoted = await _promotionService.PromoteAsync(
        artifact.Id, toChannel, EnvironmentChannel.Development);
    
    Assert.NotNull(promoted);
    Assert.Equal(toChannel, promoted.ReleaseChannel);
}

[Theory]
[InlineData(EnvironmentChannel.Development, EnvironmentChannel.Testing)]
[InlineData(EnvironmentChannel.Testing, EnvironmentChannel.UAT)]
[InlineData(EnvironmentChannel.UAT, EnvironmentChannel.Production)]
public async Task EnvironmentChannelProgression_Validates(
    EnvironmentChannel fromEnv,
    EnvironmentChannel toEnv)
{
    // Can promote from lower to higher environment channel
    var artifact = await CreateArtifactInChannels(
        ReleaseChannel.Stable, fromEnv);
    
    var promoted = await _promotionService.PromoteAsync(
        artifact.Id, ReleaseChannel.Stable, toEnv);
    
    Assert.NotNull(promoted);
    Assert.Equal(toEnv, promoted.EnvironmentChannel);
}
```

### 4. Invalid Progression Tests

Verify invalid progressions fail:

```csharp
[Theory]
[InlineData(ReleaseChannel.Beta, ReleaseChannel.Alpha)]  // Backward
[InlineData(ReleaseChannel.Alpha, ReleaseChannel.Stable)]  // Skips Beta
[InlineData(ReleaseChannel.LTS, ReleaseChannel.Stable)]  // Backward from LTS
public async Task InvalidReleaseProgression_Throws(
    ReleaseChannel fromChannel, 
    ReleaseChannel toChannel)
{
    var artifact = await CreateArtifactInChannels(
        fromChannel, EnvironmentChannel.Development);
    
    await Assert.ThrowsAsync<InvalidProgressionException>(
        () => _promotionService.PromoteAsync(
            artifact.Id, toChannel, EnvironmentChannel.Development));
}

[Theory]
[InlineData(EnvironmentChannel.Testing, EnvironmentChannel.Development)]  // Backward
[InlineData(EnvironmentChannel.Development, EnvironmentChannel.UAT)]  // Skips Testing
[InlineData(EnvironmentChannel.Production, EnvironmentChannel.Testing)]  // Backward
public async Task InvalidEnvironmentProgression_Throws(
    EnvironmentChannel fromEnv,
    EnvironmentChannel toEnv)
{
    var artifact = await CreateArtifactInChannels(
        ReleaseChannel.Stable, fromEnv);
    
    await Assert.ThrowsAsync<InvalidProgressionException>(
        () => _promotionService.PromoteAsync(
            artifact.Id, ReleaseChannel.Stable, toEnv));
}
```

### 5. Full Workflow Tests

Test complete workflows for each channel combination:

```csharp
[Theory]
[MemberData(nameof(GetAllValidChannelCombinations))]
public async Task FullWorkflow_PerChannelCombination(
    ReleaseChannel release, 
    EnvironmentChannel env)
{
    // Test complete workflow for each channel combination
    var artifact = await CreateAsync(release, env);
    var instance = await CreateInstanceAsync(artifact);
    var results = await QueryAsync(artifact.Id, release, env);
    
    Assert.NotEmpty(results);
    Assert.All(results, r => 
        r.ReleaseChannel == release && 
        r.EnvironmentChannel == env);
}

public static IEnumerable<object[]> GetAllValidChannelCombinations()
{
    // Test common/valid combinations
    yield return new object[] { ReleaseChannel.Alpha, EnvironmentChannel.Development };
    yield return new object[] { ReleaseChannel.Beta, EnvironmentChannel.Testing };
    yield return new object[] { ReleaseChannel.Beta, EnvironmentChannel.UAT };
    yield return new object[] { ReleaseChannel.Stable, EnvironmentChannel.Production };
    yield return new object[] { ReleaseChannel.Stable, EnvironmentChannel.Training };
    yield return new object[] { ReleaseChannel.LTS, EnvironmentChannel.Support };
}
```

### 6. Data Integrity Tests

Verify data isolation at database level:

```csharp
[Fact]
public async Task Database_Respects_ChannelBoundaries()
{
    // Create data in different channels
    await CreateArtifactInChannels(ReleaseChannel.Alpha, EnvironmentChannel.Development);
    await CreateArtifactInChannels(ReleaseChannel.Stable, EnvironmentChannel.Production);
    
    // Execute raw SQL queries without channel filters
    var allRecords = await _context.Artifacts.FromSqlRaw(
        "SELECT * FROM Artifacts WHERE TenantId = @tenantId",
        new SqlParameter("@tenantId", _tenantId))
        .ToListAsync();
    
    // Verify we got both (database has both)
    Assert.Equal(2, allRecords.Count);
    
    // But ORM queries must include channel filters
    var alphaRecords = await _context.Artifacts
        .Where(a => a.TenantId == _tenantId)
        .Where(a => a.ReleaseChannel == ReleaseChannel.Alpha)
        .Where(a => a.EnvironmentChannel == EnvironmentChannel.Development)
        .ToListAsync();
    
    Assert.Single(alphaRecords);
}
```

### 7. Performance Tests

Verify queries perform well with channel filters:

```csharp
[Fact]
public async Task Query_WithChannelFilters_PerformsEfficiently()
{
    // Create 10,000 artifacts across different channels
    for (int i = 0; i < 10000; i++)
    {
        var release = (ReleaseChannel)(i % 4 + 1);
        var env = (EnvironmentChannel)(i % 5 + 1);
        await CreateArtifactInChannels(release, env);
    }
    
    // Query with channel filters should be fast
    var stopwatch = Stopwatch.StartNew();
    var results = await _repository.GetAsync(
        _tenantId, 
        ReleaseChannel.Stable,
        EnvironmentChannel.Production);
    stopwatch.Stop();
    
    Assert.True(stopwatch.ElapsedMilliseconds < 100);  // Should be sub-100ms
}
```

---

## Test Organization

Organize tests into suites:

```
ChannelTests/
├── IsolationTests.cs (cross-channel prevention)
├── ProgressionTests.cs (valid/invalid progressions)
├── WorkflowTests.cs (end-to-end per channel)
├── DataIntegrityTests.cs (database-level)
└── PerformanceTests.cs (query efficiency)
```

---

## Continuous Verification

### Automated Checks

```csharp
// CI/CD pipeline check: Every commit must pass
- Channel isolation tests ✓
- Progression validation tests ✓
- Cross-channel prevention tests ✓
- Performance benchmarks ✓
- Query plan analysis ✓
```

### Regular Audits

**Monthly:**
- Review channel audit logs for anomalies
- Verify data isolation in production
- Check query performance trends
- Validate data retention policies

**Quarterly:**
- Comprehensive channel security review
- Test rollback procedures
- Validate compliance logging
- Performance tuning review

---

## Related Documentation

- [Channel Progression](channel-progression.md)
- [Data Isolation](data-isolation.md)
- [Configuration & Governance](channel-governance-config.md)
- [Performance & Scaling](performance-and-scaling.md)
- [Migration Guide](channel-migration.md)
