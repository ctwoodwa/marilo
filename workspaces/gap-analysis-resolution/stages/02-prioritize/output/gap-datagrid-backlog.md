# DataGrid Resolution Backlog

> Date: 2026-04-04
> Source: `stages/01-intake/output/gap-datagrid-per-feature-checklist.md`
> Component: `MariloDataGrid<TItem>` — `src/Marilo.Components/DataGrid/`
> Scope: batch (71 gaps across 24 feature areas)

## Scoring Dimensions

| Dimension | Weight | Description |
|-----------|--------|-------------|
| Risk | High | Does this gap cause runtime errors, data loss, or security issues? |
| User Impact | High | How many users hit this gap in normal usage? |
| Architectural | Medium | Does resolving this unblock other gaps or components? |
| Effort | Medium | Implementation complexity (inverse — lower effort = higher priority) |

## Priority Bands

### Blocking (must resolve before any release)

| ID | Gap | Area | Rationale | Status |
|----|-----|------|-----------|--------|
| DG-B1 | Component naming `MariloGrid` vs `MariloDataGrid` | API Shape | Spec/code mismatch — spec needs updating to match code. Code already uses correct `MariloDataGrid` naming. | **Pre-resolved** (rename cleanup 2026-04-03) |
| DG-B2 | Column tag `GridColumn` vs `MariloGridColumn` | API Shape | Spec/code mismatch — spec needs updating. Code uses `MariloGridColumn`. | **Pre-resolved** (code is correct) |
| DG-B3 | Virtual scroll API: `EnableVirtualization` bool vs `ScrollMode` enum | Virtual Scrolling | API mismatch with spec. Current bool works; spec wants enum for future scroll modes. | Defer — current API is functional; enum migration is breaking change |

### Phase 1 — Pure C# Critical (no JS dependency, high user impact)

| ID | Gap | Area | Risk | Impact | Arch | Effort | Score | Deps |
|----|-----|------|------|--------|------|--------|-------|------|
| DG-P1-01 | `SortMode` (Single/Multiple) enum | Sorting | Low | High | Low | Low | 8 | None |
| DG-P1-02 | `Editable` column parameter | Columns | Med | High | Low | Low | 9 | None |
| DG-P1-03 | `ConfirmDelete` parameter | Editing | Med | High | Low | Low | 9 | None |
| DG-P1-04 | `SetStateAsync()` public method | State | Low | High | Med | Low | 8 | None |
| DG-P1-05 | `AddFilter()`/`ClearFilters()` public methods | Filtering | Low | High | Low | Low | 8 | None |
| DG-P1-06 | Pager with page number buttons | Paging | Low | High | Low | Med | 7 | None |
| DG-P1-07 | `Format` vs `DisplayFormat` column parameter | Columns | Low | Med | Low | Low | 6 | None |
| DG-P1-08 | `Groupable` per-column parameter | Grouping | Low | Med | Low | Low | 6 | None |
| DG-P1-09 | `ExpandedItems` in GridState | State | Low | Med | Med | Low | 6 | None |
| DG-P1-10 | Expand/collapse event args typed | Row Features | Low | Med | Low | Low | 5 | None |

### Phase 2 — Pure C# Important (medium impact)

| ID | Gap | Area | Score | Deps |
|----|-----|------|-------|------|
| DG-P2-01 | Per-column `Groupable` control | Grouping | 6 | None |
| DG-P2-02 | `GridState<TItem>` generic state | State | 5 | DG-P1-04 |
| DG-P2-03 | DataAnnotations validation integration | Editing | 7 | None |
| DG-P2-04 | Composite filter descriptors (AND/OR) | Filtering | 5 | None |
| DG-P2-05 | Auto-generate columns with `[Display]`/`[Editable]` attributes | Columns | 5 | None |
| DG-P2-06 | Group aggregate functions (Count/Sum/Avg/Min/Max) | Grouping | 5 | None |
| DG-P2-07 | `ExportAllPages` option + export lifecycle events | Export | 4 | None |
| DG-P2-08 | `CancellationToken` in `GridReadEventArgs` | Data Binding | 4 | None |

### Phase 3 — JS Interop & Advanced (require browser interaction)

| ID | Gap | Area | Score | Deps |
|----|-----|------|-------|------|
| DG-P3-01 | Frozen/Locked columns (`position: sticky`) | Columns | 7 | JS interop exists |
| DG-P3-02 | Cell selection mode | Selection | 5 | None |
| DG-P3-03 | Row drag-and-drop reorder | Row Features | 5 | JS interop exists |
| DG-P3-04 | `CheckBoxList` filter mode | Filtering | 4 | None |

### Phase 4 — Nice-to-Have (defer)

All remaining 24 nice-to-have gaps from the checklist. Defer to future cycles.

## Dependency Graph

```
DG-P1-04 (SetStateAsync) ← DG-P2-02 (GridState<TItem>)
All Phase 1 items are independent of each other.
All Phase 2 items are independent except DG-P2-02.
Phase 3 items require JS module (already exists in MariloDataGrid.Interop.cs).
```

## Sequencing

1. **Phase 1 items DG-P1-01 through DG-P1-06** — implement in current cycle (highest ROI, no JS)
2. **Phase 1 items DG-P1-07 through DG-P1-10** — implement in current cycle if time permits
3. **Phase 2** — next cycle
4. **Phase 3** — after Phase 2 foundation
5. **Phase 4** — backlog

## Test Coverage Target

Current: 4 bUnit tests (critically low)
Target after Phase 1: ≥20 bUnit tests covering all Phase 1 features
