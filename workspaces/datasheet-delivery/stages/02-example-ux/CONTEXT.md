# Example UX

Audit the demo page for completeness against the API spec, then create or update scenarios to fill gaps.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | Demo page paths |
| Stage 01 output | stages/01-spec-review/output/datasheet-spec-gap-list.md | Full file | Which parameters need demo coverage |
| API spec | /workspaces/Marilo/docs/component-specs/spreadsheet/ | Full directory | Parameter definitions and use cases |
| Demo scenario format | shared/demo-scenario-format.md | Full file | What a complete scenario looks like |
| Existing demo page(s) | /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/DataSheet/ | Full file | Current state |
| Component source | /workspaces/Marilo/src/Marilo.Components/DataGrid/ | Parameter and event declarations | Accurate API surface |

## Process

1. Read the existing demo page(s) and inventory all current scenarios.
2. Read the API spec. For every parameter and event, check demo coverage.
3. Produce a demo gap list: (a) no scenario, (b) stale snippet, (c) no event scenario, (d) missing edge cases.
4. [CHECKPOINT]
5. Write new scenarios following demo-scenario-format.md.
6. Update stale code snippets.
7. Run the Audit checklist before writing to output/.
8. Write output/datasheet-demo-gap-list.md.
9. Write updated demo page file(s).
10. Update _config/delivery-context.md.

## Checkpoint

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| Step 3 | Demo gap list grouped by type (a/b/c/d) with counts | Approve gap list, deprioritize or defer any items, confirm scope |

## Audit

| Check | Pass Condition |
|-------|----------------|
| Every API parameter has at least one scenario | Count of parameters = count with coverage |
| Every API event has at least one scenario | Same check for events |
| All code snippets use current parameter names and types | No deprecated references |
| Every new scenario is interactive | Each has user-controllable input |
| Edge cases demonstrated | Disabled, readonly, empty, error states |
| No Telerik component references in demo page | Search for Telerik imports |
| Scenario titles match use-case language from the spec | No generic titles |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Demo gap list | output/datasheet-demo-gap-list.md | demo-scenario-format.md |
| Updated demo page | /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/DataSheet/ | Blazor .razor file(s) |
