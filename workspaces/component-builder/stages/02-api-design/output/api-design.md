# API Design: MariloDataSheet

## Parameters

| Name | Type | Default | Required | Description |
|---|---|---|---|---|
| Data | IEnumerable<TItem>? | null | Yes | Row dataset |
| KeyField | string | "Id" | No | Property name used as row key for dirty tracking |
| OnSaveAll | EventCallback<DataSheetSaveArgs<TItem>> | — | No | Fires with dirty rows on Save All |
| OnRowChanged | EventCallback<DataSheetRowChangedArgs<TItem>> | — | No | Fires after each cell commit |
| OnValidate | EventCallback<DataSheetValidateArgs<TItem>> | — | No | Fires before Save All |
| IsSaving | bool | false | No | Shows saving spinner |
| AllowAddRow | bool | false | No | Shows Add Row button |
| AllowDeleteRow | bool | false | No | Shows delete button |
| AllowBulkPaste | bool | true | No | Enables Ctrl+V TSV paste |
| EmptyStateMessage | string | "No data." | No | Empty state text |
| Height | string? | null | No | Container height |
| IsLoading | bool | false | No | Shows skeleton rows |
| EnableVirtualization | bool | true | No | Use Virtualize |
| AriaLabel | string | "Editable data grid" | No | Accessible label |
| ChildContent | RenderFragment? | — | No | Column definitions |
| ToolbarTemplate | RenderFragment? | — | No | Toolbar content slot |

## Events
- DataSheetSaveArgs<TItem>: DirtyRows, DeletedRows
- DataSheetRowChangedArgs<TItem>: Row, Field, OldValue, NewValue
- DataSheetValidateArgs<TItem>: DirtyRows, Errors list

## Enums
- DataSheetColumnType: Text, Number, Date, Select, Checkbox, Computed
- CellState: Pristine, Dirty, Invalid, Saving, Saved

## CSS Provider Methods
- DataSheetClass(bool isLoading)
- DataSheetCellClass(CellState state, bool isActive, bool isEditable)
- DataSheetHeaderCellClass(bool isSortable)
- DataSheetRowClass(bool isDirty, bool isSelected, bool isDeleted)
- DataSheetToolbarClass()
- DataSheetBulkBarClass(bool isVisible)
- DataSheetSaveFooterClass(int dirtyCount)

## Column: MariloDataSheetColumn<TItem>
- Field (string, required), Title, ColumnType, Editable, Required
- MinWidth, Format, Validate, Options, CellTemplate, Width

## Public Ref Methods
- ResetAsync(), ValidateAllAsync(), GetDirtyRows()
- SetDataAsync(IEnumerable<TItem>), ScrollToRowAsync(object key)
