# Stage 01 - Audit Current

Understand how the target project works today and produce a structured current-state architecture summary.

## Inputs

| Source                  | File/Location                                   | Section/Scope        | Why                                           |
| ----------------------- | ----------------------------------------------- | -------------------- | --------------------------------------------- |
| Target project metadata | Manual input (repo path, branch, stack, owners) | Full metadata set    | Defines scan scope and context                |
| Enterprise context      | ../../references/layer-3-defaults-index.md      | Context section      | Anchors the definition of enterprise standard |
| Architecture defaults   | ../../references/layer-3-defaults-index.md      | Architecture section | Guides what areas must be inspected           |

## Process

1. Capture target repo metadata: path, project name, primary runtime, deployment model, and ownership.
2. Inspect current project architecture and delivery patterns across: auth, data access, integration, error handling, logging, deployment, and testing.
3. Summarize what is present, missing, or unclear in each area.
4. Save outputs.

## Outputs

| Artifact                     | Location                              | Format   |
| ---------------------------- | ------------------------------------- | -------- |
| Current architecture summary | output/[slug]-current-architecture.md | Markdown |

## Checkpoints

| After Step | Agent Presents                      | Human Decides                               |
| ---------- | ----------------------------------- | ------------------------------------------- |
| 1          | Target repo metadata table          | Confirm scope and slug                      |
| 3          | Draft current-state summary by area | Approve completeness or request deeper scan |

## Audit

| Check            | Pass Condition                                                                         |
| ---------------- | -------------------------------------------------------------------------------------- |
| Coverage         | Summary includes auth, data, integration, error handling, logging, deployment, testing |
| Evidence quality | Findings reference concrete code/docs paths when available                             |
| Output naming    | Artifact matches `output/[slug]-current-architecture.md`                               |
