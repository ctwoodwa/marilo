# Closure Report: GAP-15 — Batch Expand (ExpandAllAsync / CollapseAllAsync)

**Closure Status:** Resolved
**Phase:** 2 — Enhanced
**Pipeline note:** Reconstructed — code predates formal records
**Validated:** 2026-04-02

## Criteria Verification

| Criterion | Source location | Test name | Status |
|-----------|----------------|-----------|--------|
| `ExpandAllAsync()` expands every node in the tree | MariloTreeView.razor.cs:176 — `GetTree()` + `CollectAllIds` + `_expandedIds` | `TreeView_ExpandAllAsync_MakesAllChildrenVisible` | ✅ |
| `CollapseAllAsync()` collapses every node in the tree | MariloTreeView.razor.cs:186 — `_expandedIds.Clear()` | `TreeView_CollapseAllAsync_HidesAllChildren` | ✅ |
| `ExpandAllAsync()` fires `ExpandedItemsChanged` with all node IDs | MariloTreeView.razor.cs:176 — `ExpandedItemsChanged.HasDelegate` guard | `TreeView_ExpandAllAsync_FiresExpandedItemsChanged` | ✅ |
| `CollapseAllAsync()` fires `ExpandedItemsChanged` with an empty collection | MariloTreeView.razor.cs:186 — `ExpandedItemsChanged.HasDelegate` guard | `TreeView_CollapseAllAsync_FiresExpandedItemsChangedWithEmptyCollection` | ✅ |
| Both methods are publicly accessible via `@ref` | MariloTreeView.razor.cs:176, 186 — `public async Task` signatures | `TreeView_ExpandAllAsync_MakesAllChildrenVisible`, `TreeView_CollapseAllAsync_HidesAllChildren` | ✅ |

## Evidence

- **Source:** Navigation/MariloTreeView.razor.cs
- **Tests:** TreeViewTests.cs — 4 tests covering Gap 15, all passing
- **Gap no longer present:** Yes — `ExpandAllAsync` and `CollapseAllAsync` are public `async Task` methods callable via `@ref`; both fire `ExpandedItemsChanged` when a delegate is bound and call `StateHasChanged()` unconditionally

## Enforcement Guardrails

- Code review: `ExpandAllAsync` must call `CollectAllIds` (not `ToggleNodeAsync`) to avoid triggering `SingleExpand` accordion logic during a bulk-expand operation
- Code review: `CollapseAllAsync` must call `_expandedIds.Clear()` rather than removing IDs one by one — piecemeal removal could trigger unwanted `SingleExpand` side-effects if the implementation is ever refactored to route through `ToggleNodeAsync`
- Code review: the `ExpandedItemsChanged.HasDelegate` guard must be preserved on both methods to prevent `NullReferenceException` when the callback is not bound

## Follow-up Tasks

None
