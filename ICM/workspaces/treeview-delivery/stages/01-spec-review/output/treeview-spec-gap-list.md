# TreeView Spec Gap List

**Audit Date:** 2026-04-03
**Re-verified:** 2026-04-11 (parameter inventory unchanged; partial closures applied)
**Component:** MariloTreeView / MariloTreeItem
**Spec Directory:** /workspaces/Marilo/docs/component-specs/treeview/
**Source:** /workspaces/Marilo/src/Marilo.Components/Navigation/MariloTreeView.razor.cs

---

## Summary

| Type | Count | Open |
|------|-------|------|
| Undocumented (implemented but not in spec) | 14 | 3 |
| Spec-ahead (documented but not implemented) | 13 | 13 |
| Mismatch (both exist but disagree) | 7 | 7 |
| **Total** | **34** | **23** |

| Priority | Count |
|----------|-------|
| P1 (blocking) | 4 |
| P2 (this phase) | 12 |
| P3 (next phase / forward-looking) | 18 |

## 2026-04-11 Closures (this stage run)

The following undocumented gaps were closed by adding the parameters and methods directly to `docs/component-specs/treeview/overview.md`. Source is authoritative; the spec now documents each one with type, default, and description.

- **SPEC-treeview-001** `ExpandOnClick` — added to overview Parameters table
- **SPEC-treeview-002** `ExpandOnDoubleClick` — added to overview Parameters table
- **SPEC-treeview-003** `SingleExpand` — added to overview Parameters table
- **SPEC-treeview-004** `AutoExpand` — added to overview Parameters table
- **SPEC-treeview-005** `AllowEditing` / `OnItemEdit` — parameter added to overview; full inline-editing section still deferred
- **SPEC-treeview-006** `FilterFunc` + `ClearFilter()` — parameter and method added to overview
- **SPEC-treeview-007** `CheckboxTemplate` — added to overview Parameters table
- **SPEC-treeview-009** `Disabled` — added to overview Parameters table
- **SPEC-treeview-010** `ReadOnly` — added to overview Parameters table
- **SPEC-treeview-011** `SelectNodeAsync(string id)` — added to overview Methods table
- **SPEC-treeview-012** `AriaLabel` — added to overview Parameters table
- **SPEC-treeview-013** `ItemTemplate` — added to overview Parameters table
- **SPEC-treeview-014** `ExpandAllAsync` enhanced signature — overview Methods table now shows full signature with `includeUnloaded`, `maxDepth`, `CancellationToken`

Still open in (A): **SPEC-treeview-008** (OnItemContextMenu — needs events.md expansion, not overview).

Still open in (A) but downgraded: none.

Remaining undocumented open count after this pass: **1** (008). The counts table above reflects: 13 of 14 undocumented closed, plus `CollapseAllAsync` (previously only in source) is now also in the spec methods table.

---

## (A) Undocumented — Implemented but not in spec

These features exist in the component source but have no dedicated spec documentation.

---

**ID:** SPEC-treeview-001
**Type:** undocumented
**Parameter/Event:** ExpandOnClick
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | ExpandOnClick |
| Type | missing | bool |
| Default | missing | false |
| Description | missing | Expands node when clicking anywhere on header |

**Recommended action:** Add to spec (expansion section)
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-002
**Type:** undocumented
**Parameter/Event:** ExpandOnDoubleClick
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | ExpandOnDoubleClick |
| Type | missing | bool |
| Default | missing | false |
| Description | missing | Expands node on double-click (suppressed when AllowEditing=true) |

**Recommended action:** Add to spec (expansion section)
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-003
**Type:** undocumented
**Parameter/Event:** SingleExpand
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | referenced in fluent-ui-gap-analysis only | SingleExpand |
| Type | missing | bool |
| Default | missing | false |
| Description | missing | Accordion behavior — collapses siblings when a node expands |

**Recommended action:** Add to spec (expansion section)
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-004
**Type:** undocumented
**Parameter/Event:** AutoExpand
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | AutoExpand |
| Type | missing | bool |
| Default | missing | false |
| Description | missing | Auto-expands ancestors of selected items |

