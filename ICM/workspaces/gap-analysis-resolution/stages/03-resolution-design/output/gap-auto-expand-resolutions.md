# Resolution Records: TreeView — Gap 14 AutoExpand

## Summary

Gap 14 introduced the `AutoExpand` parameter for `MariloTreeView`. When `true`, the component automatically expands all ancestor nodes of any currently-selected item. This fires on initial render and whenever `SelectedItems` changes externally (both handled by `OnParametersSet`). The feature is fully implemented. This document is a retroactive Stage 03 record capturing the design decisions and implementation details.

---

### RES-TV-014: AutoExpand — auto-expand ancestors of selected items

**Resolves:** GAP-14 (no AutoExpand behaviour)
**Status:** Implemented
**Source files:** `src/Marilo.Components/Navigation/MariloTreeView.razor.cs`

---

#### Target Pattern

```razor
<!-- Initial render: node 3 is selected; its ancestors expand automatically -->
<MariloTreeView Data="items"
                IdField="Id"
                TextField="Name"
                ChildrenField="Children"
                SelectedItems="@(new[] { "node-3" })"
                AutoExpand="true" />

<!-- Changing SelectedItems externally re-triggers expansion -->
<MariloTreeView Data="items"
                IdField="Id"
                TextField="Name"
                ChildrenField="Children"
                @bind-SelectedItems="selectedIds"
                AutoExpand="true" />
```

When `AutoExpand` is `false` (the default), the tree renders with the collapsed state determined solely by `ExpandedItems`.

---

#### Implementation Details

**Parameter declaration** (line 89):

```csharp
/// <summary>Automatically expands ancestors of selected items.</summary>
[Parameter] public bool AutoExpand { get; set; }
```

Default: `false` (C# bool default; no explicit initialiser needed).

**OnParametersSet guard** (line 162):

```csharp
if (AutoExpand && _selectedIds.Count > 0 && Data != null)
    ExpandAncestorsOfSelected();
```

The three-part guard ensures the expansion logic is skipped when:
- `AutoExpand` is `false`
- No items are selected (`_selectedIds.Count == 0`)
- `Data` has not yet been provided (`Data == null`)

Note: `_cachedTree = null` is set earlier in `OnParametersSet` (line 160), so `GetTree()` inside `ExpandAncestorsOfSelected` always rebuilds the tree from the current `Data` value rather than a stale cache.

**ExpandAncestorsOfSelected** (line 924):

```csharp
private void ExpandAncestorsOfSelected()
{
    var tree = GetTree();
    foreach (var selectedId in _selectedIds)
    {
        var ancestors = new List<string>();
        CollectAncestorIds(tree, selectedId, ancestors);
        foreach (var ancestorId in ancestors)
            _expandedIds.Add(ancestorId);
    }
}
```

Iterates every selected ID. For each, it collects all ancestor IDs via `CollectAncestorIds`, then adds them to `_expandedIds` (a `HashSet<string>`). Adding to a `HashSet` is idempotent — repeated calls for the same ancestors (e.g., when `SelectedItems` changes to a sibling node under the same parent) do not create duplicates.

**CollectAncestorIds** (line 936):

```csharp
private static bool CollectAncestorIds(List<TreeNode> nodes, string targetId, List<string> ancestors)
{
    foreach (var node in nodes)
    {
        if (node.Id == targetId) return true;
        if (CollectAncestorIds(node.Children, targetId, ancestors))
        {
            ancestors.Add(node.Id);
            return true;
        }
    }
    return false;
}
```

Recursive depth-first walk from root. Returns `true` when the target is found so that each level on the call stack adds its own node ID to `ancestors` before returning. This produces a leaf-to-root ordering in the `ancestors` list, which is immaterial because `_expandedIds` is a `HashSet`.

---

#### Options Considered

**Option A: OnParametersSet (chosen)**
- Approach: Run expansion logic inside `OnParametersSet`, which fires on initial render and on every external parameter update.
- Pros: Covers both initial render and programmatic `SelectedItems` changes with a single code path. No event subscription or disposal required.
- Cons: Runs on every parameter change, not just when `SelectedItems` changes. The guard (`_selectedIds.Count > 0 && Data != null`) keeps this cheap when no selection is present.
- Effort: Low

**Option B: OnAfterRender**
- Approach: Run expansion logic after the first render.
- Pros: Avoids running before DOM is ready.
- Cons: Triggers a second render cycle on initial load. Does not naturally respond to external `SelectedItems` changes without additional state tracking.
- Effort: Medium

**Option C: Explicit public method (ExpandAncestors)**
- Approach: Expose a public `ExpandAncestors()` method that callers invoke manually.
- Pros: Full caller control.
- Cons: Breaks the declarative Blazor model; callers must hold a component reference. Unsuitable as a default auto-expand mechanism.
- Effort: Low (but wrong abstraction level)

---

#### Decision

**Chosen:** Option A — OnParametersSet
**Rationale:** `OnParametersSet` is the standard Blazor lifecycle hook for responding to external parameter changes. It fires on initial render and whenever a parent re-renders with new parameter values, covering both the initial selection and programmatic updates with no extra wiring. The three-part guard keeps the cost negligible when the feature is unused.

---

#### Undocumented Behaviour Found

1. **Cache invalidation sequence:** `_cachedTree = null` is set unconditionally at the top of `OnParametersSet` (before the `AutoExpand` guard). This means `GetTree()` inside `ExpandAncestorsOfSelected` always reflects the current `Data` — including any items added or removed since the last render. This is correct behaviour but is not called out in comments.

2. **Multiple selected items:** When multiple IDs are in `_selectedIds`, ancestors are collected and added for each independently. If two selected nodes share a common ancestor, that ancestor is added to `_expandedIds` twice (once per selected child), but `HashSet<string>.Add` silently ignores duplicates. The end result is correct.

3. **Non-existent selected IDs:** If a value in `_selectedIds` does not exist in the tree (e.g., stale external state), `CollectAncestorIds` returns `false` for all nodes and no ancestors are added. No exception is thrown.

4. **AutoExpand does not collapse:** Setting `AutoExpand=true` only adds to `_expandedIds`; it never removes entries. If a user manually expands a node and then the selection changes to a completely different subtree, the previously-expanded node remains expanded. AutoExpand is additive, not declarative.

---

#### Success Criteria

- [ ] AutoExpand=true expands ancestors of selected items on initial render (unit test)
- [ ] AutoExpand=true expands ancestors when SelectedItems changes externally (unit test)
- [ ] AutoExpand=false does not auto-expand ancestors (unit test)
- [ ] AutoExpand defaults to false (unit test)
- [ ] AutoExpand only runs when Data is not null (unit test)
- [ ] AutoExpand only runs when at least one item is selected (unit test)
- [ ] Multiple selected items each expand their own ancestor chains (unit test)
- [ ] Non-existent selected IDs do not throw (unit test)
- [ ] AutoExpand is additive: previously expanded nodes are not collapsed when selection changes (unit test)
