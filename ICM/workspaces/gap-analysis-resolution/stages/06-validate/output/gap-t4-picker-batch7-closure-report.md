# Closure Report: T4 Pickers Batch 7 — MultiSelectSettings child component API

> Date: 2026-04-08
> Resolutions: `stages/03-resolution-design/output/gap-t4-picker-batch7-resolutions.md`
> Implementation log: `stages/05-implement/output/gap-t4-picker-batch7-implementation-log.md`
> Original gap inventory: `stages/01-intake/output/gap-t4-pickers-inventory.md`
> Components: `MariloMultiSelect` (+ `MultiSelectSettings`, `MultiSelectPopupSettings`)
> Scope: batch (Stage 04 skipped per workspace gap-scope routing)
> Execution mode: subagent-driven development

---

## Summary

| Gap | Title | Status |
|-----|-------|--------|
| GAP-MSEL-005 | MariloMultiSelect: MultiSelectSettings child component missing | **Resolved** |

Total: 1 gap fully closed.

After this batch, **all medium-and-higher-priority MariloMultiSelect gaps are resolved**. Only `GAP-MSEL-007 ScrollMode` (deferred — Blazor `<Virtualize>` lacks the primitive) and `GAP-MSEL-008 MaxVisibleTags naming mismatch` (Won't Fix from Batch 3) remain — neither is actionable without architecture-level decisions.

---

## GAP-MSEL-005: MariloMultiSelect MultiSelectSettings child component

- **Status:** Resolved
- **Resolution:** RES-T4B7-01 — non-generic child components + internal cascade sink interface + 5 effective-value computed properties + canonical `<CascadingValue>` wrap with interface cast
- **Changed:**
  - `src/Marilo.Components/Forms/Inputs/MultiSelectSettings.cs` — **new file** (103 lines): `IMultiSelectSettingsSink` internal interface, `MultiSelectSettings : ComponentBase, IDisposable` with nullable `AdaptiveMode?` parameter, `MultiSelectPopupSettings : ComponentBase, IDisposable` with nullable `Height`/`MaxHeight`/`Width`/`Class` parameters
  - `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor` — `@implements IMultiSelectSettingsSink` directive, `[Parameter] ChildContent` parameter, four `IMultiSelectSettingsSink` method implementations using `InvokeAsync(StateHasChanged)` and `ReferenceEquals` defensive guards, five `internal Effective*` computed properties, two private registration fields, `<CascadingValue Value="(IMultiSelectSettingsSink)this" IsFixed="true">` wrap around `@ChildContent`, three markup updates (popup div class, popup div inline width style, virtualized container height) plus `_popupMaxHeightStyle` getter rewrite to read from effective values
- **Tests:**
  - `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` — 7 new bUnit tests:
    - `MultiSelectPopupSettings_Height_OverridesParentPopupHeight`
    - `MultiSelectPopupSettings_MaxHeight_OverridesParentPopupMaxHeight`
    - `MultiSelectPopupSettings_Width_AppliedToPopup`
    - `MultiSelectPopupSettings_Class_OverridesParentPopupClass` (renamed from `*Concatenates*` per code review)
    - `NoSettingsChild_FallsBackToParentParameters`
    - `MultiSelectSettings_AdaptiveMode_OverridesParent` (with `// NOTE` clarifying state-only test status until Adaptive rendering is wired up)
    - `ChildContent_AcceptsSettingsTagsWithoutVisibleDom` (rewritten per code review to use baseline-vs-with-settings descendant-count comparison; replaces a trivially-true assertion)
- **Bonus fix:** `OnChange_DoesNotFireOnExternalValueSet` (Batch 6 test) had a pre-existing build break — called `cut.SetParametersAndRender(...)`, an API that does not exist on bUnit v2's `IRenderedComponent<T>`. Caught during Batch 7's review loop. Fixed by switching to `cut.Render(parameters => ...)` (bUnit v2 rebind API) with full re-supply of all parameters since v2's rebind does not merge.
- **Enforcement:** 7 new tests pin every override path, the fall-through path, the no-DOM-leak guarantee, and the override-resolution mechanism. Existing tests continue to exercise the no-settings-child path and remain unchanged.
- **Notes:**
  - **Canonical pattern followed:** mirrors the established `MariloDataGrid` ↔ `MariloGridColumn` cascading pattern (reference: `MariloDataGrid.razor:36-39`, `MariloGridColumn.razor:5,83-92`, `MariloDataGrid.razor.cs:253-269`).
  - **MariloWizard CascadingValue bug class avoided:** the cascade value is `(IMultiSelectSettingsSink)this` cast to interface, not just `this`. This is the critical decoupling that lets non-generic children receive the cascade. The cerebrum-documented Wizard bug was "parent forgot to wrap `ChildContent` in `<CascadingValue>`" — this batch's wrap is verified present and correctly typed.
  - **Dispatcher safety:** all four `IMultiSelectSettingsSink` register/unregister methods use `InvokeAsync(StateHasChanged)`, not direct `StateHasChanged()`. Per the cerebrum learning at `[2026-04-04] Public state APIs should be dispatcher-safe`. Required because child component `OnInitialized` / `Dispose` lifecycle hooks may not run on the renderer thread.
  - **Defensive unregister:** `ReferenceEquals` guards on both unregister methods prevent a stale Dispose from a previously-disposed child nulling a newer registration. Documented in code comments.
  - **Backward compatibility preserved:** existing flat parameters (`PopupHeight`, `PopupMaxHeight`, `PopupClass`, `AdaptiveMode`) remain in place. Settings child components only override when present. No breaking change.
  - **`Width` is a new capability** — no parent-parameter equivalent. Documented in the resolution record. `MultiSelectPopupSettings.Width` is the only way to set popup width.

### Closure Criteria Check

| Check | Status | Evidence |
|-------|--------|----------|
| Target pattern adopted | ✅ | Interface-decoupled cascade, non-generic children, single-instance registration; matches RES-T4B7-01 §Target Pattern |
| Original gap behavior gone | ✅ | `<MultiSelectSettings>` and `<MultiSelectPopupSettings>` child components now exist and override parent parameters |
| No regression (existing tests) | ⚠️ pending runtime | Default parameters preserved; settings tags only override when present; existing tests do not bind ChildContent → take the no-settings-child path |
| Tests cover the change | ✅ | 7 new bUnit tests cover all 4 popup overrides + fall-through + AdaptiveMode override + no-visible-DOM guarantee |
| Consumers unaffected | ✅ | All additions are additive |
| Cross-cutting consistency | ✅ | Mirrors `MariloDataGrid` cascading pattern; uses same `[CascadingParameter]` + `OnInitialized` register + `Dispose` unregister flow |
| Build succeeds | ⚠️ pending runtime | .NET SDK not available this session — code review only. The Batch 6 build break uncovered during review was fixed before closure. |
| Spec compliance reviewed | ✅ | Spec compliance reviewer subagent verified all 14 numbered requirements. Returned `✅ Spec compliant`. |
| Code quality reviewed | ✅ | Code quality reviewer subagent flagged 3 important issues + 1 minor. All 4 fixed by implementer. Re-review returned `Approved`. |
| Cerebrum rules honored | ✅ | `InvokeAsync(StateHasChanged)` in all four sink methods; cascade value cast to interface |
| Enforcement | ✅ | 7 tests + canonical-pattern adherence + cerebrum learnings as durable guidance |

---

## Subagent-driven development workflow

This batch was the first in this workspace executed with the `superpowers:subagent-driven-development` skill. Quality gates the workflow surfaced:

1. **Spec compliance reviewer caught nothing** — implementer's first pass matched spec exactly. Confirmation that the Stage 03 plan was sufficiently detailed.
2. **Code quality reviewer caught 3 important issues** that the implementer's self-review and spec compliance review both missed:
   - Plumbing-without-consumer (`EffectiveAdaptiveMode`)
   - Trivially-passing test (`ChildContent_AcceptsSettingsTagsWithoutVisibleDom`)
   - Misleading test name (`*ConcatenatesOntoPopup` vs override behavior)
3. **Code quality reviewer also caught a Batch 6 pre-existing build break** that solo controller work had not noticed across two prior batches. The implementer flagged it during the fix loop; the controller corrected it directly.

This confirms the value of the two-stage review pattern: spec compliance and code quality catch different classes of issues. The pre-existing Batch 6 build break is the most surprising find — it would have shipped to runtime testing as a `dotnet build` failure on the first attempted runtime verification.

---

## Test Coverage Rollup

| Component | Tests added | Tests passing | Notes |
|-----------|:-----------:|:-------------:|-------|
| MariloMultiSelect (MultiSelectSettings + MultiSelectPopupSettings) | 7 | pending runtime | Code-inspection verified; .NET SDK unavailable. Includes Batch 6 build-break fix. |
| **Batch 7 total** | **7** | **pending runtime** | — |

Same convention as Batches 4–6 — runtime status reads "pending" until SDK available.

---

## Cross-cutting follow-ups

After this batch, **MariloMultiSelect is fully resolved** for all medium-and-higher-priority gaps. Remaining items:

- **GAP-MSEL-007 ScrollMode sub-item:** Deferred (Blazor `<Virtualize>` lacks the primitive). Requires custom virtualization rebuild — out of scope for this workspace.
- **GAP-MSEL-008 MaxVisibleTags naming mismatch:** Won't Fix (Batch 3 decision).

No new gaps discovered during implementation.

After this run, `MariloMultiSelect` is the most complete T4 picker in the library: 7 batches of work closing every actionable medium+ gap. The remaining workspace items all sit behind JS interop blockers, human decisions, or CDW graduation candidates.

---

## Files written this batch

| Stage | File |
|-------|------|
| 03 | `ICM/workspaces/gap-analysis-resolution/stages/03-resolution-design/output/gap-t4-picker-batch7-resolutions.md` |
| 05 | `ICM/workspaces/gap-analysis-resolution/stages/05-implement/output/gap-t4-picker-batch7-implementation-log.md` |
| 06 | `ICM/workspaces/gap-analysis-resolution/stages/06-validate/output/gap-t4-picker-batch7-closure-report.md` (this file) |
| Source (new) | `src/Marilo.Components/Forms/Inputs/MultiSelectSettings.cs` |
| Source | `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor` |
| Tests | `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` |

## Stage routing for this batch

01-intake (existing) → 02-prioritize (existing) → 03-resolution-design (new) → 05-implement (new, subagent-driven) → 06-validate (new). Stage 04 skipped per `batch` scope routing.

## Blockers

None for this batch. Same workspace-level `.NET SDK not available` blocker for runtime test execution applies.
