---
name: jquery-to-blazor
description: "ICM workspace: Migrate jQuery-based pages to .NET Core Blazor within existing Blazor project. Stages: feature-inventory, codebase-alignment, component-mapping, page-conversion, integration-validation."
argument-hint: "[setup|status|inventory|convert]"
allowed-tools: Read Glob Grep Edit Write
---

# jQuery to Blazor Migration — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `workspaces/jquery-to-blazor/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `workspaces/jquery-to-blazor/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-feature-inventory → 02-codebase-alignment → 03-component-mapping → 04-page-conversion → 05-integration-validation
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md — only load files relevant to the current task

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `inventory` | Jump to Stage 01 feature inventory |
| `convert` | Jump to Stage 04 page conversion |

## Shared Resources

- `shared/jquery-to-blazor-patterns.md` — conversion patterns
- `shared/jquery-plugin-map.md` — jQuery plugin mapping
- `shared/telerik-component-map.md` — component mapping
- `shared/js-interop-bridge.md` — JS interop bridge guide
