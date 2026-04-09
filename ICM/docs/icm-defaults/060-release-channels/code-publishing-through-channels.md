# Code Publishing Through Release & Environment Channels

> **Role in ICM:** Layer 3 - release-channel defaults for progression, isolation, deployment flow, and environment policy.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: code-publishing-through-channels | docs/icm-defaults/060-release-channels/code-publishing-through-channels.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

## Overview

The {{ PRODUCT_NAME }} platform uses a structured two-dimensional channel system to manage how code and services are published, deployed, and distributed to users. This document explains how software moves through **Release Channels** (maturity levels) and **Environment Channels** (deployment stages).

---

## Release Channels: Publishing Maturity Levels

Release channels define the stability and readiness of code versions. Every service, microservice, and code artifact is published to a specific release channel based on its maturity.

### The Four Release Channels

```
Alpha (Internal Development)
  ↓ validated by development team
Beta (Early Access & UAT)
  ↓ validated through testing
Stable (General Availability)
  ↓ proven in production
LTS (Long-Term Support)
```

### Alpha Release: Very Early, Unstable

**Purpose**: Internal development builds with experimental features

**Characteristics**:
- Breaking changes expected and encouraged
- Frequent updates (multiple per day)
- Minimal testing, high risk
- For internal teams and trusted testers only
- Features may be completely removed between builds

**Example**:
```
FormService v3.3.0-alpha.1
├─ New field validation framework (may change API)
├─ Refactored data serialization (not backward compatible)
└─ Experimental AI-powered form suggestions
```

**Users**: Developers, architects, bleeding-edge testers

### Beta Release: Feature-Complete, Potentially Buggy

**Purpose**: Early access for public or chosen groups; User Acceptance Testing

**Characteristics**:
- All planned features present
- API stable, but may have quality issues
- Candidate for production (pending UAT approval)
- Updates weekly/bi-weekly
- Known issues documented

**Example**:
```
FormService v3.3.0-beta.2
├─ Field validation framework stable (backward compatible)
├─ New PDF export feature (ready for UAT)
├─ Known issue: Special characters in form names
└─ Approved for user acceptance testing
```

**Users**: QA teams, business analysts, chosen early adopters

### Stable Release: Fully Tested, Reliable

**Purpose**: Production-ready versions for general users

**Characteristics**:
- Thoroughly tested, proven in production
- No known critical bugs
- Full backward compatibility
- Enterprise-grade stability
- Updates monthly/quarterly

**Example**:
```
FormService v3.3.0
├─ Field validation framework (proven stable for 2 months)
├─ PDF export feature (used by 10,000+ users)
├─ Advanced search capabilities
└─ Full SLA support (99.99% uptime)
```

**Users**: All end users, customers, production systems

### LTS Release: Long-Term Support

**Purpose**: Stable versions with guaranteed long-term support

**Characteristics**:
- No new features (security/critical fixes only)
- Guaranteed support for 3-5 years
- Backward compatibility maintained forever
- Performance optimized
- Conservative testing approach

**Example**:
```
FormService v3.2.0-LTS (supported until Jan 2029)
├─ Security patch: SQL injection vulnerability fix
├─ Critical fix: Database connection pooling issue
├─ No new features
└─ Approved for migration from earlier versions
```

**Users**: Enterprise customers, mission-critical systems

---

## Environment Channels: Deployment Stages

Environment channels define where code runs and what purpose it serves. Code progresses through environments in a defined order, with each stage adding confidence and validation.

### The Five Environment Channels

```
Development
  ↓ (code complete)
Testing/QA
  ↓ (tests pass)
UAT
  ↓ (business approves)
Production
  ↓ (issues found)
Support
```

### Development Environment: Active Coding

**Purpose**: Where developers write and initially test code

**Characteristics**:
- Rapid, frequent changes (multiple per day)
- Unstable and unreliable
- Limited infrastructure (cost-optimized)
- Synthetic/test data only
- Automatic deployment on code commit

