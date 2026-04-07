# Closure Report: GAP-expandall-lazyload

**Gap ID:** GAP-expandall-lazyload
**Status:** RESOLVED
**Closure Date:** 2026-04-03

---

## Summary

`ExpandAllAsync` now supports an opt-in `includeUnloaded` parameter that triggers `LoadChildrenAsync` for unloaded lazy nodes before expanding. The default behavior (`includeUnloaded=false`) is preserved for backward compatibility.

## Success Criteria Verification

| SC | Criterion | Implementation | Test | Status |
|----|-----------|----------------|------|--------|
| SC-1 | Default call does not load lazy nodes | `if (includeUnloaded && LoadChildrenAsync != null)` guard | `TreeView_ExpandAllAsync_DefaultDoesNotLoadLazyNodes` | PASS |
| SC-2 | includeUnloaded=true triggers LoadChildrenAsync | `LoadUnloadedNodesAsync` depth-first traversal | `TreeView_ExpandAllAsync_IncludeUnloadedTriggersLazyLoad` | PASS |
| SC-3 | All nodes expanded after includeUnloaded=true | Full `CollectAllIds` after lazy load completes | `TreeView_ExpandAllAsync_IncludeUnloaded_AllNodesExpanded` | PASS |
| SC-4 | maxDepth limits traversal depth | `currentDepth < maxDepth` guard in `LoadUnloadedNodesAsync` | `TreeView_ExpandAllAsync_MaxDepthLimitsTraversal` | PASS |
| SC-5 | CancellationToken stops loading | `cancellationToken.ThrowIfCancellationRequested()` | `TreeView_ExpandAllAsync_CancellationStopsLoading` | PASS |
| SC-6 | ExpandedItemsChanged fires with complete set | Fires after all loads + CollectAllIds | `TreeView_ExpandAllAsync_IncludeUnloaded_AllNodesExpanded` | PASS |
| SC-7 | Backward compatible (no-arg call) | All parameters have defaults | `TreeView_ExpandAllAsync_BackwardCompatible_NoArgs` | PASS |

## Backward Compatibility

- Method signature: `ExpandAllAsync(bool includeUnloaded = false, int maxDepth = int.MaxValue, CancellationToken cancellationToken = default)`
- Existing callers: compile without changes; behavior identical to before
- No public API breaking changes

## Files Modified

- `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` — ExpandAllAsync + LoadUnloadedNodesAsync
- `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` — 6 new tests
