# Stage 04 - Environment Strategy

Define per-environment configuration, secrets injection, Aspire manifest export, and deployment
targets. Fills `_config/environment-matrix.md` with all values needed to complete the CD workflows.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Pipeline config | `../../_config/pipeline-context.md` | Environment names, deploy targets | Scope |
| CD workflow files | `../03-pipeline-scaffold/output/[slug]-cd-*.yml` | Current TODO items | TODOs reveal gaps to fill |
| App configuration files | `{{SOLUTION_PATH}}/**/appsettings.*.json` | All environments | Current config keys |

## Process

1. For each environment (dev, test, prod): list every config key that differs from the base `appsettings.json`.
2. Classify each key: safe to commit (non-secret), or must be a secret (credentials, connection strings, API keys).
3. Define the secrets store per environment: local user-secrets (dev), CI secret variables (test), Key Vault (prod).
4. Document the Aspire manifest export process: `dotnet run --project AppHost -- --publisher manifest --output-path aspire-manifest.json` and how the manifest is used in CD.
5. Fill `_config/environment-matrix.md` with the complete config and secrets table.
6. Resolve every TODO left in the Stage 03 CD workflow files by adding the correct values or references.
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Environment matrix | `_config/environment-matrix.md` | Filled table |
| Secrets registration guide | `output/[slug]-secrets-guide.md` | Steps to register each secret per environment |
| Aspire manifest guide | `output/[slug]-aspire-manifest.md` | Export command, manifest structure, CD usage |
| Updated CD workflows | `../03-pipeline-scaffold/output/[slug]-cd-*.yml` | TODOs resolved |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 3 | Secrets classification table | Confirm every key is correctly classified before secrets guide is written |
| 6 | Resolved CD workflow files | Final review — confirm no TODOs remain |

## Audit

| Check | Pass Condition |
|---|---|
| All config keys documented | Every key in any `appsettings.*.json` has a row in the environment matrix |
| Secrets correctly classified | No credential or connection string is marked as safe-to-commit |
| No open TODOs in CD workflows | Every TODO from Stage 03 is resolved |
| Aspire manifest process documented | Export command and usage are in the guide |
