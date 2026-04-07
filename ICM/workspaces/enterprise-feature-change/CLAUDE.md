# Enterprise Feature Change Workspace

Take a Jira ticket or user story from intake to merged PR. One agent reads the ticket,
reads the affected code, writes the changes, verifies the acceptance criteria, and
produces a PR-ready package — without losing context between steps.

## Folder Map

```
enterprise-feature-change/
├── CLAUDE.md                            (you are here)
├── CONTEXT.md                           (workspace routing and rules)
├── _config/
│   └── ticket-context.md                (ticket details, ACs, size, affected files)
├── stages/
│   ├── 01-discovery/                    (parse ticket, extract ACs, identify scope)
│   ├── 02-architecture-impact/          (read affected code, map changes to files)
│   ├── 03-design-spec/                  (write code changes — the main implementation stage)
│   ├── 04-implementation-plan/          (write/update tests, verify acceptance criteria)
│   └── 05-review-and-pr/               (draft PR description, run final checklist)
├── shared/
│   ├── branch-naming.md                 (branch name conventions)
│   ├── pr-template.md                   (PR description template)
│   └── acceptance-criteria-guide.md     (how to verify ACs systematically)
├── setup/
│   └── questionnaire.md                 (onboarding questions)
└── output/                              (consolidated artifacts across all stages)
```

## Ticket Size and Stage Routing

The number of stages to run depends on ticket size. Determine size in Stage 01.

| Size | Definition | Stages |
|---|---|---|
| `xs` | Bug fix or 1–2 file change, < 4 hours | 01 → 03 → 05 |
| `s` | Small feature or enhancement, 3–5 files, < 2 days | 01 → 02 → 03 → 05 |
| `m` | Feature touching multiple services or adding new endpoints, < 2 weeks | 01 → 02 → 03 → 04 → 05 |
| `l` | Feature with architecture impact, breaking changes, or migration required | 01 → 02 + human approval → 03 → 04 → 05 |

## Task Routing

| Task Type | Go To |
|---|---|
| First-time setup | `setup/questionnaire.md` |
| Parse ticket and determine scope | `stages/01-discovery/CONTEXT.md` |
| Read code, map changes to files | `stages/02-architecture-impact/CONTEXT.md` |
| Write the code implementation | `stages/03-design-spec/CONTEXT.md` |
| Write tests, verify acceptance criteria | `stages/04-implementation-plan/CONTEXT.md` |
| Draft PR and run final checklist | `stages/05-review-and-pr/CONTEXT.md` |

## Triggers

| Keyword | Action |
|---|---|
| `setup` | Read `setup/questionnaire.md` and populate `_config/ticket-context.md` |
| `status` | Scan `stages/*/output/` and show what's done, what's next |
| `ticket` | Start Stage 01 immediately — paste the ticket text or provide the ticket ID |
| `implement` | Jump to Stage 03; requires `_config/ticket-context.md` and Stage 02 output |
| `pr` | Jump to Stage 05; requires Stage 03 output |

## Global Constraints

- Read the affected code before writing any changes. Do not guess at existing patterns.
- Follow the conventions in the files you read — naming, formatting, DI approach, error handling.
- Do not change files outside the scope identified in Stage 01 unless a new dependency is discovered and approved.
- Every code change must map to at least one acceptance criterion from `_config/ticket-context.md`.
- Do not mark a stage complete until its Audit checks pass.
