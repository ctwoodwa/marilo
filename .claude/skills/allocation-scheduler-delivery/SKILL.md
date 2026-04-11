---
name: allocation-scheduler-delivery
description: "ICM workspace: Coordinate MariloAllocationScheduler spec accuracy, Example UX completeness, visual parity, and source+tests alignment. Stages: spec-review, example-ux, visual-parity, sync-check."
argument-hint: "[setup|status|spec|demo|visual|sync|deliver]"
allowed-tools: Read Glob Grep
---

# AllocationScheduler Delivery — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `ICM/workspaces/allocation-scheduler-delivery/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `ICM/workspaces/allocation-scheduler-delivery/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-spec-review → 02-example-ux → 03-visual-parity → 04-sync-check
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
| `visual` | Enter stages/03-visual-parity/CONTEXT.md |
| `sync` | Enter stages/04-sync-check/CONTEXT.md |
| `deliver` | Run all four stages in sequence |

## Component Context

- **Source:** `src/Marilo.Components/DataDisplay/AllocationScheduler/`
- **Entry file:** `MariloAllocationScheduler.razor.cs`
- **Specs:** `docs/component-specs/allocation-scheduler/`
- **Tests:** `tests/Marilo.Tests.Unit/DataDisplay/AllocationScheduler/`
- **Focus topics:** selection, keyboard-navigation, refresh-data, editing (see the four topic-specific spec docs for parity targets)

## Gap Analysis Partner

`ICM/workspaces/allocation-scheduler-gap-analysis/` — source changes and test writing are delegated there.
