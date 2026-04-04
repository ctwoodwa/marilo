---
name: ci-cd-pipeline
description: "ICM workspace: Design and scaffold complete CI/CD pipeline for multi-project .NET solutions. Stages: build-audit, pipeline-design, pipeline-scaffold, environment-strategy, release-checklist."
argument-hint: "[setup|status|scaffold]"
allowed-tools: Read Glob Grep Edit Write
---

# CI/CD Pipeline — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `workspaces/ci-cd-pipeline/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `workspaces/ci-cd-pipeline/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-build-audit → 02-pipeline-design → 03-pipeline-scaffold → 04-environment-strategy → 05-release-checklist
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `scaffold` | Jump to Stage 03 pipeline scaffold |

## Shared Resources

- `shared/pipeline-patterns.md` — CI/CD patterns
- `shared/github-actions-reference.md` — GitHub Actions reference
- `shared/azure-pipelines-reference.md` — Azure Pipelines reference
