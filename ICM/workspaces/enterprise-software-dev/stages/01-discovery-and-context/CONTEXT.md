# Stage 01 - Discovery and Context

Capture the problem domain, constraints, stakeholders, and environment landscape.
This is the foundation every later stage builds on - accuracy here matters most.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Setup answers | ../../_config/org-context.md | Full file | Org, cloud, compliance baseline |
| Setup answers | ../../_config/tech-stack-defaults.md | Full file | Tech constraints and preferences |

## Process

1. Ask conversationally for the project slug. Confirm it is lowercase-with-hyphens.
2. Ask: solution name, business domain (one sentence), primary stakeholders, and SLAs.
3. Identify existing systems this solution integrates with, replaces, or depends on.
4. List environments (dev, test, prod) and any cloud, hosting, or on-prem constraints.
5. Identify compliance or security requirements (GDPR, SOC2, HIPAA, FedRAMP, or none).
6. Draft the problem brief and context map.
7. CHECKPOINT - present both drafts to the human before saving.
8. Apply corrections from human review.
9. Run audit.
10. Save outputs with project slug prefix.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 6 | Draft problem brief and context map side by side | Approve or correct domain description, stakeholder list, integration points, and constraints |

## Audit

| Check | Pass Condition |
|---|---|
| Problem brief is one page or less | Brief is concise and does not contain implementation details |
| Domain is described in business terms | No technical jargon in the domain description |
| All environments are listed | At least dev and prod are named |
| Integration points are named | Every external system is listed, even if detail is TBD |
| Compliance requirements are explicit | Either a named standard or explicitly stated as none |
| Project slug is confirmed | Slug is lowercase-with-hyphens and recorded for this session |

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Problem brief | output/[slug]-problem-brief.md | Markdown |
| Context map | output/[slug]-context-map.md | Markdown |
| Constraint list | output/[slug]-constraints.md | Markdown |