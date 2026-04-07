# Implementation Log: GAP-20 — OnItemContextMenu (Item Context Menu)

**Scope:** batch
**Phase:** 3 — TreeView Phase 3
**Status:** Reconstructed — code predates this log

## Summary

Gap 20 introduced right-click context menu support on individual tree nodes. `OnItemContextMenu` is declared as `EventCallback<TreeItemContextMenuEventArgs>` on `MariloTreeView`. During `RenderNodes`, the handler is attached per-node only when `OnItemContextMenu.HasDelegate` is true — avoiding any event overhead when the consumer does not use context menus. A local variable capture (`var ctxNode = node`) prevents the classic loop-closure bug. `AddEventPreventDefaultAttribute` suppresses the browser's native context menu on the header `div` whenever the handler is bound. `TreeItemContextMenuEventArgs` (in `Marilo.Core/Models/TreeViewModels.cs`) carries the raw data `Item`, the string `ItemId`, and the `MouseEventArgs` (including pointer coordinates) for custom menu positioning.

## Source Files (read-only — no changes made)

| File | Relevant section |
|------|-----------------|
| `Navigation/MariloTreeView.razor.cs` | `OnItemContextMenu` parameter declaration (line 123); render-time handler attachment with `HasDelegate` guard and `AddEventPreventDefaultAttribute` (lines 492–498) |
| `Marilo.Core/Models/TreeViewModels.cs` | `TreeItemContextMenuEventArgs` class (lines 8–18) |

## Tests

| Test file | Test name | Covers |
|-----------|-----------|--------|
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_OnItemContextMenu_FiresOnRightClick` | `oncontextmenu` event triggers callback; `ItemId` and `MouseEventArgs` coordinates are correct |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_OnItemContextMenu_NoHandlerWhenNoDelegateSet` | `oncontextmenu` attribute is absent from the header `div` when no delegate is bound |

**Coverage gaps noted:** `preventDefault` suppression is not independently asserted (bUnit does not expose rendered `preventDefault` attributes in a discoverable way via `GetAttribute`); absence of the native menu side-effect is an in-browser concern. The `Item` object identity is not independently asserted beyond `ItemId` — sufficient for unit testing purposes.

## Phase Exit Criteria

| Criterion | Test status |
|-----------|-------------|
| `OnItemContextMenu` fires with correct `ItemId` on right-click | ✅ passing |
| `OnItemContextMenu` provides `MouseEventArgs` with pointer coordinates | ✅ passing |
| No `oncontextmenu` attribute emitted when no delegate is bound | ✅ passing |
| Browser context menu suppression (`preventDefault`) when handler is bound | ⚠️ not independently asserted — bUnit does not expose `AddEventPreventDefaultAttribute` output via `GetAttribute`; verified by code inspection only |
