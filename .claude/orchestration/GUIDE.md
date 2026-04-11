# Marilo tmux Orchestration Guide

**Short operational guide. Read once, keep nearby.** For the authoritative rules, see [.claude/rules/orchestration.md](../rules/orchestration.md).

---

## When to use orchestrator mode vs normal mode

**Normal mode (default):**
- Single feature, single component, <3 files
- Gap closure inside one ICM stage
- Fast iterative work where parallelism would be overkill
- Anything <2 hours of focused work

**Orchestrator mode:**
- Parallel work across **2 or more components** where lanes do not share files
- Batch spec-review / spec-delivery across multiple components at once
- Fan-out gap analysis (one worker per component-workflow pair)
- Long-running cross-component refactors that take >1 day
- Any time you catch yourself wanting to run two Claude Code windows side by side on different components

**Rule of thumb:** If you can name >=2 workers with disjoint `files_owned` lists right now, orchestration probably pays off. If not, use normal mode.

## Worker lane naming

Marilo work splits naturally along **(component × workflow)**. Name workers accordingly:

```
w-<component-slug>-<workflow-type>
```

| component slug | workflow type examples |
|---|---|
| datagrid, gantt, scheduler, allocation-scheduler, chart, editor, treelist, treeview, map, filemanager, wizard, splitter, pivotgrid, dockmanager, datasheet, diagram, resizable-container | `spec-review`, `gap-intake`, `gap-resolve`, `example-ux`, `visual-parity`, `sync-check`, `test-expansion`, `api-expansion` |

Examples:
- `w-datagrid-spec-review`
- `w-gantt-example-ux`
- `w-chart-gap-intake`
- `w-allocation-scheduler-sync-check`

Tmux sessions:
- Orchestrator: `marilo-orchestrator`
- Workers: `marilo-w-<component>-<workflow>` (full prefix)

## Starting an orchestration session

### Option A — Slash command (recommended for most runs)

```
/start-multi-agent-work datagrid,datasheet,gantt
```

This invokes the `start-multi-agent-work` skill (see [.claude/skills/start-multi-agent-work/SKILL.md](../skills/start-multi-agent-work/SKILL.md)), which performs one idempotent orchestrator tick:

1. **Preflight** — validates slugs, reads session state + all worker states, checks `git status`.
2. **Process escalations** — resolves scope/ownership issues autonomously, surfaces architectural decisions to the user.
3. **Review pending results** — fills out `review-record.md` per worker; PASS integrates, FAIL feedbacks the worker with `receiving-code-review` instructions.
4. **Dispatch new work** — one `Agent` subagent per component, **all in parallel in a single assistant message**. Workers boot from their inbox files with no session-context inheritance.
5. **Advance or park** — workers that pass review get queued for the next stage; the wave only closes when all workers in scope pass.
6. **Update session state** — writes `session.json` heartbeat, appends `log.jsonl`, writes a `wave-<N>-tick-<ts>.md` summary to `_orchestrator/reviews/`.
7. **Surface summary inline** — the user sees the wave summary in the assistant response without opening files.

Each tick is idempotent. Invoke manually between waves, or wrap with the `loop` skill for automated ticks:

```
/loop 10m /start-multi-agent-work datagrid,datasheet,gantt
```

Arguments:
- **Required:** comma-separated component slugs (lowercase kebab)
- **Optional:** workflow type (`delivery` | `gap-analysis` | `gap-resolve` | `spec-review` | `example-ux` | `visual-parity` | `sync-check`) — default `delivery`
- **Optional:** wave number override

The skill stops the tick and asks for user input when: a slug is invalid, an escalation needs a human decision (architecture, review-retry-loop), uncommitted changes exist outside any worker's `files_owned`, 3+ escalations pile up in one tick, or all components in scope are `complete`.

### Option B — tmux multi-window (legacy, manual)

Use this when you want true parallel Claude Code windows per worker instead of in-process parallel subagents. The following is a **pattern**, not an enforced script. Adapt to your tmux flavour (iTerm, Windows Terminal, tmux-on-Linux).

### 1. Orchestrator setup

```bash
# In a fresh terminal at the repo root
export MARILO_ORCHESTRATION_ROLE=orchestrator
tmux new-session -s marilo-orchestrator
# inside the tmux session, launch Claude Code
claude
```

Then in Claude Code:
1. "Activate orchestrator mode. Read `.claude/orchestration/_orchestrator/session.json` and initialize a new session."
2. Claude reads the template, sets `status` to `active`, fills in `session_id`, `shared_goal`, `started_at`, `orchestrator_tmux_session`, and appends to `log.jsonl`.
3. Claude proposes worker lanes and asks for confirmation.

### 2. Worker setup (one per lane)

For each confirmed lane, open a new tmux session:

