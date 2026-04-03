WORKSPACE
- workspaces/Marilo/workspaces/gap-analysis-resolution

OBJECTIVE
Fix the two open TreeView issues called out in the executive report:

1) ExpandAllAsync lazy-load behaviour — silent data loss when lazy subtrees are skipped.
2) ReadOnly drag-and-drop guard — OnItemDrop firing when ReadOnly is true.

These must be implemented and fully validated so TreeView can be treated as the reference component for delivery.

BOOTSTRAP
1. Read:
   - CLAUDE.md
   - CONTEXT.md
   - config/gap-context.md
   - src/Marilo.Components/GAPANALYSISRESOLUTIONPLAN.md (TreeView section, including any Phase 2.5 entries)
   - Navigation/resolution/IMPLEMENTATIONNOTES.md (TreeView sections)
   - Navigation/resolution/RESOLUTIONSTATUS.md
   - Navigation/resolution/TESTPLAN.md
2. Read existing Stage outputs for the relevant gaps:
   - stages01-intake/output/gap-*-inventory.md
   - stages03-resolution-design/output/gap-*-resolutions.md
   - stages05-implement/output/gap-*-implementation-log.md
   - stages06-validate/output/gap-*-closure-report.md (if present)
3. Read TreeView source and tests:
   - src/Marilo.Components/Navigation/MariloTreeView.razor
   - src/Marilo.Components/Navigation/MariloTreeView.razor.cs
   - src/Marilo.Components/Navigation/MariloTreeItem.razor(.cs)
   - tests/*TreeViewTests*.cs

ROUTING
1. Confirm the current stage for:
   - GAP-expandall-lazyload
   - GAP-readonly-guards (or equivalent readonly/drag-drop gap)
2. If a gap has no Stage 03 resolution record, you must start at Stage 03.
3. If Stage 03 exists but Stage 05 does not, start at Stage 05.
4. If Stage 05 exists but Stage 06 does not, run Stage 06 validation only.

PHASE A – ExpandAllAsync lazy-load fix
1. If Stage 03 is missing, create it per the pre-approved Option C pattern:
   - Task ExpandAllAsync(bool includeUnloaded = false, int maxDepth = int.MaxValue, CancellationToken cancellationToken = default)
   - includeUnloaded = false preserves current behaviour but documents it explicitly.
   - includeUnloaded = true performs depth-first traversal, calls LoadChildrenAsync for unloaded nodes, respects maxDepth and cancellation.
   - Document all three options A/B/C and mark C as selected.
   - Write checkbox-form Success Criteria that cover default path, opt-in path, maxDepth, cancellation, and any optional progress callback.
2. Implement Stage 05:
   - Update ExpandAllAsync signature and implementation in MariloTreeView.razor.cs.
   - Implement both code paths correctly.
   - Add bUnit tests for each Success Criterion.
   - No Telerik dependencies. Every line must trace to a criterion.
3. Implement Stage 06:
   - Verify each criterion has implementation + tests.
   - Confirm backward compatibility (existing callers still compile and behave correctly).
   - Write the closure report.

PHASE B – ReadOnly drag-and-drop guard
1. Identify the gap record for ReadOnly safeguards; if missing, create an intake + Stage 03 resolution record.
2. Stage 03:
   - Define semantics: ReadOnly means no data mutation (e.g., drag/drop, edit, check) but may still allow some navigation/expansion, as appropriate.
   - Specify exact behaviour for OnItemDrop, OnItemClick, and any other mutating events when ReadOnly is true.
   - Write testable Success Criteria per event.
3. Stage 05:
   - Guard the relevant handlers in TreeView/TreeItem so that ReadOnly prevents mutation and event firing as designed.
   - Add bUnit tests for each ReadOnly scenario.
4. Stage 06:
   - Verify behaviour and tests.
   - Add enforcement notes for reviewers.

RULES
- No opportunistic refactors.
- No Telerik or vendor dependencies.
- Follow existing MariloComponentBase conventions.
- Every change must trace to a documented gap + Success Criterion.

FINAL REPORT
Return:
1. Which TreeView gaps were moved through which stages.
2. Files modified and tests added.
3. Evidence that the two open issues are now resolved.
4. Any follow-on TreeView gaps discovered but not fixed.