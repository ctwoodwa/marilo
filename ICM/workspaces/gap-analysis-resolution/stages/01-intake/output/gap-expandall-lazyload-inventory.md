# Gap Inventory: GAP-expandall-lazyload

**Gap ID:** GAP-expandall-lazyload
**Title:** ExpandAllAsync does not trigger LoadChildrenAsync for unloaded nodes
**Phase:** 2.5 (post-Phase 2 fix, pre-Phase 3)
**Scope:** single
**Severity:** High
**Discovered:** 2026-04-02 — during Phase 2 pipeline reconstruction (gap-batch-expand resolution record, Undocumented Behaviour #2)
**Affected files:** `Navigation/MariloTreeView.razor.cs`

---

## Problem Statement

When a `MariloTreeView` uses lazy loading via `LoadChildrenAsync`, calling `ExpandAllAsync()` silently skips all nodes whose children have not yet been fetched. The user sees a partially expanded tree with no indication that unloaded subtrees were omitted. There is no error, no warning, and no loading indicator — the operation appears complete.

This is a **silent data loss** problem: the user invokes "expand all" expecting full visibility of the hierarchy, but lazy-loaded branches remain collapsed and invisible.

---

## Exact Behaviour — Code Trace

### The correct path: `ToggleNodeAsync` (single-node expand)

When a user clicks a toggle button, `ToggleNodeAsync(id)` runs (line 633). For a node with `HasChildren=true` and no loaded children:

```
1. _expandedIds.Add(id)                           — mark as expanded
2. if (LoadChildrenAsync != null && !_loadedNodeIds.Contains(id))
3.   if (node.HasChildren && !node.Children.Any())
4.     _loadingIds.Add(id) → StateHasChanged()     — show loading indicator
5.     children = await LoadChildrenAsync(node.Item) — FETCH children
6.     _cachedTree = null                           — invalidate cache
7.     _loadedNodeIds.Add(id)                       — mark as loaded
8.     _loadingIds.Remove(id)                       — hide loading indicator
```

This path correctly: fetches children, shows loading UI, invalidates cache, and prevents re-fetching.

### The broken path: `ExpandAllAsync` (bulk expand)

When the consumer calls `ExpandAllAsync()` (line 176):

```
1. var tree = GetTree()           — returns cached tree (or builds from Data)
2. CollectAllIds(tree, _expandedIds) — recurse tree, add every node ID to expanded set
3. Fire ExpandedItemsChanged
4. StateHasChanged()
```

**What `CollectAllIds` sees for a lazy-loaded node:**

A node created by `BuildHierarchical` with `HasChildren=true` but whose `ItemsField` property returns null/empty (because children haven't been fetched) produces:

```
TreeNode(Id="parent-1", ..., Children=[], HasChildren=true)
```

`CollectAllIds` adds `"parent-1"` to `_expandedIds` (correct) but recurses into `Children` which is **empty** — there are no child IDs to add. The children don't exist in the tree model yet.

**Result:** The parent is marked expanded, but when the tree re-renders:
- The render guard at line 618: `if (hasKids && isExpanded && node.Children.Any())` — `node.Children.Any()` is **false**, so the children `<ul>` is not rendered
- The loading indicator guard at line 608: `if (hasKids && isExpanded && _loadingIds.Contains(node.Id))` — `_loadingIds` is **empty** (ExpandAllAsync never adds to it), so no loading indicator appears
- The node appears expanded (toggle arrow points down) but shows no children and no loading state

**No `LoadChildrenAsync` call is ever made.** The lazy-load trigger only lives inside `ToggleNodeAsync`, which `ExpandAllAsync` does not call.

---

## Affected Scenarios

| Scenario | Impact | Frequency |
|----------|--------|-----------|
| Consumer calls `ExpandAllAsync()` on a tree with `LoadChildrenAsync` bound | Unloaded subtrees silently omitted; tree appears fully expanded but is incomplete | Every use of ExpandAll with lazy trees |
| Consumer calls `ExpandAllAsync()` after partially loading some branches | Only previously loaded branches expand; other branches silently skipped | Common — progressive disclosure pattern |
| Consumer binds `ExpandedItems` and inspects the result | `ExpandedItemsChanged` fires with IDs of loaded nodes only; consumer has no way to know which nodes were skipped | Whenever ExpandedItems binding is used with lazy data |

### Not affected

- Trees without `LoadChildrenAsync` — all nodes are in the initial `Data`, `CollectAllIds` finds everything
- `CollapseAllAsync()` — collapse does not need child data; it just clears the set
- Manual expand via toggle button — `ToggleNodeAsync` correctly triggers lazy loading

---

## Risk Assessment

**Severity: High**

| Factor | Assessment |
|--------|------------|
| Silent failure | Yes — no error, no warning, no loading indicator, no callback |
| Data correctness | Violated — `ExpandedItemsChanged` reports incomplete state as if it were complete |
| User confusion | High — tree looks "fully expanded" but missing branches are invisible |
| Workaround available | None — consumer cannot distinguish between "no children exist" and "children not loaded" from the callback result |
| Blast radius | Any consumer using both `LoadChildrenAsync` and `ExpandAllAsync()` |

---

## Proposed Fix Approaches

### Option A: Sequential LoadChildrenAsync in ExpandAllAsync (recommended)

**Approach:** Modify `ExpandAllAsync` to iterate over nodes with `HasChildren=true && !Children.Any() && !_loadedNodeIds.Contains(id)`, call `LoadChildrenAsync` for each, rebuild the tree cache, then collect all IDs.

```csharp
public async Task ExpandAllAsync()
{
    // Phase 1: Load all unloaded lazy nodes
    if (LoadChildrenAsync != null)
    {
        var tree = GetTree();
        var unloadedNodes = FindUnloadedNodes(tree);
        foreach (var node in unloadedNodes)
        {
            _loadingIds.Add(node.Id);
            try
            {
                var children = await LoadChildrenAsync(node.Item);
                if (children != null)
                    _cachedTree = null; // invalidate for rebuild
            }
            finally
            {
                _loadingIds.Remove(node.Id);
                _loadedNodeIds.Add(node.Id);
            }
        }
    }

    // Phase 2: Now collect all IDs (tree rebuilt with loaded children)
    var fullTree = GetTree();
    CollectAllIds(fullTree, _expandedIds);
    if (ExpandedItemsChanged.HasDelegate)
        await ExpandedItemsChanged.InvokeAsync(_expandedIds.ToList());
    StateHasChanged();
}
```

- **Pros:** Correct behaviour; consumers get the full tree. Loading is sequential (avoids thundering herd). Can show loading indicators per node during the process.
- **Cons:** May be slow for deeply nested trees with many lazy levels (N sequential awaits). Each `LoadChildrenAsync` call invalidates cache, requiring rebuild for next level. No parallelism.
- **Risk:** Long-running operation with no cancellation support. UI is blocked during sequential fetches (Blazor single-threaded render).
- **Effort:** Medium

### Option B: Opt-in parameter `ExpandAllIncludeLazyNodes`

**Approach:** Add a boolean parameter to `ExpandAllAsync(bool includeLazyNodes = false)`. When `false` (default), current behaviour is preserved (backward compatible). When `true`, loads lazy nodes first.

- **Pros:** No breaking change. Consumer explicitly opts in to the potentially expensive operation.
- **Cons:** Default behaviour is still the silent failure. Discoverability is poor — consumers must know the parameter exists.
- **Risk:** Low (no breaking change), but the silent failure remains the default.
- **Effort:** Medium

### Option C: Throw when lazy-load and ExpandAll combine

**Approach:** If `LoadChildrenAsync != null` and any node has `HasChildren=true && !_loadedNodeIds.Contains(id)`, throw `InvalidOperationException` with a message explaining the limitation.

- **Pros:** Fail-fast; consumer knows immediately. Zero ambiguity.
- **Cons:** Hostile UX. Breaking change if consumers already call ExpandAll on lazy trees (they get incomplete results but no crash today).
- **Risk:** Medium (breaking change)
- **Effort:** Small

### Recommendation

**Option A** for correctness. The primary purpose of "expand all" is to show the full tree. A sequential load with loading indicators per node is the expected UX. Add a `CancellationToken`-accepting overload or an `onProgress` callback for large trees.

If Option A's performance is unacceptable for specific use cases, add Option B's `includeLazyNodes` parameter with `true` as the default (not `false` — the correct behaviour should be the default).

---

## Stage Routing

| Stage | Action |
|-------|--------|
| 01-intake | ✅ This document |
| 02-prioritize | Skip (single scope, severity High) |
| 03-resolution-design | Design the fix; choose between Options A/B/C; write Success Criteria |
| 04-remediation-plan | Skip (single scope) |
| 05-implement | Apply fix + write tests |
| 06-validate | Verify ExpandAll with lazy trees works correctly |

---

## Cross-References

- Discovered in: `stages/03-resolution-design/output/gap-batch-expand-resolutions.md` (Undocumented Behaviour #2)
- Related: `ToggleNodeAsync` lazy-load path (line 656-681) — the correct implementation to model
- Related: `_loadedNodeIds` guard set — tracks which nodes have already been loaded
- Plan entry: `GAP_ANALYSIS_RESOLUTION_PLAN.md` → Phase 2.5 section
