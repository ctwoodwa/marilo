# Stage 05 -- Implement

Execute code, configuration, and process changes to adopt the resolved patterns. Work phase by phase per the remediation plan.

## Purpose

Make the actual code changes that close each gap. Produce an implementation log with per-task evidence, test mappings, and deviation notes. This stage owns the test files.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Remediation plan | `../04-remediation-plan/output/gap-resizable-container-remediation-plan.md` | Current phase | Tasks to execute |
| Resolution records | `../03-resolution-design/output/gap-resizable-container-resolutions.md` | Relevant records | Target patterns and decisions |
| Config | `../../_config/gap-context.md` | Target Project section | Project path and stack |
| Component source | `src/Marilo.Components/Layout/ResizableContainer/` | Affected source files | Code to modify |
| Test file | `tests/Marilo.Tests.Unit/Layout/MariloResizableContainerTests.cs` | Test class | Tests to add/update |
| Test coverage ownership | `../../shared/test-coverage-ownership.md` | Full file | Test mapping contract |

Note: For `single` or `batch` scope (no Stage 04), read directly from Stage 03 resolution records.

## Process

1. Read the remediation plan (or resolution records for single/batch scope). Identify the current phase and its tasks.
2. For the **pilot task** (first area in the phase):
   a. Read the target source file(s).
   b. Implement the change according to the resolution record's target pattern.
   c. Verify the change compiles and existing tests pass.
   d. Write tests that satisfy the Stage 03 success criteria.
   e. Document the implementation as a reference example in the output.
3. Pause for human review of the pilot implementation.
4. For each remaining task in the current phase:
   a. Apply the same pattern, adapting to the specific area.
   b. Write tests for each change.
   c. Verify compilation and tests after each change.
   d. Log what was changed and any deviations from the plan.
5. After all tasks in the phase are complete, run the phase exit criteria from the remediation plan.
6. Save the implementation log to `output/`. Update `_config/gap-context.md` stage status.

## Audit Checklist

| Check | Pass Condition |
|-------|---------------|
| Pilot reviewed | Human approved the pilot before rollout |
| All tasks executed | Every task in the current phase has a log entry |
| Tests pass | No test regressions introduced (`dotnet test` succeeds) |
| Tests written | Each gap has a test file:method mapping in the implementation log |
| No scope creep | Every change traces to a task in the plan |
| Deviations documented | Any deviation from the plan is logged with rationale |
| SCSS rebuilt | `npm run scss:build` ran if any style changes were made |
| Providers synced | IMariloCssProvider, BootstrapCssProvider, FluentUICssProvider, and ProviderSwitcher updated if CSS classes changed |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Implementation log | `output/gap-resizable-container-implementation-log.md` | Per-task log: what changed, files modified, tests written, deviations |
| Reference example | `output/gap-resizable-container-reference-example.md` | Pilot implementation as a pattern reference |
