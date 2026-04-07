# API Visibility and Tooling

## Rule

All REST APIs expose OpenAPI and a Scalar UI. All GraphQL endpoints expose a playground.
Exposure is environment-gated and routed through the YARP gateway. Services do not expose
these UIs directly - all traffic goes through the gateway.

## Environment Policy

| Environment | Scalar UI | GraphQL Playground | Auth Required | Network | Notes |
|---|---|---|---|---|---|
| dev | Enabled | Enabled | None | Local only | Wide open for DX |
| test | Enabled | Enabled | SSO (read-only role) | Internal network | QA and integration verification |
| prod | Optional | Optional | Admin role claim + MFA | IP allowlist + VPN | Explicit approval required per project |

## Gateway Routing

All UI routes are registered at the YARP gateway, not on individual services.

| Route | Upstream | Notes |
|---|---|---|
| /scalar/{service} | {service}/scalar | Gateway strips auth header before forwarding in dev only |
| /graphql/{service}/ui | {service}/graphql/ui | Playground UI route |
| /graphql/{service} | {service}/graphql | GraphQL query endpoint |
| /health | all services | Aggregated health check, always enabled, no auth |

## Production Admin Requirements

Before enabling Scalar or GraphQL Playground in prod:

- Explicit approval recorded as an ADR in Stage 02.
- Admin role claim verified at gateway before forwarding.
- MFA enforced by the identity provider for the admin role.
- IP allowlist or VPN requirement documented in the deployment runbook.
- Introspection disabled on the GraphQL endpoint (playground only shows schema if explicitly enabled).
- Rate limiting enabled on all UI routes in prod.

## Purpose in Production

API UIs in production are admin smoke-test tools only, not primary monitoring.
Use the observability stack (Grafana or Azure Monitor) for ongoing health validation.
Scalar and GraphQL Playground answer: is this specific endpoint alive and returning expected data?