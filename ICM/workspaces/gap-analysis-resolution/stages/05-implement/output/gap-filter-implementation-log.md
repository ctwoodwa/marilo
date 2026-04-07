# Implementation Log: GAP-16 — FilterFunc / Search

**Scope:** batch
**Phase:** 2 — Enhanced
**Status:** Reconstructed — code predates this log

## Summary

Gap 16 introduced filtering support for `MariloTreeView` following Fancytree's "keep ancestors visible" pattern. `ApplyFilter()` recursively prunes non-matching nodes while retaining ancestors whose subtrees contain at least one match; matching nodes receive the `mar-tree-item--filter-match` CSS class. `ClearFilter()` invalidates the cached tree and triggers a re-render with the full unfiltered set.

## Source Files (read-only — no changes made)

| File | Relevant section |
|------|-----------------|
| `Navigation/MariloTreeView.razor.cs` | `FilterFunc` parameter (line 141), `ApplyFilter()` (lines 435–448), filter call after `BuildTree` (lines 361–362), `ClearFilter()` (lines 195–199), CSS class assignment in `RenderNodes` (line 466) |

## Tests

| Test file | Test name | Covers |
|-----------|-----------|--------|
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_FilterFunc_HidesNonMatchingLeafNodes` | Non-matching leaf nodes absent from rendered output; ancestors of matching nodes retained |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_FilterFunc_MatchingNodesGetFilterMatchCssClass` | Matching node carries `mar-tree-item--filter-match`; non-matching ancestor does not |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_FilterFunc_NullShowsAllNodes` | No `FilterFunc` renders complete tree without filter-match classes |
| `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` | `TreeView_ClearFilter_RestoresAllNodes` | After `ClearFilter()` all nodes visible and filter-match class absent |

**Coverage gaps noted:** None

## Phase Exit Criteria

| Criterion | Test status |
|-----------|-------------|
| FilterFunc hides non-matching leaf nodes | ✅ passing |
| Ancestors of matching nodes remain visible | ✅ passing |
| Matching nodes receive `mar-tree-item--filter-match` CSS class | ✅ passing |
| Non-matching ancestors do NOT receive `mar-tree-item--filter-match` CSS class | ✅ passing |
| `ClearFilter()` restores all nodes | ✅ passing |
| `FilterFunc = null` shows all nodes (default behaviour) | ✅ passing |
