# Step 04 — Fix MultiSelect Build + Prioritize Picker Gaps

## Context

The CI build is currently broken due to a compilation error in MultiSelect.
T4 Pickers have 58 total gaps at intake with 18 classified High severity.
This prompt fixes the build blocker first, then prioritizes the 18 High gaps for implementation.

**Known build error:**
`MariloMultiSelect.razor.cs` has a compilation error (exact error TBD — read build output).
This error blocks the entire CI pipeline and must be fixed before any other work.

**High-severity Picker gaps (categories):**
- DateRangePicker: range validation, min/max boundary enforcement
- MultiSelect: tag chip rendering, virtual scrolling with large datasets
- ColorPicker: hex input sync, opacity slider
- DateTimePicker: time zone handling, 12/24h toggle

---

## Your Task

You are a Claude agent working in the `pickers-gap-analysis` ICM workspace at:
`/workspaces/Marilo/workspaces/pickers-gap-analysis/`

Read `CLAUDE.md` and `CONTEXT.md` first.

### Phase 1 — Fix the MultiSelect Build Error

1. Run `dotnet build src/Marilo.Components/Marilo.Components.csproj` and capture the full error output.
2. Read the error. Locate the offending file (expected: `MariloMultiSelect.razor.cs` or `MariloMultiSelect.razor`).
3. Fix the compilation error. Do not change any public API surface — this is a build fix only.
4. Run `dotnet build` again. Confirm it passes.
5. Write `output/multiselect-build-fix.md` documenting:
   - Error message verbatim
   - Root cause (one sentence)
   - Fix applied (one sentence)
   - Build status: PASSING

**Do not proceed to Phase 2 until the build passes.**

---

### Phase 2 — Triage High Severity Picker Gaps

1. Read `output/pickers-gap-list.md`.
2. Extract all 18 gaps tagged `priority: high`.
3. Group them by component: `DateRangePicker`, `MultiSelect`, `ColorPicker`, `DateTimePicker`, `Other`.
4. For each gap assign:
   - `complexity: low | medium | high` (estimated implementation effort)
   - `dependency: none | [gap-slug]` (if this gap must be resolved after another)
   - `type: FIX | DESIGN | SPEC`
5. Order the gaps within each component group: dependencies first, then by complexity ascending.
6. Write the prioritized list to `output/pickers-high-priority-plan.md` as a table:

```markdown
# Pickers High Priority — Implementation Plan

## DateRangePicker
| Order | Gap Slug | Type | Complexity | Dependency | Description |
|-------|----------|------|------------|------------|-------------|

## MultiSelect
...

## ColorPicker
...

## DateTimePicker
...

## Recommended Sprint Sequence
[List the gaps in the order a developer should implement them across all components,
 respecting cross-component dependencies if any.]
```

---

### Phase 3 — Implement Low-Complexity FIX Gaps

For any High priority gap tagged `type: FIX` AND `complexity: low`, implement it now using the sub-agent pattern:

```
You are resolving Pickers gap: [gap-slug] on component [component-name]
Workspace: /workspaces/Marilo/workspaces/pickers-gap-analysis/
Gap definition: [paste gap record]

Steps:
1. Read the relevant source file(s).
2. Implement the fix. Low-complexity only — if the fix grows beyond 30 lines of net-new code, stop and flag as DESIGN.
3. Write bUnit tests in tests/Marilo.Components.Tests/Pickers/ for this gap.
4. Run `dotnet build` and confirm it passes.
5. Update the gap record: set status to RESOLVED.
6. Write fix summary to output/resolved/[gap-slug]-resolution.md.
```

Leave Medium and High complexity gaps for a subsequent dedicated run.

---

## Constraints

- Phase 1 (build fix) is mandatory. Do not skip even if you believe the error is trivial.
- Do not implement any Medium or High complexity gaps in this run — triage and plan only.
- Do not change public parameter names or event signatures without a DESIGN decision.
- Sub-agents in Phase 3 operate on one gap slug only and must not touch other components.
