---
adr_id: "ADR-0018"
title: "ADR Metadata and Numbering Standardization"
status: "accepted"
date_proposed: "2026-01-15"
date_accepted: "2026-02-01"
decision_makers:
  - "Architecture Team"
tags:
  - "governance"
  - "adr"
  - "standards"
supersedes: ""
superseded_by: ""
related_adrs:
  - "ADR-0017"
  - "ADR-0019"
---

# ADR-0018: ADR Metadata and Numbering Standardization

> **Role in ICM:** Layer 3 - governance defaults for versioning, contracts, compatibility, and interface lifecycle controls.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: ADR-0018 | docs/icm-defaults/040-governance/ADR-0018-adr-metadata-and-numbering-standardization.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/\<workspace\>/stages/\<stage\>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Accepted
> Owner: Architecture Team
> Last updated: 2026-03-21

## Status

Accepted

## Context

As {{ PRODUCT_NAME }} scales across multiple services, teams, and repositories, Architecture Decision Records have become critical artifacts for maintaining architectural consistency and institutional memory. However, without a standardized format:

1. **Discoverability suffers** — ADRs use inconsistent naming, making them hard to find and cross-reference.
2. **Metadata is incomplete** — Some ADRs lack dates, decision-makers, or status, making lifecycle tracking impossible.
3. **Numbering collisions** — Without a reserved numbering scheme, teams risk ID conflicts when creating ADRs independently.
4. **ICM vs project ADRs blur** — No clear separation between enterprise-wide standards and project-specific technology selections.

The ICM methodology requires that governance documents (Layer 3) provide clear, unambiguous defaults that workspaces consume without modification.

## Decision

We will standardize all ADRs across {{ PRODUCT_NAME }} using:

### 1. YAML Frontmatter (Required)

Every ADR must include machine-readable frontmatter:

```yaml
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
```

### 2. Four-Digit Numbering with Reserved Ranges

| Range | Category | Governance level |
|-------|----------|-----------------|
| 0001–0009 | Foundation & methodology | ICM pattern |
| 0010–0019 | Cross-cutting standards | ICM pattern |
| 0020–0039 | Architecture patterns | ICM pattern |
| 0040–0059 | Security & identity | ICM pattern |
| 0060–0079 | Integration & messaging | ICM pattern |
| 0080–0099 | Observability & operations | ICM pattern |
| 0100–0199 | Project-specific decisions | Project |

### 3. Filename Convention

```
ADR-NNNN-<kebab-case-short-title>.md
```

- Four-digit zero-padded number
- Kebab-case descriptive suffix
- `.md` extension

### 4. Status Lifecycle

```
proposed → accepted → [deprecated | superseded]
```

- **proposed**: Under review, not yet binding.
- **accepted**: Binding decision; all implementations must comply.
- **deprecated**: No longer recommended; existing implementations may remain but new work must not use.
- **superseded**: Replaced by a newer ADR (link via `superseded_by` field).

### 5. ICM Pattern vs Project-Specific Separation

| Type | Numbering | Location | Authority |
|------|-----------|----------|-----------|
| ICM pattern ADR | 0001–0099 | `docs/icm-defaults/040-governance/` | Enterprise-wide canonical default |
| Project-specific ADR | 0100+ | Project repo or workspace stage output | Project-scoped decision |

ICM pattern ADRs define **what** the enterprise standard is (e.g., "use System.Text.Json with camelCase"). Project-specific ADRs define **how** a particular project implements or deviates (e.g., "this project uses Dapper for data access because...").

## Consequences

### Positive

- Uniform metadata enables automated ADR indexing and status dashboards.
- Reserved numbering prevents collisions across teams.
- Clear ICM/project separation prevents governance confusion.
- YAML frontmatter supports tooling (linters, search, dependency graphs).
- Lifecycle tracking via status field ensures ADRs stay current.

### Negative

- Existing ADRs require migration to the new format (see [ADR Implementation Plan](ADR-Implementation-Plan.md)).
- Teams must learn the numbering convention and frontmatter schema.
- Stricter format may slow initial ADR authoring slightly.

### Neutral

- The four-digit scheme supports up to 9,999 ADRs per category, which exceeds any foreseeable need.
- Frontmatter parsing requires Markdown tooling that understands YAML blocks.

## Alternatives Considered

### Alternative 1: Free-form ADR Numbering

- **Description:** Allow teams to pick any number without reserved ranges.
- **Pros:** Maximum flexibility, no coordination needed.
- **Cons:** Collisions across teams, no semantic meaning in numbers, harder to categorize.

### Alternative 2: Date-based ADR IDs (YYYY-MM-DD)

- **Description:** Use date stamps instead of sequential numbers.
- **Pros:** Natural chronological ordering, no collision risk.
- **Cons:** Loses categorical grouping, harder to reference in conversation ("ADR from January 15th" vs "ADR-0018").

### Alternative 3: UUID-based ADR IDs

- **Description:** Use UUIDs for globally unique identification.
- **Pros:** Zero collision risk, works across distributed teams.
- **Cons:** Not human-readable, impossible to reference in conversation, no semantic structure.

## Compliance

- [x] Reviewed by architecture team
- [x] Consistent with enterprise standards (icm-defaults)
- [x] No unresolved conflicts with existing ADRs

## References

- [ADR Template](adr-template.md) — canonical template following this standard
- [ADR Implementation Plan](ADR-Implementation-Plan.md) — rollout plan for migrating existing ADRs
- [Interface Governance](interface-governance.md) — parent governance framework
- Michael Nygard, "Documenting Architecture Decisions" (2011)
