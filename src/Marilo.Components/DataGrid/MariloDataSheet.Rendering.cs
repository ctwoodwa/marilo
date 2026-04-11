using Marilo.Core.Enums;
using Marilo.Core.Helpers;
using Marilo.Core.Models.DataSheet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Marilo.Components.DataGrid;

/// <summary>
/// RenderTreeBuilder helpers, row/cell class computation for MariloDataSheet.
/// </summary>
public partial class MariloDataSheet<TItem>
{
    // ── Row Rendering ──────────────────────────────────────────────────

    internal RenderFragment RenderRow(TItem row) => builder =>
    {
        var isDirty = IsRowDirty(row);
        var isSelected = _selectedRows.Contains(row);
        var isDeleted = IsRowDeleted(row);
        var rowKey = GetRowKey(row);
        var rowClass = CssProvider.DataSheetRowClass(isDirty, isSelected, isDeleted);

        builder.OpenElement(0, "tr");
        builder.AddAttribute(1, "class", rowClass);
        builder.AddAttribute(2, "role", "row");
        if (rowKey != null) builder.AddAttribute(3, "data-row-key", rowKey.ToString());
        if (isDeleted) builder.AddAttribute(4, "aria-hidden", "true");

        // Checkbox column
        if (AllowDeleteRow)
        {
            var cbRow = row;
            builder.OpenElement(10, "td");
            builder.AddAttribute(11, "role", "gridcell");
            builder.AddAttribute(12, "class", "mar-datasheet__select-cell");
            builder.OpenElement(13, "input");
            builder.AddAttribute(14, "type", "checkbox");
            builder.AddAttribute(15, "checked", isSelected);
            builder.AddAttribute(16, "aria-label", "Select row");
            builder.AddAttribute(17, "onchange", EventCallback.Factory.Create(this, () => ToggleRowSelection(cbRow)));
            builder.CloseElement(); // input
            builder.CloseElement(); // td
        }

        // Data cells
        foreach (var column in _columns)
        {
            var cellRow = row;
            var cellField = column.Field;
            var cellState = GetCellState(row, column.Field);
            var isActive = IsCellActive(row, column.Field);
            var isEditing = IsCellEditing(row, column.Field);
            var cellClass = CssProvider.DataSheetCellClass(cellState, isActive, column.Editable);
            var cellError = GetCellError(row, column.Field);
            var cellWidth = column.Width != null ? $"width:{column.Width};" :
                            column.MinWidth.HasValue ? $"min-width:{column.MinWidth}px;" : null;

            builder.OpenElement(20, "td");
            builder.AddAttribute(21, "class", cellClass);
            builder.AddAttribute(22, "role", "gridcell");
            if (cellWidth != null) builder.AddAttribute(23, "style", cellWidth);
            if (!column.Editable || column.ColumnType == DataSheetColumnType.Computed)
                builder.AddAttribute(24, "aria-readonly", "true");
            if (cellState == CellState.Invalid)
                builder.AddAttribute(25, "aria-invalid", "true");
            if (cellError != null)
                builder.AddAttribute(26, "title", cellError);
            builder.AddAttribute(27, "data-field", column.Field);

            // Click handler
            builder.AddAttribute(28, "onclick",
                EventCallback.Factory.Create<MouseEventArgs>(this, (_) => OnCellClick(cellRow, cellField)));

            // Cell content
            if (column.CellTemplate != null && !isEditing)
            {
                var context = new DataSheetCellContext<TItem>
                {
                    Item = row,
                    Field = column.Field,
                    Value = GridReflectionHelper.GetValue(row, column.Field),
                    IsEditing = isEditing,
                    IsDirty = cellState == CellState.Dirty,
                    ValidationError = cellError
                };
                builder.AddContent(30, column.CellTemplate(context));
            }
            else if (isEditing)
            {
                RenderCellEditor(builder, row, column);
            }
            else
            {
                RenderCellDisplay(builder, row, column);
            }

            builder.CloseElement(); // td
        }

        // Delete action column
        if (AllowDeleteRow)
        {
            var delRow = row;
            builder.OpenElement(90, "td");
            builder.AddAttribute(91, "role", "gridcell");
            builder.AddAttribute(92, "class", "mar-datasheet__actions-cell");
            builder.OpenElement(93, "button");
            builder.AddAttribute(94, "type", "button");
            builder.AddAttribute(95, "class", "mar-datasheet__delete-btn");
            builder.AddAttribute(96, "aria-label", "Delete row");
            builder.AddAttribute(97, "onclick",
                EventCallback.Factory.Create<MouseEventArgs>(this, (_) => MarkRowDeleted(delRow)));
            builder.AddContent(98, "\u2715"); // X symbol
            builder.CloseElement(); // button
            builder.CloseElement(); // td
        }

        builder.CloseElement(); // tr
    };

    // ── Cell Display ───────────────────────────────────────────────────

