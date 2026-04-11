# DataGrid Example-UX Gap List

## 2026-04-11 orchestrator wave 2 (subagent dispatch)

**Session:** `marilo-grid-pipeline-2026-04-11-1200`
**Worker:** `w-datagrid-delivery` (ICM stage 02-example-ux, Wave 2)
**Audit date:** 2026-04-11
**Scope:** Inventory every MariloDataGrid demo under `samples/Marilo.Demo/Pages/Components/DataGrid/` and cross-reference against Wave 1 focus spec topics: `selection/overview.md`, `selection/rows.md`, `selection/cells.md`, `keyboard-navigation.md`, `refresh-data.md`, `editing/overview.md`. Wave 1 gap list (`ICM/workspaces/datagrid-delivery/stages/01-spec-review/output/datagrid-spec-gap-list.md`) consulted to tag `Blocked-by-source` where the scenario requires not-yet-implemented source API.

Cross-reference with Wave 1 gaps: SA-01..SA-14, SRC-01..SRC-08, NM-01..NM-06 (Wave 1 section) and S-03, S-15, M-05, M-13 (refresh pass).

### Demo Inventory (DataGrid only; MariloDataSheet excluded per scope)

| # | File | Route | Sections |
|---|---|---|---|
| D1 | `samples/Marilo.Demo/Pages/Components/DataGrid/Overview.razor` | `/components/DataGrid` and `/components/DataGrid/overview` | Basic Usage, Auto-Generated Columns, Paging (Basic / PageSize), Sorting (Single / Multi / Disable-per-col), Filtering (FilterRow / FilterMenu / SearchBox), Selection (Single / Multiple+Checkboxes), Grouping (Basic / Header+Footer templates), Editing (Inline / Popup / InCell) |
| D2 | `samples/Marilo.Demo/Pages/Components/DataGrid/Events.razor` | `/components/DataGrid/events` | Row Click, Row Double-Click, Row Context Menu, OnRowExpand/Collapse, CRUD event lifecycle (OnEdit/Create/Update/Delete/Cancel/ModelInit), OnStateChanged, PageChanged, SelectedItemsChanged |
| D3 | `samples/Marilo.Demo/Pages/Components/DataGrid/Appearance.razor` | `/components/DataGrid/appearance` | Striped rows, Width/TextAlign, Height scrolling, Fixed width, Column Visibility toggles, OnRowRender, OnCellRender, RowTemplate |
| D4 | `samples/Marilo.Demo/Pages/Components/DataGrid/Accessibility.razor` | `/components/DataGrid/accessibility` | Navigable grid, ARIA attribute showcase, Accessible inline editing, Accessible filter row + searchbox |

**Total DataGrid demos:** 4 pages, ~32 demo sections. No sub-pages for selection, editing, keyboard, or refresh-data topic areas — all covered inline within the four top-level pages.

---

### Classification — Wave 1 focus topics

Format: **scenario** — **status** — rationale. Statuses: `Covered` / `Partial` / `Missing` / `Orphan` / `Blocked-by-source`.

#### Topic 1 — `selection/overview.md` (selection basics)

| Scenario | Status | Notes |
|---|---|---|
| `SelectionMode.None` (default disables selection) | **Missing** | No demo explicitly shows `None` as a configured state or documents the default-off behavior. Trivial to add. |
| `SelectionMode.Single` — row selection | **Covered** | D1 "Single Selection" + D4 "Navigable Grid" exercise this. |
| `SelectionMode.Multiple` — row selection | **Covered** | D1 "Multiple Selection with Checkboxes", D2 "Selection Changed". |
| `SelectedItems` two-way binding | **Partial** | D1 / D2 / D4 all bind `SelectedItems` one-way with explicit `SelectedItemsChanged`. No demo uses `@bind-SelectedItems` shorthand. |
| `SelectedCells` two-way binding (cell unit) | **Missing** | Zero DataGrid demo exercises `SelectionUnit=Cell` or `SelectedCells`, even though source supports it (Wave 1 refresh confirmed `SelectionUnit`, `SelectedCells`, `SelectedCellsChanged` at `MariloDataGrid.razor.cs:107-116`). |
| `SelectedItemsChanged` event | **Covered** | D2 "Selection Changed" section. |
| `SelectedCellsChanged` event | **Missing** | Not demoed. Source available. |
| Pre-selection via pre-populated collection | **Missing** | No demo pre-seeds `SelectedItems` or `SelectedCells` to show initial selection. |

