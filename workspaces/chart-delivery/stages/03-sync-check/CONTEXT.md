# Sync Check

Confirm all three artifacts (spec, Example UX, source+tests) are in sync and evaluate the delivery gate.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | All artifact paths |
| Stage 01 output | stages/01-spec-review/output/ | Full file | Spec gap list |
| Stage 02 output | stages/02-example-ux/output/ | Full file | Demo gap list |
| Gap workspace coverage summary | /workspaces/Marilo/workspaces/chart-gap-analysis/_config/coverage-summary.md | Full file | Test and closure state |
| Delivery checklist | shared/delivery-checklist.md | Full file | Gate criteria |

## Process

1. Read all three stage outputs and the coverage summary.
2. For each item in the delivery checklist, evaluate pass/fail using the stage outputs as evidence.
3. Assign overall gate status:
   - CLEAR: all checklist items pass; component is in sync.
   - AMBER: minor gaps remain; documented with follow-up tasks.
   - BLOCKED: one or more blocking items prevent delivery gate passing.
4. Write output/chart-delivery-report.md.
5. Update _config/delivery-context.md: last sync check date, gate status, and blocking item count.

## Audit

| Check | Pass Condition |
|-------|----------------|
| All checklist items evaluated | No checklist item left as "unknown" |
| Every BLOCKED item has a follow-up task with owner and phase | No open BLOCKED items without a remediation path |
| Gate status matches checklist results | CLEAR only if zero failures; AMBER if all failures are non-blocking |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Delivery report | output/chart-delivery-report.md | delivery-checklist.md |
