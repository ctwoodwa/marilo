# Step 02 — Run TreeView Delivery Pipeline

## Context

TreeView source code is solid (87%+ complete, 2 bugs now targeted in Step 01).
However the delivery pipeline in `treeview-delivery` has never been run — all three stages are PENDING.
This prompt executes all three stages in sequence.

**Prerequisite:** Step 01 (fix open bugs) should be complete before this prompt is run,
but this prompt can begin Stage 01 (Spec Review) in parallel since it reads source, not bug status.

---

## Your Task

You are a Claude agent working in the `treeview-delivery` ICM workspace at:
`/workspaces/Marilo/workspaces/treeview-delivery/`

Read `CLAUDE.md` first, then `CONTEXT.md`, then `_config/delivery-context.md`.

Run all three stages using the `deliver` keyword flow. Specific instructions per stage follow.

---

### Stage 01 — Spec Review (`stages/01-spec-review/`)

1. Read `CONTEXT.md` in this stage.
2. Read the current TreeView spec at `docs/component-specs/treeview/` (all markdown files).
3. Read the TreeView source at `src/Marilo.Components/TreeView/` to extract the full current parameter, event, and method surface.
4. Compare spec vs. implementation:
   - Parameters in source but not in spec → Type A gap
   - Parameters in spec but removed from source → Type B gap (stale)
   - Parameters with incorrect types or defaults documented → Type C gap
5. Write the gap list to `stages/01-spec-review/output/treeview-spec-gap-list.md` using `shared/spec-coverage-format.md`.
6. For every Type A gap that is Critical or High priority, also write the spec addition directly into the appropriate spec file under `docs/component-specs/treeview/`.
7. Update `_config/delivery-context.md`: set Stage 01 status to COMPLETE.

---

### Stage 02 — Example UX (`stages/02-example-ux/`)

1. Read `CONTEXT.md` in this stage.
2. Read the TreeView demo page at the path specified in `_config/delivery-context.md`.
3. Audit all demo scenarios against `stages/02-example-ux/shared/demo-scenario-format.md`:
   - Verify each scenario has: title, description, working code, and expected output
   - Identify missing scenarios for key TreeView features: lazy load, checkboxes, drag-and-drop, ReadOnly mode, ExpandAll, virtualization
4. Write missing scenarios directly to the demo page file.
5. Write a demo gap list to `stages/02-example-ux/output/treeview-demo-gap-list.md`.
6. Update `_config/delivery-context.md`: set Stage 02 status to COMPLETE.

---

### Stage 03 — Sync Check (`stages/03-sync-check/`)

1. Read `CONTEXT.md` in this stage.
2. Read all three stage outputs (spec gap list, demo gap list, source) and run through `shared/delivery-checklist.md`.
3. Confirm:
   - All Critical and High spec gaps are resolved
   - All key demo scenarios are present
   - Component API in source matches documented API
   - No breaking parameter renames since last spec version
4. Write the delivery report to `stages/03-sync-check/output/treeview-delivery-report.md`.
5. Set final delivery status to one of: `DELIVERED`, `DELIVERED_WITH_WARNINGS`, or `BLOCKED`.
6. Update `_config/delivery-context.md` with final status and date.

---

## Constraints

- Run stages in order. Do not start Stage 02 until Stage 01 output is written.
- Do not modify TreeView source files from this workspace. Source changes belong in `treeview-gap-analysis`.
- If Stage 03 results in BLOCKED status, list each blocker with a one-line resolution path.
- Write one file at a time. Do not batch outputs.
