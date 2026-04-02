# Implementation Log: GAP-19 — SelectNodeAsync (Programmatic Navigation)

**Scope:** batch
**Phase:** 3 — TreeView Phase 3
**Status:** Reconstructed — code predates this log

## Summary

Gap 19 introduced `SelectNodeAsync`, a public async method on `MariloTreeView` accessible via `@ref`. A single call atomically expands all ancestor nodes of the target (using existing private helpers `FindNode` and `CollectAncestorIds`), clears and replaces the selection with only the target node, sets `_focusedNodeId` for keyboard navigation continuity, fires `ExpandedItemsChanged` (when a delegate is bound) and `SelectedItemsChanged` (always), invalidates `_cachedTree`, and calls `StateHasChanged`. If the requested ID is not present in the tree the method returns immediately without mutating any state.

## Source Files (read-only — no changes made)

| File | Relevant section |
|------|-----------------|
| `Navigation/MariloTreeView.razor.cs` | `SelectNodeAsync` public method (lines 201–225); `FindNode`, `CollectAncestorIds` private helpers; `_expandedIds`, `_selectedIds`, `_focusedNodeId` internal fields |

## Tests

| Test file | Test name | Covers |
|-----------|-----------|--------|
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_SelectNodeAsync_ExpandsAncestors` | All ancestors of the target node become visible after the call |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_SelectNodeAsync_SelectsTargetNode` | `SelectedItemsChanged` fires with only the target node's ID |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_SelectNodeAsync_FiresExpandedItemsChanged` | `ExpandedItemsChanged` fires when a delegate is bound and ancestors are expanded |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_SelectNodeAsync_SetsFocusToTargetNode` | Target node's `li` element carries `mar-tree-item--focused` class after the call |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_SelectNodeAsync_SilentlyReturnsForNonExistentId` | No exception, no `SelectedItemsChanged`, no `ExpandedItemsChanged` for an unknown ID |

**Coverage gaps noted:** The asymmetry between `ExpandedItemsChanged` (guarded by `HasDelegate`) and `SelectedItemsChanged` (always awaited) is intentional per the resolution record and is confirmed by the test suite but not independently asserted in a dedicated test. Multi-select preservation (i.e., that prior selection is fully replaced) is covered implicitly by `SelectsTargetNode` asserting `Single(lastReceived)`.

## Phase Exit Criteria

| Criterion | Test status |
|-----------|-------------|
| `SelectNodeAsync` expands ancestors of the target node | ✅ passing |
| `SelectNodeAsync` selects only the target node | ✅ passing |
| `SelectNodeAsync` sets keyboard focus to the target node | ✅ passing |
| `SelectNodeAsync` fires `ExpandedItemsChanged` when delegate bound | ✅ passing |
| `SelectNodeAsync` fires `SelectedItemsChanged` | ✅ passing |
| `SelectNodeAsync` returns silently for a non-existent node ID | ✅ passing |
