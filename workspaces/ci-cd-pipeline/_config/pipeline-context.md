# Pipeline Context

Filled during `setup`. Replace all `{{PLACEHOLDERS}}` before running stages.

## Identity

| Field | Value |
|---|---|
| Project name | `{{PROJECT_NAME}}` |
| Project slug | `{{PROJECT_SLUG}}` |
| Solution path | `{{SOLUTION_PATH}}` |
| Default branch | `{{DEFAULT_BRANCH}}` |

## Platform

| Field | Value |
|---|---|
| CI/CD platform | `{{CICD_PLATFORM}}` |
| Repository host | `{{REPO_HOST}}` |
| Repository URL | `{{REPO_URL}}` |
| Container registry | `{{CONTAINER_REGISTRY}}` |

> Supported platforms: `github-actions`, `azure-pipelines`.

## Build Requirements

| Field | Value |
|---|---|
| .NET version | `{{DOTNET_VERSION}}` |
| Node version | `{{NODE_VERSION}}` |
| Has npm build step | `{{HAS_NPM_BUILD}}` |
| Has Docker targets | `{{HAS_DOCKER}}` |
| Has Aspire AppHost | `{{HAS_ASPIRE}}` |

## Environments

| Field | Value |
|---|---|
| Environment names | `{{ENVIRONMENT_NAMES}}` |
| Deploy on push to | `{{DEPLOY_BRANCH_MAP}}` |

> Example deploy branch map: `dev → develop, test → main, prod → tags/v*`

## Notifications

| Field | Value |
|---|---|
| Failure notification target | `{{NOTIFICATION_TARGET}}` |

## Notes

`{{PIPELINE_NOTES}}`
