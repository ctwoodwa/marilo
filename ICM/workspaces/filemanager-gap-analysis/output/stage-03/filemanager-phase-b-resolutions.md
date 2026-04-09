# FileManager Phase B — Gap Resolutions

**Date:** 2026-04-09
**Phase:** B — Events & Data
**Status:** RESOLVED

---

## Gaps Resolved

| Gap ID | Description | Resolution |
|--------|-------------|-----------|
| FM-B-01 | OnCreate already renamed from OnCreateFolder (Phase A) — verify fires `FileManagerCreateEventArgs<TItem>` | Verified. `CreateFolder()` now uses `OnModelInit` factory when bound. |
| FM-B-02 | OnDelete already using `FileManagerDeleteEventArgs<TItem>` (Phase A) — verify `AllowDelete` is honored | Fixed: `DeleteItem()` now guards with `if (!AllowDelete) return;` before firing `OnDelete`. |
| FM-B-03 | OnEdit — new event parameter | Added `[Parameter] EventCallback<FileManagerEditEventArgs<TItem>> OnEdit` + `EditItem(TItem)` invoke method. |
| FM-B-04 | OnUpdate — new event parameter | Added `[Parameter] EventCallback<FileManagerUpdateEventArgs<TItem>> OnUpdate` + `UpdateItem(TItem)` invoke method. |
| FM-B-05 | OnDownload — new cancellable event parameter | Added `[Parameter] EventCallback<FileManagerDownloadEventArgs<TItem>> OnDownload` + `DownloadItem(TItem)` returns args so callers can inspect `IsCancelled`. |
| FM-B-06 | OnModelInit — factory callback for new item instances | Added `[Parameter] Func<TItem>? OnModelInit`. `CreateFolder()` calls it when bound; falls back to `default`. |
| FM-B-07 | SelectedItems — replace `_selectedItemPath` string with typed list | Replaced `string? _selectedItemPath` with `List<TItem> _selectedItems`. Added `[Parameter] IEnumerable<TItem> SelectedItems` + sync in `OnParametersSetAsync`. |
| FM-B-08 | SelectedItemsChanged — two-way binding counterpart | Added `[Parameter] EventCallback<IEnumerable<TItem>> SelectedItemsChanged`. Fired from `SelectItem()`. |
| FM-B-09 | ViewChanged — verify two-way binding plumbing | Verified present from Phase A. `SetViewType()` and `ToggleView()` fire `ViewChanged`. |
| FM-B-10 | Rebind() — verify triggers OnRead | Verified. `Rebind()` delegates to `LoadDataAsync()` which invokes `OnRead` when bound. |

---

## EventArgs Types Used

All EventArgs types already existed as stubs in `Marilo.Core.Models.FileManagerModels`. No new types created.

- `FileManagerEditEventArgs<TItem>` — `Item` property
- `FileManagerUpdateEventArgs<TItem>` — `Item` property
- `FileManagerDownloadEventArgs<TItem>` — `Item`, `Stream`, `MimeType`, `FileName`, `IsCancelled` (settable)

---

## Selection Model Change

Old: `private string? _selectedItemPath` — path-string comparison, breaks with custom PathField values that collide.

New: `private List<TItem> _selectedItems` — reference equality via `List.Contains()`. Synced from `SelectedItems` parameter in `OnParametersSetAsync`. `SelectItem()` sets single-item list, fires `SelectedItemsChanged`, then fires `OnSelect`.

`IsSelected(TItem item)` now checks `_selectedItems.Contains(item)` (reference equality), which is consistent with how bUnit and typical Blazor consumers pass item references.
