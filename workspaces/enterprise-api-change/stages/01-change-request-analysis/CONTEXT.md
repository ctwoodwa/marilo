# Stage 01 - Change Request Analysis

Understand the current API/event state and the desired change before designing contract modifications.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Feature or change request | Existing project ticket system | Ticket, acceptance criteria, linked docs | Establish what change is needed and why |
| Canonical context | ../../references/layer-3-defaults-index.md | Context section | Anchor analysis in enterprise principles |
| Integration architecture | ../../references/layer-3-defaults-index.md | Architecture section (integration) | Understand current integration topology |
| Existing API/event specs | Target project OpenAPI specs, event schemas, or code | Current contract definitions | Baseline for what exists today |

## Process

1. Read the change request and summarize the desired API/event behavior change.
2. Inventory existing API endpoints and event schemas affected by the change.
3. Document current consumers and their usage patterns where known.
4. Identify which contracts are candidates for modification, extension, or deprecation.
5. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Current state and change goal | output/api-[slug]-current-and-goal.md | Markdown |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 1 | Change summary and scope | Confirm interpretation and boundaries |
| 3 | Consumer inventory | Confirm completeness and known dependencies |

## Audit

| Check | Pass Condition |
|---|---|
| Scope clarity | Change goal is stated unambiguously with expected inputs/outputs |
| Baseline completeness | Current API endpoints and/or event schemas are inventoried |
| Consumer awareness | Known consumers are listed or explicitly marked as unknown |
| Output naming | Artifact matches `output/api-[slug]-current-and-goal.md` |
