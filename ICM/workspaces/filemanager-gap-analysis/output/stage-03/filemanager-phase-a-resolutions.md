# FileManager Phase A — Gap Resolutions

**Stage:** 03 — Resolutions  
**Date:** 2026-04-09  
**Author:** Claude Code  
**Status:** Resolved

---

## Summary

All 8 Phase A gaps have been resolved. The component is now generic, field-mapped,
event-driven, and split into a clean razor / code-behind structure.

---

## Resolved Gaps

### RES-FM-PA-001 — Generic `MariloFileManager<TItem>`

| Field | Value |
|-------|-------|
| Gap ID | RES-FM-PA-001 |
| Priority | P0 |
| Type | foundation |
| Status | Resolved |

**Before:** `Items: IEnumerable<FileManagerEntry>` — concrete type hardwired.  
**After:** `@typeparam TItem` in `.razor`; `Data: IEnumerable<TItem>` parameter in code-behind.
`FileManagerEntry` remains as a convenience default model.

---

### RES-FM-PA-002 — 14 Field-Binding Parameters

| Field | Value |
|-------|-------|
| Gap ID | RES-FM-PA-002 |
| Priority | P0 |
| Type | feature |
| Status | Resolved |

Added parameters with defaults:

| Parameter | Default |
|-----------|---------|
| `IdField` | `"Id"` |
| `ParentIdField` | `"ParentId"` |
| `NameField` | `"Name"` |
| `PathField` | `"Path"` |
| `ExtensionField` | `"Extension"` |
| `IsDirectoryField` | `"IsDirectory"` |
| `HasDirectoriesField` | `"HasDirectories"` |
| `SizeField` | `"Size"` |
| `DateCreatedField` | `"DateCreated"` |
| `DateCreatedUtcField` | `"DateCreatedUtc"` |
| `DateModifiedField` | `"DateModified"` |
| `DateModifiedUtcField` | `"DateModifiedUtc"` |
| `DirectoriesField` | `"Directories"` |
| `ItemsField` | `"Items"` |

`GetFieldValue<T>(TItem item, string fieldName)` uses `typeof(TItem).GetProperty(field)?.GetValue(item)`.
`PropertyInfo` results are cached in `_propCache` (`Dictionary<string, PropertyInfo?>`) to avoid
per-render reflection overhead.

---

### RES-FM-PA-003 — Parameter Renames

| Field | Value |
|-------|-------|
| Gap ID | RES-FM-PA-003 |
| Priority | P0 |
| Type | breaking-rename |
| Status | Resolved |

| Old | New |
|-----|-----|
| `CurrentPath` | `Path` |
| `CurrentPathChanged` | `PathChanged` |

---

### RES-FM-PA-004 — Enum Rename + `ViewChanged`

| Field | Value |
|-------|-------|
| Gap ID | RES-FM-PA-004 |
| Priority | P0 |
| Type | breaking-rename |
| Status | Resolved |

- `FileManagerViewMode` → `FileManagerViewType` (marked `[Obsolete]` for backward compat).
- `List` → `ListView`, `Grid` stays `Grid`.
- `ViewMode` parameter → `View`.
- `ViewChanged: EventCallback<FileManagerViewType>` added for two-way binding.

---

### RES-FM-PA-005 — `OnRead` Dual-Mode Loading

| Field | Value |
|-------|-------|
| Gap ID | RES-FM-PA-005 |
| Priority | P0 |
| Type | feature |
| Status | Resolved |

- `OnRead: EventCallback<FileManagerReadEventArgs>` added.
- When `OnRead.HasDelegate`, `LoadDataAsync()` invokes it and consumes `args.Data`.
- `Data` parameter is the local-data fallback when `OnRead` is not bound.
- `_readCts` cancels in-flight reads when a newer one starts (same pattern as `MariloMultiSelect`).
- Fires on `OnInitializedAsync` and on every `NavigateTo`.
- `Rebind()` public method available for external refresh.

---

### RES-FM-PA-006 — `Height` Parameter

| Field | Value |
|-------|-------|
| Gap ID | RES-FM-PA-006 |
| Priority | P1 |
| Type | feature |
| Status | Resolved |

`[Parameter] public string? Height` — wired via `CombineStyles(GetHeightStyle())` on the root `div`.
`GetHeightStyle()` returns `"height:{Height};"` or `""` when null.

---

### RES-FM-PA-007 — Extract to Partial Files

| Field | Value |
|-------|-------|
| Gap ID | RES-FM-PA-007 |
| Priority | P1 |
| Type | structure |
| Status | Resolved |

- `MariloFileManager.razor` — markup only (no `@code` block).
- `MariloFileManager.razor.cs` — partial class with all parameters, state, and logic.

---

### RES-FM-PA-008 — EventArgs Types

| Field | Value |
|-------|-------|
| Gap ID | RES-FM-PA-008 |
| Priority | P0 |
| Type | feature |
| Status | Resolved |

Added to `Marilo.Core.Models.FileManagerModels.cs`:

| Type | Phase |
|------|-------|
| `FileManagerReadEventArgs` | A — active |
| `FileManagerCreateEventArgs<TItem>` | A — active |
| `FileManagerDeleteEventArgs<TItem>` | A — active |
| `FileManagerEditEventArgs<TItem>` | B — stub |
| `FileManagerUpdateEventArgs<TItem>` | B — stub |
| `FileManagerDownloadEventArgs<TItem>` | B — stub |

- `OnCreateFolder: EventCallback<string>` → `OnCreate: EventCallback<FileManagerCreateEventArgs<TItem>>`
- `OnDelete: EventCallback<FileManagerEntry>` → `OnDelete: EventCallback<FileManagerDeleteEventArgs<TItem>>`
