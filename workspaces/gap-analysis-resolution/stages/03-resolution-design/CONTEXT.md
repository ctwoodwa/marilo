# Resolution Design

For each prioritized gap (or batch of related gaps), define the target pattern, evaluate solution options, and capture the decision in a durable resolution record.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../02-prioritize/output/gap-[slug]-backlog.md` | Current phase gaps | Which gaps to resolve and in what order |
| Resolution format | `../../shared/resolution-record-format.md` | Full file | Standard shape for decisions |
| Config | `../../_config/gap-context.md` | "Target State" section | What resolved looks like |
| Target project | Path from gap-context.md | Affected source files | Current implementation to design against |
| Canonical defaults | `../../references/layer-3-defaults-index.md` | Relevant sections | Enterprise patterns that apply |

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
4. If a resolution requires an ADR (architectural decision), draft it using the ADR template from canonical defaults.
5. Save all resolution records to output.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| Step 2d | Recommended option with alternatives | Approve, choose a different option, or request more analysis |
| Step 4 | Draft ADR (if applicable) | Approve or revise the architectural decision |

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
| Resolution records | `output/gap-[slug]-resolutions.md` | One resolution record per gap or batch, following shared format |
| ADRs (if any) | `output/gap-[slug]-adr-[NNN].md` | ADR format from canonical defaults |
