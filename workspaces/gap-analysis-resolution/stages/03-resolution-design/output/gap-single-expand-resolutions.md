# Resolution Records: MariloTreeView — Gap 13: SingleExpand (Accordion Mode)

## Summary

Gap 13 adds accordion-style expand behaviour to MariloTreeView. When `SingleExpand=true`, expanding a node automatically collapses all sibling nodes (nodes at the same level sharing the same parent). The feature is implemented as a lightweight guard inside `ToggleNodeAsync` and a recursive sibling-lookup helper. No new types were required.

**Status:** IMPLEMENTED — retroactive Stage 03 record.
**Validation stage:** `stages/06-validate/output/gap-treeview-closure-report.md`

---

### RES-TREEVIEW-013: SingleExpand — accordion collapse of siblings on expand

**Resolves:** Gap 13 (SingleExpand accordion mode)
**Status:** Implemented
**Component:** `MariloTreeView` (`src/Marilo.Components/Navigation/MariloTreeView.razor.cs`)

---

#### Target Pattern

```razor
<MariloTreeView Items="@_items"
                ItemTextBinding="@(x => ((MyNode)x).Label)"
                ItemChildrenBinding="@(x => ((MyNode)x).Children)"
                SingleExpand="true" />
```

When `SingleExpand="true"`, expanding any node collapses every sibling node that was previously expanded. Non-sibling nodes (cousins, ancestors, nodes in other branches) are unaffected. The default is `false`, restoring the standard multi-expand behaviour.

---

#### Implementation

**Parameter declaration (line 86):**

```csharp
[Parameter] public bool SingleExpand { get; set; }
```

Default: `false` (standard multi-expand).

**ToggleNodeAsync guard (lines 633–688):**

```csharp
private async Task ToggleNodeAsync(string id)
{
    if (Disabled || ReadOnly) return;

    var wasExpanded = _expandedIds.Contains(id);

    if (wasExpanded)
    {
        _expandedIds.Remove(id);                     // collapse branch — no sibling logic
    }
    else
    {
        if (SingleExpand)
        {
            var tree = GetTree();
            var siblingIds = FindSiblingIds(tree, id);
            foreach (var sibId in siblingIds)
                _expandedIds.Remove(sibId);          // collapse all siblings
        }

        _expandedIds.Add(id);                        // expand target node
        // ... lazy load, etc.
    }

    if (ExpandedItemsChanged.HasDelegate)
        await ExpandedItemsChanged.InvokeAsync(_expandedIds.ToList());

    StateHasChanged();
}
```

**FindSiblingIds helper (lines 862–876):**

```csharp
private static List<string> FindSiblingIds(List<TreeNode> tree, string nodeId)
{
    var siblings = FindSiblingList(tree, nodeId);
    return siblings?.Where(n => n.Id != nodeId).Select(n => n.Id).ToList()
           ?? new List<string>();
}

private static List<TreeNode>? FindSiblingList(List<TreeNode> nodes, string nodeId)
{
    foreach (var node in nodes)
    {
        if (node.Id == nodeId) return nodes;          // this list is the sibling list
        var found = FindSiblingList(node.Children, nodeId);
        if (found != null) return found;
    }
    return null;
}
```

The helper walks the tree recursively. When it finds the target node it returns the containing list — every other node in that list is a sibling. The target node itself is excluded via the `Where` clause.

---

#### Behaviour Specification

| Scenario | Expected Result |
|----------|----------------|
| `SingleExpand=false` (default), expand multiple siblings | All siblings remain expanded |
| `SingleExpand=true`, expand a node | All previously expanded siblings collapse; target expands |
| `SingleExpand=true`, collapse a node | Only the target collapses; no sibling side-effects |
| `SingleExpand=true`, expand a node with no expanded siblings | Target expands; no-op on siblings |
| `SingleExpand=true`, expand a node in branch A | Nodes in branch B (non-siblings) remain unchanged |
| `SingleExpand=true`, tree is `Disabled` or `ReadOnly` | Early return; no expansion, no sibling collapse |
| `SingleExpand=true`, expand triggers `ExpandedItemsChanged` | Event fires with the post-collapse, post-expand `_expandedIds` list |

