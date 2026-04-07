# Example UX

Audit the demo page for completeness against the API spec, then create or update scenarios to fill gaps.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | Demo page paths |
| Stage 01 output | stages/01-spec-review/output/pivotgrid-spec-gap-list.md | Full file | Which parameters need demo coverage |
| API spec | /workspaces/Marilo/docs/component-specs/pivotgrid/ | Full directory | Parameter definitions and use cases |
| Demo scenario format | shared/demo-scenario-format.md | Full file | What a complete scenario looks like |
| Existing demo page(s) | /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/PivotGrid/ | Full file | Current state |
| Component source | UNKNOWN | Parameter and event declarations | Accurate API surface |

## Process

1. Read the existing demo page(s) and inventory all current scenarios.
2. Read the API spec. For every parameter and event, check demo coverage.
3. Produce a demo gap list (a-d categories).
4. [CHECKPOINT]
5. Write new Blazor scenario sections for each gap.
6. Update stale code snippets.
7. Run the Audit checklist.
8. Write output/pivotgrid-demo-gap-list.md.
9. Write the updated demo page file(s).
10. Update _config/delivery-context.md.

## Checkpoint

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| Step 3 | Demo gap list grouped by type with counts | Approve gap list, confirm scope |

## Audit

| Check | Pass Condition |
|-------|----------------|
| Every API parameter has at least one scenario | Count match |
| Every API event has at least one scenario | Count match |
| All code snippets use current parameter names and types | No deprecated refs |
| Every new scenario is interactive | User-controllable input present |
| Edge cases demonstrated | Disabled, readonly, empty, error |
| No Telerik component references in demo page | Clean search |
| Scenario titles match use-case language | No generic titles |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Demo gap list | output/pivotgrid-demo-gap-list.md | demo-scenario-format.md |
| Updated demo page | /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/PivotGrid/ | Blazor .razor file(s) |
