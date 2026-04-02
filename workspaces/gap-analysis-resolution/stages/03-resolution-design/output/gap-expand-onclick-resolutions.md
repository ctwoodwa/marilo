# Resolution Records: MariloTreeView — ExpandOnClick / ExpandOnDoubleClick

## Summary

Gap 12 covers interactive expand/collapse behaviour triggered by pointer events on a tree node header. Both `ExpandOnClick` and `ExpandOnDoubleClick` are fully implemented. This record is reconstructed retroactively; the implementation predates the record.

---

### RES-TREEVIEW-012: ExpandOnClick and ExpandOnDoubleClick parameters

**Resolves:** GAP-12 (ExpandOnClick / ExpandOnDoubleClick interactive expand)
**Status:** Reconstructed — implementation predates this record

#### Problem Statement

By default, MariloTreeView nodes expand and collapse only via the dedicated chevron/toggle icon. Two optional interaction modes were required:

1. **ExpandOnClick** — clicking anywhere on the node header row expands or collapses a parent node.
2. **ExpandOnDoubleClick** — double-clicking on the header row expands or collapses a parent node.

Both modes must be inert when the node is disabled. Additionally, `ExpandOnDoubleClick` must be suppressed when `AllowEditing` is true, because double-click is already claimed by the inline-edit activation path in that mode.

#### Options Considered

**Option A (Selected): Event attributes injected during render, guarded by parameter flags**

- Approach: During `RenderNodes`, after confirming the node has children (`hasKids`) and the tree is not disabled (`!Disabled`), conditionally add `onclick` and `ondblclick` event attributes that call the existing `ToggleNodeAsync` method.
- The `ExpandOnDoubleClick` attribute is further suppressed when `AllowEditing` is true, preventing a conflict with double-click-to-edit.
- Both parameters default to `false`, so there is no behaviour change for existing consumers.
- Pros: Reuses the existing `ToggleNodeAsync` logic; no new state; minimal render cost (attributes are only emitted when needed); conflict with AllowEditing is resolved at render time.
- Cons: Click and double-click are independent; a consumer could set both `true` simultaneously, which is technically valid but unusual.
- Effort: Small

**Option B (Not chosen): Expand on click as the default, with an opt-out parameter**

- Approach: Nodes would expand on click by default; a `ClickToExpand=false` parameter would restore icon-only expansion.
- Pros: Mirrors common tree-widget conventions.
- Cons: Breaking change for existing consumers relying on icon-only expansion; not aligned with the documented gap requirement that the feature should be opt-in.
- Effort: Small (but with migration burden)

#### Decision

**Chosen:** Option A
**Rationale:** Opt-in via boolean parameters is the lowest-friction approach and avoids breaking existing usage. Guarding at render time keeps the conflict-resolution logic close to the attribute emission, making it easy to audit.

#### Target Pattern

```razor
<MariloTreeView Nodes="@nodes"
                ExpandOnClick="true" />

<MariloTreeView Nodes="@nodes"
                ExpandOnDoubleClick="true" />

<!-- AllowEditing suppresses ExpandOnDoubleClick automatically -->
<MariloTreeView Nodes="@nodes"
                AllowEditing="true"
                ExpandOnDoubleClick="true" />
```

Parameter signatures (from `MariloTreeView.razor.cs`):

```csharp
// Line 80
[Parameter] public bool ExpandOnClick { get; set; }

// Line 83
[Parameter] public bool ExpandOnDoubleClick { get; set; }
```

Render-time guard (from `MariloTreeView.razor.cs`, lines 500–508):

```csharp
if (hasKids && !Disabled)
{
    var clickNodeId = node.Id;
    if (ExpandOnClick)
        builder.AddAttribute(20, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, () => ToggleNodeAsync(clickNodeId)));
    if (ExpandOnDoubleClick && !AllowEditing)
        builder.AddAttribute(21, "ondblclick", EventCallback.Factory.Create<MouseEventArgs>(this, () => ToggleNodeAsync(clickNodeId)));
}
```

#### Consequences

- No breaking change: both parameters default to `false`.
- `ExpandOnDoubleClick` is silently suppressed (not an error) when `AllowEditing=true`. Consumers should not rely on `ondblclick` for expand when editing is enabled.
- Leaf nodes (nodes without children) are unaffected: the guard `hasKids` prevents unnecessary event attributes on them.
- Setting both `ExpandOnClick` and `ExpandOnDoubleClick` to `true` simultaneously is supported; click fires `ToggleNodeAsync` and double-click also fires it (browser behaviour means a double-click generates two `onclick` events followed by one `ondblclick` — consumers combining both modes should be aware of this).

#### Success Criteria

- [ ] `ExpandOnClick=true` triggers `ToggleNodeAsync` on header click for a parent node (unit test)
- [ ] `ExpandOnClick=false` does NOT attach an `onclick` handler to the node header (unit test)
- [ ] `ExpandOnDoubleClick=true` triggers `ToggleNodeAsync` on header double-click (unit test)
- [ ] `ExpandOnDoubleClick=true` combined with `AllowEditing=true` does NOT attach an `ondblclick` handler (unit test)
- [ ] Both `ExpandOnClick` and `ExpandOnDoubleClick` default to `false` (unit test)
- [ ] `Disabled=true` prevents `ExpandOnClick` from attaching an `onclick` handler regardless of parameter value (unit test)
- [ ] Leaf nodes (no children) never receive `onclick` or `ondblclick` expand handlers (unit test)

<!-- Reconstructed retroactively — implementation predates this record -->
