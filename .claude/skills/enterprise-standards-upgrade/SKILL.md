---
name: enterprise-standards-upgrade
description: "ICM workspace: Assess existing repository against enterprise standards and produce upgrade path. Stages: audit-current, gap-analysis, evolution-design, migration-plan, rollout-checklist."
argument-hint: "[setup|status]"
allowed-tools: Read Glob Grep Edit Write
---

# Enterprise Standards Upgrade — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `workspaces/enterprise-standards-upgrade/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `workspaces/enterprise-standards-upgrade/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-audit-current → 02-gap-analysis → 03-evolution-design → 04-migration-plan → 05-rollout-checklist
- **Output naming:** [slug]-*.md
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
