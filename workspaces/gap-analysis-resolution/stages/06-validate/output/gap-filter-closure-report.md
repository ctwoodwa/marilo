# Closure Report: GAP-16 — FilterFunc / Search

**Closure Status:** Resolved
**Phase:** 2 — Enhanced
**Pipeline note:** Reconstructed — code predates formal records
**Validated:** 2026-04-02

## Criteria Verification

| Criterion | Source location | Test name | Status |
|-----------|----------------|-----------|--------|
| FilterFunc hides non-matching leaf nodes | `MariloTreeView.razor.cs:ApplyFilter()` (lines 435–448) — non-matching nodes not added to result list | `TreeView_FilterFunc_HidesNonMatchingLeafNodes` | ✅ |
| Ancestors of matching nodes remain visible | `MariloTreeView.razor.cs:ApplyFilter()` (line 444) — `filteredChildren.Count > 0` retains parent | `TreeView_FilterFunc_HidesNonMatchingLeafNodes` | ✅ |
| Matching nodes receive `mar-tree-item--filter-match` CSS class | `MariloTreeView.razor.cs:RenderNodes` (line 466) — CSS class conditional on `FilterFunc(node.Item)` | `TreeView_FilterFunc_MatchingNodesGetFilterMatchCssClass` | ✅ |
| Non-matching ancestors do NOT receive `mar-tree-item--filter-match` CSS class | `MariloTreeView.razor.cs:RenderNodes` (line 466) — class only applied when predicate returns `true` for the node itself | `TreeView_FilterFunc_MatchingNodesGetFilterMatchCssClass` | ✅ |
| `ClearFilter()` restores all nodes | `MariloTreeView.razor.cs:ClearFilter()` (lines 195–199) — sets `_cachedTree = null` and calls `StateHasChanged()` | `TreeView_ClearFilter_RestoresAllNodes` | ✅ |
| `FilterFunc = null` shows all nodes (default behaviour) | `MariloTreeView.razor.cs` (lines 361–362) — `ApplyFilter` skipped when `FilterFunc == null` | `TreeView_FilterFunc_NullShowsAllNodes` | ✅ |

## Evidence

- **Source:** `Navigation/MariloTreeView.razor.cs` — `FilterFunc` parameter (line 141), `ApplyFilter()` recursive pruning (lines 435–448), post-`BuildTree` filter invocation (lines 361–362), `ClearFilter()` (lines 195–199), CSS class emission in `RenderNodes` (line 466)
- **Tests:** `TreeViewTests.cs` — 4 tests, all passing
- **Gap no longer present:** Yes — `FilterFunc` predicate is supported with ancestor-preserving pruning, `mar-tree-item--filter-match` CSS class is applied to matching nodes only, and `ClearFilter()` correctly restores the full unfiltered tree

## Enforcement Guardrails

- 4 bUnit tests in `TreeViewTests.cs` cover every resolved criterion; any regression in `ApplyFilter()` pruning logic, CSS class assignment, or `ClearFilter()` cache invalidation will produce a test failure in CI
- `_cachedTree` memoisation means the filter result is stable across Blazor render cycles; `OnParametersSet` already invalidates the cache unconditionally, so a caller who sets `FilterFunc = null` as a parameter change does not need to call `ClearFilter()` — the test suite implicitly guards this behaviour
- `FilterFunc` is evaluated twice per visible node (once in `ApplyFilter`, once in `RenderNodes` for CSS assignment); this is a latent performance concern for expensive predicates but is not a correctness issue

## Follow-up Tasks

None.
