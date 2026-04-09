# FileManager Phase F — Implementation Log

> Stage 05 · Phase F (Polish) · Date: 2026-04-09

## Files Modified

### `src/Marilo.Components/Forms/Inputs/MariloFileManager.razor.cs`

**Additions:**
- State fields: `_sortField = "Name"`, `_sortAscending = true`, `_isLoading = false`
- Parameters: `Width` (string?), `EnableLoaderContainer` (bool)
- Methods: `SetSortField(string)`, `ToggleSortDirection()`, `HandleSortFieldChange(ChangeEventArgs)`, `GetWidthStyle()`, `GetContainerStyle()`
- `LoadDataAsync()` updated: sets `_isLoading = true` before OnRead, `= false` after
- `GetCurrentItems()` replaced with dynamic sort dispatch (switch expression on `_sortField`)

**Constraints satisfied:**
- `CombineStyles` only accepts one argument — combined Height+Width via `GetContainerStyle()`
- Razor parser rejects embedded double-quotes in attribute lambdas — extracted sort-change handler to named method `HandleSortFieldChange`
- `IEnumerable<TItem>` (not `var`) used for `filtered` variable to allow reassignment to `.Where(...)` result (per cerebrum rule on IOrderedEnumerable vs IEnumerable)

### `src/Marilo.Components/Forms/Inputs/MariloFileManager.razor`

**Root element:**
- `style` → `CombineStyles(GetContainerStyle())`
- Added `role="application"`, `aria-label="File Manager"`

**Toolbar:**
- Added `role="toolbar"`, `aria-label="File manager toolbar"` to the toolbar `<div>`
- Added sort `<select>` (options: Name, Size, Date Modified, Extension, Type) and direction `<button>` to default toolbar
- Added `aria-label="Search files"` to search input

**Folder tree:**
- `<ul>` — added `aria-label="Folder tree"`
- `<li>` items — added `tabindex="0"`, `aria-selected="@isActive"` (extracted `isActive` local)

**File list:**
- Added loader overlay at top: `@if (EnableLoaderContainer && _isLoading)`
- Grid items — added `role="option"`, `aria-selected="@IsSelected(entry)"`
- Table — added `role="grid"`, all `<tr>` get `role="row"`, all `<td>`/`<th>` get `role="gridcell"`

**Preview pane:**
- Added `tabindex="0"`, `aria-label="File details"`

**Context menu:**
- Added `role="menu"` to container
- Added `role="menuitem"` to all three action buttons (Rename, Download, Delete)

**Delete confirmation:**
- Added `role="alertdialog"`, `aria-modal="true"` to confirm dialog `<div>`

## Files Created

### `tests/Marilo.Tests.Unit/Forms/Inputs/FileManagerPhaseFTests.cs`

29 tests across 5 gap areas:

| Area | Tests |
|---|---|
| Sort (FM-031) | 7 tests — Name asc/desc, Size asc, DateModified asc, dirs-always-first, direction toggle state, select/button presence |
| Width (FM-008) | 3 tests — renders in style, null=no width style, Height+Width both render |
| Loader (FM-006) | 4 tests — not shown when not loading, shown when _isLoading=true, not shown when flag disabled, false after OnRead completes |
| ARIA (FM-032) | 13 tests — root role/label, toolbar role/label, search label, tree role/label, treeitem role/tabindex, grid role, row/cell roles, grid-item role, context menu role/menuitem, alertdialog |
| Class (FM-009) | 2 tests — with Class param, without Class param |

**All 29 tests pass. Total FileManager test count: 151 (Phases A–F).**

## Build Notes

- One pre-existing build warning in `MariloMultiSelect.razor` (CS8714 nullability) — not related to this change
- All Marilo.Components build errors from this phase resolved before test run
