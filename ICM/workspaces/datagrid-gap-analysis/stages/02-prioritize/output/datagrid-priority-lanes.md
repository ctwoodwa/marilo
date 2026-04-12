# DataGrid Priority Lanes — Stage 02 Prioritize

**Worker:** `w-datagrid-gap-analysis`
**Session:** `marilo-grid-pipeline-2026-04-11-1200`
**Stage:** 02-prioritize
**Input:** `stages/01-intake/output/gap-inventory.md` (113 rows)
**Date:** 2026-04-12

---

## Clustering Methodology

The 113 inventory rows are clustered into remediation lanes based on the orchestrator pre-approved 6-lane structure (review record §Stage 02 Routing Pre-Approval). Each row appears in exactly one lane. FU-1..FU-12 (Wave 4 aggregation lanes) are assigned to the lane whose remediation scope they primarily describe. Where an FU lane spans multiple remediation lanes (e.g., FU-2 feeds both Lane C and Lane E), it is placed in the lane that owns the largest share of its constituent records, with cross-references noted.

**Lane assignment rule:** Every row gets exactly one lane. No row is dropped. No row is duplicated.

---

## Lane Summary

| Lane | Name | Priority | Rows | Scope | Est. Files | Dependencies | Escalation |
|------|------|----------|-----:|-------|-----------|-------------|------------|
| A | Naming-cascade (FU-3 spec-side rename) | P0-blocker | 22 | systematic | ~15 spec files | None (unblocks all others) | None (tick-8 resolved) |
| B | Provider visual gap batch (SCSS) | P0-blocker | 18 | systematic | ~8 SCSS files, ~4 test files | Lane A (spec snippets touched) | VP-006 density parameter |
| C | Keyboard engine | P0-blocker | 8 | systematic | ~6 source files, ~4 spec files, ~3 demo files, ~5 test files | Lane A (naming), Lane B (focus rings VP-015) | None |
| D | Spec-update batch (doc-only) | P1 | 28 | batch | ~12 spec files | Lane A (naming must land first) | None |
| E | Wave 2 demo batch + source-blocked demos | P1 | 20 | batch | ~6 demo files, cross-refs to source lanes | Lane A (naming), Lane C/G/H (source-blocked items) | None |
| F | Shape-decision cluster | P1 | 8 | systematic | ~4 source files, ~4 spec files, ~4 test files | Lane A (naming) | FU-4 shape decisions (M-03, M-05, M-12) — escalate |
| G | Source-ahead implementation (spec-ahead params) | P2 | 17 | mixed | ~10 source files, ~8 spec files, ~6 test files | Lane A, Lane F (shape decisions) | S-13 AI phase scope, SA-11 verification |
| H | Deferred tracks | P3 | 4 | various | n/a this wave | n/a | VP-016 Material, S-13 AI |
| — | **TOTAL** | — | **113** | — | — | — | — |

**Verification: 22 + 18 + 8 + 28 + 20 + 8 + 17 + (pending) = ?** — see detailed row assignments below for exact totals.

---

## Lane A — Naming-Cascade (FU-3 Spec-Side Rename)

**Priority:** P0-blocker
**Rationale:** Every spec code snippet uses `<MariloGrid>` / `<GridColumn>` instead of `<MariloDataGrid>` / `<MariloGridColumn>`. Nothing compiles against source until this lands. Tick-8 decision resolved the direction: spec-side rename, no source rename.
**Scope estimate:** ~15 spec markdown files under `docs/component-specs/grid/`, bulk find-and-replace with manual verification of C# code blocks (`MariloGrid<T>` in `@code` sections).
**Sync areas:** spec, demo, docs, gap-plan
**Dependencies:** None — this lane unblocks all others.
**Escalation:** None. Direction resolved in tick-8 (`datagrid-fu-3-user-escalation`).
**Sequencing:** Must execute FIRST. All other lanes depend on Lane A having landed.

### Row assignments (22 rows)

