# Step 03 — Forms Gap Resolution (Critical Priority)

## Context

Forms has 60 total gaps at intake stage with no implementation started.
19 are classified Critical priority. These block real-world form usage and must be resolved first.
This prompt targets only the 19 Critical gaps using the `forms-gap-analysis` workspace.

**Critical gap categories (inferred from gap analysis intake):**
- Validation: `EditForm` integration, `DataAnnotationsValidator`, `ValidationMessage` binding
- Submission: `OnValidSubmit` / `OnInvalidSubmit` event handling
- Field state: `FieldChanged`, `FieldIdentifier` propagation
- Error display: Inline field error rendering, summary error list
- Accessibility: `aria-invalid`, `aria-describedby` on error state

---

## Your Task

You are a Claude agent working in the `forms-gap-analysis` ICM workspace at:
`/workspaces/Marilo/workspaces/forms-gap-analysis/`

Read `CLAUDE.md` and `CONTEXT.md` first.

### Phase 1 — Triage (Do not skip)

1. Read `output/forms-gap-list.md` (the intake gap list).
2. Extract all 19 gaps tagged `priority: critical`.
3. For each critical gap, produce a one-line resolution plan:
   - `FIX`: Can be resolved by editing existing source
   - `DESIGN`: Requires a design decision before implementation (flag for human)
   - `SPEC`: Spec is missing or wrong; fix spec first before source
4. Write the triage table to `output/forms-critical-triage.md`.

**CHECKPOINT — Human Approval Required**

After writing `forms-critical-triage.md`, output:

```
FORMS CRITICAL GAPS — TRIAGE CHECKPOINT

  FIX gaps ready to implement:    [count]
  DESIGN gaps needing decision:   [count — list gap slugs]
  SPEC gaps needing spec first:   [count — list gap slugs]

Design decisions needed:
  [gap slug]: [one-line description of the decision]

Proceed with FIX gaps now? (yes / review design gaps first)
```

Wait for approval before Phase 2.

---

### Phase 2 — Implement FIX gaps (one per sub-agent)

For each gap tagged `FIX` in the triage output, spawn a sub-agent with this instruction pattern:

```
You are resolving Forms gap: [gap-slug]
Workspace: /workspaces/Marilo/workspaces/forms-gap-analysis/
Gap definition: [paste gap record from forms-gap-list.md]

Steps:
1. Read the relevant source file(s) for this gap.
2. Implement the fix. Do not change unrelated code.
3. Write or update bUnit tests in tests/Marilo.Components.Tests/Forms/ for this gap.
4. Run `dotnet build` and confirm it passes.
5. Update the gap record in output/forms-gap-list.md: set status to RESOLVED and add a one-line fix summary.
6. Write your fix summary to output/resolved/[gap-slug]-resolution.md.
```

Run sub-agents sequentially (not in parallel) to avoid merge conflicts on shared files.

---

### Phase 3 — Report

After all FIX gaps are resolved, write `output/forms-critical-resolution-report.md`:

```markdown
# Forms Critical Gaps — Resolution Report

## Summary
- Critical gaps targeted: 19
- FIX gaps resolved: [count]
- DESIGN gaps pending: [count] — [slugs]
- SPEC gaps pending: [count] — [slugs]

## Resolved Gaps
| Slug | Fix summary | Tests added |
|------|-------------|-------------|

## Pending Human Decisions
| Slug | Decision needed |
|------|-----------------|

## Next Recommended Action
[For each DESIGN gap: one sentence on what decision is needed and who should make it]
```

---

## Constraints

- Implement only Critical priority gaps in this run. High/Medium/Low gaps are out of scope.
- Do not rename any public parameters without a DESIGN decision approval.
- Do not modify `EditContext` or cascade value patterns without a DESIGN decision approval.
- Each sub-agent operates on one gap slug only. Never let a sub-agent touch files owned by a different gap.
- If `dotnet build` fails after a fix, revert the change and flag the gap as BLOCKED in the output.
