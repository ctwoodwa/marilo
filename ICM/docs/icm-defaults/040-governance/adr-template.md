# ADR Template

> **Role in ICM:** Layer 3 - governance defaults for versioning, contracts, compatibility, and interface lifecycle controls.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: adr-template | docs/icm-defaults/040-governance/adr-template.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/\<workspace\>/stages/\<stage\>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft
> Owner: Architecture Team
> Last updated: 2026-03-21

## Purpose

This template defines the canonical format for all Architecture Decision Records (ADRs) across {{ PRODUCT_NAME }} projects. Every ADR must follow this structure to ensure consistency, discoverability, and traceability.

See [ADR-0018](ADR-0018-adr-metadata-and-numbering-standardization.md) for the full numbering and metadata standard.

---

## Template

```markdown
---
adr_id: "ADR-NNNN"
title: "<Short descriptive title>"
status: "proposed | accepted | deprecated | superseded"
date_proposed: "YYYY-MM-DD"
date_accepted: "YYYY-MM-DD"
decision_makers:
  - "<name or role>"
tags:
  - "<category>"
supersedes: ""
superseded_by: ""
related_adrs:
  - "<ADR-NNNN>"
---

# ADR-NNNN: <Short Descriptive Title>

## Status

<proposed | accepted | deprecated | superseded>

## Context

Describe the forces at play, including technical, business, and organizational.
What problem are we trying to solve? What constraints exist?

## Decision

State the architecture decision clearly and unambiguously.
Use active voice: "We will use X for Y."

## Consequences

### Positive

- List beneficial outcomes of this decision.

### Negative

- List trade-offs, risks, or costs.

### Neutral

- List side effects that are neither clearly positive nor negative.

## Alternatives Considered

### Alternative 1: <Name>

- **Description:** Brief explanation.
- **Pros:** What it does well.
- **Cons:** Why it was not chosen.

### Alternative 2: <Name>

- **Description:** Brief explanation.
- **Pros:** What it does well.
- **Cons:** Why it was not chosen.

## Compliance

- [ ] Reviewed by architecture team
- [ ] Consistent with enterprise standards (icm-defaults)
- [ ] No unresolved conflicts with existing ADRs

## References

- Link to relevant icm-defaults documents, external standards, or prior art.
```

---

## Numbering Convention

| Range | Category | Examples |
|-------|----------|---------|
| 0001–0009 | Foundation & methodology | ADR-0001 through ADR-0009 |
| 0010–0019 | Cross-cutting standards | JSON serialization, error handling, configuration |
| 0020–0039 | Architecture patterns | Service boundaries, data access, state management |
| 0040–0059 | Security & identity | Authentication, authorization, secrets |
| 0060–0079 | Integration & messaging | Event schemas, API contracts, async patterns |
| 0080–0099 | Observability & operations | Logging, tracing, health checks |
| 0100–0199 | Project-specific decisions | Technology selections, library choices |

---

## Filename Convention

```
ADR-NNNN-<kebab-case-short-title>.md
```

Examples:
- `ADR-0017-json-serialization-standards.md`
- `ADR-0019-error-handling-and-problem-details-standardization.md`
- `ADR-0101-data-access-technology-selection.md`

---

## ICM vs Project ADRs

| Type | Scope | Location | Numbering |
|------|-------|----------|-----------|
| **ICM pattern ADR** | Generic enterprise standard (e.g., JSON, error handling, configuration) | `docs/icm-defaults/040-governance/` or linked from there | 0001–0099 |
| **Project-specific ADR** | Implementation choice for one project (e.g., "use Dapper", "use MediatR") | Project repo `docs/adrs/` or workspace stage output | 0100+ |

ICM pattern ADRs are canonical Layer-3 defaults. Project-specific ADRs reference ICM defaults and document deviations or selections within the allowed design space.
