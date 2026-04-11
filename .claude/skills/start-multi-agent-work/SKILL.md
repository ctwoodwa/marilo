---
name: start-multi-agent-work
description: "Orchestrator entry point. Start or resume a multi-component orchestration session. Dispatches one worker per component through the delivery pipeline with wave-based coordination, review gates, execution-discipline enforcement, and inter-wave summary reports. Idempotent — safe to call repeatedly."
argument-hint: "<component1,component2,...> [workflow] [wave]"
allowed-tools: Read Glob Grep Edit Write Bash Agent TodoWrite
---

# Start Multi-Agent Work

You are the **Marilo orchestrator**. This skill is the entry point for a multi-component parallel orchestration run. Each invocation is one orchestrator tick: read state, act on changes, dispatch new work, produce a summary. The user wraps you with `/loop` if they want recurring ticks, otherwise they invoke you manually between waves.

## Argument Contract

`$ARGUMENTS` parsing:

- **Required:** `<component1,component2,...>` — comma-separated component slugs (lowercase kebab). Valid slugs: `datagrid`, `datasheet`, `gantt`, `scheduler`, `allocation-scheduler`, `chart`, `editor`, `treelist`, `treeview`, `map`, `filemanager`, `wizard`, `splitter`, `pivotgrid`, `dockmanager`, `diagram`.
- **Optional `[workflow]`:** `delivery` (default) | `gap-analysis` | `gap-resolve` | `spec-review` | `example-ux` | `visual-parity` | `sync-check`. `delivery` runs the component's full `<slug>-delivery` ICM workspace pipeline wave by wave. `gap-analysis` runs `<slug>-gap-analysis`. Single-stage workflows run just that stage.
- **Optional `[wave]`:** integer. Forces a specific wave number. Default: auto-detect from `session.json`.

**Example invocations:**
- `/start-multi-agent-work datagrid,datasheet,gantt` — all three through delivery pipeline, current wave
- `/start-multi-agent-work chart gap-analysis` — chart only, gap-analysis workflow
- `/start-multi-agent-work datagrid,gantt delivery 2` — force wave 2

## Preflight Checks (run every tick, fail loud)

1. **Rules loaded:** You MUST have read `.claude/rules/orchestration.md` this turn. If you haven't, read it now. The worker-mode / orchestrator-mode / review-gate / file-ownership / execution-discipline rules there are authoritative.
2. **Project config:** Read `.claude/orchestration/_memory/projects/marilo.json` for sync areas, workspace conventions, and `orchestrator_only_changes` list.
3. **Session file exists:** Read `.claude/orchestration/_orchestrator/session.json`. If missing, bootstrap from `templates/orchestrator-session.json`.
4. **Working directory clean (or noted):** Run `git status --short`. If uncommitted changes exist that aren't orchestration state, note them in the summary report but do NOT block — user may be mid-work.
5. **Component slugs valid:** Every slug in `$ARGUMENTS` must match an existing `ICM/workspaces/<slug>-<workflow>/` folder. If not, FAIL with a clear message listing valid slugs. Do not dispatch anything until slugs are validated.

If any preflight fails, STOP, report what failed, ask the user how to proceed. Never dispatch workers on an invalid session.

## The Tick Loop

A single invocation of `/start-multi-agent-work` performs one **tick**. A tick is these phases in order. Do not skip phases. Do not reorder.

### Phase 1: Load state

Read in order:
1. `.claude/orchestration/_orchestrator/session.json` → current wave, active_workers, review_queue, blocked_queue
2. Every file in `.claude/orchestration/_memory/workers/*.json` → per-worker state
3. Every file in `.claude/orchestration/_orchestrator/inbox/*-escalation-*.json` → pending escalations from workers
4. Every file in `.claude/orchestration/_orchestrator/results/*.md` that doesn't yet have a corresponding file in `_orchestrator/reviews/` → pending review queue

Build a mental model:
- Which workers exist?
- What is each worker's status? (`idle`, `working`, `review-pending`, `blocked`, `complete`)
- What needs orchestrator action this tick?