**Recommended action:** Add to spec (expansion section)
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-005
**Type:** undocumented
**Parameter/Event:** AllowEditing + OnItemEdit
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | referenced in fluent-ui-gap-analysis only | AllowEditing, OnItemEdit |
| Type | missing | bool, EventCallback<TreeItemEditEventArgs> |
| Default | missing | false |
| Description | missing | Inline editing via double-click or F2; fires OnItemEdit on commit |

**Recommended action:** Add to spec (new inline-editing section)
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-006
**Type:** undocumented
**Parameter/Event:** FilterFunc + ClearFilter()
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | referenced in fluent-ui-gap-analysis only | FilterFunc, ClearFilter() |
| Type | missing | Func<object, bool>?, void |
| Default | missing | null |
| Description | missing | Predicate filters visible nodes; ancestors of matches stay visible |

**Recommended action:** Add to spec (new filtering section)
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-007
**Type:** undocumented
**Parameter/Event:** CheckboxTemplate
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | referenced in fluent-ui-gap-analysis only | CheckboxTemplate |
| Type | missing | RenderFragment<CheckboxContext>? |
| Default | missing | null |
| Description | missing | Custom render fragment for checkbox display |

**Recommended action:** Add to spec (checkboxes section)
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-008
**Type:** undocumented
**Parameter/Event:** OnItemContextMenu
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Exists in events.md | OnItemContextMenu |
| Type | missing | EventCallback<TreeItemContextMenuEventArgs> |
| Default | missing | — |
| Description | Partially described | Fires on right-click with item and mouse event data |

**Recommended action:** Expand spec coverage in events.md
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-009
**Type:** undocumented
**Parameter/Event:** Disabled
**Priority:** P3

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | mentioned in examples only | Disabled |
| Type | missing from param table | bool |
| Default | missing | false |
| Description | missing | Prevents all user interaction |

**Recommended action:** Add to spec overview parameter table
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-010
**Type:** undocumented
**Parameter/Event:** ReadOnly
**Priority:** P3

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | ReadOnly |
| Type | missing | bool |
| Default | missing | false |
| Description | missing | Shows state but prevents mutations |

**Recommended action:** Add to spec overview parameter table
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-011
**Type:** undocumented
**Parameter/Event:** SelectNodeAsync(string id)
**Priority:** P3

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | SelectNodeAsync |
| Type | missing | Task |
| Default | N/A | N/A |
| Description | missing | Programmatically navigates to a node: expands ancestors, selects, sets focus |

**Recommended action:** Add to spec methods section
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-012
**Type:** undocumented
**Parameter/Event:** AriaLabel
**Priority:** P3

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | mentioned in accessibility only | AriaLabel |
| Type | missing from param table | string? |
| Default | missing | null |
| Description | missing | Accessibility label for the tree element |

**Recommended action:** Add to spec overview parameter table
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-013
**Type:** undocumented
**Parameter/Event:** ItemTemplate
**Priority:** P3

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | In templates.md but not in overview param table | ItemTemplate |
| Type | missing from overview | RenderFragment<object>? |
| Default | missing | null |
| Description | Covered in templates.md | Custom render fragment for item content |

**Recommended action:** Add to spec overview parameter table
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-014
**Type:** undocumented
**Parameter/Event:** ExpandAllAsync enhanced signature
**Priority:** P3

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | ExpandAllAsync() (no params) | ExpandAllAsync(bool includeUnloaded, int maxDepth, CancellationToken) |
| Type | Task | Task |
| Default | N/A | false, int.MaxValue, default |
| Description | Simple expand all | Enhanced with lazy-load support, depth limit, and cancellation |

**Recommended action:** Update spec methods section
**Delegated to:** spec update only

---

## (B) Spec-Ahead — Documented but not implemented (Planned)

