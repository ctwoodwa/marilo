# Stage 01 -- Intake

Import existing gap analysis or assess current state to identify gaps in MariloResizableContainer. Normalize all gaps into a standard record format.

## Purpose

Discover and document all gaps between the ResizableContainer specification and its current implementation. Produce a normalized gap inventory that downstream stages can prioritize, design against, and resolve.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Config | `../../_config/gap-context.md` | Full file | Entry path, project details, target state |
| Gap source files | Path(s) from gap-context.md | Full file(s) | Raw gap analysis to import |
| Gap record format | `../../shared/gap-record-format.md` | Full file | Normalization target (ID prefix: GAP-RESIZABLE-CONTAINER) |
| Component source | `src/Marilo.Components/Layout/ResizableContainer/` | Full directory | Current implementation to assess |

## Process

### Import Mode (existing gap analysis)

1. Read the gap analysis source file(s) listed in `_config/gap-context.md`.
2. For each gap found, extract: category, gap description, severity, current behavior, target behavior, and recommended change.
3. Normalize each gap into the standard record format. Assign a unique ID: `GAP-RESIZABLE-CONTAINER-[NNN]`.
4. Walk the component source tree to verify that referenced files and APIs still exist. Flag any stale references.
5. Identify cross-cutting themes (gaps that repeat across multiple categories). Tag each gap with its theme(s).
6. Count totals by severity. Update `_config/gap-context.md` with counts and determine scope (single/batch/systematic).
7. Save the normalized gap inventory to `output/`.

### Assess Mode (fresh analysis)

1. Read the target state description from `_config/gap-context.md`.
2. Walk the component source tree at `src/Marilo.Components/Layout/ResizableContainer/`. Compare current implementation against the target state / component specification.
3. For each deviation, create a gap record in the standard format. Assign severity based on impact.
4. Continue from step 5 of Import Mode above.

## Audit Checklist

| Check | Pass Condition |
|-------|---------------|
| Every gap has a unique ID | No duplicate GAP-RESIZABLE-CONTAINER-* IDs in the inventory |
| Every gap references real artifacts | File paths verified against component source |
| Severity assigned to all gaps | No gap record missing severity |
| Target state documented | `_config/gap-context.md` has a non-placeholder target state |
| Counts match | Sum of severity counts equals total gap count |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Gap inventory | `output/gap-resizable-container-inventory.md` | Normalized gap records grouped by category |
| Updated config | `../../_config/gap-context.md` | Counts, scope, and stage routing filled in |
