# Resolution Design

For each prioritized gap (or batch of related gaps), define the target pattern, evaluate solution options, and capture the decision in a durable resolution record.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | ../02-prioritize/output/gap-scheduler-backlog.md | Current phase gaps | Which gaps to resolve and in what order |
| Config | ../../_config/gap-context.md | "Target Component" section | What resolved looks like |
| Target project | src/Marilo.Components/ | Affected source files | Current implementation to design against |

## Process

1. Read the resolution backlog. Select the next unresolved phase or batch.
2. For each gap (or group of related gaps):
   a. Read the affected source code in the target project.
   b. Define the **target pattern**: what the code/config/process should look like when the gap is closed.
   c. Identify 2-3 **solution options**. For each, describe: approach, pros, cons, effort estimate, and risk.
   d. Choose the recommended option with rationale.
   e. Identify **consequences**: what changes downstream, what breaks, what needs migration.
   f. Write the resolution record.
3. For cross-cutting gaps, produce a single resolution record that covers all affected areas with a consistent pattern.
4. Save all resolution records to output.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| Step 2d | Recommended option with alternatives | Approve, choose a different option, or request more analysis |

## Audit

| Check | Pass Condition |
|-------|---------------|
| Every gap has a resolution | All gaps in the current phase have a resolution record |
| Options evaluated | Each resolution considered at least 2 options |
| Consequences documented | Every resolution lists downstream impacts |
| Patterns grounded in code | Target patterns reference real files/namespaces in the project |
| No spec drift | Resolution aligns with the target state in gap-context.md |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Resolution records | output/gap-scheduler-resolutions.md | One resolution record per gap or batch |

Each resolution record SHOULD include a `### Success Criteria` subsection listing testable conditions that confirm the gap is closed.
