# Resolution Records: MariloTreeView — ExpandAll / CollapseAll (Gap 15)

## Summary

Gap 15 identified that MariloTreeView lacked public programmatic methods to expand or collapse all nodes at once.
Resolved by adding two `public async Task` methods — `ExpandAllAsync` and `CollapseAllAsync` — accessible via
Blazor `@ref`. Both methods mutate the internal `_expandedIds` HashSet and fire `ExpandedItemsChanged` when a
delegate is bound, keeping two-way binding state in sync.

---

### RES-BATCH-EXPAND-001: Add ExpandAllAsync and CollapseAllAsync public methods

**Resolves:** GAP-15 (no ExpandAll / CollapseAll)
**Status:** Implemented
**File:** `src/Marilo.Components/Navigation/MariloTreeView.razor.cs`

#### Target Pattern

```razor
@* Caller obtains a component reference *@
<MariloTreeView @ref="_tree" Data="@_items" ExpandedItemsChanged="@OnExpandedChanged" />

@code {
    private MariloTreeView _tree = default!;

    async Task ExpandAll()  => await _tree.ExpandAllAsync();
    async Task CollapseAll() => await _tree.CollapseAllAsync();

    void OnExpandedChanged(IEnumerable<string> ids)
    {
        // fires on both expand-all and collapse-all
    }
}
```

#### Implementation (as built)

```csharp
/// <summary>Expands all nodes in the tree.</summary>
public async Task ExpandAllAsync()
{
    var tree = GetTree();
    CollectAllIds(tree, _expandedIds);
    if (ExpandedItemsChanged.HasDelegate)
        await ExpandedItemsChanged.InvokeAsync(_expandedIds.ToList());
    StateHasChanged();
}

/// <summary>Collapses all nodes in the tree.</summary>
public async Task CollapseAllAsync()
{
    _expandedIds.Clear();
    if (ExpandedItemsChanged.HasDelegate)
        await ExpandedItemsChanged.InvokeAsync(_expandedIds.ToList());
    StateHasChanged();
}
```

#### Options Considered

**Option A: Public async Task methods with @ref access (chosen)**
- Approach: Methods are `public async Task` on the partial class. Callers bind via `@ref` and call imperatively.
  `ExpandAllAsync` walks the cached tree via `GetTree()`, adding every node ID to `_expandedIds` using
  `CollectAllIds`. `CollapseAllAsync` calls `_expandedIds.Clear()`. Both fire `ExpandedItemsChanged` (guarded
  by `HasDelegate`) then call `StateHasChanged()`.
- Pros: Consistent with Blazor component ref pattern. No new parameters. Works with existing two-way binding.
  Collapse is O(1) (HashSet.Clear) rather than O(n).
- Cons: Cannot be called from markup directly — requires `@ref`.
- Effort: Small
- Decision: Chosen. Lowest API surface, consistent with existing public methods (`Rebind`, `ClearFilter`).

**Option B: Parameter-driven (ExpandAll="true" / CollapseAll="true")**
- Approach: Boolean parameters trigger expansion/collapse during `OnParametersSet`.
- Pros: No `@ref` required.
- Cons: Two-way binding on a boolean trigger is awkward; caller must reset the flag. Adds render-cycle coupling.
- Effort: Small
- Decision: Rejected.

#### Decisions

| # | Decision | Rationale |
|---|----------|-----------|
| 1 | Both methods are `public async Task` | Consistent with Blazor async component API conventions |
| 2 | `ExpandAllAsync` uses `CollectAllIds` overload accepting `HashSet<string>` | Direct mutation avoids intermediate allocation; HashSet deduplication is safe |
| 3 | `ExpandedItemsChanged` guarded by `HasDelegate` | Prevents NullReferenceException when caller has not bound the callback |
| 4 | `CollapseAllAsync` calls `_expandedIds.Clear()` | Clears all expanded state in O(1); does not selectively remove nodes |
| 5 | `StateHasChanged()` called unconditionally after mutation | Ensures render even if `ExpandedItemsChanged` is not bound |

---

## Success Criteria

| # | Criterion | Type |
|---|-----------|------|
| 1 | `ExpandAllAsync()` expands every node in the tree | Unit test |
| 2 | `CollapseAllAsync()` collapses every node in the tree | Unit test |
| 3 | `ExpandAllAsync()` fires `ExpandedItemsChanged` with all node IDs | Unit test |
| 4 | `CollapseAllAsync()` fires `ExpandedItemsChanged` with an empty collection | Unit test |
| 5 | Both methods are publicly accessible via `@ref` | Unit test |

---

## Test Plan

```
ExpandAllAsync_ExpandsEveryNodeInTree
    Given a MariloTreeView with a multi-level data set
    When ExpandAllAsync() is called via @ref
    Then every node ID is present in ExpandedItems

CollapseAllAsync_CollapsesEveryNodeInTree
    Given a MariloTreeView with some nodes already expanded
    When CollapseAllAsync() is called via @ref
    Then ExpandedItems is empty

ExpandAllAsync_FiresExpandedItemsChangedWithAllIds
    Given a MariloTreeView with ExpandedItemsChanged bound
    When ExpandAllAsync() is called
    Then ExpandedItemsChanged fires once with a collection containing all node IDs

CollapseAllAsync_FiresExpandedItemsChangedWithEmptyCollection
    Given a MariloTreeView with ExpandedItemsChanged bound and some nodes expanded
    When CollapseAllAsync() is called
    Then ExpandedItemsChanged fires once with an empty collection

BothMethods_ArePubliclyAccessibleViaRef
    Given a MariloTreeView bound with @ref
    Then ExpandAllAsync and CollapseAllAsync are callable without reflection
```

---

## Undocumented Behaviour

| # | Observation | Impact |
|---|-------------|--------|
| 1 | `CollectAllIds` has two overloads: one accepting `ICollection<string>` (used by checkbox tri-state logic) and one accepting `HashSet<string>` (used by `ExpandAllAsync`). The `HashSet` overload is called by `ExpandAllAsync` — duplicate IDs are silently deduplicated, which is correct but not stated in any doc comment. | Low — correct behaviour, but the overload split may surprise maintainers. |
| 2 | `GetTree()` returns the **cached** tree of already-loaded nodes. For trees using `LoadChildrenAsync` (lazy loading), nodes whose children have not yet been fetched are omitted from `_cachedTree.Children`. Calling `ExpandAllAsync()` on a lazy-loaded tree will only expand nodes that have been previously loaded — it does not trigger lazy-load fetches for unloaded subtrees. | Medium — callers expecting all nodes (including unloaded ones) to expand will see incomplete results. No warning is issued. |
| 3 | `ExpandAllAsync` **adds** to the existing `_expandedIds` set rather than replacing it. If called when `ExpandedItems` parameter is stale (e.g., not re-bound after a data change), previously held IDs from the old binding remain in the set after the call. `CollapseAllAsync` is not affected — it calls `Clear()`. | Low — only relevant when `ExpandedItems` two-way binding is not used and state is managed externally. |