---

#### Options Considered

**Option A (chosen): Guard inside ToggleNodeAsync, sibling lookup via recursive helper**
- Approach: Check `SingleExpand` only in the expand branch. Find siblings by locating the containing list in the tree structure. Remove sibling IDs from `_expandedIds` before adding the new ID.
- Pros: Minimal surface area. Collapse path is completely unaffected. No change to data model. `ExpandedItemsChanged` fires once with the fully updated set.
- Cons: None significant at this scale.
- Effort: Small.

**Option B: Sibling lookup via parent reference stored on TreeNode**
- Approach: Add `ParentId` to `TreeNode`; find siblings via linear scan of parent's children.
- Pros: O(1) parent lookup.
- Cons: Increases TreeNode model size; requires propagating parent during tree build. Not worth the complexity for typical tree sizes.
- Effort: Medium.

**Option B was rejected.** The recursive walk (Option A) is sufficient for realistic tree depths and keeps the TreeNode model lean.

---

#### Undocumented Behaviour Found

1. **`ExpandedItemsChanged` fires the fully reconciled set.** The event is invoked at the end of `ToggleNodeAsync` after sibling removal and target insertion (line 684). A caller using `@bind-ExpandedItems` will therefore receive a list that no longer contains collapsed siblings — there is no separate "siblings collapsed" event. This is correct but worth noting for consumers who mirror `ExpandedItems` into external state.

2. **Collapse path does not apply sibling logic.** When `wasExpanded == true` (line 639), the method only removes the target ID and returns. `SingleExpand` is not checked, and no siblings are modified. This means re-collapsing a node in accordion mode does not re-expand anything — the tree simply becomes fully collapsed at that level. This is the expected accordion contract but is not stated explicitly anywhere.

3. **`FindSiblingIds` is also called from the keyboard navigation handler** (line 801, `*` key expand-siblings shortcut). The two call sites share the same helper but have independent semantics — the keyboard handler expands all siblings while `SingleExpand` collapses them. No conflict exists because they are in separate code paths, but the dual use of `FindSiblingIds` is worth documenting.

4. **`SingleExpand` interacts with `AutoExpand`.** When `AutoExpand=true`, ancestors of selected nodes are expanded during `OnParametersSet` via `ExpandAncestorsOfSelected()`. That path writes directly to `_expandedIds` without invoking `ToggleNodeAsync`, so the `SingleExpand` guard is bypassed. Ancestor expansion triggered by `AutoExpand` will never collapse siblings. This is likely intentional (auto-expansion serves navigation, not accordion UX) but is an undocumented interaction.

---

#### Test Plan

The following test cases satisfy the five stated success criteria and cover undocumented behaviour.

| # | Test | Criterion |
|---|------|-----------|
| T1 | `SingleExpand=true`: expand Node B after Node A expanded at same level → `_expandedIds` contains B, does not contain A | Criterion 1 |
| T2 | `SingleExpand=true`: expand Node B (level 1) after Node C (level 2, different parent) expanded → both A-subtree node and B remain expanded | Criterion 2 |
| T3 | `SingleExpand=false`: expand Node A then Node B at same level → both remain expanded | Criterion 3 |
| T4 | Instantiate `MariloTreeView` with no parameters → `SingleExpand` is `false` | Criterion 4 |
| T5 | `SingleExpand=true`: expand Node B → `ExpandedItemsChanged` fires; received list contains B and does not contain former sibling A | Criterion 5 |
| T6 | `SingleExpand=true`: collapse already-expanded Node A → only A removed; siblings unaffected | Undocumented behaviour 2 |
| T7 | `SingleExpand=true`, `Disabled=true`: attempt expand → `ExpandedItemsChanged` does not fire; `_expandedIds` unchanged | Implementation guard |
| T8 | `SingleExpand=true`: expand a root-level node that has no expanded siblings → expands cleanly; no exception | Edge case |

---

#### References

- `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` — lines 86, 633–688, 862–876
- `src/Marilo.Components/Navigation/resolution/RESOLUTION_STATUS.md` — Gap 13 row
- `workspaces/gap-analysis-resolution/stages/06-validate/output/gap-treeview-closure-report.md` — validation record
