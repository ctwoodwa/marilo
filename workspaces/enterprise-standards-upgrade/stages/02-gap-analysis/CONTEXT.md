# Stage 02 - Gap Analysis

Compare the existing project against the enterprise software standards and identify concrete gaps and opportunities.

## Inputs

| Source                 | File/Location                                             | Section/Scope                | Why                                                                    |
| ---------------------- | --------------------------------------------------------- | ---------------------------- | ---------------------------------------------------------------------- |
| Previous stage         | ../01-audit-current/output/[slug]-current-architecture.md | Full file                    | Baseline of current practices                                          |
| Enterprise context     | ../../references/layer-3-defaults-index.md                | Context section              | Defines enterprise standard and constraints                            |
| Domain standards       | ../../references/layer-3-defaults-index.md                | Domain section               | Target domain modeling and rule governance                             |
| Architecture standards | ../../references/layer-3-defaults-index.md                | Architecture section         | Target logical/data/integration/security/reactive/observability models |
| Interface governance   | ../../references/layer-3-defaults-index.md                | Interface Governance section | Target API/event evolution controls                                    |
| Operating model        | ../../references/layer-3-defaults-index.md                | Operating Model section      | Team ownership and migration expectations                              |
| Release channels       | ../../references/layer-3-defaults-index.md                | Release Channels section     | Rollout constraints and progression model                              |

## Process

1. Review the current-state summary and extract current practices for auth, data access, integration, error handling, logging, deployment, and testing.
2. Compare current practices to enterprise context and principles; record wins and divergences.
3. Assess domain and business logic alignment against domain primitives and governance defaults.
4. Assess architecture alignment across logical, data, integration, security, signal/reactive, and observability defaults.
5. Assess interface governance alignment across versioning, contract testing, service registry, breaking change detection, and deprecation.
6. Assess operating model and migration feasibility constraints.
7. Classify each area as `Aligned`, `Partial`, `Not`, or `NA`, then prioritize top gaps and opportunities.
8. Save outputs.

## Outputs

| Artifact                       | Location                                | Format   |
| ------------------------------ | --------------------------------------- | -------- |
| Standards gap analysis summary | output/[slug]-standards-gap-analysis.md | Markdown |

## Checkpoints

| After Step | Agent Presents                           | Human Decides                                   |
| ---------- | ---------------------------------------- | ----------------------------------------------- |
| 2          | Initial alignment and divergence summary | Confirm interpretation of enterprise principles |
| 5          | Governance gap matrix draft              | Confirm severity and applicability              |
| 7          | Prioritized top gaps list                | Approve priorities for Stage 03 design          |

## Audit

| Check              | Pass Condition                                                                           |
| ------------------ | ---------------------------------------------------------------------------------------- |
| Required structure | Output contains overview, category tables, and prioritized top 5-10 gaps                 |
| Table fields       | Each table row includes Area, Current State, Enterprise Standard, Status, Notes/Examples |
| Status fidelity    | Each area uses only `Aligned`, `Partial`, `Not`, or `NA`                                 |
| Traceability       | Priority gaps include concrete notes or references to source evidence                    |
| Output naming      | Artifact matches `output/[slug]-standards-gap-analysis.md`                               |
