---
name: pm-demo-builder
description: "ICM workspace: Plan, sequence, and execute implementation passes for the Marilo PM Demo. Covers settings pages, asset management, dynamic forms, inspections, deficiencies, and domain expansion. 6 stages."
argument-hint: "[setup|status]"
allowed-tools: Read Glob Grep Edit Write
---

# PM Demo Builder — ICM Workspace Skill

You are entering an **Interpretive Context Methodology (ICM)** workspace. The folder structure IS the orchestration — numbered folders are stages, markdown files carry prompts and context.

## Entry Point

1. Read `ICM/workspaces/pm-demo-builder/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `ICM/workspaces/pm-demo-builder/CONTEXT.md` for task routing

## ICM Rules

- **Stage progression:** 01-current-state → 02-ia-and-shell → 03-domain-modeling → 04-page-build → 05-integration → 06-review
- **Output handoffs:** Each stage writes to its `output/` folder. The next stage reads from there.
- **What-to-Load:** Follow the "What to Load" matrix in CLAUDE.md — only load files relevant to the current task
- **Reality over aspiration:** Stage 01 captures what actually exists in code, not what is planned
- **Canonical source:** Settings progress is tracked in `samples/Marilo.PmDemo/SETTINGS_STATUS.md` — always read this, not copies

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding — asks about which build pass to execute (settings, assets, dynamic forms, etc.) |
| `status` | Show pipeline completion for all six stages |

## Shared Resources

- `shared/implementation-guardrails.md` — agreed architecture and coding rules
- `shared/component-inventory.md` — existing Marilo components available to the demo
- `shared/shell-and-ia.md` — app shell, navigation, layout conventions
- `_config/pm-demo-context.md` — current state assessment from samples/Marilo.PmDemo
- `_config/domain-expansion.md` — asset management, dynamic forms, inspections scope

## Key Context

- **Sample app:** `samples/Marilo.PmDemo` (Blazor Server via Aspire, EF Core + PostgreSQL, DAB + GraphQL, Wolverine messaging)
- **Notification pipeline:** Canonical `IUserNotificationService` already implemented and wired
- **Settings area:** 1 stub page exists, 9 more planned, settings layout not yet created
- **Domain expansion:** Asset management, dynamic forms (cross-cutting), inspections, deficiencies, conditions, remodeling projects
