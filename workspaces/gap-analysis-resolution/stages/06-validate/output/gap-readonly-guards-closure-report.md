# Closure Report: GAP-readonly-guards

**Gap ID:** GAP-readonly-guards
**Status:** RESOLVED
**Closure Date:** 2026-04-03

---

## Summary

All ReadOnly guard gaps in `MariloTreeView` and `MariloTreeItem` have been fixed. The only true mutation leak (`OnItemDrop` firing on a ReadOnly tree) is now guarded. All cosmetic consistency issues (disabled attributes, onclick handler attachment) are also resolved.

## Success Criteria Verification

| SC | Criterion | Implementation | Test | Status |
|----|-----------|----------------|------|--------|
| SC-1 | ReadOnly + DragDrop: OnItemDrop does not fire | `if (ReadOnly) return;` in HandleDrop | (covered by SC-2 — no handlers attached) | PASS |
| SC-2 | ReadOnly + DragDrop: no draggable="true" | `EnableDragDrop && !Disabled && !ReadOnly` | `TreeView_ReadOnly_DragDropHandlersNotAttached` | PASS |
| SC-3 | ReadOnly + ExpandOnClick: no expand on click | `hasKids && !Disabled && !ReadOnly` | `TreeView_ReadOnly_ExpandOnClick_DoesNotAttachHandler` | PASS |
| SC-4 | ReadOnly: toggle button disabled | `Disabled \|\| ReadOnly` on disabled attr | `TreeView_ReadOnly_ToggleButtonShowsDisabled` | PASS |
| SC-5 | ReadOnly: title click no selection | `!Disabled && !ReadOnly` on title onclick | `TreeView_ReadOnly_TitleClickDoesNotAttachHandler` | PASS |
| SC-6 | ReadOnly: keyboard navigation works | HandleKeyDown checks only Disabled (intentional) | `TreeView_ReadOnly_KeyboardNavigationStillWorks` | PASS |
| SC-7 | ReadOnly: checkbox disabled | Pre-existing (line 548, already correct) | `TreeView_ReadOnly_PreventsCheckboxChanges` (pre-existing) | PASS |
| SC-8 | TreeItem toggle disabled when ReadOnly | `TreeView?.Disabled == true \|\| TreeView?.ReadOnly == true` | (visual consistency, covered by SC-4 pattern) | PASS |

## Files Modified

- `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` — 5 guard changes
- `src/Marilo.Components/Navigation/MariloTreeItem.razor` — 1 disabled attr change
- `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` — 6 new tests

## Enforcement Notes for Reviewers

When adding new interactive features to TreeView, always include `ReadOnly` alongside `Disabled` in guards. The pattern is:
- DOM handler attachment: `if (!Disabled && !ReadOnly)` 
- Method-level guard: `if (Disabled || ReadOnly) return;`
- HTML disabled attr: `disabled="@(Disabled || ReadOnly)"`
- Exception: `HandleKeyDown` — ReadOnly intentionally allows keyboard navigation
