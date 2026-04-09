---
name: enterprise-quality-control
description: "ICM workspace: Reference toolkit for quality gates, checklists, metrics, playbooks, and test examples. Non-sequential reference workspace — browse by topic, not pipeline stages."
argument-hint: "[setup|status]"
allowed-tools: Read Glob Grep
---

# Enterprise Quality Control — ICM Workspace Skill

You are entering an **Interpretable Context Methodology (ICM)** workspace. Unlike pipeline workspaces, this is a **reference toolkit** — browse by topic, not sequential stages.

## Entry Point

1. Read `ICM/workspaces/enterprise-quality-control/CLAUDE.md` — this is the workspace routing layer
2. If `$ARGUMENTS` is provided, treat it as a **trigger keyword** and follow the Triggers table in CLAUDE.md
3. If no argument, read `ICM/workspaces/enterprise-quality-control/CONTEXT.md` for topic routing

## ICM Rules

- **Non-sequential** — this is a reference workspace, not a pipeline
- **5 topic folders:** quality-gates, checklists, metrics-and-reporting, playbooks, examples
- **Read-only reference** — this workspace defines standards; other workspaces implement them

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show inventory of available quality control resources |

## Topic Areas

| Folder | Purpose |
|--------|---------|
| `quality-gates/` | Gate definitions and pass/fail criteria |
| `checklists/` | Reusable checklists for common quality tasks |
| `metrics-and-reporting/` | Metric definitions and reporting templates |
| `playbooks/` | Step-by-step guides for quality processes |
| `examples/` | Reference implementations and test examples |
