# Environment Matrix

Filled during Stage 04. Defines per-environment configuration, secrets, and deployment targets.

## Environments

| Environment | Trigger | Approval Required | Deploy Target |
|---|---|---|---|
| `dev` | Push to `{{DEV_BRANCH}}` | No | `{{DEV_DEPLOY_TARGET}}` |
| `test` | Push to `{{TEST_BRANCH}}` | No | `{{TEST_DEPLOY_TARGET}}` |
| `prod` | Tag `{{PROD_TAG_PATTERN}}` | Yes | `{{PROD_DEPLOY_TARGET}}` |

## Per-Environment Config Values

| Config Key | Dev | Test | Prod |
|---|---|---|---|
| `ASPNETCORE_ENVIRONMENT` | `Development` | `Test` | `Production` |
| SQL Server connection | `{{SQL_DEV}}` | `{{SQL_TEST}}` | `{{SQL_PROD}}` |
| Okta IssuerUrl | `{{OKTA_ISSUER_DEV}}` | `{{OKTA_ISSUER_TEST}}` | `{{OKTA_ISSUER_PROD}}` |
| OTEL endpoint | `{{OTEL_DEV}}` | `{{OTEL_TEST}}` | `{{OTEL_PROD}}` |
| UseRedisOutputCache | `false` | `true` | `true` |

## Secrets Required Per Environment

| Secret Name | Used In | Dev Source | Test/Prod Source |
|---|---|---|---|
| `OKTA_CLIENT_SECRET` | Gateway | Local user-secrets | Key Vault |
| `DB_CONNECTION_STRING` | ApiService | Local user-secrets | Key Vault |
| `GATEWAY_ADMIN_SHARED_SECRET` | Gateway, Admin.Web | Local user-secrets | Key Vault |
| `CONTAINER_REGISTRY_TOKEN` | Pipeline | — | CI secret |

## Notes

`{{ENVIRONMENT_MATRIX_NOTES}}`
