# Gap Analysis -- MariloResizableContainer

**Component:** MariloResizableContainer
**Category:** Layout
**Complexity:** Medium-High (CDW warranted)
**Active phase:** Phase 1 (initial build)
**Status:** Not started -- all stages pending

## Folder Map

```
resizable-container-gap-analysis/
├── CLAUDE.md                (you are here)
├── CONTEXT.md               (task routing and shared resources)
├── _config/
│   ├── gap-context.md       (scope, target project, resolution tracking)
│   └── coverage-summary.md  (test coverage state)
├── _status/
│   └── workspace-status.md  (pipeline snapshot)
├── setup/
│   └── questionnaire.md     (onboarding questionnaire)
├── shared/
│   ├── gap-record-format.md
│   ├── priority-framework.md
│   ├── resolution-record-format.md
│   ├── validation-checklist.md
│   └── test-coverage-ownership.md
└── stages/
    ├── 01-intake/         CONTEXT.md + output/
    ├── 02-prioritize/     CONTEXT.md + output/
    ├── 03-resolution-design/ CONTEXT.md + output/
    ├── 04-remediation-plan/  CONTEXT.md + output/
    ├── 05-implement/      CONTEXT.md + output/
    └── 06-validate/       CONTEXT.md + output/
```

## Entry Paths

| Path | When | Start At | Stages |
|------|------|----------|--------|
| **Existing analysis** | Gap analysis files already exist | Stage 01 (import mode) | 01 through 06 |
| **Fresh analysis** | No gap analysis yet | Stage 01 (assess mode) | 01 through 06 |

## Cold Start

Load `_status/workspace-status.md` first for pipeline orientation.
Then load `_config/coverage-summary.md` for batch-level detail.
Then load `_config/gap-context.md` for full configuration and resolution tracking.

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `ingest` | Fast path: paste/point to gap analysis file, jump to Stage 01 |
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
| Resolution design | Stage 02 output, shared/resolution-record-format.md | Stage 05-06 files |
| Remediation plan | Stage 03 output, _config/gap-context.md | Stage 01-02 files |
| Implement | Stage 04 output (or Stage 03 for batch/single), target project code | Stage 01-02 files |
| Validate | Stage 05 output, shared/validation-checklist.md, original gap analysis | Stage 01-04 files |

## Stage Handoffs

Each stage writes its output to its own `output/` folder. The next stage reads from there. If you edit an output file, the next stage picks up your edits.

## Gap Scope Routing

| Scope | Description | Stages |
|-------|-------------|--------|
| `single` | One isolated gap | 01 > 03 > 05 > 06 |
| `batch` | Related gaps in one area | 01 > 02 > 03 > 05 > 06 |
| `systematic` | Cross-cutting gaps | 01 > 02 > 03 > 04 > 05 > 06 |

## Global Constraints

- Every resolution must trace back to a documented gap. No opportunistic changes.
- Read affected code before designing a resolution. Patterns found in the codebase take precedence over assumptions.
- Gap records are append-only during resolution. Never delete or modify the original gap description.
- One gap record = one decision. If a gap has multiple valid resolutions, split into separate records.
