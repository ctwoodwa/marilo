# Stage 02 -- Gap Prioritization: MariloTreeList

**Date:** 2026-04-12
**Worker:** w-treelist-gap-analysis
**Input:** Stage 01 inventory (47 gaps: 43 functional + 4 style)
**Scope classification:** systematic

---

## Prioritization Strategy

MariloTreeList is at early scaffold stage (~18% implemented). The prioritization follows two principles:

1. **Dependency ordering** -- GAP-001 (child-tag architecture) is the critical path that unblocks everything else.
2. **DataGrid reuse** -- 22 of 43 functional gaps (~51%) are direct DataGrid-parity items. These can reuse DataGrid subsystem implementations, dramatically reducing scope.

Tree-specific gaps (002, 004, 006, 017, 034, 036, 037, 041) require original work and are scheduled based on their blocking impact.

---

## Wave 1: Critical Blockers (Basic Usage)

**Goal:** Make MariloTreeList compilable against spec examples and usable for read-only hierarchical display.

| Gap ID | Description | Severity | Complexity |
|--------|-------------|----------|------------|
| GAP-001 | `<TreeListColumns>` / `<TreeListColumn>` child-tag architecture | Critical | High |
| GAP-002 | `Expandable` column parameter | Critical | Low |
| GAP-039 | Height / Width / Class / Navigable parameters | Medium | Low |
| GAP-036 | ARIA attributes (aria-expanded, aria-setsize, aria-posinset) | High | Low |
| STYLE-03 | BEM naming inconsistency (toggle class) | Low | Trivial |
| STYLE-04 | Inline styles migrate to SCSS | Low | Low |
| STYLE-01 | FluentUI `_treelist.scss` | High | Medium |
| STYLE-02 | Bootstrap `_treelist.scss` | High | Medium |

**Files touched:** ~8
- `src/Marilo.Components/DataGrid/MariloTreeList.razor` (major rewrite)
- NEW: `src/Marilo.Components/DataGrid/TreeListColumns.razor`
- NEW: `src/Marilo.Components/DataGrid/TreeListColumn.razor` (Blazor component)
- `src/Marilo.Core/Models/TreeListColumn.cs` (deprecate or rename to avoid collision)
- NEW: FluentUI `_treelist.scss`
- NEW: Bootstrap `_treelist.scss`

**Complexity:** High -- GAP-001 is the single largest structural change. It converts the component from POCO-parameter-driven to child-tag-driven, following the MariloDataGrid / MariloGridColumn pattern. Every subsequent wave depends on this.

**Architecture decisions required before starting:**
- Decision 1: TreeListColumn backward compat strategy
- Decision 3: Branch strategy

**Estimated scope:** ~400-600 lines new/modified across 6-8 files.

---

## Wave 2: Data Operations (Enterprise Readiness)

**Goal:** Paging, sorting, filtering, remote data, and basic templates -- the minimum for enterprise data display.

| Gap ID | Description | Severity | Complexity | DataGrid Reuse? |
|--------|-------------|----------|------------|-----------------|
| GAP-007 | Paging | Critical | Medium | Yes -- reuse MariloPagination |
| GAP-008 | Sorting | Critical | Medium | Yes -- reuse DataGrid sorting subsystem |
| GAP-009 | FilterMode (Menu/Row/Checkbox) | High | High | Yes -- reuse DataGrid filter subsystem |
| GAP-010 | Filter SearchBox | Medium | Low | Yes -- toolbar child |
| GAP-033 | OnRead remote-data pattern | High | Medium | Yes -- mirror GridReadEventArgs |
| GAP-026 | Column DisplayFormat | Medium | Low | No -- small addition |
| GAP-028 | Cell Template (RenderFragment<TItem>) | High | Medium | No -- but follows DataGrid pattern |
| GAP-029 | HeaderTemplate | Medium | Low | No -- follows DataGrid pattern |
| GAP-031 | NoDataTemplate | Medium | Low | No -- trivial conditional |
| GAP-034 | Expand/collapse events | Medium | Low | No -- tree-specific, trivial |
| GAP-042 | Demo: replace placeholder with working examples | Medium | Medium | N/A |

