# Naming Conventions

Reference for file, folder, and section naming across all generated documentation.

## File Names

| Rule | Pattern | Example |
|---|---|---|
| All doc files | `lowercase-hyphenated.md` | `getting-started.md`, `api-reference.md` |
| ADR files | `NNN-short-description.md` | `001-use-event-sourcing.md` |
| Runbook files | `[service]-[scenario].md` | `payments-service-restart.md` |
| Template files | `*-template.md` | `readme-template.md` |

## Folder Names

| Rule | Pattern | Example |
|---|---|---|
| All doc folders | `lowercase-hyphenated/` | `how-to/`, `architecture/`, `api/` |
| Stage output folders | `output/` | Always `output/` exactly |

## Heading Patterns

| Level | Rule | Example |
|---|---|---|
| H1 | Title Case, one per file, matches the file name conceptually | `# Getting Started` |
| H2 | Title Case | `## Prerequisites` |
| H3 | Sentence case | `### Set up the database` |
| H4 | Sentence case | `#### Example response` |

## Standard Section Names

Use these names consistently across doc types:

| Doc Type | Standard Section Names |
|---|---|
| All | `## Overview`, `## Prerequisites`, `## Getting Started` |
| API docs | `## Endpoints`, `## Authentication`, `## Errors`, `## Examples` |
| Component docs | `## Purpose`, `## Usage`, `## Props / Parameters`, `## Dependencies`, `## Known Limitations` |
| Architecture | `## Context`, `## Decision`, `## Consequences`, `## Diagram` |
| Runbook | `## Trigger`, `## Steps`, `## Rollback`, `## Owner` |
| How-to | `## Goal`, `## Prerequisites`, `## Steps`, `## Verification`, `## Troubleshooting` |

## Stage Artifact File Names

All stage artifacts follow `[slug]-[artifact-type].md`:

| Artifact | Pattern |
|---|---|
| Project structure | `[slug]-project-structure.md` |
| Docs inventory | `[slug]-docs-inventory.md` |
| Source artifacts | `[slug]-source-artifacts.md` |
| Doc audit | `[slug]-doc-audit.md` |
| Gap list | `[slug]-gap-list.md` |
| Confirmed standards | `[slug]-confirmed-standards.md` |
| Generation log | `[slug]-generation-log.md` |
| Consistency review | `[slug]-consistency-review.md` |
| Final checklist | `[slug]-final-checklist.md` |
| Final inventory | `[slug]-final-inventory.md` |