#### Topic 2 — `selection/rows.md` (row selection basics, events, integration)

| Scenario | Status | Notes |
|---|---|---|
| Click-to-select row | **Covered** | D1 Single/Multiple. |
| Shift-click range selection | **Blocked-by-source** | Wave 1 SA-04 confirms no Shift/Ctrl modifier handling in row click. Cannot demo until source implements. |
| Ctrl-click toggle selection | **Blocked-by-source** | Wave 1 SA-04, same scan. |
| CheckBoxColumn-driven selection (`ShowCheckboxColumn`) | **Covered** | D1 "Multiple Selection with Checkboxes", D2 "Selection Changed". |
| `<GridCheckboxColumn SelectAll="true">` dedicated element + `CheckBoxOnlySelection` | **Blocked-by-source** | Wave 1 SA-03 — no `GridCheckboxColumn` component exists in source; only the flat `ShowCheckboxColumn` bool. |
| `<GridSelectionSettings SelectionType="Row">` alternate enablement | **Blocked-by-source** | Wave 1 SA-02 — type absent from source. |
| `SelectedItemsChanged` async-limitation note (use `OnRowClick` for async) | **Missing** | No demo illustrates the spec guidance to use `OnRowClick` / `OnRowDoubleClick` when async is required; could share D2 infrastructure. |
| Selection persists across paging | **Partial** | D1 "Multiple Selection with Checkboxes" has no paging; D4 Navigable Grid has paging + single selection but does not call out persistence as the scenario. Gap: an explicit "select across pages" demo section is missing. |
| Selection cleared on row drag-drop | **Blocked-by-source** | Wave 1 SA-13 — row-drop handler does not clear `_selectedItems`. Behavior undefined. |
| Selection + Inline edit mode compatibility | **Partial** | Editing demos exist (D1 Inline) and selection demos exist (D1 Single/Multiple), but no combined demo binds a grid with both `SelectionMode` AND `EditMode=Inline`. |
| Selection + InCell edit mode (checkbox-only selection) | **Blocked-by-source** | Wave 1 SA-12 — source has no guard enforcing "InCell requires checkbox-column selection". The scenario is undefined, so the demo cannot yet exist. |
| Selection + Popup edit mode | **Partial** | Same as Inline — no combined demo. |
| Selection + virtual scrolling (range limited to rendered rows) | **Missing** | `EnableVirtualization` exists in source (U-02 / M-03), but no demo exercises selection + virtualization together. |
| Overriding `Equals`/`GetHashCode` for `OnRead`-bound data | **Missing** | Spec highlights this as a common pitfall. No demo. |
| Selection with column template / row template (custom markup + checkbox) | **Missing** | D3 has `RowTemplate` demo and D1 has selection demo, but not combined. Row-template + selection is a known tricky interaction. |

#### Topic 3 — `selection/cells.md` (cell selection)

| Scenario | Status | Notes |
|---|---|---|
| Click-to-select cell (`SelectionUnit.Cell`) | **Missing** | Source supports it (`MariloDataGrid.razor.cs:107-116`) — no demo wires `SelectionUnit=Cell`. High-value gap since Wave 1 marked this source-closed. |
| Shift-click cell range selection | **Blocked-by-source** | Wave 1 SA-04 — no Shift/Ctrl handling. |
| Ctrl-click cell toggle | **Blocked-by-source** | Wave 1 SA-04. |
| Drag-to-select cells (`DragToSelect`) | **Blocked-by-source** | Wave 1 SA-01 — `DragToSelect` parameter/`GridSelectionSettings` absent from source. |
| `SelectedCells` binding + iteration using `GridCellReference` | **Missing** | No demo binds `SelectedCells`. |
| `SelectedCellsChanged` event | **Missing** | Not demoed. |
| `GridCellReference` members (`Item`, `Field`, `Value`, `RowIndex`) | **Missing** | No demo surfaces these for user inspection. Also flags NM-02 ambiguity (spec uses both `GridCellReference<TItem>` and `GridSelectedCellDescriptor` names). |
| Cell selection + paging persistence | **Missing** | Not demoed. |
| Cell selection + virtual scrolling caveat | **Missing** | Not demoed. |
| Cell selection + column template (`stopPropagation` for clickable elements) | **Missing** | Not demoed. |
| "Cell selection NOT supported with row template" guidance | **Missing** | Not demoed. Partially **Blocked-by-source** since spec limitation may not yet be enforced. |