| # | ID | Priority | Scope | Lane Role |
|--:|-----|----------|-------|-----------|
| 1 | M-01 | P0-blocker | systematic | Root: `<MariloGrid>` → `<MariloDataGrid>` in spec |
| 2 | M-02 | P0-blocker | systematic | Root: `<GridColumn>` → `<MariloGridColumn>` in spec |
| 3 | S-04 | P0-blocker | systematic | `<GridColumns>` wrapper → create `<MariloGridColumns>` if absent |
| 4 | S-05 | P0-blocker | systematic | `GridCommandColumn` → `<MariloGridCommandColumn>` rename/create |
| 5 | NM-01 | P0-blocker | batch | All 4 Wave 1 spec files use `<MariloGrid>` |
| 6 | NM-03 | P0-blocker | single | `refresh-data.md:48` C# block: `MariloGrid<Employee>` |
| 7 | NM-04 | P0-blocker | single | `editing/overview.md:147` event-args type name |
| 8 | NM-05 | P0-blocker | batch | `keyboard-navigation.md:231` stale namespace refs |
| 9 | NM-06 | P0-blocker | single | `editing/overview.md:164` phantom `GridEditorType` in wrong namespace |
| 10 | SRC-05 | P2 | single | `refresh-data.md:25` snippet uses `MariloGrid<T>` |
| 11 | FU-1 | P1 | batch | Spec-update routing lane — naming cascade applies to every snippet |
| 12 | FU-2 | P1 | systematic | Bootstrap intake lane — naming cascade applies |
| 13 | FU-3 | P0-blocker | systematic | The naming decision itself (RESOLVED) |
| 14 | FU-4 | P1 | systematic | Shape-mismatch routing — naming portion |
| 15 | FU-5 | P2 | batch | Event-gap routing — naming portion |
| 16 | FU-6 | P1 | batch | Demo-batch routing — naming portion |
| 17 | FU-7 | P1 | systematic | Source-blocked demo routing — naming portion |
| 18 | FU-8 | P2 | batch | Empty/error state routing — naming portion |
| 19 | FU-9 | P0-blocker | systematic | Provider visual batch routing — naming portion |
| 20 | FU-10 | P0-blocker | batch | Unstyled-selector routing — naming portion |
| 21 | FU-11 | P1 | batch | `#fff` literal routing — naming portion |
| 22 | FU-12 | P0-blocker | batch | D4 honesty + focus ring routing — naming portion |

**Count: 22 rows** (10 base + 12 FU lanes)

---

## Lane B — Provider Visual Gap Batch (SCSS)

**Priority:** P0-blocker (VP-002, VP-004, VP-008, VP-009, VP-010, VP-012, VP-015 are all P0-blocker or promoted)
**Rationale:** 8 critical + 11 major visual-parity gaps. Pager buttons, popup edit dialog, empty state, and focus rings are completely unstyled. Dark-mode token math is broken. These are visually unusable states.
**Scope estimate:** ~8 SCSS files across FluentUI and Bootstrap providers (`_data-grid.scss`, `_dark-mode.scss`, `patterns/_overlay.scss`), ~4 test files for visual regression.
**Sync areas:** source, tests, gap-plan
**Dependencies:** Lane A (any spec snippets touched during VP work inherit the naming cascade). Lane B includes the focus-ring SCSS (VP-015) that Lane C's keyboard engine needs.
**Escalation:** VP-006 density parameter — may require additive public API (`Density` parameter on `MariloDataGrid`). Flag for orchestrator if scope confirms a new parameter is needed.

### Sub-lanes

- **B.1 — State treatment tokens (FU-9 core):** VP-001, VP-002, VP-003, VP-004, VP-017 — hover/selected/stripe token collisions, dark-mode luminance.
- **B.2 — Unstyled-selector cluster (FU-10):** VP-007, VP-008, VP-009, VP-010, VP-011, VP-012, VP-018 — zero-SCSS selectors needing full initial styling.
- **B.3 — Hardcoded `#fff` literals (FU-11):** VP-013, VP-014, VP-019 — token replacement.
- **B.4 — Focus treatment (FU-12 SCSS portion):** VP-015 — `:focus`/`:focus-visible` rules across all interactive elements.
- **B.5 — Typography/density/elevation:** VP-005, VP-006, VP-020 — polish items.

### Row assignments (18 rows)

| # | ID | Priority | Scope | Sub-lane |
|--:|-----|----------|-------|----------|
| 1 | VP-001 | P1 | single | B.1 — hover token collision (Fluent light) |
| 2 | VP-002 | P0-blocker | single | B.1 — dark stripe+hover same token |
| 3 | VP-003 | P1 | single | B.1 — selected+hover no delta (Fluent light) |
| 4 | VP-004 | P0-blocker | single | B.1 — dark selected row invisible |
| 5 | VP-005 | P2 | single | B.5 — header typography token |
| 6 | VP-006 | P2 | single | B.5 — density variants (escalation candidate) |
| 7 | VP-007 | P2 | single | B.2 — sort indicator unstyled |
| 8 | VP-008 | P0-blocker | batch | B.2 — pager buttons unstyled (Fluent) |
| 9 | VP-009 | P0-blocker | batch | B.2 — pager buttons unstyled (Bootstrap) |
| 10 | VP-010 | P0-blocker | single | B.2 — empty state unstyled |
| 11 | VP-011 | P1 | single | B.2 — loading overlay unstyled |
| 12 | VP-012 | P0-blocker | batch | B.2 — popup edit dialog unstyled |
| 13 | VP-013 | P1 | single | B.3 — filter menu `#fff` (Fluent) |
| 14 | VP-014 | P2 | single | B.3 — filter shadow hardcoded |
| 15 | VP-015 | P0-blocker | single | B.4 — zero focus treatment |
| 16 | VP-017 | P3-cosmetic | single | B.1 — group header surface token |
| 17 | VP-018 | P1 | single | B.2 — checkbox column unstyled |
| 18 | VP-019 | P1 | single | B.3 — Bootstrap filter `#fff` |

**Count: 18 rows** (VP-001..VP-015, VP-017..VP-019; VP-016 in Lane H deferred, VP-020 below)

