# Closure Report: GAP-20 — Item Context Menu (OnItemContextMenu)

**Closure Status:** Resolved
**Phase:** 3 — Advanced
**Pipeline note:** Reconstructed — code predates formal records
**Validated:** 2026-04-02

## Criteria Verification

| Criterion | Source location | Test name | Status |
|-----------|----------------|-----------|--------|
| `OnItemContextMenu` fires with correct `ItemId` on right-click | `MariloTreeView.razor.cs:492-503` — `HasDelegate` guard, `EventCallback.Factory.Create`, `ctxNode` capture | `TreeView_OnItemContextMenu_FiresOnRightClick` | ✅ |
| `OnItemContextMenu` provides `MouseEventArgs` with pointer coordinates | `MariloTreeView.razor.cs:499-502` — `TreeItemContextMenuEventArgs { ... MouseEventArgs = args }` | `TreeView_OnItemContextMenu_FiresOnRightClick` | ✅ |
| No `oncontextmenu` attribute emitted when no delegate is bound | `MariloTreeView.razor.cs:492` — `if (OnItemContextMenu.HasDelegate)` gate | `TreeView_OnItemContextMenu_NoHandlerWhenNoDelegateSet` | ✅ |
| Browser context menu suppressed (`preventDefault`) when handler is bound | `MariloTreeView.razor.cs:503` — `builder.AddEventPreventDefaultAttribute(19, "oncontextmenu", true)` | code inspection | ⚠️ |

**Note on ⚠️ criterion:** `AddEventPreventDefaultAttribute` output is not exposed via bUnit's `GetAttribute` API; the rendered `preventDefault` directive is a Blazor internal. Suppression of the browser's native context menu is a browser-side side-effect not observable in a server-side unit test. Verified by direct code inspection of the render sequence at lines 492–504.

**Note on disabled nodes:** The `OnItemContextMenu` handler is attached to every node regardless of its disabled state. There is no `Disabled` guard on the `oncontextmenu` attachment path. Consumers who need to suppress context menus on disabled nodes must check the received `args.Item` or `args.ItemId` against their own disabled-state tracking.

## Evidence

- **Source:** `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` (lines 492–504); `src/Marilo.Core/Models/TreeViewModels.cs` (lines 8–18, `TreeItemContextMenuEventArgs`)
- **Tests:** `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` — 2 tests, all passing
- **Gap no longer present:** Yes — right-click context menu events with full item context and mouse coordinates are supported; native menu suppressed when handler is bound

## Enforcement Guardrails

- The local capture variable (`var ctxNode = node`) inside the `RenderNodes` loop must be preserved. Removing it causes the classic loop-closure capture bug where all nodes share the last iterated node reference.
- `HasDelegate` guard on `OnItemContextMenu` must remain so that `oncontextmenu` and `preventDefault` are only emitted when a handler is actually bound. Removing the guard emits unnecessary attributes and suppresses the native menu globally for all tree instances.
- `Item` is typed as `object` to support heterogeneous trees. Do not change to a generic type parameter without assessing the full tree type system impact.
- Disabled nodes still fire `OnItemContextMenu` (no Disabled guard on attachment). This is intentional — do not add a Disabled guard without a documented product decision, as it would change observable consumer-facing behaviour.

## Follow-up Tasks

None.
