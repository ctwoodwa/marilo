# Implementation Log: T4 Pickers Batch 7 — MultiSelectSettings child component API

> Date: 2026-04-08
> Resolutions: `stages/03-resolution-design/output/gap-t4-picker-batch7-resolutions.md`
> Components: `MariloMultiSelect` (+ two new sibling components)
> Scope: batch (Stage 04 skipped per workspace gap-scope routing)
> Execution mode: subagent-driven development (one implementer subagent + spec compliance review + code quality review with one fix-and-re-review loop)

---

## RES-T4B7-01: MultiSelectSettings + MultiSelectPopupSettings

### Files modified

- `src/Marilo.Components/Forms/Inputs/MultiSelectSettings.cs` — **new file** (103 lines): contains `IMultiSelectSettingsSink` internal interface, `MultiSelectSettings` public class, `MultiSelectPopupSettings` public class.
- `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor` — added `@implements IMultiSelectSettingsSink` directive, `ChildContent` parameter, four sink methods, five `Effective*` computed properties, two private registration fields, `<CascadingValue>` wrap, three markup updates.
- `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` — added 7 new bUnit tests (lines 689–842 after fixes); also fixed a pre-existing build break in `OnChange_DoesNotFireOnExternalValueSet` (Batch 6 test that called the wrong bUnit rebind API).

### Architectural decisions made by implementer

1. **`@implements` directive over partial class.** Implementer chose `@implements IMultiSelectSettingsSink` at the top of `MariloMultiSelect.razor` rather than creating a sibling `MariloMultiSelect.razor.cs`. Rationale verified: 22+ `.razor` files in the codebase already use `@implements`, including `MariloAutocomplete.razor` and `MariloColorPicker.razor` in the same `Forms/Inputs` folder. Single-file footprint preserved.

2. **`internal` access modifier on `Effective*` properties.** Implementer verified `[InternalsVisibleTo("Marilo.Tests.Unit")]` is configured at `src/Marilo.Components/Marilo.Components.csproj:20`, then made the five `Effective*` properties `internal` so the `MultiSelectSettings_AdaptiveMode_OverridesParent` test can read `EffectiveAdaptiveMode` directly. Cleaner than indirect verification.

3. **CascadingValue placement.** Wrap placed at end of root component `<div>`, **outside** the `@if (_isOpen)` dropdown block, wrapped in `@if (ChildContent != null)`. Persists across dropdown open/close cycles; avoids allocating a CascadingValue when no settings children are present.

4. **Interface-cast cascade value.** `<CascadingValue Value="(IMultiSelectSettingsSink)this" IsFixed="true">@ChildContent</CascadingValue>` — the cast to interface is the critical decoupling that lets the non-generic `MultiSelectSettings` / `MultiSelectPopupSettings` child components receive the cascade without generic type parameters. Avoids the `MariloWizard CascadingValue bug` class documented in cerebrum.

5. **`IDisposable` directly on children, not via base.** Children inherit `ComponentBase` (not `MariloComponentBase`) and implement `IDisposable.Dispose()` directly. Cleaner — they don't need `Class`/`Style`/`AdditionalAttributes` since they render no markup.

6. **`ReferenceEquals` defensive unregister.** Both `UnregisterSettings` and `UnregisterPopupSettings` guard with `if (ReferenceEquals(_registeredX, s))` before nulling, defending against a stale Dispose from a previously-disposed child nulling a newer registration.

### Subagent execution flow

1. **Implementer subagent dispatched** with full plan in prompt. Implementer reported `DONE` after writing all three files, applying all 14 numbered requirements, and adding 7 tests.

2. **Spec compliance reviewer subagent dispatched.** Verified each numbered acceptance criterion against actual code. Found everything compliant; no missing items, no extras, no drive-by changes. Returned `✅ Spec compliant`.

