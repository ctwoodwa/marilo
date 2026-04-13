# TreeView Spec Gap List

**Audit Date:** 2026-04-03
**Re-verified:** 2026-04-11 (parameter inventory unchanged; partial closures applied)
**Re-verified:** 2026-04-11 (events.md rewrite; additional closures applied — see 2026-04-11 (events) section below)
**Component:** MariloTreeView / MariloTreeItem
**Spec Directory:** /workspaces/Marilo/docs/component-specs/treeview/
**Source:** /workspaces/Marilo/src/Marilo.Components/Navigation/MariloTreeView.razor.cs

---

## Summary

| Type | Count | Open |
|------|-------|------|
| Undocumented (implemented but not in spec) | 14 | 0 |
| Spec-ahead (documented but not implemented) | 13 | 13 |
| Mismatch (both exist but disagree) | 7 | 4 |
| **Total** | **34** | **17** |

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

## 2026-04-11 Closures (events.md rewrite)

The following gaps were closed by rewriting `docs/component-specs/treeview/events.md` and updating `overview.md`:

- **SPEC-treeview-008** `OnItemContextMenu` — events.md now documents `EventCallback<TreeItemContextMenuEventArgs>` with full properties table (`Item`, `ItemId`, `MouseEventArgs`). **CLOSED.**
- **SPEC-treeview-033** Collection parameter types — events.md example code updated to use `IEnumerable<string>` for `CheckedItems`, `ExpandedItems`, `SelectedItems`. overview.md `ExpandedItems` example also updated to `IEnumerable<string>`. **CLOSED.**
- **SPEC-treeview-034** `OnItemClick` type — events.md now explicitly documents that current source exposes `EventCallback<object>` and notes that `TreeViewItemClickEventArgs` is Planned. **CLOSED (documented).**

Additionally in this pass:
- **OnItemEdit** — new section added to events.md documenting `EventCallback<TreeItemEditEventArgs>` with properties table (`ItemId`, `NewText`). This closes the inline-editing event documentation deferred from SPEC-treeview-005.
- **Planned markers** — `OnExpand`, `OnItemDoubleClick`, `OnItemRender` all marked as **Planned** in events.md.
- **Drag Events** — section rewritten: `OnItemDrop` (current source, tuple type) documented as implemented; `OnDragStart`/`OnDrag`/`OnDrop`/`OnDragEnd` marked as **Planned**. SPEC-028/029 naming decision noted inline.

Remaining open mismatches (require code or spec decision — cannot close by spec-only update):
- **SPEC-028** `EnableDragDrop` vs `Draggable` — name decision needed
- **SPEC-029** `OnItemDrop` vs `OnDrop` (type + name) — code change needed for full resolution
- **SPEC-030** `TreeSelectionMode` vs `TreeViewSelectionMode` — enum name decision
- **SPEC-031** `AllowCheckChildren` vs `CheckChildren` — name decision
- **SPEC-032** `AllowCheckParents` vs `CheckParents` — name decision

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

---

## 2026-04-11 Orchestrator Wave 1

**Auditor:** w-treeview-delivery (tick 6, session `marilo-grid-pipeline-2026-04-11-1200`)
**Source reviewed:** `src/Marilo.Components/Navigation/MariloTreeView.razor` + `MariloTreeView.razor.cs` (1077 lines, 33 `[Parameter]`s, 5 public methods)
**Spec reviewed:** all 21 files under `docs/component-specs/treeview/` (overview, fluent-ui-gap-analysis, 4 data-binding, 3 selection, 3 checkboxes, 2 accessibility, expanded-items, navigation, events, templates, drag-drop, icons, refresh-data)
**Build check:** `dotnet build Marilo.slnx` → exit 0, 0 warnings, 0 errors (14.04s).

### Coverage Table (21 spec topics)