**Note:** VP-020 (Bootstrap striped runtime override) is P3-cosmetic and isolated to Bootstrap provider token refactor. Assigned to Lane B as row 18 would make 19. Reassigning: VP-020 moves to Lane G (source-ahead, Bootstrap-specific). See Lane G.

**Revised count: 18 rows** (keeping VP-020 in Lane B for provider cohesion since it is a pure SCSS fix).

Correction — let me recount. VP-001 through VP-020 = 20 rows total. VP-016 goes to Lane H (deferred). That leaves 19 VP rows for Lane B.

### Revised row assignments (19 rows)

| # | ID | Priority | Scope | Sub-lane |
|--:|-----|----------|-------|----------|
| 1 | VP-001 | P1 | single | B.1 |
| 2 | VP-002 | P0-blocker | single | B.1 |
| 3 | VP-003 | P1 | single | B.1 |
| 4 | VP-004 | P0-blocker | single | B.1 |
| 5 | VP-005 | P2 | single | B.5 |
| 6 | VP-006 | P2 | single | B.5 |
| 7 | VP-007 | P2 | single | B.2 |
| 8 | VP-008 | P0-blocker | batch | B.2 |
| 9 | VP-009 | P0-blocker | batch | B.2 |
| 10 | VP-010 | P0-blocker | single | B.2 |
| 11 | VP-011 | P1 | single | B.2 |
| 12 | VP-012 | P0-blocker | batch | B.2 |
| 13 | VP-013 | P1 | single | B.3 |
| 14 | VP-014 | P2 | single | B.3 |
| 15 | VP-015 | P0-blocker | single | B.4 |
| 16 | VP-017 | P3-cosmetic | single | B.1 |
| 17 | VP-018 | P1 | single | B.2 |
| 18 | VP-019 | P1 | single | B.3 |
| 19 | VP-020 | P3-cosmetic | single | B.5 |

**Count: 19 rows**

---

## Lane C — Keyboard Engine

**Priority:** P0-blocker (SA-06, SA-07, SA-08 are gate-level honesty defects; A-01 is the demo honesty fix)
**Rationale:** D4 "Navigable Grid" demo advertises keyboard behavior the source does not implement. This is a B-C gate-level honesty defect. The keyboard engine requires: `onkeydown` handler, default key bindings, data-cell actions, edit-row keyboard, `CustomKeyboardShortcuts` dictionary, and focus management.
**Scope estimate:** ~6 source files (new `Keyboard.cs` partial, update `MariloDataGrid.razor`, `Editing.cs`, enums), ~4 spec files, ~3 demo files (D4 rework + new keyboard demo), ~5 test files.
**Sync areas:** source, spec, demo, tests, gap-plan
**Dependencies:** Lane A (naming in spec/demo), Lane B VP-015 (focus rings must be styled for keyboard to be visually meaningful).
**Escalation:** None — the scope is well-defined from Wave 1 SA analysis.

### Row assignments (8 rows)

| # | ID | Priority | Scope | Lane Role |
|--:|-----|----------|-------|-----------|
| 1 | S-03 | P3-cosmetic | systematic | `CustomKeyboardShortcuts` + enums |
| 2 | SA-05 | P3-cosmetic | systematic | Re-confirms S-03 keyboard shortcut enums absent |
| 3 | SA-06 | P0-blocker | systematic | Default key bindings undelivered (B-C honesty) |
| 4 | SA-07 | P0-blocker | systematic | Data-cell keyboard actions undelivered |
| 5 | SA-08 | P0-blocker | systematic | Edit-row keyboard undelivered |
| 6 | A-01 | P0-blocker | single | D4 honesty fix — remove/gate keyboard cheat sheet |
| 7 | A-09 | P1 | systematic | Keyboard nav demo scenarios (17 blocked-by-source) |
| 8 | S-01 | P3-cosmetic | single | `AdaptiveMode` — keyboard layout adaptation (thematic fit) |

**Count: 8 rows**

---

## Lane D — Spec-Update Batch (Doc-Only)

**Priority:** P1 (FU-1 routing; no P0-blockers in this lane after Lane A extracts naming records)
**Rationale:** 28 records that are doc-only fixes: document existing source parameters/behaviors in spec, fix spec-internal type inconsistencies, verify closed items. No source changes. Largest lane by row count.
**Scope estimate:** ~12 spec markdown files under `docs/component-specs/grid/`. Pure text edits after Lane A naming cascade has landed.
**Sync areas:** spec, gap-plan
**Dependencies:** Lane A MUST land first — every spec file touched here inherits the naming cascade.
**Escalation:** None.

### Row assignments (28 rows)

