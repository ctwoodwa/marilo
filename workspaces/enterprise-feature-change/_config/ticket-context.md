# Ticket Context

Filled during Stage 01 from the Jira ticket or user story.
This is the single source of truth for what is being built. Every stage reads from here.

## Ticket Identity

| Field | Value |
|---|---|
| Ticket ID | `{{TICKET_ID}}` |
| Ticket type | `{{TICKET_TYPE}}` |
| Title | `{{TICKET_TITLE}}` |
| Ticket URL | `{{TICKET_URL}}` |
| Reporter | `{{REPORTER}}` |
| Assignee | `{{ASSIGNEE}}` |

> Ticket types: `bug`, `feature`, `enhancement`, `chore`, `spike`.

## User Story

```
As a {{PERSONA}},
I want {{GOAL}},
so that {{BENEFIT}}.
```

`{{USER_STORY_OR_DESCRIPTION}}`

## Acceptance Criteria

Numbered list. Every code change must map to at least one item.

| # | Criterion | Verified |
|---|---|---|
| 1 | `{{AC_1}}` | ☐ |
| 2 | `{{AC_2}}` | ☐ |
| 3 | `{{AC_3}}` | ☐ |

> Add rows as needed. Mark `☑` during Stage 04.

## Scope

| Field | Value |
|---|---|
| Ticket size | `{{TICKET_SIZE}}` |
| Stage routing | `{{STAGE_ROUTING}}` |
| Affected projects | `{{AFFECTED_PROJECTS}}` |
| Affected files (initial estimate) | `{{AFFECTED_FILES}}` |
| Out of scope | `{{OUT_OF_SCOPE}}` |

> Ticket sizes: `xs`, `s`, `m`, `l`. See `CLAUDE.md` for routing rules per size.

## Branch

| Field | Value |
|---|---|
| Branch name | `{{BRANCH_NAME}}` |
| Base branch | `{{BASE_BRANCH}}` |

> See `shared/branch-naming.md` for naming conventions.

## Dependencies

| Field | Value |
|---|---|
| Blocked by | `{{BLOCKED_BY}}` |
| Related tickets | `{{RELATED_TICKETS}}` |
| Related ADRs | `{{RELATED_ADRS}}` |

## Notes and Constraints

`{{TICKET_NOTES}}`