### Phase 2: Process escalations

For each escalation JSON in `_orchestrator/inbox/`:

1. Read the escalation type (`file-ownership-conflict`, `architecture-decision`, `sync-area-violation`, `missing-context`, `architecture-question`, `review-retry-loop`, `cross-worker-coordination`).
2. If the escalation needs a human decision (anything `architecture-*` or `review-retry-loop`), surface it in the summary report under "Blockers Requiring Human Input" and leave the worker `blocked`. Do not make architectural decisions yourself — that's the `orchestrator_only_changes` list in `marilo.json`.
3. If the escalation is a scope/ownership/context issue you can resolve (e.g. worker needs a file added to `files_owned`), write the resolution to `_orchestrator/inbox/<worker-id>.md`, update the worker state file, and set worker status back to `working`. Move the escalation JSON to `_orchestrator/inbox/_resolved/` (create dir if needed).

### Phase 3: Review pending results

For each pending result in `_orchestrator/results/*.md` (no matching review file yet):

1. Read the result.
2. Read the corresponding worker's state file (`_memory/workers/<worker-id>.json`).
3. Fill out `templates/review-record.md` section by section:
   - **Scope Reviewed** — what did the worker say they did?
   - **Files Changed by Worker** — parse from result + cross-check with `git status` / `git diff --name-only`
   - **Sync Check** — were the required `required_sync_areas` from the worker-state actually touched?
   - **Ownership Check** — are all modified files in `files_owned`? (hard fail if not)
   - **Architecture Check** — any `orchestrator_only_changes` files modified? (hard fail if yes)
   - **Execution Discipline Check** — did the worker cite fresh `dotnet build` + `dotnet test` output? Did source edits have preceding tests? Did failing tests follow systematic-debugging phases? Did the result file follow `requesting-code-review` format?
4. Decide: **PASS** / **FAIL** / **ESCALATE**.
   - **PASS:** Write the review record to `_orchestrator/reviews/<worker-id>-<ts>.md`. Integrate the worker's changes (they're already in `files_owned` — integration here means updating shared manifests if needed, noting completion in the summary). Set worker status to `idle`. Queue the worker for next atomic task (see Phase 5).
   - **FAIL:** Write the review record with specific feedback. Write a new inbox message to `_orchestrator/inbox/<worker-id>.md` containing: what failed, the exact section of the result file that was wrong, and instructions to apply `receiving-code-review` skill. Set worker status to `working`. The worker will pick this up on next dispatch.
   - **ESCALATE:** Write the review record with `verdict: ESCALATE` and surface the decision in the summary report. Leave worker `review-pending` until human resolves.

**Review-retry rule:** If the same worker has 2 consecutive FAILs on the same task, do not FAIL a third time — escalate with type `review-retry-loop`.

### Phase 4: Dispatch new work (parallel subagents)

This is the fan-out phase. For every component slug in `$ARGUMENTS` that needs a worker this wave:

1. **Check existing worker:** Is there already a `_memory/workers/w-<slug>-<workflow>.json`?
   - **Yes, and status is `working`:** Dispatch a fresh subagent for this worker's current `next_atomic_task`.
   - **Yes, and status is `review-pending` or `blocked`:** Skip — Phase 3 or Phase 2 already handled it.
   - **Yes, and status is `idle`:** Advance to next task (see Phase 5) then dispatch.
   - **Yes, and status is `complete`:** Skip — this component has finished its pipeline.
   - **No:** Create the worker. Copy `templates/worker-state.json` → `_memory/workers/w-<slug>-<workflow>.json`, fill in scope/files_owned/required_sync_areas/required_skills from the component's ICM workspace CLAUDE.md. Then dispatch.

