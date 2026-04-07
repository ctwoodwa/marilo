# Implementation Log: GAP-21 — CheckboxTemplate (Custom Checkbox Rendering)

**Scope:** batch
**Phase:** 3 — TreeView Phase 3
**Status:** Reconstructed — code predates this log

## Summary

Gap 21 introduced a `RenderFragment<CheckboxContext>? CheckboxTemplate` parameter on `MariloTreeView`. When non-null, the default `<input type="checkbox">` is replaced entirely by the consumer's render fragment, which receives a `CheckboxContext` instance carrying the current logical state (`Checked`, `Indeterminate`, `Disabled`) and a typed mutation callback (`OnChange: Action<bool>`). When null, rendering falls through to the unchanged built-in checkbox path. `CheckboxContext` is defined in `Marilo.Core/Models/TreeViewModels.cs`. The `OnChange` callback includes a change-guard that calls `ToggleItemChecked` only when the incoming value differs from the current state, preventing spurious double-toggles on mount.

## Source Files (read-only — no changes made)

| File | Relevant section |
|------|-----------------|
| `Navigation/MariloTreeView.razor.cs` | `CheckboxTemplate` parameter declaration (line 128); render-time branching on `CheckboxTemplate != null` with `CheckboxContext` construction (lines 529–558) |
| `Marilo.Core/Models/TreeViewModels.cs` | `CheckboxContext` class with `Checked`, `Indeterminate`, `Disabled`, `OnChange` members (lines 35–48) |

## Tests

| Test file | Test name | Covers |
|-----------|-----------|--------|
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_CheckboxTemplate_RendersCustomContent` | Custom template markup appears; default `<input>` is absent |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_CheckboxTemplate_ProvidesCorrectContext` | `CheckboxContext.Checked`, `Indeterminate` match tree state (checked child, unchecked child, indeterminate parent) |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_CheckboxTemplate_DefaultCheckboxWhenNull` | No template → `<input type="checkbox">` with `mar-tree-item__checkbox` class renders |

**Coverage gaps noted:** `CheckboxContext.Disabled` reflection of `Disabled OR ReadOnly` and `OnChange` mutation callback are not independently asserted by these three tests. The resolution record lists these as success criteria; they are covered by code inspection. Adding dedicated tests would require either exposing the `OnChange` delegate on the rendered output (not straightforward in bUnit) or accepting them as implementation-verified-only.

## Phase Exit Criteria

| Criterion | Test status |
|-----------|-------------|
| `CheckboxTemplate` renders custom content instead of default checkbox | ✅ passing |
| `CheckboxContext.Checked` reflects node checked state | ✅ passing |
| `CheckboxContext.Indeterminate` reflects partial-check state | ✅ passing |
| Default `<input type="checkbox">` renders when `CheckboxTemplate` is null | ✅ passing |
| `CheckboxContext.Disabled` reflects `Disabled OR ReadOnly` | ⚠️ not independently asserted — verified by code inspection only |
| `CheckboxContext.OnChange` triggers `ToggleItemChecked` when value changes | ⚠️ not independently asserted — `OnChange` is an `Action<bool>` with no bUnit-accessible event handle |
