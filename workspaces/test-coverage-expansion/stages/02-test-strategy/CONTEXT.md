# Stage 02 - Test Strategy

Assign each high- and medium-risk untested area to the correct test layer. Agree on coverage
targets per layer. Produce the strategy document that Stages 03–05 execute.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Coverage audit config | `../../_config/coverage-audit.md` | Untested surface table | Prioritized list to assign |
| Coverage config | `../../_config/coverage-context.md` | Coverage targets, test framework | Target context |
| Test patterns | `../../shared/test-patterns.md` | Layer capabilities and constraints | Informs what each layer can test |
| Risk scoring guide | `../../shared/risk-matrix.md` | Layer assignment guidance | Validates layer choices |

## Process

1. Work through the untested surface table (high-risk first).
2. For each area, assign a test layer (unit / integration / component / e2e) using the layer assignment rules in `shared/risk-matrix.md`.
3. Flag any area where coverage would require significant test infrastructure investment. Propose deferring to a later iteration.
4. Confirm coverage targets per layer with the human. Adjust defaults from `_config/coverage-context.md` if needed.
5. List any areas explicitly out of scope and record the reason.
6. Fill `_config/test-strategy.md` — assignment table and out-of-scope table.
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Test strategy doc | `_config/test-strategy.md` | Filled assignment and out-of-scope tables |
| Strategy summary | `output/[slug]-test-strategy-summary.md` | Layer breakdown counts, target summary, deferred items |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 3 | High-infrastructure items flagged for deferral | Approve deferral list |
| 6 | Complete assignment table and targets | Sign off before scaffolding begins |

## Audit

| Check | Pass Condition |
|---|---|
| All high-risk areas assigned | No high-risk item in the audit is unassigned |
| Layer assignments are valid | No unit test assigned to an area that requires infrastructure |
| Targets agreed | Coverage targets per layer are explicitly recorded in `_config/test-strategy.md` |
| Out-of-scope documented | Every deferred item has a stated reason |
