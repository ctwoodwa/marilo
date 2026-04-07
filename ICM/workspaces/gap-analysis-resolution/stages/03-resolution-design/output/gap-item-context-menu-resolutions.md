# Resolution Records: MariloTreeView — Item Context Menu (OnItemContextMenu)

## Summary

Gap 20 covers right-click context menu support on individual tree nodes. `OnItemContextMenu` is fully implemented as a component-level `EventCallback<TreeItemContextMenuEventArgs>` parameter whose handler is attached per-node during render, with `preventDefault` applied to suppress the browser's native context menu. This record is reconstructed retroactively; the implementation predates the record.

---

### RES-TREEVIEW-020: OnItemContextMenu event parameter

**Resolves:** GAP-20 (item-context-menu — OnItemContextMenu event)
**Status:** Reconstructed — implementation predates this record

#### Problem Statement

Tree views used in file explorers, resource managers, and administrative UIs require a right-click context menu on individual nodes to expose per-item actions (rename, delete, copy path, etc.). Three requirements must be met simultaneously:

1. The consumer must receive the data item and its ID so it can route actions without inspecting the DOM.
2. The consumer must receive the raw `MouseEventArgs` to support context-menu positioning (pointer coordinates).
3. The browser's native context menu must be suppressed when a handler is bound, so custom menus rendered by the consumer are not obscured.

When no handler is bound the component must emit no `oncontextmenu` attribute at all, so the browser behaves normally and there is no unnecessary event overhead.

#### Options Considered

**Option A (Selected): EventCallback parameter with HasDelegate guard and per-node preventDefault**

- Approach: Declare `[Parameter] public EventCallback<TreeItemContextMenuEventArgs> OnItemContextMenu { get; set; }` on the component. During `RenderNodes`, after building each node's header `div`, check `OnItemContextMenu.HasDelegate`. If true, add an `oncontextmenu` attribute that creates an `EventCallback<MouseEventArgs>` capturing the current `node` via a local variable (`ctxNode`), invokes `OnItemContextMenu` with a new `TreeItemContextMenuEventArgs` instance, and immediately follow with `AddEventPreventDefaultAttribute` for `oncontextmenu`.
- `TreeItemContextMenuEventArgs` carries `Item` (the raw data object), `ItemId` (the node's string ID), and `MouseEventArgs` (pointer position and button info).
- The local variable capture (`var ctxNode = node`) is necessary to avoid the classic loop-closure capture bug.
- Pros: Zero cost when no handler is bound; `preventDefault` is scoped to the exact element and event so it does not affect sibling events; event args model is minimal and covers the primary use cases; no JavaScript interop required for the event itself.
- Cons: Consumers wanting a fully custom popup must handle positioning themselves using `MouseEventArgs.ClientX`/`ClientY`; the API does not provide a built-in context menu renderer.
- Effort: Small

**Option B (Not chosen): JavaScript interop with a global document contextmenu listener**

- Approach: Register a global JS listener that intercepts right-click on any tree node, prevents default, and invokes a .NET method via `DotNetObjectReference`.
- Pros: Centralised; could support delegated event listening.
- Cons: Requires JS file distribution and registration; lifecycle coupling between .NET and JS is complex; `preventDefault` at the document level is broader than needed and can interfere with other components on the page; not tree-instance-scoped without additional ID routing.
- Effort: High

#### Decision

**Chosen:** Option A
**Rationale:** Blazor's `EventCallback` and `AddEventPreventDefaultAttribute` APIs provide everything needed without any JavaScript. Attaching the handler only when `HasDelegate` is true keeps the rendered output clean for consumers that do not need context menus. The local capture pattern (`var ctxNode = node`) is already used throughout `RenderNodes` for other per-node event closures, making this consistent with the existing render code style.

#### Target Pattern

```razor
<MariloTreeView Nodes="@nodes"
                OnItemContextMenu="HandleContextMenu" />

@if (_menuVisible)
{
    <div class="context-menu" style="left:@_x px; top:@_y px;">
        <button @onclick="RenameItem">Rename</button>
        <button @onclick="DeleteItem">Delete</button>
    </div>
}

@code {
    private bool _menuVisible;
    private double _x, _y;
    private object? _contextItem;

    private void HandleContextMenu(TreeItemContextMenuEventArgs args)
    {
        _contextItem = args.Item;
        _x = args.MouseEventArgs.ClientX;
        _y = args.MouseEventArgs.ClientY;
        _menuVisible = true;
    }
}
```

Parameter declaration (from `MariloTreeView.razor.cs`, line 123):

```csharp
/// <summary>Fires when a tree item is right-clicked. Provides the item and mouse event args.</summary>
[Parameter] public EventCallback<TreeItemContextMenuEventArgs> OnItemContextMenu { get; set; }
```

Event args model (from `Marilo.Core/Models/TreeViewModels.cs`, lines 8–18):

```csharp
public class TreeItemContextMenuEventArgs
{
    public object Item { get; set; } = default!;
    public string ItemId { get; set; } = string.Empty;
    public MouseEventArgs MouseEventArgs { get; set; } = default!;
}
```

Render-time handler attachment (from `MariloTreeView.razor.cs`, lines 492–498):

```csharp
if (OnItemContextMenu.HasDelegate)
{
    var ctxNode = node;
    builder.AddAttribute(18, "oncontextmenu", EventCallback.Factory.Create<MouseEventArgs>(this, (MouseEventArgs args) =>
        OnItemContextMenu.InvokeAsync(new TreeItemContextMenuEventArgs { Item = ctxNode.Item, ItemId = ctxNode.Id, MouseEventArgs = args })));
    builder.AddEventPreventDefaultAttribute(19, "oncontextmenu", true);
}
```

#### Consequences

- When `OnItemContextMenu` has no delegate, no `oncontextmenu` attribute and no `preventDefault` directive are emitted for any node. The browser's native context menu is unaffected.
- `preventDefault` is set on the node header `div`'s `oncontextmenu` event, not at the document level. Only nodes in this tree instance suppress the native menu; other elements on the page are unaffected.
- The handler is not guarded by `Disabled` or `ReadOnly`: right-clicking a disabled node still fires `OnItemContextMenu`. Consumers that need to suppress context menus on disabled nodes must check `args.Item` or `args.ItemId` against their disabled state.
- `Item` is typed as `object` to support heterogeneous trees. Consumers must cast to their concrete type.
- The `oncontextmenu` handler is attached to the node header `div`, not to the tree's root element. This means right-clicking the tree's empty space (no node) does not fire the event.
- `MouseEventArgs` includes `ClientX`, `ClientY`, `ScreenX`, `ScreenY`, and `Button`, sufficient for positioning a custom popup at the pointer location.

#### Success Criteria

- [ ] `OnItemContextMenu` fires with correct `Item` and `ItemId` on right-click (unit test)
- [ ] `OnItemContextMenu` provides `MouseEventArgs` from the context menu event (unit test)
- [ ] Browser context menu is suppressed (`preventDefault`) when handler is bound (unit test)
- [ ] No `oncontextmenu` handler attached when `OnItemContextMenu` has no delegate (unit test)

<!-- Reconstructed retroactively — implementation predates this record -->
