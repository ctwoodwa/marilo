# DataGrid Per-Feature Gap Checklist — CDW Handoff

> Date: 2026-04-03
> Source: DataGrid spec review (Stage 01 complete), GAP_ANALYSIS.md (134 tracked tasks)
> Purpose: Input for datagrid-delivery CDW Stage 01
> Component: `MariloDataGrid<TItem>` — `/workspaces/Marilo/src/Marilo.Components/DataGrid/`

## Summary

| Metric | Value |
|--------|-------|
| Parameters audited | 49 (31 grid + 14 column + 4 sub-component) |
| Events audited | 18 |
| Feature areas | 24 |
| Estimated spec coverage | ~55-60% |
| Resolved gaps (prior passes) | 44 |
| Remaining tracked tasks | 134 (Phase A: 49, B: 35, C: 29, D: 21) |
| Additional spec gaps found | ~27+ |
| Total estimated remaining gaps | ~35-50 (blocking + important + nice-to-have) |
| Tests | 4 bUnit (critically low) |
| Demo pages | 4 |

## Severity Legend

| Type | Description |
|------|-------------|
| `implemented` | Works as specced |
| `undocumented` | Exists in code but not in spec |
| `spec-ahead` | In spec but not implemented |
| `mismatch` | Exists in both but behavior/API differs |

---

## Feature Area Checklist

### 1. Component Naming & API Shape
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| Component tag `MariloGrid` vs `MariloDataGrid` | mismatch | Spec uses `<MariloGrid>`, code uses `<MariloDataGrid>`. Consumers following spec get compile errors. | **Blocking** |
| Column tag `GridColumn` vs `MariloGridColumn` | mismatch | Spec uses `<GridColumn>`, code uses `<MariloGridColumn>` | **Blocking** |
| `<GridColumns>` wrapper | spec-ahead | Spec wraps columns in `<GridColumns>` container; code uses direct children | Important |
| `<GridToolBarTemplate>` vs `ToolbarTemplate` | mismatch | Spec uses child component; code uses RenderFragment parameter | Important |

### 2. Data Binding
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `Data` | implemented | IEnumerable<TItem> binding works as specced | -- |
| `OnRead` | implemented | Server-side data binding via `GridReadEventArgs<TItem>` works | -- |
| `Rebind()` | undocumented | Public method exists but not documented as public API in spec | Low |
| `CancellationToken` in `GridReadEventArgs` | spec-ahead | QuickGrid pattern for long-running queries not implemented (A6.11) | Important |

### 3. Sorting
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `Sortable` (grid) | implemented | Works as specced | -- |
| `Sortable` (column) | implemented | Per-column control works | -- |
| Multi-sort (Ctrl+Click) | implemented | Works | -- |
| `SortMode` (Single/Multiple) | spec-ahead | Spec documents Single/Multiple enum; code always allows multi-sort | Important |

### 4. Filtering
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `FilterMode` (None/FilterMenu) | implemented | Menu mode with 11 operators works | -- |
| `Filterable` (column) | implemented | Per-column control works | -- |
| Composite filter descriptors (AND/OR) | spec-ahead | Spec requires AND/OR composition; code supports one filter per field | Important |
| `GridFilterMode.CheckBoxList` | spec-ahead | Distinct-value checkbox filter not implemented (C5) | Important |
| `FilterEditorType` / `FilterEditorFormat` | spec-ahead | Column-level filter editor customization missing (D5.1-D5.2) | Nice-to-have |
| `FilterCellTemplate` / `FilterMenuTemplate` | spec-ahead | Custom filter templates missing (D5.3) | Nice-to-have |
| `AddFilter()` / `ClearFilters()` public methods | spec-ahead | Programmatic filter control missing (A3.6) | Important |

