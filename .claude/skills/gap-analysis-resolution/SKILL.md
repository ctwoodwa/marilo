---
name: gap-analysis-resolution
description: "ICM workspace: Resolve documented gaps through intake, prioritization, resolution design, remediation, implementation, and validation. Supports single/batch/systematic scopes."
argument-hint: "[setup|status|ingest|resolve|close]"
allowed-tools: Read Glob Grep Edit Write
---

# Gap Analysis Resolution — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `ICM/workspaces/gap-analysis-resolution/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `ICM/workspaces/gap-analysis-resolution/CONTEXT.md` for task routing

## Cold Start Protocol

1. Load `_status/workspace-status.md` first (Layer 0 snapshot — not authoritative)
2. Then load `_config/coverage-summary.md` for batch-level detail
3. Then load `_config/gap-context.md` for full configuration and resolution tracking

## ICM Rules

- **Stage progression:** 01-intake → 02-prioritize → 03-resolution-design → 04-remediation-plan → 05-implement → 06-validate
- **Scope routing:** `single` skips 02+04 | `batch` skips 04 | `systematic` runs all 6
- **Entry paths:** Existing analysis (import mode) or Fresh analysis (assess mode)
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md — only load files relevant to the current task
- **Gap records are append-only** — never delete or modify original gap descriptions, add resolution status alongside

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `ingest` | Fast path: paste/point to gap analysis file, jump to Stage 01 import mode |
| `resolve` | Start or continue resolution design (Stage 03) |
| `close` | Jump to validation (Stage 06) for a specific gap |

## Global Constraints

- Every resolution must trace back to a documented gap. No opportunistic changes.
- Read affected code before designing a resolution.
- One gap record = one decision. Split if multiple valid resolutions exist.
- When a component outgrows this workspace, graduate to a dedicated Component Delivery Workspace.
