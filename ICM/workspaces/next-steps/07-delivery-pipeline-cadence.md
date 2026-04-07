# Step 07 — Delivery Pipeline Cadence

## Context

Once gaps are resolved (Steps 01–06), components must be formally closed through the 3-stage
delivery pipeline. This prompt establishes the cadence for running delivery pipelines on
components as they move from gap-resolved to delivered.

**Components eligible for delivery pipeline after Steps 01–06 complete:**
- TreeView (Step 02 already runs its delivery pipeline directly)
- Forms — after Step 03 resolves all Critical gaps
- MultiSelect / DateRangePicker / ColorPicker / DateTimePicker — after Step 04 resolves High gaps
- Any of the 12 batch-intake components (Step 06) whose gaps are subsequently resolved

**This prompt is a standing cadence template.** Run it once per component as that component
reaches gap-resolved status. It is not a one-time run.

---

## Your Task

You are a Claude agent running the delivery pipeline for a single component.
Replace `[COMPONENT_SLUG]` and `[COMPONENT_NAME]` throughout with the actual values.

Workspace: `/workspaces/Marilo/workspaces/[COMPONENT_SLUG]-delivery/`

Read `CLAUDE.md`, then `CONTEXT.md`, then `_config/delivery-context.md`.

---

### Pre-Flight Check

Before running any stage, verify:

1. The gap analysis workspace for this component has status `COMPLETE` or `RESOLVED` in
   `output/[component-slug]-gap-list.md`. If it is still PENDING or INTAKE, stop and output:
   ```
   DELIVERY BLOCKED: [COMPONENT_NAME] gap analysis is not complete.
   Current gap status: [status from gap list]
   Recommended action: Complete gap resolution first, then re-run this prompt.
   ```

2. `dotnet build` passes for the component's source project.
   If not, stop and output a build failure report.

3. `dotnet test` passes for all tests in `tests/Marilo.Components.Tests/[ComponentName]/`.
   If tests fail, list the failing tests and stop.

---

### Stage 01 — Spec Review

Follow `stages/01-spec-review/CONTEXT.md` exactly.

Delivery criteria:
- All Critical and High spec gaps from the gap list are documented in the spec
- All Type B (stale) entries are removed from the spec
- Parameter types and defaults match source exactly
- Write output to `stages/01-spec-review/output/[component-slug]-spec-gap-list.md`

---

### Stage 02 — Example UX

Follow `stages/02-example-ux/CONTEXT.md` exactly.

Delivery criteria:
- At minimum one demo scenario per Critical/High feature area
- Each scenario has: title, description, working Blazor code snippet, expected output
- No scenario references a removed or renamed parameter
- Write output to `stages/02-example-ux/output/[component-slug]-demo-gap-list.md`

---

### Stage 03 — Sync Check

Follow `stages/03-sync-check/CONTEXT.md` exactly.

Delivery criteria:
- Source API, spec API, and demo scenarios are all consistent
- No Critical or High gaps remain open
- Delivery checklist from `shared/delivery-checklist.md` is fully checked
- Write final report to `stages/03-sync-check/output/[component-slug]-delivery-report.md`

---

### Final Status

Set the final delivery status in `_config/delivery-context.md`:

| Status | When to use |
|--------|-------------|
| `DELIVERED` | All stages complete, all criteria met |
| `DELIVERED_WITH_WARNINGS` | All stages complete, Medium/Low gaps remain (documented) |
| `BLOCKED` | Any Critical/High gap remains open or a pre-flight check failed |

Write the final delivery report:

```markdown
# [ComponentName] Delivery Report
Date: [today]
Final Status: [DELIVERED / DELIVERED_WITH_WARNINGS / BLOCKED]

## Stage Results
| Stage | Status | Gaps found | Gaps resolved |
|-------|--------|------------|---------------|
| 01 Spec Review | | | |
| 02 Example UX | | | |
| 03 Sync Check | | | |

## Warnings (if DELIVERED_WITH_WARNINGS)
[List remaining Medium/Low gaps with estimated resolution timeline]

## Blockers (if BLOCKED)
[List each blocker with a one-line resolution path and responsible owner]

## Recommended Next Component
[Based on gap analysis batch summary, which component should enter delivery pipeline next]
```

---

## Constraints

- Run pre-flight checks before every delivery run. Do not skip even for components that
  previously passed — source may have changed.
- Do not modify source files from the delivery workspace.
- Do not mark a component `DELIVERED` if any Critical or High gap is still open.
- One delivery pipeline run per component per invocation. Do not batch multiple components
  in a single run — workspace state conflicts will occur.
