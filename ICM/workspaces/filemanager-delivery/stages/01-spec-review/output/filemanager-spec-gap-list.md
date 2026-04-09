# MariloFileManager — Stage 01 Spec Review: Gap List

**Audit date:** 2026-04-09
**Source file:** `src/Marilo.Components/Forms/Inputs/MariloFileManager.razor`
**Source parameter count:** 10 `[Parameter]` properties + 1 two-way binding pair (see breakdown below)
**Spec parameter count:** 40+ documented parameters, events, and field-binding strings across all spec files
**Total gaps:** 36

| Gap type | Count |
|----------|-------|
| Undocumented (in source, not in spec) | 4 |
| Spec-ahead (in spec, not in source) | 28 |
| Mismatch (both exist, differ in name/type/behavior) | 4 |

---

## Source Inventory

### Parameters extracted from `MariloFileManager.razor`

| # | Name | Type | Default |
|---|------|------|---------|
| 1 | `Items` | `IEnumerable<FileManagerEntry>` | `Enumerable.Empty<FileManagerEntry>()` |
| 2 | `CurrentPath` | `string` | `"/"` |
| 3 | `CurrentPathChanged` | `EventCallback<string>` | — |
| 4 | `ViewMode` | `FileManagerViewMode` (enum) | `FileManagerViewMode.List` |
| 5 | `ShowFolderTree` | `bool` | `true` |
| 6 | `AllowCreate` | `bool` | `false` |
| 7 | `AllowDelete` | `bool` | `false` |
| 8 | `AllowRename` | `bool` | `false` |
| 9 | `OnSelect` | `EventCallback<FileManagerEntry>` | — |
| 10 | `OnOpen` | `EventCallback<FileManagerEntry>` | — |
| 11 | `OnCreateFolder` | `EventCallback<string>` | — |
| 12 | `OnDelete` | `EventCallback<FileManagerEntry>` | — |

**Private state (not parameters):** `SelectedItemPath`, `CanNavigateUp`

---

## Gap Records

---

### A. Undocumented (in source, missing from spec)

---

**ID:** SPEC-filemanager-001
**Type:** undocumented
**Parameter/Event:** `Items`
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Data` | `Items` |
| Type | `IEnumerable<TItem>` (generic) | `IEnumerable<FileManagerEntry>` (concrete class) |
| Default | missing | `Enumerable.Empty<FileManagerEntry>()` |
| Description | Data source bound to component | Data source bound to component |

**Note:** This is both an undocumented name AND a mismatch on type. The spec consistently uses `Data` (generic `TItem`); source uses `Items` (concrete `FileManagerEntry`). Recorded here as undocumented because `Items` has no spec entry at all; the canonical spec parameter is `Data` (see SPEC-filemanager-025 mismatch section).

**Recommended action:** Rename source parameter from `Items` to `Data`; make component generic `<TItem>` with field-binding strings.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-002
**Type:** undocumented
**Parameter/Event:** `ShowFolderTree`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `ShowFolderTree` |
| Type | missing | `bool` |
| Default | missing | `true` |
| Description | missing | Controls visibility of the left folder-tree sidebar |

**Recommended action:** Add `ShowFolderTree` to spec (overview parameters table) with `bool`, default `true`.
**Delegated to:** spec update only

---

**ID:** SPEC-filemanager-003
**Type:** undocumented
**Parameter/Event:** `AllowCreate`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `AllowCreate` |
| Type | missing | `bool` |
| Default | missing | `false` |
| Description | missing | Shows/hides the "New Folder" button in the toolbar |

**Note:** The spec references `OnCreate` event and `FileManagerToolBarNewFolderTool` but has no parameter to gate the button visibility.

**Recommended action:** Add `AllowCreate` to spec parameters table; cross-reference toolbar.md.
**Delegated to:** spec update only

---

**ID:** SPEC-filemanager-004
**Type:** undocumented
**Parameter/Event:** `OnOpen`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `OnOpen` |
| Type | missing | `EventCallback<FileManagerEntry>` |
| Default | missing | — |
| Description | missing | Fires when a file (non-directory) is double-clicked |

**Note:** The spec's `OnRead` and `OnDownload` cover retrieval; there is no spec event for the raw double-click/open action on a file item. The source `OnOpen` fires from `OpenItem()` when `!entry.IsDirectory`.

**Recommended action:** Add `OnOpen` event to events.md with `FileManagerEntry` arg; clarify relationship to `OnDownload`.
**Delegated to:** spec update only

---

### B. Spec-Ahead (in spec, not implemented in source)

---

**ID:** SPEC-filemanager-005
**Type:** spec-ahead
**Parameter/Event:** `Data` (generic `TItem`)
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Data` | missing (source uses `Items`) |
| Type | `IEnumerable<TItem>` | missing |
| Default | missing | missing |
| Description | Generic data source; component is generic `MariloFileManager<TItem>` | missing |

