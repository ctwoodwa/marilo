# Closure Report: GAP-19 — Programmatic Navigation (SelectNodeAsync)

**Closure Status:** Resolved
**Phase:** 3 — Advanced
**Pipeline note:** Reconstructed — code predates formal records
**Validated:** 2026-04-02

## Criteria Verification

| Criterion | Source location | Test name | Status |
|-----------|----------------|-----------|--------|
| `SelectNodeAsync` expands all ancestors of target node | `MariloTreeView.razor.cs:209-212` — `CollectAncestorIds` + `_expandedIds.Add` loop | `TreeView_SelectNodeAsync_ExpandsAncestors` | ✅ |
| `SelectNodeAsync` selects only the target node | `MariloTreeView.razor.cs:218-219` — `_selectedIds.Clear(); _selectedIds.Add(id)` | `TreeView_SelectNodeAsync_SelectsTargetNode` | ✅ |
| `SelectNodeAsync` sets keyboard focus to target node | `MariloTreeView.razor.cs:220` — `_focusedNodeId = id` | `TreeView_SelectNodeAsync_SetsFocusToTargetNode` | ✅ |
| `SelectNodeAsync` fires `ExpandedItemsChanged` when delegate bound | `MariloTreeView.razor.cs:214-215` — `HasDelegate` guard + `ExpandedItemsChanged.InvokeAsync` | `TreeView_SelectNodeAsync_FiresExpandedItemsChanged` | ✅ |
| `SelectNodeAsync` fires `SelectedItemsChanged` | `MariloTreeView.razor.cs:221` — `SelectedItemsChanged.InvokeAsync` (no guard) | `TreeView_SelectNodeAsync_SelectsTargetNode` | ✅ |
| `SelectNodeAsync` returns silently for non-existent node ID | `MariloTreeView.razor.cs:205-206` — `FindNode` + early `return` when null | `TreeView_SelectNodeAsync_SilentlyReturnsForNonExistentId` | ✅ |
| `SelectNodeAsync` publicly accessible via `@ref` | `MariloTreeView.razor.cs:202` — `public async Task SelectNodeAsync` | code inspection | ✅ |

## Evidence

- **Source:** `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` (lines 201–225)
- **Tests:** `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` — 5 tests, all passing
- **Gap no longer present:** Yes — programmatic navigation to any node by ID is fully supported in a single awaitable call

## Enforcement Guardrails

- `SelectNodeAsync` intentionally does not check `Disabled` or `ReadOnly`. These parameters govern user interaction only; programmatic API callers are expected to manage disabled-state semantics externally. Do not add a guard inside the method without a documented product decision.
- `ExpandedItemsChanged` uses `HasDelegate` guard (matching the convention in `ExpandAllAsync`/`CollapseAllAsync`); `SelectedItemsChanged` is always awaited. This asymmetry is intentional — do not add a `HasDelegate` guard to `SelectedItemsChanged` without updating the convention across all callers.
- `SelectNodeAsync` replaces the entire selection (not an additive multi-select). Callers requiring selection preservation must manage `SelectedItems` externally before calling.
- `_cachedTree = null` must remain in the method to ensure the next render reflects newly expanded ancestors. Do not remove the cache invalidation.

## Follow-up Tasks

None.
