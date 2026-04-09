---
name: enterprise-test-coverage
description: "ICM workspace: Systematically increase and improve test coverage for existing services. Stages: test-inventory, coverage-gap-analysis, test-strategy-design, test-implementation-plan, verification-and-reporting."
argument-hint: "[setup|status]"
allowed-tools: Read Glob Grep Edit Write
---

# Enterprise Test Coverage — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `ICM/workspaces/enterprise-test-coverage/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `ICM/workspaces/enterprise-test-coverage/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-test-inventory → 02-coverage-gap-analysis → 03-test-strategy-design → 04-test-implementation-plan → 05-verification-and-reporting
- **Output naming:** test-[slug]-*.md
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
