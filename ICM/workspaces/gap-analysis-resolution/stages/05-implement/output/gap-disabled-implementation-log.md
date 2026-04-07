# Implementation Log: GAP-17 — Disabled / ReadOnly

**Scope:** batch
**Phase:** 2 — Enhanced
**Status:** Reconstructed — code predates this log

## Summary

Gap 17 introduced two access-state parameters, `Disabled` and `ReadOnly`, that restrict user interaction with `MariloTreeView`. `Disabled` blocks all pointer, keyboard, checkbox, selection, and editing interactions and emits `aria-disabled="true"` on the root `<ul>`. `ReadOnly` blocks all mutations but intentionally preserves keyboard navigation so that keyboard users can still traverse and read the tree.

## Source Files (read-only — no changes made)

| File | Relevant section |
|------|-----------------|
| `Navigation/MariloTreeView.razor.cs` | `Disabled` parameter (line 115), `ReadOnly` parameter (line 118), `ToggleNodeAsync` guard (line 635), `ToggleItemChecked` guard (line 254), `SelectItem` guard (line 302), `HandleKeyDown` guard — `Disabled` only (line 705), `ExpandOnClick` render guard — `Disabled` only (line 501), drag-drop render guard — `Disabled` only (line 480), title click guard (line 597), inline edit guards (lines 599, 784) |
| `Navigation/MariloTreeView.razor` | `aria-disabled` on root `<ul>` (line 8), toggle button `disabled` attribute (line 519), checkbox `disabled` attribute (line 548) |

## Tests

| Test file | Test name | Covers |
|-----------|-----------|--------|
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_Disabled_SetsAriaDisabledOnRoot` | `Disabled=true` sets `aria-disabled="true"` on root element |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_Disabled_False_NoAriaDisabled` | `Disabled=false` omits `aria-disabled` attribute |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_Disabled_PreventsExpandCollapseViaToggle` | `Disabled=true` blocks expand/collapse via `ToggleNodeAsync` |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_Disabled_PreventsSelection` | `Disabled=true` blocks `SelectItem` |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_Disabled_PreventsCheckboxChanges` | `Disabled=true` blocks `ToggleItemChecked` |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_Disabled_PreventsKeyboardNavigation` | `Disabled=true` causes `HandleKeyDown` to return immediately |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_ReadOnly_PreventsCheckboxChanges` | `ReadOnly=true` blocks `ToggleItemChecked` |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_ReadOnly_AllowsKeyboardFocusMovement` | `ReadOnly=true` does not block `HandleKeyDown` focus movement |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_BothDefaultToFalse` | Both `Disabled` and `ReadOnly` default to `false` |

**Coverage gaps noted:** None

## Phase Exit Criteria

| Criterion | Test status |
|-----------|-------------|
| `Disabled=true` prevents expand/collapse via toggle button | ✅ passing |
| `Disabled=true` prevents selection | ✅ passing |
| `Disabled=true` prevents checkbox changes | ✅ passing |
| `Disabled=true` prevents keyboard navigation | ✅ passing |
| `Disabled=true` sets `aria-disabled="true"` on root element | ✅ passing |
| `ReadOnly=true` prevents expand/collapse via `ToggleNodeAsync` | ✅ passing |
| `ReadOnly=true` prevents checkbox changes | ✅ passing |
| `ReadOnly=true` allows keyboard navigation — focus movement is not blocked | ✅ passing |
| Both `Disabled` and `ReadOnly` default to `false` | ✅ passing |
