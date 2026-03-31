# CI/CD Pipeline Workspace

Design and scaffold a complete CI/CD pipeline for a multi-project .NET solution. Covers build
ordering, test execution, security scanning, artifact publishing, and multi-environment deployment.

## Folder Map

```
ci-cd-pipeline/
├── CLAUDE.md                          (you are here)
├── CONTEXT.md                         (workspace routing and rules)
├── _config/
│   ├── pipeline-context.md            (project paths, platform choice, environment matrix)
│   ├── build-steps.md                 (ordered build requirements filled during Stage 01)
│   └── environment-matrix.md          (per-environment config, secrets, deployment targets)
├── stages/
│   ├── 01-build-audit/                (document all build steps and dependencies)
│   ├── 02-pipeline-design/            (platform, job structure, environment strategy)
│   ├── 03-pipeline-scaffold/          (generate YAML pipeline definitions)
│   ├── 04-environment-strategy/       (deployment targets, secrets injection, Aspire manifests)
│   └── 05-release-checklist/          (pre-release gates, rollback plan, runbook)
├── shared/
│   ├── pipeline-patterns.md           (reusable job and step patterns)
│   ├── github-actions-reference.md    (GitHub Actions syntax quick reference)
│   └── azure-pipelines-reference.md   (Azure Pipelines syntax quick reference)
├── setup/
│   └── questionnaire.md               (onboarding questions)
└── output/                            (workspace-level artifacts)
```

## Task Routing

| Task Type | Go To |
|---|---|
| First-time setup | `setup/questionnaire.md` |
| Audit build requirements | `stages/01-build-audit/CONTEXT.md` |
| Design pipeline structure | `stages/02-pipeline-design/CONTEXT.md` |
| Generate pipeline YAML | `stages/03-pipeline-scaffold/CONTEXT.md` |
| Define environment deployments | `stages/04-environment-strategy/CONTEXT.md` |
| Build release gates and runbook | `stages/05-release-checklist/CONTEXT.md` |

## Triggers

| Keyword | Action |
|---|---|
| `setup` | Read `setup/questionnaire.md` and populate `_config/` |
| `status` | Scan `stages/*/output/` and show pipeline completion |
| `scaffold` | Jump to Stage 03; requires Stages 01–02 outputs or filled `_config/` |

## Global Constraints

- Generated pipeline YAML must be immediately runnable — no placeholder commands that require manual editing.
- All secrets are referenced by name (e.g., `${{ secrets.OKTA_CLIENT_SECRET }}`); values are never written to files.
- Build ordering must respect the npm → MSBuild dependency for projects with SCSS compilation.
- Every pipeline must include a test job that gates the deploy job.
- Environment-specific config overrides are documented in `_config/environment-matrix.md`.
