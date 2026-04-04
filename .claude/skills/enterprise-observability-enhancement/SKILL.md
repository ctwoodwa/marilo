---
name: enterprise-observability-enhancement
description: "ICM workspace: Bring existing service up to enterprise observability, error handling, and JSON standards. Stages: telemetry-inventory, standards-gap-analysis, observability-design, instrumentation-plan, verification-and-tuning."
argument-hint: "[setup|status]"
allowed-tools: Read Glob Grep Edit Write
---

# Enterprise Observability Enhancement — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `workspaces/enterprise-observability-enhancement/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `workspaces/enterprise-observability-enhancement/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-telemetry-inventory → 02-standards-gap-analysis → 03-observability-design → 04-instrumentation-plan → 05-verification-and-tuning
- **Output naming:** obs-[slug]-*.md
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
