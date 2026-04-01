# Stage 02 - Contract Design

Design new or changed API endpoints and event schemas under interface governance controls.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Previous stage | ../01-change-request-analysis/output/api-[slug]-current-and-goal.md | Full file | Carry forward current state and change goal |
| Interface governance | ../../references/layer-3-defaults-index.md | Interface Governance section (interface-governance) | Framework for contract evolution |
| API versioning | ../../references/layer-3-defaults-index.md | Interface Governance section (api-versioning) | Versioning strategy and rules |
| Event schema versioning | ../../references/layer-3-defaults-index.md | Interface Governance section (event-schema-versioning) | Event evolution rules |
| Deprecation process | ../../references/layer-3-defaults-index.md | Interface Governance section (deprecation-process) | Lifecycle for retired contracts |

## Process

1. Review Stage 01 output and confirm which contracts require changes.
2. Design new or modified endpoints: paths, methods, request/response schemas, headers.
3. Design new or modified event schemas: topic, payload, metadata.
4. Classify each change as breaking or non-breaking per governance rules.
5. Define versioning strategy for each changed contract (major bump, new endpoint, or additive).
6. For deprecated contracts, define deprecation timeline and consumer migration guidance.
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Contract design specification | output/api-[slug]-contract-design.md | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 4 | Breaking vs non-breaking classification | Confirm classification and acceptable impact |
| 6 | Deprecation timeline and migration guidance | Approve consumer communication plan |

## Audit

| Check | Pass Condition |
|---|---|
| Contract completeness | All changed endpoints and events have full schema definitions |
| Breaking change classification | Every change is explicitly classified as breaking or non-breaking |
| Versioning compliance | Versioning follows api-versioning and event-schema-versioning defaults |
| Deprecation compliance | Deprecated contracts include timeline and migration path per deprecation-process |
| Output naming | Artifact matches `output/api-[slug]-contract-design.md` |
