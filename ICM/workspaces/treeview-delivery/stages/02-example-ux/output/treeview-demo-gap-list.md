# TreeView Demo Gap List

**Audit Date:** 2026-04-11 (Wave 2 — orchestrator dispatch, supersedes 2026-04-03)
**Demo Page:** `samples/Marilo.Demo/Pages/Components/TreeView/Overview.razor`
**Stage 01 Input:** `stages/01-spec-review/output/treeview-spec-gap-list.md` (48 gap records incl. Wave 1 SPEC-035..048)
**Worker:** `w-treeview-delivery` — stage 02-example-ux, steps 1–3 + CHECKPOINT
**Wave scope:** scenario-coverage focus (per human decision 2026-04-09), not visual variety.

---

## Current Demo Inventory

`Overview.razor` currently hosts **21 interactive scenarios + 1 accessibility reference block** across 8 page sections. Inventory below is keyed to the page's `DemoSection` blocks as written in source.

| # | Section | Scenario Title | Parameters / Events Demonstrated (primary) | Interactive? | Snippet Status |
|---|---------|----------------|--------------------------------------------|--------------|----------------|
| 1 | Overview | Basic Usage (ChildContent) | `ChildContent`, `MariloTreeItem.Title`, `MariloTreeItem.IsExpanded` | No input | current |
| 2 | Overview | Data-Driven (Flat Data) | `Data`, `IdField`, `ParentIdField`, `TextField` | No input | current |
| 3 | Overview | Data-Driven (Hierarchical) | `Data`, `IdField`, `TextField`, `ItemsField` | No input | current |
| 4 | Appearance | With Icons | `MariloTreeItem.Icon` | No input | current |
| 5 | Appearance | Selection | `SelectionMode=Single` | No input | current |
| 6 | Events | Item Selection | — (no parameters set; duplicates #1 structure) | No input | current |
| 7 | Events | Item Expand / Collapse | — (implicit expand/collapse) | No input | current |
| 8 | Checkboxes | CheckBoxMode (Multiple) | `CheckBoxMode=Multiple`, `@bind-CheckedItems`, cascade defaults | Click checkboxes | current |
| 9 | Checkboxes | CheckBoxMode (Single) | `CheckBoxMode=Single`, `@bind-CheckedItems` | Click checkboxes | current |
| 10 | Checkboxes | AllowCheckChildren / AllowCheckParents | `AllowCheckChildren=false`, `AllowCheckParents=false` | Click checkboxes | current |
| 11 | Checkboxes | CheckedItemsChanged Event | `CheckedItemsChanged` event, `CheckedItems` (one-way) | Click checkboxes + log | current |
| 12 | Expansion | ExpandOnClick | `ExpandOnClick=true` | Click nodes | current |
| 13 | Expansion | SingleExpand (Accordion) | `SingleExpand=true` | Click nodes | current |
| 14 | Expansion | AutoExpand | `AutoExpand=true`, `SelectionMode=Single`, `SelectedItems` seed | No input | current (minor quirk: seeded via `SelectedItems` rather than interactive) |
| 15 | Expansion | ExpandAllAsync / CollapseAllAsync | Public API methods `ExpandAllAsync`, `CollapseAllAsync` | Two buttons | current |
| 16 | Drag and Drop | EnableDragDrop + OnItemDrop | `EnableDragDrop=true`, `OnItemDrop` event | Drag-and-drop + log | current |
| 17 | Lazy Loading | LoadChildrenAsync | `Data`, `HasChildrenField`, `LoadChildrenAsync` | Click expand arrows | current |
| 18 | Filtering and Editing | FilterFunc | `FilterFunc` | Text input drives filter | current |
| 19 | Filtering and Editing | AllowEditing + OnItemEdit | `AllowEditing=true`, `OnItemEdit` event | Double-click / F2 + log | current |
| 20 | Filtering and Editing | ItemTemplate | `ItemTemplate` render fragment | No input | current |
| 21 | State | Disabled | `Disabled=true` | (no interaction by design) | current |
| 22 | State | ReadOnly | `ReadOnly=true`, `CheckBoxMode=Multiple`, seed `CheckedItems` | (view-only by design) | current |
| — | Accessibility | `AccessibilityInfo` reference | Keyboard table, ARIA table, screen-reader notes | Documentation | n/a |

**Notes on the inventory:**
- Scenarios #6 and #7 under *Events* use `MariloTreeView` with no parameters set and have no user-controllable input. They duplicate the structure of #1 and demonstrate no event. They are flagged below as **(b)** stale-by-effect — the section *title* claims "Item Selection" / "Item Expand/Collapse" but the snippet doesn't wire `SelectedItemsChanged` / `ExpandedItemsChanged`.
- No Telerik namespace references found in the demo page (checked `Telerik` — zero matches).
- All code snippet constants (`_basicCode` … `_readOnlyCode`) were cross-referenced against the current source parameter surface. All 17 code constants use names that exist in `MariloTreeView.razor.cs` at HEAD.

---

## API Surface Audit (source of truth = `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` + `MariloTreeItem.razor.cs`)

**Parameters on `MariloTreeView` (33 total):**

| # | Parameter | Primary scenario in Overview.razor | Status |
|---|-----------|------------------------------------|--------|
| 1 | `ChildContent` | #1 | covered |
| 2 | `ItemTemplate` | #20 | covered |
| 3 | `Data` | #2, #3 | covered |
| 4 | `IdField` | #2, #3 | covered |
| 5 | `ParentIdField` | #2 | covered |
| 6 | `TextField` | #2, #3 | covered |
| 7 | `IconField` | — | **gap (a)** |
| 8 | `ItemsField` | #3 | covered |
| 9 | `HasChildrenField` | #17 | covered |
| 10 | `CheckBoxMode` | #8, #9 | covered |
| 11 | `AllowCheckChildren` | #10 | covered |
| 12 | `AllowCheckParents` | #10 | covered |
| 13 | `CheckedItems` (param+bind) | #8, #9, #11, #22 | covered |
| 14 | `CheckedItemsChanged` | #11 | covered (event) |
| 15 | `SelectionMode` | #5, #14 | covered |
| 16 | `SelectedItems` | #14 (seeded) | partial — never demonstrated as two-way bound with a visible selection indicator |
| 17 | `SelectedItemsChanged` | — | **gap (c)** |
| 18 | `OnItemClick` | — | **gap (c)** |
| 19 | `ExpandedItems` | — | **gap (a)** |
| 20 | `ExpandedItemsChanged` | — | **gap (c)** |
| 21 | `ExpandOnClick` | #12 | covered |
| 22 | `ExpandOnDoubleClick` | — | **gap (a)** |
| 23 | `SingleExpand` | #13 | covered |
| 24 | `AutoExpand` | #14 | covered |
| 25 | `LoadChildrenAsync` | #17 | covered |
| 26 | `EnableDragDrop` | #16 | covered |
| 27 | `OnItemDrop` | #16 | covered (event) |
| 28 | `Size` | — | **gap (a)** |
| 29 | `AriaLabel` | — | **gap (a)** |
| 30 | `Disabled` | #21 | covered |
| 31 | `ReadOnly` | #22 | covered |
| 32 | `OnItemContextMenu` | — | **gap (c)** |
| 33 | `CheckboxTemplate` | — | **gap (a)** |
| 34 | `AllowEditing` | #19 | covered |
| 35 | `OnItemEdit` | #19 | covered (event) |
| 36 | `FilterFunc` | #18 | covered |

**Public methods on `MariloTreeView`:**

| Method | Scenario | Status |
|--------|---------|--------|
| `ExpandAllAsync(bool, int)` | #15 | covered (default args only — `includeUnloaded` + `maxDepth` overloads not demonstrated) |
| `CollapseAllAsync()` | #15 | covered |
| `SelectNodeAsync(string)` | — | **gap (a)** (programmatic navigation API, zero demo) |

**Parameters on `MariloTreeItem` (child component):**

| Parameter | Scenario | Status |
|-----------|---------|--------|
| `Id` | #22 | covered |
| `Title` | #1 etc. | covered |
| `Icon` | #4 | covered |
| `Url` | — | **gap (a)** (link-rendering node — zero demo) |
| `IsExpanded` | #1 | covered |
| `IsExpandedChanged` | — | **gap (c)** (per-item event) |
| `IsSelected` | — | **gap (a)** (direct parameter, never set) |
| `OnClick` | — | **gap (c)** (per-item click, distinct from tree-level `OnItemClick`) |
| `ChildContent` | #1 | covered |

**Events on `MariloTreeView` declared in `events.md` but NOT present in source** (do not treat as demo gaps — already recorded as Wave 1 spec-vs-source drift):

| Spec Event | Wave 1 Record | Note |
|-----------|---------------|------|
| `OnExpand` | **SPEC-038 (P1)** | `LoadChildrenAsync` replaces it in source. Any demo written from spec would fail to compile. |
| `OnItemDoubleClick` | Wave 1 gap record | No source parameter. Demo section #7 titled "Item Expand / Collapse" is unrelated. |
| `OnItemRender` | Wave 1 gap record | No source parameter. |
| `OnDragStart` / `OnDrag` / `OnDragEnd` | Wave 1 gap record | Source exposes only `OnItemDrop`. |

---

## Demo Gap List

### (a) Parameters with NO demo scenario (primary-focus missing)

| # | Parameter | Priority | Notes |
|---|-----------|----------|-------|
| a1 | `IconField` | P2 | Spec states icons can come from data; demo #4 uses only `MariloTreeItem.Icon`. No data-binding-driven icon scenario exists. |
| a2 | ~~`ExpandedItems` (one-way or two-way)~~ | ~~P1~~ **CLOSED** | **Closed 2026-04-11 (Wave 2, step 5).** New scenario "ExpandedItems (Two-Way Bind)" added to Events section with `@bind-ExpandedItems`, programmatic expand/collapse buttons, and bound-state readout. |
| a3 | `ExpandOnDoubleClick` | P2 | Distinct behavioral option, no scenario. |
| a4 | `Size` | P3 | Appearance parameter (string). Spec overview.md lists it. Zero coverage. |
| a5 | `AriaLabel` | P2 | Important for a11y spec coverage; zero coverage. Worth a single scenario with screen-reader callout text. |
| a6 | `CheckboxTemplate` | P3 | Template render fragment. Advanced customization, no demo. |
| a7 | `SelectNodeAsync` (public method) | P2 | Programmatic navigation API, no demo. Useful for "jump to node" scenarios. |
| a8 | `MariloTreeItem.Url` | P2 | Makes a node render as a hyperlink. Zero demo. |
| a9 | `MariloTreeItem.IsSelected` | P3 | Direct declarative selection. Never set in any scenario. Lower priority since data-binding path is primary. |

### (b) Scenarios with a STALE code snippet

| # | Scenario | Priority | Issue |
|---|----------|----------|-------|
| b1 | ~~Events → "Item Selection" (#6)~~ | ~~P2~~ **CLOSED** | **Closed 2026-04-11 (Wave 2, step 6).** Scenario rewritten as "SelectedItemsChanged" — wires `SelectionMode=Single`, `SelectedItems`, `SelectedItemsChanged` with event log output. |
| b2 | ~~Events → "Item Expand / Collapse" (#7)~~ | ~~P2~~ **CLOSED** | **Closed 2026-04-11 (Wave 2, step 6).** Scenario rewritten as "ExpandedItemsChanged" — wires `ExpandedItems` + `ExpandedItemsChanged` with expanded-IDs readout. |
| b3 | Expansion → "AutoExpand" (#14) | P3 | Snippet in `_autoExpandCode` uses `SelectedItems="@(new[] { \"4\" })"` — syntactically correct against source, but the scenario provides no interactive re-selection and no toggle for `AutoExpand`. Effective demo is static. Minor — rewrite to make `AutoExpand` toggleable. |

**Spec-code-snippet blocked (not in this gap list, tracked in Wave 1):** Any demo scenario authored verbatim from the spec markdown files would not compile, because:
- 14 of 21 spec files wrap their examples in `<TreeViewBindings>` (**SPEC-037 P1**) — that component does not exist in source.
- `load-on-demand.md` (×3) + `events.md` demonstrate `OnExpand` with `TreeViewExpandEventArgs` (**SPEC-038 P1**) — that event does not exist in source; source exposes `LoadChildrenAsync` Func.
Demo scenarios written for Wave 2 must be authored from **source API**, not spec snippets, until SPEC-037 and SPEC-038 are resolved in the `treeview-gap-analysis` workspace.

### (c) Events with no demo scenario

| # | Event | Priority | Notes |
|---|-------|----------|-------|
| c1 | ~~`SelectedItemsChanged`~~ | ~~P1~~ **CLOSED** | **Closed 2026-04-11 (Wave 2, step 5).** New "SelectedItemsChanged" scenario in Events section wires `SelectionMode=Single`, `SelectedItems`, `SelectedItemsChanged` with event log. Also closes b1 (same rewrite). |
| c2 | ~~`OnItemClick`~~ | ~~P1~~ **CLOSED** | **Closed 2026-04-11 (Wave 2, step 5).** New "OnItemClick" scenario in Events section wires `OnItemClick` callback with click log showing item name and ID. |
| c3 | ~~`ExpandedItemsChanged`~~ | ~~P1~~ **CLOSED** | **Closed 2026-04-11 (Wave 2, step 5).** New "ExpandedItemsChanged" scenario in Events section wires `ExpandedItems` + `ExpandedItemsChanged`. Also closes a2 (paired bind scenario) and b2 (same rewrite). |
| c4 | `OnItemContextMenu` | P2 | Right-click handler, zero demo. |
| c5 | `MariloTreeItem.IsExpandedChanged` | P3 | Per-item event, low utility but spec'd. |
| c6 | `MariloTreeItem.OnClick` | P3 | Per-item click, low utility but spec'd. |

### (d) Edge cases not demonstrated

| # | Edge Case | Priority | Notes |
|---|-----------|----------|-------|
| d1 | Empty data (`Data=[]`) | P2 | No graceful-empty scenario. Wave 1 flagged as deferred P3; scenario-coverage wave raises it to P2. |
| d2 | Single-root tree (1 node, no children) | P3 | Degenerate minimum input. Not demonstrated. |
| d3 | Selection + Checkbox simultaneously | P2 | Realistic combo (check multiple, single-select one). Never demonstrated together. |
| d4 | Keyboard navigation demo | P2 | Accessibility section documents keys but no scenario lets a user put focus on the tree and see them work. Pairs with gap a5 (`AriaLabel`). |
| d5 | Drag-drop rejected target / OnItemDrop cancel | P3 | Scenario #16 only logs a success — no reject/cancel branch is demonstrated. |
| d6 | Lazy load failure / empty-children | P3 | `LoadChildrenAsync` returning empty or throwing is not demonstrated. Realistic for remote sources. |

---

## Coverage Summary

| Group | Count | P1 | P2 | P3 | Closed |
|-------|-------|----|----|-----|--------|
| (a) No demo scenario | 9 | ~~1~~ 0 | 5 | 3 | 1 (a2) |
| (b) Stale / effectively stale | 3 | 0 | ~~2~~ 0 | 1 | 2 (b1, b2) |
| (c) Events no demo | 6 | ~~3~~ 0 | 1 | 2 | 3 (c1, c2, c3) |
| (d) Edge cases not demonstrated | 6 | 0 | 4 | 2 | 0 |
| **Total** | **24** | ~~**4**~~ **0** | ~~**12**~~ **10** | **8** | **6** |

**Wave 2 step 5-6 closure (2026-04-11):** All 4 P1 gaps closed + 2 P2 stale-scenario rewrites (b1, b2). 6 of 24 gaps resolved. Remaining: 0 P1, 10 P2, 8 P3 = 18 open.

**Scenario-coverage delta vs 2026-04-03 baseline:** 16 of the 24 gaps flagged in the 2026-04-03 audit have been resolved by the current Overview.razor (checkbox trio, drag-drop, lazy load, filter, editing, item template, disabled, readonly, expand/collapse all, expand-on-click, single expand, auto expand). Remaining gap surface is dominated by **event wiring** and **programmatic APIs** rather than missing parameter primaries.

### Wave 2 headline findings

- **Event coverage is the real gap.** 3 of 4 P1 items are events (`SelectedItemsChanged`, `OnItemClick`, `ExpandedItemsChanged`) that are documented in `events.md` and exist in source but never fire in any demo. Two of the three "Events" section scenarios are effectively no-ops — they're titled after events but don't wire handlers.
- **Scenario-coverage focus (per 2026-04-09 decision) is well-served by a small P1 set.** Addressing the 4 P1 items (`ExpandedItems` bind, `SelectedItemsChanged`, `OnItemClick`, `ExpandedItemsChanged`) lifts every core parameter/event the spec advertises.
- **No stale snippets in the C# sense.** All 17 `_*Code` string constants reference parameter names that exist in current source. "Staleness" in this audit is semantic: the Events section scenarios claim to demo events they don't actually wire.
- **Spec-driven authoring is blocked by SPEC-037/SPEC-038.** Demo scenarios for Wave 2 must be authored from source API, not spec snippets. Flagged in Wave 1 gap list as P1; out-of-scope for this worker to resolve.
- **No Telerik references found in the demo page.** Audit checklist item passes cleanly.

---

## CHECKPOINT — Orchestrator Approval Required

This is the stage 02-example-ux **step 3 checkpoint**. Per `stages/02-example-ux/CONTEXT.md` and the Wave 2 inbox, scenario authoring (steps 5–9) does NOT proceed until the orchestrator approves this gap list.

### What needs approval before scenario authoring begins

1. **Scope of Wave 2 authoring.** Recommended minimum: the **4 P1 items** (gap a2 `ExpandedItems`, gap c1 `SelectedItemsChanged`, gap c2 `OnItemClick`, gap c3 `ExpandedItemsChanged`). This lifts primary event coverage to 100% against the current source surface. Orchestrator to approve/adjust the P1 set.

2. **P2 stretch items.** If time allows: gaps a5 (`AriaLabel`), a7 (`SelectNodeAsync`), a8 (`MariloTreeItem.Url`), b1 + b2 (rewrite the two no-op Events section scenarios to actually fire events), c4 (`OnItemContextMenu`), d1 (empty data), d3 (selection + checkbox combo), d4 (keyboard navigation). These are 8 P2 items. Orchestrator to accept/defer.

3. **P3 defer list.** 8 items — recommend deferring to a later wave. Orchestrator to confirm.

4. **Structural decision — single file vs split.** Overview.razor already has 22 scenarios across 8 `PageSection` blocks; adding 4 P1 + up to 8 P2 pushes it toward 30+ scenarios. Two options:
   - **Option A — keep one file.** Add to Overview.razor. Preserves the current single-page URL `/components/TreeView`.
   - **Option B — split by concern.** Move Checkboxes, Events, Drag & Drop, Lazy Loading, State into dedicated sub-pages (`/components/TreeView/checkboxes`, etc.), matching how some of the larger components in the library are laid out. This would be a larger refactor and is a scope expansion beyond "fill gaps". Not recommended for Wave 2 unless orchestrator explicitly asks for it.
   - **Recommendation: Option A.** Consistent with scenario-coverage scope; page stays long but manageable with `PageSection` headings.

5. **Handling of spec-driven snippet block.** New scenarios will be authored from **source API**, not spec snippets, because of SPEC-037/SPEC-038 drift. Orchestrator to confirm this is the right call (the alternative — author against spec and have the scenarios fail to compile — is not viable).

6. **SPEC-036 upgrade applied.** Part A of this turn is complete. The `treeview-spec-gap-list.md` record for SPEC-036 is now P1 with the orchestrator-approval citation note. No other record was touched.

### Out of scope this turn (per inbox hard stops)

- Step 5 (authoring new Blazor scenarios)
- Step 6 (updating stale code snippets in existing scenarios)
- Step 9 (writing updated demo page)
- Step 10 (updating `_config/delivery-context.md`)
- Any source, spec, or provider edits

**Awaiting orchestrator review + approval. Worker will set status to `review-pending` after writing result file and handoff.**