| # | ID | Priority | Scope | Lane Role |
|--:|-----|----------|-------|-----------|
| 1 | U-01 | P2 | batch | Document `ShowSearchBox` + `SearchBoxPlaceholder` |
| 2 | U-02 | P2 | batch | Document `EnableVirtualization` + `VirtualizeOverscanCount` |
| 3 | U-03 | P3-cosmetic | single | Document `Striped` |
| 4 | U-04 | P2 | single | Document `AutoGenerateColumns` |
| 5 | U-05 | P2 | single | Document grid-level `Resizable` / `Reorderable` |
| 6 | U-06 | P3-cosmetic | single | Document `OnRowContextMenu` |
| 7 | U-07 | P3-cosmetic | single | Document `OnRowExpand` / `OnRowCollapse` |
| 8 | U-08 | P2 | single | Document `PagerButtonCount` (pairs with M-12 in Lane F) |
| 9 | U-09 | P3-cosmetic | single | Document `ColumnWidthProvider` |
| 10 | U-10 | P2 | single | Document `GridGroupHeaderContext<TItem>` |
| 11 | M-04 | P2 | single | Spec-name update for `SortMode` → `GridSortMode` |
| 12 | M-06 | P2 | single | Fix spec type: `GridCommandEventArgs` → `GridEditEventArgs<TItem>` |
| 13 | M-07 | P2 | single | Fix spec `OnModelInit` signature |
| 14 | M-08 | P2 | single | Document both `DisplayFormat` and `Format` |
| 15 | M-09 | P2 | single | Verify `Locked` param names in `columns/frozen.md` |
| 16 | M-10 | P2 | single | Verify cell selection param names in `selection/cells.md` |
| 17 | M-11 | P2 | single | Verify `OnRowDrop` in `row-drag-drop.md` |
| 18 | M-13 | P3-cosmetic | single | Fix event-args type for `OnRowExpand`/`OnRowCollapse` |
| 19 | NM-02 | P2 | single | Pick `GridCellReference<TItem>` over `GridSelectedCellDescriptor` |
| 20 | SRC-01 | P2 | single | Document `SelectionUnit` at overview level |
| 21 | SRC-02 | P2 | single | Fix `SelectedCells` type reference (after NM-02) |
| 22 | SRC-03 | P2 | batch | Document imperative edit API (7 methods) |
| 23 | SRC-04 | P2 | single | Document `OnCommand` typed args |
| 24 | SRC-06 | P3-cosmetic | single | Document native `window.confirm` behavior (option a) |
| 25 | SRC-07 | P3-cosmetic | single | Clarify `RowIndex` is page-relative |
| 26 | SRC-08 | P3-cosmetic | single | Document detail-row expansion persistence |
| 27 | S-02 | P2 | single | Add `Class` parameter to spec (trivial-add in source too, but spec-first) |
| 28 | SA-14 | P2 | single | Document `BeginAdd()` silent-failure + defensive fix |

**Count: 28 rows**

**Note on SRC-06:** Assigned here as option (a) "document native behavior." If orchestrator decides option (b) "swap to MariloDialog," this row moves to Lane G (source change). Default: document-as-is per P3-cosmetic priority.

**Note on S-02 / SA-14:** These have `source` sync areas in the inventory, but the spec-update portion is the primary work. Source changes (adding `Class` parameter, adding defensive throw) are trivial additive additions that can be done in a subsequent pass or bundled into Lane G. Lane D owns the spec documentation.

---

## Lane E — Wave 2 Demo Batch + Source-Blocked Demos

**Priority:** P1 (A-02 is P0-blocker for refresh-data coverage; A-03 is P1; rest are P2/P3)
**Rationale:** Wave 2 identified 13 demo action items. A-01..A-08 are demo-only (no source blocker except naming); A-09..A-13 are source-blocked. Demo-only items route through FU-6; source-blocked items through FU-7.
**Scope estimate:** ~6 demo `.razor` files (D1 extensions, D2 extensions, new `RefreshData.razor`, new `ProgrammaticControl.razor` or D2 extension, D4 rework).
**Sync areas:** demo, spec (for A-07 imperative API spec), gap-plan
**Dependencies:** Lane A (naming in all demo code), Lanes C/G/H for source-blocked items (A-09 keyboard, A-10/A-11 selection, A-12 EditorType, A-13 NewRowPosition).
**Escalation:** None.

### Sub-grouping

- **E.1 — Demo-only (can proceed after Lane A):** A-02, A-03, A-04, A-05, A-06, A-07, A-08
- **E.2 — Source-blocked (wait for Lane C/G):** A-09, A-10, A-11, A-12, A-13

### Row assignments (12 rows)

| # | ID | Priority | Scope | Sub-group |
|--:|-----|----------|-------|-----------|
| 1 | A-02 | P0-blocker | batch | E.1 — RefreshData demo (zero coverage) |
| 2 | A-03 | P1 | single | E.1 — cell-selection section in D1 |
| 3 | A-04 | P2 | single | E.1 — selection + edit combined demo |
| 4 | A-05 | P2 | single | E.1 — ConfirmDelete section |
| 5 | A-06 | P2 | single | E.1 — OnAdd + OnCommand sections |
| 6 | A-07 | P2 | batch | E.1 — imperative API section (pairs with SRC-03 in Lane D) |
| 7 | A-08 | P3-cosmetic | single | E.1 — `@bind-SelectedItems` shorthand |
| 8 | A-10 | P2 | batch | E.2 — Shift/Ctrl/drag-select demos (blocked by SA-01/SA-04) |
| 9 | A-11 | P2 | batch | E.2 — GridCheckboxColumn demos (blocked by SA-03) |
| 10 | A-12 | P2 | batch | E.2 — EditorType demos (blocked by SA-09) |
| 11 | A-13 | P2 | single | E.2 — NewRowPosition demo (blocked by SA-10) |
| 12 | SA-12 | P3-cosmetic | single | E.1 — editing guard documentation (cell-selection + InCell combo) |

