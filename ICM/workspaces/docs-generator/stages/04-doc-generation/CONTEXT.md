# Stage 04 - Doc Generation

Write new documentation and update existing docs. Apply confirmed standards to every file produced.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Gap list | `../02-doc-audit/output/[slug]-gap-list.md` | All high + medium priority gaps | Drives what gets generated |
| Doc audit report | `../02-doc-audit/output/[slug]-doc-audit.md` | Docs needing update | Drives what gets revised |
| Confirmed standards | `../03-standards-alignment/output/[slug]-confirmed-standards.md` | All rules | Applied to every doc written |
| Project structure | `../01-project-scan/output/[slug]-project-structure.md` | Full tree | Source of truth for content accuracy |
| Source artifacts | `../01-project-scan/output/[slug]-source-artifacts.md` | All | Content source for API and component docs |
| Doc templates | `../../shared/doc-templates/` | Matching type | Starting point for each doc type |
| Scope config | `../../_config/scope.md` | Priority order, target paths, actions | Determines write order, destinations, and skip list |

## Process

1. Work through `[slug]-gap-list.md` in priority order (high first).
2. For each gap: select the matching template from `shared/doc-templates/`, fill it from project scan artifacts, apply confirmed standards, write to the target path from `_config/scope.md`.
3. For each existing doc flagged in the audit as needing update: apply standards corrections in place at its current path.
4. After writing the first doc, present it for approval before continuing with the remainder.
5. Log every file written or updated.
6. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Generated docs | `{{DOCS_FOLDER}}/...` | Markdown (one file per gap or update) |
| Generation log | `output/[slug]-generation-log.md` | Table: file, action (generated/updated), source gap or audit item |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 2 (first doc only) | First generated doc at highest priority | Approve quality before bulk generation continues |
| 5 | Full generation log | Confirm all items were handled; flag any rework needed |

## Audit

| Check | Pass Condition |
|---|---|
| All high-priority gaps addressed | Every high-priority item in the gap list has a generated file |
| Standards applied | Each generated doc passes completeness check against confirmed standards |
| Paths correct | Every file is written to the path defined in `_config/scope.md` |
| Log complete | `[slug]-generation-log.md` accounts for every gap and every update |
