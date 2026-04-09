---
name: splitter-delivery
description: "ICM workspace: Coordinate MariloSplitter spec accuracy, Example UX completeness, and source+tests alignment. Stages: spec-review, example-ux, sync-check."
argument-hint: "[setup|status|spec|demo|sync|deliver]"
allowed-tools: Read Glob Grep
---

# Splitter Delivery — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `ICM/workspaces/splitter-delivery/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `ICM/workspaces/splitter-delivery/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-spec-review → 02-example-ux → 03-sync-check
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md — only load files relevant to the current task
- **Status check:** Scan `stages/*/output/` — files present (excluding .gitkeep) = COMPLETE, otherwise PENDING
- **Never modify component source files directly from this workspace** — delegate to gap-analysis partner

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `spec` | Enter stages/01-spec-review/CONTEXT.md |
| `demo` | Enter stages/02-example-ux/CONTEXT.md |
| `sync` | Enter stages/03-sync-check/CONTEXT.md |
| `deliver` | Run all three stages in sequence |

## Gap Analysis Partner

`ICM/workspaces/splitter-gap-analysis/` — source changes and test writing are delegated there.
