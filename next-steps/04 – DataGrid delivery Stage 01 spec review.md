WORKSPACE
- workspaces/Marilo/workspaces/datagrid-delivery

OBJECTIVE
Run Stage 01 Spec Review for DataGrid, treating it as a complex component whose spec, Example UX, source, and tests must be coordinated.

BOOTSTRAP
1. Read:
   - workspaces/datagrid-delivery/CLAUDE.md
   - workspaces/datagrid-delivery/CONTEXT.md
   - workspaces/datagrid-delivery/config/delivery-context.md
2. Confirm these paths:
   - DataGrid spec docs (docs/component-specs/grid or similar).
   - DataGrid demo page(s) (samples/Marilo.Demo/Pages/Components/*Grid*.razor).
   - DataGrid source (src/Marilo.Components/DataGrid* partials).
   - DataGrid tests (tests/*Grid*).
   - Linked gap workspace (datagrid-gap-analysis or global gap-analysis-resolution).
3. Read enterprise-icm component-delivery-template.md.

STAGE 01 – Spec Review
1. From the spec, identify major feature areas (e.g., data binding, editing, sorting, filtering, grouping, virtualization, selection).
2. For each feature area:
   - list documented parameters/events/behaviours;
   - compare to implementation and tests.
3. Build a DataGrid spec gap list:
   - area → missing or inconsistent API/behaviour,
   - severity (blocking vs important vs nice-to-have).

GAP WORKSPACE INTEGRATION
1. For each code-impacting spec gap:
   - raise gaps in datagrid-gap-analysis (preferred) or gap-analysis-resolution.
   - ensure each gap includes enough context to be actionable (spec section, current implementation, desired behaviour).
2. For spec-only inconsistencies with correct code:
   - log them as documentation gaps in a separate section.

STAGE 01 OUTPUT
1. Write stages01-spec-review/output/datagrid-spec-gaps.md containing:
   - summary table by feature area and severity,
   - mapping from gap slugs to workspace locations,
   - explicit list of “found but deferred” advanced features.

STATUS SNAPSHOT
1. Update/create workspaces/datagrid-delivery/status/workspace-status.md with:
   - Stage 01 status,
   - counts of gaps per severity,
   - pointer to primary gap workspace,
   - recommended next trigger (Stage 02 Example UX vs begin datagrid-gap-analysis).

RULES
- No code or tests implemented here.
- Focus on making the gap set comprehensive enough to guide later work without overexplaining.

FINAL REPORT
Return:
1. DataGrid feature-area gap summary.
2. Where each gap set was recorded.
3. Which feature areas are sufficiently specified vs under-specified.
4. Next recommended DataGrid actions.