| # | Spec file | Classification | Notes |
|---|---|---|---|
| 1 | `overview.md` | **Covered** | Parameters table + Methods table match source (post 2026-04-11 closures). |
| 2 | `fluent-ui-gap-analysis.md` | **Covered** (aspirational) | Roadmap doc; 10 API extensions documented as Planned; authoritative baseline. |
| 3 | `data-binding/overview.md` | **Partial** | `TreeViewBinding` component table documented; source has flat parameters only. Matches SPEC-015. |
| 4 | `data-binding/flat-data.md` | **Partial** | Entire example wraps `TreeViewBinding` — not renderable against current source. |
| 5 | `data-binding/hierarchical-data.md` | **Partial** | Uses `TreeViewBinding` + `CheckChildren`/`CheckParents` aliases (see SPEC-031/032). |
| 6 | `data-binding/load-on-demand.md` | **Partial** (all 3 examples broken) | Every example uses `OnExpand` + `TreeViewExpandEventArgs`; source has `LoadChildrenAsync` Func. Refines SPEC-016. |
| 7 | `selection/overview.md` | **Partial** | Enum type `TreeViewSelectionMode` vs source `TreeSelectionMode` (SPEC-030). `IEnumerable<object>` vs `IEnumerable<string>` (SPEC-033). |
| 8 | `selection/single.md` | **Partial** | "Ctrl-click to deselect" behavior documented but NOT implemented in source. |
| 9 | `selection/multiple.md` | **Missing** (behavior) | Shift-click range select + Ctrl-click toggle/deselect documented but NOT implemented. Source only handles click → SelectItem. |
| 10 | `checkboxes/overview.md` | **Partial** | `CheckBoxMode` enum type name `TreeViewCheckBoxMode` vs source `CheckBoxMode`. `CheckOnClick` planned (SPEC-020). `CheckChildren`/`CheckParents` rename (SPEC-031/032). |
| 11 | `checkboxes/single.md` | **Partial** | Wraps `TreeViewBinding`; type-name mismatch. |
| 12 | `checkboxes/multiple.md` | **Partial** | Same issues as single. |
| 13 | `expanded-items.md` | **Covered** | Programmatic expand/collapse patterns work with current source (via ExpandedItems binding). |
| 14 | `navigation.md` | **Spec-ahead** | `UrlField` documented but not in source (SPEC-021). |
| 15 | `events.md` | **Partial** | Documents 9 events: 4 implemented (CheckedItemsChanged, ExpandedItemsChanged, OnItemClick, OnItemContextMenu), 5 planned (OnExpand SPEC-016, OnItemDoubleClick SPEC-017, OnItemRender SPEC-018, 4× drag events SPEC-019). |
| 16 | `templates.md` | **Partial** | Spec puts `ItemTemplate` inside `TreeViewBinding` per-level; source has single top-level `ItemTemplate` parameter only. |
| 17 | `drag-drop.md` | **Partial** | Uses `Draggable` (SPEC-028), 4 drag events (SPEC-019), typed `TreeViewDrop*EventArgs` (SPEC-029). |
| 18 | `icons.md` | **Covered** | `IconField` binding aligns with source. |
| 19 | `refresh-data.md` | **Covered** | `Rebind()` + ObservableCollection + new collection ref patterns all work. |
| 20 | `accessibility/overview.md` | **Partial** | Example uses typed `TreeViewItemClickEventArgs`/`TreeViewItemContextMenuEventArgs`; source has bare `object`/custom args. |
| 21 | `accessibility/wai-aria-support.md` | **Partial** | Documents `aria-level`, `aria-setsize`, filter searchbox role, load-more button role; none rendered by source. Also uses `k-treeview-*` CSS prefix (source emits `mar-tree-*`). |

### Summary Counts

| Classification | Count |
|---|---|
| Covered | 5 |
| Partial | 14 |
| Missing (behavior) | 1 |
| Spec-ahead | 1 |
| Blocked-by-source | 0 |
| Orphan | 0 |

### Gap Records — New findings this wave

Each record includes a **cross-ref** column: `Yes` = already addressed by `fluent-ui-gap-analysis.md` (log for tracking, not a new gap); `No` = novel gap; `Partial` = touched by the baseline but not fully resolved.

