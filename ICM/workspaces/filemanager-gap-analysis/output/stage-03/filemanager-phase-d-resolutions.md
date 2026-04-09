# FileManager Phase D — Gap Resolutions

> Stage 03 — Resolution Mapping
> Date: 2026-04-09

## Summary

Phase D resolves 5 gaps covering context menu, inline rename, delete confirmation, OnCreate
toolbar wiring, and permission gating in markup.

---

## RES-FM-D-001 — Right-click context menu (SPEC-FM-025)

**Gap:** No context menu on right-click of file/folder items in either view.

**Resolution:** Added context menu state fields (`_contextMenuItem`, `_contextMenuVisible`,
`_contextMenuX`, `_contextMenuY`) and `ShowContextMenu(TItem, MouseEventArgs)` /
`CloseContextMenu()` methods to `MariloFileManager.razor.cs`. Context menu renders as a
`<div class="mar-filemanager__context-menu">` positioned via `fixed` CSS at the mouse
coordinates. Root `<div>` has `@onclick="CloseContextMenu"` to dismiss on outside click.
Items use `@onclick:stopPropagation` so menu clicks do not bubble to root. Both grid items
and list rows now have `@oncontextmenu` + `@oncontextmenu:preventDefault`.

**Status:** Resolved

---

## RES-FM-D-002 — Inline rename UI (SPEC-FM-026)

**Gap:** `AllowRename` parameter existed but was not wired to any UI.

**Resolution:** Added `_renamingItem` (TItem?) and `_renameText` (string, internal) state
fields. `StartRename(TItem)` gates on `AllowRename`, sets `_renamingItem`, calls `OnEdit`.
`CommitRename()` writes the new name back via `PropertyInfo.SetValue`, fires `OnUpdate`, clears
rename state. `CancelRename()` clears state without mutation or event. `HandleRenameKeyDown`
dispatches Enter→CommitRename and Escape→CancelRename. `IsRenaming(TItem)` uses
`ReferenceEquals` for identity check. Razor renders an `<input class="mar-filemanager__rename-input">`
in place of the name `<span>` for both grid and list views when `IsRenaming(entry)` is true.

**Status:** Resolved

---

## RES-FM-D-003 — Delete confirmation dialog (SPEC-FM-027)

**Gap:** Delete fired immediately from `DeleteItem()`; no UI confirmation step.

**Resolution:** Added `_deleteConfirmItem` (TItem?) state field. `ConfirmDelete(TItem)` gates
on `AllowDelete` and sets `_deleteConfirmItem`. `ExecuteDelete()` calls `DeleteItem()` and
clears state. `CancelDelete()` clears state without firing event. Razor renders a
`<div class="mar-filemanager__confirm-dialog">` inside a `<div class="mar-filemanager__confirm-overlay">`
when `_deleteConfirmItem is not null`. Dialog shows "Delete [name]?" with OK / Cancel buttons.

**Status:** Resolved

---

## RES-FM-D-004 — Wire OnCreate to toolbar (SPEC-FM-012)

**Gap:** Verify only — `CreateFolder()` and the New Folder button were already connected in
Phase B.

**Resolution:** Verified: `CreateFolder()` in `.razor.cs` invokes `OnModelInit` to get a new
`TItem` instance (or `default` when null), wraps it in `FileManagerCreateEventArgs<TItem>`,
and fires `OnCreate`. The New Folder toolbar button is only rendered when `AllowCreate = true`.
Two tests (`CreateFolder_Fires_OnCreate_Event` and `CreateFolder_Uses_OnModelInit_When_Provided`)
confirm correct behavior. No code changes needed.

**Status:** Verified — already complete

---

## RES-FM-D-005 — Honor AllowDelete in markup (SPEC-FM-025 cross-cut)

**Gap:** `AllowDelete` guarded `DeleteItem()` at the method level (Phase B) but the delete
context menu item was not yet gated in markup.

**Resolution:** Context menu delete button is wrapped in `@if (AllowDelete)` so it is not
rendered when the permission is false. Similarly, `AllowRename` gates the rename context menu
item. `ConfirmDelete()` also re-checks `AllowDelete` in the method body as defense-in-depth.

**Status:** Resolved