**Recommended action:** Implement generic `TItem` parameter; replace `Items` with `Data`.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-006
**Type:** spec-ahead
**Parameter/Event:** `EnableLoaderContainer`
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `EnableLoaderContainer` | missing |
| Type | `bool` | missing |
| Default | missing | missing |
| Description | Shows a loading overlay on slow async operations | missing |

**Recommended action:** Implement loader overlay on async data loads.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-007
**Type:** spec-ahead
**Parameter/Event:** `Height`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Height` | missing |
| Type | `string` | missing |
| Default | missing | missing |
| Description | Height of the component (CSS value) | missing |

**Recommended action:** Add `Height` parameter wired to inline style.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-008
**Type:** spec-ahead
**Parameter/Event:** `Width`
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Width` | missing |
| Type | `string` | missing |
| Default | missing | missing |
| Description | Width of the component (CSS value) | missing |

**Recommended action:** Add `Width` parameter wired to inline style.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-009
**Type:** spec-ahead
**Parameter/Event:** `Class`
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Class` | missing |
| Type | `string` | missing |
| Default | missing | missing |
| Description | Additional CSS class on the root element | missing |

**Note:** Source uses `CombineClasses("mar-filemanager")` from `MariloComponentBase`, which likely already handles `Class` via base class — needs verification. Logged as spec-ahead until confirmed.

**Recommended action:** Verify base class provides `Class`; if so, update spec to note it is inherited.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-010
**Type:** spec-ahead
**Parameter/Event:** Field-binding strings (13 parameters)
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `NameField`, `SizeField`, `PathField`, `ExtensionField`, `IsDirectoryField`, `DateCreatedField`, `DateCreatedUtcField`, `DateModifiedField`, `DateModifiedUtcField`, `IdField`, `ParentIdField`, `HasDirectoriesField`, `DirectoriesField`, `ItemsField` | missing (all 14) |
| Type | `string` (each) | missing |
| Default | `"Name"`, `"Size"`, `"Path"`, `"Extension"`, `"IsDirectory"`, `"DateCreated"`, `"DateCreatedUtc"`, `"DateModified"`, `"DateModifiedUtc"`, `"Id"`, `"ParentId"`, `"HasDirectories"`, `"Directories"`, `"Items"` | missing |
| Description | Reflection-based field accessor strings enabling TItem model property mapping | missing |

**Note:** The source component uses the concrete `FileManagerEntry` model with hardcoded property names. The spec requires a generic component that resolves fields by name string to support arbitrary model types. This is the core architectural gap that blocks data-binding parity with the spec.

**Recommended action:** Implement all 14 field-binding string parameters alongside the `TItem` generic refactor (SPEC-filemanager-005).
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-011
**Type:** spec-ahead
**Parameter/Event:** `OnRead`
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnRead` | missing |
| Type | `EventCallback<FileManagerReadEventArgs>` | missing |
| Default | — | missing |
| Description | Alternative data provision via event; fires on init and path change; enables on-demand loading | missing |

