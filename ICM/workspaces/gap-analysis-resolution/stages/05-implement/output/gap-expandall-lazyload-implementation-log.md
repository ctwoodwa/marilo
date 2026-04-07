# Implementation Log: GAP-expandall-lazyload

**Gap ID:** GAP-expandall-lazyload
**Implementation Date:** 2026-04-03
**Resolution Option:** C (opt-in parameter)

---

## Changes Made

### MariloTreeView.razor.cs

1. **Modified `ExpandAllAsync` signature** (was line 176):
   - Old: `public async Task ExpandAllAsync()`
   - New: `public async Task ExpandAllAsync(bool includeUnloaded = false, int maxDepth = int.MaxValue, CancellationToken cancellationToken = default)`
   - When `includeUnloaded=false` (default): identical to previous behavior
   - When `includeUnloaded=true`: loads lazy nodes depth-first before expanding

2. **Added `LoadUnloadedNodesAsync` private helper**:
   - Depth-first traversal of tree
   - For each node with `HasChildren && !Children.Any() && !_loadedNodeIds.Contains(id)`:
     - Adds to `_loadingIds` (shows loading indicator)
     - Calls `LoadChildrenAsync(node.Item)`
     - Invalidates `_cachedTree`
     - Marks in `_loadedNodeIds`
     - Rebuilds tree and recurses into new children
   - Respects `maxDepth` and `cancellationToken`

## Tests Added

| Test | Success Criterion | Description |
|------|-------------------|-------------|
| `TreeView_ExpandAllAsync_DefaultDoesNotLoadLazyNodes` | SC-1 | No-arg call does not trigger LoadChildrenAsync |
| `TreeView_ExpandAllAsync_IncludeUnloadedTriggersLazyLoad` | SC-2 | includeUnloaded=true calls LoadChildrenAsync |
| `TreeView_ExpandAllAsync_IncludeUnloaded_AllNodesExpanded` | SC-3, SC-6 | All nodes expanded; ExpandedItemsChanged fires with complete set |
| `TreeView_ExpandAllAsync_MaxDepthLimitsTraversal` | SC-4 | maxDepth=1 only loads one level |
| `TreeView_ExpandAllAsync_CancellationStopsLoading` | SC-5 | CancellationToken throws OperationCanceledException |
| `TreeView_ExpandAllAsync_BackwardCompatible_NoArgs` | SC-7 | Existing callers without args still work |

**Test file:** `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs`

## Backward Compatibility

- `ExpandAllAsync()` (no arguments) compiles and behaves identically to before
- New parameters all have default values
- No breaking API surface changes
