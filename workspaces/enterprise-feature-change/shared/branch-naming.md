# Branch Naming

Conventions for branch names derived from Jira ticket IDs and titles.

## Pattern

```
[type]/[TICKET-ID]-[short-title]
```

- `[type]` — one of the values below
- `[TICKET-ID]` — Jira ticket ID, uppercase (e.g. `COMET-42`)
- `[short-title]` — 3–5 words from the ticket title, lowercase-hyphenated

## Type Prefixes

| Ticket Type | Branch Prefix | Example |
|---|---|---|
| Feature | `feature/` | `feature/COMET-42-budget-filter-endpoint` |
| Bug fix | `fix/` | `fix/COMET-87-scheduler-overflow` |
| Enhancement | `enhance/` | `enhance/COMET-103-export-csv` |
| Chore / housekeeping | `chore/` | `chore/COMET-55-update-dependencies` |
| Spike / investigation | `spike/` | `spike/COMET-61-wolverine-feasibility` |
| Hotfix (prod incident) | `hotfix/` | `hotfix/COMET-99-null-ref-in-solver` |

## Rules

- Lowercase everything except the Jira ticket ID
- Hyphens only — no underscores, no slashes within the title segment
- Keep the short title under 40 characters total (after the type and ticket ID)
- Branch from the agreed base branch recorded in `_config/ticket-context.md`

## Commit Message Pattern

```
[TICKET-ID] Short imperative description

Optional longer explanation if the change needs context.

Closes [TICKET-ID]
```

**Examples:**

```
COMET-42 Add budget filter to prioritization endpoint

Adds a `maxBudget` query parameter to GET /api/prioritization.
Projects exceeding the budget are excluded from the ranked list.

Closes COMET-42
```

```
COMET-87 Fix integer overflow in GreedyScheduler budget accumulation
```