**Recommended action:** Implement `OnRead` with `FileManagerReadEventArgs` (Data, Path, CancellationToken) and lazy loading path.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-012
**Type:** spec-ahead
**Parameter/Event:** `OnCreate`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnCreate` | `OnCreateFolder` (see mismatch SPEC-filemanager-033) |
| Type | `EventCallback<FileManagerCreateEventArgs>` | `EventCallback<string>` |
| Default | — | — |
| Description | Fires when a new folder is created; provides `FileManagerCreateEventArgs` with `Item` | missing |

**Recommended action:** See mismatch SPEC-filemanager-033 — rename `OnCreateFolder` to `OnCreate` and change arg type.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-013
**Type:** spec-ahead
**Parameter/Event:** `OnEdit`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnEdit` | missing |
| Type | `EventCallback<FileManagerEditEventArgs>` | missing |
| Default | — | missing |
| Description | Fires when the user begins renaming an item (before `OnUpdate`) | missing |

**Recommended action:** Implement inline rename with `OnEdit` event as the entry point.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-014
**Type:** spec-ahead
**Parameter/Event:** `OnUpdate`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnUpdate` | missing |
| Type | `EventCallback<FileManagerUpdateEventArgs>` | missing |
| Default | — | missing |
| Description | Fires when a rename operation completes; provides `FileManagerUpdateEventArgs` with updated `Item` | missing |

**Recommended action:** Implement as the completion event for the inline rename flow.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-015
**Type:** spec-ahead
**Parameter/Event:** `OnDownload`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnDownload` | missing |
| Type | `EventCallback<FileManagerDownloadEventArgs>` | missing |
| Default | — | missing |
| Description | Fires before a file download; consumer must set `args.Stream`, `args.MimeType`; cancellable | missing |

**Recommended action:** Implement download via context menu; fire `OnDownload` with `FileManagerDownloadEventArgs` (Stream, MimeType, FileName, Item).
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-016
**Type:** spec-ahead
**Parameter/Event:** `OnModelInit`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnModelInit` | missing |
| Type | `Func<TItem>` | missing |
| Default | — | missing |
| Description | Invoked when a new model instance is needed for folder creation; consumer provides the initial item | missing |

**Recommended action:** Implement `OnModelInit` callback; required for generic `TItem` new-folder creation.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-017
**Type:** spec-ahead
**Parameter/Event:** `SelectedItems`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `SelectedItems` | missing (source uses private `SelectedItemPath` string) |
| Type | `IEnumerable<TItem>` | missing |
| Default | missing | missing |
| Description | The currently selected files/folders; supports two-way binding | missing |

**Recommended action:** Replace private `SelectedItemPath` string with public `SelectedItems` parameter supporting multi-select.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-018
**Type:** spec-ahead
**Parameter/Event:** `SelectedItemsChanged`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `SelectedItemsChanged` | missing |
| Type | `EventCallback<IEnumerable<TItem>>` | missing |
| Default | — | missing |
| Description | Fires whenever the user changes selection in the main pane | missing |

**Recommended action:** Implement as counterpart to `SelectedItems` two-way binding.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-019
**Type:** spec-ahead
**Parameter/Event:** `ViewChanged`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `ViewChanged` | missing |
| Type | `EventCallback<FileManagerViewType>` | missing |
| Default | — | missing |
| Description | Fires when user toggles between Grid and ListView; consumer must update `View` | missing |

**Recommended action:** Implement alongside toolbar view-toggle buttons.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-020
**Type:** spec-ahead
**Parameter/Event:** `PathChanged`
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `PathChanged` | `CurrentPathChanged` (mismatch — see SPEC-filemanager-034) |
| Type | `EventCallback<string>` | `EventCallback<string>` |
| Default | — | — |
| Description | Fires when the user navigates to a different folder | spec = `PathChanged` |

**Note:** See mismatch record SPEC-filemanager-034. Logged here as spec-ahead because the canonical spec name `PathChanged` is absent.

**Recommended action:** Rename `CurrentPathChanged` to `PathChanged`.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-021
**Type:** spec-ahead
**Parameter/Event:** `Rebind()` method
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Rebind` | missing |
| Type | `void` public method | missing |
| Default | N/A | missing |
| Description | Programmatically refreshes FileManager data; requires `@ref` on component | missing |