---

**ID:** SPEC-treeview-035
**Type:** mismatch
**Parameter/Event:** Shift+click range selection (behavior)
**Priority:** P2
**Cross-ref fluent-ui-gap-analysis.md:** No

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `selection/multiple.md` §Basics: "To select a range of nodes hold the `Shift` key and click on two nodes. All the items in-between will be selected. If there is a focused node, range selection starts from that node." | No modifier-key handling in `SelectItem(...)` or `HandleKeyDown`; click toggles Single/Multiple without reading Shift/Ctrl from `MouseEventArgs`. |
| Type | Documented user behavior | Not implemented |
| Description | WAI-ARIA tree multi-select pattern | Missing |

**Recommended action:** Either implement shift-click range selection (follow-on gap for gap-analysis-resolution) or remove the behavior claim from `selection/multiple.md`.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treeview-036
**Type:** mismatch
**Parameter/Event:** Ctrl+click deselect / toggle
**Priority:** P1
**Cross-ref fluent-ui-gap-analysis.md:** No

> Priority upgraded P2 → P1 per orchestrator review record `_orchestrator/reviews/w-treeview-delivery-2026-04-11-1755.md` (user-visible behavior divergence from spec).

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `selection/single.md` + `selection/multiple.md` both state: "To deselect a node hold the `Ctrl` key and click on it." | `SelectItem` in Multiple mode uses `_selectedIds.Remove(id) ?: Add(id)` (toggle on every click regardless of Ctrl); Single mode replaces selection, never deselects. Ctrl key is not read. |
| Type | Documented user behavior | Partial / incorrect |
| Description | Ctrl-click deselect in both single and multiple modes | Single: missing. Multiple: happens on every click (wrong — should only happen with Ctrl). |

**Recommended action:** Decide canonical semantic (match spec → honor Ctrl modifier; or update spec to reflect toggle-on-every-click behavior). Multiple-mode behavior is currently user-visible wrong vs spec.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treeview-037
**Type:** mismatch
**Parameter/Event:** `TreeViewBinding` component (spec-wide impact)
**Priority:** P1
**Cross-ref fluent-ui-gap-analysis.md:** Partial (SPEC-015 already tracks the component gap; this record quantifies the spec-wide blast radius)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Referenced in `data-binding/overview.md`, `flat-data.md`, `hierarchical-data.md`, `load-on-demand.md` (×3 examples), `checkboxes/overview.md` (×4 examples), `single.md`, `multiple.md`, `events.md`, `accessibility/overview.md`, `icons.md`, `refresh-data.md` (×3), `templates.md` — **14 of 21 spec files use `<TreeViewBinding>`**. | Component does not exist; source exposes flat binding parameters (`IdField`, `ParentIdField`, `TextField`, `IconField`, `ItemsField`, `HasChildrenField`, `UrlField`). |
| Type | Component with per-level binding support | Missing |
| Description | Enables per-level field mapping, per-level templates, per-level CheckBoxMode | Not supported — all binding is global |

**Recommended action:** Elevate SPEC-015 to P1. Every spec example that wraps `<TreeViewBindings>` will fail at demo time. Either (a) implement the component or (b) rewrite every affected spec example to use flat parameters.
**Delegated to:** gap-analysis-resolution intake (architecture review required — component introduction)

---