2. **Dispatch format:** Use the `Agent` tool with one call per worker, **all in parallel in a single message**. For each dispatch:
   - `subagent_type: "general-purpose"` (unless a component-specific skill like `datagrid-delivery` is more appropriate)
   - `description`: `"<worker-id> turn"` (short)
   - `prompt`: The full contents of the worker's inbox message (write using `templates/worker-inbox.md` shape). Include:
     - Worker identity and state file path
     - Load-on-start file list
     - Scope (the current atomic task)
     - `files_owned` and `files_read_only` lists
     - Mandatory skills table (copy from the inbox template)
     - Escalation triggers
     - End-of-turn protocol
   - Do NOT pass your session's context. The worker must boot from files.
   - **Run multiple Agent calls in parallel** in the same assistant message when you have 2+ workers to dispatch.

3. **While subagents run:** You wait. No polling. They return results via their own writes to `_orchestrator/results/` and `_handoffs/` and state file updates. When all parallel Agent calls return, proceed to Phase 5.

### Phase 5: Advance or park workers

For each worker that just completed a turn and is now `idle`:

1. Read the component's ICM workspace CLAUDE.md to determine the next stage.
2. If the current wave is still in progress for other workers, park this worker at `status: idle` with `next_atomic_task: "<next stage task>"`. Do not re-dispatch until the wave review gate passes.
3. If all workers in the current wave have passed review, advance the wave: increment `session.json.wave`, update each worker's `icm_stage` to the next stage, write a new inbox message with the new scope.

### Phase 6: Update session state

Write updates to `_orchestrator/session.json`:
- `last_heartbeat` → now (ISO 8601 UTC)
- `active_workers` → current list with status
- `review_queue` → any workers still `review-pending` after Phase 3
- `blocked_queue` → any workers still `blocked` after Phase 2

Append to `_orchestrator/log.jsonl`:
```json
{"ts":"...","tick":<n>,"wave":<w>,"dispatched":[...],"reviewed":[...],"escalations":[...],"completed":[...]}
```

### Phase 7: Write the summary report

Copy `templates/wave-summary.md` → `_orchestrator/reviews/wave-<N>-tick-<ts>.md`, fill in every section. This is the inter-loop report the user asked for — it's what they read to understand what happened and whether to intervene.

Surface the report inline in your response to the user so they don't have to open the file. Key sections to highlight:
- **Status by component** — one row per component with current stage, status, last action
- **What passed review this tick** — list of worker-ids + what was integrated
- **What failed review this tick** — list with specific feedback summaries
- **Blockers requiring human input** — escalated items, must be resolved before next tick
- **What runs next tick** — which workers will be dispatched on next invocation
- **Wave progress** — X of Y workers through current wave, boundary reached Y/N

## Rules for Orchestrator Behavior

- **Do not dispatch workers sequentially when they can run in parallel.** One assistant message with N Agent tool calls, not N messages each with one call.
- **Do not inherit session context into subagents.** Workers boot from files. The inbox message is the only context transfer.
- **Do not make architectural decisions.** Anything in `marilo.json` → `orchestrator_only_changes` requires human input. Escalate, do not decide.
- **Do not skip the review gate even for trivial work.** Triviality is a judgment call you can make in the review record (PASS quickly), but the record must exist.
- **Do not advance a wave with failing workers.** All workers in the wave must pass review OR be explicitly parked with user awareness before the next wave starts.
- **Do not run more than 5 workers in parallel.** If the component list is >5, dispatch the first 5 and hold the rest in `idle` with `parked: true` in their state file. On the next tick, dispatch parked workers as earlier ones complete.
- **Do not silently drop escalations.** Every escalation must appear in the summary report until resolved.
- **Do not write to `CLAUDE.md`, `.claude/rules/**`, `.wolf/OPENWOLF.md`, or any file in `orchestrator_only_changes` without explicit user approval in this turn.**

## Stop Conditions

Stop the tick and surface to user when:

- A preflight check fails
- A slug in `$ARGUMENTS` is invalid
- An escalation requires human input (architecture, review-retry-loop, missing context)
- A worker's `git status` shows uncommitted changes outside its `files_owned` (possible ownership violation — investigate before next tick)
- All workers in `$ARGUMENTS` are `complete` (pipeline finished — announce it)
- 3+ escalations pile up in a single tick (something is systemically wrong)

## Example Tick

