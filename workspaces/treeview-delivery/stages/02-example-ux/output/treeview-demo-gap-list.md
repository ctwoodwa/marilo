# TreeView Demo Gap List

**Audit Date:** 2026-04-03
**Demo Page:** /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/TreeView/Overview.razor
**Stage 01 Input:** stages/01-spec-review/output/treeview-spec-gap-list.md

---

## Current Demo Inventory

The demo page has 6 scenarios across 3 page sections:

| # | Scenario Title | Parameters Demonstrated | API Style |
|---|---------------|------------------------|-----------|
| 1 | Basic Usage (ChildContent) | ChildContent, MariloTreeItem (Title, IsExpanded) | Declarative |
| 2 | Data-Driven (Flat Data) | Data, IdField, ParentIdField, TextField | Data-binding |
| 3 | Data-Driven (Hierarchical) | Data, IdField, TextField, ItemsField | Data-binding |
| 4 | With Icons | Icon (on MariloTreeItem) | Declarative |
| 5 | Selection | SelectionMode (Single) | Declarative |
| 6 | Item Selection | (same as #5 but no SelectionMode set) | Declarative |
| 7 | Item Expand / Collapse | (basic expand) | Declarative |
| 8 | Accessibility | Keyboard/ARIA reference table | Documentation |

---

## Demo Gap List

### (A) Parameters with no demo scenario

| # | Parameter | Priority | Notes |
|---|-----------|----------|-------|
| A1 | CheckBoxMode (Single/Multiple) | P1 | Core feature, no demo at all |
| A2 | AllowCheckChildren / AllowCheckParents | P1 | Tri-state cascade, highly visual |
| A3 | CheckedItems (two-way binding) | P1 | Key data-binding feature |
| A4 | EnableDragDrop + OnItemDrop | P1 | Interactive feature, no demo |
| A5 | LoadChildrenAsync (lazy loading) | P1 | Core data pattern, no demo |
| A6 | ExpandOnClick | P2 | Behavioral option |
| A7 | SingleExpand (accordion) | P2 | Behavioral option |
| A8 | AutoExpand | P2 | Behavioral option |
| A9 | FilterFunc | P2 | Filtering capability |
| A10 | AllowEditing + OnItemEdit | P2 | Inline editing |
| A11 | OnItemContextMenu | P2 | Right-click menu |
| A12 | CheckboxTemplate | P3 | Advanced customization |
| A13 | Size parameter | P3 | Appearance option |
| A14 | ExpandAllAsync / CollapseAllAsync | P2 | Programmatic API |
| A15 | SelectNodeAsync | P3 | Programmatic navigation |
| A16 | ItemTemplate | P2 | Custom rendering |

### (B) Scenarios with stale code snippets

None identified — all existing snippets use current API.

### (C) Events with no demo scenario

| # | Event | Priority | Notes |
|---|-------|----------|-------|
| C1 | CheckedItemsChanged | P1 | Checkbox binding event |
| C2 | ExpandedItemsChanged | P2 | Expand/collapse binding event |
| C3 | OnItemClick | P2 | Already partially shown but no output display |
| C4 | SelectedItemsChanged | P2 | Selection binding event |

### (D) Edge cases not demonstrated

| # | Edge Case | Priority | Notes |
|---|-----------|----------|-------|
| D1 | Disabled tree | P2 | No interaction allowed |
| D2 | ReadOnly tree | P2 | View-only mode |
| D3 | Empty tree (no data) | P3 | Graceful empty state |
| D4 | Large dataset (performance) | P3 | Scrolling/rendering with many nodes |

---

## Summary

| Gap Type | Count | P1 | P2 | P3 |
|----------|-------|----|----|-----|
| (A) No demo scenario | 16 | 5 | 7 | 4 |
| (B) Stale snippets | 0 | — | — | — |
| (C) Events no demo | 4 | 1 | 3 | — |
| (D) Edge cases | 4 | — | 2 | 2 |
| **Total** | **24** | **6** | **12** | **6** |

---

## CHECKPOINT — Human Decision Required

This is the Stage 02 checkpoint. Before writing new demo scenarios, the following decisions are needed:

1. **Scope**: Should all 24 gaps be addressed, or only P1 (6 items)?
2. **Priority order**: The proposed P1 set covers the most impactful missing demos (checkboxes, drag-drop, lazy loading). Approve or adjust.
3. **Deferred items**: P3 items (CheckboxTemplate, Size, empty state, large dataset) can be deferred to a later cycle. Confirm.
4. **Demo page structure**: Should new scenarios be added to the existing Overview.razor, or split into separate pages (e.g., Checkboxes.razor, DragDrop.razor)?

**Awaiting approval before proceeding to scenario writing.**
