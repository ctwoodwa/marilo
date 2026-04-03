# MariloDataSheet

A schema-driven spreadsheet-like grid where developers define typed columns and users edit cells inline, paste from Excel, and commit changes in bulk via Save All.

## Basic Usage

```razor
<MariloDataSheet TItem="MyRow" Data="@_rows" KeyField="Id"
                        OnSaveAll="@HandleSaveAll">
    <MariloDataSheetColumn TItem="MyRow" Field="Name" Title="Name"
        ColumnType="DataSheetColumnType.Text" Editable="true" Required="true" />
    <MariloDataSheetColumn TItem="MyRow" Field="Amount" Title="Amount"
        ColumnType="DataSheetColumnType.Number" Editable="true" />
    <MariloDataSheetColumn TItem="MyRow" Field="Total" Title="Total"
        ColumnType="DataSheetColumnType.Computed" Editable="false"
        Format="@(r => r.Total.ToString("C2"))" />
</MariloDataSheet>
```

## Parameters

| Name | Type | Default | Description |
|---|---|---|---|
| `Data` | `IEnumerable<TItem>?` | `null` | Row dataset (required) |
| `KeyField` | `string` | `"Id"` | Property name used as row key for dirty tracking |
| `OnSaveAll` | `EventCallback<DataSheetSaveArgs<TItem>>` | — | Fires with dirty/deleted rows when Save All is confirmed |
| `OnRowChanged` | `EventCallback<DataSheetRowChangedArgs<TItem>>` | — | Fires after each cell commit |
| `OnValidate` | `EventCallback<DataSheetValidateArgs<TItem>>` | — | Fires before Save All; handler may add errors |
| `IsSaving` | `bool` | `false` | Shows saving spinner; disables Save All button |
| `AllowAddRow` | `bool` | `false` | Shows "+ Add Row" button in toolbar |
| `AllowDeleteRow` | `bool` | `false` | Shows delete button per row and bulk action bar |
| `AllowBulkPaste` | `bool` | `true` | Enables Ctrl+V TSV paste into cell range |
| `EmptyStateMessage` | `string` | `"No data."` | Text shown when Data is empty |
| `Height` | `string?` | `null` | Container height (enables scroll + sticky header) |
| `IsLoading` | `bool` | `false` | Shows loading skeleton rows |
| `EnableVirtualization` | `bool` | `true` | Use Blazor Virtualize for rows |
| `AriaLabel` | `string` | `"Editable data grid"` | Accessible label |
| `ChildContent` | `RenderFragment?` | — | Column definitions |
| `ToolbarTemplate` | `RenderFragment?` | — | Additional toolbar content |

## Column Parameters (MariloDataSheetColumn)

| Name | Type | Default | Description |
|---|---|---|---|
| `Field` | `string` | required | Bound property name |
| `Title` | `string` | Field value | Header display label |
| `ColumnType` | `DataSheetColumnType` | `Text` | Cell editor type |
| `Editable` | `bool` | `true` | Whether cells are editable |
| `Required` | `bool` | `false` | Blocks save if empty |
| `MinWidth` | `int?` | `null` | Minimum width in px |
| `Width` | `string?` | `null` | Column width (CSS value) |
| `Format` | `Func<TItem, string?>?` | `null` | Display formatter |
| `Validate` | `Func<TItem, string?>?` | `null` | Returns null=valid, string=error |
| `Options` | `IEnumerable<DataSheetSelectOption>?` | `null` | For Select columns |
| `CellTemplate` | `RenderFragment<DataSheetCellContext<TItem>>?` | `null` | Custom cell override |

## Events

### DataSheetSaveArgs\<TItem\>

| Property | Type | Description |
|---|---|---|
| `DirtyRows` | `IReadOnlyList<TItem>` | Rows with pending changes |
| `DeletedRows` | `IReadOnlyList<TItem>` | Rows marked for deletion |

### DataSheetRowChangedArgs\<TItem\>

| Property | Type | Description |
|---|---|---|
| `Row` | `TItem` | The row containing the change |
| `Field` | `string` | Changed field name |
| `OldValue` | `object?` | Previous value |
| `NewValue` | `object?` | New value |

### DataSheetValidateArgs\<TItem\>

| Property | Type | Description |
|---|---|---|
| `DirtyRows` | `IReadOnlyList<TItem>` | Rows to validate |
| `Errors` | `List<DataSheetValidationError<TItem>>` | Handler appends errors here |

## Enums

### DataSheetColumnType

`Text` | `Number` | `Date` | `Select` | `Checkbox` | `Computed`

### CellState

`Pristine` | `Dirty` | `Invalid` | `Saving` | `Saved`

## CSS Provider Methods

| Method | Description |
|---|---|
| `DataSheetClass(bool isLoading)` | Root grid class |
| `DataSheetCellClass(CellState, bool isActive, bool isEditable)` | Cell class |
| `DataSheetHeaderCellClass(bool isSortable)` | Header cell class |
| `DataSheetRowClass(bool isDirty, bool isSelected, bool isDeleted)` | Row class |
| `DataSheetToolbarClass()` | Toolbar class |
| `DataSheetBulkBarClass(bool isVisible)` | Bulk action bar class |
| `DataSheetSaveFooterClass(int dirtyCount)` | Save footer class |

## Public Methods (via @ref)

| Method | Returns | Description |
|---|---|---|
| `ResetAsync()` | `Task` | Discard all dirty state |
| `ValidateAllAsync()` | `Task<bool>` | Run full validation; true=clean |
| `GetDirtyRows()` | `IReadOnlyList<TItem>` | Current dirty rows snapshot |
| `SetDataAsync(IEnumerable<TItem>)` | `Task` | Replace dataset |
| `ScrollToRowAsync(object key)` | `Task` | Scroll row into view |

## Keyboard Shortcuts

| Key | Behavior |
|---|---|
| Tab / Shift+Tab | Move focus right/left (wraps to next row) |
| Enter | Commit cell, move focus down |
| Escape | Cancel edit, restore original value |
| F2 | Enter edit mode on focused cell |
| Arrow keys | Navigate non-editing cells |
| Ctrl+S | Save All |
| Ctrl+Z | Undo last cell change |
| Ctrl+C | Copy selected cell as TSV |
| Ctrl+V | Paste TSV from clipboard |
| Delete | Clear selected cell(s) |
| Ctrl+D | Fill selected range down |

## Accessibility

- Root element: `role="grid"` with `aria-label`
- Rows: `role="row"`
- Cells: `role="gridcell"` with `aria-readonly` on computed cells, `aria-invalid` on errors
- Save announcements via `aria-live="polite"` region
- Full keyboard navigation with visible focus ring