#### Topic 4 — `keyboard-navigation.md`

| Scenario | Status | Notes |
|---|---|---|
| `Navigable=true` enables keyboard nav | **Partial** | D4 "Navigable Grid" sets `Navigable=true` and shows a shortcut cheat-sheet. However, Wave 1 SA-06 reports that the underlying key dispatch is NOT implemented in source (`razor.cs:139` only exposes the bool; no `onkeydown` handler). The demo **claims** functionality the source does not deliver — this is a high-severity demo honesty gap: status effectively `Blocked-by-source` for every sub-scenario below. |
| Arrow-key cell navigation | **Blocked-by-source** | SA-06. D4 cheat-sheet documents it; source does not implement. |
| Home/End, Ctrl+Home/End | **Blocked-by-source** | SA-06. |
| PageUp/PageDown | **Blocked-by-source** | SA-06. |
| Enter on header = sort | **Blocked-by-source** | SA-06 / SA-07 — no header keydown handler. |
| Enter / F2 on data cell = enter edit | **Blocked-by-source** | SA-07. |
| Esc on data cell = cancel edit | **Blocked-by-source** | SA-07 / SA-08. |
| Space / Ctrl+Space / Shift+Space row selection | **Blocked-by-source** | SA-07. |
| Delete / Backspace fires `OnDelete` | **Blocked-by-source** | SA-07. |
| Alt+Down opens Filter Menu / Column Menu | **Blocked-by-source** | SA-07; plus S-12 (no column menu component). |
| Ctrl+Space group/ungroup column | **Blocked-by-source** | SA-07. |
| Ctrl+Arrow reorder column | **Blocked-by-source** | SA-07; plus U-05 (grid-level reorder). |
| Tab / Shift+Tab across inline editors | **Blocked-by-source** | SA-08 — no editor focus management. |
| Tab / Shift+Tab in popup edit form | **Blocked-by-source** | SA-08. |
| Detail template row Enter-to-toggle + focus trap | **Blocked-by-source** | SA-07. |
| Command column button activation | **Blocked-by-source** | SA-07. |
| Checkbox column Space toggle | **Blocked-by-source** | SA-07. |
| `CustomKeyboardShortcuts` dictionary override | **Blocked-by-source** | Wave 1 S-03 / SA-05 — `CustomKeyboardShortcuts` param, `GridKeyboardScope` and `GridKeyboardCommand` enums all absent from source. |

> **Headline demo-honesty risk:** D4 "Navigable Grid" advertises a 5-row keyboard cheat sheet in its description and inline `<kbd>` panel. None of it works. A Wave 2 remediation should either (a) remove the cheat sheet until keyboard dispatch is shipped, or (b) scope D4 to ARIA attributes only and park keyboard examples behind a "Pending keyboard engine" note.

#### Topic 5 — `refresh-data.md`

| Scenario | Status | Notes |
|---|---|---|
| `Rebind()` method on `@ref` grid | **Missing** | No demo exercises `Rebind()`. Source exposes it (`razor.cs:511`, Wave 1 SRC-05). |
| Observable data — `ObservableCollection<T>` live-update | **Missing** | Not demoed. `ObservableCollection` binding is a common pattern. |
| New collection reference (swap `Data`) | **Missing** | Not demoed. |
| Call `OnRead` via `SetStateAsync(GetState())` | **Missing** | No demo uses `OnRead` at all, let alone manual rebind. Source has `OnRead` + `SetStateAsync` per Wave 1 refresh. |
| Entity Framework rebind pattern | **Missing** | Not demoed. Partial scope-creep, but the spec explicitly calls out this pitfall. |
| Auto-rebind after edit operations | **Partial / Blocked-by-source** | Wave 1 SA-11 flags uncertainty about whether source fires `OnRead` automatically after `OnCreate`/`OnUpdate`/`OnDelete`/`OnCancel`. No demo verifies. Cannot cleanly demo until behavior confirmed or fixed. |

**Topic 5 headline:** `refresh-data.md` has zero demo coverage across all five spec scenarios. This is a full-topic coverage gap.

#### Topic 6 — `editing/overview.md`

