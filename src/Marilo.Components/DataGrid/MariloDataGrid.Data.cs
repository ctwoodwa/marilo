using Marilo.Core.Data;
using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Marilo.Components.DataGrid;

/// <summary>
/// Data processing pipeline for MariloDataGrid: filtering, sorting, grouping, paging.
/// </summary>
public partial class MariloDataGrid<TItem>
{
    // ── Data Processing ─────────────────────────────────────────────────

    internal async Task ProcessDataAsync()
    {
        if (OnRead.HasDelegate)
        {
            var args = new GridReadEventArgs<TItem> { Request = GetState() };
            await OnRead.InvokeAsync(args);
            _displayedItems = args.Data.ToList();
            _state.TotalCount = args.Total;
        }
        else
        {
            ProcessDataClientSide();
        }
    }

    private void ProcessDataClientSide()
    {
        if (Data is null)
        {
            _displayedItems = [];
            _groupedRows = [];
            _state.TotalCount = 0;
            return;
        }

        IEnumerable<TItem> items = Data;

        // Apply global search
        if (!string.IsNullOrWhiteSpace(_searchText))
        {
            items = ApplySearch(items, _searchText);
        }

        // Apply filters
        foreach (var filter in _state.FilterDescriptors)
        {
            items = ApplyFilter(items, filter);
        }

        // Apply sorting
        items = ApplySort(items);

        var allItems = items.ToList();
        _state.TotalCount = allItems.Count;

        // Apply grouping
        if (_state.GroupDescriptors.Count > 0)
        {
            _groupedRows = BuildGroups(allItems, _state.GroupDescriptors, 0);

            // For grouped data, build a flat display list respecting collapsed state
            _displayedItems = FlattenGroups(_groupedRows);

            // Paging on grouped data applies to the flattened visible items
            if (Pageable && _state.PageSize > 0)
            {
                // TotalCount should reflect total data items, not group rows
                _state.TotalCount = allItems.Count;
            }
        }
        else
        {
            _groupedRows = [];

            // Apply paging
            if (Pageable && _state.PageSize > 0)
            {
                var skip = (_state.CurrentPage - 1) * _state.PageSize;
                _displayedItems = allItems.Skip(skip).Take(_state.PageSize).ToList();
            }
            else
            {
                _displayedItems = allItems;
            }
        }
    }

    // ── Grouping ────────────────────────────────────────────────────────

    private List<GridGroupRow<TItem>> BuildGroups(
        List<TItem> items,
        List<GroupDescriptor> descriptors,
        int depth)
    {
        if (depth >= descriptors.Count)
            return [];

        var descriptor = descriptors[depth];
        var prop = typeof(TItem).GetProperty(descriptor.Field);
        if (prop is null) return [];

        var grouped = items.GroupBy(item => prop.GetValue(item));

        // Sort groups by key
        var orderedGroups = descriptor.Direction == SortDirection.Ascending
            ? grouped.OrderBy(g => g.Key)
            : grouped.OrderByDescending(g => g.Key);

        var result = new List<GridGroupRow<TItem>>();
        foreach (var group in orderedGroups)
        {
            var groupItems = group.ToList();
            var groupRow = new GridGroupRow<TItem>
            {
                Field = descriptor.Field,
                Key = group.Key,
                Items = groupItems,
                Depth = depth,
                ChildGroups = depth + 1 < descriptors.Count
                    ? BuildGroups(groupItems, descriptors, depth + 1)
                    : []
            };
            result.Add(groupRow);
        }

        return result;
    }

    private List<TItem> FlattenGroups(List<GridGroupRow<TItem>> groups)
    {
        var result = new List<TItem>();
        foreach (var group in groups)
        {
            if (_collapsedGroups.Contains(group.GroupKey))
                continue; // Group is collapsed, skip its items

            if (group.HasChildGroups)
            {
                result.AddRange(FlattenGroups(group.ChildGroups));
            }
            else
            {
                result.AddRange(group.Items);
            }
        }
        return result;
    }