**Files touched:** ~12-15
- `MariloTreeList.razor` (add paging/sorting/filtering parameters + render logic)
- `TreeListColumn.razor` (add Sortable, Filterable, DisplayFormat, Template, HeaderTemplate)
- NEW: `TreeListFilterMode.cs` enum
- NEW: `TreeListReadEventArgs.cs`
- NEW: `TreeListExpandEventArgs.cs`
- Demo page update
- Test files

**Complexity:** High overall, but most items reuse proven DataGrid subsystems. Tree-specific consideration: sorting must sort within-parent (preserving hierarchy), and filtering must keep ancestor path visible when a child matches.

**Architecture decisions required:**
- Decision 2: DataGrid subsystem reuse strategy (extract shared abstractions vs copy-paste)

**Estimated scope:** ~800-1200 lines across 12-15 files.

---

## Wave 3: Feature Completeness (Selection, Editing, State, Templates)

**Goal:** Full read-write capability, selection, state persistence, and remaining templates.

### Wave 3A: Selection + State

| Gap ID | Description | Severity | Complexity | DataGrid Reuse? |
|--------|-------------|----------|------------|-----------------|
| GAP-013 | Selection (row + cell) | High | Medium | Yes |
| GAP-014 | Checkbox column | Medium | Medium | Partial -- tri-state is tree-specific |
| GAP-016 | State API (GetState/SetState/OnStateChanged) | High | Medium | Yes -- mirror DataGrid pattern |

### Wave 3B: Editing Pipeline

| Gap ID | Description | Severity | Complexity | DataGrid Reuse? |
|--------|-------------|----------|------------|-----------------|
| GAP-011 | Editing modes (Inline/InCell/Popup) | Critical | High | Yes -- major DataGrid reuse |
| GAP-012 | CRUD events | High | Medium | Yes |
| GAP-015 | Command column | Medium | Medium | Yes |
| GAP-005 | TreeListToolBar | High | Medium | Yes -- mirror MariloGridToolbar |
| GAP-032 | Editor/Popup/Filter/Pager templates | Low | Medium | Yes -- follows from parent features |

### Wave 3C: Templates + Events

| Gap ID | Description | Severity | Complexity | DataGrid Reuse? |
|--------|-------------|----------|------------|-----------------|
| GAP-030 | RowTemplate | Medium | Low | No |
| GAP-035 | OnItemRender event | Medium | Low | Yes |

**Files touched:** ~20-25
- `MariloTreeList.razor` (selection, editing, state params)
- `TreeListColumn.razor` (Editable, per-column events)
- NEW: `TreeListState.cs`
- NEW: `TreeListEditMode.cs` enum
- NEW: `TreeListCommandColumn.razor`
- NEW: `TreeListCheckboxColumn.razor`
- NEW: `TreeListToolBar.razor` + tool children
- NEW: Event arg types (5-8 new classes)
- Test files

**Complexity:** High -- editing is the largest subsystem. However, if DataGrid reuse strategy is in place from Wave 2, much of this is wiring.

**Architecture decisions required:**
- Decision 5: Editing UX (built-in popup vs consumer EditTemplate)

**Estimated scope:** ~1500-2000 lines across 20-25 files.

---

## Wave 4: Nice-to-Haves (Advanced Features)

**Goal:** Power-user features, performance optimization, and documentation polish.

### Wave 4A: Column Advanced Features

| Gap ID | Description | Severity | Complexity | DataGrid Reuse? |
|--------|-------------|----------|------------|-----------------|
| GAP-019 | Column resizing | Medium | Medium | Yes -- DataGrid JS interop |
| GAP-020 | Column reordering | Medium | Medium | Yes -- DataGrid subsystem |
| GAP-021 | Frozen/Locked columns | Medium | Medium | Yes -- DataGrid implementation |
| GAP-022 | Column Menu | Medium | Medium | Yes -- DataGrid component |
| GAP-023 | Multi-column headers | Low | Medium | Partial |
| GAP-024 | Auto-generated columns | Low | Medium | Yes |
| GAP-025 | Column Visible / column chooser | Low | Low | Yes |
| GAP-027 | Column events | Low | Low | No |

### Wave 4B: Performance + Data