    private void RenderCellDisplay(RenderTreeBuilder builder, TItem row, MariloDataSheetColumn<TItem> column)
    {
        var value = GridReflectionHelper.GetValue(row, column.Field);

        switch (column.ColumnType)
        {
            case DataSheetColumnType.Checkbox:
                builder.OpenElement(40, "input");
                builder.AddAttribute(41, "type", "checkbox");
                builder.AddAttribute(42, "checked", value is true);
                builder.AddAttribute(43, "disabled", !column.Editable);
                builder.AddAttribute(44, "aria-label", column.DisplayTitle);
                builder.CloseElement();
                break;

            case DataSheetColumnType.Computed:
                var formatted = column.Format != null ? column.Format(row) : value?.ToString() ?? "";
                builder.AddContent(40, formatted);
                break;

            default:
                var display = column.Format != null ? column.Format(row) : value?.ToString() ?? "";
                builder.OpenElement(40, "span");
                builder.AddAttribute(41, "class", "mar-datasheet__cell-text");
                builder.AddContent(42, display);
                builder.CloseElement();
                break;
        }
    }

    // ── Cell Editor ────────────────────────────────────────────────────

    private void RenderCellEditor(RenderTreeBuilder builder, TItem row, MariloDataSheetColumn<TItem> column)
    {
        var value = GridReflectionHelper.GetValue(row, column.Field);
        var editRow = row;
        var editField = column.Field;

        switch (column.ColumnType)
        {
            case DataSheetColumnType.Text:
                builder.OpenElement(50, "input");
                builder.AddAttribute(51, "type", "text");
                builder.AddAttribute(52, "class", "mar-datasheet__editor-input");
                builder.AddAttribute(53, "value", value?.ToString() ?? "");
                builder.AddAttribute(54, "aria-label", $"Edit {column.DisplayTitle}");
                builder.AddAttribute(55, "onchange",
                    EventCallback.Factory.Create<ChangeEventArgs>(this,
                        (e) => OnCellValueCommit(editRow, editField, e.Value)));
                builder.CloseElement();
                break;

            case DataSheetColumnType.Number:
                var numberTargetType = typeof(TItem).GetProperty(column.Field)?.PropertyType
                                       ?? typeof(decimal);
                builder.OpenElement(50, "input");
                builder.AddAttribute(51, "type", "number");
                builder.AddAttribute(52, "class", "mar-datasheet__editor-input");
                builder.AddAttribute(53, "value", value?.ToString() ?? "0");
                builder.AddAttribute(54, "aria-label", $"Edit {column.DisplayTitle}");
                builder.AddAttribute(55, "step", "any");
                builder.AddAttribute(56, "onchange",
                    EventCallback.Factory.Create<ChangeEventArgs>(this,
                        (e) =>
                        {
                            var (_, parsed) = ParseNumericValue(e.Value?.ToString(), numberTargetType);
                            return OnCellValueCommit(editRow, editField, parsed);
                        }));
                builder.CloseElement();
                break;

            case DataSheetColumnType.Date:
                builder.OpenElement(50, "input");
                builder.AddAttribute(51, "type", "date");
                builder.AddAttribute(52, "class", "mar-datasheet__editor-input");
                builder.AddAttribute(53, "value", value is DateTime dt ? dt.ToString("yyyy-MM-dd") : "");
                builder.AddAttribute(54, "aria-label", $"Edit {column.DisplayTitle}");
                builder.AddAttribute(55, "onchange",
                    EventCallback.Factory.Create<ChangeEventArgs>(this,
                        (e) =>
                        {
                            DateTime.TryParse(e.Value?.ToString(), out var parsed);
                            return OnCellValueCommit(editRow, editField, parsed == default ? null : (object)parsed);
                        }));
                builder.CloseElement();
                break;

            case DataSheetColumnType.Select:
                builder.OpenElement(50, "select");
                builder.AddAttribute(51, "class", "mar-datasheet__editor-select");
                builder.AddAttribute(52, "value", value?.ToString() ?? "");
                builder.AddAttribute(53, "aria-label", $"Edit {column.DisplayTitle}");
                builder.AddAttribute(54, "onchange",
                    EventCallback.Factory.Create<ChangeEventArgs>(this,
                        (e) => OnCellValueCommit(editRow, editField, e.Value)));

                if (column.Options != null)
                {
                    foreach (var option in column.Options)
                    {
                        builder.OpenElement(60, "option");
                        builder.AddAttribute(61, "value", option.Value);
                        builder.AddContent(62, option.Label);
                        builder.CloseElement();
                    }
                }
                builder.CloseElement();
                break;

            case DataSheetColumnType.Checkbox:
                builder.OpenElement(50, "input");
                builder.AddAttribute(51, "type", "checkbox");
                builder.AddAttribute(52, "checked", value is true);
                builder.AddAttribute(53, "aria-label", $"Edit {column.DisplayTitle}");
                builder.AddAttribute(54, "onchange",
                    EventCallback.Factory.Create<ChangeEventArgs>(this,
                        (e) => OnCellValueCommit(editRow, editField, e.Value is true or "true")));
                builder.CloseElement();
                break;
        }
    }
}
