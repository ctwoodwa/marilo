WORKSPACE
- workspaces/Marilo/workspaces/datasheet-delivery

OBJECTIVE
Now that the DataSheet component API spec documentation is complete, run Stage 01 Spec Review in the DataSheet Component Delivery Workspace and create the initial gap set that will drive implementation and tests.

BOOTSTRAP
1. Read:
   - workspaces/datasheet-delivery/CLAUDE.md
   - workspaces/datasheet-delivery/CONTEXT.md
   - workspaces/datasheet-delivery/config/delivery-context.md
2. Confirm these paths from delivery-context:
   - DataSheet spec docs (docs/component-specs/spreadsheet or equivalent).
   - DataSheet demo page(s) (samples/Marilo.Demo/Pages/Components/*Sheet* or Spreadsheet).
   - DataSheet source and tests (src/Marilo.Components/*Sheet* and tests/*Sheet*).
   - Linked gap workspace (datasheet-gap-analysis, if created; otherwise gap-analysis-resolution).
3. Read enterprise-icm:
   - component-delivery-template.md
   - reconstructed-pipeline-guide.md (if DataSheet is partially implemented already).

STAGE 01 – Spec Review
1. Parse the completed DataSheet spec:
   - parameters (names, types, defaults),
   - events,
   - behaviours and constraints.
2. Compare spec to actual implementation:
   - implemented-but-not-documented,
   - documented-but-not-implemented,
   - name/type mismatches,
   - behaviours missing or differing from spec.
3. Produce a DataSheet spec gap list, grouping gaps by:
   - API surface (core configuration, editing, selection, virtualization, formulas, etc.),
   - severity (blocking, important, nice-to-have).

GAP WORKSPACE INTEGRATION
1. For each spec gap that requires code changes:
   - If workspaces/datasheet-gap-analysis exists, raise gap records there following its ICM pattern.
   - Otherwise, raise them in the global gap-analysis-resolution workspace, clearly tagged as DataSheet.
2. For spec-only gaps (documentation inaccuracies where code is already correct):
   - log them in a documentation gap list for DataSheet spec only.

STAGE 01 OUTPUT
1. Create a Stage 01 output file under datasheet-delivery, e.g.:
   - stages01-spec-review/output/datasheet-spec-gaps.md
2. Include:
   - summary table of gap counts by severity and area,
   - pointers to gap records created in the relevant gap workspace,
   - any ambiguous spec areas requiring human decisions.

STATUS SNAPSHOT
1. Update or create a short status file:
   - workspaces/datasheet-delivery/status/workspace-status.md
2. Record:
   - Stage 01 completed,
   - number of DataSheet gaps raised and where,
   - ready-for-next-stage flag.

RULES
- Do NOT implement code or tests in this workspace.
- This is a spec-driven audit and gap creation step.
- Keep the output focused and under control; defer full redesign discussions to gap workspaces.

FINAL REPORT
Return:
1. DataSheet spec gap summary.
2. Where each gap was recorded (which workspace).
3. Any spec ambiguities needing human clarification.
4. Next recommended trigger for DataSheet (e.g., start Stage 02 Example UX audit or begin datasheet-gap-analysis Stage 03).