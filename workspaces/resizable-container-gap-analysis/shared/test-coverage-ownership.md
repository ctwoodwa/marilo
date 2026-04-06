# Test Coverage Ownership -- ResizableContainer

This document makes the ownership model explicit for test evidence across the gap-analysis pipeline.

## Component Test Path

| Item | Path |
|------|------|
| Component source | `src/Marilo.Components/Layout/ResizableContainer/` |
| Unit tests | `tests/Marilo.Tests.Unit/Layout/MariloResizableContainerTests.cs` |
| Test framework | bUnit + xUnit |
| Test runner | `dotnet test` |

## Ownership by Stage

### Stage 03 -- Resolution Design

Owns the **testable success criteria** for each gap resolution.

- Define the "what must be true" -- not the test code itself.
- Each resolution record SHOULD include a `### Success Criteria` subsection listing verifiable conditions that confirm the gap is closed.
- Example conditions: "Component renders without console errors", "Prop X controls behaviour Y", "Snapshot matches approved baseline".
- These criteria are the contract that Stage 05 tests must satisfy and Stage 06 validates against.

### Stage 05 -- Implement

Owns the **actual test files** and the **tests-written mapping**.

- Write the tests that satisfy the Stage 03 success criteria.
- Each implementation log SHOULD include a `### Tests Written` subsection with file:method mappings per gap ID.
- Format: `GAP-RESIZABLE-CONTAINER-NNN --> tests/Marilo.Tests.Unit/Layout/MariloResizableContainerTests.cs :: "test description"`.
- If tests are deferred, the log MUST state the reason and create a follow-up entry.
- Deferred tests do NOT count as closed coverage.

### Stage 06 -- Validate

Owns the **validation evidence** and the authoritative closure state.

- Confirm that the Stage 05 tests pass and cover the resolved behaviour.
- Record pass/fail closure state per gap.
- Define enforcement guardrails (analyzer rules, review checks, template updates) that prevent regression.
- The closure report is the authoritative source for "is this gap actually closed?".
- A gap is not Resolved until Stage 06 confirms it.

### gap-context.md (Config)

Owns **cross-gap rollup visibility only**.

- Tracks totals: resolved count, test-covered count, deferred count.
- Does NOT own detailed per-gap test evidence -- that lives in Stage 06 closure reports.

## Handoff Flow

```
Stage 03                  Stage 05                  Stage 06
Resolution Design    -->  Implement            -->  Validate & Close
-----------------         -----------------         -----------------
Define success            Write tests that           Confirm tests pass
criteria (what            satisfy the criteria.      and gap is closed.
must be true).            Log file:method            Record closure
                          mapping per gap.            state and guardrails.
                          Document deferrals.
```

## Quick Reference

| Ownership question | Answer |
|--------------------|--------|
| Who defines what "done" looks like? | Stage 03 (Success Criteria) |
| Who writes the tests? | Stage 05 (Tests Written section) |
| Who confirms the gap is closed? | Stage 06 (closure report) |
| Who tracks the overall test count? | gap-context.md (rollup only) |
| What happens when tests are deferred? | Stage 05 logs reason; Stage 06 marks gap Partially Resolved or Deferred |
