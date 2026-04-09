# FileManager Phase C — Gap Resolutions

**Stage:** 03 — Resolutions  
**Date:** 2026-04-09  
**Author:** Claude Code  
**Status:** Resolved

---

## Summary

All 4 Phase C gaps have been resolved. The toolbar is now composable via
`ToolBarTemplate`, the path display is a clickable breadcrumb, a search field
filters items, and a view toggle switches between Grid and ListView.

---

## Resolved Gaps

### RES-FM-PC-001 — FileManagerToolBar composable toolbar (SPEC-FM-022)

| Field | Value |
|-------|-------|
| Gap ID | RES-FM-PC-001 |
| Spec ID | SPEC-FM-022 |
| Priority | P2 |
| Type | feature |
| Status | Resolved |

**Before:** Toolbar was a hardcoded `<div>` with Up + New Folder only.  
**After:** `[Parameter] public RenderFragment? ToolBarTemplate` added to
`MariloFileManager<TItem>`. When null, the default toolbar is rendered with
all built-in tools. When provided, the custom fragment replaces the entire
toolbar area. This uses the simpler RenderFragment approach (preferred per
instructions) rather than the full CascadingValue child-component system.

**Pattern used:** `ToolBarTemplate` RenderFragment parameter — same as
`HeaderTemplate`/`FooterTemplate` on `MariloDataGrid`. The full
`<FileManagerToolBar>` cascading child-component system is deferred to Phase C+.

---

### RES-FM-PC-002 — Built-in toolbar tools (SPEC-FM-023)

| Field | Value |
|-------|-------|
| Gap ID | RES-FM-PC-002 |
| Spec ID | SPEC-FM-023 |
| Priority | P2 |
| Type | feature |
| Status | Resolved (functional tools) / Planned (stub tools) |

**Functional tools implemented in default toolbar:**
- Up button — `<button @onclick="NavigateUp" disabled="@(!CanNavigateUp)">`
- New Folder button — gated by `AllowCreate`, fires `OnCreate`
- View toggle button — `.mar-filemanager__view-toggle`, calls `ToggleView()`
- Search input — `.mar-filemanager__search`, bound to `SearchFilter`

**Stub tools (Phase E):** Upload, Sort direction, Sort-by dropdown, View details
pane toggle — these depend on Phase E (upload/preview) and Phase F (sort).
They are omitted from the default toolbar to avoid non-functional UI elements.

**New internal methods added to `.razor.cs`:**
- `SetViewType(FileManagerViewType)` — sets view with event + re-render
- `ToggleView()` — toggles between Grid/ListView
- `SearchFilter` property — sets `_searchFilter` + triggers re-render

---

### RES-FM-PC-003 — Breadcrumb navigation (SPEC-FM-024)

| Field | Value |
|-------|-------|
| Gap ID | RES-FM-PC-003 |
| Spec ID | SPEC-FM-024 |
| Priority | P2 |
| Type | feature |
| Status | Resolved |

**Before:** `<span class="mar-filemanager__path">@Path</span>` — not clickable.  
**After:** `<nav class="mar-filemanager__breadcrumb">` containing clickable
`.mar-filemanager__breadcrumb-segment` spans. Each segment has a click handler
that calls `NavigateTo(segPath)`. The last segment gets the
`--active` modifier and no click handler. Separators use
`.mar-filemanager__breadcrumb-separator`.

**Helper added:** `GetBreadcrumbSegments()` yields `(Label, SegmentPath)` tuples.
Root `/` is always the first segment. Does NOT integrate `MariloBreadcrumb`
component — kept self-contained per Phase C scope constraint.

---

### RES-FM-PC-004 — Search textbox (SPEC-FM-030)

| Field | Value |
|-------|-------|
| Gap ID | RES-FM-PC-004 |
| Spec ID | SPEC-FM-030 |
| Priority | P2 |
| Type | feature |
| Status | Resolved |

**Before:** `GetCurrentItems()` returned unfiltered results.  
**After:** `_searchFilter` private field added. `SearchFilter` property setter
triggers `StateHasChanged()`. `GetCurrentItems()` applies
`StringComparison.OrdinalIgnoreCase Contains` filter when `_searchFilter` is
non-empty. Clearing the search (empty string or whitespace) restores all items.

**Note:** Filter applies to the current folder view only — it does not
cross-navigate or filter across paths. Search input bound via
`value="@SearchFilter" @oninput="..."` for immediate reactive filtering.

---

## Files Changed

| File | Change |
|------|--------|
| `src/Marilo.Components/Forms/Inputs/MariloFileManager.razor` | Toolbar replaced with breadcrumb, search, view toggle, ToolBarTemplate support |
| `src/Marilo.Components/Forms/Inputs/MariloFileManager.razor.cs` | Added `ToolBarTemplate`, `SearchFilter`, `_searchFilter`, `SetViewType`, `ToggleView`, `GetBreadcrumbSegments`, search integration in `GetCurrentItems` |
| `tests/Marilo.Tests.Unit/Forms/Inputs/FileManagerPhaseCTests.cs` | New — 25 tests for all Phase C gaps |

---

## Deferred Items

| Item | Reason | Target Phase |
|------|--------|--------------|
| `<FileManagerToolBar>` child component with CascadingValue | Not needed for foundation; ToolBarTemplate covers the use case | Phase C+ |
| `FileManagerToolBarUploadTool` | Requires Phase E upload infrastructure | Phase E |
| `FileManagerToolBarSortTool` / `SortDirectionTool` | Requires Phase F sort logic | Phase F |
| `FileManagerToolBarViewDetailsTool` | Requires Phase E preview pane | Phase E |
| `MariloBreadcrumb` integration | Scope constraint: Phase C must be self-contained | Phase D+ |