### 5. Paging
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `Pageable` | implemented | Works | -- |
| `Page` / `PageChanged` | implemented | Bindable page works | -- |
| `PageSize` / `PageSizeChanged` | implemented | Works with dropdown | -- |
| `PageSizes` | implemented | Configurable page size options work | -- |
| `GridPagerSettings` (buttons, input, position) | spec-ahead | Spec defines rich pager with page buttons, input, position. Code has prev/next only | Important |
| `PagerTemplate` | spec-ahead | Custom pager template not implemented (D3) | Nice-to-have |

### 6. Grouping
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `Groupable` (grid) | implemented | Parameter exists | -- |
| `GroupHeaderTemplate` | implemented | RenderFragment parameter exists | -- |
| `GroupFooterTemplate` | implemented | RenderFragment parameter exists | -- |
| `Groupable` (column) | spec-ahead | Per-column groupable control missing (A1.1) | Important |
| `GroupResult<TItem>` recursive tree | spec-ahead | Recursive group data model not implemented (A1.2) | Important |
| `GroupByMany` recursive grouping | spec-ahead | Multi-level grouping engine missing (A1.3) | Important |
| Group header collapse/expand | spec-ahead | Collapse toggle UI not implemented (A1.4-A1.5) | Important |
| Group aggregate functions | spec-ahead | Count/Sum/Avg/Min/Max for group footers missing (A1.8) | Important |
| `CollapsedGroups` in GridState | spec-ahead | Group state persistence missing (A1.9) | Important |
| `GroupDescriptors` in `GridReadEventArgs` | spec-ahead | Server-side grouping support missing (A1.10) | Important |
| `HideGroupedColumn` | spec-ahead | Hide column when used as group key missing (A1.11) | Nice-to-have |
| Drag-to-group UI | spec-ahead | Spec requires drag panel; code has API-only `GroupBy()`/`Ungroup()` | Important |
| `GroupBy()` / `Ungroup()` / `UngroupAll()` | undocumented | Programmatic API exists but not in spec (spec uses drag UI) | Low |

### 7. Selection
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `SelectionMode` (None/Single/Multiple) | implemented | Row selection works | -- |
| `ShowCheckboxColumn` | implemented | Checkbox column works | -- |
| `SelectedItems` / `SelectedItemsChanged` | implemented | Two-way binding works | -- |
| Cell selection (`SelectedCells` / `SelectedCellsChanged`) | spec-ahead | Spec defines cell selection mode; code supports row-only (C7) | Important |

### 8. Editing — Modes & CRUD
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `EditMode` (None/Inline/InCell/Popup) | implemented | Three edit modes work | -- |
| `OnAdd` | implemented | Works | -- |
| `OnCreate` | implemented | Works | -- |
| `OnUpdate` | implemented | Works | -- |
| `OnDelete` | implemented | Works | -- |
| `OnEdit` | implemented | Works | -- |
| `OnCancel` | implemented | Works | -- |
| `OnCommand` | implemented | Custom command support works | -- |
| Event args type: `GridEditEventArgs<TItem>` vs `GridCommandEventArgs` | mismatch | Spec uses untyped `GridCommandEventArgs`; code uses typed `GridEditEventArgs<TItem>` | Important |
| `OnModelInit` signature | mismatch | Spec: return-based. Code: event args pattern (`GridModelInitEventArgs<TItem>`) | Important |
| `ConfirmDelete` | spec-ahead | Production delete confirmation not implemented | Important |
| `BeginCellEdit(item, field)` | undocumented | Public method exists but not documented in spec | Low |

### 9. Editing — Validation
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| DataAnnotations validation | spec-ahead | No `EditForm`/`DataAnnotationsValidator` integration (C8) | Important |
| `ValidationMessage` per field | spec-ahead | No inline validation messages (C8.2) | Important |
| Block save on validation failure | spec-ahead | `SaveEdit()` not gated by validation (C8.3) | Important |
| `ValidationMessageTemplate` | spec-ahead | Custom validation template missing (C8.4) | Nice-to-have |

