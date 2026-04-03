# Step 06 — Batch Gap Analysis for Scaffolded Components

## Context

12 components have gap-analysis workspaces created but no gap list output yet.
They are at intake stage — scaffolded but never run.
This prompt performs the intake pass for all 12 in a coordinated multi-agent session.

**The 12 scaffolded-only components:**
1. `chart-gap-analysis` — MariloChart
2. `datagrid-gap-analysis` — MariloDataGrid
3. `scheduler-gap-analysis` — MariloScheduler
4. `gantt-gap-analysis` — MariloGantt
5. `editor-gap-analysis` — MariloEditor (rich text)
6. `spreadsheet-gap-analysis` — MariloSpreadsheet
7. `diagram-gap-analysis` — MariloDiagram
8. `filemanager-gap-analysis` — MariloFileManager
9. `pivotgrid-gap-analysis` — MariloPivotGrid
10. `pdfviewer-gap-analysis` — MariloPDFViewer
11. `dockmanager-gap-analysis` — MariloDockManager
12. `map-gap-analysis` — MariloMap

Each gap-analysis workspace follows the same ICM structure with a `CLAUDE.md`, `CONTEXT.md`,
`_config/`, `stages/`, and `shared/` folder.

---

## Your Task

You are a Claude orchestrator agent. You will spawn 12 sub-agents, one per component.
Each sub-agent runs independently in its own workspace.

### Orchestrator Steps

1. Read `/workspaces/Marilo/workspaces/chart-gap-analysis/CLAUDE.md` as a template to understand the gap-analysis workspace contract. Do not re-read all 12 — they share the same structure.

2. For each of the 12 components, spawn a sub-agent with the instruction template below.

3. After all 12 sub-agents complete, collect their output summaries and write
   `output/batch-intake-summary.md` in the `enterprise-icm` workspaces root.

---

### Sub-Agent Instruction Template

```
You are performing the gap analysis intake for: [COMPONENT_NAME]
Workspace: /workspaces/Marilo/workspaces/[workspace-slug]-gap-analysis/

STEPS:

1. Read CLAUDE.md and CONTEXT.md to understand your scope.
2. Read _config/gap-analysis-context.md for the component source path.
3. Read ALL source files for [COMPONENT_NAME] at the path specified in the config.
4. Read the spec at docs/component-specs/[spec-slug]/ (all markdown files).

5. Produce a gap inventory by comparing spec vs. source:
   - Type A: In source, not in spec (undocumented)
   - Type B: In spec, not in source (stale/removed)
   - Type C: Documented incorrectly (wrong type, default, or description)
   For each gap: assign priority (critical/high/medium/low) and estimated fix effort (hours).

6. Write the gap list to output/[component-slug]-gap-list.md using the format in shared/gap-record-format.md.

7. Write a one-page intake summary to output/[component-slug]-intake-summary.md:
   ```
   # [ComponentName] Gap Analysis — Intake Summary

   | Metric | Value |
   |--------|-------|
   | Total gaps | |
   | Critical | |
   | High | |
   | Medium | |
   | Low | |
   | Estimated total fix effort | [hours] |
   | Most critical gap | [gap slug and one-line description] |
   | Recommended first action | [one sentence] |
   ```

8. Update _config/gap-analysis-context.md: set intake status to COMPLETE and today's date.

CONSTRAINTS:
- Do not implement any fixes. Intake and gap documentation only.
- Do not modify source files.
- If the spec directory is empty, note it in the summary and list all source parameters as Type A gaps.
- If the source directory does not exist, write a BLOCKED status file and stop.
```

---

### Orchestrator Output

After all 12 sub-agents complete, write `output/batch-intake-summary.md`:

```markdown
# Batch Gap Analysis — Intake Summary
Date: [today]

| Component | Total Gaps | Critical | High | Est. Hours | Intake Status |
|-----------|-----------|----------|------|------------|---------------|
| MariloChart | | | | | |
| MariloDataGrid | | | | | |
| MariloScheduler | | | | | |
| MariloGantt | | | | | |
| MariloEditor | | | | | |
| MariloSpreadsheet | | | | | |
| MariloDiagram | | | | | |
| MariloFileManager | | | | | |
| MariloPivotGrid | | | | | |
| MariloPDFViewer | | | | | |
| MariloDockManager | | | | | |
| MariloMap | | | | | |
| **TOTAL** | | | | | |

## Priority Recommendation
[Rank the components by (critical + high gaps) × complexity. Recommend which 3 to tackle next.]

## Blockers
[Any components where the sub-agent could not complete intake, with reason.]
```

---

## Constraints

- Sub-agents operate in parallel. Each must write only to its own workspace `output/` folder.
- No sub-agent should read or write to another component's workspace.
- This is an intake pass only — no fixing, no spec authoring beyond what the gap format requires.
- If a sub-agent encounters a BLOCKED state, the orchestrator notes it in the summary and continues with the remaining 11.
