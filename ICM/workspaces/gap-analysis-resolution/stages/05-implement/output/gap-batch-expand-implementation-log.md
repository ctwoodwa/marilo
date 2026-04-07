# Implementation Log: GAP-15 — Batch Expand (ExpandAllAsync / CollapseAllAsync)

**Scope:** batch
**Phase:** 2 — Enhanced
**Status:** Reconstructed — code predates this log

## Summary

Gap 15 added two public async methods — `ExpandAllAsync` and `CollapseAllAsync` — to MariloTreeView, accessible via Blazor `@ref`. `ExpandAllAsync` walks the cached tree via `CollectAllIds` and adds every node ID to `_expandedIds`; `CollapseAllAsync` calls `_expandedIds.Clear()`. Both fire `ExpandedItemsChanged` when a delegate is bound and call `StateHasChanged()` unconditionally, keeping two-way binding state in sync.

## Source Files (read-only — no changes made)

| File | Relevant section |
|------|-----------------|
| Navigation/MariloTreeView.razor.cs | `ExpandAllAsync` public method (line 176), `CollapseAllAsync` public method (line 186), `CollectAllIds` helper, `ExpandedItemsChanged` guard |

## Tests

| Test file | Test name | Covers |
|-----------|-----------|--------|
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_ExpandAllAsync_MakesAllChildrenVisible` | `ExpandAllAsync()` expands every node in the tree |
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_CollapseAllAsync_HidesAllChildren` | `CollapseAllAsync()` collapses every node in the tree |
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_ExpandAllAsync_FiresExpandedItemsChanged` | `ExpandAllAsync()` fires `ExpandedItemsChanged` with all node IDs |
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_CollapseAllAsync_FiresExpandedItemsChangedWithEmptyCollection` | `CollapseAllAsync()` fires `ExpandedItemsChanged` with an empty collection |

**Coverage gaps noted:** None

## Phase Exit Criteria

| Criterion | Test status |
|-----------|-------------|
| `ExpandAllAsync()` expands every node in the tree | ✅ passing |
| `CollapseAllAsync()` collapses every node in the tree | ✅ passing |
| `ExpandAllAsync()` fires `ExpandedItemsChanged` with all node IDs | ✅ passing |
| `CollapseAllAsync()` fires `ExpandedItemsChanged` with an empty collection | ✅ passing |
| Both methods are publicly accessible via `@ref` | ✅ passing |
