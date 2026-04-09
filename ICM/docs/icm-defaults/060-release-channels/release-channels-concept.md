# Release Channels: Build Maturity Lifecycle

> **Role in ICM:** Layer 3 - release-channel defaults for progression, isolation, deployment flow, and environment policy.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: release-channels-concept | docs/icm-defaults/060-release-channels/release-channels-concept.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Active  
> Owner: Platform Architecture Team  
> Last updated: 2026-01-22

## Overview

**Release Channels** describe the stability and readiness of software versions. They create a structured path for software updates, allowing developers to distribute versions to specific user groups based on stability.

---

## Release Channel Definitions

### Alpha

**Status**: Very early, unstable  
**Audience**: Internal teams only  
**Breaking Changes**: Expected, encouraged  
**Use**: Feature development, experimentation

- Bleeding-edge features under active development
- Breaking changes are expected and acceptable
- No backward compatibility guarantees
- Internal testing and proof-of-concept only

### Beta

**Status**: Early access, feature-complete  
**Audience**: Public or chosen groups  
**Breaking Changes**: Rare, announced  
**Use**: Feature validation, external testing (UAT)

- All planned features implemented
- Known issues may exist
- Potentially buggy but functionally complete
- Good for user acceptance testing and feedback

### Stable

**Status**: Fully tested, reliable  
**Audience**: General users  
**Breaking Changes**: Never, migration paths provided  
**Use**: Production deployment, general availability

- Thoroughly tested and proven
- No breaking changes without major version bump
- Recommended for all production systems
- Receives performance optimizations and bug fixes

### LTS (Long-Term Support)

**Status**: Long-term stability commitment  
**Audience**: Enterprise customers  
**Breaking Changes**: Never  
**Use**: Extended support, critical updates only

- Frozen feature set (no new features)
- Receives only critical security and stability updates
- Supported for years
- Ideal for enterprises needing maximum stability

---

## Software Publishing Flow

```
Developer writes code
    ↓
Alpha Release (Internal teams, early testers)
    ↓ (validated by internal team)
Beta Release (Public or chosen groups, new features, potentially buggy)
    ↓ (tested and stabilized)
Stable Release (General users, fully tested and reliable)
    ↓ (proven in production for 6+ months)
LTS Release (Enterprises, critical updates only)
```

### Publishing Process

Each release channel represents a maturity checkpoint:

1. **Code development** → Alpha release for internal validation
2. **Internal validation** → Beta release for external feedback
3. **Testing and stabilization** → Stable release for general availability
4. **Proven production use** → LTS release for enterprise support

---

## Feature Configuration by Release Channel

| Release | Feature Flags | New Features | Performance | Breaking Changes |
|---|---|---|---|---|
| **Alpha** | All experimental | Bleeding edge | Minimal optimization | Expected, encouraged |
| **Beta** | Most features | Recent features, likely stable | Good optimization | Rare, announced |
| **Stable** | Core features only | Proven features only | Optimized | Never, migration paths |
| **LTS** | Minimal, critical only | None | Highly optimized | Never |

---

## Promotion Rules

**Release channels progress unidirectionally:**

```
Alpha → Beta → Stable → LTS
```

### Promotion Requirements

- **Alpha → Beta**: Internal team validation complete
- **Beta → Stable**: Feature-complete, test suite passes
- **Stable → LTS**: 6+ months proven production history
- **No skipping**: Must progress through each channel in order

### Promotion Examples

```csharp
// Example: Promoting a form feature through release channels

// Phase 1: Alpha (internal)
var alphaRelease = new ServiceRelease
{
    ServiceName = "FormService",
    Version = "3.2.0",
    ReleaseChannel = ReleaseChannel.Alpha,      // Internal only
    EnvironmentChannel = EnvironmentChannel.Development
};

// Phase 2: Beta (UAT)
var betaRelease = new ServiceRelease
{
    ServiceName = "FormService",
    Version = "3.2.0",
    ReleaseChannel = ReleaseChannel.Beta,       // External testing
    EnvironmentChannel = EnvironmentChannel.Testing  // or UAT
};

// Phase 3: Stable (Production)
var stableRelease = new ServiceRelease
{
    ServiceName = "FormService",
    Version = "3.2.0",
    ReleaseChannel = ReleaseChannel.Stable,     // General availability
    EnvironmentChannel = EnvironmentChannel.Production
};

// Phase 4: LTS (Enterprise)
var ltsRelease = new ServiceRelease
{
    ServiceName = "FormService",
    Version = "3.2.0",
    ReleaseChannel = ReleaseChannel.LTS,        // Long-term support
    EnvironmentChannel = EnvironmentChannel.Support  // Or Production
};
```

---

## Key Characteristics

### Alpha Channel

✅ **DO**:
- Test thoroughly before promoting to Beta
- Allow breaking changes and major refactoring
- Gather internal feedback early

❌ **DON'T**:
- Deploy to production users
- Make backward-compatibility guarantees
- Skip Beta validation

### Beta Channel

✅ **DO**:
- Gather external feedback and bug reports
- Announce planned graduation to Stable
- Prepare migration guidance

❌ **DON'T**:
- Make breaking changes without announcement
- Deploy to all production users
- Skip Stable validation period

### Stable Channel

✅ **DO**:
- Support for minimum 2 years
- Provide migration path for any breaking changes
- Backport critical security fixes

❌ **DON'T**:
- Make breaking changes
- Add unstable experimental features
- Skip testing before promotion

### LTS Channel

✅ **DO**:
- Commit to 5+ year support
- Provide only critical security and stability updates
- Maintain strict backward compatibility

❌ **DON'T**:
- Add new features
- Make any breaking changes
- Remove supported functionality

---

## Related Documentation

- [Environment Channels Concept](environment-channels-concept.md)
- [Channel Progression](channel-progression.md)
- [Best Practices & Testing](best-practices-and-testing.md)
