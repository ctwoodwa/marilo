# Sync Check

Confirm all four artifacts (spec, Example UX, visual parity, source+tests) are in sync and evaluate the delivery gate.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | All artifact paths |
| Stage 01 output | stages/01-spec-review/output/ | Full file | Spec gap list |
| Stage 02 output | stages/02-example-ux/output/ | Full file | Demo gap list |
| Stage 03 output | stages/03-visual-parity/output/ | Full file | Parity gaps and summary |
| Gap workspace coverage summary | /workspaces/Marilo/workspaces/gap-analysis-resolution/_config/coverage-summary.md | Full file | Test and closure state |
| Delivery checklist | shared/delivery-checklist.md | Full file | Gate criteria |

## Process

1. Read all four stage outputs and the coverage summary.
2. For each item in the delivery checklist, evaluate pass/fail using the stage outputs as evidence.
3. Assign overall gate status:
   - CLEAR: all checklist items pass; component is in sync.
   - AMBER: minor gaps remain; documented with follow-up tasks.
   - BLOCKED: one or more blocking items prevent delivery gate passing.
4. Write output/treeview-delivery-report.md.
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
| Delivery report | output/treeview-delivery-report.md | delivery-checklist.md |
