# Stage 05 - Consistency Review

Cross-check all documentation in `{{DOCS_FOLDER}}` for naming consistency, link validity, required sections, and cross-reference accuracy.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Generation log | `../04-doc-generation/output/[slug]-generation-log.md` | All files written | Defines the primary review set |
| Confirmed standards | `../03-standards-alignment/output/[slug]-confirmed-standards.md` | All | Final scoring rubric |
| All docs in docs folder | `{{DOCS_FOLDER}}/` | All `.md` files | Full subject of review |
| Naming conventions | `../../shared/naming-conventions.md` | All | Validates file and section names |

## Process

1. Re-scan `{{DOCS_FOLDER}}` and produce a final inventory.
2. For each file in the generation log, verify it exists at the expected path.
3. Check all internal links (`[text](path)`) in every doc — flag any that are broken or point to wrong targets.
4. Verify required sections are present in each doc (by doc type, per confirmed standards).
5. Check naming consistency: file names, folder names, heading case patterns.
6. Produce a final review report and a pass/fail checklist.
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Consistency review report | `output/[slug]-consistency-review.md` | Issues table: file, check, status, suggested fix |
| Final docs checklist | `output/[slug]-final-checklist.md` | Checklist per doc: all checks pass/fail |
| Final docs inventory | `output/[slug]-final-inventory.md` | Updated table of all docs after generation |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 3 | Broken links list | Approve fixes or mark as known issues |
| 6 | Review report + final checklist | Accept or request a targeted rework pass |

## Audit

| Check | Pass Condition |
|---|---|
| All generated files verified | Every file in the generation log exists at its target path |
| No broken internal links | All `[text](path)` links resolve within the repo |
| Required sections present | Every doc has all sections required by its doc type |
| Naming compliant | All file and folder names match `shared/naming-conventions.md` |
| Checklist complete | Every doc has a row in `[slug]-final-checklist.md` |
