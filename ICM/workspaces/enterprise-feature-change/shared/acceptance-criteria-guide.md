# Acceptance Criteria Guide

How to rewrite raw ACs into testable statements, and how to verify each one systematically.

## Rewriting Raw ACs

Raw ACs from tickets are often vague. Rewrite them before Stage 03 begins.

**Pattern:** `Given [initial context], when [action taken], then [observable outcome].`

| Raw AC | Rewritten |
|---|---|
| "The filter should work" | Given a prioritization request with `maxBudget=500000`, when the endpoint is called, then only projects with annual cost ≤ 500000 are returned |
| "Users can export data" | Given the user is on the project list page, when they click Export CSV, then a CSV file is downloaded containing all visible project rows |
| "Fix the bug with the scheduler" | Given a project set where total cost exceeds INT32_MAX, when the scheduler runs, then no overflow exception is thrown and the result is an empty schedule with a budget-exceeded flag |
| "Admin can manage settings" | Given an authenticated admin user, when they submit the settings form with valid values, then the settings are persisted and a success notification is shown |

## Testability Check

An AC is testable if:
- It names a specific input or precondition
- It describes a specific action
- It describes a specific, observable output (not "works correctly" or "is better")

Flag any AC that fails this check and ask for clarification in Stage 01.

## Verification Methods by AC Type

| AC Type | Best Verification Method |
|---|---|
| API endpoint returns correct data | Integration test with Aspire.Hosting.Testing — assert response body |
| Business logic produces correct output | Unit test — assert return value or output collection |
| Blazor component renders correctly | bunit component test — assert rendered markup |
| Blazor component interaction | bunit component test — trigger click, assert service call or state change |
| Database record created/updated | Integration test — query DB after API call and assert row |
| Error is returned for bad input | Unit or integration test — assert correct status code and problem detail |
| Feature flag gates behavior | Unit test — test both flag-on and flag-off code paths |
| Manual-only (requires live environment) | Document explicit manual steps: precondition, action, expected result |

## Verification Report Format

For each AC, record:

```markdown
### AC 1 — [Criterion text]

**Status:** ☑ Verified

**How:** Unit test `GreedySchedulerTests.Schedule_WithBudgetExceeded_ReturnsEmptySchedule`
asserts that when total project cost > budget, the result has 0 assigned projects
and `BudgetExceeded = true`.

**Test file:** `Parsons.Comet.Tests/Unit/GreedySchedulerTests.cs`
```

For unverified ACs:

```markdown
### AC 3 — [Criterion text]

**Status:** ☐ Manual verification required

**Manual steps:**
1. Log in as an admin user
2. Navigate to /admin/settings
3. Submit the form with valid values
4. Verify: success toast appears, page reloads with saved values
5. Verify: navigating away and back shows the saved values persisted

**Why automated test isn't in scope:** Requires the full Aspire stack running with a seeded DB;
deferred to integration test expansion (COMET-105).
```

## Scope Creep Detection

During Stage 03, if a code change feels necessary but doesn't map to any AC:

1. **Stop.** Do not make the change.
2. Ask: is this a bug that should be fixed as part of this ticket (small, safe, clearly related)?
3. If yes: add it to the change summary with a note, and map it to the closest AC.
4. If no: note it as a follow-up ticket in Stage 05.

The goal is to keep the PR reviewable and the change auditable.
