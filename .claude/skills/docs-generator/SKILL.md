---
name: docs-generator
description: "ICM workspace: Generate and update project documentation with consistent standards. Stages: project-scan, doc-audit, standards-alignment, doc-generation, consistency-review."
argument-hint: "[setup|status|audit|generate|deploy]"
allowed-tools: Read Glob Grep Edit Write
---

# Docs Generator — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `ICM/workspaces/docs-generator/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `ICM/workspaces/docs-generator/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-project-scan → 02-doc-audit → 03-standards-alignment → 04-doc-generation → 05-consistency-review
- **Output naming:** [slug]-*.md where slug = PROJECT_SLUG from _config
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md
- **Stage 03** produces confirmed-standards snapshot (captures exceptions agreed with human)
- **Stage 04** requires first-doc-only checkpoint before bulk generation

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `audit` | Jump to Stage 02 doc audit |
| `generate` | Jump to Stage 04 doc generation |
| `deploy` | Copy workspace into target project at `<target>/icm/docs-generator` |
