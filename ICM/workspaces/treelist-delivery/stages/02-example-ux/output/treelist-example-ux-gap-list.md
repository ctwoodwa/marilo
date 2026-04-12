# Stage 02 — Example UX Gap List: MariloTreeList
**Audited:** 2026-04-11  
**Demo page:** `samples/Marilo.Demo/Pages/Components/TreeList/Overview.razor`  
**Spec directory:** `docs/component-specs/treelist/`  
**Stage 01 source:** `stages/01-spec-review/output/treelist-spec-gap-list.md`

---

## Summary

| Category | Count |
|---|---|
| Missing (no demo scenario exists) | 28 |
| Partial (demo exists but does not exercise the feature interactively or correctly) | 1 |
| Blocked-by-source (spec feature not yet implemented — cannot demo) | 16 |
| Complete (scenario present and correct) | 0 |

The demo page `Overview.razor` contains a **single placeholder section** that renders an info alert and a static code snippet. It does not contain any live, interactive component usage. All spec feature areas lack demo coverage.

---

## Existing Demo Inventory

**File:** `samples/Marilo.Demo/Pages/Components/TreeList/Overview.razor`  
**Lines:** 1–22

| Section Title | Live Component? | Parameters Exercised | Events Exercised | Status |
|---|---|---|---|---|
| "Basic Usage" | No — only a static code string in `_basicCode` | None (code snippet references `MariloGridColumn`, which is the DataGrid column, not `TreeListColumn`) | None | Stale/Partial |

**Stale reference found:** `_basicCode` at line 17 references `<MariloGridColumn Field="Name" Title="Name" />` — this is the DataGrid column component, not a TreeList column. The correct component per spec is `<TreeListColumn>` inside `<TreeListColumns>`. This snippet would not compile against the intended TreeList API.

---

## Section 1 — Missing Scenarios

Spec feature areas with no corresponding demo section at all.

---

### 1.1 Data Binding

**Spec area:** `docs/component-specs/treelist/data-binding/`

| Gap ID | Missing Scenario | Spec Reference | Notes |
|---|---|---|---|
| UX-treelist-001 | Flat self-referencing data (`IdField` + `ParentIdField`) | `data-binding/flat-data.md` | The most fundamental binding mode; every TreeList usage depends on this |
| UX-treelist-002 | Hierarchical nested data (`ItemsField`) | `data-binding/hierarchical-data.md` | Second binding mode supported by source (`MariloTreeList.razor` line 51) |
| UX-treelist-003 | Load-on-demand with `OnExpand` + `HasChildrenField` | `data-binding/load-on-demand.md` | Blocked by source (see Blocked section); `OnExpand` not yet implemented |

**Priority:** P1 (UX-treelist-001, UX-treelist-002 are demoable today with current source)

---

### 1.2 Paging

**Spec area:** `docs/component-specs/treelist/paging.md`

| Gap ID | Missing Scenario | Spec Reference | Notes |
|---|---|---|---|
| UX-treelist-004 | Enable paging (`Pageable`, `PageSize`) | `paging.md` lines 24-26 | Blocked by source — `Pageable`/`PageSize` not implemented |
| UX-treelist-005 | Bind page index (`@bind-Page`) | `paging.md` line 26 | Blocked by source |
| UX-treelist-006 | Dynamic page size selector | `paging.md` lines 108-130 | Blocked by source |
| UX-treelist-007 | Pager settings (InputType, PageSizes, ButtonCount, Adaptive, Position) | `paging.md` lines 215-228 | Blocked by source |

**Priority:** P1 (UX-treelist-004–007 blocked until paging is implemented)

---

### 1.3 Sorting

**Spec area:** `docs/component-specs/treelist/sorting.md`

| Gap ID | Missing Scenario | Spec Reference | Notes |
|---|---|---|---|
| UX-treelist-008 | Single-column sorting (`Sortable="true"`) | `sorting.md` line 16 | Blocked by source |
| UX-treelist-009 | Multi-column sorting (`SortMode="Multiple"`) | `sorting.md` line 25 | Blocked by source |

