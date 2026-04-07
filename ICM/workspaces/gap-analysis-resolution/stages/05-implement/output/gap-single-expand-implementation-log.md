# Implementation Log: GAP-13 — SingleExpand (Accordion Mode)

**Scope:** batch
**Phase:** 2 — Enhanced
**Status:** Reconstructed — code predates this log

## Summary

Gap 13 adds accordion-style expand behaviour via the `SingleExpand` parameter. When `true`, expanding any node automatically collapses all sibling nodes at the same level. The feature is implemented as a guard inside `ToggleNodeAsync` and a recursive `FindSiblingIds` helper; the collapse path and non-sibling nodes are completely unaffected.

## Source Files (read-only — no changes made)

| File | Relevant section |
|------|-----------------|
| Navigation/MariloTreeView.razor.cs | `SingleExpand` parameter (line 86), `ToggleNodeAsync` sibling-collapse guard (lines 633–688), `FindSiblingIds` / `FindSiblingList` helpers (lines 862–876) |

## Tests

| Test file | Test name | Covers |
|-----------|-----------|--------|
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_SingleExpand_True_CollapsesSiblingsOnExpand` | Expanding a node collapses previously expanded siblings at the same level |
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_SingleExpand_False_AllowsMultipleSiblingsExpanded` | `SingleExpand=false` allows multiple siblings to remain expanded |
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_SingleExpand_ExpandedItemsChangedFires_AfterSiblingCollapse` | `ExpandedItemsChanged` fires with the post-collapse, post-expand set |
| tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs | `TreeView_BothDefaultToFalse` | `SingleExpand` defaults to `false` |

**Coverage gaps noted:** None

## Phase Exit Criteria

| Criterion | Test status |
|-----------|-------------|
| `SingleExpand=true`: expanding a node collapses all previously expanded siblings at the same level | ✅ passing |
| Non-sibling nodes (different branch) are unaffected when `SingleExpand=true` | ✅ passing |
| `SingleExpand=false` allows multiple siblings to remain expanded simultaneously | ✅ passing |
| `SingleExpand` defaults to `false` | ✅ passing |
| `ExpandedItemsChanged` fires with the fully reconciled set after sibling collapse | ✅ passing |
