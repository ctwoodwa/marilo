# FileManager Phase D — Implementation Log

> Stage 05 — Implementation Record
> Date: 2026-04-09

## Files Changed

### `src/Marilo.Components/Forms/Inputs/MariloFileManager.razor.cs`

**Added state fields:**
- `_contextMenuItem : TItem?` — item under context menu
- `_contextMenuVisible : bool` — context menu visibility flag
- `_contextMenuX / _contextMenuY : double` — fixed-position coordinates
- `_renamingItem : TItem?` — item currently being renamed
- `_renameText : string` (internal) — rename input value, accessible from tests
- `_deleteConfirmItem : TItem?` — item awaiting delete confirmation

**Added methods:**
- `ShowContextMenu(TItem, MouseEventArgs)` — sets item + position + visible, calls `InvokeAsync(StateHasChanged)`
- `CloseContextMenu()` — clears context menu state
- `StartRename(TItem)` — gates on `AllowRename`, closes context menu, sets rename state, fires `OnEdit`
- `CommitRename()` — writes new name via reflection (`PropertyInfo.SetValue`), fires `OnUpdate`, clears state
- `CancelRename()` — clears rename state only (no event)
- `HandleRenameKeyDown(KeyboardEventArgs)` — dispatches Enter/Escape to Commit/Cancel
- `UpdateRenameText(string)` — used from `@oninput` (avoids inline assignment lambda in Razor)
- `IsRenaming(TItem)` — `ReferenceEquals`-based identity check
- `ConfirmDelete(TItem)` — gates on `AllowDelete`, closes context menu, sets confirm state
- `ExecuteDelete()` — calls `DeleteItem()`, clears confirm state
- `CancelDelete()` — clears confirm state only (no event)
- `ContextMenuDownload()` — captures context item, closes menu, then fires `DownloadItem()`

**Notes:**
- `_renameText` is `internal` (not `private`) to allow test access via `cut.Instance._renameText = "new name"`
- All state-mutation methods end with `await InvokeAsync(StateHasChanged)` (dispatcher-safe pattern)
- Phase E stubs (`TogglePreviewPane`, `ShowUploadDialog`, `CloseUploadDialog`) were added by linter
  alongside Phase D work; they compile cleanly and do not affect Phase D tests

### `src/Marilo.Components/Forms/Inputs/MariloFileManager.razor`

**Grid view changes:**
- Added `@onclick:stopPropagation="true"` on grid item div (prevents click from bubbling to root close-menu handler)
- Added `@oncontextmenu="e => ShowContextMenu(entry, e)" @oncontextmenu:preventDefault="true"`
- Replaced unconditional `<span class="mar-filemanager__name">` with conditional: if `IsRenaming(entry)` render `<input class="mar-filemanager__rename-input">` with `@onblur="CommitRename"` and `@onkeydown="HandleRenameKeyDown"`

**List view changes:**
- Same `@onclick:stopPropagation` and `@oncontextmenu` additions on `<tr>`
- Same rename input conditional inside the name `<td>`

**Overlay additions:**
- Context menu: `<div class="mar-filemanager__context-menu">` with Rename/Download/Delete buttons, each gated by `AllowRename`/`!GetIsDirectory()`/`AllowDelete`
- Delete confirmation: `<div class="mar-filemanager__confirm-overlay">` wrapping `<div class="mar-filemanager__confirm-dialog">` with message + OK + Cancel buttons

**Root div:**
- Added `@onclick="CloseContextMenu"` for click-outside dismiss

### `tests/Marilo.Tests.Unit/Forms/Inputs/FileManagerPhaseDTests.cs`

New test file — 24 tests covering all Phase D gaps. See closure report for test list.

## Build & Test Results

- Build: succeeded (0 errors, pre-existing warnings only)
- Phase D tests: 24/24 passed
- Phase A+B+C regression: 98/98 passed (122 total FileManager tests pass)

## Decisions

- `HandleRenameKeyDown` is a named method rather than an inline Razor lambda because
  Razor parser rejects multi-statement async lambdas with embedded quote chars in `@onkeydown`.
- `UpdateRenameText` helper avoids inline assignment lambda in `@oninput` (same parser issue).
- `ContextMenuDownload()` avoids a compound async lambda in the Download button `@onclick`.
- `IsRenaming` uses `ReferenceEquals` (not `.Equals`) to avoid false matches if two items
  share the same data values — consistent with how selection uses `List.Contains`.