**Recommended action:** Implement `Rebind()` public method (triggers `OnRead` or re-renders with current `Data`).
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-022
**Type:** spec-ahead
**Parameter/Event:** Toolbar child component (`<FileManagerToolBar>`)
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `FileManagerToolBar` | missing |
| Type | child component / `RenderFragment` | missing |
| Default | default built-in toolbar | missing |
| Description | Optional child tag to customize toolbar tool order and add custom tools | missing |

**Note:** Source renders a hard-coded toolbar with only "Up", path display, and optional "New Folder" button. The spec defines a rich toolbar with 7 built-in tools.

**Recommended action:** Implement `<FileManagerToolBar>` child component pattern with all built-in tool tags.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-023
**Type:** spec-ahead
**Parameter/Event:** Built-in toolbar tools (7 tools)
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `FileManagerToolBarNewFolderTool`, `FileManagerToolBarUploadTool`, `FileManagerToolBarSortDirectionTool`, `FileManagerToolBarSortTool`, `FileManagerToolBarFileViewTool`, `FileManagerToolBarViewDetailsTool`, `FileManagerToolBarSearchTool` | missing |
| Type | child component tags | missing |
| Default | all shown by default | missing |
| Description | Composable toolbar tools for new folder, upload, sort direction, sort-by, view mode, preview pane toggle, and search | missing |

**Recommended action:** Implement each tool tag; wire to respective behaviors.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-024
**Type:** spec-ahead
**Parameter/Event:** Breadcrumb navigation
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Breadcrumb | missing |
| Type | embedded `MariloBreadcrumb` | missing |
| Default | shown by default | missing |
| Description | Shows current folder path; each segment is clickable to navigate up | missing |

**Note:** Source shows path as a plain `<span class="mar-filemanager__path">@CurrentPath</span>` with no interaction.

**Recommended action:** Replace plain path span with embedded `MariloBreadcrumb` component.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-025
**Type:** spec-ahead
**Parameter/Event:** Context menu (Rename, Download, Delete)
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Context menu (right-click) | missing |
| Type | embedded `MariloContextMenu` | missing |
| Default | shown on right-click | missing |
| Description | Right-click context menu with Rename, Download, and Delete commands; Delete shows confirmation dialog | missing |

**Recommended action:** Implement right-click context menu using `MariloContextMenu`; wire Rename → `OnEdit`/`OnUpdate`; Download → `OnDownload`; Delete → confirmation dialog → `OnDelete`.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-026
**Type:** spec-ahead
**Parameter/Event:** Inline rename UI
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Inline rename input | missing |
| Type | behavior / UI pattern | missing |
| Default | triggered via context menu Rename | missing |
| Description | Renders an input over the item name; Enter or blur commits; fires `OnUpdate` | missing |

**Recommended action:** Implement inline-rename UI state management and `OnEdit`/`OnUpdate` event flow.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-027
**Type:** spec-ahead
**Parameter/Event:** Delete confirmation dialog
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Delete confirmation dialog | missing |
| Type | behavior / UI pattern | missing |
| Default | shown before `OnDelete` fires | missing |
| Description | Opens dialog after Delete context-menu command; OK fires `OnDelete`; Cancel closes without action | missing |

**Recommended action:** Add delete confirmation dialog (use `MariloDialog` or built-in confirm pattern).
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-028
**Type:** spec-ahead
**Parameter/Event:** Upload integration (`<FileManagerSettings>` / `<FileManagerUploadSettings>`)
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `FileManagerSettings`, `FileManagerUploadSettings` | missing |
| Type | child components | missing |
| Default | no upload if not configured | missing |
| Description | Configures the embedded `MariloUpload` shown in a dialog via the Upload toolbar tool | missing |

