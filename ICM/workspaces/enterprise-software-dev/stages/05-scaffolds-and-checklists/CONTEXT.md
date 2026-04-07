# Stage 05 - Scaffolds and Checklists

Generate reusable project scaffolds, config templates, operational checklists, and a
copilot-instructions.md that primes GitHub Copilot with the full project context at work.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Stage 04 output | ../04-service-and-app-specs/output/blazor-ui-spec.md | Full file | UI scaffold requirements |
| Stage 04 output | ../04-service-and-app-specs/output/gateway-yarp-spec.md | Full file | YARP config scaffold requirements |
| Stage 04 output | ../04-service-and-app-specs/output/dab-api-spec.md | Full file | DAB config scaffold requirements |
| Stage 04 output | ../04-service-and-app-specs/output/scalar-spec.md | Full file | Scalar registration requirements |
| Stage 04 output | ../04-service-and-app-specs/output/otel-spec.md | Full file | OTEL config scaffold requirements |
| Stage 03 output | ../03-standards-and-practices/output/coding-standards.md | Full file | Standards scaffolds must follow |
| Stage 03 output | ../03-standards-and-practices/output/blazor-standards.md | Full file | Blazor and Telerik rules for Copilot |
| Stage 03 output | ../03-standards-and-practices/output/12-factor-mapping.md | Full file | 12-factor rules for Copilot context |
| Stage 02 output | ../02-architecture-and-patterns/output/system-architecture.md | Full file | Architecture summary for Copilot |
| Config | ../../_config/tech-stack-defaults.md | Full file | Stack versions and choices |

## Process

1. Generate Aspire solution structure as annotated text tree showing all projects and references.
2. Generate Aspire AppHost registration stub with service wiring comments.
3. Generate YARP appsettings.json route template with inline comments per route.
4. Generate dab-config.json template with entity placeholders and permission comments.
5. Generate appsettings.{Environment}.json templates for dev, test, and prod with OTEL and API UI toggles.
6. Generate Scalar and GraphQL Playground registration code stubs for Program.cs.
7. Generate new-service checklist, new-feature checklist, and production-readiness checklist.
8. Generate copilot-instructions.md by synthesizing: solution purpose, tech stack, architecture style,
   coding standards (naming, async, error handling, nullability), Blazor and Telerik rules,
   12-factor constraints, OTEL requirements, API visibility rules, and security baseline.
   This file goes into the project repo at .github/copilot-instructions.md and primes
   GitHub Copilot in VS Code and Rider with full project context for every suggestion.
9. Verify all scaffolds follow coding standards from Stage 03. Revise any that do not.
10. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Aspire solution structure | output/solution-structure.md | Markdown |
| AppHost stub | output/scaffold-templates/AppHost-stub.cs | C# |
| YARP config template | output/scaffold-templates/yarp-appsettings.json | JSON |
| DAB config template | output/scaffold-templates/dab-config.json | JSON |
| appsettings dev template | output/scaffold-templates/appsettings.Development.json | JSON |
| appsettings test template | output/scaffold-templates/appsettings.Test.json | JSON |
| appsettings prod template | output/scaffold-templates/appsettings.Production.json | JSON |
| Program.cs stubs | output/scaffold-templates/Program-stubs.cs | C# |
| Checklists | output/checklists.md | Markdown |
| Copilot instructions | output/copilot-instructions.md | Markdown |

## Copilot Instructions Structure

The generated copilot-instructions.md must follow this structure so Copilot loads it correctly:

1. One-paragraph project summary: what the solution does, who uses it, what it must NOT do.
2. Tech stack list: exact versions, key libraries, what each layer is responsible for.
3. Architecture rules: layer boundaries, what can call what, no-cross rules.
4. Coding standards: naming, file layout, async patterns, error handling, nullability (concise bullets).
5. Blazor and Telerik rules: component conventions, state management, what Telerik components to prefer.
6. API surface rules: REST via DAB, GraphQL via DAB, Scalar and Playground env policy.
7. Observability rules: always use ILogger structured logging, always include TraceId in log scope.
8. Security rules: no secrets in code, policy-based auth only, no CORS on services (gateway only).
9. What to never do: list of explicit anti-patterns for this project.
10. Checklist reminder: tell Copilot to remind the developer to run the new-feature checklist.