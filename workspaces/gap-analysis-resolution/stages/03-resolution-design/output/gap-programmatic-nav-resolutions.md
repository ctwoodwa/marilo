# Resolution Records: MariloTreeView — Programmatic Navigation (SelectNodeAsync)

## Summary

Gap 19 covers programmatic navigation to a tree node by ID: expanding all ancestors, selecting the target node, and setting keyboard focus — all in a single awaitable call. `SelectNodeAsync` is fully implemented as a public async method on `MariloTreeView`. This record is reconstructed retroactively; the implementation predates the record.

---

### RES-TREEVIEW-019: SelectNodeAsync public API method

**Resolves:** GAP-19 (programmatic-nav — SelectNodeAsync)
**Status:** Reconstructed — implementation predates this record

#### Problem Statement

Tree views in document browsers, file explorers, and deep-linked navigation contexts need to be driven externally: given a node ID, the tree must reveal the node (by expanding its ancestors), select it, and position keyboard focus on it, without the user having to manually expand the path. Three distinct operations must happen atomically from the caller's perspective:

1. Expand every ancestor of the target node so it is visible.
2. Replace the current selection with only the target node.
3. Set the internal focused-node ID so keyboard navigation starts from that node.

Both `ExpandedItemsChanged` and `SelectedItemsChanged` event callbacks must fire so that consumers using two-way binding (`@bind-ExpandedItems`, `@bind-SelectedItems`) stay in sync. If the requested ID does not exist in the tree, the method must return without mutating any state.

#### Options Considered

**Option A (Selected): Single public async method on the component, accessed via @ref**

- Approach: Expose `public async Task SelectNodeAsync(string id)` on `MariloTreeView`. The method calls the existing private helpers `FindNode` and `CollectAncestorIds`, mutates `_expandedIds` and `_selectedIds` in-place, sets `_focusedNodeId`, fires both event callbacks if they have delegates, invalidates the render cache (`_cachedTree = null`), and calls `StateHasChanged()`.
- The guard `if (node == null) return;` ensures the method is a safe no-op for unknown IDs.
- `SelectedItemsChanged` is always awaited (no `HasDelegate` guard) because selection change is always a meaningful event; `ExpandedItemsChanged` is guarded with `HasDelegate` to mirror the pattern used in `ExpandAllAsync` / `CollapseAllAsync`.
- Pros: Single call point; caller does not need to know tree internals; reuses proven private helpers; works with both bound and unbound state; naturally awaitable for caller sequencing.
- Cons: Requires the caller to hold a `@ref` to the component instance; if the tree is in a deeply nested layout the `@ref` wiring can be verbose.
- Effort: Small

**Option B (Not chosen): Cascade the target ID through a parameter and react in OnParametersSet**

- Approach: Expose a `NavigateTo` parameter; when it changes, `OnParametersSet` would perform the same ancestor-expand and select logic.
- Pros: No `@ref` required; parameter-driven.
- Cons: Requires additional nullable parameter with change-detection logic; the parameter would need to be cleared by the parent after navigation (otherwise navigating to the same node a second time would not re-fire); awkward two-round-trip pattern for what is conceptually a single imperative action; harder to await.
- Effort: Medium (with non-obvious reset semantics)

#### Decision

**Chosen:** Option A
**Rationale:** Programmatic navigation is an imperative, one-shot action. An `async Task` method on the component instance is the idiomatic Blazor pattern for this (same as `MudAutocomplete.SelectAsync`, `MudTable.ReloadServerData`, etc.). The `@ref` requirement is standard and well-understood. Option B would introduce parameter lifecycle complexity and surprising reset semantics.

#### Target Pattern

```razor
@* Obtain a reference to the tree *@
<MariloTreeView @ref="_tree"
                Nodes="@nodes"
                @bind-SelectedItems="_selected"
                @bind-ExpandedItems="_expanded" />

<button @onclick="NavigateToNode">Go to node</button>

@code {
    private MariloTreeView _tree = default!;
    private List<string> _selected = new();
    private List<string> _expanded = new();

    private async Task NavigateToNode()
    {
        await _tree.SelectNodeAsync("some-deep-node-id");
    }
}
```

Method signature (from `MariloTreeView.razor.cs`, line 202):

```csharp
/// <summary>Programmatically navigates to a node: expands all ancestors, selects it, and sets focus.</summary>
public async Task SelectNodeAsync(string id)
```

Full implementation (from `MariloTreeView.razor.cs`, lines 202–225):

```csharp
public async Task SelectNodeAsync(string id)
{
    var tree = GetTree();
    var node = FindNode(tree, id);
    if (node == null) return;

    var ancestors = new List<string>();
    CollectAncestorIds(tree, id, ancestors);
    foreach (var ancestorId in ancestors)
        _expandedIds.Add(ancestorId);

    if (ExpandedItemsChanged.HasDelegate)
        await ExpandedItemsChanged.InvokeAsync(_expandedIds.ToList());

    _selectedIds.Clear();
    _selectedIds.Add(id);
    _focusedNodeId = id;
    await SelectedItemsChanged.InvokeAsync(_selectedIds.ToList());

    _cachedTree = null;
    StateHasChanged();
}
```

#### Consequences

- Calling `SelectNodeAsync` with an ID that does not exist in the current tree is a safe no-op: no state is mutated, no events are fired, no exception is thrown.
- `ExpandedItemsChanged` is only fired when a delegate is bound (`HasDelegate` guard); `SelectedItemsChanged` is always fired — this asymmetry mirrors the convention established in `ExpandAllAsync` and `CollapseAllAsync`.
- The method replaces the entire selection (`_selectedIds.Clear()` then `_selectedIds.Add(id)`); it does not extend a multi-select. Consumers that need to preserve existing selections must manage `SelectedItems` externally.
- Ancestor expansion is additive: any nodes already in `_expandedIds` before the call remain expanded after it.
- `_cachedTree = null` ensures the next render recomputes tree layout with the newly expanded ancestors visible.
- If the tree has `Disabled=true` or `ReadOnly=true` there is no guard inside `SelectNodeAsync`; those parameters affect user interaction only. Programmatic navigation works regardless of disabled/read-only state.

#### Success Criteria

- [ ] `SelectNodeAsync` expands ancestors of target node (unit test)
- [ ] `SelectNodeAsync` selects the target node (unit test)
- [ ] `SelectNodeAsync` sets keyboard focus to target node (unit test)
- [ ] `SelectNodeAsync` fires `ExpandedItemsChanged` (unit test)
- [ ] `SelectNodeAsync` fires `SelectedItemsChanged` (unit test)
- [ ] `SelectNodeAsync` returns silently for non-existent node ID (unit test)
- [ ] `SelectNodeAsync` is publicly accessible via `@ref` (unit test)

<!-- Reconstructed retroactively — implementation predates this record -->
