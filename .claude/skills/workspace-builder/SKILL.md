---
name: workspace-builder
description: "ICM workspace: Build new ICM workspaces for any domain. Stages: discovery, mapping, scaffolding, questionnaire-design, validation."
argument-hint: "[setup|status]"
allowed-tools: Read Glob Grep Edit Write
---

# Workspace Builder — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `workspaces/workspace-builder/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `workspaces/workspace-builder/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-discovery → 02-mapping → 03-scaffolding → 04-questionnaire-design → 05-validation
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md
- **Meta-workspace:** This workspace creates other workspaces — follow ICM conventions strictly

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |

## References

- `references/conventions-reference.md` — ICM conventions for workspace design
- `_core/templates/` — master templates for workspace types
- `workspaces/shared/workspace-routing.md` — taxonomy and graduation criteria