**Priority:** P1 (blocked)

---

### 1.4 Filtering

**Spec area:** `docs/component-specs/treelist/filter/`

| Gap ID | Missing Scenario | Spec Reference | Notes |
|---|---|---|---|
| UX-treelist-010 | Filter row (`FilterMode="FilterRow"`) | `filter/filter-row.md` | Blocked by source |
| UX-treelist-011 | Filter menu (`FilterMode="FilterMenu"`) | `filter/filter-menu.md` | Blocked by source |
| UX-treelist-012 | Checkbox list filter | `filter/checkboxlist.md` | Blocked by source |
| UX-treelist-013 | Searchbox in toolbar | `filter/searchbox.md` | Blocked by source |

**Priority:** P1 (blocked)

---

### 1.5 Selection

**Spec area:** `docs/component-specs/treelist/selection/`

| Gap ID | Missing Scenario | Spec Reference | Notes |
|---|---|---|---|
| UX-treelist-014 | Row selection — single (`SelectionMode="Single"`) | `selection/rows.md` | Blocked by source |
| UX-treelist-015 | Row selection — multiple (`SelectionMode="Multiple"`) | `selection/rows.md` | Blocked by source |
| UX-treelist-016 | Cell selection | `selection/cells.md` | Blocked by source |
| UX-treelist-017 | Pre-selected items (`SelectedItems` initial value) | `selection/overview.md` line 37 | Blocked by source |

**Priority:** P1 (blocked)

---

### 1.6 Editing

**Spec area:** `docs/component-specs/treelist/editing/`

| Gap ID | Missing Scenario | Spec Reference | Notes |
|---|---|---|---|
| UX-treelist-018 | Inline editing (`EditMode="Inline"`) with command column | `editing/inline.md` | Blocked by source |
| UX-treelist-019 | Popup editing (`EditMode="Popup"`) | `editing/popup.md` | Blocked by source |
| UX-treelist-020 | In-cell editing (`EditMode="Incell"`) | `editing/incell.md` | Blocked by source |
| UX-treelist-021 | Editing with validation | `editing/validation.md` | Blocked by source |

**Priority:** P1 (blocked)

---

### 1.7 Column Features

**Spec area:** `docs/component-specs/treelist/columns/`

| Gap ID | Missing Scenario | Spec Reference | Notes |
|---|---|---|---|
| UX-treelist-022 | Auto-generated columns (no `<TreeListColumns>` block) | `columns/auto-generated.md` | Blocked by source (child content not implemented) |
| UX-treelist-023 | Frozen (locked) columns | `columns/frozen.md` | Blocked by source |
| UX-treelist-024 | Column reorder by drag | `columns/reorder.md` | Blocked by source |
| UX-treelist-025 | Column resize | `columns/resize.md` | Blocked by source |
| UX-treelist-026 | Column visibility toggle (`Visible` parameter) | `columns/visible.md` | Blocked by source |
| UX-treelist-027 | Column menu | `columns/menu.md` | Blocked by source |
| UX-treelist-028 | Multi-column headers | `columns/multi-column-headers.md` | Blocked by source |
| UX-treelist-029 | Virtual columns | `columns/virtual.md` | Blocked by source |
| UX-treelist-030 | Checkbox column | `columns/checkbox.md` | Blocked by source |
| UX-treelist-031 | Command column | `columns/command.md` | Blocked by source |
| UX-treelist-032 | Display format (`DisplayFormat`) | `columns/display-format.md` | Blocked by source |

---

### 1.8 Templates

**Spec area:** `docs/component-specs/treelist/templates/`