```bash
export MARILO_ORCHESTRATION_ROLE=worker
export MARILO_WORKER_ID=w-datagrid-spec-review
tmux new-session -s marilo-w-datagrid-spec-review
claude
```

Then in Claude Code:
1. "I am worker `w-datagrid-spec-review`. Read my inbox and state."
2. Claude reads `_orchestrator/inbox/w-datagrid-spec-review.md` and `_memory/workers/w-datagrid-spec-review.json`.
3. Worker starts on `next_atomic_task` and operates only within `files_owned`.

### 3. Orchestrator dispatch

The orchestrator populates each inbox with scope, then the workers start. The orchestrator does not need to be active while workers run — each worker is a self-sufficient turn-loop.

## Lane patterns

### Pattern A — Fan-out spec review

Four workers, one per component, all running ICM stage `01-spec-review` on different component specs:

```
w-datagrid-spec-review           → ICM/workspaces/datagrid-delivery/
w-gantt-spec-review              → ICM/workspaces/gantt-delivery/
w-scheduler-spec-review          → ICM/workspaces/scheduler-delivery/
w-allocation-scheduler-spec-review → ICM/workspaces/allocation-scheduler-delivery/
```

Each worker owns its own `stages/01-spec-review/output/` file. Zero overlap. Orchestrator integrates the four gap lists into a consolidated report after PASS reviews.

### Pattern B — Sequential pipeline, one component, multiple stages

One component, multiple workers running different ICM stages in sequence. Workers run *sequentially*, not in parallel, but the orchestrator manages the handoff:

```
w-datagrid-spec-review    (stage 01) → then hands off to
w-datagrid-example-ux     (stage 02) → then hands off to
w-datagrid-visual-parity  (stage 03) → then hands off to
w-datagrid-sync-check     (stage 04)
```

Orchestrator holds each successor in `idle` until the predecessor passes review.

### Pattern C — Mixed fan-out with dependencies

Three components, two stages each. The `spec-review` workers run in parallel; the `example-ux` workers wait for their respective `spec-review` to pass:

```
Wave 1 (parallel):
  w-datagrid-spec-review
  w-gantt-spec-review
  w-chart-spec-review

Wave 2 (parallel, depends on Wave 1):
  w-datagrid-example-ux
  w-gantt-example-ux
  w-chart-example-ux
```

Orchestrator manages wave boundaries as review gates.

### Pattern D — Gap closure batch

Four workers, each closing gaps in a different component:

```
w-gantt-gap-resolve           → ICM/workspaces/gantt-gap-analysis/ stage 05
w-scheduler-gap-resolve       → ICM/workspaces/scheduler-gap-analysis/ stage 05
w-treelist-gap-resolve        → ICM/workspaces/treelist-gap-analysis/ stage 05
w-filemanager-gap-resolve     → ICM/workspaces/filemanager-gap-analysis/ stage 05
```

Each owns its component's `src/Marilo.Components/<Component>/**` source tree AND the corresponding `tests/Marilo.Tests.Unit/<Component>/**` tree AND the component's demo page. Disjoint `files_owned` lists.

## Handling blockers

1. Worker detects blocker (missing context, ownership conflict, architecture decision, sync-area violation, public-API need).
2. Worker writes an escalation JSON to `_orchestrator/inbox/marilo-orchestrator-escalation-<ts>.json` using [templates/escalation.json](templates/escalation.json).
3. Worker sets its state to `blocked`, writes a handoff.
4. Worker stops. No guessing.
5. Orchestrator picks up the escalation, decides, writes a response to `_orchestrator/inbox/<worker-id>.md`.
6. Worker reads new inbox message, unblocks, updates state to `working`.

## Handling reviews

1. Worker completes atomic task, writes result to `_orchestrator/results/<worker-id>-<ts>.md`, sets state to `review-pending`.
2. Orchestrator reviews using [templates/review-record.md](templates/review-record.md).
3. **PASS:** orchestrator integrates, sets worker back to `idle`, assigns next task (or retires worker).
4. **FAIL:** orchestrator writes feedback to the worker's inbox, worker returns to `working`.
5. **Two consecutive FAILs on the same task:** escalate to user. Do not let workers loop.

## Integration

Only the orchestrator integrates. Integration means:

- Merging worker-owned files into the main branch
- Updating shared manifests (e.g. `component-mapping.json`, `GAP_ANALYSIS_RESOLUTION_PLAN.md`) based on worker output
- Running `dotnet build` + `dotnet test` at the full-repo level
- Appending a consolidated entry to `.wolf/memory.md` and (if applicable) `.wolf/cerebrum.md`
- Closing out or reassigning the worker

Workers never push to remote, never run full-repo builds, never touch shared manifests directly.

## ICM and CDW fit