3. **Code quality reviewer subagent dispatched.** Found 3 important issues + 1 minor:
   - Issue 1: `EffectiveAdaptiveMode` is plumbing without runtime consumer; the test gives false impression of behavioral coverage
   - Issue 2: `ChildContent_AcceptsSettingsTagsWithoutVisibleDom` test asserts conditions that are trivially true with the dropdown closed
   - Issue 3: `MultiSelectPopupSettings_Class_ConcatenatesOntoPopup` test name says "Concatenates" but implementation/assertions are override
   - Issue 8: `GC.SuppressFinalize` calls on classes without finalizers (verified present, instructed to remove)

4. **Fix subagent dispatched** with the same context. Reported `DONE_WITH_CONCERNS` — all four issues fixed, but flagged a pre-existing build break in `OnChange_DoesNotFireOnExternalValueSet` (Batch 6 test calling `cut.SetParametersAndRender(...)`, an API that does not exist on bUnit v2's `IRenderedComponent<T>`).

5. **Controller (this session) fixed the bUnit API issue** directly in `OnChange_DoesNotFireOnExternalValueSet`: changed `cut.SetParametersAndRender(...)` to `cut.Render(parameters => ...)` and re-supplied all original parameters (`Data`, `TextField`, `ValueField`, `OnChange`, plus the new `Value`). bUnit v2's `Render` rebind does not merge with the previous parameter set — full re-supply is required. Verified `MariloTestBase : BunitContext` (bUnit v2) at `tests/Marilo.Tests.Unit/MariloTestBase.cs:18`.

6. **Re-review subagent dispatched** to verify all four code quality fixes plus the controller's `Render` API fix. Returned `Approved` — all fixes verified, no new issues introduced.

### Verification (manual code-trace, post-fix state)

| Scenario | Expected | Trace |
|---|---|---|
| `<MultiSelectPopupSettings Height="400px"/>` overrides parent `PopupHeight="200px"` | Virtualized container style contains `400px`, not `200px` | `EffectivePopupHeight => _registeredPopupSettings?.Height ?? PopupHeight` returns `"400px"` after register |
| `<MultiSelectPopupSettings MaxHeight="500px"/>` overrides parent `PopupMaxHeight="300px"` | List container style contains `500px`, not `300px` | `_popupMaxHeightStyle` reads `EffectivePopupMaxHeight ?? EffectivePopupHeight`; `EffectivePopupMaxHeight` returns `"500px"` |
| `<MultiSelectPopupSettings Width="320px"/>` adds inline width | Popup div style contains `width:320px` | New conditional `style="@(EffectivePopupWidth is not null ? $"width:{EffectivePopupWidth};" : null)"` |
| `<MultiSelectPopupSettings Class="child-class"/>` overrides parent `PopupClass="parent-class"` | Popup div class contains `child-class`, not `parent-class` | `EffectivePopupClass => _registeredPopupSettings?.Class ?? PopupClass` returns `"child-class"` |
| No settings child, only parent `PopupHeight="250px"` | Virtualized container style contains `250px` | All Effective* fall-throughs return parent param values |
| `<MultiSelectSettings AdaptiveMode="Auto"/>` overrides parent `AdaptiveMode="None"` | `cut.Instance.EffectiveAdaptiveMode` returns `Auto`; parent param unchanged | Test asserts both via `internal` accessor exposed through `[InternalsVisibleTo]` |
| Settings children present | No new visible DOM | Two-render comparison: `baseline.FindAll("*").Count == withSettings.FindAll("*").Count` |
| Child disposed before another registers | New registration not nulled | `ReferenceEquals` guard in unregister |
| Register/unregister called from non-renderer thread | Dispatcher-safe | All four sink methods use `InvokeAsync(StateHasChanged)` |
| `OnChange` external Value set test (Batch 6) | Compiles + passes | Now uses bUnit v2 `cut.Render(...)` rebind API with full parameter re-supply |

---

## Tests

### Added tests (Batch 7)

#### `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` (7 new tests, lines 689–842)

| Test | Purpose |
|------|---------|
| `MultiSelectPopupSettings_Height_OverridesParentPopupHeight` | Child Height beats parent PopupHeight; `400px` present, `200px` absent in virtualized container style |
| `MultiSelectPopupSettings_MaxHeight_OverridesParentPopupMaxHeight` | Child MaxHeight beats parent PopupMaxHeight; `500px` present, `300px` absent in list container style |
| `MultiSelectPopupSettings_Width_AppliedToPopup` | Child Width emits inline `width:320px` on popup div |
| `MultiSelectPopupSettings_Class_OverridesParentPopupClass` | (Renamed from `*Concatenates*` per code review) Child Class wins; `child-class` present, `parent-class` absent |
| `NoSettingsChild_FallsBackToParentParameters` | Default fall-through path: parent `PopupHeight="250px"` flows through when no child registered |
| `MultiSelectSettings_AdaptiveMode_OverridesParent` | Parent `AdaptiveMode` parameter unchanged (`None`); `EffectiveAdaptiveMode` returns child override (`Auto`). Has `// NOTE` clarifying state-only test status |
| `ChildContent_AcceptsSettingsTagsWithoutVisibleDom` | (Rewritten per code review) Two-render comparison: `baseline.FindAll("*").Count == withSettings.FindAll("*").Count` proves children produce zero DOM |

### Bonus fix: pre-existing Batch 6 build break

- `OnChange_DoesNotFireOnExternalValueSet` previously called `cut.SetParametersAndRender(...)` — an API that does not exist on bUnit v2's `IRenderedComponent<T>`. The test was added in Batch 6 and never validated against an actual build.
- Fixed by switching to `cut.Render(parameters => ...)` (bUnit v2 rebind API) with full re-supply of `Data`, `TextField`, `ValueField`, `OnChange`, and the new `Value`.
- This fix is recorded here, not in the Batch 6 closure report, because the issue surfaced during Batch 7's review loop and was correctable with a one-line edit by the controller.

### Test execution

Test runtime not executed in this session — `.NET SDK not available` per `_config/coverage-summary.md` Active Blockers. All 7 new tests + the Batch 6 fix verified by code inspection only. Same convention as Batches 4–6. The Batch 6 build break was caught only because Batch 7's code reviewer noticed the implementer mentioning a build error, which is itself a positive signal that the subagent-driven dev workflow surfaces issues earlier than the controller's solo work would have.

---

## Files written (this batch)

| Stage | File |
|-------|------|
| 03 | `ICM/workspaces/gap-analysis-resolution/stages/03-resolution-design/output/gap-t4-picker-batch7-resolutions.md` |
| 05 | `ICM/workspaces/gap-analysis-resolution/stages/05-implement/output/gap-t4-picker-batch7-implementation-log.md` (this file) |
| 06 | `ICM/workspaces/gap-analysis-resolution/stages/06-validate/output/gap-t4-picker-batch7-closure-report.md` |
| Source (new) | `src/Marilo.Components/Forms/Inputs/MultiSelectSettings.cs` |
| Source | `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor` |
| Tests | `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` |

## Files read (target project)

- `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor` (full)
- `src/Marilo.Components/Forms/Inputs/MultiSelectSettings.cs` (full, post-fix)
- `src/Marilo.Components/DataGrid/MariloDataGrid.razor` (lines 36–39 — canonical CascadingValue wrap reference)
- `src/Marilo.Components/DataGrid/MariloGridColumn.razor` (canonical child component reference)
- `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs` (lines 253–269 — canonical Register/Unregister reference)
- `src/Marilo.Components/Marilo.Components.csproj` (line 20 — verified `[InternalsVisibleTo]`)
- `src/Marilo.Core/Base/MariloComponentBase.cs` (verified `CombineClasses` signature)
- `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` (existing test conventions)
- `tests/Marilo.Tests.Unit/MariloTestBase.cs` (verified bUnit v2 base class)

## No opportunistic changes

Every modified file traces directly to GAP-MSEL-005 RES-T4B7-01 OR to the in-scope Batch 6 fix (caught by Batch 7's review loop). No drive-by refactors.
