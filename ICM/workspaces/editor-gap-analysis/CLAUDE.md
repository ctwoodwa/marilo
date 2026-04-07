# Gap Analysis -- MariloEditor

**Status:** STUB -- no gap phases started

Run intake (Stage 01) via ../editor-delivery/ to begin gap analysis. Feature areas tracked in ../editor-delivery/_config/delivery-context.md.

## Folder Map

```
editor-gap-analysis/
├── CLAUDE.md              (you are here)
├── _config/
│   ├── gap-context.md     (scope, target project, resolution tracking)
│   └── coverage-summary.md (test coverage state)
└── output/
    ├── stage-01/          (intake output)
    ├── stage-02/          (prioritization output)
    ├── stage-03/          (resolution design output)
    ├── stage-04/          (remediation plan output)
    ├── stage-05/          (implementation output)
    └── stage-06/          (validation output)
```

## Entry Paths

| Path | When | Start At | Stages |
|------|------|----------|--------|
| **Existing analysis** | Gap analysis files already exist | Stage 01 (import mode) | 01 through 06 |
| **Fresh analysis** | No gap analysis yet; start from target state definition | Stage 01 (assess mode) | 01 through 06 |

## Gap Scope Routing

| Scope | Description | Stages |
|-------|-------------|--------|
| `single` | One isolated gap, one component/module | 01 > 03 > 05 > 06 (skip prioritize and plan) |
| `batch` | Related gaps in one area | 01 > 02 > 03 > 05 > 06 (skip formal plan) |
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
| Import or create a gap analysis | output/stage-01/ |
| Prioritize and sequence gaps | output/stage-02/ |
| Design a resolution for a gap | output/stage-03/ |
| Plan remediation tasks and phases | output/stage-04/ |
| Implement changes | output/stage-05/ |
| Validate closure and enforce | output/stage-06/ |
