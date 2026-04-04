---
name: test-coverage-expansion
description: "ICM workspace: Audit test coverage and design layered test strategy with scaffolds. Stages: coverage-audit, test-strategy, unit-tests, component-tests, coverage-gates."
argument-hint: "[setup|status|audit|scaffold]"
allowed-tools: Read Glob Grep Edit Write
---

# Test Coverage Expansion — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `workspaces/test-coverage-expansion/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `workspaces/test-coverage-expansion/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-coverage-audit → 02-test-strategy → 03-unit-tests → 04-component-tests → 05-coverage-gates
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `audit` | Jump to Stage 01 coverage audit |
| `scaffold` | Jump to Stage 03 unit test scaffolding |

## Shared Resources

- `shared/test-patterns.md` — test patterns guide
- `shared/risk-matrix.md` — risk-based prioritization matrix
