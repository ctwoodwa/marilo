---
name: enterprise-api-change
description: "ICM workspace: Safely change APIs and events under interface governance and release channel controls. Stages: change-request-analysis, contract-design, implementation-impact, rollout-plan, review-and-validation."
argument-hint: "[setup|status]"
allowed-tools: Read Glob Grep Edit Write
---

# Enterprise API Change — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `ICM/workspaces/enterprise-api-change/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `ICM/workspaces/enterprise-api-change/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-change-request-analysis → 02-contract-design → 03-implementation-impact → 04-rollout-plan → 05-review-and-validation
- **Output naming:** api-[slug]-*.md
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