**Typical Workflow**:
```
Developer writes code
  ↓ (commit to main)
Automated build triggered
  ↓ (unit tests run)
Deploy to Development environment
  ↓ (smoke tests in Dev)
Developer tests manually
  ↓ (feature complete)
Promote to Testing when ready
```

**Infrastructure**: Single instance, minimal resources, auto-scaling disabled

### Testing/QA Environment: Comprehensive Validation

**Purpose**: QA teams validate functionality, integration, and performance

**Characteristics**:
- Stable, production-like configuration
- Comprehensive test suite execution
- Edge case and stress testing
- Performance baseline establishment
- Updated daily/weekly

**Typical Workflow**:
```
New beta release ready
  ↓
Deploy to Testing environment
  ↓
Automated test suite runs (2-4 hours)
├─ Unit tests
├─ Integration tests
├─ Performance tests
├─ Regression tests
└─ Security scans
  ↓
QA team manual testing begins
  ↓
Bugs logged if found
  ↓
Fixed in Development, re-tested
  ↓ (when all tests pass)
Approve for UAT
```

**Infrastructure**: Replicated production topology, high-availability disabled, synthetic data

### UAT Environment: User Acceptance Testing

**Purpose**: Business stakeholders validate requirements and approve for production

**Characteristics**:
- Exact replica of Production infrastructure
- Real-world business scenarios tested
- Masked PII but realistic data patterns
- Formal approval process required
- Updated weekly/monthly

**Typical Workflow**:
```
Release approved by QA
  ↓
Deploy to UAT environment
  ↓
Business team conducts acceptance testing
├─ Functional requirements validation
├─ Business process testing
├─ User interface review
├─ Performance under realistic load
└─ Integration with external systems
  ↓
UAT sign-off or issues logged
  ↓
Issues fixed, re-tested
  ↓ (when UAT passes)
Approved for Production deployment
```

**Infrastructure**: Replicated Production servers/databases, real-time alerting, masked production-like data

### Production Environment: Live Operations

**Purpose**: Live customer-facing system with real users and data

**Characteristics**:
- Maximum stability and reliability
- 24/7 monitoring and support
- Conservative change windows (specific times/days)
- Full backup/disaster recovery
- Quarterly updates

**Typical Operations**:
```
Approved release ready
  ↓
Change management approval process
  ↓
Blue-green deployment
├─ Staging servers updated first
├─ Health checks validate new version
└─ Traffic gradually shifted (canary)
  ↓
Monitoring for anomalies
  ↓
Rollback plan on standby
  ↓ (after 24 hours)
Production fully on new version
```

**Infrastructure**: High-availability, multi-region, CDN, load balancers, 99.99% SLA, real customer data

### Support Environment: Issue Resolution

**Purpose**: Isolated environment for troubleshooting and reproducing production issues

**Characteristics**:
- Copy of Production code and infrastructure
- Isolated from Production data streams
- For reproduction and diagnosis only
- Can run custom patches for testing
- Dedicated to specific issues

**Typical Usage**:
```
Customer reports production issue
  ↓
Support engineer investigates
  ↓
Create Support environment snapshot
  ↓
Reproduce issue in isolation
  ↓
Test potential fixes
  ↓
If successful, patch ready for Production
  ↓
Deploy to Production with safeguards
```

**Infrastructure**: Single instance, high-speed SSD, production schema, isolated data copy

---

## How Code Flows Through Both Dimensions

### Release Progression Example

```
Developer writes new feature
  ↓
Builds as Alpha v4.0.0-alpha.1
  ↓ (2 weeks of internal testing)
Graduates to Beta v4.0.0-beta.1
  ↓ (4 weeks of QA/UAT)
Released as Stable v4.0.0
  ↓ (6 months of production stability)
Designated as LTS v4.0.0-lts
  ↓
Supported until year 2029
```

### Environment Progression Example