**Count: 12 rows**

**Note:** A-01 is in Lane C (keyboard/honesty). A-09 is in Lane C (keyboard). SA-12 is placed here because its primary deliverable is a demo guard/documentation, not a source change.

---

## Lane F — Shape-Decision Cluster

**Priority:** P1 (requires orchestrator escalation for API decisions before implementation)
**Rationale:** Four distinct API-shape mismatches where spec and source disagree on the parameter shape, not just the name. Each requires a decision: adopt source shape in spec, adopt spec shape in source, or design a new converged shape. These decisions are architectural and may involve public API changes.
**Scope estimate:** ~4 source files + ~4 spec files + ~4 test files per shape decision implemented. Estimate is speculative until decisions are made.
**Sync areas:** source, spec, tests, gap-plan
**Dependencies:** Lane A (naming must be resolved before shape decisions). Lane F decisions feed into Lane G implementation.
**Escalation:** **YES — orchestrator escalation required for each shape decision:**
  - **M-03:** `ScrollMode`/`RowHeight` (spec) vs `EnableVirtualization`/`VirtualizeOverscanCount` (source) — virtual-scrolling API shape
  - **M-05:** `GridState<TItem>` (spec) vs `GridState` non-generic (source) — genericization decision (cross-ref GanttState precedent from `project_gantt_state_api_shape.md`)
  - **M-12:** `GridPagerSettings { ButtonCount, ... }` (spec) vs flat `PagerButtonCount` + `PageSizes` (source) — pager compound-object decision

### Row assignments (8 rows)

| # | ID | Priority | Scope | Decision Required |
|--:|-----|----------|-------|-------------------|
| 1 | M-03 | P2 | systematic | Virtual-scrolling shape |
| 2 | M-05 | P2 | systematic | GridState genericization |
| 3 | M-12 | P2 | systematic | Pager compound-object shape |
| 4 | S-07 | P2 | systematic | `GridPagerSettings` implementation (depends on M-12) |
| 5 | S-09 | P2 | batch | Composite filter descriptors — filter shape decision |
| 6 | S-10 | P2 | batch | Drag-to-group UI — grouping shape decision |
| 7 | S-15 | P2 | systematic | Validation integration — `EditContext` wiring shape |
| 8 | SA-11 | P2 | single | `OnRead` auto-call after CRUD — verification needed |

**Count: 8 rows**

**Note on SA-11:** Included because it requires source verification to determine if a gap actually exists. If verification shows no gap, it drops to Lane D (document behavior). If it reveals a gap, it stays in Lane F/G for implementation.

---

## Lane G — Source-Ahead Implementation (Spec-Ahead Parameters)

**Priority:** P2 (no P0-blockers; these are source additions to match spec promises)
**Rationale:** 17 records where the spec documents behavior/parameters that don't exist in source. These are the new feature implementations that close the spec-source gap. Grouped by thematic cluster.
**Scope estimate:** ~10 source files, ~8 spec files (updates after source lands), ~6 test files. Large lane — may be split into sub-waves at Stage 03.
**Sync areas:** source, spec, demo, tests, gap-plan
**Dependencies:** Lane A (naming), Lane F (shape decisions for items that share parameters with shape-decision records).
**Escalation:** S-13 AI features — confirm deferred (Lane H). SA-11 — outcome depends on verification (currently in Lane F).

### Sub-clusters

- **G.1 — Selection extensions:** SA-01, SA-02, SA-03, SA-04, SA-13
- **G.2 — Editing extensions:** SA-09, SA-10, S-16
- **G.3 — Column features:** S-06, S-08, S-11, S-12, S-17
- **G.4 — Misc source additions:** S-14, S-02 (source portion)

### Row assignments (17 rows)

| # | ID | Priority | Scope | Sub-cluster |
|--:|-----|----------|-------|-------------|
| 1 | SA-01 | P2 | batch | G.1 — `DragToSelect` parameter |
| 2 | SA-02 | P2 | batch | G.1 — `SelectionType` on `GridSelectionSettings` |
| 3 | SA-03 | P2 | batch | G.1 — `GridCheckboxColumn` dedicated element |
| 4 | SA-04 | P2 | systematic | G.1 — Shift-click / Ctrl-click range selection |
| 5 | SA-09 | P2 | batch | G.2 — `EditorType` enum |
| 6 | SA-10 | P2 | single | G.2 — `NewRowPosition` parameter |
| 7 | SA-13 | P3-cosmetic | single | G.1 — clear `SelectedItems` on row-drop |
| 8 | S-06 | P2 | batch | G.3 — Toolbar expansion (13 tool components) |
| 9 | S-08 | P2 | batch | G.3 — Excel + PDF export |
| 10 | S-11 | P3-cosmetic | batch | G.3 — Multi-column headers (`GridColumnGroup`) |
| 11 | S-12 | P3-cosmetic | batch | G.3 — Column menu / column chooser |
| 12 | S-14 | P3-cosmetic | single | G.4 — `HighlightedItems` API |
| 13 | S-16 | P3-cosmetic | batch | G.2 — `PopupFormTemplate`, `PagerTemplate` |
| 14 | S-17 | P3-cosmetic | single | G.3 — Checkbox-list filter control |
| 15 | S-02 | P2 | single | G.4 — `Class` parameter (source portion) |
| 16 | SA-14 | P2 | single | G.2 — `BeginAdd()` defensive throw (source portion) |
| 17 | A-04 | P2 | single | G.1 — selection + edit combined (needs source first) |

