# Resolution Design: GAP-readonly-guards

**Gap ID:** GAP-readonly-guards
**Title:** ReadOnly parameter missing from interaction guards
**Resolution Date:** 2026-04-03

---

## Semantics

**ReadOnly** means: the tree displays current state but prevents all data mutations (drag/drop, edit, check, selection changes). Navigation and expansion/collapse for viewing purposes are debatable, but the existing codebase already blocks expand/collapse in `ToggleNodeAsync` when `ReadOnly=true` (line 635), so we follow that established pattern: ReadOnly blocks ALL state changes including expansion.

Keyboard navigation (arrow keys, Home, End) remains allowed for accessibility — the `HandleKeyDown` early return at line 705 intentionally checks only `Disabled`, not `ReadOnly`.

---

## Guard Changes Required

### Must Fix

| # | Location | Line | Current Guard | New Guard | Rationale |
|---|----------|:----:|---------------|-----------|-----------|
| 1 | `HandleDrop()` | 692 | No ReadOnly check | Add `if (ReadOnly) return;` at top | **Only true mutation leak** — `OnItemDrop` fires on ReadOnly tree |
| 2 | DragDrop handler attachment | 480 | `EnableDragDrop && !Disabled` | `EnableDragDrop && !Disabled && !ReadOnly` | Prevents drag UI affordances on ReadOnly tree |

### Should Fix

| # | Location | Line | Current Guard | New Guard | Rationale |
|---|----------|:----:|---------------|-----------|-----------|
| 3 | ExpandOnClick guard | 501 | `hasKids && !Disabled` | `hasKids && !Disabled && !ReadOnly` | Prevents unnecessary onclick attachment |
| 4 | Toggle button disabled attr | 519 | `Disabled` | `Disabled \|\| ReadOnly` | Correct visual state |
| 5 | Title click guard | 597 | `!Disabled` | `!Disabled && !ReadOnly` | Prevents unnecessary onclick attachment |
| 6 | MariloTreeItem toggle disabled | .razor:16 | `TreeView?.Disabled == true` | `TreeView?.Disabled == true \|\| TreeView?.ReadOnly == true` | Correct visual state for declarative tree items |

### No Change Needed

| Location | Line | Rationale |
|----------|:----:|-----------|
| `HandleKeyDown` early return | 705 | Intentional — ReadOnly allows keyboard navigation |

---

## Success Criteria

- [ ] **SC-1**: When `ReadOnly=true` and `EnableDragDrop=true`, dragging and dropping does NOT fire `OnItemDrop`.
- [ ] **SC-2**: When `ReadOnly=true` and `EnableDragDrop=true`, drag event handlers are NOT attached to the DOM (no `draggable="true"` attribute).
- [ ] **SC-3**: When `ReadOnly=true` and `ExpandOnClick=true`, clicking a node header does NOT expand/collapse the node.
- [ ] **SC-4**: When `ReadOnly=true`, the toggle button renders with `disabled` attribute.
- [ ] **SC-5**: When `ReadOnly=true`, clicking a title span does NOT trigger selection.
- [ ] **SC-6**: When `ReadOnly=true`, keyboard navigation (arrow keys) still works for accessibility.
- [ ] **SC-7**: When `ReadOnly=true`, checkbox is visually disabled and clicking does not change state.
- [ ] **SC-8**: `MariloTreeItem` toggle button shows `disabled` when parent `TreeView.ReadOnly=true`.

---

## Cross-References

- Intake: `stages/01-intake/output/gap-readonly-guards-inventory.md`
- Source: `MariloTreeView.razor.cs` lines 480, 501, 519, 597, 692
- Source: `MariloTreeItem.razor` line 16