```
Code written in Development environment
  ↓ (merged to main)
Deployed to Testing environment
  ↓ (automated test suite: pass/fail)
Deployed to UAT environment (if tests pass)
  ↓ (business validation)
Deployed to Production environment (if UAT approved)
  ↓ (monitoring 24/7)
Issues diagnosed in Support environment (if problems)
```

### Combined Path Example

```
Developer writes new form validation feature
  ↓
Alpha × Development
├─ Very unstable, breaking API changes
├─ Fresh commit every few minutes
└─ Automated deployment

  ↓ (feature stabilizes after 2 weeks)

Beta × Testing
├─ API stable, may have bugs
├─ Full test suite running
├─ Daily deployment with test results
└─ Known issues: edge cases with special characters

  ↓ (passed all automated tests)

Beta × UAT
├─ Feature-complete and stable
├─ Business team validates functionality
├─ Weekly deployments
└─ Approved: "Ready for production"

  ↓ (business approval received)

Stable × Production
├─ Live to all users worldwide
├─ 99.99% uptime SLA
├─ Quarterly release cycle
└─ Full monitoring and support

  ↓ (proven stable for 6 months)

LTS × Support
├─ Enterprise version, long-term support
├─ Security patches only
├─ 3-year maintenance guarantee
└─ Available for migration path
```

---

## Subscription Model: Audience Targeting

Different users/groups subscribe to different release channels for each environment:

### Developer Audience

```
Development: Alpha × Development
  ↓ Direct access to bleeding-edge code
  ↓ Can change features and APIs
  ↓ Updated hourly
```

### QA/Test Team

```
Testing: Beta × Testing
  ↓ Stable API, feature-complete
  ↓ Full test suite passing
  ↓ Updated daily
```

### Business Users

```
UAT: Beta × UAT
  ↓ Business requirement validation
  ↓ Realistic scenarios
  ↓ Sign-off process

Production: Stable × Production
  ↓ Proven features only
  ↓ Full support and SLA
  ↓ Conservative updates
```

### Enterprise Customers

```
Production: Stable × Production
  ↓ General production release

OR

Support: LTS × Support
  ↓ Long-term stability commitment
  ↓ No breaking changes ever
  ↓ Security updates 3-5 years
```

---

## Release Management Workflow

### Phase 1: Development (Alpha × Development)

```
Developer → Code commit
  ↓
Automated build
  ↓
Alpha version published
  ↓
Deploy to Development
  ↓
Smoke tests
  ↓
Developer validates locally
```

### Phase 2: Feature Testing (Beta × Testing)

```
Developer → Feature complete
  ↓
Beta version published
  ↓
Deploy to Testing
  ↓
Comprehensive test suite (automated)
├─ Unit tests (5 minutes)
├─ Integration tests (10 minutes)
├─ Performance tests (20 minutes)
├─ Regression tests (30 minutes)
└─ Security scan (10 minutes)
  ↓
QA manual testing
├─ Functional validation
├─ Edge case testing
└─ Performance validation
  ↓
Test report → Development for fixes
```

### Phase 3: UAT & Approval (Beta × UAT)

```
All tests pass
  ↓
Beta version deployed to UAT
  ↓
Business team tests
├─ Requirement coverage
├─ Workflow validation
├─ Integration testing
└─ User acceptance sign-off
  ↓
Approval decision
├─ APPROVED → Proceed to Production
└─ REJECTED → Back to Development for fixes
```

### Phase 4: Production Release (Stable × Production)

```
UAT approved
  ↓
Build published as Stable version
  ↓
Formal change management review
  ↓
Schedule change window (e.g., Tuesday 2-4 AM)
  ↓
Blue-green deployment
├─ Stage new version on standby servers
├─ Health checks validate new code
├─ Canary traffic: 1% → 5% → 25% → 100%
└─ Monitor for issues
  ↓
Monitor for 24+ hours
  ↓
Rollback plan on standby
  ↓
Declare success
```

