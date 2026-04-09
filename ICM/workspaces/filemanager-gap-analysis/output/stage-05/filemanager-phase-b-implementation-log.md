# FileManager Phase B — Implementation Log

**Date:** 2026-04-09
**Implementer:** Claude (claude-sonnet-4-6)

---

## Files Changed

### `src/Marilo.Components/Forms/Inputs/MariloFileManager.razor.cs`

**Internal state changes:**
- Removed `private string? _selectedItemPath`
- Added `private List<TItem> _selectedItems = new()`

**New parameters:**
- `SelectedItems : IEnumerable<TItem>` — two-way bindable selection
- `SelectedItemsChanged : EventCallback<IEnumerable<TItem>>`
- `OnEdit : EventCallback<FileManagerEditEventArgs<TItem>>`
- `OnUpdate : EventCallback<FileManagerUpdateEventArgs<TItem>>`
- `OnDownload : EventCallback<FileManagerDownloadEventArgs<TItem>>`
- `OnModelInit : Func<TItem>?`

**Modified methods:**
- `OnParametersSetAsync` — added `_selectedItems = SelectedItems.ToList()` sync
- `SelectItem(TItem)` — replaced `_selectedItemPath = GetPath(item)` with `_selectedItems = new List<TItem> { item }; await SelectedItemsChanged.InvokeAsync(...)` 
- `CreateFolder()` — uses `OnModelInit()` factory when bound; falls back to `default`
- `DeleteItem(TItem)` — added `if (!AllowDelete) return;` guard before firing `OnDelete`
- `IsSelected(TItem)` — changed from path comparison to `_selectedItems.Contains(item)`

**New methods:**
- `EditItem(TItem)` — fires `OnEdit` with `FileManagerEditEventArgs<TItem>`
- `UpdateItem(TItem)` — fires `OnUpdate` with `FileManagerUpdateEventArgs<TItem>`
- `DownloadItem(TItem)` — fires `OnDownload` with `FileManagerDownloadEventArgs<TItem>`; returns args (callers check `IsCancelled`)

**Pre-existing methods preserved unchanged:**
- `SetViewType`, `ToggleView`, `GetBreadcrumbSegments`, `SearchFilter`, `ToolBarTemplate`, `Rebind`, `LoadDataAsync`

### `src/Marilo.Components/Forms/Inputs/MariloFileManager.razor`

No changes required. Markup already uses `IsSelected(entry)` for CSS class conditionals in both ListView and Grid views. The updated `IsSelected` implementation (reference-based) is transparently compatible.

---

## Tests Created

**File:** `tests/Marilo.Tests.Unit/Forms/Inputs/FileManagerPhaseBTests.cs`

**Test count:** 23 tests

| Category | Tests |
|----------|-------|
| OnEdit | Fires with correct item; does not throw when no delegate |
| OnUpdate | Fires with correct item; does not throw when no delegate |
| OnDownload | Fires with correct item; IsCancelled settable by handler; IsCancelled false by default |
| OnModelInit | Used in CreateFolder; null falls back to default |
| SelectedItems binding | Parameter syncs to internal state; empty means nothing selected |
| SelectedItemsChanged | Fires on SelectItem; includes the selected item |
| SelectItem | Updates IsSelected to true; fires both OnSelect and SelectedItemsChanged |
| AllowDelete | Fires OnDelete when true; suppresses when false |
| Rebind | Triggers OnRead when bound; no throw when not bound |
| ViewChanged | Fires via SetViewType; ToggleView switches ListView↔Grid |
| CSS markup | Selected class appears in ListView after selection; in Grid after selection |

---

## Build Status

- `Marilo.Components` project: **0 errors, 0 warnings**
- Phase A tests (26): **all pass**
- Phase B tests (23): **all pass**
- Combined (49 FileManager tests): **all pass**
