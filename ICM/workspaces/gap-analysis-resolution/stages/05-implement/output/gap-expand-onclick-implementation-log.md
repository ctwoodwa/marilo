# Implementation Log: GAP-12 — ExpandOnClick / ExpandOnDoubleClick

**Scope:** batch
**Phase:** 2 — Enhanced
**Status:** Reconstructed — code predates this log

## Summary

Gap 12 adds two opt-in interaction modes to MariloTreeView: `ExpandOnClick` causes a parent node to expand or collapse when its header row is clicked, and `ExpandOnDoubleClick` does the same on double-click. Both parameters default to `false` and are guarded at render time to avoid attaching event attributes to disabled trees or leaf nodes. `ExpandOnDoubleClick` is additionally suppressed when `AllowEditing=true` to prevent conflict with the inline-edit activation path.

## Source Files (read-only — no changes made)

| File | Relevant section |
|------|-----------------|
| Navigation/MariloTreeView.razor.cs | `ExpandOnClick` (line 80), `ExpandOnDoubleClick` (line 83), render-time guard with `hasKids && !Disabled` (lines 500–508), `ToggleNodeAsync` |

## Tests

| Test file | Test name | Covers |
|-----------|-----------|--------|
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_ExpandOnClick_True_TogglesExpandOnHeaderClick` | `ExpandOnClick=true` triggers `ToggleNodeAsync` on header click |
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_ExpandOnClick_False_DoesNotAttachOnClickToHeader` | `ExpandOnClick=false` does not attach `onclick` handler |
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_ExpandOnDoubleClick_True_ExpandsOnDoubleClick` | `ExpandOnDoubleClick=true` triggers `ToggleNodeAsync` on double-click |
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_ExpandOnDoubleClick_SuppressedWhenAllowEditing` | `ExpandOnDoubleClick=true` with `AllowEditing=true` does not attach `ondblclick` |
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_ExpandOnClick_Disabled_PreventsHandlerAttachment` | `Disabled=true` prevents `onclick` handler regardless of `ExpandOnClick` value |
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_BothDefaultToFalse` | `ExpandOnClick` and `ExpandOnDoubleClick` default to `false` |

**Coverage gaps noted:** None

## Phase Exit Criteria

| Criterion | Test status |
|-----------|-------------|
| `ExpandOnClick=true` triggers `ToggleNodeAsync` on header click for a parent node | ✅ passing |
| `ExpandOnClick=false` does NOT attach an `onclick` handler to the node header | ✅ passing |
| `ExpandOnDoubleClick=true` triggers `ToggleNodeAsync` on header double-click | ✅ passing |
| `ExpandOnDoubleClick=true` combined with `AllowEditing=true` does NOT attach an `ondblclick` handler | ✅ passing |
| Both `ExpandOnClick` and `ExpandOnDoubleClick` default to `false` | ✅ passing |
| `Disabled=true` prevents `ExpandOnClick` from attaching an `onclick` handler regardless of parameter value | ✅ passing |
