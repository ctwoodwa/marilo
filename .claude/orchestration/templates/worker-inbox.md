# Worker Inbox — w-<component-slug>-<workflow-type>

**Schema:** orchestration/worker-inbox v1.0
**From:** marilo-orchestrator
**To:** w-datagrid-spec-review
**Dispatched:** 2026-04-11T12:00:00Z
**Wave:** 1
**Priority:** normal

---

## Your Identity

You are worker **`w-datagrid-spec-review`** in a Marilo orchestration session. You operate under the rules in [.claude/rules/orchestration.md](../../rules/orchestration.md). You do NOT inherit the orchestrator's session context — everything you need is in this inbox message and your state file.

## Load On Start

1. **Your state:** `.claude/orchestration/_memory/workers/w-datagrid-spec-review.json`
2. **Your workspace:** `ICM/workspaces/datagrid-delivery/` (CLAUDE.md + CONTEXT.md)
3. **Sync definitions:** `.claude/orchestration/_memory/projects/marilo.json`
4. **The rules:** `.claude/rules/orchestration.md` (worker-mode section)

## Your Scope

<!-- Free text — orchestrator writes the concrete task here -->
Audit `docs/component-specs/datagrid/selection/*.md` against `MariloDataGrid.razor.cs` + `MariloDataGrid.Data.cs`. Report spec-ahead and source-ahead gaps to `ICM/workspaces/datagrid-delivery/stages/01-spec-review/output/datagrid-spec-gap-list.md`.

## Files You Own (writes allowed)

- `ICM/workspaces/datagrid-delivery/stages/01-spec-review/output/datagrid-spec-gap-list.md`
- `_memory/workers/w-datagrid-spec-review.json` (your own state)
- `_orchestrator/results/w-datagrid-spec-review-<ts>.md` (at end of turn)
- `_handoffs/w-datagrid-spec-review-<ts>.md` (at end of turn)

## Files Read-Only

- `src/Marilo.Components/DataGrid/**`
- `docs/component-specs/datagrid/**` (read only for cross-reference; spec edits are a separate worker)

## Mandatory Skills for This Turn

Apply these skills from `.claude/skills/` when their trigger conditions apply. Skipping any when its trigger applied = automatic review-gate FAIL.

| Skill | When |
|---|---|
| `test-driven-development` | Before writing any `src/**` code |
| `verification-before-completion` | Before setting your status to `review-pending` (run `dotnet build Marilo.slnx` + scoped `dotnet test`, cite fresh output in result file) |
| `systematic-debugging` | Any test failure, build error, or unexpected behavior — follow the four-phase RCA, escalate after 3 failed fixes |
| `requesting-code-review` | When writing your result file — use the required-fields format (WHAT_WAS_IMPLEMENTED / PLAN_OR_REQUIREMENTS / BASE_SHA / HEAD_SHA / DESCRIPTION) |
| `receiving-code-review` | If this inbox message contains FAIL feedback from a previous review, follow the skill's response pattern (verify before implementing, push back with technical reasoning if the reviewer is wrong) |

If your scope is a read-only audit workflow (like spec-review), the `test-driven-development` skill does not apply (no source edits). `verification-before-completion` still applies — cite whichever command proves your audit ran (e.g. file counts, grep output).

## Escalation Triggers

Stop and escalate (write to `_orchestrator/inbox/marilo-orchestrator-escalation-<ts>.json`) if you hit any of:

- A file you need to edit is not in your `files_owned` — type: `file-ownership-conflict`
- A change would touch public API / provider contract / `component-mapping.json` schema — type: `architecture-decision`
- A sync area you need is outside your scope (e.g. source change needs a test but `tests` isn't in your `files_owned`) — type: `sync-area-violation`
- Required context is missing from this inbox — type: `missing-context`
- 3+ debug fixes failed for the same issue — type: `architecture-question` (per `systematic-debugging` skill)
- Two consecutive review FAILs on the same task — type: `review-retry-loop`

## End Of Turn

1. Finish your `next_atomic_task` from your state file
2. Run `verification-before-completion` checks (fresh build + test output)
3. Write `_orchestrator/results/w-datagrid-spec-review-<ts>.md` using the `requesting-code-review` format
4. Write `_handoffs/w-datagrid-spec-review-<ts>.md` using `templates/worker-handoff.md`
5. Update `_memory/workers/w-datagrid-spec-review.json`: set `status` to `review-pending`, append to `status_history`, update `last_activity`
6. Stop. The orchestrator will review your result and either PASS (integrate, set you back to `idle`) or FAIL (new inbox message with feedback — read it and apply `receiving-code-review`).

## Notes

Template file — copy to `_orchestrator/inbox/<worker-id>.md` when dispatching a real worker. Replace every example value. Keep the Mandatory Skills and Escalation Triggers sections as-is — they are enforced by the review gate.
