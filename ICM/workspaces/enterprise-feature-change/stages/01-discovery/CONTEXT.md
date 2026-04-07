# Stage 01 - Ticket Intake

Parse the ticket or user story, extract structured acceptance criteria, identify the affected
codebase scope, estimate ticket size, and create the branch. Every downstream stage reads
from the `_config/ticket-context.md` populated here.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Ticket content | Pasted text, Jira URL, or `setup/questionnaire.md` answers | Full ticket | Primary source of truth |
| Project codebase | Target project source tree | File listing (two levels deep) | Grounds the scope estimate in reality |
| Ticket context template | `../../_config/ticket-context.md` | All fields | Output destination |

## Process

1. Parse the ticket text. Extract: ID, title, type, user story, and raw acceptance criteria.
2. Rewrite acceptance criteria as testable, unambiguous statements in the format:
   `Given [context], when [action], then [observable outcome].`
   Flag any AC that is vague or untestable and ask for clarification before continuing.
3. Walk the project source tree and identify which projects, folders, and files are likely
   affected by each acceptance criterion. Record initial estimates in the Scope section.
4. Estimate ticket size using these rules:
   - `xs`: ≤ 2 files changed, no new endpoints or DB columns, single AC
   - `s`: 3–5 files, may add a field or a simple endpoint, 2–4 ACs
   - `m`: multiple projects touched, new endpoint or schema change, 5+ ACs
   - `l`: architecture change, breaking change, new service or migration required
5. Derive the branch name from `shared/branch-naming.md`.
6. Fill all fields in `_config/ticket-context.md`.
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Discovery summary | `output/feature-[slug]-discovery.md` | Structured summary: ticket, ACs, scope, size, branch |
| Filled ticket context | `../../_config/ticket-context.md` | All placeholders replaced |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 2 | Rewritten ACs with any vague ones flagged | Approve or clarify before scope work begins |
| 4 | Size estimate with rationale | Confirm size — this determines which stages to run |
| 6 | Filled `_config/ticket-context.md` | Sign off before any code reading begins |

## Audit

| Check | Pass Condition |
|---|---|
| All ACs are testable | No AC contains "should work", "is correct", or other untestable language |
| Scope is grounded | Affected files list references real paths in the project, not generic descriptions |
| Size agreed | Ticket size is confirmed by the human, not just estimated |
| Branch name set | `{{BRANCH_NAME}}` is filled and follows `shared/branch-naming.md` |
| No placeholders remain | `_config/ticket-context.md` has no unfilled `{{PLACEHOLDER}}` values |
