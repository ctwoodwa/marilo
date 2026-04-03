WORKSPACE
- workspaces/Marilo/workspaces/gap-analysis-resolution

OBJECTIVE
Perform initial gap analysis intake for the remaining 12 complex components that currently have scaffolded-only workspaces but no outputs:

- Chart, Diagram, DockManager, Editor, FileManager, Gantt, Map, PivotGrid, Scheduler, Splitter, TreeList, Wizard

(DataGrid and DataSheet are intentionally excluded here; they now have their own delivery flows.)

BOOTSTRAP
1. Read:
   - GAPANALYSISRESOLUTIONPLAN.md (section listing scaffolded workspaces).
   - Any existing workspace folders for the 12 components (even if empty).
   - Relevant spec docs and demo pages for each component category.
2. Read the enterprise-icm reconstructed-pipeline-guide if any of these components already have code before gaps are defined.

TASKS
1. For each of the 12 components:
   - determine maturity: code exists? demos exist? tests exist?
   - decide if the analysis should be standard or reconstructed mode.
2. For each component, create Stage 01 intake outputs:
   - component inventory (what surfaces exist),
   - initial gap list (even if coarse),
   - rough severity tagging (critical/important/nice-to-have).
3. Record gaps in:
   - the global gap-analysis-resolution plan,
   - or in a component-specific gap workspace if one exists and is intended to be used.

PLAN UPDATES
1. Update GAPANALYSISRESOLUTIONPLAN.md:
   - mark each of the 12 as “Intake completed; ready to prioritize.”
   - record approximate gap counts per component.

RULES
- Keep this pass shallow but complete; detailed design comes later.
- Respect ICM stage boundaries and “Do NOT Load” guidance while running each intake.

FINAL REPORT
Return:
1. A table of the 12 components with gap counts and severity breakdown.
2. Any components that clearly merit a dedicated component-delivery workspace in the future.
3. Recommended order in which to tackle these components once TreeView/DataGrid/DataSheet work stabilizes.