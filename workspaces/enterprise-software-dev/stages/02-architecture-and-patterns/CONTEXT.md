# Stage 02 - Architecture and Patterns

Choose architecture patterns, service topology, and produce Architecture Decision Records (ADRs).

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Stage 01 output | ../01-discovery-and-context/output/[slug]-problem-brief.md | Full file | Problem and constraints to shape arch |
| Stage 01 output | ../01-discovery-and-context/output/[slug]-constraints.md | Full file | Hard constraints on patterns |
| Config | ../../_config/architecture-principles.md | Full file | Preferred patterns and rules |
| Shared | ../../shared/enterprise-patterns.md | Full file | Available patterns catalogue |

## Process

1. Review problem brief and constraints from Stage 01.
2. Define solution topology: Aspire AppHost, service projects, shared libs.
3. Select architecture style (layered, ports-and-adapters, CQRS) per service type.
4. Draft one ADR per significant decision (context, decision, consequences).
5. Produce a system architecture diagram in text showing service boundaries and data flows.
6. CHECKPOINT - present topology diagram and ADR list to human before finalising.
7. Apply corrections from human review.
8. Run audit.
9. Save outputs with project slug prefix.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 5 | Text architecture diagram and list of proposed ADRs with one-line summaries | Approve topology, challenge any decisions, add missing decisions before ADRs are written in full |

## Audit

| Check | Pass Condition |
|---|---|
| No circular dependencies | No service in the boundary map depends on a service that depends back on it |
| Every integration point from Stage 01 is addressed | Each external system in the context map appears in the architecture |
| Every significant decision has an ADR | No undocumented choices that affect service boundaries, data ownership, or auth |
| Architecture style is consistent | All services use the agreed style unless an ADR explicitly explains the exception |
| Aspire AppHost lists all services | Solution topology is complete, not partial |

## Outputs

| Artifact | Location | Format |
|---|---|---|
| System architecture | output/[slug]-system-architecture.md | Markdown |
| ADRs (one file per decision) | output/[slug]-architecture-decision-records/ | Markdown |
| Service boundary map | output/[slug]-service-boundary-map.md | Markdown |