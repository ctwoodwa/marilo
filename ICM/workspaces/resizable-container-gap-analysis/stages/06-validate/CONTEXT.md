# Stage 06 -- Validate and Close

Verify that each gap is truly resolved, add enforcement guardrails, and close the gap record.

## Purpose

Confirm every gap is genuinely closed by comparing implementation against resolution designs, running tests, and establishing guardrails to prevent regression. The closure report produced here is the authoritative source for gap status.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Implementation log | `../05-implement/output/gap-resizable-container-implementation-log.md` | Full file | What was changed |
| Original gap inventory | `../01-intake/output/gap-resizable-container-inventory.md` | Full file | Original gap descriptions to verify against |
| Resolution records | `../03-resolution-design/output/gap-resizable-container-resolutions.md` | Full file | Target patterns and consequences |
| Validation checklist | `../../shared/validation-checklist.md` | Full file | Closure criteria framework |
| Config | `../../_config/gap-context.md` | Full file | Project context |
| Component source | `src/Marilo.Components/Layout/ResizableContainer/` | Modified source files | Verify changes in place |
| Test file | `tests/Marilo.Tests.Unit/Layout/MariloResizableContainerTests.cs` | Full file | Verify tests pass |

## Process

1. Read the implementation log and original gap inventory.
2. For each gap record in the inventory:
   a. Locate the corresponding implementation in the component source.
   b. Compare the current code against the resolution record's target pattern.
   c. Run through the validation checklist: does the implementation match the target? Do tests cover the change? Is the original gap behavior no longer present?
   d. Assign a closure status: **Resolved**, **Partially resolved**, **Deferred**, or **Won't fix**.
3. For each resolved gap, define enforcement guardrails:
   - Code review checks (what reviewers should look for).
   - Analyzer rules or linting (if applicable).
   - Documentation updates (API docs, component specs).
   - Demo/example updates.
4. Run `dotnet build` and `dotnet test` to confirm no regressions.
5. Produce a closure report summarizing: resolved count, partial count, deferred count, and any new gaps discovered during implementation.
6. Update `_config/gap-context.md` with final status. Update `_config/coverage-summary.md`.

## Audit Checklist

| Check | Pass Condition |
|-------|---------------|
| All gaps have a closure status | No gap left without a status assignment |
| Evidence provided | Each "Resolved" gap cites specific code/test evidence with file:method mapping |
| Guardrails defined | Each "Resolved" gap has at least one enforcement mechanism |
| No regression | `dotnet test` passes; no previously working behavior is broken |
| Build succeeds | `dotnet build` completes without errors |
| Providers verified | All CSS provider implementations consistent |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Closure report | `output/gap-resizable-container-closure-report.md` | Per-gap closure status, evidence, and guardrails |
| Updated gap index | `output/gap-resizable-container-updated-index.md` | Gap analysis index with resolution status column |
