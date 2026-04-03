WORKSPACE
- workspaces/Marilo/workspaces/treeview-delivery

OBJECTIVE
Run the full three-stage TreeView Component Delivery Workspace to:
- Audit spec vs implementation.
- Audit Example UX (demo) coverage vs spec.
- Produce a sync-check delivery report that can be reused as a pattern for other components.

BOOTSTRAP
1. Read:
   - workspaces/treeview-delivery/CLAUDE.md
   - workspaces/treeview-delivery/CONTEXT.md
   - workspaces/treeview-delivery/config/delivery-context.md
2. From delivery-context, locate:
   - TreeView spec docs path (docs/component-specs/treeview or equivalent).
   - TreeView demo page(s) (samples/Marilo.Demo/Pages/Components/TreeView*.razor).
   - TreeView source and tests (Navigation/TreeView and TreeViewTests).
   - Linked gap workspace (gap-analysis-resolution).
3. Read enterprise-icm guides for:
   - component-delivery-template.md
   - summary-snapshot-template.md (for status)
   - any demo-scenario-format or delivery-checklist templates, if present.

STAGE 01 – Spec Review
1. Compare TreeView spec docs with actual component implementation:
   - Parameters: documented vs implemented.
   - Events: documented vs implemented.
   - Behaviour notes: any mismatches.
2. Produce a spec gap list:
   - documented-but-not-implemented,
   - implemented-but-not-documented,
   - mismatched types or names.
3. For each spec gap affecting code:
   - raise or update a gap entry in gap-analysis-resolution (do not implement code here).
4. Write a Stage 01 output file (e.g., stages01-spec-review/output/treeview-spec-gaps.md).

STAGE 02 – Example UX (Demo) Audit
1. Define or use an existing Demo Scenario Format:
   - scenario title, live example, code snippet, active parameters, linked spec section.
2. Audit the TreeView demo page(s) against the spec:
   - Which parameters are never emphasized in any scenario.
   - Which events/edge cases (disabled, readonly, empty, error, lazy load, virtualization) lack demos.
3. Propose or implement additional demo scenarios in Blazor:
   - Add minimally-invasive new sections to the TreeView demo page.
   - Keep each scenario focused and traceable back to spec.
4. Write a Stage 02 output file (e.g., stages02-example-ux/output/treeview-demo-gaps.md) describing what was added and what remains.

STAGE 03 – Sync Check & Delivery Report
1. Combine evidence from:
   - Stage 01 spec gaps.
   - Stage 02 demo gaps.
   - gap-analysis-resolution closure reports for TreeView.
2. Fill a Delivery Checklist for TreeView:
   - spec aligned with implementation,
   - Example UX covering all key parameters/events,
   - tests covering documented behaviour.
3. Write a delivery report (e.g., stages03-sync-check/output/treeview-delivery-report.md) that:
   - summarizes status,
   - calls out remaining misalignments,
   - states “ready for release” or “pending gaps” with pointers.

STATUS SNAPSHOT
1. If not present, create workspaces/treeview-delivery/status/workspace-status.md using summary-snapshot-template as a model.
2. Capture:
   - stage completion status (01–03),
   - key open issues,
   - next trigger.

FINAL REPORT
Return:
1. TreeView spec gap summary.
2. Demo coverage summary (what was added).
3. Delivery readiness assessment.
4. Any reusable patterns you recommend for DataGrid/DataSheet delivery workspaces.