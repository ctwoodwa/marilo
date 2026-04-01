# Doc Standards

Authoritative style and structure rules for this project's documentation.
Filled during Stage 03 or manually before running Stage 04.

## Naming Conventions

| Rule | Pattern | Example |
|---|---|---|
| File names | lowercase-hyphenated | `api-reference.md`, `getting-started.md` |
| Folder names | lowercase-hyphenated | `architecture/`, `how-to/` |
| ADR files | `NNN-short-title.md` | `001-use-event-sourcing.md` |
| H1 headings | Title Case, matches file name | `# Getting Started` |
| H2 headings | Title Case | `## Prerequisites` |
| H3 and below | Sentence case | `### Set up the database` |

## Required Sections by Doc Type

| Doc Type | Required Sections |
|---|---|
| README | Overview, Prerequisites, Getting Started, Architecture (link), Contributing |
| API reference | Endpoint, Method, Auth, Request schema, Response schema, Errors, Example |
| Component / module | Purpose, Usage, Props / Parameters, Dependencies, Known Limitations |
| Architecture decision record | Status, Context, Decision, Consequences |
| How-to guide | Goal, Prerequisites, Steps (numbered), Verification, Troubleshooting |
| Runbook | Trigger, Steps (numbered), Rollback, Owner, Last verified date |

## Style Rules

| Rule | Applies To |
|---|---|
| Use present tense | All docs |
| Use active voice | All docs |
| One blank line between sections | All docs |
| Code blocks must specify the language | All docs |
| Links use relative paths within the repo | All internal links |
| No trailing whitespace | All docs |
| Max heading depth H4 | All docs |

## Tone

`{{DOC_TONE}}`

> Default: Direct and precise. Avoid filler phrases. Assume the reader is a developer familiar with the tech stack.

## Exceptions

`{{DOC_STANDARDS_EXCEPTIONS}}`

> Document any agreed deviations from the above rules here.
