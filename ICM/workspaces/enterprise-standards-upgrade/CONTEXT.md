# Enterprise Standards Upgrade - Workspace Context

Task routing for brownfield standards adoption in an existing repository.

## Task Routing

| Task Type                                          | Go To Stage                    | Description                                    |
| -------------------------------------------------- | ------------------------------ | ---------------------------------------------- |
| Assess current architecture and delivery practices | `stages/01-audit-current/`     | Capture how the project works today            |
| Identify standards gaps and risks                  | `stages/02-gap-analysis/`      | Compare current state to canonical defaults    |
| Decide standards adoption approach                 | `stages/03-evolution-design/`  | Define adopt/scope/approach decisions          |
| Produce phased migration plan                      | `stages/04-migration-plan/`    | Create executable backlog and rollout sequence |
| Track execution and adoption status                | `stages/05-rollout-checklist/` | Keep a living checklist with evidence links    |

## Reference Materials

| Resource                   | Location                               | Contains                                        |
| -------------------------- | -------------------------------------- | ----------------------------------------------- |
| Canonical Layer-3 defaults | `references/layer-3-defaults-index.md` | Read-only links to `docs/icm-defaults` guidance |
| Stage outputs              | `stages/*/output/`                     | Project-specific analysis and plans             |
| Workspace-level summaries  | `output/`                              | Optional consolidated artifacts across projects |

## Rules

1. Treat every document linked from `references/layer-3-defaults-index.md` as canonical and read-only.
2. Run stages in order from 01 to 05 and carry outputs forward as listed in each stage input table.
3. Use `[slug]-*.md` output names, where `slug` is a stable project identifier.
4. Keep findings evidence-based by citing concrete repository paths or docs where possible.
5. Brownfield scope only: do not generate code scaffolds or propose rewrites without phased migration options.
