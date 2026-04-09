# FileManager Phase A — Implementation Log

**Stage:** 05 — Implementation  
**Date:** 2026-04-09  
**Author:** Claude Code  
**Build:** Passed (0 errors, 1 pre-existing warning in MariloMultiSelect)  
**Tests:** 26 / 26 passed

---

## Files Changed

| File | Action | Notes |
|------|--------|-------|
| `src/Marilo.Core/Models/FileManagerModels.cs` | Modified | Added `FileManagerViewType`, 6 EventArgs types, expanded `FileManagerEntry`, marked `FileManagerViewMode` obsolete |
| `src/Marilo.Components/Forms/Inputs/MariloFileManager.razor` | Modified | Now markup-only; added `@typeparam TItem`; updated to use field-accessor methods |
| `src/Marilo.Components/Forms/Inputs/MariloFileManager.razor.cs` | Created | Partial class — all parameters, field resolution, OnRead dual-mode, lifecycle |
| `tests/Marilo.Tests.Unit/Forms/Inputs/FileManagerPhaseATests.cs` | Created | 26 bUnit tests |

---

## Implementation Notes

### Generic conversion
The `@typeparam TItem` directive replaces the concrete `Items: IEnumerable<FileManagerEntry>` binding.
The `.razor.cs` partial class is `MariloFileManager<TItem> : MariloComponentBase`.
No `@using` directives were needed in `.razor` since `_Imports.razor` already includes `Marilo.Core.Models`.

### Field resolution
`GetFieldValue<T>(TItem item, string fieldName)` uses a `Dictionary<string, PropertyInfo?>` cache
initialized lazily on first access per field name. This avoids per-render Type.GetProperty calls.
Missing properties silently return `default(T)` — no exceptions thrown.

### Disposal pattern
`MariloComponentBase.Dispose()` is non-virtual but provides `protected virtual Dispose(bool disposing)`.
The component overrides `Dispose(bool disposing)` to cancel `_readCts`. Direct `override void Dispose()`
would have caused `CS0506` — this was caught on first build and corrected.

### OnRead dual-mode
When `OnRead.HasDelegate`:
1. A new `CancellationTokenSource` is created (previous is cancelled first).
2. `FileManagerReadEventArgs` is constructed with `Path` and the token.
3. Handler populates `args.Data`.
4. Items are cast to `IEnumerable<TItem>` via `OfType<TItem>()`.
5. `StateHasChanged` is dispatched via `InvokeAsync`.

When `OnRead` is not bound, `_resolvedItems` is synced from the `Data` parameter in `OnParametersSetAsync`.

### Height wiring
`CombineStyles(string? baseStyle)` on `MariloComponentBase` takes an optional base style.
`GetHeightStyle()` returns `"height:{Height};"` (non-null, non-empty `Height`) or `""`.

### FormatSize visibility
`FormatSize` was changed from `private static` to `internal static` to allow the bUnit test
to call `MariloFileManager<FileManagerEntry>.FormatSize(bytes)` directly.

---

## Test Coverage

| Test | Scenario | Result |
|------|----------|--------|
| `Renders_WithFileManagerEntry_AsGenericTItem` | Generic TItem renders | Pass |
| `Data_Parameter_Binds_Items_In_ListView` | Data shows rows | Pass |
| `Data_Parameter_Empty_Shows_No_Rows` | Empty data | Pass |
| `Path_Parameter_Displays_In_Toolbar` | Path displayed | Pass |
| `Path_TwoWay_Binding_Fires_PathChanged_On_Navigate` | PathChanged fires | Pass |
| `CanNavigateUp_Is_False_At_Root` | Root path | Pass |
| `CanNavigateUp_Is_True_Below_Root` | Sub-path | Pass |
| `View_Defaults_To_ListView` | Default view | Pass |
| `View_Grid_Renders_Grid_Container` | Grid view | Pass |
| `View_ListView_Renders_Table` | ListView view | Pass |
| `View_Grid_Shows_Items_As_Grid_Items` | Grid items count | Pass |
| `FieldBinding_Resolves_CustomModel_Properties` | Custom model | Pass |
| `FieldBinding_MissingProperty_Returns_Default_Without_Throwing` | Missing field | Pass |
| `OnRead_Fires_On_Init_When_Bound` | OnRead on init | Pass |
| `OnRead_Data_Is_Displayed_When_Populated_By_Handler` | OnRead data | Pass |
| `OnRead_Fires_On_PathChange` | OnRead on navigate | Pass |
| `Height_Renders_In_Style_Attribute` | Height style | Pass |
| `Height_Null_Does_Not_Add_Height_Style` | No height | Pass |
| `OnCreate_Fires_When_New_Folder_Clicked` | OnCreate args | Pass |
| `AllowCreate_False_Does_Not_Render_NewFolder_Button` | No create btn | Pass |
| `OnDelete_Fires_With_FileManagerDeleteEventArgs` | OnDelete args | Pass |
| `FileManagerViewType_Has_ListView_And_Grid_Values` | Enum values | Pass |
| `FileManagerViewMode_Obsolete_Still_Has_List_And_Grid` | Obsolete compat | Pass |
| `FormatSize_Returns_Correct_String` (3 cases) | Size formatting | Pass |

---

## Known Deferred Items (Phase B)

- `FileManagerEditEventArgs<TItem>` — stub only, no `OnEdit` event wired yet
- `FileManagerUpdateEventArgs<TItem>` — stub only, no `OnUpdate` event wired yet
- `FileManagerDownloadEventArgs<TItem>` — stub only, no `OnDownload` event wired yet
- Delete UI button (requires toolbar/context-menu work)
- Rename UI (requires inline edit)
- Drag-drop reordering
- CSS provider integration (FluentUI / Bootstrap SCSS classes for `mar-filemanager__*`)