| Gap ID | Missing Scenario | Spec Reference | Notes |
|---|---|---|---|
| UX-treelist-033 | Cell template (`Template`) | `templates/column.md` | Blocked by source |
| UX-treelist-034 | Row template | `templates/row.md` | Blocked by source |
| UX-treelist-035 | Header cell template | `templates/column-header.md` | Blocked by source |
| UX-treelist-036 | Editor template | `templates/editor.md` | Blocked by source |
| UX-treelist-037 | No-data template | `templates/no-data-template.md` | Blocked by source |
| UX-treelist-038 | Filter template | `templates/filter.md` | Blocked by source |
| UX-treelist-039 | Pager template | `templates/pager.md` | Blocked by source |
| UX-treelist-040 | Popup form template | `templates/popup-form-template.md` | Blocked by source |
| UX-treelist-041 | Popup buttons template | `templates/popup-buttons-template.md` | Blocked by source |
| UX-treelist-042 | Column chooser template | `templates/column-chooser.md` | Blocked by source |

---

### 1.9 Other Top-Level Features

| Gap ID | Missing Scenario | Spec Reference | Priority | Blocked? |
|---|---|---|---|---|
| UX-treelist-043 | Toolbar with Add/SearchBox/custom tools | `toolbar.md` | P1 | Yes |
| UX-treelist-044 | Row drag-drop (`OnRowDrop`) | `row-drag-drop.md` | P2 | Yes |
| UX-treelist-045 | Aggregates | `aggregates.md` | P2 | Yes |
| UX-treelist-046 | State get/set (`OnStateInit`, `OnStateChanged`) | `state.md` | P2 | Yes |
| UX-treelist-047 | Refresh data / `Rebind()` method | `refresh-data.md` | P2 | Yes |
| UX-treelist-048 | Virtual scrolling | `virtual-scrolling.md` | P2 | Yes |
| UX-treelist-049 | Keyboard navigation (`Navigable="true"`) | `overview.md` line 151 | P3 | Yes |
| UX-treelist-050 | Accessibility — WAI-ARIA attributes | `accessibility/wai-aria-support.md` | P2 | Partial — source renders `role="treegrid"` and `aria-level` but no demo validates it |
| UX-treelist-051 | `OnExpand` / `OnCollapse` events | `events.md` lines 43-48 | P1 | Yes |
| UX-treelist-052 | `OnRowClick` — basic interaction scenario | `events.md` line 36 | P1 | No — implementable today |
| UX-treelist-053 | `OnRowDoubleClick` | `events.md` line 36 | P2 | Yes |
| UX-treelist-054 | `OnRowRender` — custom row CSS | `events.md` line 38 | P2 | Yes |
| UX-treelist-055 | Width / Height parameters | `overview.md` lines 152-153 | P2 | Yes |

---

## Section 2 — Partial Scenarios

Scenarios that partially exist but do not meet the demo-scenario-format requirements.

---

**Gap ID:** UX-treelist-P01  
**Scenario:** Basic Usage  
**Demo file:** `samples/Marilo.Demo/Pages/Components/TreeList/Overview.razor` lines 6–13  
**What exists:** A `<DemoSection>` block with an info alert and a `_basicCode` static string.  
**What is missing:**
- No live `<MariloTreeList>` component renders — the section shows only an `<MariloAlert>` placeholder.
- The `_basicCode` static string (line 17) references `<MariloGridColumn>` — this is the DataGrid column component, not a TreeList column. This is a stale/incorrect reference.
- No user-controllable input.
- No parameter table.
- No link to spec section.

**Action required:** Replace the placeholder with a real live flat-data binding scenario (UX-treelist-001). This is the minimum viable demo scenario and is demoable with current source.

---

## Section 3 — Blocked-by-Source

Scenarios that cannot be written until the corresponding source implementation lands. These are tracked here to provide exact demo requirements for each implementation ticket.