### 10. Columns — Core
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `Field` | implemented | Works | -- |
| `Title` | implemented | Works | -- |
| `Width` | implemented | Works | -- |
| `Visible` | implemented | Works | -- |
| `TextAlign` | implemented | Works | -- |
| `Template` | implemented | Cell template works | -- |
| `HeaderTemplate` | implemented | Works | -- |
| `EditorTemplate` | implemented | Works | -- |
| `FooterTemplate` | implemented | Works | -- |
| `OnCellRender` | implemented | Cell render callback works | -- |
| `Format` vs `DisplayFormat` | mismatch | Spec uses `DisplayFormat` with `{0:C2}` format strings; code uses `Format` with `C2` | Important |
| `Editable` (column) | spec-ahead | Per-column edit control missing (A5.1-A5.2) | Important |
| `HeaderClass` | spec-ahead | Column header CSS class missing (A5.3) | Nice-to-have |
| `Id` (column) | spec-ahead | Column identifier missing (A5.4) | Nice-to-have |

### 11. Columns — Frozen/Locked
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `Locked` | spec-ahead | Table-stakes feature; `position: sticky` frozen columns not implemented (B5) | Important |
| `FrozenPosition` (Left/Right) | spec-ahead | Freeze direction missing (B5.2) | Important |
| `Lockable` | spec-ahead | User-toggleable lock missing (B5.3) | Nice-to-have |

### 12. Columns — Resize
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `AllowColumnResize` (grid) | spec-ahead | Table-stakes feature not implemented (B2) | Important |
| `Resizable` (column) | spec-ahead | Per-column resize control missing (B2.2) | Important |
| `MinResizableWidth` / `MaxResizableWidth` | spec-ahead | Resize constraints missing (B2.3) | Nice-to-have |
| `OnColumnResized` event | spec-ahead | Resize event callback missing (B2.8) | Important |

### 13. Columns — Reorder
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `AllowColumnReorder` (grid) | spec-ahead | Table-stakes feature not implemented (B3) | Important |
| `Reorderable` (column) | spec-ahead | Per-column reorder control missing (B3.2) | Important |
| `OrderIndex` (column) | spec-ahead | Column order persistence missing (B3.3) | Important |
| `OnColumnReordering` / `OnColumnReordered` events | spec-ahead | Reorder events missing (B3.5) | Important |

### 14. Columns — Menu & Chooser
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `ShowColumnMenu` | spec-ahead | Column context menu not implemented (C3) | Nice-to-have |
| `VisibleInColumnChooser` | spec-ahead | Column chooser visibility control missing (A5.6) | Nice-to-have |
| Column chooser dialog | spec-ahead | `MariloGridColumnChooser` component missing (C4) | Nice-to-have |
| `ColumnChooserTemplate` | spec-ahead | Custom chooser template missing (C4.5) | Nice-to-have |

### 15. Columns — Multi-Column Headers
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `MariloGridColumnGroup` | spec-ahead | Stacked/grouped headers not implemented (C6) | Nice-to-have |
| Multi-row `<thead>` | spec-ahead | Nested header rendering missing (C6.2) | Nice-to-have |

### 16. Columns — Auto-Generate
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `AutoGenerateColumns` | implemented | Parameter exists | -- |
| Reflect `typeof(TItem)` properties | spec-ahead | Auto-generation logic not fully implemented (A2.2) | Important |
| `[Display]` / `[Editable]` attribute support | spec-ahead | Attribute-driven column config missing (A2.3) | Important |
| Override auto-generated with explicit columns | spec-ahead | Merge logic missing (A2.4) | Important |

### 17. Row Features
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `OnRowClick` | implemented | Works | -- |
| `OnRowDoubleClick` | implemented | Works | -- |
| `OnRowContextMenu` | implemented | Works | -- |
| `OnRowRender` | implemented | Row render callback works | -- |
| `OnRowExpand` / `OnRowCollapse` | implemented | Works (but see mismatch below) | -- |
| Expand/collapse event args | mismatch | Spec uses typed event args; code uses direct `EventCallback<TItem>` | Important |
| Row drag-and-drop (`OnRowDrop`) | spec-ahead | Row reordering via drag not implemented (B4) | Important |
| `RowDraggable` | spec-ahead | Drag enable parameter missing (B4.1) | Important |

