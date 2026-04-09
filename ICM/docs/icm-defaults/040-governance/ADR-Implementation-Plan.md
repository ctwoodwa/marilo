# ADR Implementation Plan

> **Role in ICM:** Layer 3 - governance defaults for versioning, contracts, compatibility, and interface lifecycle controls.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: ADR-Implementation-Plan | docs/icm-defaults/040-governance/ADR-Implementation-Plan.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/\<workspace\>/stages/\<stage\>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft
> Owner: Architecture Team
> Last updated: 2026-03-21

## Purpose

This document provides a step-by-step rollout plan for adopting the ADR standards defined in [ADR-0018](ADR-0018-adr-metadata-and-numbering-standardization.md) across all {{ PRODUCT_NAME }} projects. It covers migration of existing ADRs, team onboarding, and ongoing governance.

---

## Phase 1: Foundation (Week 1–2)

### Objectives

- Establish canonical ADR artifacts in `docs/icm-defaults/040-governance/`.
- Validate the template and numbering standard with the architecture team.

### Tasks

| # | Task | Owner | Status |
|---|------|-------|--------|
| 1.1 | Publish `adr-template.md` to `docs/icm-defaults/040-governance/` | Architecture Team | Complete |
| 1.2 | Publish `ADR-0018-adr-metadata-and-numbering-standardization.md` | Architecture Team | Complete |
| 1.3 | Publish this implementation plan | Architecture Team | Complete |
| 1.4 | Update `layer-3-defaults-index.md` in all workspaces to reference ADR docs | Architecture Team | Complete |
| 1.5 | Architecture team review and sign-off | Architecture Team | Pending |

### Exit Criteria

- [ ] All three ADR governance documents committed and tracked in `docs/icm-defaults/040-governance/`.
- [ ] Layer-3 index files updated in all workspaces.
- [ ] Architecture team has reviewed and approved ADR-0018.

---

## Phase 2: Inventory & Migration (Week 3–4)

### Objectives

- Audit all existing ADRs across project repositories.
- Migrate existing ADRs to the standardized format.

### Tasks

| # | Task | Owner | Status |
|---|------|-------|--------|
| 2.1 | Inventory all existing ADRs across all project repos | Tech Lead per team | Pending |
| 2.2 | Classify each ADR as ICM pattern (0001–0099) or project-specific (0100+) | Architecture Team | Pending |
| 2.3 | Assign canonical ADR numbers using the reserved range scheme | Architecture Team | Pending |
| 2.4 | Add YAML frontmatter to each existing ADR | ADR author or team lead | Pending |
| 2.5 | Rename files to `ADR-NNNN-<kebab-case>.md` convention | ADR author or team lead | Pending |
| 2.6 | Validate frontmatter with a linter or manual review | Architecture Team | Pending |

### Migration Checklist (Per ADR)

```markdown
- [ ] YAML frontmatter added with all required fields
- [ ] adr_id assigned from correct numbering range
- [ ] Filename renamed to ADR-NNNN-<kebab-case>.md
- [ ] Status field set accurately (proposed/accepted/deprecated/superseded)
- [ ] related_adrs populated where cross-references exist
- [ ] supersedes / superseded_by fields set if applicable
- [ ] Moved to correct location (icm-defaults for patterns, project repo for project-specific)
```

### Exit Criteria

- [ ] 100% of existing ADRs migrated to new format.
- [ ] No ADR numbering collisions.
- [ ] ICM pattern ADRs separated from project-specific ADRs.

---

## Phase 3: Team Onboarding (Week 5–6)

### Objectives

- Ensure all development teams understand the ADR standard and can author new ADRs independently.

### Tasks

| # | Task | Owner | Status |
|---|------|-------|--------|
| 3.1 | Distribute ADR template and ADR-0018 to all teams | Architecture Team | Pending |
| 3.2 | Conduct brief walkthrough session (30 min) per team | Architecture Team | Pending |
| 3.3 | Each team creates at least one new ADR using the template | Team leads | Pending |
| 3.4 | Architecture team reviews first-round ADRs for compliance | Architecture Team | Pending |

### Exit Criteria

- [ ] All teams have received the ADR standard.
- [ ] Each team has authored at least one compliant ADR.
- [ ] No common mistakes identified (or documented and communicated).

---

## Phase 4: Ongoing Governance (Continuous)

### Objectives

- Maintain ADR quality and currency over time.

### Practices

| Practice | Frequency | Owner |
|----------|-----------|-------|
| Review new ADRs for compliance during PR review | Every PR that adds/modifies an ADR | Reviewer |
| Audit ADR statuses (deprecate stale decisions) | Quarterly | Architecture Team |
| Update numbering ranges if new categories emerge | As needed | Architecture Team |
| Validate frontmatter consistency (automated if possible) | CI pipeline | DevOps |

### ADR Lifecycle Triggers

| Event | Required ADR Action |
|-------|-------------------|
| New technology adoption | Create project-specific ADR (0100+) |
| Enterprise standard change | Create or update ICM pattern ADR (0001–0099) |
| Standard deprecated | Set `status: deprecated`, document replacement |
| Standard replaced | Set `status: superseded`, populate `superseded_by` |
| Quarterly review | Audit all `accepted` ADRs for continued relevance |

---

## Success Metrics

| Metric | Target | Measurement |
|--------|--------|-------------|
| ADR format compliance | 100% of new ADRs | PR review checklist |
| Existing ADR migration | 100% within Phase 2 | Inventory tracking |
| Team adoption | All teams authoring ADRs independently | Phase 3 exit criteria |
| ADR currency | No `accepted` ADR older than 12 months without review | Quarterly audit |

---

## References

- [ADR Template](adr-template.md)
- [ADR-0018: ADR Metadata and Numbering Standardization](ADR-0018-adr-metadata-and-numbering-standardization.md)
- [Interface Governance](interface-governance.md)
