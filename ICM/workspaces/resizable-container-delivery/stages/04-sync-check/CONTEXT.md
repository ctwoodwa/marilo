# Sync Check

Confirm all four artifacts (spec, Example UX, visual parity, source+tests) are in sync and evaluate the delivery gate.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | All artifact paths |
| Stage 01 output | stages/01-spec-review/output/ | Full file | Spec gap list |
| Stage 02 output | stages/02-example-ux/output/ | Full file | Demo gap list |
| Stage 03 output | stages/03-visual-parity/output/ | Full file | Parity gaps and summary |
| Gap workspace coverage summary | /workspaces/Marilo/workspaces/resizable-container-gap-analysis/_config/coverage-summary.md | Full file | Test and closure state |
| Delivery checklist | shared/delivery-checklist.md | Full file | Gate criteria |

## Process

1. Read all four stage outputs and the coverage summary.
2. For each item in the delivery checklist, evaluate pass/fail.
3. Assign overall gate status: CLEAR / AMBER / BLOCKED.
4. Write output/resizable-container-delivery-report.md.
5. Update _config/delivery-context.md.

## Audit

| Check | Pass Condition |
|-------|----------------|
| All checklist items evaluated | No "unknown" items |
| Every BLOCKED item has a follow-up task | Remediation path exists |
| Gate status matches checklist results | CLEAR only if zero failures |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Delivery report | output/resizable-container-delivery-report.md | delivery-checklist.md |
