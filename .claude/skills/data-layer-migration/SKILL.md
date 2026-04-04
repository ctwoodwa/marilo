---
name: data-layer-migration
description: "ICM workspace: Replace in-memory data store with SQL Server using hybrid DAB + EF Core architecture. Stages: endpoint-classification, schema-design, dab-setup, ef-core-setup, repository-wiring."
argument-hint: "[setup|status|classify|schema]"
allowed-tools: Read Glob Grep Edit Write
---

# Data Layer Migration — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `workspaces/data-layer-migration/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `workspaces/data-layer-migration/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-endpoint-classification → 02-schema-design → 03-dab-setup → 04-ef-core-setup → 05-repository-wiring
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md — only load files relevant to the current task

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `classify` | Jump to Stage 01 endpoint classification |
| `schema` | Jump to Stage 02 schema design |

## Shared Resources

- `shared/dab-reference.md` — Data API Builder reference
- `shared/dab-config-template.json` — DAB configuration template
- `shared/ef-core-patterns.md` — EF Core patterns guide
