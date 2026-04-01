# Setup Questionnaire

Run the `setup` trigger to start onboarding, or paste your ticket content directly after `ticket`.
The agent fills `_config/ticket-context.md` from your answers.

---

**Option A — Paste the full ticket**

Paste the Jira ticket or user story text. The agent will extract all fields automatically
and ask you to confirm the acceptance criteria and scope before proceeding.

---

**Option B — Answer questions**

1. What is the Jira ticket ID? (e.g. COMET-42)
2. What is the ticket title?
3. What type of ticket is this? (bug / feature / enhancement / chore / spike)
4. What is the ticket URL? (or "none")
5. Paste the full ticket description or user story.
6. List the acceptance criteria, one per line. Number them.
7. Which projects or services does this change touch?
   (e.g. Parsons.Comet.ApiService, Parsons.Comet.Web — or "unknown, determine in Stage 01")
8. Is anything explicitly out of scope for this ticket?
9. What is the base branch to branch from? (default: main or develop)
10. Are there any blocking tickets or dependencies?

---

Derived automatically from your answers:

- `{{TICKET_SLUG}}` — Jira ID lowercased, used in output file names (e.g. `comet-42`)
- `{{BRANCH_NAME}}` — derived from ticket type + ID + short title (see `shared/branch-naming.md`)
- `{{TICKET_SIZE}}` — estimated during Stage 01 based on affected file count and AC complexity
- `{{STAGE_ROUTING}}` — set during Stage 01 once size is confirmed
