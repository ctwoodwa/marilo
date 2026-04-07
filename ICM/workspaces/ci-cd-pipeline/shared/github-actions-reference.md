# GitHub Actions Quick Reference

## File Location

`.github/workflows/[workflow-name].yml`

## Triggers

```yaml
on:
  push:
    branches: [main, develop]
    tags: ['v*']
  pull_request:
    branches: [main]
  workflow_dispatch:          # manual trigger
```

## Top-Level Structure

```yaml
name: CI

on: [push]

env:
  DOTNET_NOLOGO: true
  DOTNET_SKIP_FIRST_TIME_EXPERIENCE: true

jobs:
  job-name:
    runs-on: ubuntu-latest    # or windows-latest
    environment: production   # links to GitHub environment (enables protection rules)

    steps:
      - uses: actions/checkout@v4
      - name: Step name
        run: echo "hello"
```

## Common Actions

| Action | Purpose |
|---|---|
| `actions/checkout@v4` | Check out the repo |
| `actions/setup-dotnet@v4` | Install .NET SDK |
| `actions/setup-node@v4` | Install Node.js |
| `actions/upload-artifact@v4` | Upload build artifacts |
| `actions/download-artifact@v4` | Download artifacts between jobs |
| `azure/login@v2` | Authenticate to Azure |
| `azure/container-apps-deploy-action@v1` | Deploy to Azure Container Apps |
| `docker/login-action@v3` | Log in to container registry |

## Expressions and Context

```yaml
# Conditional step
- name: Deploy only on main
  if: github.ref == 'refs/heads/main'

# Tag-based trigger
- name: Deploy release
  if: startsWith(github.ref, 'refs/tags/v')

# Environment variable from secret
env:
  MY_SECRET: ${{ secrets.MY_SECRET }}

# Variable (non-secret)
env:
  MY_VAR: ${{ vars.MY_VAR }}

# Output from a previous step
- id: get-version
  run: echo "version=1.0.0" >> $GITHUB_OUTPUT
- run: echo "Version is ${{ steps.get-version.outputs.version }}"
```

## Matrix Strategy

```yaml
strategy:
  matrix:
    os: [windows-latest, ubuntu-latest]
    dotnet: ['8.0', '9.0']
runs-on: ${{ matrix.os }}
```

## Caching

```yaml
- uses: actions/cache@v4
  with:
    path: ~/.nuget/packages
    key: ${{ runner.os }}-nuget-${{ hashFiles('**/Directory.Packages.props') }}
    restore-keys: |
      ${{ runner.os }}-nuget-
```

## Concurrency (prevent overlapping deploys)

```yaml
concurrency:
  group: deploy-${{ github.ref }}
  cancel-in-progress: false  # do not cancel in-flight deploys
```