| Scenario | Status | Notes |
|---|---|---|
| `EditMode.Inline` basic flow | **Covered** | D1 "Inline Editing", D2 "CRUD Event Lifecycle". |
| `EditMode.Popup` basic flow | **Covered** | D1 "Popup Editing". |
| `EditMode.InCell` basic flow | **Partial** | D1 "InCell Editing" wires `OnUpdate`, but does not demo the Tab / Shift+Tab auto-advance behavior (spec L130-137) — that is **Blocked-by-source** per SA-08 anyway. |
| `OnAdd` event | **Partial** | D2 lifecycle demo fires `OnEdit/Create/Update/Delete/Cancel/ModelInit` but does not surface `OnAdd`. Source exposes it (razor.cs:225). |
| `OnEdit` event | **Covered** | D2. |
| `OnCreate` event | **Covered** | D1 inline/popup, D2. |
| `OnUpdate` event | **Covered** | D1 all three edit modes, D2. |
| `OnDelete` event | **Covered** | D1 inline, D2. |
| `OnCancel` event | **Covered** | D2. |
| `OnModelInit` (for types w/o parameterless ctor) | **Covered** | D1 + D2 demonstrate the pattern of mutating `args.Item` to assign a new model. |
| `OnCommand` typed event | **Missing** | Source exposes `OnCommand` + `GridCommandEventArgs<TItem>` (Wave 1 SRC-04); no demo. |
| Validation integration (DataAnnotations / `EditContext`) | **Blocked-by-source** | Wave 1 S-15 — edit pipeline does not wire validation. |
| Column `EditorType` enum (`CheckBox`/`Switch`/`DatePicker`/`DateTimePicker`/`TimePicker`/`TextArea`/`TextBox`) | **Blocked-by-source** | Wave 1 SA-09 — `EditorType` parameter and `GridEditorType` enum absent from `MariloGridColumn`. D1 works around via `<EditorTemplate>` with raw `<input>` — which is the *only* path currently available. |
| Custom `EditorTemplate` | **Covered** | D1 all three edit modes use `<EditorTemplate>`. |
| `ConfirmDelete` dialog (browser `confirm` per SRC-06) | **Missing** | D1/D2 delete flows do not set `ConfirmDelete=true`. Also Wave 1 SRC-06 flags that source delegates to `window.confirm`, not a `MariloDialog` — demoing would expose this UX tension. |
| `NewRowPosition.Top` / `Bottom` | **Blocked-by-source** | Wave 1 SA-10 — parameter missing. |
| `GridCommandColumn` + `GridCommandButton` elements | **Blocked-by-source** | Wave 1 S-05 — not present in source. D1 relies on built-in implicit command rendering, not the spec's explicit command-column element. |
| Toolbar `Add` button flow | **Partial** | D1 inline editing relies on implicit Add via the grid's internal command handling. No demo explicitly wires `<ToolbarTemplate>` with an Add command. |
| Programmatic Begin/Save/Cancel (imperative API) | **Missing** | Wave 1 SRC-03 — source exposes `BeginEdit`, `BeginCellEdit`, `BeginAdd`, `SaveEdit`, `CancelEdit`, `DeleteItem`, `ExecuteCommand` as public methods; zero demo coverage. |
| Programmatic add/edit via `GridState` | **Missing** | Spec mentions; not demoed. |
| Detail template + edit mode integration | **Missing** | D2 has detail template demo separately; not combined with edit. |

---

### Orphaned demos (demo exists but no matching Wave 1 focus-topic spec coverage)

These demo sections exist but don't map cleanly to the six Wave 1 focus topics. They may map to *other* spec topics (sorting, filtering, grouping, paging, hierarchy, etc.) but are orphans relative to this wave's scope. Not defects; recorded for transparency.

| # | Demo section | Orphan reason |
|---|---|---|
| O-01 | D1 "Auto-Generated Columns" | Maps to `columns/auto-generated.md`, not Wave 1 focus. |
| O-02 | D1 Paging / PageSize selector | Maps to `paging.md`. |
| O-03 | D1 Sorting (single/multi/disable) | Maps to `sorting.md`. |
| O-04 | D1 FilterRow / FilterMenu / SearchBox | Maps to `filter/*` and `filter/searchbox.md`. |
| O-05 | D1 Grouping (basic + header/footer templates) | Maps to `grouping/*` and `templates/group-header.md`. |
| O-06 | D2 `OnRowClick` / `OnRowDoubleClick` / `OnRowContextMenu` | Maps to `events.md`. (Wave 1 U-06 flagged `OnRowContextMenu` as undocumented in spec events page — demo is **Orphan — spec gap**.) |
| O-07 | D2 `OnRowExpand` / `OnRowCollapse` | Maps to `hierarchy.md`. Wave 1 U-07 flagged these as undocumented in spec events page — same **Orphan — spec gap** condition. |
| O-08 | D2 `OnStateChanged` + `PageChanged` | Maps to `state.md` + `paging.md`. |
| O-09 | D3 Striped, Width, Height, Column Visibility, RowTemplate | Maps to `sizing.md`, `columns/visible.md`, `columns/width.md`, `templates/row.md`. Wave 1 U-03 (Striped undocumented) makes this another **Orphan — spec gap**. |
| O-10 | D3 `OnRowRender` / `OnCellRender` | Maps to `events.md` / `templates/overview.md`. |
| O-11 | D4 ARIA attribute showcase | Maps to `accessibility/overview.md` and `accessibility/wai-aria-support.md`. Legitimate accessibility content, not in the six Wave 1 focus topics. |
| O-12 | D4 Accessible filter row + searchbox | Same as O-11. |

