---
name: enterprise-feature-change
description: "ICM workspace: Take a Jira ticket from intake to merged PR with full context preservation. Adaptive routing by ticket size (xs/s/m/l). Stages: discovery, architecture-impact, design-spec, implementation-plan, review-and-pr."
argument-hint: "[setup|status|ticket|implement|pr]"
allowed-tools: Read Glob Grep Edit Write
---

# Enterprise Feature Change — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `ICM/workspaces/enterprise-feature-change/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `ICM/workspaces/enterprise-feature-change/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-discovery → 02-architecture-impact → 03-design-spec → 04-implementation-plan → 05-review-and-pr
- **Ticket size routing:** `xs` = 01→03→05 | `s` = 01→02→03→05 | `m` = all stages | `l` = all stages + human approval after 02
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Read questionnaire, populate `_config/ticket-context.md` |
| `status` | Scan `stages/*/output/` and show what's done, what's next |
| `ticket` | Start Stage 01 — paste the ticket text or provide ticket ID |
| `implement` | Jump to Stage 03; requires ticket-context.md and Stage 02 output |
| `pr` | Jump to Stage 05; requires Stage 03 output |

## Global Constraints

- Read affected code before writing any changes
- Follow conventions in the files you read
- Every code change must map to at least one acceptance criterion
- Do not change files outside the scope identified in Stage 01
