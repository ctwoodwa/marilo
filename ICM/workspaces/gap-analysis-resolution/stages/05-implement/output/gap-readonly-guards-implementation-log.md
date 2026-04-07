# Implementation Log: GAP-readonly-guards

**Gap ID:** GAP-readonly-guards
**Implementation Date:** 2026-04-03

---

## Changes Made

### MariloTreeView.razor.cs

| # | Location | Change |
|---|----------|--------|
| 1 | `HandleDrop()` method | Added `if (ReadOnly) return;` guard at top |
| 2 | DragDrop handler attachment | Changed `EnableDragDrop && !Disabled` to `EnableDragDrop && !Disabled && !ReadOnly` |
| 3 | ExpandOnClick guard | Changed `hasKids && !Disabled` to `hasKids && !Disabled && !ReadOnly` |
| 4 | Toggle button disabled attr | Changed `Disabled` to `Disabled \|\| ReadOnly` |
| 5 | Title click guard | Changed `!Disabled` to `!Disabled && !ReadOnly` |

### MariloTreeItem.razor

| # | Location | Change |
|---|----------|--------|
| 6 | Toggle button disabled attr (line 16) | Changed `TreeView?.Disabled == true` to `TreeView?.Disabled == true \|\| TreeView?.ReadOnly == true` |

### No Changes Made

- `HandleKeyDown` early return (line 705): Intentionally left as `Disabled`-only — ReadOnly allows keyboard navigation.

## Tests Added

| Test | Success Criterion | Description |
|------|-------------------|-------------|
| `TreeView_ReadOnly_DragDropHandlersNotAttached` | SC-2 | No `draggable="true"` when ReadOnly |
| `TreeView_ReadOnly_DragDropEnabled_NoReadOnly_HasDraggable` | (control) | Confirms draggable IS present when ReadOnly=false |
| `TreeView_ReadOnly_ExpandOnClick_DoesNotAttachHandler` | SC-3 | No onclick on header when ReadOnly+ExpandOnClick |
| `TreeView_ReadOnly_ToggleButtonShowsDisabled` | SC-4 | Toggle button has disabled attr when ReadOnly |
| `TreeView_ReadOnly_TitleClickDoesNotAttachHandler` | SC-5 | Title span has no onclick when ReadOnly |
| `TreeView_ReadOnly_KeyboardNavigationStillWorks` | SC-6 | Arrow keys still move focus when ReadOnly |

**Test file:** `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs`
