# Gap Analysis -- MariloFileManager

**Status:** ACTIVE -- Stage 01 intake complete (36 gaps from CDW spec review)

## Folder Map

```
filemanager-gap-analysis/
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
| **Fresh analysis** | No gap analysis yet | Stage 01 (assess mode) | 01 through 06 |

## Gap Scope Routing

| Scope | Description | Stages |
|-------|-------------|--------|
| `single` | One isolated gap | 01 > 03 > 05 > 06 |
| `batch` | Related gaps in one area | 01 > 02 > 03 > 05 > 06 |
| `systematic` | Cross-cutting gaps | 01 > 02 > 03 > 04 > 05 > 06 |

## Current State

- **Intake source:** `filemanager-delivery/stages/01-spec-review/output/filemanager-spec-gap-list.md`
- **36 gaps:** 5 P1 (blocking), 26 P2 (this phase), 5 P3 (next phase)
- **Architecture:** Full rewrite from 170-line scaffold to generic `MariloFileManager<TItem>`
- **Next step:** Stage 02 (prioritize) — batch gaps into implementation phases

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `ingest` | Fast path: paste/point to gap analysis file, jump to Stage 01 import mode |
| `resolve` | Start or continue resolution design (Stage 03) |
| `close` | Jump to validation (Stage 06) for a specific gap |