**ID:** SPEC-treeview-038
**Type:** mismatch
**Parameter/Event:** `OnExpand` event — scope re-assessment
**Priority:** P1 (upgrade from SPEC-016 P2)
**Cross-ref fluent-ui-gap-analysis.md:** Partial (baseline acknowledges `LoadChildrenAsync` / `OnExpand` equivalence but did not flag that every load-on-demand example is unusable)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnExpand` (`EventCallback<TreeViewExpandEventArgs>`) | `LoadChildrenAsync` (`Func<object, Task<IEnumerable<object>>>`) |
| Type | Event with `.Expanded`, `.Item` on args | Func — not fired on collapse; returns children directly |
| Description | Used by all 3 examples in `load-on-demand.md` and 1 example in `events.md` | Different programming model |

**Recommended action:** Decide canonical API — add `OnExpand` event (implements SPEC-016) OR rewrite all `load-on-demand.md` examples to use `LoadChildrenAsync`. Priority upgrade because `load-on-demand.md` is currently 100% non-functional against the shipping source.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treeview-039
**Type:** spec-ahead
**Parameter/Event:** `aria-level` / `aria-setsize` / `aria-posinset` attributes
**Priority:** P2
**Cross-ref fluent-ui-gap-analysis.md:** No

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `accessibility/wai-aria-support.md` table: `aria-level`, `aria-setsize` required on `li.k-treeview-item` | `RenderNodes` emits `role="treeitem"`, `aria-expanded`, `aria-selected`, `id` — but no `aria-level`, no `aria-setsize`, no `aria-posinset`. |
| Type | ARIA attributes | Missing |
| Description | Required for WCAG 2.2 AA + WAI-ARIA Authoring Practices tree pattern | Not emitted |

**Recommended action:** Implement aria-level/aria-setsize/aria-posinset on treeitem elements (accessibility compliance gap).
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treeview-040
**Type:** spec-ahead
**Parameter/Event:** Filter input `searchbox` role
**Priority:** P3
**Cross-ref fluent-ui-gap-analysis.md:** No

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `accessibility/wai-aria-support.md` table references `.k-input-inner` with `role=searchbox` + `aria-controls=.k-treeview-lines id` for a built-in filter input. | Source `FilterFunc` has no UI — filtering is programmatic only via the `Func<object,bool>?` parameter. No filter input is rendered. |
| Type | Built-in filter searchbox | Missing |
| Description | Built-in text-input filter control with WAI-ARIA searchbox role | Programmatic filtering only |

**Recommended action:** Either remove searchbox references from wai-aria-support.md (keep programmatic filter) or add a filter input sub-component. Low priority — programmatic FilterFunc is the team's current design choice.
**Delegated to:** spec update (most likely path)

---

**ID:** SPEC-treeview-041
**Type:** mismatch
**Parameter/Event:** CSS class prefix `k-treeview-*` vs `mar-tree-*`
**Priority:** P3
**Cross-ref fluent-ui-gap-analysis.md:** No

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `accessibility/wai-aria-support.md` selectors: `.k-treeview-lines`, `.k-treeview-item`, `.k-treeview-group`, `.k-treeview-lines id`, `.k-input-inner`, `.k-checkbox`, `.k-selected` | Source `RenderNodes` emits: `mar-tree-item`, `mar-tree-item__header`, `mar-tree-item__toggle`, `mar-tree-item__children`, `mar-tree-item__checkbox` |
| Type | CSS selectors documented in accessibility spec | All Marilo-native class names |
| Description | Documentation carries over from Telerik K-prefix conventions | Marilo uses `mar-tree-*` |

**Recommended action:** Update `accessibility/wai-aria-support.md` to use Marilo CSS class names, OR add a note that `k-*` selectors refer to legacy parity mapping. Spec-only change.
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-042
**Type:** spec-ahead
**Parameter/Event:** PageUp / PageDown / Type-ahead keyboard navigation
**Priority:** P3
**Cross-ref fluent-ui-gap-analysis.md:** No (WAI-ARIA gap not in baseline)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `accessibility/wai-aria-support.md` claims "WAI-ARIA Authoring Practices: File Directory Treeview Example" compliance. That pattern specifies PageUp/PageDown (jump by page), typeahead first-character search, and Ctrl+Shift+End (select to end). | Source `HandleKeyDown` handles: ArrowUp/Down/Left/Right, Enter/Space, Home, End, F2, Escape, `*`. Missing: PageUp, PageDown, typeahead, Ctrl+Shift combos. |
| Type | Keyboard navigation | Partial |
| Description | WAI-ARIA tree pattern keyboard completeness | Source is ~70% of the pattern |

**Recommended action:** Add PageUp/PageDown + typeahead to source (accessibility completeness) OR soften the WCAG 2.2 AA / WAI-ARIA compliance claim in the accessibility docs.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-treeview-043
**Type:** mismatch
**Parameter/Event:** `CheckBoxMode` enum type name
**Priority:** P2
**Cross-ref fluent-ui-gap-analysis.md:** No (baseline only surfaces behavioural gaps, not type-name drift)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `TreeViewCheckBoxMode` (referenced in `checkboxes/overview.md`, `single.md`, `multiple.md`, `hierarchical-data.md`, etc.) | `CheckBoxMode` (type in `Marilo.Core.Enums`) |
| Type | Enum type name | Enum type name |
| Description | Same 3 values (None, Single, Multiple) — name differs | — |

**Recommended action:** Decide canonical name (parallel to SPEC-030 `TreeSelectionMode` vs `TreeViewSelectionMode`). Update spec or rename source.
**Delegated to:** spec update or code rename (package with SPEC-030 decision)

---

**ID:** SPEC-treeview-044
**Type:** mismatch
**Parameter/Event:** `ItemTemplate` location (per-level binding vs global parameter)
**Priority:** P2
**Cross-ref fluent-ui-gap-analysis.md:** Partial (baseline flags per-level templates as gap #7 "Templates — Slot-based vs ItemTemplate per level")

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `templates.md` §Basics: "The `ItemTemplate` of a node is defined under the `TreeViewBinding` tag… different templates for the different levels in each `TreeViewBinding` tag." | `[Parameter] public RenderFragment<object>? ItemTemplate` at the top level of `MariloTreeView`; single template for all levels. |
| Type | Per-level `RenderFragment<object>` | Global `RenderFragment<object>?` |
| Description | Different visual per level via multiple TreeViewBindings | One template for whole tree |

**Recommended action:** Tied to SPEC-037 (`TreeViewBinding` component). If the component ships, per-level ItemTemplate becomes natural. Otherwise, update templates.md to use top-level `ItemTemplate`.
**Delegated to:** gap-analysis-resolution intake (blocked on SPEC-037 decision)

---

**ID:** SPEC-treeview-045
**Type:** undocumented
**Parameter/Event:** `*` (asterisk) keyboard handler — expand siblings
**Priority:** P3
**Cross-ref fluent-ui-gap-analysis.md:** No

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Not mentioned in `accessibility/wai-aria-support.md` or any navigation doc | `HandleKeyDown` case `"*"`: expands all siblings of the focused node + the focused node itself, emits `ExpandedItemsChanged`. |
| Type | WAI-ARIA tree pattern shortcut | Implemented |
| Description | Standard tree shortcut but undocumented | Silent feature |

**Recommended action:** Document the `*` shortcut in `accessibility/wai-aria-support.md` keyboard table. Spec-only change.
**Delegated to:** spec update only

---

**ID:** SPEC-treeview-046
**Type:** undocumented
**Parameter/Event:** `IsCancelled` on drag events
**Priority:** P3 (blocked until drag events ship)
**Cross-ref fluent-ui-gap-analysis.md:** Partial (fluent-ui-gap-analysis.md says Marilo is already ahead on drag, but spec documents `IsCancelled` semantics without a source implementation)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `drag-drop.md` §OnDragStart: event args include `IsCancelled: bool` — "Whether the event is to be prevented." | Source only has `OnItemDrop`; no cancellation semantics on any drag event. |
| Type | Cancellation flag on typed drag args | Missing |
| Description | Lets handler cancel a drag | Not supported |

**Recommended action:** Blocked on SPEC-019 (drag event family). When ported, add `IsCancelled` semantics per spec.
**Delegated to:** blocked on SPEC-019

---

**ID:** SPEC-treeview-047
**Type:** mismatch
**Parameter/Event:** Accessibility example uses typed event args not supported by source
**Priority:** P2
**Cross-ref fluent-ui-gap-analysis.md:** Partial (SPEC-034 already flags `OnItemClick` args; this is the cross-cutting impact in the accessibility demo)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `accessibility/overview.md` example binds `OnItemClick="@OnTreeViewItemClick"` where handler signature is `TreeViewItemClickEventArgs args`; `OnItemContextMenu` similarly uses `TreeViewItemContextMenuEventArgs`. | `OnItemClick` is `EventCallback<object>`; `OnItemContextMenu` is `EventCallback<TreeItemContextMenuEventArgs>` — name and shape differ. |
| Type | Typed event args classes | Partially typed |
| Description | Accessibility compliance example does not compile against current source | Will error at demo build |

**Recommended action:** Package with SPEC-034. Either create `TreeViewItemClickEventArgs` / `TreeViewItemContextMenuEventArgs` types, or rewrite the accessibility example to use the current shapes.
**Delegated to:** gap-analysis-resolution intake (paired with SPEC-034)

---

**ID:** SPEC-treeview-048
**Type:** undocumented
**Parameter/Event:** `CheckboxContext` public shape
**Priority:** P3
**Cross-ref fluent-ui-gap-analysis.md:** No

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `overview.md` parameter table mentions `CheckboxTemplate` is `RenderFragment<CheckboxContext>?` but no reference to the `CheckboxContext` shape (Checked, Indeterminate, OnChange, Disabled). | `RenderNodes` builds `new CheckboxContext { Checked, Indeterminate, Disabled, OnChange }` inline. |
| Type | Public context record | Implemented but undocumented |
| Description | Users who bring a custom `CheckboxTemplate` need to know the context fields | Self-documenting via XML doc only |

**Recommended action:** Add a short "CheckboxContext shape" section under `checkboxes/overview.md` documenting the 4 fields.
**Delegated to:** spec update only

---

### Headline Summary

- **Build clean:** `dotnet build Marilo.slnx` → 0 errors / 0 warnings on 2026-04-11 against current workingInProgress tip. Spec audit is structurally comparable to the last run.
- **Existing 2026-04-03 baseline is still fully valid.** Closures recorded on 2026-04-11 (A.01–A.14) already absorbed most overview-level undocumented params. The 34 prior records remain the core of the gap surface.
- **Biggest NEW finding: `TreeViewBinding` blast radius (SPEC-037).** 14 of 21 spec files wrap `<TreeViewBindings>`, and none of those examples build against the current source. Priority should move up to P1 — the decision to ship or redesign the binding component gates every multi-level demo.
- **`OnExpand` / `LoadChildrenAsync` API drift is unusable in the wild (SPEC-038).** All 3 `load-on-demand.md` examples + the events.md example reference `OnExpand` + `TreeViewExpandEventArgs`; source has a Func. Upgrade SPEC-016 from P2 → P1.
- **Selection behavior gap is real (SPEC-035, SPEC-036).** Shift-click range select and Ctrl-click deselect are spec'd in `selection/multiple.md` but not implemented. Multiple-mode toggle currently fires on every click unconditionally — user-visible divergence from the documented behavior.
- **Accessibility spec runs ahead of source in 3 places (SPEC-039, SPEC-040, SPEC-041, SPEC-042).** aria-level/aria-setsize missing, filter searchbox role undocumented vs implementation, CSS class prefix still k-* in wai-aria-support.md, and PageUp/PageDown + typeahead missing from keyboard handler. WCAG 2.2 AA compliance claim is partial.

### Cross-ref roll-up

- **New P1:** SPEC-037 (TreeViewBinding — upgrade), SPEC-038 (OnExpand — upgrade).
- **New P2:** SPEC-035, 036, 039, 043, 044, 047.
- **New P3:** SPEC-040, 041, 042, 045, 046, 048.
- **Count of brand-new novel gaps (cross-ref = No):** 8. Cross-ref = Partial: 5. Cross-ref = Yes: 1 (SPEC-015 scope confirmed).
- **Total open count after this wave:** 23 (prior) + 14 (new) = **37**.