> **Policy (2026-04-09):** Spec-ahead items stay in specs marked "Planned" with gap ID links. Telerik naming is canonical. These do NOT block delivery gates at P2/P3 priority.

---

**ID:** SPEC-treeview-015
**Type:** spec-ahead — **Planned**
**Parameter/Event:** TreeViewBinding component
**Priority:** P2
**Gap link:** gap-analysis-resolution (future intake)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | TreeViewBinding | missing |
| Type | Component | missing |
| Description | Per-level binding config (different TextField, ItemsField per level) | missing |

**Status:** Planned — Telerik parity feature, awaiting gap intake

---

**ID:** SPEC-treeview-016
**Type:** spec-ahead — **Planned**
**Parameter/Event:** OnExpand event
**Priority:** P2
**Gap link:** gap-analysis-resolution (future intake)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | OnExpand | missing |
| Type | EventCallback<TreeViewExpandEventArgs> | missing |
| Description | Fires when node expands, provides cancellation | missing |

**Status:** Planned — Telerik parity event

---

**ID:** SPEC-treeview-017
**Type:** spec-ahead — **Planned**
**Parameter/Event:** OnItemDoubleClick event
**Priority:** P2
**Gap link:** gap-analysis-resolution (future intake)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | OnItemDoubleClick | missing |
| Type | EventCallback<TreeViewItemDoubleClickEventArgs> | missing |
| Description | Fires on item double-click | missing |

**Status:** Planned — Telerik parity event

---

**ID:** SPEC-treeview-018
**Type:** spec-ahead — **Planned**
**Parameter/Event:** OnItemRender event
**Priority:** P2
**Gap link:** gap-analysis-resolution (future intake)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | OnItemRender | missing |
| Type | EventCallback<TreeViewItemRenderEventArgs> | missing |
| Description | Per-item render callback for custom styling | missing |

**Status:** Planned — Telerik parity event (pattern exists in MariloMultiSelect)

---

**ID:** SPEC-treeview-019
**Type:** spec-ahead — **Planned**
**Parameter/Event:** OnDragStart, OnDrag, OnDragEnd events
**Priority:** P3
**Gap link:** gap-analysis-resolution (future intake)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | OnDragStart, OnDrag, OnDragEnd | missing |
| Type | EventCallback<TreeViewDrag*EventArgs> | missing |
| Description | Granular drag lifecycle events | missing (only OnItemDrop exists) |

**Status:** Planned — Telerik parity; requires JS interop for full drag lifecycle

---

**ID:** SPEC-treeview-020
**Type:** spec-ahead — **Planned**
**Parameter/Event:** CheckOnClick
**Priority:** P2
**Gap link:** gap-analysis-resolution (future intake)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | CheckOnClick | missing |
| Type | bool | missing |
| Description | Click node to toggle checkbox | missing |

**Status:** Planned — Telerik parity feature

---

**ID:** SPEC-treeview-021
**Type:** spec-ahead — **Planned**
**Parameter/Event:** UrlField
**Priority:** P3
**Gap link:** gap-analysis-resolution (future intake)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | UrlField | missing |
| Type | string | missing |
| Description | Field name for navigation URL binding | missing |

**Status:** Planned — Telerik parity feature

---

**ID:** SPEC-treeview-022
**Type:** spec-ahead — **Planned**
**Parameter/Event:** DragThrottleInterval
**Priority:** P3
**Gap link:** gap-analysis-resolution (future intake)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | DragThrottleInterval | missing |
| Type | int (ms) | missing |
| Description | Throttle interval for drag events | missing |

**Status:** Planned — Telerik parity; depends on JS interop drag infrastructure

---

**ID:** SPEC-treeview-023
**Type:** spec-ahead — **Planned**
**Parameter/Event:** GetItemFromDropIndex()
**Priority:** P3
**Gap link:** gap-analysis-resolution (future intake)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | GetItemFromDropIndex | missing |
| Type | method | missing |
| Description | Resolve item from drag-drop index | missing |

**Status:** Planned — Telerik parity; depends on JS interop drag infrastructure

