# Enterprise Software Dev - Workspace Context

Task routing for the enterprise-software-dev workspace.

## Task Routing

| Task Type                          | Go To Stage                              | Description                                  |
|------------------------------------|------------------------------------------|----------------------------------------------|
| Capture problem and constraints    | `stages/01-discovery-and-context/`       | Domain, stakeholders, env, compliance        |
| Define architecture and decisions  | `stages/02-architecture-and-patterns/`   | Topology, ADRs, service boundaries           |
| Generate coding standards          | `stages/03-standards-and-practices/`     | C# standards, 12-factor, KISS, CI/CD rules   |
| Spec Blazor, YARP, DAB, OTEL       | `stages/04-service-and-app-specs/`       | Per-service specs with contracts             |
| Generate scaffolds and checklists  | `stages/05-scaffolds-and-checklists/`    | Project stubs, config templates, checklists  |

## Reference Materials

| Resource                  | Location                          | Contains                                      |
|---------------------------|-----------------------------------|-----------------------------------------------|
| Org context               | `_config/org-context.md`          | Domain, cloud, environments, compliance       |
| Tech stack defaults       | `_config/tech-stack-defaults.md`  | .NET version, DB, identity, observability     |
| Architecture principles   | `_config/architecture-principles.md` | Preferred patterns, rules                  |
| API visibility rules      | `shared/api-visibility.md`        | Scalar, GraphQL Playground env-gating rules   |
| Observability guide       | `shared/observability-guide.md`   | OTEL, Grafana, Azure Monitor setup rules      |
| Enterprise patterns       | `shared/enterprise-patterns.md`   | Retry, circuit breaker, SAGA, eventing        |
| Security baseline         | `shared/security-baseline.md`     | Auth, secrets, data protection rules          |