**Recommended action:** Implement `<FileManagerSettings>` / `<FileManagerUploadSettings>` child tag pattern; wire to Upload toolbar tool.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-029
**Type:** spec-ahead
**Parameter/Event:** Preview pane
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Preview pane | missing |
| Type | right-pane panel, toggled via toolbar Switch | missing |
| Default | hidden by default | missing |
| Description | Shows thumbnail, file type, size, date created, date modified for selected item; toggled by `FileManagerToolBarViewDetailsTool` | missing |

**Recommended action:** Implement `.mar-filemanager-preview` right pane with item detail display; wire to toolbar switch.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-030
**Type:** spec-ahead
**Parameter/Event:** Search (toolbar textbox)
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Search | missing |
| Type | embedded textbox, `FileManagerToolBarSearchTool` | missing |
| Default | filters current folder items by name | missing |
| Description | Textbox in toolbar that filters files by name in the current folder | missing |

**Recommended action:** Implement search filter logic and `FileManagerToolBarSearchTool` wired to it.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-031
**Type:** spec-ahead
**Parameter/Event:** Sort (toolbar sort-by + direction)
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Sort | missing |
| Type | toolbar tools: `FileManagerToolBarSortTool`, `FileManagerToolBarSortDirectionTool` | missing |
| Default | not sorted / or by name ascending | missing |
| Description | Sorts current folder items by Name, Type, Size, Date Created, or Date Modified; Ascending or Descending | missing |

**Note:** Source sorts by `IsDirectory desc, Name asc` hardcoded in `GetCurrentItems()` — no user-facing sort control.

**Recommended action:** Expose sort parameters and implement toolbar sort tools.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-032
**Type:** spec-ahead
**Parameter/Event:** ARIA roles and keyboard navigation
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | WAI-ARIA compliance | partial / incomplete |
| Type | markup attributes | missing |
| Default | WCAG 2.2 AA | missing |
| Description | Spec requires: `role="tree"` on TreeView (present), `tabindex=0` on preview pane, full keyboard nav via embedded ToolBar/Splitter/TreeView/Breadcrumb/ListView/Grid ARIA specs | missing |

**Note:** Source has `role="tree"` on the sidebar `<ul>` but lacks: `role="treeitem"` keyboard nav, `tabindex`, focusable preview pane, Splitter ARIA, Breadcrumb ARIA, and all associated keyboard patterns. The embedded composite component ARIA (Toolbar, ListView, Grid) is entirely absent since those components are not yet integrated.

**Recommended action:** After composite component integration (toolbar, breadcrumb, context menu, preview pane), audit ARIA completeness against wai-aria-support.md spec.
**Delegated to:** gap-analysis-resolution intake

---

### C. Mismatch (both exist, differ in name / type / behavior)

---

**ID:** SPEC-filemanager-033
**Type:** mismatch
**Parameter/Event:** `OnCreate` / `OnCreateFolder`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnCreate` | `OnCreateFolder` |
| Type | `EventCallback<FileManagerCreateEventArgs>` | `EventCallback<string>` |
| Default | — | — |
| Description | Fires when a new folder is created | Fires when "New Folder" is clicked; provides current path string |

**Recommended action:** Rename source `OnCreateFolder` to `OnCreate`; change arg type from `string` to `FileManagerCreateEventArgs` (which carries the new `TItem` model instance). The path is derivable from the event args via the item's properties.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-034
**Type:** mismatch
**Parameter/Event:** `Path` / `CurrentPath`
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Path` | `CurrentPath` |
| Type | `string` | `string` |
| Default | (root path string) | `"/"` |
| Description | Current folder path; two-way bindable (`@bind-Path`) | Two-way bindable via `CurrentPath` + `CurrentPathChanged` |

