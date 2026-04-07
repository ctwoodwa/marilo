# Stage 03 - Design Spec

Produce a contract-safe feature design aligned with governance defaults.

## Inputs

| Source                    | File/Location                                                                                                                                  | Section/Scope | Why                                               |
| ------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------- | ------------- | ------------------------------------------------- |
| Previous stage output     | ../02-architecture-impact/output/feature-[slug]-architecture-impact.md                                                                         | Full file     | Carry architecture decisions into detailed design |
| Interface governance      | ../../../../docs/icm-defaults/040-governance/interface-governance.md                                                                           | Full file     | Enforce contract safety and interface discipline  |
| API versioning            | ../../../../docs/icm-defaults/040-governance/api-versioning.md                                                                                 | Full file     | Decide API versioning strategy                    |
| Event schema versioning   | ../../../../docs/icm-defaults/040-governance/event-schema-versioning.md                                                                        | Full file     | Decide event evolution path                       |
| Contract quality controls | ../../../../docs/icm-defaults/040-governance/contract-testing.md and ../../../../docs/icm-defaults/040-governance/service-contract-registry.md | Full files    | Define contract tests and registry updates        |

## Process

1. Define endpoint, payload, and event schema changes.
2. Identify backward compatibility expectations and version decisions.
3. Specify contract test updates and registry updates.
4. Record performance, security, and observability checks required by design.
5. Save outputs.

## Outputs

| Artifact             | Location                             | Format   |
| -------------------- | ------------------------------------ | -------- |
| Design specification | output/feature-[slug]-design-spec.md | Markdown |
