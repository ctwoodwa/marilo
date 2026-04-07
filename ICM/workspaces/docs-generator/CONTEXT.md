# Docs Generator — Workspace Context

Task routing and rules for generating and updating project documentation.

## Task Routing

| Task Type | Go To Stage | Description |
|---|---|---|
| Read project structure and inventory existing docs | `stages/01-project-scan/` | Build the foundation for all later stages |
| Score existing docs against standards | `stages/02-doc-audit/` | Identify gaps, inconsistencies, and violations |
| Confirm or derive doc standards for this project | `stages/03-standards-alignment/` | Lock in style, naming, and structure rules |
| Write new docs or update existing ones | `stages/04-doc-generation/` | Produce or revise documentation in `{{DOCS_FOLDER}}` |
| Cross-check all docs for consistency | `stages/05-consistency-review/` | Validate links, sections, naming, and completeness |

## Reference Materials

| Resource | Location | Contains |
|---|---|---|
| Project paths and tech stack | `_config/project-context.md` | Filled during setup |
| Style rules and required sections | `_config/doc-standards.md` | Authoritative standards for this run |
| Generation scope and priority | `_config/scope.md` | Which doc types to produce and in what order |
| Doc templates | `shared/doc-templates/` | Blank starting points for common doc types |
| Naming reference | `shared/naming-conventions.md` | File naming and section heading patterns |
| Stage outputs | `stages/*/output/` | Artifacts produced at each stage |
| Workspace-level reports | `output/` | Optional consolidated summaries |

## Rules

1. Run stages 01 → 05 in order unless a shortcut is explicitly permitted.
2. Carry each stage's output forward as input to the next stage.
3. Use `[slug]-*.md` for all output file names, where `slug` matches `{{PROJECT_SLUG}}` from `_config/project-context.md`.
4. Generate documentation only — do not read, modify, or propose changes to source code files.
5. Apply `_config/doc-standards.md` consistently; flag any rule conflict rather than silently choosing one.
