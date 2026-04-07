# Regression Triage — Pre-existing Test Failures

**Triage date:** 2026-04-02
**Total tests:** 186 (173 passing, 13 failing)
**Failures introduced by gap-analysis-resolution run:** 0

All 13 failures pre-date this run. None of the failing test files were modified during the gap-analysis-resolution work. The failures are caused by component source changes in prior commits that were not accompanied by corresponding test updates.

---

## Failure Summary

| Component | Failing tests | Root cause |
|-----------|:---:|---|
| MariloWindow | 4 | JS interop not mocked in tests |
| MariloEditor | 7 | JS interop not mocked in tests |
| MariloMultiSelect | 2 | DOM structure changed; test selectors stale |

---

## Detailed Triage

| Test | Component | Category | Introduced by | Recommended action |
|------|-----------|----------|---------------|--------------------|
| `Window_Renders_With_Title_And_CloseButton` | MariloWindow | Test setup issue | `f0b0314` — Window implementation added inline `eval()` JS interop for drag/resize; tests in `3891918` never set up JSInterop mock | Add `JSInterop.SetupModule()` or `JSInterop.Mode = JSRuntimeMode.Loose` to test setup |
| `Window_Overlay_Renders_When_Modal` | MariloWindow | Test setup issue | Same as above | Same fix |
| `Window_No_Overlay_When_Not_Modal` | MariloWindow | Test setup issue | Same as above | Same fix |
| `Window_CloseButton_Fires_VisibleChanged` | MariloWindow | Test setup issue | Same as above | Same fix |
| `Editor_Renders_Container_With_CssProvider_Class` | MariloEditor | Test setup issue | `92c760a` — Editor implementation added inline `eval()` JS interop for contenteditable; tests updated in `0ad04a1` but JS mock not added | Add `JSInterop.SetupModule()` or `JSInterop.Mode = JSRuntimeMode.Loose` to test setup |
| `Editor_Renders_All_Tools_When_None_Specified` | MariloEditor | Test setup issue | Same as above | Same fix |
| `Editor_Renders_With_Placeholder` | MariloEditor | Test setup issue | Same as above | Same fix |
| `Editor_Renders_Toolbar_With_Tools` | MariloEditor | Test setup issue | Same as above | Same fix |
| `Editor_Hides_Toolbar_When_ReadOnly` | MariloEditor | Test setup issue | Same as above | Same fix |
| `Editor_Renders_ContentArea` | MariloEditor | Test setup issue | Same as above | Same fix |
| `Editor_Value_Binding_Works` | MariloEditor | Test setup issue | Same as above | Same fix |
| `AllowsMultipleSelections` | MariloMultiSelect | API change | `702526f` — MultiSelect restructured to popup-based dropdown; `role="listbox"` moved inside conditionally-rendered popup div; test in `0e5169e` seeks `div[role='listbox']` before popup is open | Update test to first open the dropdown (click the combobox trigger), then find the listbox inside the popup |
| `DeselectingItemRemovesIt` | MariloMultiSelect | API change | Same as above | Same fix — open popup first, then interact with listbox |

---

## Root Cause Analysis

### JS Interop — Window and Editor (11 tests)

Both `MariloWindow` and `MariloEditor` use an inline `eval()` call to inject a JS module at render time:

```csharp
InvokeAsync<IJSObjectReference>("eval", "(() => { const mod = {}; ... return mod; })()")
```

This pattern bypasses bUnit's module-based JSInterop setup (`SetupModule`). The tests do not configure bUnit's JSInterop to handle this `eval` invocation, causing `JSRuntimeUnhandledInvocationException` immediately on render.

**Fix:** Either:
1. Set `JSInterop.Mode = JSRuntimeMode.Loose` in the test class to allow unhandled JS calls to return default values, OR
2. Add `JSInterop.Setup<IJSObjectReference>("eval", _ => true).SetResult(...)` to return a mock module object.

Option 1 is simpler and sufficient for rendering tests that don't assert on JS behavior.

### DOM Structure — MultiSelect (2 tests)

The MultiSelect was restructured from an always-visible `div[role='listbox']` to a popup-triggered dropdown. The `role="listbox"` element now renders conditionally inside a popup that only appears when the combobox trigger is activated. Tests search for `div[role='listbox']` immediately after render, before the popup is open.

**Fix:** Update test flow to:
1. Click the combobox trigger element (`div[role='combobox']` or the input area)
2. Then find the `div[role='listbox']` inside the now-visible popup
3. Then interact with `li[role='option']` elements

---

## Commit Timeline

```
3891918  Add many components, demos, tests, and services     ← Window/Editor/MultiSelect tests written
f0b0314  Resolve MariloWindow from API SURFACE to IMPLEMENTED ← Window JS interop added, tests not updated
92c760a  Resolve MariloEditor from API SURFACE to IMPLEMENTED  ← Editor JS interop added
0ad04a1  Fix test: update Editor tests for Preview button     ← Partial test fix, but JS mock still missing
702526f  Implement revamped ColorPicker and upload demos       ← MultiSelect restructured to popup
0e5169e  Refactor Menu/ContextMenu; MultiSelect API update     ← MultiSelect tests updated but DOM flow not fixed
```

All failures are attributable to component implementation evolving faster than test maintenance. No test was broken by the gap-analysis-resolution work performed on 2026-04-02.
