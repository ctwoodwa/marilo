---
name: dotnet-framework-to-blazor
description: "ICM workspace: Migrate .NET Framework (4.5-4.8) projects to .NET Core Blazor. Stages: inventory-assessment, architecture-design, data-layer-migration, ui-conversion, integration-validation."
argument-hint: "[setup|status|inventory|convert]"
allowed-tools: Read Glob Grep Edit Write
---

# .NET Framework to Blazor Migration — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `ICM/workspaces/dotnet-framework-to-blazor/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `ICM/workspaces/dotnet-framework-to-blazor/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-inventory-assessment → 02-architecture-design → 03-data-layer-migration → 04-ui-conversion → 05-integration-validation
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md — only load files relevant to the current task

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `inventory` | Jump to Stage 01 inventory assessment |
| `convert` | Jump to Stage 04 UI conversion |

## Shared Resources

- `shared/dotnet-fx-to-blazor-patterns.md` — conversion patterns
- `shared/telerik-component-map.md` — component mapping
- `shared/dab-integration-guide.md` — DAB integration
- `shared/webforms-controls-map.md` — WebForms control mapping
