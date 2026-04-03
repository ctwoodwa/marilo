Here is a revised version that sharpens the **DataSheet vs DataGrid vs Spreadsheet** boundaries while preserving your existing API surface.

***

# MariloDataSheet

A **typed, schema‑driven data sheet** for bulk editing application data, built on a grid but scoped to rows of strongly typed records rather than workbook worksheets or formula cells.

MariloDataSheet is:

- Closer to a **data grid specialized for editing** than to an Excel‑style spreadsheet.
- Focused on **typed fields, validation, and bulk commit** of changes via Save All.
- Designed for **line‑of‑business workflows** where each row is a domain entity (order line, invoice row, configuration item).

It is **not** a general‑purpose spreadsheet engine: it does not manage workbooks, worksheets, or arbitrary cell formulas like `=SUM(A1:A10)`. For Excel‑style scenarios, use the Spreadsheet component instead.

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
| `Data` | `IEnumerable<TItem>?` | `null` | Strongly typed row dataset (required). Each row is a domain entity, not an arbitrary cell array. |
| `KeyField` | `string` | `"Id"` | Property name used as row key for dirty tracking and scroll APIs. |
| `OnSaveAll` | `EventCallback<DataSheetSaveArgs<TItem>>` | — | Fires with dirty and deleted rows when the user invokes Save All. Use this to persist changes in bulk. |
| `OnRowChanged` | `EventCallback<DataSheetRowChangedArgs<TItem>>` | — | Fires after each cell commit so callers can react per‑row if needed. |
| `OnValidate` | `EventCallback<DataSheetValidateArgs<TItem>>` | — | Fires before Save All; handler can append validation errors across dirty rows. |
| `IsSaving` | `bool` | `false` | Shows a saving indicator and disables Save All while persistence is in progress. |
| `AllowAddRow` | `bool` | `false` | Shows a “+ Add Row” action in the sheet toolbar. |
| `AllowDeleteRow` | `bool` | `false` | Enables per‑row delete and bulk delete actions. |
| `AllowBulkPaste` | `bool` | `true` | Enables Ctrl+V TSV paste into a typed cell range for fast data entry. |
| `EmptyStateMessage` | `string` | `"No data."` | Message when `Data` is null or empty. |
| `Height` | `string?` | `null` | Fixed container height (enables scroll with sticky header and bulk bar). |
| `IsLoading` | `bool` | `false` | Shows skeleton rows while data is loading. |
| `EnableVirtualization` | `bool` | `true` | Uses Blazor `Virtualize` for rows in large datasets. |
| `AriaLabel` | `string` | `"Editable data grid"` | Accessible label for the sheet’s grid region. |
| `ChildContent` | `RenderFragment?` | — | Column definitions via `MariloDataSheetColumn`. |
| `ToolbarTemplate` | `RenderFragment?` | — | Additional toolbar content (filters, per‑view actions, etc.). |

## Column Parameters (MariloDataSheetColumn)

Each column represents a **typed field** on `TItem`. DataSheet focuses on mapping those fields to in‑cell editors with validation and formatting.

| Name | Type | Default | Description |
|---|---|---|---|
| `Field` | `string` | required | Bound property name on `TItem`. |
| `Title` | `string` | Field value | Header label shown in the sheet. |
| `ColumnType` | `DataSheetColumnType` | `Text` | Editor type for the field (text, number, date, select, etc.). |
| `Editable` | `bool` | `true` | Whether users can edit the field in‑place. |
| `Required` | `bool` | `false` | If true, blocks Save All if the field is empty. |
| `MinWidth` | `int?` | `null` | Minimum column width in px. |
| `Width` | `string?` | `null` | Column width (CSS value, e.g. `"120px"` or `"10rem"`). |
| `Format` | `Func<TItem, string?>?` | `null` | Display formatter for read mode and computed columns. |
| `Validate` | `Func<TItem, string?>?` | `null` | Per‑row validator; return `null` for valid, error message for invalid. |
| `Options` | `IEnumerable<DataSheetSelectOption>?` | `null` | Option list for `Select` columns. |
| `CellTemplate` | `RenderFragment<DataSheetCellContext<TItem>>?` | `null` | Custom cell rendering/editing for advanced cases. |