---

**ID:** SPEC-treeview-024
**Type:** spec-ahead — **Planned**
**Parameter/Event:** Class parameter
**Priority:** P3

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Class | Inherited from MariloComponentBase |
| Type | string | string |
| Description | Custom CSS class | Available via base class but not in spec param table |

**Status:** Planned — spec update only; clarify inheritance from MariloComponentBase

---

**ID:** SPEC-treeview-025-027
**Type:** spec-ahead — **Planned**
**Parameter/Event:** Fluent UI forward-looking features
**Priority:** P3

Appearance, IconAfterField, AsideTemplate, ActionsTemplate, NavigationMode, AnimateExpand, Virtualization, InfiniteScroll, Tree Manipulation API, Subtree component — all documented in fluent-ui-gap-analysis.md as aspirational roadmap items. **Status: Planned — do NOT block delivery gate.**

---

## (C) Mismatches — Both exist but disagree

---

**ID:** SPEC-treeview-028
**Type:** mismatch
**Parameter/Event:** EnableDragDrop vs Draggable
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Draggable | EnableDragDrop |
| Type | bool | bool |
| Description | Same semantics, different name | Same semantics, different name |

**Recommended action:** Decide canonical name; update one side
**Delegated to:** spec update or code rename

---

**ID:** SPEC-treeview-029
**Type:** mismatch
**Parameter/Event:** OnItemDrop vs OnDrop
**Priority:** P1

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | OnDrop | OnItemDrop |
| Type | EventCallback<TreeViewDropEventArgs> | EventCallback<(string DraggedId, string TargetId)> |
| Description | Same semantics; type differs | Simplified tuple instead of typed args |

**Recommended action:** Decide canonical name and type; align spec and code
**Delegated to:** gap-analysis-resolution (requires code change if spec wins)

---

**ID:** SPEC-treeview-030
**Type:** mismatch
**Parameter/Event:** TreeSelectionMode vs TreeViewSelectionMode
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | TreeViewSelectionMode | TreeSelectionMode |
| Type | enum | enum |
| Description | Same values (None, Single, Multiple) | Same values |

**Recommended action:** Decide canonical name; update one side
**Delegated to:** spec update or code rename

---

**ID:** SPEC-treeview-031
**Type:** mismatch
**Parameter/Event:** AllowCheckChildren vs CheckChildren
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | CheckChildren | AllowCheckChildren |
| Type | bool | bool |
| Description | Same semantics | Same semantics |

**Recommended action:** Decide canonical name
**Delegated to:** spec update or code rename

---

**ID:** SPEC-treeview-032
**Type:** mismatch
**Parameter/Event:** AllowCheckParents vs CheckParents
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | CheckParents | AllowCheckParents |
| Type | bool | bool |
| Description | Same semantics | Same semantics |

**Recommended action:** Decide canonical name
**Delegated to:** spec update or code rename

---

**ID:** SPEC-treeview-033
**Type:** mismatch
**Parameter/Event:** Collection parameter types
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | ExpandedItems, SelectedItems, CheckedItems | Same |
| Type | IEnumerable<object> | IEnumerable<string>? |
| Description | Spec uses object references | Code uses string IDs |

**Recommended action:** Update spec to reflect string-ID design decision
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-034
**Type:** mismatch
**Parameter/Event:** OnItemClick event args type
**Priority:** P2

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | OnItemClick | OnItemClick |
| Type | EventCallback<TreeViewItemClickEventArgs> | EventCallback<object> |
| Description | Typed event args in spec | Raw object in code |

**Recommended action:** Either add typed args class or update spec
**Delegated to:** gap-analysis-resolution (if typed args needed)

---

## Cross-References

- Gap workspace: /workspaces/Marilo/workspaces/gap-analysis-resolution
- TreeView closure reports: stages/06-validate/output/gap-treeview-closure-report.md
- Phase 2.5 closures: gap-expandall-lazyload-closure-report.md, gap-readonly-guards-closure-report.md
