# Docs Generator Workspace

Generate and update documentation for an existing project: scan the source tree, audit what exists, apply style and naming standards, and write consistent markdown files into the project's `docs/` folder.

## Folder Map

```
docs-generator/
├── CLAUDE.md                      (you are here)
├── CONTEXT.md                     (workspace routing and rules)
├── _config/                       (filled during setup)
│   ├── project-context.md         (project paths, tech stack, audience)
│   ├── doc-standards.md           (style rules, naming conventions, required sections)
│   └── scope.md                   (which doc types to generate, priority order)
├── stages/
│   ├── 01-project-scan/           (read project structure and inventory existing docs)
│   ├── 02-doc-audit/              (score existing docs for completeness and standards compliance)
│   ├── 03-standards-alignment/    (confirm or derive doc standards for this project)
│   ├── 04-doc-generation/         (write new docs and update existing ones)
│   └── 05-consistency-review/     (cross-check all docs, links, naming, required sections)
├── shared/
│   ├── doc-templates/             (blank templates for common doc types)
│   └── naming-conventions.md      (file and section naming reference)
├── setup/
│   └── questionnaire.md           (onboarding questions)
└── output/                        (workspace-level summaries and reports)
```

## Deployment

This workspace is designed to run **inside the target project**.

To deploy into a project:

```bash
cp -r workspaces/docs-generator <target-project>/icm/docs-generator
```

Then fill `_config/project-context.md`. Set `{{PROJECT_ROOT}}` to the absolute path of the
target project, or use `../..` if running from the deployed location inside the project.

## Task Routing

| Task Type | Go To |
|---|---|
| First-time setup | `setup/questionnaire.md` |
| Scan project and inventory docs | `stages/01-project-scan/CONTEXT.md` |
| Audit existing docs quality | `stages/02-doc-audit/CONTEXT.md` |
| Set or confirm doc standards | `stages/03-standards-alignment/CONTEXT.md` |
| Generate or update docs | `stages/04-doc-generation/CONTEXT.md` |
| Final consistency check | `stages/05-consistency-review/CONTEXT.md` |

## Triggers

| Keyword | Action |
|---|---|
| `setup` | Read `setup/questionnaire.md` and run onboarding, populate `_config/` |
| `status` | Scan all `stages/*/output/` and show pipeline completion view |
| `audit` | Skip to Stage 02 with a pre-filled project path prompt |
| `generate` | Skip to Stage 04; requires Stage 03 output or a filled `_config/doc-standards.md` |
| `deploy` | Print the copy command and post-copy checklist for deploying into a target project |

## Global Constraints

- Never modify source code files; this workspace produces documentation artifacts only.
- All generated docs are written to `{{DOCS_FOLDER}}` defined in `_config/project-context.md`.
- Output file names follow the naming conventions in `shared/naming-conventions.md`.
- Stage outputs carry forward as inputs to the next stage; do not skip stages unless permitted.
- Treat `_config/doc-standards.md` as authoritative; do not invent style rules mid-run.
