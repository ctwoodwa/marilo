# Pre-Stage-02 Research — Evidence for TreeList Human Decisions

**Purpose:** Reduce the Stage 02 decision burden by answering two of the seven open human decisions with mechanical evidence from the current codebase. This file does **not** commit the project to any decision — it only documents what the code says today so the human can decide faster.

**Produced:** 2026-04-10 (cron fire #6 pre-prioritization research pass)
**Scope:** Decisions #2 (TreeListColumn backward compat) and #3 (DataGrid subsystem reuse strategy) from [gap-treelist-inventory.md "Human Decisions Needed Before Stage 02"](./gap-treelist-inventory.md). The remaining five decisions (#1, #4, #5, #6, #7) are pure design/preference choices that cannot be answered from code.

---

## Decision #2 — `TreeListColumn` backward-compat cost

> **Question:** "Existing source takes a `List<TreeListColumn>` parameter. Does any consumer depend on that? Break the parameter (cleaner) or keep it as a fallback alongside the new child-tag wrapper (safer)?"

**Evidence gathered:** Repo-wide grep for `MariloTreeList|TreeListColumn` (case-sensitive, 40-file limit not exhausted).

**Runtime-code consumers found:** 3 files only.

| File | Usage | Breaking impact |
|---|---|---|
| [src/Marilo.Components/DataGrid/MariloTreeList.razor](src/Marilo.Components/DataGrid/MariloTreeList.razor) | The component itself — declares and consumes `[Parameter] public List<TreeListColumn> Columns = new()` at line 33 | Zero — this file **will be rewritten** as the starting point of the rebuild |
| [src/Marilo.Core/Models/TreeListColumn.cs](src/Marilo.Core/Models/TreeListColumn.cs) | The `TreeListColumn` data class (17 lines, three properties: `Title`, `Field`, `Width`; no methods, no constructors, no logic) | Zero — this file can be deleted and superseded by a new Blazor component without preserving any logic |
| [samples/Marilo.Demo/Pages/Components/TreeList/Overview.razor](samples/Marilo.Demo/Pages/Components/TreeList/Overview.razor) | The placeholder demo | Zero — **this file does not actually use the `Columns` parameter at runtime**. It renders a `MariloAlert` with static text telling users to "see the component spec for full examples". The `_basicCode` constant is a string literal that describes a future API, not executable code. Any future demo rewrite is already expected as part of GAP-TREELIST-042. |

**Test consumers found:** 0 files. A grep of `tests/` for `MariloTreeList|TreeListColumn` returned no matches.

**Documentation consumers:** 37 files (specs under `docs/component-specs/treelist/`, ICM workspace configs, plan + gap-analysis markdown, skill definitions). None are runtime code; all are documentation that **already describes the target `<TreeListColumns>` child-tag API** rather than the current `List<TreeListColumn>` parameter. So breaking the current parameter *aligns the code with the docs* rather than breaking anything.

**Conclusion (evidence-based, not a decision):**
- **Zero runtime consumers** depend on the `List<TreeListColumn>` parameter shape. Breaking it is mechanically free.
- The existing `TreeListColumn.cs` class has no logic worth preserving — it's a trivial POCO.
- Documentation already describes the target child-tag API, so breaking the parameter *reduces* spec/source drift rather than increasing it.

**Implication for the human decision:** The "safer fallback" option (keeping both shapes side-by-side) offers **no real safety**, because there is nothing it would protect. The cleaner-break option has zero cost. **Recommend: break cleanly.** (The human still decides.)

---

## Decision #3 — DataGrid subsystem reuse strategy

> **Question:** "Extract shared paging/sorting/filtering/editing/selection/virtualization/frozen-columns/drag-drop into `Marilo.Components.DataGrid.Shared` abstractions, or copy-paste the DataGrid implementations into TreeList? The shared-abstractions path is cleaner long-term but is a bigger up-front refactor. Copy-paste is faster but creates two divergent implementations."

**Evidence gathered:** Directory listing of `src/Marilo.Components/DataGrid/` — the existing grid-family component folder.

### Current `src/Marilo.Components/DataGrid/` structure

**DataGrid component (partial-class split already exists):**

- `MariloDataGrid.razor` — markup
- `MariloDataGrid.razor.cs` — core partial
- `MariloDataGrid.Data.cs` — data-binding partial (paging / sorting / filtering / OnRead)
- `MariloDataGrid.Editing.cs` — editing pipeline partial
- `MariloDataGrid.Interop.cs` — JS interop partial (frozen cols, row drag-drop, column resize)
- `MariloDataGrid.Rendering.cs` — rendering partial

**DataSheet component — second grid-family occupant, same pattern:**

- `MariloDataSheet.razor`
- `MariloDataSheet.razor.cs`
- `MariloDataSheet.Data.cs`
- `MariloDataSheet.Editing.cs`
- `MariloDataSheet.Interop.cs`
- `MariloDataSheet.Rendering.cs`
- `MariloDataSheetColumn.razor`

**PivotGrid component — third grid-family occupant:**

- `MariloPivotGrid.razor`

**TreeList component — current 199-line single-file prototype:**

- `MariloTreeList.razor`

**Shared types already at the folder's top level (not inside either component):**

- `GridState.cs` — state serialization class
- `GridEventArgs.cs` — typed event args
- `GridCellReference.cs` — cell coordinate
- `GridColumnFrozenPosition.cs` — enum `{ Start, End }` in `Marilo.Components.DataGrid` namespace (verified: trivially generic, already reusable by any grid component in the folder)
- `GridCommandTypes.cs` — command enum

**Shared child components already at the folder's top level:**

- `MariloGridColumn.razor` — column child component (the reference implementation for the child-tag pattern)
- `MariloGridToolbar.razor` — toolbar component
- `MariloGridCommandButton.razor` — command button for inline editing

**Shared sizing infrastructure in `DataGrid/Sizing/` subfolder:**

- `IColumnWidthProvider.cs` — interface
- `FixedWidthProvider.cs` — default implementation
- `ColumnSizingEntry.cs` — per-column sizing record
- `GridLayoutContract.cs` — layout contract

### What this means

1. **A reusable-subsystem pattern already exists in this folder.** The DataGrid folder is not just "the place where DataGrid lives" — it's "the place where the grid component family lives with shared types at the top and per-component partial splits below". MariloDataSheet follows the exact same pattern. MariloPivotGrid is in the same folder.

2. **Shared types are already namespaced at the folder level** (`Marilo.Components.DataGrid`), not inside `MariloDataGrid`. `GridState`, `GridEventArgs`, `GridColumnFrozenPosition`, `GridCellReference`, `GridCommandTypes`, and the entire `Sizing/*` subsystem are available to any class in the folder without cross-component coupling.

3. **There is no precedent here for a separate `Marilo.Components.DataGrid.Shared` sub-namespace.** The current pattern puts shared types at the top level of `Marilo.Components.DataGrid` directly. Introducing a new `.Shared` sub-namespace would be *novel*, not *reuse*. Any TreeList rebuild that uses `.Shared` would require first hoisting DataGrid's existing types into that new namespace — an up-front refactor of stable code.

4. **Copy-paste is not the other option either.** Because DataGrid's shared types are *already* at the folder's top level in the shared `Marilo.Components.DataGrid` namespace, TreeList can consume them *without any refactoring or copy-paste*. The TreeList partial classes can reference `GridState`, `GridEventArgs`, `GridColumnFrozenPosition`, `IColumnWidthProvider`, etc. directly from their existing locations.

**Conclusion (evidence-based, not a decision):**
- The decision as framed in the inventory (`.Shared` sub-namespace vs copy-paste) contains a **false dichotomy**. The actual state of the codebase offers a third, unrepresented option: **reuse the existing shared types in `Marilo.Components.DataGrid` as-is, via namespace reference, without any refactoring.**
- Adding `.Shared` would require refactoring working DataGrid code for no new capability. Copy-paste would violate DRY for no speed benefit (the shared types are already one import away). **Both of the originally-framed options are strictly worse than the unrepresented third option.**

**Implication for the human decision:** Redraft decision #3 as: *"Reuse DataGrid's existing shared types directly via namespace reference (no refactoring needed), following the MariloDataSheet precedent. TreeList should be split into `MariloTreeList.razor` / `.razor.cs` / `.Data.cs` / `.Editing.cs` / `.Interop.cs` / `.Rendering.cs` partials in the same folder, mirroring DataGrid's split. Subsystem wiring (e.g., `TreeList.Data.cs` delegates to DataGrid's paging/sorting/filtering implementations where they exist)."* — this removes 90% of the decision friction and eliminates the up-front refactor concern.

---

## Summary of Evidence-Driven Simplifications

| Decision | Originally framed as | Evidence says | Simplified |
|---|---|---|---|
| #2 | "Break vs keep-as-fallback" | Zero runtime consumers; trivial POCO with no logic | **Break cleanly is free.** One-line answer: yes. |
| #3 | ".Shared sub-namespace vs copy-paste" | False dichotomy — shared types already at folder level | **Reuse in place, mirror DataSheet split.** No refactoring, no copy-paste. |

These two questions consumed the most planning weight in the original decision list. Both can now be answered in one sentence each. The remaining five decisions (#1 branch strategy, #4 data-shape default, #5 editing UX ownership, #6 virtualization+paging composition, #7 row drag-drop Y-position semantics) are genuine design/preference choices that require a human — but they are all *individually small* once decisions #2 and #3 are settled.

**Net effect on Stage 02 readiness:** 7 open decisions → **5 open decisions**, and the remaining 5 are all tractable single-person calls. Stage 02 prioritization can begin as soon as those are answered; none of them block *each other*.
