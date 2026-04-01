# Stage 02 - Architecture Impact

Map the requested feature to architecture boundaries and rollout constraints.

## Inputs

| Source                    | File/Location                                                          | Section/Scope                                                        | Why                                                     |
| ------------------------- | ---------------------------------------------------------------------- | -------------------------------------------------------------------- | ------------------------------------------------------- |
| Previous stage output     | ../01-discovery/output/feature-[slug]-discovery.md                     | Full file                                                            | Carry forward discovered impact candidates              |
| Architecture defaults     | ../../../../docs/icm-defaults/030-architecture/*.md                    | Logical, data, integration, security, signal/reactive, observability | Validate component boundaries and non-functional impact |
| Domain defaults           | ../../../../docs/icm-defaults/020-domain/domain-primitives.md          | Full file                                                            | Keep domain language and primitives consistent          |
| Business logic governance | ../../../../docs/icm-defaults/020-domain/business-logic-governance.md  | Full file                                                            | Apply enterprise rule boundaries                        |
| Release channel defaults  | ../../../../docs/icm-defaults/060-release-channels/release-channels.md | Full file                                                            | Plan where and how change should roll out               |

## Process

1. Map the feature to domains, services, and integration boundaries.
2. Decide which APIs, events, signals, and data models change.
3. Identify migration implications and operational risk areas.
4. Propose rollout channel strategy for validation and release.
5. Save outputs.

## Outputs

| Artifact                   | Location                                     | Format   |
| -------------------------- | -------------------------------------------- | -------- |
| Architecture impact report | output/feature-[slug]-architecture-impact.md | Markdown |
