---
name: diagram-gap-analysis
description: "ICM workspace: Gap analysis for MariloDiagram. Intake, prioritize, design, plan, implement, validate. Stages: 01-intake through 06-validate."
argument-hint: "[setup|status|ingest|resolve|close]"
allowed-tools: Read Glob Grep Edit Write
---

# MariloDiagram Gap Analysis — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `workspaces/diagram-gap-analysis/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read the CLAUDE.md routing table for task routing

## ICM Rules

- **Stage progression:** 01-intake → 02-prioritize → 03-resolution-design → 04-remediation-plan → 05-implement → 06-validate
- **Scope routing:** `single` skips 02+04 | `batch` skips 04 | `systematic` runs all 6
- **Entry paths:** Existing analysis (import mode) or Fresh analysis (assess mode)
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **Gap records are append-only** — never delete or modify original gap descriptions

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `ingest` | Fast path: paste/point to gap analysis file, jump to Stage 01 import mode |
| `resolve` | Start or continue resolution design (Stage 03) |
| `close` | Jump to validation (Stage 06) for a specific gap |

## Delivery Partner

`workspaces/diagram-delivery/` — spec accuracy and Example UX coordination happen there.
