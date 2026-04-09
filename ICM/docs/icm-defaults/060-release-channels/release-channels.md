# Release & Environment Channels

> **Role in ICM:** Layer 3 - release-channel defaults for progression, isolation, deployment flow, and environment policy.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: release-channels | docs/icm-defaults/060-release-channels/release-channels.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Active  
> Owner: Platform Architecture Team  
> Last updated: 2026-01-22

## Overview

{{ PRODUCT_NAME }} uses a **dual-channel architecture** to manage the complete software lifecycle across the entire platform:

1. **Release Channels** - Describe build maturity and audience eligibility
2. **Environment Channels** - Describe platform installation locations and deployment stages

Every piece of code, configuration, form definition, report template, workflow, and data artifact flows through both dimensions independently, enabling controlled, segmented software distribution to specific user groups while maintaining clean separation across development stages.

---

## The Two Channel Dimensions

### Release Channels (Build Maturity)

Describe the stability and readiness of software versions:

```csharp
public enum ReleaseChannel
{
    Alpha = 1,      // Very early, unstable - internal teams only, breaking changes expected
    Beta = 2,       // Early access, feature-complete, new features, potentially buggy (UAT)
    Stable = 3,     // Fully tested, reliable, production-ready versions
    LTS = 4         // Long-term support, critical security updates only, for enterprises
}
```

**See:** [Release Channels Concept](release-channels-concept.md)

---

### Environment Channels (Deployment Stages)

Describe where software runs and its operational purpose:

```csharp
public enum EnvironmentChannel
{
    Development = 1,  // DEV - Active development, coding, frequent updates, unstable
    Testing = 2,      // TEST/QA - Dedicated testing, validation, stable simulated environment
    UAT = 3,          // UAT - User acceptance testing, business approval, pre-production
    Production = 4,   // PROD - Live environment, actual customer operations
    Support = 5       // SUPPORT - Post-release fixes, issue reproduction, post-deployment QA
}
```

**See:** [Environment Channels Concept](environment-channels-concept.md)

---

## Channel Matrix

| Release × Environment | Purpose | Stability | Users | Update Frequency |
|---|---|---|---|---|
| **Alpha × Development** | Feature development, experimentation | Very Low | Developers, Designers | Hourly/Daily |
| **Beta × Testing** | QA validation, bug verification | Medium | QA Engineers, Test Automation | Daily/Weekly |
| **Beta × UAT** | User acceptance testing, business approval | Medium-High | Business Analysts, End Users | Weekly |
| **Stable × Production** | Live business operations | Maximum | All Production Users | Monthly/Quarterly |
| **Stable × Training** | End-user training, onboarding | High | Trainers, New Users | Monthly |
| **LTS × Support** | Enterprise support, critical fixes only | Maximum | Enterprise Customers | Security/Stability |

---

## Related Documentation

- [Release Channels Concept](release-channels-concept.md)
- [Environment Channels Concept](environment-channels-concept.md)
- [Channel Progression](channel-progression.md)
- [Data Isolation](data-isolation.md)
- [Configuration & Governance](channel-governance-config.md)
- [Performance & Scaling](performance-and-scaling.md)
- [Migration Guide](channel-migration.md)
- [Best Practices & Testing](best-practices-and-testing.md)
- [Code Publishing Through Channels](111-code-publishing-through-channels.md)

---

## Key Principle

**Channels are primitives, not features.** Every artifact is channel-scoped on **both** dimensions from creation, ensuring clean separation and controlled flow throughout the platform.
