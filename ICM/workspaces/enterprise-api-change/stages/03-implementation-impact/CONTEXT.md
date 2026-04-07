# Stage 03 - Implementation Impact

Assess which services, tests, and registry entries are affected by the contract changes.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Previous stage | ../02-contract-design/output/api-[slug]-contract-design.md | Full file | Contract changes to assess impact for |
| Contract testing | ../../references/layer-3-defaults-index.md | Interface Governance section (contract-testing) | Required test changes |
| Service contract registry | ../../references/layer-3-defaults-index.md | Interface Governance section (service-contract-registry) | Registry updates needed |
| Breaking change detection | ../../references/layer-3-defaults-index.md | Interface Governance section (breaking-change-detection) | Detection tooling requirements |
| Architecture defaults | ../../references/layer-3-defaults-index.md | Architecture section | Service boundaries and data flow context |

## Process

1. Identify all services that produce or consume the affected contracts.
2. Map required code changes per service (controllers, handlers, DTOs, mappers).
3. Define contract test additions or modifications per contract-testing defaults.
4. Identify service-contract-registry entries that must be created or updated.
5. Define breaking-change detection checks to add to CI pipeline.
6. Assess data migration or backward-compatibility shim requirements.
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Implementation impact report | output/api-[slug]-implementation-impact.md | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 2 | Per-service change inventory | Confirm scope completeness |
| 5 | CI pipeline additions | Confirm tooling and automation approach |

## Audit

| Check | Pass Condition |
|---|---|
| Service coverage | All producing and consuming services are listed with specific changes |
| Test plan | Contract test additions are defined per contract-testing defaults |
| Registry updates | Service-contract-registry entries are identified |
| Detection tooling | Breaking-change detection additions are specified |
| Output naming | Artifact matches `output/api-[slug]-implementation-impact.md` |
