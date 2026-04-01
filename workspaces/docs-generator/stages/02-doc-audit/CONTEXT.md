# Stage 02 - Doc Audit

Score each existing document for completeness, style compliance, and naming compliance. Identify documentation gaps.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Docs inventory | `../01-project-scan/output/[slug]-docs-inventory.md` | Full list | Defines what to audit |
| Project structure | `../01-project-scan/output/[slug]-project-structure.md` | Full tree | Reveals what should be documented but isn't |
| Doc standards | `../../_config/doc-standards.md` | All rules | Scoring rubric |
| Scope config | `../../_config/scope.md` | Doc types, exclusions | Determines what gaps to flag |

## Process

1. For each doc in the inventory, score it against `_config/doc-standards.md`:
   - **Completeness** (0–100): count required sections present vs. missing for the doc type.
   - **Style compliance** (pass/fail per rule): naming, tone, formatting, heading depth.
   - **Coverage fit**: does this doc map to a required doc type in `_config/scope.md`?
2. Compare the scope's required doc types against the inventory. Flag each missing doc as a gap.
3. Assign a priority to each gap: high (blocking a required doc type), medium (important but not blocking), low (nice-to-have).
4. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Doc audit report | `output/[slug]-doc-audit.md` | Scored table per doc: completeness, style violations, coverage fit |
| Gap list | `output/[slug]-gap-list.md` | Table: gap description, doc type, priority, suggested target path |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 1 | Draft audit scores | Confirm the rubric is applied correctly; adjust any scores |
| 3 | Gap list with priorities | Approve priorities or reorder before Stage 04 |

## Audit

| Check | Pass Condition |
|---|---|
| All inventory docs scored | Every doc in `[slug]-docs-inventory.md` has a row in the audit report |
| Gaps are traceable | Each gap references the scope doc type it satisfies |
| Priorities assigned | Every gap has a priority (high / medium / low) |
| Output naming | Artifacts match `output/[slug]-*.md` |