| Gap ID | Description | Severity | Complexity | DataGrid Reuse? |
|--------|-------------|----------|------------|-----------------|
| GAP-017 | Row virtualization | High | High | Partial -- tree-specific flattened list |
| GAP-018 | Column virtualization | Low | Medium | Yes |
| GAP-003 | Interface-driven binding | High | Medium | No |
| GAP-004 | Load-on-demand | High | Medium | No -- tree-specific |
| GAP-006 | Expression<Func> field overloads | Medium | Low | No |

### Wave 4C: Interaction + Polish

| Gap ID | Description | Severity | Complexity | DataGrid Reuse? |
|--------|-------------|----------|------------|-----------------|
| GAP-037 | Keyboard navigation | High | High | Partial -- tree-specific arrow keys |
| GAP-041 | Row drag-drop / reparenting | High | High | Partial -- tree-specific cycle detection |
| GAP-040 | Aggregates | Medium | Medium | Yes -- DataGrid subsystem |
| GAP-038 | Public methods (Rebind, AutoFit) | Medium | Low | Yes |
| GAP-043 | Additional demo pages | Low | Low | N/A |

**Architecture decisions required:**
- Decision 6: Virtualization + paging composition
- Decision 7: Row drag-drop reparenting semantics

**Estimated scope:** ~2000-3000 lines across 25-35 files.

---

## Wave Summary

| Wave | Gap Count | Critical | High | Scope (est. lines) | Key Dependency |
|------|-----------|----------|------|--------------------:|----------------|
| 1: Critical Blockers | 8 | 2 | 2 | 400-600 | None (start here) |
| 2: Data Operations | 11 | 2 | 3 | 800-1200 | Wave 1 complete |
| 3: Feature Completeness | 11 | 1 | 5 | 1500-2000 | Wave 2 complete |
| 4: Nice-to-Haves | 17 | 0 | 7 | 2000-3000 | Wave 3 complete (mostly) |
| **Total** | **47** | **5** | **17** | **4700-6800** | |

---

## Cross-Wave Dependencies

```
Wave 1 (Architecture)
  |
  +---> Wave 2 (Data Ops)
  |       |
  |       +---> Wave 3A (Selection + State)
  |       |       |
  |       |       +---> Wave 3B (Editing) ---> Wave 3C (Templates)
  |       |
  |       +---> Wave 4A (Column Advanced) [can start after Wave 2]
  |       |
  |       +---> Wave 4B (Performance) [can start after Wave 2]
  |
  +---> Wave 4C (A11y keyboard nav) [can start after Wave 1]
```

**Parallelization opportunities:**
- Wave 3A and Wave 4A can run in parallel after Wave 2
- Wave 4B (virtualization, load-on-demand) can run in parallel with Wave 3
- Wave 4C keyboard nav can start as soon as Wave 1 lands (does not need data ops)

---

## Architecture Decisions Summary

| # | Decision | Needed By | Impact |
|---|----------|-----------|--------|
| 1 | TreeListColumn backward compat | Wave 1 start | Breaking change scope |
| 2 | DataGrid subsystem reuse strategy | Wave 2 start | Implementation velocity for 22 gaps |
| 3 | Branch strategy | Wave 1 start | Release management |
| 4 | Flat vs hierarchical default | Wave 2 (demo) | Documentation, not code |
| 5 | Editing UX (popup vs template) | Wave 3B start | Editing architecture |
| 6 | Virtualization + paging composition | Wave 4B start | Can defer |
| 7 | Row drag-drop semantics | Wave 4C start | Can defer |

**Decisions 1-3 must be resolved before any implementation begins.**
Decision 2 has the highest long-term impact: if shared abstractions are extracted, every DataGrid-parity gap benefits. If copy-paste is chosen, TreeList and DataGrid diverge permanently.

---

## Recommended First Wave Scope

Start with Wave 1 only. It delivers:
- Spec-compilable `<TreeListColumns>` / `<TreeListColumn>` declarative API
- Configurable Expandable column placement
- Proper ARIA treegrid attributes
- Base SCSS for both providers
- Height/Width/Class/Navigable parameters

This transforms MariloTreeList from "scaffold that cannot run spec examples" to "functional read-only hierarchical display with proper accessibility and styling."

**Prerequisite:** Resolve architecture decisions 1 and 3 with the orchestrator/user.
