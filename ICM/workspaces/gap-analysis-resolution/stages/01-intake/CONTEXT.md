# Intake

Import existing gap analysis or assess current state to identify gaps. Normalize all gaps into a standard record format.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Config | `../../_config/gap-context.md` | Full file | Entry path, project details, target state |
| Gap source files | Path(s) from gap-context.md | Full file(s) | Raw gap analysis to import |
| Gap record format | `../../shared/gap-record-format.md` | Full file | Normalization target |
| Target project | Path from gap-context.md | File listing (2 levels deep) | Understand project structure |

## Process

### Import Mode (existing gap analysis)

1. Read the gap analysis source file(s) listed in `_config/gap-context.md`.
2. For each gap found, extract: component/area, gap description, severity, current behavior, target behavior, and recommended change.
3. Normalize each gap into the standard record format from `shared/gap-record-format.md`. Assign a unique ID: `GAP-[area]-[NNN]` (e.g., `GAP-BUTTON-001`).
4. Walk the target project source tree to verify that referenced files, components, and APIs still exist. Flag any stale references.
5. Identify cross-cutting themes (gaps that repeat across multiple components/areas). Tag each gap with its theme(s).
6. Count totals by severity. Update `_config/gap-context.md` with counts and determine scope (single/batch/systematic).
7. Save the normalized gap inventory to output.

### Assess Mode (fresh analysis)

1. Read the target state description from `_config/gap-context.md`.
2. Walk the target project source tree. For each module/component in scope, compare current implementation against the target state.
3. For each deviation, create a gap record in the standard format. Assign severity based on impact.
4. Continue from step 5 of Import Mode above.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| Step 4 (import) or Step 3 (assess) | Normalized gap inventory with counts by severity and theme | Confirm scope, flag any gaps to exclude or split |
| Step 6 | Proposed scope classification (single/batch/systematic) and stage routing | Approve routing |

## Audit

| Check | Pass Condition |
|-------|---------------|
| Every gap has a unique ID | No duplicate GAP-* IDs in the inventory |
| Every gap references real artifacts | File paths verified against target project |
| Severity assigned to all gaps | No gap record missing severity |
| Target state documented | `_config/gap-context.md` has a non-placeholder target state |
| Counts match | Sum of severity counts equals total gap count |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Gap inventory | `output/gap-[slug]-inventory.md` | Normalized gap records grouped by area/theme |
| Updated config | `../../_config/gap-context.md` | Counts, scope, and stage routing filled in |
