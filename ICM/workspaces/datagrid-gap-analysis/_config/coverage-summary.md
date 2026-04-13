# Coverage Summary -- MariloDataGrid

**Last updated:** 2026-04-11
**Source:** `ICM/workspaces/datagrid-gap-analysis/stages/01-intake/output/datagrid-gap-intake-2026-04-11.md`

## Gap Count by Priority

| Priority | Count | Description |
|----------|-------|-------------|
| P1 | 2 | Coordinator escalation — naming/API decision required (S-04, S-05) |
| P2 | 12 | Core gaps — this resolution phase (S-02, S-06..S-10, S-15, SA-06..SA-08, SA-11, SA-14) |
| P3 | 16 | Next phase (S-01, S-03, S-11..S-14, S-16..S-17, SA-01..SA-04, SA-05*, SA-09..SA-10, SA-12..SA-13) |
| **Total** | **30** | 17 S-series + 14 SA-series (SA-05 duplicates S-03) |

> `*` SA-05 is a re-confirmation of S-03; tracked for traceability, counts as one resolved item.

## Intake Status

| Stage | Status | Output |
|-------|--------|--------|
| 01-intake | **COMPLETE** | `stages/01-intake/output/datagrid-gap-intake-2026-04-11.md` |
| 02-prioritize | not started | -- |
| 03-resolution-design | not started | -- |
| 04-remediation-plan | not started | -- |
| 05-implement | not started | -- |
| 06-validate | not started | -- |

## Coordinator-Blocked Gaps

The following 6 gaps are blocked pending coordinator decision and will not enter the implementation pipeline:

| Gap ID | Block reason |
|--------|-------------|
| S-04 | Column wrapper markup contract |
| S-05 | CommandColumn element shape (depends on S-04) |
| M-01 | Component naming (`MariloGrid` vs `MariloDataGrid`) |
| M-02 | Column naming (`GridColumn` vs `MariloGridColumn`) |
| M-03 | Virtual-scrolling parameter shape |
| M-05 | `GridState<TItem>` genericization |
| M-12 | Pager settings shape |

## Resolution Progress

| Phase | Gaps | Resolved | Tests Passing | Closure Report |
|-------|------|----------|---------------|----------------|
| Intake | 30 | 0 | n/a | datagrid-gap-intake-2026-04-11.md |
| Prioritize | -- | -- | -- | -- |
| Resolution Design | -- | -- | -- | -- |
| Remediation Plan | -- | -- | -- | -- |
| Implement | -- | -- | -- | -- |
| Validate | -- | -- | -- | -- |
