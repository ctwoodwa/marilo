# CI/CD Pipeline — Workspace Context

Task routing and rules for designing and scaffolding a complete CI/CD pipeline.

## Task Routing

| Task Type | Go To Stage | Description |
|---|---|---|
| Document all build steps and dependencies | `stages/01-build-audit/` | Foundation — nothing else can be accurate without this |
| Choose platform and design job structure | `stages/02-pipeline-design/` | Platform, triggers, job graph, environment matrix |
| Generate pipeline YAML definitions | `stages/03-pipeline-scaffold/` | Runnable pipeline files for the chosen platform |
| Define deployment targets and secrets | `stages/04-environment-strategy/` | Per-env config, Aspire manifest export, secret injection |
| Build release gates and rollback runbook | `stages/05-release-checklist/` | Pre-release checklist, rollback procedure, on-call guide |

## Reference Materials

| Resource | Location | Contains |
|---|---|---|
| Pipeline config | `_config/pipeline-context.md` | Project paths, platform, repo, environments |
| Build step inventory | `_config/build-steps.md` | Filled in Stage 01 — ordered build requirements |
| Environment matrix | `_config/environment-matrix.md` | Per-env config and secrets list |
| Reusable step patterns | `shared/pipeline-patterns.md` | Job templates for .NET, npm, Docker, Aspire |
| GitHub Actions reference | `shared/github-actions-reference.md` | Syntax quick reference |
| Azure Pipelines reference | `shared/azure-pipelines-reference.md` | Syntax quick reference |
| Stage outputs | `stages/*/output/` | Artifacts produced at each stage |

## Rules

1. Run stages 01 → 05 in order. The build audit must be complete before generating YAML.
2. Generated YAML must be immediately runnable — flag any step that requires manual configuration as a TODO with a specific instruction.
3. Secrets are always referenced by variable name. Never write secret values into pipeline files.
4. The test job must be a required dependency of every deploy job.
5. npm → MSBuild ordering must be preserved for any project with a `CompileScss` or `build:styles` step.
6. Use `[slug]-*.md` and `[slug]-*.yml` naming for all stage output artifacts.
