# Enterprise Feature Change - Workspace Context

Task routing for adding a feature to an existing .NET enterprise project.

## Task Routing

| Task Type                                   | Go To Stage                    | Description                                            |
| ------------------------------------------- | ------------------------------ | ------------------------------------------------------ |
| Understand current behavior and constraints | stages/01-discovery/           | Capture current state, assumptions, and impacted areas |
| Decide architecture impact                  | stages/02-architecture-impact/ | Map feature to services, data, contracts, and channels |
| Produce design and contract spec            | stages/03-design-spec/         | Define APIs, events, versions, and contract tests      |
| Build implementation and rollout plan       | stages/04-implementation-plan/ | Define owners, tasks, migration, rollout, and rollback |
| Prepare review and PR support               | stages/05-review-and-pr/       | Produce PR draft and required validation checklist     |

## Reference Materials

| Resource                   | Location                             | Contains                                                |
| -------------------------- | ------------------------------------ | ------------------------------------------------------- |
| Canonical Layer-3 defaults | references/layer-3-defaults-index.md | Read-only links to docs/icm-defaults guidance           |
| Stage outputs              | stages/*/output/                     | Discovery, impact, design, plan, and PR draft artifacts |

## Rules

1. Treat all documents linked from references/layer-3-defaults-index.md as canonical and read-only.
2. Run stages in order from 01 to 05 and carry outputs forward as listed in each stage input table.
3. Use feature-[slug]-*.md output names where slug is a stable feature identifier from your tracking artifact.