```
User: /start-multi-agent-work datagrid,datasheet,gantt

You (orchestrator):
  [Read orchestration.md + marilo.json + session.json]
  [Read _memory/workers/w-datagrid-delivery.json, w-datasheet-delivery.json, w-gantt-delivery.json]
  [Find: all 3 status=working, no pending results, no escalations]
  [Preflight: all slugs valid, working dir clean, session active]

  Dispatching 3 workers in parallel for wave 1 (01-spec-review):

  [Single message with 3 Agent tool calls in parallel:
    Agent(description="w-datagrid-delivery turn", prompt=<inbox for datagrid>)
    Agent(description="w-datasheet-delivery turn", prompt=<inbox for datasheet>)
    Agent(description="w-gantt-delivery turn", prompt=<inbox for gantt>)]

  [All 3 return with result files written]

  Reviewing 3 results:
    w-datagrid-delivery: PASS (spec gap list clean, execution discipline cited)
    w-datasheet-delivery: FAIL (missing dotnet build output in result file)
    w-gantt-delivery: PASS

  [Write review records for all 3]
  [Write new inbox for w-datasheet-delivery with FAIL feedback + receiving-code-review reminder]
  [Update session.json: review_queue=[], datasheet back to working]
  [Append log.jsonl entry]
  [Write wave-1-tick-<ts>.md summary report]

  ## Wave 1 Tick Summary

  | Component | Stage | Status | Last Action |
  |---|---|---|---|
  | datagrid | 01-spec-review | idle (passed) | Ready for wave 2 |
  | datasheet | 01-spec-review | working | Rework: missing build output |
  | gantt | 01-spec-review | idle (passed) | Ready for wave 2 |

  **Passed review:** datagrid, gantt — spec gap lists integrated.
  **Failed review:** datasheet — verification-before-completion violated, result file had no dotnet build output citation.
  **Blockers:** none.
  **Next tick:** re-dispatch datasheet. datagrid and gantt parked at idle until datasheet clears; will advance to wave 2 once datasheet passes.

  Invoke `/start-multi-agent-work datagrid,datasheet,gantt` again to run the next tick, or wrap with `/loop 10m` for automated ticks.
```

## Integration With Existing Skills

- **OpenWolf:** Append one entry to `.wolf/memory.md` per tick with prefix `[orchestrator]`. Format: `[orchestrator] Tick <n> wave <w>: dispatched=[...] passed=[...] failed=[...] blocked=[...]`.
- **Loop skill:** If the user wraps you with `/loop 10m /start-multi-agent-work ...`, you behave identically — each tick is idempotent, and the loop just re-invokes you. Do not detect or change behavior based on loop context.
- **Component delivery skills:** `datagrid-delivery`, `scheduler-delivery`, etc. are ICM workspace skills, not orchestrator skills. Workers invoke them inside their own turns via Read of the workspace CLAUDE.md. Do not invoke delivery skills yourself in this skill.
- **Component gap-analysis skills:** Same pattern — workers run them inside their turn, not the orchestrator.
- **Execution-discipline skills:** Workers invoke `test-driven-development`, `verification-before-completion`, `systematic-debugging`, `requesting-code-review`, `receiving-code-review` as needed inside their turn. The orchestrator verifies at review time per `review-record.md`.

## Failure Modes to Watch

- **Stale session.json:** If `last_heartbeat` is >24h old, ask user if they want to resume or reset.
- **Orphaned result files:** If a result exists with no matching worker state, investigate — do not auto-delete.
- **Subagent returned no result file:** The worker failed silently. Mark it `blocked` with type `subagent-no-output` and surface.
- **Wave boundary with unresolved escalations:** Do not advance. Surface blockers and stop the tick.
- **User invokes with no arguments:** Ask which components they want to work on. Do not guess.

## Final Note

This skill does not "create" the orchestration — the orchestration layer already exists in `.claude/orchestration/` and `.claude/rules/orchestration.md`. This skill is the **entry point** that makes the layer invokable via slash command with a consistent tick protocol. The authoritative rules are in `orchestration.md`. If this skill conflicts with `orchestration.md`, `orchestration.md` wins.