### 18. Detail Template
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `DetailTemplate` | implemented | RenderFragment<TItem> works | -- |
| `OnRowExpand` / `OnRowCollapse` | implemented | Expand/collapse events work | -- |

### 19. Templates
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `ToolbarTemplate` | implemented | Works (as parameter, not child component) | -- |
| `NoDataTemplate` | implemented | Empty state template works | -- |
| `RowTemplate` | implemented | Custom row rendering works | -- |
| `PopupFormTemplate` | spec-ahead | Popup edit form template missing (D2.1) | Nice-to-have |
| `PopupButtonsTemplate` | spec-ahead | Popup edit buttons template missing (D2.2) | Nice-to-have |

### 20. State Management
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `OnStateInit` | implemented | Works | -- |
| `OnStateChanged` | implemented | Works | -- |
| `GridState` (non-generic) | mismatch | Spec requires `GridState<TItem>`; code uses untyped `GridState` | Important |
| `SetStateAsync()` | spec-ahead | Programmatic state setter missing | Important |
| `EditItem` / `OriginalEditItem` / `InsertedItem` in state | spec-ahead | Editing state not in GridState (A6.1-A6.3) | Important |
| `ExpandedItems` in state | spec-ahead | Detail expansion state not persisted (A6.4) | Important |
| `ColumnStates` in state | spec-ahead | Column order/width/visible state missing (A6.5) | Important |
| `SearchFilter` in state | spec-ahead | Search term not persisted (A6.6) | Nice-to-have |
| `Skip` in state | spec-ahead | Virtual scroll position not persisted (A6.7) | Nice-to-have |

### 21. Search
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `ShowSearchBox` | undocumented | Parameter exists but spec references only as toolbar tool | Low |
| `SearchBoxPlaceholder` | undocumented | Exists in code, not in spec | Low |
| Debounced search (`SearchDelay`) | spec-ahead | Configurable debounce missing (A3.5) | Nice-to-have |
| `SearchFilter` in GridState | spec-ahead | Search term persistence missing (A3.4) | Nice-to-have |

### 22. Virtual Scrolling
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `EnableVirtualization` | mismatch | Spec requires `ScrollMode` enum (Scrollable/Virtual) + `RowHeight` decimal; code uses bool | **Blocking** |
| `VirtualizeOverscanCount` | undocumented | Exists in code, not in spec | Low |

### 23. Export
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `ExportToCsv()` (returns string) | mismatch | Spec expects JS-triggered browser download; code returns string only | Important |
| `OnBeforeExport` / `OnAfterExport` events | spec-ahead | Export lifecycle events missing (A8.5) | Important |
| `ExportAllPages` option | spec-ahead | All-data export missing (A8.6) | Important |
| Excel export (`ExportToExcel()`) | spec-ahead | Requires ClosedXML dependency (C1) | Nice-to-have |
| PDF export (`ExportToPdf()`) | spec-ahead | Requires QuestPDF dependency (C2) | Nice-to-have |

### 24. Keyboard Navigation & Accessibility
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `Navigable` | implemented | Parameter exists (placeholder — no keyboard logic yet) | -- |
| Cell focus tracking | spec-ahead | `_focusedRow` / `_focusedCol` tracking not implemented (B1.2) | Important |
| Arrow key navigation | spec-ahead | JS `focusCell()` + C# index tracking missing (B1.3-B1.4) | Important |
| Enter/Escape/Tab edit triggers | spec-ahead | Keyboard edit shortcuts missing (B1.5-B1.7) | Important |
| `aria-activedescendant` | spec-ahead | Screen reader support missing (B1.9) | Important |
| `CustomKeyboardShortcuts` | spec-ahead | User-defined shortcut dictionary missing (B1.10) | Nice-to-have |

---

## Auxiliary Components

