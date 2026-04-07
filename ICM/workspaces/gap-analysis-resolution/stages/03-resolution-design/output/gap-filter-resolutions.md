# Resolution Records: MariloTreeView — Gap 16 FilterFunc / Search

## Summary

Gap 16 covers filtering support for `MariloTreeView`. The feature is **already implemented** following Fancytree's "keep ancestors visible" pattern. When a `FilterFunc` predicate is active, `BuildTree()` passes the constructed node tree through `ApplyFilter()`, which recursively prunes non-matching nodes while retaining any ancestor whose subtree contains at least one match. Matching nodes receive the `mar-tree-item--filter-match` CSS class for visual highlighting. `ClearFilter()` invalidates the cached tree and triggers a re-render without the filter applied.

---

### RES-FILTER-001: FilterFunc predicate with ancestor-preserving recursive pruning

**Resolves:** GAP-16 (no FilterFunc / Search support)
**Status:** Implemented
**Source files:**
- `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` — lines 141, 195–199, 361–362, 435–448, 466

#### Implemented Pattern

```csharp
// Parameter declaration (line 141)
[Parameter] public Func<object, bool>? FilterFunc { get; set; }

// Applied after BuildTree (line 361)
if (FilterFunc != null)
    tree = ApplyFilter(tree);

// Recursive pruning — keep node if it matches OR has matching descendants (lines 435–448)
private List<TreeNode> ApplyFilter(List<TreeNode> nodes)
{
    var result = new List<TreeNode>();
    foreach (var node in nodes)
    {
        var filteredChildren = ApplyFilter(node.Children);
        var matches = FilterFunc!(node.Item);
        if (matches || filteredChildren.Count > 0)
            result.Add(node with { Children = filteredChildren });
    }
    return result;
}

// CSS class applied in RenderNodes (line 466)
+ (FilterFunc != null && FilterFunc(node.Item) ? " mar-tree-item--filter-match" : "")

// Public reset method (lines 195–199)
public void ClearFilter()
{
    _cachedTree = null;
    StateHasChanged();
}
```

#### Design Decision

**Chosen approach:** Recursive subtree-pruning at `BuildTree()` time, with CSS class applied during render.

**Rationale:** Filtering at tree-build time (rather than at render time with a visibility flag) means the pruned tree is small — `RenderNodes` never visits hidden nodes at all. The `_cachedTree` field memoises the result so the predicate is not evaluated on every Blazor render cycle; the cache is invalidated whenever parameters change (`OnParametersSet` always sets `_cachedTree = null`) or when `ClearFilter()` is called explicitly.

The ancestor-preservation rule (keep any node whose children list becomes non-empty after filtering) is achieved by evaluating children before the parent node: `filteredChildren` is computed first, and the parent is retained when `filteredChildren.Count > 0` even if the parent itself does not match the predicate. This is the Fancytree pattern referenced in IMPLEMENTATION_NOTES.md §6.

#### Options Considered

**Option A (chosen): Recursive pruning at build time**
- Pros: Pruned tree is minimal; RenderNodes visits no hidden nodes; memoised via `_cachedTree`
- Cons: Filter result is tied to the cached tree lifetime; any parameter change rebuilds the full tree (acceptable — `OnParametersSet` already invalidates the cache for all other reasons)

**Option B: Visibility flag per node, hidden at render time**
- Pros: Nodes stay in the tree; could animate show/hide
- Cons: RenderNodes must evaluate every node; increases DOM size; more complex CSS management
- Not chosen

#### Undocumented Behaviours Found

1. **Parameter-change resets the filter automatically.** `OnParametersSet` unconditionally sets `_cachedTree = null` (line 160). A caller who sets `FilterFunc = null` from the outside does not need to call `ClearFilter()` — the parameter change alone triggers a full tree rebuild. `ClearFilter()` is therefore only necessary when the filter must be cleared without a parameter change (e.g., from imperative code that sets a backing field directly without reassigning `FilterFunc`).

2. **FilterFunc is evaluated twice per visible node during render.** `RenderNodes` calls `FilterFunc(node.Item)` to assign the CSS class (line 466) after `ApplyFilter` already called it during tree build (line 441). Nodes that survive the filter are evaluated a second time. For expensive predicates this is a latent performance concern.

3. **Non-leaf ancestors that do not match are still rendered without `mar-tree-item--filter-match`.** The CSS class is applied only to nodes for which `FilterFunc` returns `true` (line 466). An ancestor retained purely because a descendant matched receives no visual distinction from normal (unfiltered) nodes. This is intentional by design but is not stated explicitly in the implementation notes.

4. **Filtering does not affect expanded state.** When a filter is active, collapsed ancestor nodes are retained in the tree structure but their `_expandedIds` membership is unchanged. If a parent node was collapsed before filtering, its matching children are present in the pruned tree but will not be visible until the parent is expanded. Callers must expand ancestors manually (or set `AutoExpand`) to guarantee visibility of all matches.

#### Success Criteria

- [ ] FilterFunc hides non-matching leaf nodes — leaf nodes whose items return `false` from the predicate must not appear in the rendered output (unit test)
- [ ] Ancestors of matching nodes remain visible — a parent node must be retained in the rendered output when at least one descendant matches, even if the parent itself does not match (unit test)
- [ ] Matching nodes receive `mar-tree-item--filter-match` CSS class — nodes whose items return `true` from the predicate must carry this class in the rendered `<li>` element (unit test)
- [ ] Non-matching ancestors do NOT receive `mar-tree-item--filter-match` CSS class — ancestor nodes retained only because a descendant matched must not carry the filter-match class (unit test)
- [ ] `ClearFilter()` restores all nodes — after `ClearFilter()` is called, the full unfiltered tree is rendered and the filter-match class is absent from all nodes (unit test)
- [ ] `FilterFunc = null` shows all nodes (default behaviour) — when no `FilterFunc` is provided the complete tree is rendered without any filter-match classes (unit test)