---

### Coverage Counts (Wave 1 focus topics only)

| Topic | Covered | Partial | Missing | Blocked-by-source | Total scenarios |
|---|---:|---:|---:|---:|---:|
| `selection/overview.md` | 2 | 1 | 4 | 0 | 8 |
| `selection/rows.md` | 2 | 3 | 4 | 6 | 15 |
| `selection/cells.md` | 0 | 0 | 7 | 4 | 11 |
| `keyboard-navigation.md` | 0 | 1 | 0 | 17 | 18 |
| `refresh-data.md` | 0 | 1 | 4 | 0 | 5 |
| `editing/overview.md` | 7 | 3 | 5 | 5 | 20 |
| **TOTAL** | **11** | **9** | **24** | **32** | **77** |

Demos inventoried: **4 files, ~32 sections**.
Orphaned demo sections (mapping outside Wave 1 focus): **12**.

---

### Top 5 Headline Findings

1. **D4 "Navigable Grid" is a demo-honesty defect.** The Accessibility page advertises a full keyboard-shortcut cheat sheet for Tab/Arrow/Home/End/PageUp/PageDown/Enter/F2/Esc/Space, but Wave 1 SA-06..SA-08 confirm that the source has zero key-dispatch wiring behind `Navigable=true` (only the bool is exposed; no `onkeydown` handler). The demo claims capability the component does not deliver. **Recommendation:** scope D4 to ARIA-only + move the keyboard cheat sheet behind a "Pending — `CustomKeyboardShortcuts` engine in flight" banner, OR descope `Navigable` from the demo entirely until the engine lands. This is the single highest-severity finding in the wave.

2. **`refresh-data.md` has zero demo coverage.** None of the five scenarios (Rebind, ObservableCollection, new collection reference, `OnRead` + `SetStateAsync`, EF pattern) are demoed anywhere in `samples/Marilo.Demo/Pages/Components/DataGrid/`. Source supports all of them (per Wave 1 refresh: `Rebind()` L511, `SetStateAsync` L518). This is a full-topic gap and would be cheap wins — suggest a new `DataGrid/RefreshData.razor` page or a new `## Refreshing Data` section on Overview.razor.

3. **Cell selection is source-closed but demo-missing.** Wave 1 refresh confirmed `SelectionUnit`, `SelectedCells`, `SelectedCellsChanged` as implemented (razor.cs L107-116), and the 2026-04-03 gap #10 is closed. But no DataGrid demo wires `SelectionUnit=Cell` — 7 Missing + 4 Blocked-by-source out of 11 cells-topic scenarios. Recommend adding at least one "Cell selection basics" section to D1 with a `GridCellReference` inspection panel.