## Events

### DataSheetSaveArgs\<TItem\>

| Property | Type | Description |
|---|---|---|
| `DirtyRows` | `IReadOnlyList<TItem>` | Rows with pending changes that will be persisted. |
| `DeletedRows` | `IReadOnlyList<TItem>` | Rows the user has marked for deletion. |

### DataSheetRowChangedArgs\<TItem\>

| Property | Type | Description |
|---|---|---|
| `Row` | `TItem` | Row containing the edited cell. |
| `Field` | `string` | Name of the field that changed. |
| `OldValue` | `object?` | Previous value. |
| `NewValue` | `object?` | New value after commit. |

### DataSheetValidateArgs\<TItem\>

| Property | Type | Description |
|---|---|---|
| `DirtyRows` | `IReadOnlyList<TItem>` | Rows to validate before Save All. |
| `Errors` | `List<DataSheetValidationError<TItem>>` | Handler appends row/field errors here. |

## Enums

### DataSheetColumnType

`Text` | `Number` | `Date` | `Select` | `Checkbox` | `Computed`  

Represents **typed field editors**, not arbitrary spreadsheet formula cell types.

### CellState

`Pristine` | `Dirty` | `Invalid` | `Saving` | `Saved`  

Tracks per‑cell lifecycle for UX and CSS.

## CSS Provider Methods

| Method | Description |
|---|---|
| `DataSheetClass(bool isLoading)` | Root sheet container (grid + toolbar + bulk bar). |
| `DataSheetCellClass(CellState, bool isActive, bool isEditable)` | Data cell styling based on state and focus. |
| `DataSheetHeaderCellClass(bool isSortable)` | Header cell styling. |
| `DataSheetRowClass(bool isDirty, bool isSelected, bool isDeleted)` | Row styling for dirty, selection, and delete states. |
| `DataSheetToolbarClass()` | Toolbar region styling. |
| `DataSheetBulkBarClass(bool isVisible)` | Bulk action bar (Save All, bulk delete, etc.). |
| `DataSheetSaveFooterClass(int dirtyCount)` | Save footer styling based on dirty row count. |

## Public Methods (via @ref)

These APIs operate on the **typed row set** and validation state, not on arbitrary cells or worksheets.

| Method | Returns | Description |
|---|---|---|
| `ResetAsync()` | `Task` | Discard all dirty state and revert the sheet to the last committed dataset. |
| `ValidateAllAsync()` | `Task<bool>` | Run full validation across dirty rows; returns true if there are no errors. |
| `GetDirtyRows()` | `IReadOnlyList<TItem>` | Snapshot of all dirty rows. |
| `SetDataAsync(IEnumerable<TItem>)` | `Task` | Replace the underlying dataset and reset state. |
| `ScrollToRowAsync(object key)` | `Task` | Scroll the row identified by `KeyField` into view. |

## Keyboard Shortcuts

DataSheet uses spreadsheet‑style shortcuts, but they operate in the context of **typed fields and rows**, not workbook formulas.

| Key | Behavior |
|---|---|
| Tab / Shift+Tab | Move focus right/left (wraps to next row). |
| Enter | Commit cell and move focus down. |
| Escape | Cancel edit and restore original value. |
| F2 | Enter edit mode on the focused cell. |
| Arrow keys | Navigate non‑editing cells. |
| Ctrl+S | Invoke Save All. |
| Ctrl+Z | Undo last cell change. |
| Ctrl+C | Copy selected cell(s) as TSV. |
| Ctrl+V | Paste TSV from clipboard into the active range (honoring column types where possible). |
| Delete | Clear selected cell(s). |
| Ctrl+D | Fill selected range down from the active cell. |

## Accessibility

MariloDataSheet exposes a grid‑like accessibility model tuned for bulk editable data:

- Root element: `role="grid"` with `aria-label`.
- Rows: `role="row"`.
- Cells: `role="gridcell"`, with:
  - `aria-readonly="true"` on computed/non‑editable cells.
  - `aria-invalid="true"` on cells with validation errors.
- Save/validation feedback announced via an `aria-live="polite"` region.
- Full keyboard navigation with a visible focus ring for all interactive cells.

***