### Phase 5: Enterprise Support (LTS × Support)

```
Stable version used for 6+ months
  ↓
Strong demand for long-term stability?
  ↓
Build published as LTS version
  ↓
Enterprise customers migrate
  ↓
Maintenance period: 3-5 years
├─ Security patches applied immediately
├─ Critical bug fixes included
└─ No new features, guaranteed compatibility
```

---

## Key Principles

### 1. Strict Progression

Code must flow through channels in defined order. No skipping stages.

```
❌ Cannot do: Alpha → Production (skip Beta, Testing, UAT)
✅ Must do: Alpha → Beta → Testing → UAT → Production
```

### 2. Independence

Release channels and environment channels are independent. Code can be any release at any environment.

```
Examples:
- Alpha × Development (development version in dev environment)
- Beta × Testing (early version in test environment)
- Stable × Production (general release in production)
- LTS × Support (long-term version in support environment)
```

### 3. Subscription

Users subscribe to combinations, not just environments.

```
Developer → subscribes to → Alpha × Development
QA engineer → subscribes to → Beta × Testing
End user → subscribes to → Stable × Production
Enterprise customer → subscribes to → LTS × Support
```

### 4. Separation

Channels create absolute boundaries. Code in one channel cannot access or affect another.

```
Alpha code never affects Beta
Development code never affects Production
Test data never contaminates Production
```

### 5. Auditing

Every channel transition is logged with full traceability.

```
Who promoted code?
When did it transition?
What version?
Why (approval/reason)?
What changed?
```

---

## Benefits of Two-Dimensional Channels

### For Developers

- Work on Alpha features without affecting others
- Push code frequently without production risk
- Control when features reach different audiences
- Easy rollback if issues discovered

### For QA/Testing

- Stable APIs for testing
- Comprehensive test environment
- Clear pass/fail metrics
- Reproducible test data

### For Business

- Control feature availability
- Validate functionality before release
- Manage user expectations
- Plan rollouts strategically

### For Operations

- Manage infrastructure per environment needs
- Scale Dev/Test/Prod independently
- Different monitoring/alerting per environment
- Clear deployment procedures

### For Users

- Choose update frequency (Alpha/Beta/Stable/LTS)
- Predictable release schedule
- Time to prepare for updates
- Enterprise support options

---

## Common Workflows

### Feature Development

```
Feature idea
  ↓ develop as Alpha in Development
  ↓ integrate and stabilize as Beta in Testing
  ↓ validate requirements in Beta in UAT
  ↓ release as Stable in Production
  ↓ monitor for 6 months
  ↓ mark as LTS in Support
```

### Bug Fix

```
Bug found in Production (Stable version)
  ↓ diagnose in Support environment
  ↓ develop fix in Alpha in Development
  ↓ test fix as Beta in Testing
  ↓ release patch as Stable in Production
```

### Enterprise Migration

```
Customer on Stable wants long-term support
  ↓ migrate from Stable to LTS version
  ↓ deploy to Support environment
  ↓ receive critical fixes for 3+ years
```

---

## Summary

The two-dimensional channel system (Release × Environment) provides comprehensive control over how code is published, tested, deployed, and supported:

**Release Channels** handle **what** and **to whom**:
- Alpha for developers
- Beta for early adopters
- Stable for general users
- LTS for enterprises

**Environment Channels** handle **where** and **when**:
- Development for coding
- Testing for validation
- UAT for approval
- Production for live use
- Support for troubleshooting

Together, they enable **controlled software lifecycle management** from initial development through long-term enterprise support.

---

**Related Documentation**:
- [Release & Environment Channels (Platform Architecture)](./release-channels.md)
- [Forms: Channel Architecture](../10-business-capabilities/forms/05-channels/channel-architecture.md)
- [Deployment Architecture](./deployment-architecture.md)

**Last Updated**: January 2026  
**Status**: Active  
**Applies To**: All code, services, and artifacts in {{ PRODUCT_NAME }} platform