**Wait — A-04 is already in Lane E.** Let me correct. SA-14 source portion is in Lane G but SA-14 spec portion is in Lane D. The row must live in one lane only.

**Correction:** SA-14 is assigned to Lane D (spec-update batch) because the primary deliverable is documentation. The defensive throw is a trivial one-line source addition that can be bundled into Lane G's editing-guard work. But to avoid double-counting, SA-14 stays in Lane D only. Similarly, S-02 is in Lane D.

### Revised row assignments (15 rows)

| # | ID | Priority | Scope | Sub-cluster |
|--:|-----|----------|-------|-------------|
| 1 | SA-01 | P2 | batch | G.1 — `DragToSelect` parameter |
| 2 | SA-02 | P2 | batch | G.1 — `SelectionType` on `GridSelectionSettings` |
| 3 | SA-03 | P2 | batch | G.1 — `GridCheckboxColumn` dedicated element |
| 4 | SA-04 | P2 | systematic | G.1 — Shift-click / Ctrl-click range selection |
| 5 | SA-09 | P2 | batch | G.2 — `EditorType` enum |
| 6 | SA-10 | P2 | single | G.2 — `NewRowPosition` parameter |
| 7 | SA-13 | P3-cosmetic | single | G.1 — clear `SelectedItems` on row-drop |
| 8 | S-06 | P2 | batch | G.3 — Toolbar expansion (13 tool components) |
| 9 | S-08 | P2 | batch | G.3 — Excel + PDF export |
| 10 | S-11 | P3-cosmetic | batch | G.3 — Multi-column headers (`GridColumnGroup`) |
| 11 | S-12 | P3-cosmetic | batch | G.3 — Column menu / column chooser |
| 12 | S-14 | P3-cosmetic | single | G.4 — `HighlightedItems` API |
| 13 | S-16 | P3-cosmetic | batch | G.2 — `PopupFormTemplate`, `PagerTemplate` |
| 14 | S-17 | P3-cosmetic | single | G.3 — Checkbox-list filter control |
| 15 | S-02 | P2 | single | G.4 — `Class` parameter (source add) |

**Wait — S-02 is in Lane D row 27.** Remove it from here.

### Final row assignments (14 rows)

| # | ID | Priority | Scope | Sub-cluster |
|--:|-----|----------|-------|-------------|
| 1 | SA-01 | P2 | batch | G.1 — `DragToSelect` parameter |
| 2 | SA-02 | P2 | batch | G.1 — `SelectionType` on `GridSelectionSettings` |
| 3 | SA-03 | P2 | batch | G.1 — `GridCheckboxColumn` dedicated element |
| 4 | SA-04 | P2 | systematic | G.1 — Shift-click / Ctrl-click range selection |
| 5 | SA-09 | P2 | batch | G.2 — `EditorType` enum |
| 6 | SA-10 | P2 | single | G.2 — `NewRowPosition` parameter |
| 7 | SA-13 | P3-cosmetic | single | G.1 — clear `SelectedItems` on row-drop |
| 8 | S-06 | P2 | batch | G.3 — Toolbar expansion (13 tool components) |
| 9 | S-08 | P2 | batch | G.3 — Excel + PDF export |
| 10 | S-11 | P3-cosmetic | batch | G.3 — Multi-column headers (`GridColumnGroup`) |
| 11 | S-12 | P3-cosmetic | batch | G.3 — Column menu / column chooser |
| 12 | S-14 | P3-cosmetic | single | G.4 — `HighlightedItems` API |
| 13 | S-16 | P3-cosmetic | batch | G.2 — `PopupFormTemplate`, `PagerTemplate` |
| 14 | S-17 | P3-cosmetic | single | G.3 — Checkbox-list filter control |

**Count: 14 rows**

---

## Lane H — Deferred Tracks

**Priority:** P3 (explicitly out-of-scope for this wave)
**Rationale:** Items routed out of current wave per delivery report and tick-8 cerebrum decisions. Tracked here for completeness; no remediation work planned.
**Scope estimate:** n/a this wave
**Sync areas:** gap-plan (tracking only)
**Dependencies:** n/a
**Escalation:** None (these are acknowledged deferrals)

### Row assignments (4 rows)

