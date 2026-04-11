# Wave <N> Tick Summary — <ts>

**Schema:** orchestration/wave-summary v1.0
**Session:** <session_id>
**Tick:** <n>
**Wave:** <w> (stage: <stage-slug>)
**Orchestrator:** marilo-orchestrator
**Components in scope:** <comma-separated slugs>

---

## Status by Component

| Component | Worker ID | ICM Stage | Status | Last Action | Next |
|---|---|---|---|---|---|
| datagrid | w-datagrid-delivery | 01-spec-review | idle (passed) | integrated 6-gap list | advance to 02-example-ux when wave closes |
| datasheet | w-datasheet-delivery | 01-spec-review | working | rework after FAIL | re-dispatch next tick |
| gantt | w-gantt-delivery | 01-spec-review | idle (passed) | integrated 4-gap list | advance to 02-example-ux when wave closes |

## Passed Review This Tick

- **w-datagrid-delivery** — Spec-review complete. 6 gaps logged to `ICM/workspaces/datagrid-delivery/stages/01-spec-review/output/datagrid-spec-gap-list.md`. Execution discipline: TDD N/A (read-only), verification cited grep + file counts, result file in required-fields format. Integrated.
- **w-gantt-delivery** — Spec-review complete. 4 gaps logged. Same discipline evidence. Integrated.

## Failed Review This Tick

- **w-datasheet-delivery** — FAIL reason: `execution-discipline-violation: verification-before-completion`. Result file did not cite fresh `dotnet build Marilo.slnx` output before setting status to `review-pending`. Feedback written to `_orchestrator/inbox/w-datasheet-delivery.md` with instructions to re-run the build, cite exit code, and re-submit. Worker set to `working`. Apply `receiving-code-review` on next turn.

## Escalations

- (none this tick)

## Blockers Requiring Human Input

- (none this tick — if present, list each with worker-id, escalation type, summary, and what decision is needed)

## Dispatched This Tick

- w-datagrid-delivery → stage 01-spec-review audit of selection/*.md
- w-datasheet-delivery → stage 01-spec-review audit of editing/*.md
- w-gantt-delivery → stage 01-spec-review audit of dependencies/*.md

## Not Dispatched This Tick

- (workers parked because the wave has not closed, or because they're mid-rework, or because they're `complete`)

## Wave Progress

- Workers total: 3
- Passed wave: 2 (datagrid, gantt)
- Still in wave: 1 (datasheet — rework in progress)
- **Wave boundary:** NOT REACHED — datasheet must pass before wave 1 closes
- When boundary reached: advance all 3 to wave 2 (02-example-ux) and dispatch next inbox messages

## Files Integrated This Tick

| File | Sync area | Worker | Change |
|---|---|---|---|
| `ICM/workspaces/datagrid-delivery/stages/01-spec-review/output/datagrid-spec-gap-list.md` | spec | w-datagrid-delivery | appended 6 gap records |
| `ICM/workspaces/gantt-delivery/stages/01-spec-review/output/gantt-spec-gap-list.md` | spec | w-gantt-delivery | appended 4 gap records |

## What Runs Next Tick

1. **Priority:** re-dispatch w-datasheet-delivery with the FAIL feedback inbox
2. **Hold:** w-datagrid-delivery and w-gantt-delivery parked at `idle` until wave 1 closes
3. **Advance:** once datasheet passes, advance all 3 to wave 2 in the same tick

## Orchestrator Notes

- Tick duration: <seconds>
- Git status clean: yes
- Session heartbeat updated
- log.jsonl entry appended
- .wolf/memory.md entry appended with `[orchestrator]` prefix

## How to Continue

```
/start-multi-agent-work datagrid,datasheet,gantt
```

Or wrap with the loop skill for automated ticks:

```
/loop 10m /start-multi-agent-work datagrid,datasheet,gantt
```

## Notes

Template file — the orchestrator writes one of these per tick to `_orchestrator/reviews/wave-<N>-tick-<ts>.md` and surfaces the key sections inline in the user-facing response. Every section above is mandatory even if empty (write "(none this tick)"). Empty sections are themselves a signal — e.g. "Failed Review This Tick" being empty means a clean tick.
