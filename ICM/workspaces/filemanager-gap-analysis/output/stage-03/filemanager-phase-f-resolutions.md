# FileManager Phase F — Gap Resolutions

> Stage 03 · Phase F (Polish) · Date: 2026-04-09

## Summary

Phase F resolves the final 5 gaps in the MariloFileManager spec backlog, completing all Polish-priority items.

---

## SPEC-FM-031 — Sort toolbar tools (P3)

**Status:** Resolved

**Resolution:**
- Added `_sortField` (string, default "Name") and `_sortAscending` (bool, default true) internal state fields
- Added `SetSortField(string)`, `ToggleSortDirection()`, and `HandleSortFieldChange(ChangeEventArgs)` methods to `.razor.cs`
- Added `<select class="mar-filemanager__sort-select">` with options: Name, Size, Date Modified, Extension, Type
- Added `<button class="mar-filemanager__sort-dir">` for direction toggle
- Both controls render in the default toolbar (not in custom `ToolBarTemplate`)
- Updated `GetCurrentItems()` to use a switch-expression dispatch: Size, DateModified, Extension, Type, and default (Name)
- Directories always remain first regardless of sort field (all branches use `OrderByDescending(GetIsDirectory)` as the primary sort key, except Type which uses `OrderBy(GetIsDirectory)` since the intent is file-vs-folder ordering)

---

## SPEC-FM-008 — Width parameter (P3)

**Status:** Resolved

**Resolution:**
- Added `[Parameter] public string? Width { get; set; }` to `.razor.cs`
- Added `GetWidthStyle()` helper returning `"width:{Width};"` or `""` when null/empty
- Added `GetContainerStyle()` combining `GetHeightStyle() + GetWidthStyle()` (needed because `CombineStyles` only accepts one base-style argument)
- Root `<div>` now uses `style="@CombineStyles(GetContainerStyle())"` — backward compatible since `GetContainerStyle()` passes through to `Height` behavior when `Width` is null

---

## SPEC-FM-006 — EnableLoaderContainer (P3)

**Status:** Resolved

**Resolution:**
- Added `[Parameter] public bool EnableLoaderContainer { get; set; }` to `.razor.cs`
- Added `internal bool _isLoading` state field
- Updated `LoadDataAsync()` to set `_isLoading = true` (+ `InvokeAsync(StateHasChanged)`) before invoking `OnRead`, and `_isLoading = false` immediately after `await OnRead.InvokeAsync(args)`
- Added `@if (EnableLoaderContainer && _isLoading)` block rendering `<div class="mar-filemanager__loader">Loading...</div>` at the top of the file list area
- When `OnRead` is not bound, `_isLoading` stays false (sync path, no loading state needed)

---

## SPEC-FM-032 — ARIA roles and keyboard navigation (P3)

**Status:** Resolved

**Resolution (attributes added to `.razor`):**

| Element | Attribute(s) added |
|---|---|
| Root `<div>` | `role="application"`, `aria-label="File Manager"` |
| Toolbar `<div>` | `role="toolbar"`, `aria-label="File manager toolbar"` |
| Tree `<ul>` | `aria-label="Folder tree"` (already had `role="tree"`) |
| Tree `<li>` items | `tabindex="0"`, `aria-selected="@isActive"` (already had `role="treeitem"`) |
| Grid `<div>` items | `role="option"`, `aria-selected="@IsSelected(entry)"` |
| List `<table>` | `role="grid"` |
| Table `<tr>` rows (thead + tbody) | `role="row"` |
| Table `<td>/<th>` cells | `role="gridcell"` |
| Search `<input>` | `aria-label="Search files"` |
| Preview pane `<div>` | `tabindex="0"`, `aria-label="File details"` |
| Context menu `<div>` | `role="menu"` |
| Context menu `<button>` items | `role="menuitem"` |
| Delete confirm `<div>` | `role="alertdialog"`, `aria-modal="true"` |

---

## SPEC-FM-009 — Verify Class from base class (P3)

**Status:** Verified — no code change required

**Resolution:**
- Root `<div>` already uses `class="@CombineClasses("mar-filemanager")"` from Phase A
- `CombineClasses` is a `MariloComponentBase` method that appends the consumer-supplied `Class` parameter value
- Confirmed by bUnit test: `Class_Parameter_Applied_Via_CombineClasses` verifies both `mar-filemanager` and the supplied `my-custom-class` appear in `ClassList`