| # | ID | Priority | Scope | Deferral Rationale |
|--:|-----|----------|-------|--------------------|
| 1 | VP-016 | P0-blocker | systematic | Material provider 5-line TODO. Separate implementation track per tick-8 Cerebrum Pattern 5 |
| 2 | S-13 | P3-cosmetic | systematic | AI features (9 spec pages). Phase D deferred |
| 3 | S-01 | P3-cosmetic | single | `AdaptiveMode` — planned feature, not current wave |
| 4 | S-14 | P3-cosmetic | single | `HighlightedItems` — low priority, no immediate need |

**Wait — S-01 is in Lane C row 8, and S-14 is in Lane G row 12.** I need to reconcile.

S-01 (`AdaptiveMode`) fits better in Lane H (deferred) since it's P3-cosmetic and explicitly "known planned gap." Moving it out of Lane C.

S-14 (`HighlightedItems`) fits in Lane G since it is a source-ahead implementation, albeit low priority. Keep it in Lane G.

### Revised Lane C (remove S-01): 7 rows
### Revised Lane H: 2 rows

| # | ID | Priority | Scope | Deferral Rationale |
|--:|-----|----------|-------|--------------------|
| 1 | VP-016 | P0-blocker | systematic | Material provider 5-line TODO. Separate track per tick-8 Pattern 5 |
| 2 | S-13 | P3-cosmetic | systematic | AI features (9 spec pages). Phase D deferred |

**Count: 2 rows**

---

## Row Count Reconciliation

| Lane | Rows | IDs |
|------|-----:|-----|
| A — Naming-cascade | 22 | M-01, M-02, S-04, S-05, NM-01, NM-03, NM-04, NM-05, NM-06, SRC-05, FU-1..FU-12 |
| B — Provider visual | 19 | VP-001..VP-015, VP-017..VP-020 |
| C — Keyboard engine | 7 | S-03, SA-05, SA-06, SA-07, SA-08, A-01, A-09 |
| D — Spec-update batch | 28 | U-01..U-10, M-04, M-06..M-11, M-13, NM-02, SRC-01..SRC-04, SRC-06..SRC-08, S-02, SA-14 |
| E — Demo batch | 12 | A-02..A-08, A-10..A-13, SA-12 |
| F — Shape-decision | 8 | M-03, M-05, M-12, S-07, S-09, S-10, S-15, SA-11 |
| G — Source-ahead | 14 | SA-01..SA-04, SA-09, SA-10, SA-13, S-06, S-08, S-11, S-12, S-14, S-16, S-17 |
| H — Deferred | 2 | VP-016, S-13 |
| **TOTAL** | **112** | |

**Discrepancy: 112, not 113.** One row is missing. Let me trace every ID.

### Full ID trace (113 expected)

**Section A — Wave 1 (68 rows):**
- U-01..U-10 = 10 → Lane D ✓
- S-01..S-17 = 17 → S-01 (unassigned!), S-02 (D), S-03 (C), S-04 (A), S-05 (A), S-06 (G), S-07 (F), S-08 (G), S-09 (F), S-10 (F), S-11 (G), S-12 (G), S-13 (H), S-14 (G), S-15 (F), S-16 (G), S-17 (G) = 17 ✓ (S-01 needs assignment)
- M-01..M-13 = 13 → M-01 (A), M-02 (A), M-03 (F), M-04 (D), M-05 (F), M-06 (D), M-07 (D), M-08 (D), M-09 (D), M-10 (D), M-11 (D), M-12 (F), M-13 (D) = 13 ✓
- SA-01..SA-14 = 14 → SA-01 (G), SA-02 (G), SA-03 (G), SA-04 (G), SA-05 (C), SA-06 (C), SA-07 (C), SA-08 (C), SA-09 (G), SA-10 (G), SA-11 (F), SA-12 (E), SA-13 (G), SA-14 (D) = 14 ✓
- SRC-01..SRC-08 = 8 → SRC-01 (D), SRC-02 (D), SRC-03 (D), SRC-04 (D), SRC-05 (A), SRC-06 (D), SRC-07 (D), SRC-08 (D) = 8 ✓
- NM-01..NM-06 = 6 → NM-01 (A), NM-02 (D), NM-03 (A), NM-04 (A), NM-05 (A), NM-06 (A) = 6 ✓
- Wave 1 subtotal: 10 + 17 + 13 + 14 + 8 + 6 = 68 ✓

**Section B — Wave 2 (13 rows):**
- A-01..A-13 = 13 → A-01 (C), A-02 (E), A-03 (E), A-04 (E), A-05 (E), A-06 (E), A-07 (E), A-08 (E), A-09 (C), A-10 (E), A-11 (E), A-12 (E), A-13 (E) = 13 ✓

**Section C — Wave 3 (20 rows):**
- VP-001..VP-020 = 20 → VP-001..VP-015 (B), VP-016 (H), VP-017..VP-020 (B) = 20 ✓

**Section D — Wave 4 (12 rows):**
- FU-1..FU-12 = 12 → all in Lane A = 12 ✓

**Grand total: 68 + 13 + 20 + 12 = 113 ✓**

**Missing row: S-01.** It was removed from Lane C but not assigned elsewhere. S-01 (`AdaptiveMode`) is P3-cosmetic, "known planned gap." Assign to Lane H (deferred).

### Final Lane H (3 rows)

