# Stage 03 -- Resolution Design

For each prioritized gap (or batch of related gaps), define the target pattern, evaluate solution options, and capture the decision in a durable resolution record.

## Purpose

Design how each gap will be closed. Produce resolution records with target patterns, evaluated options, and testable success criteria that Stage 05 implements and Stage 06 validates.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../02-prioritize/output/gap-resizable-container-backlog.md` | Current phase gaps | Which gaps to resolve and in what order |
| Resolution format | `../../shared/resolution-record-format.md` | Full file | Standard shape for decisions (ID prefix: RES-RESIZABLE-CONTAINER) |
| Config | `../../_config/gap-context.md` | Target State section | What resolved looks like |
| Component source | `src/Marilo.Components/Layout/ResizableContainer/` | Affected source files | Current implementation to design against |
| Test coverage ownership | `../../shared/test-coverage-ownership.md` | Full file | Success criteria contract |

## Process

1. Read the resolution backlog. Select the next unresolved phase or batch.
2. For each gap (or group of related gaps):
   a. Read the affected source code in the component.
   b. Define the **target pattern**: what the code should look like when the gap is closed.
   c. Identify 2-3 **solution options**. For each, describe: approach, pros, cons, effort estimate, and risk.
   d. Choose the recommended option with rationale.
   e. Identify **consequences**: what changes downstream, what breaks, what needs migration.
   f. Define **success criteria**: testable conditions that confirm the gap is closed.
   g. Write the resolution record.
3. For cross-cutting gaps, produce a single resolution record that covers all affected areas with a consistent pattern.
4. Save all resolution records to `output/`.

## Audit Checklist

| Check | Pass Condition |
|-------|---------------|
| Every gap has a resolution | All gaps in the current phase have a resolution record |
| Options evaluated | Each resolution considered at least 2 options |
| Consequences documented | Every resolution lists downstream impacts |
| Patterns grounded in code | Target patterns reference real files/namespaces in the project |
| Success criteria defined | Every resolution has testable success criteria |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Resolution records | `output/gap-resizable-container-resolutions.md` | One resolution record per gap or batch, following shared format |
