# Sync Check

Confirm all three artifacts (spec, Example UX, source+tests) are in sync and evaluate the delivery gate.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | All artifact paths |
| Stage 01 output | stages/01-spec-review/output/ | Full file | Spec gap list |
| Stage 02 output | stages/02-example-ux/output/ | Full file | Demo gap list |
| Gap workspace coverage summary | /workspaces/Marilo/workspaces/datasheet-gap-analysis/_config/coverage-summary.md | Full file | Test and closure state |
| Delivery checklist | shared/delivery-checklist.md | Full file | Gate criteria |

## Process

1. Read all three stage outputs and the coverage summary.
2. For each item in the delivery checklist, evaluate pass/fail.
3. Assign overall gate status: CLEAR / AMBER / BLOCKED.
4. Write output/datasheet-delivery-report.md.
5. Update _config/delivery-context.md: last sync check date, gate status, blocking item count.

## Audit

| Check | Pass Condition |
|-------|----------------|
| All checklist items evaluated | No item left as "unknown" |
| Every BLOCKED item has a follow-up task | No open BLOCKED items without remediation |
| Gate status matches checklist results | CLEAR only if zero failures |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Delivery report | output/datasheet-delivery-report.md | delivery-checklist.md |
