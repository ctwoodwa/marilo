# Example UX

Audit the demo page for completeness against the API spec, then create or update scenarios to fill gaps.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | Demo page paths |
| Stage 01 output | stages/01-spec-review/output/dockmanager-spec-gap-list.md | Full file | Which parameters need demo coverage |
| API spec | /workspaces/Marilo/docs/component-specs/dockmanager/ | Full directory | Parameter definitions and use cases |
| Demo scenario format | shared/demo-scenario-format.md | Full file | What a complete scenario looks like |
| Existing demo page(s) | /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/DockManager/ | Full file | Current state |
| Component source | UNKNOWN | Parameter and event declarations | Accurate API surface |

## Process

1. Read the existing demo page(s) and inventory all current scenarios.
2. Read the API spec. For every parameter and event, check whether at least one existing scenario demonstrates it.
3. Produce a demo gap list:
   a. Parameters with no demo scenario.
   b. Parameters with a scenario but a stale code snippet.
   c. Events with no demo scenario.
   d. Edge cases (disabled, readonly, empty, error states) not demonstrated.
4. [CHECKPOINT]
5. For each item in the approved demo gap list: write a new Blazor scenario section.
6. Update stale code snippets for scenarios already in the demo page.
7. Run the Audit checklist before writing to output/.
8. Write output/dockmanager-demo-gap-list.md.
9. Write the updated demo page file(s) to /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/DockManager/.
10. Update _config/delivery-context.md.

## Checkpoint

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| Step 3 | Demo gap list grouped by type (a/b/c/d) with counts | Approve gap list, confirm scope |

## Audit

| Check | Pass Condition |
|-------|----------------|
| Every API parameter has at least one scenario | Count match |
| Every API event has at least one scenario | Count match |
| All code snippets use current parameter names and types | No deprecated references |
| Every new scenario is interactive | Each has user-controllable input |
| Edge cases demonstrated | Disabled, readonly, empty, error states |
| No Telerik component references in demo page | Clean search |
| Scenario titles match use-case language | No "Test 1" or "Example A" |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Demo gap list | output/dockmanager-demo-gap-list.md | demo-scenario-format.md |
| Updated demo page | /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/DockManager/ | Blazor .razor file(s) |
