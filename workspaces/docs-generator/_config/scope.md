# Generation Scope

Defines which documentation to generate or update in this run.
Filled during setup or before Stage 04.

## Target Docs Folder

`{{DOCS_FOLDER}}`

## Priority Order

| Priority | Doc Type | Action | Target Path |
|---|---|---|---|
| 1 | README | `{{README_ACTION}}` | `{{PROJECT_ROOT}}/README.md` |
| 2 | Architecture overview | `{{ARCH_ACTION}}` | `{{DOCS_FOLDER}}/architecture/overview.md` |
| 3 | API reference | `{{API_ACTION}}` | `{{DOCS_FOLDER}}/api/` |
| 4 | Component docs | `{{COMPONENT_ACTION}}` | `{{DOCS_FOLDER}}/components/` |
| 5 | How-to guides | `{{HOW_TO_ACTION}}` | `{{DOCS_FOLDER}}/how-to/` |
| 6 | ADRs | `{{ADR_ACTION}}` | `{{DOCS_FOLDER}}/decisions/` |
| 7 | Runbooks | `{{RUNBOOK_ACTION}}` | `{{DOCS_FOLDER}}/ops/` |

> **Actions:** `generate` (create from scratch), `update` (revise existing), `skip` (exclude from this run).

## Exclusions

`{{SCOPE_EXCLUSIONS}}`

> List any files or folders to ignore during scan and generation (e.g., auto-generated files, vendor docs).

## Scope Notes

`{{SCOPE_NOTES}}`