4. **Keyboard-navigation topic is ~94% Blocked-by-source.** 17 out of 18 scenarios require source key-dispatch + `CustomKeyboardShortcuts` engine that does not exist. This is not a demo problem — the demo gap list here should route to `datagrid-gap-analysis` as a single large intake item, not to Wave 2 demo work. Only 1 "Partial" (the existing D4 page that needs honesty remediation per finding #1).

5. **Three orphan demo sections map to Wave 1 spec gaps (`U-03`, `U-06`, `U-07`).** Striped, OnRowContextMenu, and OnRowExpand/OnRowCollapse are demoed in D3/D2 but undocumented in the corresponding spec pages. These are **Orphan — spec gap**: the demo is correct, the spec needs to catch up. Delegated: spec update only (routes back to Wave 1's spec-update batch, items U-03, U-06, U-07).

**Secondary findings worth recording:**

- Selection + edit-mode integration (Inline, Popup, InCell) has no combined demos even though Inline/Popup are fully source-supported. Missing, not blocked.
- `OnAdd` event and `OnCommand` typed event are source-exposed but not demoed in the CRUD lifecycle section of D2.
- Public imperative edit API (`BeginEdit` / `BeginCellEdit` / `BeginAdd` / `SaveEdit` / `CancelEdit` / `DeleteItem` / `ExecuteCommand`) has zero demo coverage — Wave 1 SRC-03 flagged this for spec update AND it needs a Wave 2 demo slot.
- `ConfirmDelete=true` path is never exercised in any demo, which means the Wave 1 SRC-06 concern (source uses native `window.confirm` instead of `MariloDialog`) is invisible to users inspecting demos. Add at minimum one section that sets `ConfirmDelete=true`.
- No demo exercises `@bind-SelectedItems` two-way shorthand; every selection demo uses explicit `SelectedItems` + `SelectedItemsChanged`. Minor documentation gap.

---

### Demo-level Action Items (suggested for orchestrator triage)

| # | Action | Sync areas affected | Blocked? |
|---|---|---|---|
| A-01 | D4 Navigable Grid honesty fix — remove or gate keyboard cheat sheet | demo | No |
| A-02 | Add `DataGrid/RefreshData.razor` or new Overview section covering Rebind / ObservableCollection / new collection / `OnRead+SetStateAsync` | demo + (optionally spec cross-link) | No |
| A-03 | Add cell-selection section to D1 (`SelectionUnit=Cell`, `SelectedCells`, `SelectedCellsChanged`, `GridCellReference` inspection) | demo | No |
| A-04 | Add combined "Selection + Inline edit" and "Selection + Popup edit" sections to D1 | demo | No |
| A-05 | Add `ConfirmDelete` section to D1 editing or D2 lifecycle | demo | No |
| A-06 | Add `OnAdd` and `OnCommand` sections to D2 | demo | No |
| A-07 | Add imperative API section (Begin*/Save/Cancel/DeleteItem/ExecuteCommand) to D2 or new `DataGrid/ProgrammaticControl.razor` | demo + spec (SRC-03) | No |
| A-08 | Add `@bind-SelectedItems` shorthand variant to an existing selection demo | demo | No |
| A-09 | Keyboard navigation topic — route entire scenario list to `datagrid-gap-analysis` intake (single large item) | n/a | Yes — no Wave 2 demo work possible until source engine lands |
| A-10 | Shift-click / Ctrl-click / drag-to-select demos — route to `datagrid-gap-analysis` (SA-01/SA-04) | n/a | Yes |
| A-11 | `GridCheckboxColumn` with SelectAll + `CheckBoxOnlySelection` — route to `datagrid-gap-analysis` (SA-03) | n/a | Yes |
| A-12 | `EditorType` enum demos — route to `datagrid-gap-analysis` (SA-09) | n/a | Yes |
| A-13 | `NewRowPosition` demo — route to `datagrid-gap-analysis` (SA-10) | n/a | Yes |

A-01 through A-08 are all **demo-only** actions within Wave 2 scope and do NOT require source changes. A-09 through A-13 should not be taken up by Wave 2 demo workers — they must be escalated to gap-analysis intake before any demo work can land.

---

### Files read (scope-verification audit trail)

- `samples/Marilo.Demo/Pages/Components/DataGrid/Overview.razor` (full)
- `samples/Marilo.Demo/Pages/Components/DataGrid/Events.razor` (full)
- `samples/Marilo.Demo/Pages/Components/DataGrid/Appearance.razor` (full)
- `samples/Marilo.Demo/Pages/Components/DataGrid/Accessibility.razor` (full)
- `docs/component-specs/grid/selection/overview.md` (full)
- `docs/component-specs/grid/selection/rows.md` (full)
- `docs/component-specs/grid/selection/cells.md` (full)
- `docs/component-specs/grid/keyboard-navigation.md` (full)
- `docs/component-specs/grid/refresh-data.md` (full)
- `docs/component-specs/grid/editing/overview.md` (full)
- `ICM/workspaces/datagrid-delivery/stages/01-spec-review/output/datagrid-spec-gap-list.md` (full; cross-referenced for Blocked-by-source tagging)

**MariloDataSheet demos were explicitly excluded per scope.**

No source files, test files, or provider files were modified in this pass. No spec files were modified. All writes are confined to files_owned.