**Note:** The spec uses `@bind-Path` throughout all documentation examples. Source exposes `CurrentPath` / `CurrentPathChanged`. This naming mismatch means all spec examples will not compile against source without parameter renaming.

**Recommended action:** Rename `CurrentPath` → `Path` and `CurrentPathChanged` → `PathChanged` in source.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-035
**Type:** mismatch
**Parameter/Event:** `View` / `ViewMode`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `View` | `ViewMode` |
| Type | `FileManagerViewType` (enum) | `FileManagerViewMode` (enum) |
| Default | `ListView` | `FileManagerViewMode.List` |
| Description | Controls the file content visualization (Grid or ListView) | Controls Grid vs List display |

**Note:** Two mismatches: (1) parameter name `View` vs `ViewMode`; (2) enum type name `FileManagerViewType` vs `FileManagerViewMode`; (3) the spec default view is `ListView` (thumbnail tiles), but source default is `List` (table view). The spec's "Grid" view is a table view; the spec's "ListView" is thumbnails — the source has the semantics inverted relative to spec naming.

**Recommended action:** Rename `ViewMode` → `View`; rename enum to `FileManagerViewType`; align enum values with spec (`ListView` for thumbnails, `Grid` for table). Update `@bind-View` two-way binding.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-filemanager-036
**Type:** mismatch
**Parameter/Event:** `OnDelete`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnDelete` | `OnDelete` |
| Type | `EventCallback<FileManagerDeleteEventArgs>` | `EventCallback<FileManagerEntry>` |
| Default | — | — |
| Description | Fires when item(s) are deleted after confirmation | Fires on delete; no confirmation dialog; passes entry directly |

**Note:** The parameter name matches, but the arg type is different. The spec uses `FileManagerDeleteEventArgs` (with an `Item` property of type `TItem`). Source passes `FileManagerEntry` directly. Additionally, source has no delete confirmation dialog (spec-required) and does not gate deletion on `AllowDelete` — the `AllowDelete` bool parameter is declared but unused in markup.

**Recommended action:** Change `OnDelete` arg from `FileManagerEntry` to `FileManagerDeleteEventArgs`; implement confirmation dialog; honor `AllowDelete` in markup.
**Delegated to:** gap-analysis-resolution intake

---

## Priority Summary

| Priority | Gap IDs | Count |
|----------|---------|-------|
| P1 — blocking | 001, 005, 010, 011, 020 | 5 |
| P2 — this phase | 002, 003, 004, 007, 012, 013, 014, 015, 016, 017, 018, 019, 021, 022, 023, 024, 025, 026, 027, 028, 029, 030, 033, 034, 035, 036 | 26 |
| P3 — next phase | 006, 008, 009, 031, 032 | 5 |

---

## Recommended Resolution Order

1. **P1 — Architecture** (SPEC-filemanager-001/005/010): Make component generic `<TItem>`, rename `Items` → `Data`, implement all field-binding string parameters. This unblocks all field-resolution and data-binding work.
2. **P1 — Naming** (SPEC-filemanager-034, 020): Rename `CurrentPath` → `Path` and `CurrentPathChanged` → `PathChanged` so spec examples compile.
3. **P1 — OnRead** (SPEC-filemanager-011): Implement the alternate data-provision event for on-demand loading.
4. **P2 — Events** (SPEC-filemanager-012–019, 033, 036): Align event names and arg types; implement `OnEdit`, `OnUpdate`, `OnDownload`, `OnModelInit`, `OnCreate`, `SelectedItems`, `SelectedItemsChanged`, `ViewChanged`.
5. **P2 — UI Surface** (SPEC-filemanager-022–030, 035): Implement toolbar composite, breadcrumb, context menu, inline rename, delete dialog, upload integration, preview pane, search.
6. **P3 — Polish** (SPEC-filemanager-006, 008, 009, 031, 032): Loader container, Width/Class, sort UI, ARIA keyboard nav.
