# Implement

Execute code, configuration, and process changes to adopt the resolved patterns. Work phase by phase per the remediation plan.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Remediation plan | `../04-remediation-plan/output/gap-[slug]-remediation-plan.md` | Current phase | Tasks to execute |
| Resolution records | `../03-resolution-design/output/gap-[slug]-resolutions.md` | Relevant records | Target patterns and decisions |
| Config | `../../_config/gap-context.md` | "Target Project" section | Project path and stack |
| Target project | Path from gap-context.md | Affected source files | Code to modify |

Note: For `single` or `batch` scope (no Stage 04), read directly from Stage 03 resolution records.

## Process

1. Read the remediation plan (or resolution records for single/batch scope). Identify the current phase and its tasks.
2. For the **pilot task** (first component in the phase):
   a. Read the target source file(s).
   b. Implement the change according to the resolution record's target pattern.
   c. Verify the change compiles and existing tests pass.
   d. Document the implementation as a reference example in the output.
3. Pause for human review of the pilot implementation.
4. For each remaining task in the current phase:
   a. Apply the same pattern, adapting to the specific component/module.
   b. Verify compilation and tests after each change.
   c. Log what was changed and any deviations from the plan.
5. After all tasks in the phase are complete, run the phase exit criteria from the remediation plan.
6. Save the implementation log to output. Update `_config/gap-context.md` stage status.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| Step 2 | Pilot implementation diff and reference example | Approve pattern, request changes, or abort |
| Step 5 | Phase completion report with exit criteria results | Approve phase, proceed to next phase or validation |

## Audit

| Check | Pass Condition |
|-------|---------------|
| Pilot reviewed | Human approved the pilot before rollout |
| All tasks executed | Every task in the current phase has a log entry |
| Tests pass | No test regressions introduced |
| No scope creep | Every change traces to a task in the plan |
| Deviations documented | Any deviation from the plan is logged with rationale |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Implementation log | `output/gap-[slug]-implementation-log.md` | Per-task log: what changed, files modified, deviations |
| Reference example | `output/gap-[slug]-reference-example.md` | Pilot implementation as a pattern reference |
