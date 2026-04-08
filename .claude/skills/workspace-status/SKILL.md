---
name: workspace-status
description: "ICM overview: Show pipeline status across all workspaces. Scans every workspace's stages/*/output/ folders and renders a dashboard of completion state by category."
argument-hint: "[category|workspace-name]"
allowed-tools: Read Glob Grep Bash
---

# Workspace Status Dashboard — ICM Overview Skill

Scan all ICM workspaces and report pipeline completion status.

## How to Check Status

For each workspace under `ICM/workspaces/`, scan its `stages/*/output/` folders (or `output/stage-*/` for gap analysis stubs). A stage is **COMPLETE** if its output folder contains files other than `.gitkeep`. Otherwise it is **PENDING**.

## Procedure

1. If `$ARGUMENTS` names a specific workspace, run status for that workspace only
2. If `$ARGUMENTS` names a category, run status for all workspaces in that category
3. If no argument, run a summary across all workspaces

## Categories

| Category | Workspaces |
|----------|-----------|
| **Component Delivery** | chart-delivery, datagrid-delivery, datasheet-delivery, diagram-delivery, dockmanager-delivery, editor-delivery, filemanager-delivery, gantt-delivery, map-delivery, pivotgrid-delivery, scheduler-delivery, splitter-delivery, treelist-delivery, treeview-delivery, wizard-delivery |
| **Gap Analysis** | gap-analysis-resolution, chart-gap-analysis, datagrid-gap-analysis, datasheet-gap-analysis, diagram-gap-analysis, dockmanager-gap-analysis, editor-gap-analysis, filemanager-gap-analysis, gantt-gap-analysis, map-gap-analysis, pivotgrid-gap-analysis, scheduler-gap-analysis, splitter-gap-analysis, treelist-gap-analysis, wizard-gap-analysis |
| **Migration** | asp-classic-to-blazor, dotnet-framework-to-blazor, jquery-to-blazor, data-layer-migration |
| **Enterprise** | enterprise-software-dev, enterprise-api-change, enterprise-feature-change, enterprise-test-coverage, enterprise-observability-enhancement, enterprise-quality-control, enterprise-standards-upgrade |
| **Infrastructure** | ci-cd-pipeline, workspace-builder, docs-generator, component-builder, test-coverage-expansion |

## Output Format

Render each workspace as a pipeline diagram:

```
workspace-name
  [Stage 01]  -->  [Stage 02]  -->  [Stage 03]
   COMPLETE         PENDING          PENDING
```

For the summary view, show a compact table:

```
| Workspace | Stages | Completed | Next Stage |
|-----------|--------|-----------|------------|
```

## Routing Reference

For workspace taxonomy and graduation criteria, read `ICM/workspaces/shared/workspace-routing.md`.
