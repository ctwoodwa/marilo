---
name: enterprise-software-dev
description: "ICM workspace: Design and scaffold enterprise-grade .NET solutions with Aspire, Blazor, YARP, DAB, Scalar, GraphQL, OpenTelemetry. Stages: discovery, architecture, standards, specs, scaffolds."
argument-hint: "[setup|status]"
allowed-tools: Read Glob Grep Edit Write
---

# Enterprise Software Dev — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `ICM/workspaces/enterprise-software-dev/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `ICM/workspaces/enterprise-software-dev/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-discovery-and-context → 02-architecture-and-patterns → 03-standards-and-practices → 04-service-and-app-specs → 05-scaffolds-and-checklists
- **No shortcuts allowed** — full pipeline always runs
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md
- **Stage 05** produces copilot-instructions.md that primes GitHub Copilot with full project context

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire and populate `_config/` |
| `status` | Show pipeline completion for all stages |

## Global Constraints

- All services emit OpenTelemetry
- API UIs are environment-gated (dev/test only)
- 12-factor principles enforced
- KISS over complexity