- Orchestration is a **coordination layer on top of** ICM/CDW, not a replacement.
- Each worker runs inside an existing ICM or CDW workspace (`ICM/workspaces/<slug>-*` or `workspaces/Marilo/workspaces/<slug>/`).
- Stage outputs still flow through the existing `stages/NN-*/output/` folders.
- Orchestration adds worker identity, ownership enforcement, review gates, and cross-lane coordination. Nothing more.
- If you are running a single ICM workspace with a single agent, do NOT start an orchestration session — normal mode is simpler and the ICM skill pattern already handles stage handoffs.

## OpenWolf fit

- `.wolf/anatomy.md`, `.wolf/memory.md`, `.wolf/cerebrum.md`, `.wolf/buglog.json` are shared between orchestrator and workers.
- Workers should prefix their memory.md entries with `[<worker-id>]` so the activity log is readable.
- Orchestrator's own entries are prefixed `[orchestrator]`.
- Cerebrum learnings from worker runs are applied at **review time**, not during the worker's turn, so they benefit from orchestrator judgment.

## Worker execution discipline (vendored Superpowers skills)

Workers that edit source/tests MUST apply these skills during their turn. Full rules: [.claude/rules/orchestration.md](../rules/orchestration.md) → "Worker Execution Discipline". Quick reference:

| Skill | Trigger | What it enforces |
|---|---|---|
| `test-driven-development` | Before writing any `src/**` code | Write failing test in `tests/**` first, watch it fail, write minimal code, verify pass |
| `verification-before-completion` | Before setting status to `review-pending` | `dotnet build Marilo.slnx` exit 0 + `dotnet test --filter <Component>` 0 failures, fresh output cited in result file |
| `systematic-debugging` | Any test failure, build error, or unexpected behavior | Four-phase RCA; escalate after 3 failed fixes instead of attempting fix #4 |
| `requesting-code-review` | When writing result file | Required fields: WHAT_WAS_IMPLEMENTED / PLAN_OR_REQUIREMENTS / BASE_SHA / HEAD_SHA / DESCRIPTION |
| `receiving-code-review` | When reading FAIL feedback from orchestrator inbox | Verify before implementing; push back with technical reasoning; no performative agreement |

The orchestrator's review gate checks the "Execution Discipline Check" section in `templates/review-record.md`. Skipping a mandatory skill when its trigger applied = automatic FAIL with reason `execution-discipline-violation`.

Reference-only skills (orchestrator-side patterns, not enforced on workers):

- `subagent-driven-development` — two-stage review pattern (spec-compliance then code-quality)
- `dispatching-parallel-agents` — fan-out prompt structure for Wave dispatch

## Non-goals

- **Not a daemon.** Nothing in this layer runs as a background process. Every update is driven by Claude Code turns in one of the tmux sessions.
- **Not a multi-process broker.** No IPC, no sockets, no shared memory. Coordination is file-based and eventually consistent.
- **Not a scheduler.** The orchestrator does not pre-plan worker schedules — it reacts to inboxes, results, and blockers as they appear.
- **Not an auto-merger.** Workers never self-integrate. Integration is always a human-in-the-loop step (orchestrator acting with user awareness).

## Graceful degradation

- `session.json` with `status: "inactive"` → orchestration rules off → normal Claude Code operation.
- No `MARILO_ORCHESTRATION_ROLE` env var, no active session → ask user if they want to start a session, otherwise run normally.
- Corrupted state files → orchestrator bootstraps from templates/, asks user to confirm before overwriting anything.

## Quick reference

```
START SESSION
  1. Set session.json status=active, populate session_id + shared_goal
  2. Create per-worker state JSONs in _memory/workers/
  3. Create per-worker inbox messages in _orchestrator/inbox/
  4. Launch worker tmux sessions with MARILO_WORKER_ID set

WORKER TURN
  1. Read inbox → read state → pick next_atomic_task
  2. Work within files_owned ONLY
  3. Write result → set state=review-pending → write handoff
  4. Wait for orchestrator review

ORCHESTRATOR TURN
  1. Read inboxes (escalations) → decide → write responses
  2. Read results → write reviews → PASS integrates, FAIL feedbacks
  3. Update session.json heartbeat + queues
  4. Append log.jsonl entry

END SESSION
  1. All workers at state=complete or retired
  2. Orchestrator writes final summary to _orchestrator/log.jsonl
  3. Set session.json status=completed
  4. Archive worker states and handoffs (optional)
```

---

See also:
- [.claude/rules/orchestration.md](../rules/orchestration.md) — authoritative rules
- [.claude/rules/universal-planning.md](../rules/universal-planning.md) — UPF with parallel lanes
- [.claude/rules/openwolf.md](../rules/openwolf.md) — OpenWolf memory protocol
- [CLAUDE.md](../../CLAUDE.md) — project context
