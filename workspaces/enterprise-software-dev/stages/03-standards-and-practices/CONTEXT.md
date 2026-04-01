# Stage 03 - Standards and Practices

Generate coding standards, 12-factor mapping, KISS rules, testing strategy, and CI/CD expectations.
These outputs become canonical Layer 3 references consumed by all later stages.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Stage 02 output | ../02-architecture-and-patterns/output/[slug]-system-architecture.md | Full file | Architecture to align standards to |
| Stage 02 output | ../02-architecture-and-patterns/output/[slug]-architecture-decision-records/ | Full folder | ADR decisions that constrain standards |
| Config | ../../_config/architecture-principles.md | Full file | Org-level rules to encode |
| Config | ../../_config/quality-gates.md | Full file | Definition of done and SLAs |
| Shared | ../../shared/observability-guide.md | Full file | OTEL rules to bake into standards |
| Shared | ../../shared/security-baseline.md | Full file | Security rules to bake in |

## Process

1. Generate C# and .NET coding standards: naming, file layout, nullability, async patterns, error handling.
2. Generate Blazor-specific standards: component structure, state management, Telerik usage rules.
3. Map all 12 factors to this platform - one entry per factor explaining how Aspire/YARP/DAB implements it.
4. Define KISS rules: max abstraction layers, when NOT to use patterns, simplicity checkpoints.
5. Define testing strategy: unit, integration, contract, and smoke test expectations per service type.
6. Define CI/CD expectations: branch strategy, required pipeline gates, environment promotion rules.
7. Cross-check every standard against the ADRs from Stage 02. Flag any conflict.
8. Audit: every standard must be specific and testable, not aspirational. Revise any that are not.
9. Save outputs with project slug prefix.

## Audit

| Check | Pass Condition |
|---|---|
| No aspirational standards | Every rule has a concrete pass/fail test (e.g. not 'write clean code' but 'methods max 30 lines') |
| No conflict with ADRs | Every standard is consistent with Stage 02 architecture decisions |
| 12-factor covers all 12 factors | One entry exists for each factor, none skipped |
| KISS rules are actionable | Each rule names a specific anti-pattern to avoid, not a general principle |
| Testing strategy covers all service types | Blazor, YARP, DAB, and any background services each have a test expectation |

## Outputs

| Artifact | Location | Format |
|---|---|---|
| C# coding standards | output/[slug]-coding-standards.md | Markdown |
| Blazor and Telerik standards | output/[slug]-blazor-standards.md | Markdown |
| 12-factor mapping | output/[slug]-12-factor-mapping.md | Markdown |
| KISS rules | output/[slug]-kiss-rules.md | Markdown |
| Testing strategy | output/[slug]-testing-strategy.md | Markdown |
| CI/CD expectations | output/[slug]-cicd-expectations.md | Markdown |