### MariloGridCommandButton
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| Command types (Add/Edit/Save/Cancel/Delete) | implemented | Works | -- |
| Custom commands | implemented | Works via `OnCommand` | -- |
| `GridCommandPlacement` enum | implemented | Works | -- |

### MariloGridToolbar
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| Toolbar container | implemented | Bare container works | -- |
| Built-in toolbar tools (13 specced) | spec-ahead | Spec defines 13 `GridToolBar*` components; code has bare container only (D4) | Nice-to-have |

### MariloDataSheet (sibling component)
Not in scope for this checklist — has its own delivery flow.

---

## JS Interop Infrastructure (Cross-Cutting)

| Item | Type | Gap Description | Severity |
|------|------|-----------------|----------|
| `marilo-datagrid.js` ES module | spec-ahead | No JS module exists; needed for B-phase features (B0.1) | Important |
| `IJSRuntime` injection | spec-ahead | Not injected in grid component (B0.4) | Important |
| `IAsyncDisposable` for JS cleanup | spec-ahead | Not implemented (B0.3) | Important |
| `<colgroup>/<col>` elements | spec-ahead | Needed for efficient column width management (B0.5) | Important |

---

## Sizing & Appearance
| Parameter/Event | Type | Gap Description | Severity |
|----------------|------|-----------------|----------|
| `Height` | implemented | Works | -- |
| `Width` | implemented | Works | -- |
| `Striped` | undocumented | Works but not in spec overview | Low |
| `IsLoading` | implemented | Loading overlay works | -- |
| `Size` (Small/Medium/Large) | spec-ahead | Size enum with CSS variable mapping missing (A7.1-A7.2) | Nice-to-have |
| `HighlightedItems` | spec-ahead | Row highlighting missing (A7.3-A7.4) | Nice-to-have |
| `AdaptiveMode` | spec-ahead | Responsive card layout missing (D6) | Nice-to-have |

---

## Gap Count by Severity

| Severity | Count |
|----------|-------|
| Blocking | 3 (naming x2, virtual scroll API) |
| Important | 38 |
| Nice-to-have | 24 |
| Undocumented (code-only) | 6 |
| **Total gaps** | **71** |

## Gap Count by Type

| Type | Count |
|------|-------|
| `implemented` | 42 parameters/events working as specced |
| `spec-ahead` | 52 items in spec but not implemented |
| `mismatch` | 9 items with API/behavior differences |
| `undocumented` | 6 items in code but not in spec |

## Phase Mapping (from GAP_ANALYSIS.md)

| Phase | Scope | Tasks | Priority |
|-------|-------|-------|----------|
| **A — Pure C#** | Grouping, AutoGen, Search, Templates, Columns, State, Sizing, CSV Export | 49 | High (no JS dependency) |
| **B — JS Interop** | Keyboard Nav, Column Resize, Column Reorder, Row Drag, Frozen Columns | 35 | High (table-stakes: B2, B3, B5) |
| **C — Advanced** | Excel/PDF Export, Column Menu/Chooser, CheckBoxList Filter, Multi-Headers, Cell Selection, Validation | 29 | Medium |
| **D — Future** | AI Features, Popup Templates, Pager Template, Toolbar Tools, Advanced Column Types, AdaptiveMode | 21 | Low |

## Recommended CDW Sequencing

1. **Resolve blocking gaps first** — naming decision (#1, #2) and virtual scroll API (#22) gate all other work
2. **Phase A (Pure C#)** — no external dependencies, can start immediately
3. **Phase B table-stakes** — Column resize (B2), reorder (B3), frozen (B5) are customer-expected features
4. **Phase C validation** — Editing validation (C8) is a production blocker despite being Phase C
5. **Phase B keyboard** — Accessibility compliance requires B1
6. **Remaining C and D** — schedule based on customer demand

---

*Generated from: `datagrid-spec-gaps.md`, `GAP_ANALYSIS.md`, and source file audit of MariloDataGrid.*
*Cross-reference: `/workspaces/Marilo/workspaces/datagrid-delivery/stages/01-spec-review/output/datagrid-spec-gaps.md`*