| Gap ID | Feature | Spec File(s) | Stage 01 Gap ID(s) | Demo Requirement When Unblocked |
|---|---|---|---|---|
| UX-treelist-004–007 | Paging | `paging.md` | SPEC-treelist-002–004, 025–026, 047 | Interactive page size selector + page navigator |
| UX-treelist-008–009 | Sorting | `sorting.md` | SPEC-treelist-005–006 | Clickable column header that re-sorts; indicator arrow visible |
| UX-treelist-010–013 | Filtering | `filter/*.md` | SPEC-treelist-007 | Toggle between FilterRow and FilterMenu; searchbox in toolbar |
| UX-treelist-014–017 | Selection | `selection/*.md` | SPEC-treelist-008–010, 022–023 | Radio-style and checkbox-style selection; selected items displayed below grid |
| UX-treelist-018–021 | Editing | `editing/*.md` | SPEC-treelist-014, 029 | Inline, popup, incell scenarios with CRUD callbacks logged |
| UX-treelist-022 | Auto-generated columns | `columns/auto-generated.md` | SPEC-treelist-045 | Model class bound without any column declarations |
| UX-treelist-023–032 | Column features | `columns/*.md` | SPEC-treelist-030–043 | Each column feature needs its own interactive section |
| UX-treelist-033–042 | Templates | `templates/*.md` | SPEC-treelist-044 | At minimum: cell template, header template, no-data template |
| UX-treelist-003 | Load-on-demand | `data-binding/load-on-demand.md` | SPEC-treelist-016, M04 | Lazy expand triggering an async data fetch |
| UX-treelist-043 | Toolbar | `toolbar.md` | SPEC-treelist-046 | Toolbar with Add button and custom button |
| UX-treelist-044 | Row drag-drop | `row-drag-drop.md` | SPEC-treelist-021 | Drag between rows; drop event logged |
| UX-treelist-045 | Aggregates | `aggregates.md` | (not in stage 01 — spec-ahead feature) | Sum/Average per column |
| UX-treelist-046 | State | `state.md` | SPEC-treelist-027–028 | Save/load state via JSON; restore sorting + paging |
| UX-treelist-047 | Refresh / Rebind | `refresh-data.md` | (method — not a parameter) | Button that calls `Rebind()` |
| UX-treelist-048 | Virtual scrolling | `virtual-scrolling.md` | (column virtual: SPEC-treelist-035) | Large dataset (10k rows); no pager |
| UX-treelist-051 | OnExpand / OnCollapse | `events.md` | SPEC-treelist-016–017 | Event log panel showing expand/collapse actions |

---

## Demoable Today (No Source Blocker)

The following scenarios can be written against the current source and should be prioritised for the next demo iteration:

| Gap ID | Scenario | Source Parameters Available |
|---|---|---|
| UX-treelist-001 (fix P01) | Flat self-referencing data | `Data`, `IdField`, `ParentIdField`, `Columns`, `OnRowClick` |
| UX-treelist-002 | Hierarchical nested data | `Data`, `ItemsField`, `HasChildrenField`, `Columns` |
| UX-treelist-052 | `OnRowClick` event | `OnRowClick` (note: type mismatch SPEC-treelist-M01 — fires `TItem` not event args) |

**Note on UX-treelist-052:** The `OnRowClick` in source fires `TItem` directly (source line 34), not a `TreeListRowClickEventArgs<T>` as spec requires. The demo scenario should document this discrepancy in the parameter table notes until SPEC-treelist-M01 is resolved.

---

## Demo Scenario Format Checklist (for future scenario authors)

When writing new scenarios, each must include (per `stages/02-example-ux/shared/demo-scenario-format.md`):

- [ ] Scenario title that describes a real use case (not "Test" or "Example 1")
- [ ] 1–2 sentence description of when a developer would use this
- [ ] Live interactive `<MariloTreeList>` usage
- [ ] At least one user-controllable input that changes behaviour in real time
- [ ] Code snippet panel with minimal Razor markup
- [ ] Parameter table: Parameter | Value in scenario | Notes
- [ ] Link/anchor to corresponding spec section

Edge cases to cover before the demo page is complete:

- [ ] Empty/no-data state (pass empty `Data`)
- [ ] Disabled column state
- [ ] `null` data (initial load before data arrives)
- [ ] Very deep hierarchy (5+ levels of nesting)
