---
name: component-builder
description: "ICM workspace: End-to-end new Blazor component creation from requirements discovery through implementation, theming, documentation, demos, and testing. 6 stages."
argument-hint: "[setup|status]"
allowed-tools: Read Glob Grep Edit Write
---

# Component Builder — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `workspaces/component-builder/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `workspaces/component-builder/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-discovery → 02-api-design → 03-implementation → 04-theming → 05-demos-and-docs → 06-testing
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md — only load files relevant to the current task
- **Checkpoint pattern:** Each stage has a checkpoint where human approves before proceeding
- Stages 04, 05, 06 all read from Stage 02's API design output directly (not purely sequential)

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding — asks about the component you want to build |
| `status` | Show pipeline completion for all six stages |

## Shared Resources

- `shared/component-patterns.md` — base class, CSS provider, parameter conventions
- `shared/css-naming.md` — BEM-like mar- prefix naming rules
- `shared/file-organization.md` — where each artifact goes in the repo
- `references/examples/button-walkthrough.md` — completed component example
