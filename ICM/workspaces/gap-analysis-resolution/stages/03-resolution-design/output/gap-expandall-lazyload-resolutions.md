# Resolution Design: GAP-expandall-lazyload

**Gap ID:** GAP-expandall-lazyload
**Title:** ExpandAllAsync does not trigger LoadChildrenAsync for unloaded nodes
**Resolution Date:** 2026-04-03
**Selected Option:** C (opt-in parameter with correct default)

---

## Options Evaluated

### Option A: Always load lazy nodes in ExpandAllAsync

**Approach:** Modify `ExpandAllAsync()` to always call `LoadChildrenAsync` for every unloaded node before expanding.

- **Pros:** Simple; always correct.
- **Cons:** Breaking change for consumers who call `ExpandAllAsync()` on lazy trees today and expect the (broken) fast behavior. No cancellation or depth limit.
- **Verdict:** Rejected — no control over potentially expensive operation.

### Option B: Separate method `ExpandAllIncludingLazyAsync()`

**Approach:** Keep `ExpandAllAsync()` as-is and add a new method.

- **Pros:** No breaking change.
- **Cons:** Duplicated logic; the default method remains silently broken. Poor discoverability.
- **Verdict:** Rejected — preserves the silent failure as the default.

### Option C: Opt-in parameter with explicit documentation (SELECTED)

**Approach:** Extend `ExpandAllAsync` signature:

```csharp
public async Task ExpandAllAsync(
    bool includeUnloaded = false,
    int maxDepth = int.MaxValue,
    CancellationToken cancellationToken = default)
```

- `includeUnloaded = false` — preserves current behavior (only expands already-loaded nodes). This is backward compatible.
- `includeUnloaded = true` — performs depth-first traversal, calling `LoadChildrenAsync` for every unloaded node (`HasChildren=true && !Children.Any() && !_loadedNodeIds.Contains(id)`), respecting `maxDepth` and `cancellationToken`.
- After all lazy nodes are loaded, collects all IDs and expands.

- **Pros:** Backward compatible. Consumer explicitly opts in. Supports cancellation for large trees. `maxDepth` prevents unbounded recursion.
- **Cons:** Slightly more complex API surface. Default is still the "skip" behavior — but now documented explicitly.
- **Verdict:** Selected. Best balance of correctness, safety, and backward compatibility.

---

## Selected Resolution: Option C

### API Design

```csharp
/// <summary>
/// Expands all nodes in the tree.
/// When includeUnloaded is true, triggers LoadChildrenAsync for lazy-loaded
/// nodes that haven't been fetched yet, up to maxDepth levels deep.
/// </summary>
public async Task ExpandAllAsync(
    bool includeUnloaded = false,
    int maxDepth = int.MaxValue,
    CancellationToken cancellationToken = default)
```

### Implementation Pattern

1. If `includeUnloaded` is false: current behavior (CollectAllIds + fire event + StateHasChanged).
2. If `includeUnloaded` is true and `LoadChildrenAsync` is set:
   a. Walk the tree depth-first.
   b. For each node where `HasChildren && !Children.Any() && !_loadedNodeIds.Contains(id)` and `depth < maxDepth`:
      - Check `cancellationToken.ThrowIfCancellationRequested()`.
      - Add node ID to `_loadingIds`, call `StateHasChanged()` to show indicator.
      - `await LoadChildrenAsync(node.Item)`.
      - Invalidate `_cachedTree`, mark in `_loadedNodeIds`, remove from `_loadingIds`.
      - Rebuild tree, continue to next level.
   c. After all loads complete, collect all IDs and expand.

### Depth-First Load Helper

```csharp
private async Task LoadUnloadedNodesAsync(
    List<TreeNode> nodes, int currentDepth, int maxDepth,
    CancellationToken cancellationToken)
{
    foreach (var node in nodes)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (node.HasChildren && !node.Children.Any()
            && !_loadedNodeIds.Contains(node.Id)
            && LoadChildrenAsync != null
            && currentDepth < maxDepth)
        {
            _loadingIds.Add(node.Id);
            StateHasChanged();
            try
            {
                var children = await LoadChildrenAsync(node.Item);
                if (children != null)
                    _cachedTree = null;
            }
            finally
            {
                _loadingIds.Remove(node.Id);
                _loadedNodeIds.Add(node.Id);
            }
            // Rebuild tree to pick up new children, then recurse
            var updatedTree = GetTree();
            var updatedNode = FindNode(updatedTree, node.Id);
            if (updatedNode != null)
                await LoadUnloadedNodesAsync(
                    updatedNode.Children, currentDepth + 1, maxDepth, cancellationToken);
        }
        else if (node.Children.Any())
        {
            await LoadUnloadedNodesAsync(
                node.Children, currentDepth + 1, maxDepth, cancellationToken);
        }
    }
}
```

---

## Success Criteria

- [ ] **SC-1**: Calling `ExpandAllAsync()` (no arguments) on a tree with `LoadChildrenAsync` does NOT call `LoadChildrenAsync` and only expands already-loaded nodes (backward compatibility).
- [ ] **SC-2**: Calling `ExpandAllAsync(includeUnloaded: true)` on a tree with lazy nodes triggers `LoadChildrenAsync` for each unloaded node.
- [ ] **SC-3**: After `ExpandAllAsync(includeUnloaded: true)` completes, all nodes (including previously unloaded) are in `_expandedIds` and visible in render output.
- [ ] **SC-4**: `maxDepth` parameter limits how many levels deep lazy loading traverses (e.g., `maxDepth: 1` loads only direct children of root nodes).
- [ ] **SC-5**: `CancellationToken` cancellation stops the loading process and throws `OperationCanceledException`.
- [ ] **SC-6**: `ExpandedItemsChanged` fires with the complete set of IDs after all loading is done.
- [ ] **SC-7**: Existing callers that call `ExpandAllAsync()` without arguments continue to compile and behave identically (no breaking change).

---

## Cross-References

- Intake: `stages/01-intake/output/gap-expandall-lazyload-inventory.md`
- Source: `MariloTreeView.razor.cs` lines 176-183 (current ExpandAllAsync)
- Model: `ToggleNodeAsync` lines 656-681 (correct lazy-load path)