| # | ID | Priority | Scope | Deferral Rationale |
|--:|-----|----------|-------|--------------------|
| 1 | VP-016 | P0-blocker | systematic | Material provider 5-line TODO. Separate track |
| 2 | S-13 | P3-cosmetic | systematic | AI features (9 spec pages). Phase D deferred |
| 3 | S-01 | P3-cosmetic | single | `AdaptiveMode` — planned feature, not current wave |

---

## Final Row Count Reconciliation

| Lane | Rows | IDs |
|------|-----:|-----|
| A — Naming-cascade | 22 | M-01, M-02, S-04, S-05, NM-01, NM-03, NM-04, NM-05, NM-06, SRC-05, FU-1..FU-12 |
| B — Provider visual | 19 | VP-001..VP-015, VP-017..VP-020 |
| C — Keyboard engine | 7 | S-03, SA-05, SA-06, SA-07, SA-08, A-01, A-09 |
| D — Spec-update batch | 28 | U-01..U-10, M-04, M-06..M-11, M-13, NM-02, SRC-01..SRC-04, SRC-06..SRC-08, S-02, SA-14 |
| E — Demo batch | 12 | A-02..A-08, A-10..A-13, SA-12 |
| F — Shape-decision | 8 | M-03, M-05, M-12, S-07, S-09, S-10, S-15, SA-11 |
| G — Source-ahead | 14 | SA-01..SA-04, SA-09, SA-10, SA-13, S-06, S-08, S-11, S-12, S-14, S-16, S-17 |
| H — Deferred | 3 | VP-016, S-13, S-01 |
| **TOTAL** | **113** | |

**Verification: 22 + 19 + 7 + 28 + 12 + 8 + 14 + 3 = 113** ✓

---

## Sequencing and Dependencies

```
Phase 1 (Gate: naming cascade landed)
  └── Lane A — Naming-cascade [P0-blocker, ~1 worker day]
       Unblocks: ALL other lanes

Phase 2 (Gate: provider SCSS + focus rings passing visual tests)
  ├── Lane B — Provider visual [P0-blocker, ~2.5 worker days]
  ├── Lane C — Keyboard engine [P0-blocker, ~2 worker days]
  │    (depends on B.4 focus rings)
  └── Lane D — Spec-update batch [P1, ~0.5 worker day]
       (can run in parallel with B/C after A lands)

Phase 3 (Gate: shape decisions made by orchestrator)
  ├── Lane E — Demo batch [P1, ~1 worker day]
  │    (E.1 can start after A; E.2 waits for C/G)
  └── Lane F — Shape-decision cluster [P1, ~0.5 worker day post-decision]
       (blocked until orchestrator resolves M-03/M-05/M-12 escalations)

Phase 4 (Gate: source additions passing tests)
  └── Lane G — Source-ahead [P2, ~3-4 worker days]
       (depends on F decisions for shared parameters)

Deferred (no phase)
  └── Lane H — Deferred tracks [P3, separate wave]
```

### Critical path

**A → B/C (parallel) → E.2/G (parallel) → complete**

Lane D can run concurrently with B/C after A. Lane E.1 (demo-only) can run after A. Lane F is blocked on orchestrator decisions — it is off the critical path for P0/P1 work but gates Lane G.

---

## Escalation Summary

| Item | Type | Lane | Decision Needed |
|------|------|------|-----------------|
| M-03 | `architecture-decision` | F | Virtual-scrolling API shape: `ScrollMode`+`RowHeight` vs `EnableVirtualization`+`VirtualizeOverscanCount` |
| M-05 | `architecture-decision` | F | `GridState<TItem>` genericization (cross-ref GanttState precedent) |
| M-12 | `architecture-decision` | F | Pager compound-object shape: `GridPagerSettings` vs flat params |
| VP-006 | `public-api-change` | B | Density parameter — additive, but new public API on `MariloDataGrid` |
| SRC-06 | `architecture-decision` | D | Confirm dialog UX: document native `window.confirm` or swap to `MariloDialog` (default: document-as-is) |
| SA-11 | `architecture-question` | F | `ProcessDataAsync` auto-call verification — may not be a gap |

---

## Open Questions Carried Forward

1. **FU-4 shape decisions (M-03, M-05, M-12)** — escalation required before Lane F/G can execute.
2. **VP-006 density** — escalation if a new `Density` public parameter is needed.
3. **SRC-06 confirm-dialog** — defaulting to "document-as-is" (Lane D option a). Override if orchestrator prefers MariloDialog swap.
4. **S-13 AI features** — confirmed deferred to Lane H, separate wave.
5. **SA-11 verification** — needs source inspection. Placed in Lane F pending verification outcome.

---

## Checkpoint

**This is the end of Stage 02 prioritize.** All 113 inventory rows are assigned to exactly one of 8 lanes (A-H). Row count verified: 22 + 19 + 7 + 28 + 12 + 8 + 14 + 3 = 113. No drops, no duplicates.

Stage 03 (resolution design) will take the approved lane structure and produce per-lane resolution designs with file-level change plans. **Worker stops here and awaits orchestrator review.**