    /// <summary>Toggles a group's collapsed state.</summary>
    internal async Task ToggleGroup(string groupKey)
    {
        if (!_collapsedGroups.Remove(groupKey))
            _collapsedGroups.Add(groupKey);

        // Rebuild the flat display list
        if (_state.GroupDescriptors.Count > 0)
        {
            _displayedItems = FlattenGroups(_groupedRows);
        }

        await NotifyStateChanged("Group");
        StateHasChanged();
    }

    /// <summary>Adds a group descriptor and reprocesses data.</summary>
    public async Task GroupBy(string field, SortDirection direction = SortDirection.Ascending)
    {
        if (!Groupable) return;
        if (_state.GroupDescriptors.Any(g => g.Field == field)) return;

        _state.GroupDescriptors.Add(new GroupDescriptor { Field = field, Direction = direction });
        _state.CurrentPage = 1;
        await ProcessDataAsync();
        await NotifyStateChanged("Group");
        StateHasChanged();
    }

    /// <summary>Removes a group descriptor and reprocesses data.</summary>
    public async Task Ungroup(string field)
    {
        var existing = _state.GroupDescriptors.FirstOrDefault(g => g.Field == field);
        if (existing is null) return;

        _state.GroupDescriptors.Remove(existing);
        _collapsedGroups.RemoveWhere(k => k.StartsWith($"{field}:"));
        _state.CurrentPage = 1;
        await ProcessDataAsync();
        await NotifyStateChanged("Group");
        StateHasChanged();
    }

    /// <summary>Removes all group descriptors.</summary>
    public async Task UngroupAll()
    {
        _state.GroupDescriptors.Clear();
        _collapsedGroups.Clear();
        _state.CurrentPage = 1;
        await ProcessDataAsync();
        await NotifyStateChanged("Group");
        StateHasChanged();
    }

    /// <summary>Whether the given group key is collapsed.</summary>
    internal bool IsGroupCollapsed(string groupKey) => _collapsedGroups.Contains(groupKey);

    // ── Search ──────────────────────────────────────────────────────────

    private IEnumerable<TItem> ApplySearch(IEnumerable<TItem> items, string searchText)
    {
        var lower = searchText.ToLowerInvariant();
        var props = _visibleColumns
            .Where(c => !string.IsNullOrEmpty(c.Field))
            .Select(c => typeof(TItem).GetProperty(c.Field))
            .Where(p => p is not null)
            .ToList();

        return items.Where(item =>
            props.Any(prop =>
                prop!.GetValue(item)?.ToString()?.Contains(lower, StringComparison.OrdinalIgnoreCase) == true));
    }

    internal async Task OnSearchChanged(ChangeEventArgs e)
    {
        _searchText = e.Value?.ToString() ?? "";
        _state.CurrentPage = 1;
        await ProcessDataAsync();
        await NotifyStateChanged("Search");
    }

    // ── CSV Export ──────────────────────────────────────────────────────

    /// <summary>
    /// Generates CSV content from the current data (after filtering/sorting, before paging).
    /// Returns the CSV as a string. Use with JS interop to trigger download.
    /// </summary>
    public string ExportToCsv(bool includeHeaders = true, string separator = ",")
    {
        var sb = new System.Text.StringBuilder();
        var columns = _visibleColumns;

        if (includeHeaders)
        {
            sb.AppendLine(string.Join(separator, columns.Select(c => EscapeCsv(c.DisplayTitle, separator))));
        }

        // Use all filtered/sorted data, not just current page
        IEnumerable<TItem> items;
        if (Data is not null)
        {
            items = Data;
            if (!string.IsNullOrWhiteSpace(_searchText))
                items = ApplySearch(items, _searchText);
            foreach (var filter in _state.FilterDescriptors)
                items = ApplyFilter(items, filter);
            items = ApplySort(items);
        }
        else
        {
            items = _displayedItems;
        }

        foreach (var item in items)
        {
            var values = columns.Select(c => EscapeCsv(c.GetDisplayValue(item), separator));
            sb.AppendLine(string.Join(separator, values));
        }

        return sb.ToString();
    }

