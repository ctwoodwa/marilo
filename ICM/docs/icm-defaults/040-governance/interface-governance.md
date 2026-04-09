# Interface Governance

> **Role in ICM:** Layer 3 - governance defaults for versioning, contracts, compatibility, and interface lifecycle controls.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: interface-governance | docs/icm-defaults/040-governance/interface-governance.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: Architecture Team  
> Last updated: 2026-01-22

## Purpose

Interface Governance defines how {{ PRODUCT_NAME }} services **evolve APIs and contracts** without breaking consumers. This document establishes the framework for:
- Versioning strategy for APIs and events
- Contract testing requirements
- Breaking change detection and prevention
- Deprecation policies
- Interface documentation standards

**Steenberg principle:** *"Design black box boundaries with perfect interfaces. Implementation can change freely; contracts cannot."*

---

## Governance Principles

### 1. Contracts Are Sacred

Once an interface is published and consumed:
- **CANNOT** remove fields or endpoints
- **CANNOT** change field types or semantics
- **CANNOT** add required fields without default values
- **CAN** add optional fields
- **CAN** add new endpoints
- **CAN** deprecate with migration period

### 2. Consumer-Driven Contracts

Services cannot break their consumers:
- Consumers define expectations as tests (Pact/contract tests)
- Providers run consumer tests in CI/CD
- Breaking changes fail the build

### 3. Versioning Is Mandatory

All interfaces have explicit versions:
- APIs: semantic versioning in URL path (`/v1/`, `/v2/`)
- Events: schema version in event metadata
- Multiple versions supported concurrently during migration

---

## Reference Documentation

### [040-020 - API Versioning](api-versioning.md)
Semantic versioning strategy for REST APIs
- Version support policy (N-1 support rule)
- Breaking vs. non-breaking changes
- Deprecation timeline

### [040-030 - Event Schema Versioning](event-schema-versioning.md)
Schema versioning for domain events
- Event metadata and schema version tracking
- Evolution rules (backward-compatible vs. breaking)
- Multi-version consumer handling

### [040-040 - Contract Testing](contract-testing.md)
Consumer-driven contract testing patterns
- Pact testing framework
- CI/CD integration and enforcement
- Contract registry integration

### [040-050 - Service Contract Registry](service-contract-registry.md)
Central registry for service interfaces
- OpenAPI and AsyncAPI specifications
- Consumer contracts and Pact files
- Publishing and discovery workflows

### [040-060 - Breaking Change Detection](breaking-change-detection.md)
Automated detection and prevention
- OpenAPI diff tools
- CI/CD enforcement
- Build failure on breaking changes

### [040-070 - Deprecation Process](deprecation-process.md)
Safe deprecation of APIs and events
- Deprecation metadata and headers
- Migration period requirements
- Removal timeline

---

## Design Philosophy

This governance framework follows **contract-first design**:

1. **Interfaces are contracts** - treat them as legal agreements with consumers
2. **Consumers drive evolution** - not providers
3. **Compatibility is paramount** - breaking changes require careful planning
4. **Versioning enables coexistence** - multiple versions run simultaneously
5. **Automation prevents violations** - CI/CD catches breaking changes

---

## Related Documentation

- [API Versioning](api-versioning.md)
- [Event Schema Versioning](event-schema-versioning.md)
- [Contract Testing](contract-testing.md)
- [Service Contract Registry](service-contract-registry.md)
- [Breaking Change Detection](breaking-change-detection.md)
- [Deprecation Process](deprecation-process.md)
