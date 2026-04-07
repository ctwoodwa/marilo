# Security Baseline

## Authentication

- All services authenticate via {{IDENTITY_PROVIDER}}.
- Tokens are validated at the YARP gateway. Upstream services trust the forwarded identity.
- Blazor frontend uses interactive server or WASM auth flow per the architecture spec.

## Authorization

- Use policy-based authorization. No magic strings in controllers or components.
- Define all policies in a central AuthorizationPolicies class per service.
- DAB permissions are environment-specific: dev allows broader access, prod is least-privilege.

## Secrets Management

- No secrets in source code, appsettings.json, or environment variables in plain text.
- Use {{PRIMARY_CLOUD}} key vault in test and prod.
- Use .NET user-secrets for local dev.
- Aspire injects secrets via the resource model - do not bypass this.

## API Surface

- All public endpoints require authentication unless explicitly marked anonymous.
- Scalar and GraphQL Playground UIs in prod require admin-role claim. See shared/api-visibility.md.
- CORS is configured at the YARP gateway only. Services do not configure CORS independently.

## Data Protection

- Sensitive data at rest uses {{PRIMARY_CLOUD}} encryption.
- PII fields are identified in the DAB spec and must not appear in logs or traces.