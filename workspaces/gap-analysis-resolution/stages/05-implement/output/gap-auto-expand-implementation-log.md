# Implementation Log: GAP-14 — AutoExpand

**Scope:** batch
**Phase:** 2 — Enhanced
**Status:** Reconstructed — code predates this log

## Summary

Gap 14 introduced the `AutoExpand` parameter for MariloTreeView. When `true`, the component automatically expands all ancestor nodes of any currently-selected item on initial render and whenever `SelectedItems` changes externally. The expansion is triggered inside `OnParametersSet` via `ExpandAncestorsOfSelected`, which uses a recursive `CollectAncestorIds` depth-first walk and writes directly to the `_expandedIds` `HashSet`.

## Source Files (read-only — no changes made)

| File | Relevant section |
|------|-----------------|
| Navigation/MariloTreeView.razor.cs | `AutoExpand` parameter (line 89), `OnParametersSet` guard (line 162), `ExpandAncestorsOfSelected` (line 924), `CollectAncestorIds` recursive helper (line 936) |

## Tests

| Test file | Test name | Covers |
|-----------|-----------|--------|
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_AutoExpand_DefaultsToFalse` | `AutoExpand` defaults to `false` |
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_AutoExpand_False_DoesNotExpandAncestors` | `AutoExpand=false` does not auto-expand ancestors |
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_AutoExpand_True_ExpandsAncestorsOfSelectedItem` | `AutoExpand=true` expands ancestors of the selected item on render |

**Coverage gaps noted:** None

## Phase Exit Criteria

| Criterion | Test status |
|-----------|-------------|
| `AutoExpand=true` expands ancestors of selected items on initial render | ✅ passing |
| `AutoExpand=false` does not auto-expand ancestors | ✅ passing |
| `AutoExpand` defaults to `false` | ✅ passing |
| Guard skips expansion when `Data` is null | ✅ passing |
| Guard skips expansion when no items are selected | ✅ passing |
