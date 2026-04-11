---
description: Orchestration rules — active only when .claude/orchestration/_orchestrator/session.json has status=active
globs: **/*
---

# Orchestration Rules (tmux parallel workers)

These rules define orchestrator-mode and worker-mode behavior for tmux-based parallel Claude Code work on Marilo. They are **additive** to the existing Marilo rules in [CLAUDE.md](../../CLAUDE.md), [openwolf.md](openwolf.md), and [universal-planning.md](universal-planning.md) — they do not replace any of them.

## Scope and Activation

**These rules apply ONLY when** [.claude/orchestration/_orchestrator/session.json](../orchestration/_orchestrator/session.json) has `"status": "active"`.

When `status` is `"inactive"` (the default), Claude Code operates in normal single-session mode. All existing rules apply unchanged. There is no file-ownership enforcement, no review gate, no orchestration memory to update. **This is graceful degradation — the repo works normally with or without orchestration.**

**Detection at session start:**
1. Read `.claude/orchestration/_orchestrator/session.json`.
2. If `status == "inactive"`, skip the rest of this file. Operate normally.
3. If `status == "active"`, determine mode:
   - If `MARILO_ORCHESTRATION_ROLE=orchestrator` env var is set, OR the current tmux session matches `session.orchestrator_tmux_session`, you are the **orchestrator**.
   - If `MARILO_ORCHESTRATION_ROLE=worker` env var is set, OR `MARILO_WORKER_ID` is set, OR the tmux session matches `marilo-w-*`, you are a **worker**.
   - Otherwise, ask the user which role to assume before proceeding.

## Modes

### Orchestrator Mode

An orchestrator coordinates parallel workers. It is the only role allowed to:

- Read and mutate `.claude/orchestration/_orchestrator/session.json`
- Assign scopes to workers via `_orchestrator/inbox/<worker-id>.md`
- Review worker results in `_orchestrator/results/<worker-id>-<ts>.md` and write review records to `_orchestrator/reviews/<worker-id>-<ts>.md`
- Integrate approved worker work into the main branch
- Mutate files listed under `_memory/projects/marilo.json -> orchestrator_only_changes`
- Mutate `CLAUDE.md`, files in `.claude/rules/`, `.wolf/OPENWOLF.md`, or the orchestration layer itself
- Modify `component-mapping.json` schema, public `IMariloProvider`/`IMariloCssProvider` contracts, `MariloComponentBase` public API, ICM/CDW workspace structure

Orchestrator responsibilities each turn:

1. Boot with full repo context (CLAUDE.md, cerebrum.md, relevant component specs).
2. Read `_orchestrator/session.json` and every file in `_memory/workers/*.json`.
3. Process `_orchestrator/inbox/` (escalations from workers) — answer, decide, or reassign.
4. Process `_orchestrator/results/` (completed work awaiting review) — review, approve, or return with notes.
5. Integrate approved work.
6. Reassign idle workers or start new ones.
7. Update `session.json` (`last_heartbeat`, `active_workers`, `review_queue`, `blocked_queue`).
8. Append an entry to `_orchestrator/log.jsonl`.

### Worker Mode

A worker runs one scoped task at a time. It MUST:

- Read its scope from `_orchestrator/inbox/<worker-id>.md` on start
- Read its state from `_memory/workers/<worker-id>.json` on every turn
- Edit ONLY files in `files_owned` (declared in its worker-state JSON)
- Treat `files_read_only` as read-only — no edits, even for typos
- Write results to `_orchestrator/results/<worker-id>-<ts>.md`
- Write handoffs to `_handoffs/<worker-id>-<ts>.md` after every non-trivial turn
- Write escalations as JSON to `_orchestrator/inbox/<orchestrator-id>-escalation-<ts>.json` when blocked
- Set its own `status` field in `_memory/workers/<worker-id>.json` to one of: `idle`, `working`, `blocked`, `review-pending`, `complete`

A worker MUST NOT:

- Edit any file outside its `files_owned` list (hard stop)
- Merge, integrate, or self-close its own work
- Modify `session.json`, other workers' state, or the orchestration rules
- Modify any file listed in `_memory/projects/marilo.json -> orchestrator_only_changes`
- Modify `CLAUDE.md`, `.claude/rules/*`, `.wolf/OPENWOLF.md`
- Start other workers or dispatch subagents without orchestrator approval
- Make architectural decisions — escalate instead

### Normal Mode

No orchestration session is active. Normal Claude Code operation. All existing Marilo rules apply. This file does nothing.

## File Ownership

1. Every active worker declares `files_owned` in its state JSON.
2. Before assigning a worker, the orchestrator scans existing worker states and **rejects any assignment that overlaps** an already-owned file — unless `allow_overlap: true` is set and `overlap_authorized_by` names the orchestrator.
3. If a worker discovers mid-task that it needs to touch a file not in `files_owned`, it MUST escalate (type: `file-ownership-conflict`), not edit.
4. A worker may read any file freely. Ownership is only about writes.

**Enforcement:** When a worker is about to write, it MUST check the target path against its `files_owned` list. No match = hard stop + escalation.

## Review Gate

No worker self-integrates. Integration flow:

1. Worker completes its atomic task.
2. Worker writes a result doc to `_orchestrator/results/<worker-id>-<ts>.md`.
3. Worker sets its state to `review-pending` and writes a handoff.
4. Orchestrator detects the result (next poll or next turn).
5. Orchestrator reads the result, verifies ownership + sync areas + architecture boundaries.
6. Orchestrator writes a review record to `_orchestrator/reviews/<worker-id>-<ts>.md` using `templates/review-record.md`.
7. If **PASS**, orchestrator integrates and sets worker state back to `idle`.
8. If **FAIL**, orchestrator writes feedback to `_orchestrator/inbox/<worker-id>.md` and sets worker state to `working`.
9. If **ESCALATE**, orchestrator asks the user.

**A worker may not skip the review gate, even for trivial changes, while a session is active.** Triviality is the orchestrator's judgment call.

## Sync Enforcement

Marilo's public-behavior changes require **source + spec + demo + docs + tests + gap-plan** to stay in sync (see [CLAUDE.md](../../CLAUDE.md) and [.wolf/cerebrum.md](../../.wolf/cerebrum.md)). In orchestrator mode:

- Each worker declares `required_sync_areas` in its state JSON (e.g. `["source", "spec", "tests"]`).
- The orchestrator review step checks that declared sync areas were actually touched.
- **Source-only work that changes public API = automatic review FAIL**, returned to worker with instructions to pick up the missing sync areas OR to escalate for a new lane.
- If a worker's work requires a sync area outside its scope (e.g. a source change needs a test update but `tests` isn't in `files_owned`), the worker escalates with type `sync-area-violation` rather than skipping.

**The sync rule is not optional in orchestrator mode.** It exists because parallel workers are easy to leave half-synced.

## Architecture-Level Changes

The following changes are **orchestrator-only** (or explicit human escalation):

- Public API of any `IMariloProvider` / `IMariloCssProvider` implementation
- Public API of `MariloComponentBase`
- Provider contract modifications
- ICM workspace structural changes (new stages, renamed stage folders)
- CDW workspace structural changes
- `component-mapping.json` schema changes
- New top-level folders
- Changes to `.wolf/OPENWOLF.md` or any file in `.claude/rules/`
- Changes to `CLAUDE.md`

If a worker needs one of these, it escalates (`type: "architecture-decision"` or `"public-api-change"` or `"provider-contract-change"`). The worker does NOT make the change, even if it seems "obvious" or "small".

## Cross-Worker Coordination

Workers do not talk to each other directly. All coordination goes through the orchestrator:

- If worker A needs output from worker B, A escalates with type `cross-worker-coordination`.
- Orchestrator may hold A in `blocked` state until B produces the output, then hand it to A via `_orchestrator/inbox/A.md`.
- Never read another worker's state JSON speculatively to "save time" — states are only guaranteed consistent from the orchestrator's perspective.

## Escalation

Workers escalate rather than guess when:

- A file they need to edit is not in `files_owned` (ownership conflict)
- The task requires an architecture-level change
- A declared sync area needs a file outside scope
- Required context is missing from the inbox message
- A blocker takes more than one edit-retry cycle to resolve
- Two consecutive review cycles fail with similar feedback (review-retry loop — escalate instead of trying again)

Escalation format: [templates/escalation.json](../orchestration/templates/escalation.json). Write to `_orchestrator/inbox/<orchestrator-id>-escalation-<ts>.json`. Set worker state to `blocked`.

## OpenWolf Integration

- `.wolf/anatomy.md`, `.wolf/memory.md`, `.wolf/cerebrum.md`, `.wolf/buglog.json` remain shared and are updated per [openwolf.md](openwolf.md) rules.
- In orchestrator mode, memory.md entries should include `[worker-id]` or `[orchestrator]` prefixes so the activity log is readable.
- Cerebrum learnings discovered by workers belong to the whole session — the orchestrator (or the worker, with a note) appends them to `.wolf/cerebrum.md` at review time, not at worker commit time.
- Bug log entries tie to worker-id when applicable.

## UPF Integration

When an orchestration session is wrapped in a UPF plan (see [universal-planning.md](universal-planning.md)), each **phase** of the plan maps to an orchestration wave, and each **worker lane** is a slice of the phase that a single worker can own. See the "Parallel Lanes" section in universal-planning.md for details.

## Graceful Degradation

- If `_orchestrator/session.json` is missing, corrupt, or has `status: "inactive"`: ignore this file entirely. Work normally.
- If a worker's state JSON is missing: the worker treats itself as newly created and asks the orchestrator for initial scope via a handoff with status `idle`.
- If `_memory/projects/marilo.json` is missing: orchestrator bootstraps it from the template before assigning any workers.
- No orchestration rule may cause the repo to reject a single-session Claude Code operation. If this file starts causing single-session breakage, **this file is wrong** — fix it, do not work around it.

## Conflict Resolution with Other Rules

- Where this file conflicts with `universal-planning.md`, the UPF rule wins. Orchestration is a UPF execution strategy, not a replacement.
- Where this file conflicts with `CLAUDE.md`'s "Never do" list, CLAUDE.md wins.
- Where this file conflicts with [openwolf.md](openwolf.md), OpenWolf memory rules win but get the orchestrator prefix noted above.
- Worker-mode rules never override orchestrator-mode rules within a single turn — if you are acting as both (you shouldn't), stop and ask the user which role applies.
