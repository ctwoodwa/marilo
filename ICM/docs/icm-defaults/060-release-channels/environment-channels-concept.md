# Environment Channels: Deployment Stages

> **Role in ICM:** Layer 3 - release-channel defaults for progression, isolation, deployment flow, and environment policy.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: environment-channels-concept | docs/icm-defaults/060-release-channels/environment-channels-concept.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Active  
> Owner: Platform Architecture Team  
> Last updated: 2026-01-22

## Overview

**Environment Channels** describe where software runs and its operational purpose. They streamline development by separating code stages, from initial coding through production stabilization.

---

## Environment Channel Definitions

### Development (DEV)

**Purpose**: Active coding, unit testing, rapid iteration  
**Stability**: Very Low  
**Users**: Developers, Architects  
**Update Frequency**: Hourly/Daily

- Where developers write and test code before formal QA
- Minimal infrastructure (cost-effective)
- Sample/synthetic data, frequently wiped
- 30-day data retention
- No formal backup required (as-needed basis)

**Example Workflow**: Developer pushes code → automated unit tests run → integration with other services

### Testing/QA (TEST)

**Purpose**: Comprehensive functionality and regression testing  
**Stability**: Medium  
**Users**: QA Engineers, Test Automation  
**Update Frequency**: Daily/Weekly

- Dedicated testing and validation environment
- Stable, production-like infrastructure
- Test scenarios, edge cases, performance data
- 90-day data retention
- Daily backup

**Example Workflow**: New Beta release deployed → QA runs full test suite → automated regression tests pass

### User Acceptance Testing (UAT)

**Purpose**: Business validation before production  
**Stability**: Medium-High  
**Users**: Business Analysts, End Users, Product Owners  
**Update Frequency**: Weekly

- Replica of production environment
- Realistic business scenarios
- Masked PII (anonymized customer data)
- 90-180 day data retention
- Daily backup with disaster recovery

**Example Workflow**: Finance team validates new approval workflow before going live → business sign-off → ready for production

### Production (PROD)

**Purpose**: Live operations with real customer data  
**Stability**: Maximum  
**Users**: All End Users, Customers  
**Update Frequency**: Monthly/Quarterly

- High-availability, multi-region infrastructure
- Real customer data, highly sensitive
- Permanent data retention
- Hourly backup with disaster recovery
- 99.99% SLA target

**Example Workflow**: Form builder update deployed to all users worldwide → monitoring alerts active → incidents immediately escalated

### Support (SUPPORT)

**Purpose**: Post-release issue resolution and long-term stability  
**Stability**: Maximum  
**Users**: Support Engineers, Enterprise Customers  
**Update Frequency**: Critical fixes only

- Production-grade infrastructure
- Real but isolated data for issue reproduction
- Permanent data retention
- Hourly backup with disaster recovery
- Used to reproduce production issues

**Example Workflow**: Reproduce production issue in isolated Support environment → apply fix → validate fix in UAT before production deployment

---

## Environment Progression Flow

Code flows through environments sequentially:

```
Development (DEV)
    ↓ (code complete, ready for testing)
Testing/QA (TEST)
    ↓ (all tests passing, approved)
User Acceptance Testing (UAT)
    ↓ (business approved, ready for deployment)
Production (PROD)
    ↓ (proving in production, handling issues)
Support (SUPPORT)
    ↓ (long-term stability, issue reproduction)
```

### Progression Rules

- **Development → Testing**: Code completion required
- **Testing → UAT**: All tests must pass
- **UAT → {Production, Training, Support}**: Business approval required (can deploy to multiple environments in parallel)
- **Production ↔ Support**: Bidirectional (for issue troubleshooting and reproduction)

---

## Infrastructure Configuration by Environment

| Environment | Retention | SLA | Updates | Backup | Availability |
|---|---|---|---|---|---|
| **Development** | 30 days | None | Daily | On-demand | 80% |
| **Testing** | 90 days | 99% | Daily/Weekly | Daily | 95% |
| **UAT** | 180 days | 99.5% | Weekly | Daily | 99% |
| **Production** | Permanent | 99.99% | Monthly/Quarterly | Hourly | 99.99% |
| **Support** | Permanent | 99.99% | Critical only | Hourly | 99.99% |

---

## Data Characteristics by Environment

### Development Data

- **Source**: Synthetic/test data only
- **PII Handling**: Not applicable (no real data)
- **Retention**: 30 days
- **Refresh**: Frequently wiped
- **Backup**: As-needed only

**Use**: Testing code changes, feature development, experimentation

### Testing Data

- **Source**: Realistic test scenarios with edge cases
- **PII Handling**: May contain realistic patterns (not real customer data)
- **Retention**: 90 days
- **Refresh**: Weekly full reset
- **Backup**: Daily

**Use**: Functional testing, regression testing, performance validation

### UAT Data

- **Source**: Realistic business scenarios
- **PII Handling**: Masked/anonymized customer data
- **Retention**: 90-180 days
- **Refresh**: Monthly or on-demand
- **Backup**: Daily with disaster recovery

**Use**: Business approval, user acceptance, final validation before production

### Production Data

- **Source**: Real customer data
- **PII Handling**: Full security controls, compliance required
- **Retention**: Permanent (regulatory requirement)
- **Refresh**: Never (live data only)
- **Backup**: Hourly with geo-redundancy

**Use**: Live operations, real customer usage

### Support Data

- **Source**: Real but isolated for issue reproduction
- **PII Handling**: Full security controls
- **Retention**: Permanent (for historical issue reference)
- **Refresh**: Snapshot from Production (as-needed)
- **Backup**: Hourly with geo-redundancy

**Use**: Reproduce and diagnose production issues in isolation

---

## Key Characteristics

### Development Environment

✅ **DO**:
- Iterate rapidly with frequent deployments
- Use synthetic or test data exclusively
- Clean up resources daily
- Test code changes in isolation

❌ **DON'T**:
- Use production data
- Make production backups
- Deploy to production users
- Assume high availability

### Testing Environment

✅ **DO**:
- Run full test suites automatically
- Use realistic (but non-real) data
- Keep a stable baseline
- Test against production-like infrastructure

❌ **DON'T**:
- Use production data
- Skip automated regression tests
- Deploy to production users
- Leave long-running tests incomplete

### UAT Environment

✅ **DO**:
- Mirror production exactly
- Use realistic business scenarios
- Get explicit business sign-off
- Test with actual users/stakeholders

❌ **DON'T**:
- Use unmasked real customer data
- Skip business validation
- Deploy to production without UAT approval
- Deploy manually without change management

### Production Environment

✅ **DO**:
- Follow strict change management
- Monitor continuously
- Have incident response procedures
- Maintain backups and disaster recovery

❌ **DON'T**:
- Test experimental features
- Make ad-hoc changes
- Skip UAT validation
- Use production for development

### Support Environment

✅ **DO**:
- Keep isolated from production
- Use snapshots for issue reproduction
- Archive after issues resolved
- Maintain historical data for compliance

❌ **DON'T**:
- Share data with production
- Modify production from support issues
- Use for active development
- Delete historical issue data

---

## Related Documentation

- [Release Channels Concept](release-channels-concept.md)
- [Channel Progression](channel-progression.md)
- [Data Isolation](data-isolation.md)
- [Configuration & Governance](channel-governance-config.md)
- [Best Practices & Testing](best-practices-and-testing.md)
