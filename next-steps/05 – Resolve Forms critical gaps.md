WORKSPACE
- workspaces/Marilo/workspaces/gap-analysis-resolution

OBJECTIVE
Begin resolving the 19 Critical Forms gaps identified in the executive report, focusing on high-impact form containers and controls (MariloForm, validation, core inputs).

BOOTSTRAP
1. Read:
   - CLAUDE.md
   - CONTEXT.md
   - config/gap-context.md
   - src/Marilo.Components/GAPANALYSISRESOLUTIONPLAN.md (Forms section).
   - Forms-related spec docs (docs/component-specs/forms/…).
2. Identify:
   - the 19 Critical Forms gaps,
   - their current stage (Intake only vs Stage 03 vs Stage 05).

ROUTING
1. Build a small routing table:
   - gap ID → current stage → next stage.
2. Only pick Critical severity gaps for this run.

EXECUTION
1. For gaps without Stage 03:
   - create Stage 03 resolution records:
     - clearly define target pattern (e.g., Model + EditContext semantics),
     - list testable Success Criteria.
2. For gaps with Stage 03 but no Stage 05:
   - implement code and tests in Stage 05, respecting existing patterns and conventions.
3. For gaps with Stage 05 but no Stage 06:
   - run Stage 06 validation and closure.
4. Update test coverage documentation for each gap using the established Stage 03/05/06 pattern (Success Criteria, Tests table, closure evidence).

PLAN / STATUS
1. Update GAPANALYSISRESOLUTIONPLAN.md:
   - mark each touched gap with new stage and status,
   - briefly annotate the Forms section with progress.

RULES
- No opportunistic refactors.
- All changes must be traceable to specific gaps.
- Preserve consistent Forms API semantics across components.

FINAL REPORT
Return:
1. Which critical Forms gaps were moved and to which stages.
2. Summary of API decisions (Model/EditContext, validation patterns).
3. Test files and counts added.
4. Remaining Critical Forms gaps and blockers.