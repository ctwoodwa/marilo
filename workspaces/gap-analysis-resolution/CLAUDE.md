# Gap Analysis Resolution

Resolves documented gaps from gap analysis through a structured lifecycle: intake, prioritization, resolution design, remediation planning, implementation, and validation.

## Folder Map

```
gap-analysis-resolution/
├── CLAUDE.md              (you are here)
├── CONTEXT.md             (start here for task routing)
├── setup/                 (onboarding questionnaire)
├── _config/
│   └── gap-context.md     (scope, target project, resolution tracking)
├── stages/
│   ├── 01-intake/         (import gap analysis, define scope and target state)
│   ├── 02-prioritize/     (score impact, sequence dependencies, create backlog)
│   ├── 03-resolution-design/ (design target patterns, capture decisions)
│   ├── 04-remediation-plan/  (tasks, phases, success criteria)
│   ├── 05-implement/      (execute changes, migrate artifacts)
│   └── 06-validate/       (verify closure, enforce guardrails)
├── shared/                (cross-stage templates and frameworks)
└── references/            (read-only pointers to canonical defaults)
```

## Entry Paths

Gap analyses vary in maturity. The workspace supports two entry paths:

| Path | When | Start At | Stages |
|------|------|----------|--------|
| **Existing analysis** | Gap analysis files already exist (e.g., GAP_ANALYSIS.md, GAP_ANALYSIS_INDEX.md) | Stage 01 (import mode) | 01 through 06 |
| **Fresh analysis** | No gap analysis yet; start from target state definition | Stage 01 (assess mode) | 01 through 06 |

## Gap Scope Routing

After intake, the scope of the gap set determines which stages run:

| Scope | Description | Stages |
|-------|-------------|--------|
| `single` | One isolated gap, one component/module | 01 > 03 > 05 > 06 (skip prioritize and plan) |
| `batch` | Related gaps in one area (e.g., all Button gaps) | 01 > 02 > 03 > 05 > 06 (skip formal plan) |
| `systematic` | Cross-cutting gaps across multiple areas | 01 > 02 > 03 > 04 > 05 > 06 (full pipeline) |

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `ingest` | Fast path: paste/point to gap analysis file, jump to Stage 01 import mode |
| `resolve` | Start or continue resolution design (Stage 03) |
| `close` | Jump to validation (Stage 06) for a specific gap |

## Routing

| Task | Go To |
|------|-------|
| Import or create a gap analysis | `stages/01-intake/CONTEXT.md` |
| Prioritize and sequence gaps | `stages/02-prioritize/CONTEXT.md` |
| Design a resolution for a gap | `stages/03-resolution-design/CONTEXT.md` |
| Plan remediation tasks and phases | `stages/04-remediation-plan/CONTEXT.md` |
| Implement changes | `stages/05-implement/CONTEXT.md` |
| Validate closure and enforce | `stages/06-validate/CONTEXT.md` |

## What to Load

| Task | Load These | Do NOT Load |
|------|-----------|-------------|
| Intake | CONTEXT.md, _config/gap-context.md, shared/gap-record-format.md | Stage 03-06 files |
| Prioritize | Stage 01 output, shared/priority-framework.md, _config/gap-context.md | Stage 03-06 files |
| Resolution design | Stage 02 output, shared/resolution-record-format.md, relevant icm-defaults | Stage 05-06 files, unrelated gap records |
| Remediation plan | Stage 03 output, _config/gap-context.md | Stage 01-02 files |
| Implement | Stage 04 output (or Stage 03 for batch/single), target project code | Stage 01-02 files, unrelated gap records |
| Validate | Stage 05 output, shared/validation-checklist.md, original gap analysis | Stage 01-04 files |

## Stage Handoffs

Each stage writes its output to its own `output/` folder. The next stage reads from there. If you edit an output file, the next stage picks up your edits.

## Global Constraints

- Every resolution must trace back to a documented gap. No opportunistic changes.
- Read affected code before designing a resolution. Patterns found in the codebase take precedence over assumptions.
- Gap records are append-only during resolution. Never delete or modify the original gap description. Add resolution status alongside it.
- One gap record = one decision. If a gap has multiple valid resolutions, split into separate records.
