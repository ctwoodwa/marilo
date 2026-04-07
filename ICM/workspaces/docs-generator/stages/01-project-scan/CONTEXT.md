# Stage 01 - Project Scan

Read the target project's structure and produce an inventory of all existing documentation and key source artifacts.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Project config | `../../_config/project-context.md` | All fields | Defines root paths, scope, and audience |
| Scope config | `../../_config/scope.md` | Exclusions | Filters ignored paths from the scan |

## Process

1. Read `_config/project-context.md` and confirm `{{PROJECT_ROOT}}`, `{{DOCS_FOLDER}}`, and `{{SOURCE_FOLDER}}` are filled. Halt and request values for any unresolved placeholder.
2. Walk `{{PROJECT_ROOT}}` and produce a directory tree — two levels deep by default; expand `{{DOCS_FOLDER}}` and `{{SOURCE_FOLDER}}` fully.
3. List all existing `.md` files under `{{PROJECT_ROOT}}` with their relative paths, titles (H1 if present), and approximate size.
4. Read the root `README.md` if it exists and summarize its current state.
5. Identify key source artifacts: solution or project files, entry points, config files, CI/CD definitions.
6. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Project structure tree | `output/[slug]-project-structure.md` | Markdown |
| Existing docs inventory | `output/[slug]-docs-inventory.md` | Markdown table: path, title, size, has-frontmatter |
| Key source artifacts list | `output/[slug]-source-artifacts.md` | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 1 | Filled config values | Confirm paths are correct before scanning |
| 6 | Docs inventory table | Confirm coverage; flag any missing folders to include |

## Audit

| Check | Pass Condition |
|---|---|
| Paths resolved | No unresolved `{{PLACEHOLDERS}}` in config inputs |
| Docs inventory complete | All `.md` files under `{{PROJECT_ROOT}}` are listed (minus exclusions) |
| Source artifacts identified | At least one solution, project, or entry-point file is recorded |
| Output naming | Artifacts match `output/[slug]-*.md` |