    private static string EscapeCsv(string value, string separator)
    {
        if (string.IsNullOrEmpty(value)) return "";
        if (value.Contains(separator) || value.Contains('"') || value.Contains('\n') || value.Contains('\r'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
        return value;
    }

    // ── Filtering (extended operators) ──────────────────────────────────

    private static IEnumerable<TItem> ApplyFilter(IEnumerable<TItem> items, FilterDescriptor filter)
    {
        if (string.IsNullOrEmpty(filter.Field)) return items;

        var prop = typeof(TItem).GetProperty(filter.Field);
        if (prop is null) return items;

        // Null check operators don't need a filter value
        if (filter.Operator == FilterOperator.IsNull)
            return items.Where(item => prop.GetValue(item) is null);

        if (filter.Operator == FilterOperator.IsNotNull)
            return items.Where(item => prop.GetValue(item) is not null);

        if (filter.Value is null) return items;

        var filterValue = filter.Value.ToString()?.ToLowerInvariant() ?? "";

        return filter.Operator switch
        {
            FilterOperator.Contains => items.Where(item =>
                prop.GetValue(item)?.ToString()?.Contains(filterValue, StringComparison.OrdinalIgnoreCase) == true),

            FilterOperator.Equals => items.Where(item =>
                string.Equals(prop.GetValue(item)?.ToString(), filterValue, StringComparison.OrdinalIgnoreCase)),

            FilterOperator.NotEquals => items.Where(item =>
                !string.Equals(prop.GetValue(item)?.ToString(), filterValue, StringComparison.OrdinalIgnoreCase)),

            FilterOperator.StartsWith => items.Where(item =>
                prop.GetValue(item)?.ToString()?.StartsWith(filterValue, StringComparison.OrdinalIgnoreCase) == true),

            FilterOperator.EndsWith => items.Where(item =>
                prop.GetValue(item)?.ToString()?.EndsWith(filterValue, StringComparison.OrdinalIgnoreCase) == true),

            FilterOperator.GreaterThan => ApplyComparison(items, prop, filterValue, (cmp) => cmp > 0),
            FilterOperator.GreaterThanOrEqual => ApplyComparison(items, prop, filterValue, (cmp) => cmp >= 0),
            FilterOperator.LessThan => ApplyComparison(items, prop, filterValue, (cmp) => cmp < 0),
            FilterOperator.LessThanOrEqual => ApplyComparison(items, prop, filterValue, (cmp) => cmp <= 0),

            _ => items
        };
    }

    private static IEnumerable<TItem> ApplyComparison(
        IEnumerable<TItem> items,
        System.Reflection.PropertyInfo prop,
        string filterValue,
        Func<int, bool> comparison)
    {
        return items.Where(item =>
        {
            var value = prop.GetValue(item);
            if (value is null) return false;

            if (value is IComparable comparable)
            {
                // Try to convert filter value to the property type for proper comparison
                try
                {
                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    var convertedFilter = Convert.ChangeType(filterValue, targetType);
                    return comparison(comparable.CompareTo(convertedFilter));
                }
                catch
                {
                    // Fall back to string comparison
                    return comparison(string.Compare(value.ToString(), filterValue, StringComparison.OrdinalIgnoreCase));
                }
            }

            return false;
        });
    }

    // ── Sorting ─────────────────────────────────────────────────────────

    private IEnumerable<TItem> ApplySort(IEnumerable<TItem> items)
    {
        if (_state.SortDescriptors.Count == 0) return items;

        IOrderedEnumerable<TItem>? ordered = null;
        foreach (var sort in _state.SortDescriptors)
        {
            var prop = typeof(TItem).GetProperty(sort.Field);
            if (prop is null) continue;

            if (ordered is null)
            {
                ordered = sort.Direction == SortDirection.Ascending
                    ? items.OrderBy(item => prop.GetValue(item))
                    : items.OrderByDescending(item => prop.GetValue(item));
            }
            else
            {
                ordered = sort.Direction == SortDirection.Ascending
                    ? ordered.ThenBy(item => prop.GetValue(item))
                    : ordered.ThenByDescending(item => prop.GetValue(item));
            }
        }

        return ordered ?? items;
    }

    // ── Event Handlers: Sorting ─────────────────────────────────────────

    internal async Task OnHeaderClick(MariloGridColumn<TItem> column, bool isSortable, MouseEventArgs? e = null)
    {
        if (!isSortable) return;

        var isMultiSort = e?.CtrlKey == true || e?.MetaKey == true;
        var existing = _state.SortDescriptors.FirstOrDefault(s => s.Field == column.Field);

        if (existing is null)
        {
            if (!isMultiSort)
                _state.SortDescriptors.Clear();
            _state.SortDescriptors.Add(new SortDescriptor
            {
                Field = column.Field,
                Direction = SortDirection.Ascending
            });
        }
        else if (existing.Direction == SortDirection.Ascending)
        {
            existing.Direction = SortDirection.Descending;
        }
        else
        {
            _state.SortDescriptors.Remove(existing);
        }

        _state.CurrentPage = 1;
        await ProcessDataAsync();
        await NotifyPageChanged();
        await NotifyStateChanged("Sort");
    }

    // ── Event Handlers: Filtering ───────────────────────────────────────

    internal async Task OnFilterChanged(string field, ChangeEventArgs e)
    {
        var value = e.Value?.ToString();
        var existing = _state.FilterDescriptors.FirstOrDefault(f => f.Field == field);

        if (string.IsNullOrWhiteSpace(value))
        {
            if (existing != null)
                _state.FilterDescriptors.Remove(existing);
        }
        else
        {
            if (existing != null)
            {
                existing.Value = value;
            }
            else
            {
                _state.FilterDescriptors.Add(new FilterDescriptor
                {
                    Field = field,
                    Operator = FilterOperator.Contains,
                    Value = value
                });
            }
        }

        _state.CurrentPage = 1;
        await ProcessDataAsync();
        await NotifyPageChanged();
        await NotifyStateChanged("Filter");
    }

    // ── FilterMenu support ──────────────────────────────────────────────

    internal void ToggleFilterMenu(string field)
    {
        if (_filterMenuField == field)
        {
            _filterMenuField = null;
        }
        else
        {
            _filterMenuField = field;
            var existing = _state.FilterDescriptors.FirstOrDefault(f => f.Field == field);
            _filterMenuOperator = existing?.Operator ?? FilterOperator.Contains;
            _filterMenuValue = existing?.Value?.ToString() ?? "";
        }
    }

    internal async Task ApplyFilterMenu()
    {
        if (_filterMenuField == null) return;

        var existing = _state.FilterDescriptors.FirstOrDefault(f => f.Field == _filterMenuField);

        if (string.IsNullOrWhiteSpace(_filterMenuValue)
            && _filterMenuOperator != FilterOperator.IsNull
            && _filterMenuOperator != FilterOperator.IsNotNull)
        {
            if (existing != null)
                _state.FilterDescriptors.Remove(existing);
        }
        else
        {
            if (existing != null)
            {
                existing.Operator = _filterMenuOperator;
                existing.Value = _filterMenuValue;
            }
            else
            {
                _state.FilterDescriptors.Add(new FilterDescriptor
                {
                    Field = _filterMenuField,
                    Operator = _filterMenuOperator,
                    Value = _filterMenuValue
                });
            }
        }

        _filterMenuField = null;
        _state.CurrentPage = 1;
        await ProcessDataAsync();
        await NotifyPageChanged();
        await NotifyStateChanged("Filter");
    }

    internal async Task ClearFilterMenu()
    {
        if (_filterMenuField == null) return;
        var existing = _state.FilterDescriptors.FirstOrDefault(f => f.Field == _filterMenuField);
        if (existing != null)
            _state.FilterDescriptors.Remove(existing);

        _filterMenuField = null;
        _filterMenuValue = "";
        _state.CurrentPage = 1;
        await ProcessDataAsync();
        await NotifyPageChanged();
        await NotifyStateChanged("Filter");
    }

    // ── Event Handlers: Selection ───────────────────────────────────────

    internal async Task HandleRowClick(TItem item, MouseEventArgs e)
    {
        if (SelectionMode != GridSelectionMode.None && !ShowCheckboxColumn)
        {
            await ToggleSelection(item);
        }

        if (OnRowClick.HasDelegate)
        {
            await OnRowClick.InvokeAsync(new GridRowClickEventArgs<TItem>
            {
                Item = item,
                EventArgs = e
            });
        }
    }

    internal async Task HandleRowDoubleClick(TItem item, MouseEventArgs e)
    {
        // In InCell mode, double-click enters edit mode
        if (EditMode == GridEditMode.InCell && !IsItemEditing(item))
        {
            await BeginEdit(item);
        }

        if (OnRowDoubleClick.HasDelegate)
        {
            await OnRowDoubleClick.InvokeAsync(new GridRowClickEventArgs<TItem>
            {
                Item = item,
                EventArgs = e
            });
        }
    }

    internal async Task HandleRowContextMenu(TItem item, MouseEventArgs e)
    {
        if (OnRowContextMenu.HasDelegate)
        {
            await OnRowContextMenu.InvokeAsync(new GridRowClickEventArgs<TItem>
            {
                Item = item,
                EventArgs = e
            });
        }
    }

    internal async Task ToggleSelection(TItem item)
    {
        if (SelectionMode == GridSelectionMode.Single)
        {
            _selectedItems.Clear();
            _selectedItems.Add(item);
        }
        else if (SelectionMode == GridSelectionMode.Multiple)
        {
            if (!_selectedItems.Remove(item))
                _selectedItems.Add(item);
        }

        await SelectedItemsChanged.InvokeAsync(_selectedItems.ToList());
        await NotifyStateChanged("Selection");
    }

    internal async Task OnCheckboxToggle(TItem item)
    {
        await ToggleSelection(item);
    }

    internal async Task OnSelectAllChanged(ChangeEventArgs e)
    {
        var selectAll = e.Value is true or "true";
        if (selectAll)
        {
            _selectedItems = new HashSet<TItem>(_displayedItems);
        }
        else
        {
            _selectedItems.Clear();
        }

        await SelectedItemsChanged.InvokeAsync(_selectedItems.ToList());
        await NotifyStateChanged("Selection");
    }

    // ── Event Handlers: Paging ──────────────────────────────────────────

    internal async Task GoToPreviousPage()
    {
        if (_state.CurrentPage > 1)
        {
            _state.CurrentPage--;
            await ProcessDataAsync();
            await NotifyPageChanged();
            await NotifyStateChanged("Page");
        }
    }

    internal async Task GoToNextPage()
    {
        if (_state.CurrentPage < TotalPages)
        {
            _state.CurrentPage++;
            await ProcessDataAsync();
            await NotifyPageChanged();
            await NotifyStateChanged("Page");
        }
    }

    internal async Task OnPageSizeDropdownChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var newSize) && newSize > 0)
        {
            _state.PageSize = newSize;
            _state.CurrentPage = 1;
            await ProcessDataAsync();
            await PageSizeChanged.InvokeAsync(newSize);
            await NotifyPageChanged();
            await NotifyStateChanged("PageSize");
        }
    }
}
