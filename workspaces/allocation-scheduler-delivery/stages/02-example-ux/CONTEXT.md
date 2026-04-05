# Example UX

Audit the demo page for completeness against the API spec, then create or update scenarios to fill gaps.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Delivery config | _config/delivery-context.md | Full file | Demo page paths |
| Stage 01 output | stages/01-spec-review/output/allocation-scheduler-spec-gap-list.md | Full file | Which parameters need demo coverage |
| API spec | /workspaces/Marilo/docs/component-specs/allocation-scheduler/ | Full directory | Parameter definitions and use cases |
| Demo scenario format | shared/demo-scenario-format.md | Full file | What a complete scenario looks like |
| Existing demo page(s) | /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/AllocationScheduler/ | Full file | Current state |
| Component source | /workspaces/Marilo/src/Marilo.Components/DataDisplay/AllocationScheduler/ | Parameter and event declarations | Accurate API surface |

## Process

1. Read the existing demo page(s) and inventory all current scenarios. For each scenario note: which parameters it demonstrates, whether the code snippet matches the current API, and whether it is interactive.
2. Read the API spec. For every parameter and event, check whether at least one existing scenario demonstrates it as the primary focus.
3. Produce a demo gap list:
   a. Parameters with no demo scenario.
   b. Parameters with a scenario but a stale code snippet.
   c. Events with no demo scenario.
   d. Edge cases (disabled, readonly, empty, error states) not demonstrated.
4. [CHECKPOINT]
5. For each item in the approved demo gap list:
   a. Write a new Blazor scenario section following demo-scenario-format.md.
   b. Scenario must use the actual Marilo component (not pseudocode).
   c. Include: title, live interactive example, code snippet panel, parameter table, link to spec section.
   d. Place the new section in the correct location in the demo page.
6. Update stale code snippets for scenarios already in the demo page.
7. Run the Audit checklist before writing to output/.
8. Write output/allocation-scheduler-demo-gap-list.md (the gap audit).
9. Write the updated demo page file(s) to /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/AllocationScheduler/.
10. Update _config/delivery-context.md: last demo audit date and open demo gap count (0 after this stage completes).

## Checkpoint

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| Step 3 | Demo gap list grouped by type (a/b/c/d) with counts | Approve gap list, deprioritize or defer any items, confirm scope before writing begins |

## Audit

| Check | Pass Condition |
|-------|----------------|
| Every API parameter has at least one scenario | Count of parameters = count of parameters with scenario coverage |
| Every API event has at least one scenario | Same check for events |
| All code snippets use current parameter names and types | No snippet references a deprecated or renamed parameter |
| Every new scenario is interactive | Each scenario has at least one user-controllable input |
| Edge cases demonstrated | Disabled, readonly, empty state, and error state each have a scenario |
| No Telerik component references in demo page | Search demo page source for Telerik namespace imports |
| Scenario titles match use-case language from the spec | No scenario titled "Test 1" or "Example A" |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Demo gap list | output/allocation-scheduler-demo-gap-list.md | demo-scenario-format.md |
| Updated demo page | /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/AllocationScheduler/ | Blazor .razor file(s) |
