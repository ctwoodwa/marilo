# Validate and Close

Verify that each gap is truly resolved, add enforcement guardrails, and close the gap record.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Implementation log | ../05-implement/output/gap-scheduler-implementation-log.md | Full file | What was changed |
| Original gap inventory | ../01-intake/output/gap-scheduler-inventory.md | Full file | Original gap descriptions to verify against |
| Resolution records | ../03-resolution-design/output/gap-scheduler-resolutions.md | Full file | Target patterns and consequences |
| Config | ../../_config/gap-context.md | Full file | Project context |
| Target project | src/Marilo.Components/ | Modified source files | Verify changes in place |

## Process

1. Read the implementation log and original gap inventory.
2. For each gap record in the inventory:
   a. Locate the corresponding implementation in the target project.
   b. Compare the current code against the resolution record's target pattern.
   c. Run through the validation checklist: does the implementation match the target? Do tests cover the change? Is the original gap behavior no longer present?
   d. Assign a closure status: **Resolved** (fully closed), **Partially resolved** (needs follow-up), **Deferred** (intentionally postponed), **Won't fix** (accepted as-is with rationale).
3. For each resolved gap, define enforcement guardrails:
   - Code review checks (what reviewers should look for).
   - Analyzer rules or linting (if applicable).
   - Documentation updates (API docs, component specs, guides).
   - Template/example updates (reference implementations).
4. Produce a closure report summarizing: resolved count, partial count, deferred count, and any new gaps discovered during implementation.
5. Update `_config/gap-context.md` with final status.
6. Update `_config/coverage-summary.md` with test counts.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| Step 2 | Closure status for each gap with evidence | Override any closure status, flag gaps needing more work |
| Step 4 | Final closure report | Accept report, create follow-up tickets for partial/deferred gaps |

## Audit

| Check | Pass Condition |
|-------|---------------|
| All gaps have a closure status | No gap left without a status assignment |
| Evidence provided | Each "Resolved" gap cites specific code/test evidence |
| Guardrails defined | Each "Resolved" gap has at least one enforcement mechanism |
| No regression | All existing tests still pass after changes |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Closure report | output/gap-scheduler-closure-report.md | Per-gap closure status, evidence, and guardrails |
| Updated gap index | output/gap-scheduler-updated-index.md | Gap analysis index with resolution status column |
