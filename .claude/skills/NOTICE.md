# Third-Party Skills

The following skills in `.claude/skills/` are vendored from external projects and retain their original licenses.

## obra/superpowers (MIT)

Source: https://github.com/obra/superpowers
Copyright (c) 2025 Jesse Vincent
License: MIT

Vendored skills:

| Skill | Purpose in Marilo |
|---|---|
| `test-driven-development` | Red-Green-Refactor discipline for workers editing `src/**` + `tests/**` |
| `verification-before-completion` | Gates worker transition to `review-pending` — requires fresh `dotnet build` + `dotnet test` output |
| `systematic-debugging` | Four-phase RCA when a worker hits a failing test or bug |
| `requesting-code-review` | Standardizes the format workers write to `_orchestrator/results/` |
| `receiving-code-review` | Standardizes how workers handle FAIL feedback from `_orchestrator/inbox/` |
| `dispatching-parallel-agents` | Pattern reference for orchestrator fan-out (Wave dispatch) |
| `subagent-driven-development` | Pattern reference for per-task fresh-subagent dispatch |

Marilo-specific adaptations (dotnet commands, workspace paths, sync-area references) are called out in a `> **Marilo note:**` block at the top of each adapted `SKILL.md`. The core content is preserved under the original MIT license.

The orchestration layer in `.claude/rules/orchestration.md` is Marilo-owned and is the authoritative source for worker lifecycle, file ownership, review gates, and escalation. The vendored skills cover worker-turn execution discipline *inside* that layer — they do not replace it.
