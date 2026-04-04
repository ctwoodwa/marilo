# Resolution Records: MariloDataGrid Phase 1 — Pure C# Gaps

> Date: 2026-04-04
> Source: `stages/02-prioritize/output/gap-datagrid-backlog.md`
> Component: `MariloDataGrid<TItem>` — `src/Marilo.Components/DataGrid/`

---

## RES-DG-001: Add `SortMode` enum (Single/Multiple)

**Resolves:** DG-P1-01
**Status:** Ready for implementation

### Target Pattern

```csharp
// In GridEnums.cs
public enum GridSortMode { Single, Multiple }

// In MariloDataGrid.razor.cs
[Parameter] public GridSortMode SortMode { get; set; } = GridSortMode.Multiple;
```

When `SortMode` is `Single`, clicking a column header always clears other sorts first (Ctrl+Click is ignored). When `Multiple`, existing multi-sort behavior is preserved.

### Options Considered

**A: Add `SortMode` enum parameter (chosen)**
- Approach: New enum, single parameter, modify `OnHeaderClick` to respect it
- Pros: Clean API, matches spec, backward compatible (default Multiple)
- Cons: None significant
- Effort: Low (1 enum + 3-line logic change)

**B: Add `AllowMultiSort` bool**
- Approach: Simple boolean toggle
- Pros: Simpler
- Cons: Less extensible if future sort modes needed

### Decision: Option A
Enum is more extensible and matches Telerik convention.

### Success Criteria
- [ ] `GridSortMode` enum exists with `Single` and `Multiple` values
- [ ] `SortMode` parameter defaults to `Multiple`
- [ ] When `SortMode` is `Single`, Ctrl+Click does not add multi-sort
- [ ] When `SortMode` is `Single`, clicking a new column clears previous sort
- [ ] Existing multi-sort behavior preserved when `SortMode` is `Multiple`

---

## RES-DG-002: Add `Editable` column parameter

**Resolves:** DG-P1-02
**Status:** Ready for implementation

### Target Pattern

```csharp
// In MariloGridColumn.razor
[Parameter] public bool Editable { get; set; } = true;
```

When `Editable` is `false`, the column's cell shows the display value (not the editor template) even when the row is in edit mode. Inline/Popup/InCell modes all respect this.

### Decision: Simple bool parameter
Add `Editable` parameter to `MariloGridColumn`. In `MariloDataGrid.Rendering.cs`, check `column.Editable` before rendering `EditorTemplate`. Popup dialog also checks it.

### Success Criteria
- [ ] `Editable` parameter exists on `MariloGridColumn`, defaults to `true`
- [ ] When `Editable` is `false`, inline editing shows display value instead of editor
- [ ] When `Editable` is `false`, popup dialog shows disabled/read-only field
- [ ] When `Editable` is `false`, InCell double-click does not enter edit mode for that cell

---

## RES-DG-003: Add `ConfirmDelete` parameter

**Resolves:** DG-P1-03
**Status:** Ready for implementation

### Target Pattern

```csharp
// In MariloDataGrid.razor.cs
[Parameter] public bool ConfirmDelete { get; set; }
[Parameter] public string ConfirmDeleteText { get; set; } = "Are you sure you want to delete this item?";
```

When `ConfirmDelete` is `true`, the `DeleteItem` method shows a browser confirmation dialog (via JS interop `confirm()`) before proceeding. This is a production safety feature.

### Decision: Browser `confirm()` via JS interop
The grid already has IJSRuntime injected. Use `JS.InvokeAsync<bool>("confirm", ConfirmDeleteText)` for simplicity. No custom dialog needed.

### Success Criteria
- [ ] `ConfirmDelete` bool parameter exists, defaults to `false`
- [ ] `ConfirmDeleteText` string parameter exists with default message
- [ ] When `ConfirmDelete` is `true`, `DeleteItem` calls JS `confirm()` before firing `OnDelete`
- [ ] When user cancels the confirm dialog, `OnDelete` is NOT fired
- [ ] When `ConfirmDelete` is `false`, existing behavior is unchanged

---

## RES-DG-004: Add `SetStateAsync()` public method

**Resolves:** DG-P1-04
**Status:** Ready for implementation

### Target Pattern

```csharp
// In MariloDataGrid.razor.cs
public async Task SetStateAsync(GridState state)
{
    _state.CurrentPage = state.CurrentPage;
    _state.PageSize = state.PageSize;
    _state.SortDescriptors = state.SortDescriptors ?? [];
    _state.FilterDescriptors = state.FilterDescriptors ?? [];
    _state.GroupDescriptors = state.GroupDescriptors ?? [];
    _state.SearchFilter = state.SearchFilter;
    _searchText = state.SearchFilter ?? "";
    
    if (state.CollapsedGroups != null)
        _collapsedGroups = new HashSet<string>(state.CollapsedGroups);
    
    await ProcessDataAsync();
    StateHasChanged();
}
```

### Decision: Direct state setter
Complements existing `GetState()` method. Applies all state properties and reprocesses data.

### Success Criteria
- [ ] `SetStateAsync(GridState)` public method exists
- [ ] Setting state updates page, sort, filter, group descriptors
- [ ] Setting state triggers data reprocessing
- [ ] Setting state updates search text

---

## RES-DG-005: Add `AddFilter()` and `ClearFilters()` public methods

**Resolves:** DG-P1-05
**Status:** Ready for implementation

### Target Pattern

```csharp
// In MariloDataGrid.Data.cs
public async Task AddFilter(FilterDescriptor filter)
{
    var existing = _state.FilterDescriptors.FirstOrDefault(f => f.Field == filter.Field);
    if (existing != null)
        _state.FilterDescriptors.Remove(existing);
    _state.FilterDescriptors.Add(filter);
    _state.CurrentPage = 1;
    await ProcessDataAsync();
    await NotifyStateChanged("Filter");
    StateHasChanged();
}

public async Task ClearFilters()
{
    _state.FilterDescriptors.Clear();
    _state.CurrentPage = 1;
    await ProcessDataAsync();
    await NotifyStateChanged("Filter");
    StateHasChanged();
}
```

### Decision: Public filter API
Follows existing pattern of `GroupBy()`/`Ungroup()` public methods.

### Success Criteria
- [ ] `AddFilter(FilterDescriptor)` public method exists
- [ ] `ClearFilters()` public method exists
- [ ] `AddFilter` replaces existing filter on same field
- [ ] `ClearFilters` removes all active filters
- [ ] Both methods trigger data reprocessing and state notification

---

## RES-DG-006: Enhanced pager with page number buttons

**Resolves:** DG-P1-06
**Status:** Ready for implementation

### Target Pattern

The current pager shows only "Previous" and "Next" buttons. Enhance to show page number buttons with configurable button count.

```csharp
// In MariloDataGrid.razor.cs
[Parameter] public int PagerButtonCount { get; set; } = 5;
```

Pager renders: `« Previous | 1 | 2 | [3] | 4 | 5 | Next »` with the current page highlighted. When there are more pages than `PagerButtonCount`, show a sliding window centered on the current page.

### Decision: Sliding window pager
Add page number buttons in the `.razor` file between the Previous and Next buttons. Calculate visible page range centered on current page.

### Success Criteria
- [ ] `PagerButtonCount` parameter exists, defaults to 5
- [ ] Page number buttons render between Previous and Next
- [ ] Current page button has active/selected styling
- [ ] When total pages > button count, a sliding window is shown
- [ ] Clicking a page number navigates to that page
- [ ] ARIA labels on all pager buttons

---

## RES-DG-007: Add `DisplayFormat` alias for `Format` column parameter

**Resolves:** DG-P1-07
**Status:** Ready for implementation

### Target Pattern

```csharp
// In MariloGridColumn.razor
[Parameter] public string? DisplayFormat { get; set; }
```

`DisplayFormat` accepts `{0:C2}` style format strings (Telerik convention). `Format` continues to accept short format strings (`C2`). `DisplayFormat` takes precedence when both are set.

### Decision: Add `DisplayFormat` parameter
The `GetDisplayValue` method checks `DisplayFormat` first, then `Format`.

### Success Criteria
- [ ] `DisplayFormat` parameter exists on `MariloGridColumn`
- [ ] `DisplayFormat` accepts `{0:C2}` composite format strings
- [ ] `Format` continues to work with short strings like `C2`
- [ ] `DisplayFormat` takes precedence over `Format` when both are set

---

## RES-DG-008: Add per-column `Groupable` parameter

**Resolves:** DG-P1-08
**Status:** Ready for implementation

### Target Pattern

```csharp
// In MariloGridColumn.razor
[Parameter] public bool Groupable { get; set; } = true;
```

When `Groupable` is `false` on a column, `GroupBy(field)` skips it and it's excluded from drag-to-group UI.

### Decision: Simple bool parameter
Check `column.Groupable` in `GroupBy()` method.

### Success Criteria
- [ ] `Groupable` parameter exists on `MariloGridColumn`, defaults to `true`
- [ ] `GroupBy(field)` respects column's `Groupable` setting
- [ ] Column with `Groupable="false"` cannot be grouped

---

## RES-DG-009: Populate `ExpandedItems` in GridState

**Resolves:** DG-P1-09
**Status:** Ready for implementation

### Target Pattern

The `GetState()` method already returns `ExpandedItems` but it's always empty. Populate it from `_expandedDetailItems`.

```csharp
// In GetState()
ExpandedItems = new HashSet<object>(_expandedDetailItems.Cast<object>()),
```

### Decision: Wire existing state property
Simple one-line fix in `GetState()`.

### Success Criteria
- [ ] `GetState().ExpandedItems` reflects the currently expanded detail rows
- [ ] State change notification fires when detail rows are expanded/collapsed

---

## RES-DG-010: Type expand/collapse event args

**Resolves:** DG-P1-10
**Status:** Ready for implementation

### Target Pattern

Currently `OnRowExpand`/`OnRowCollapse` use `EventCallback<TItem>` directly. Add typed event args for consistency and extensibility.

```csharp
public class GridRowExpandEventArgs<TItem>
{
    public TItem Item { get; init; } = default!;
}
```

**Decision: Defer** — This is a breaking change to the event signature. Current `EventCallback<TItem>` is simpler and functional. Mark as deferred until a breaking-change cycle.

### Success Criteria
- Deferred. No implementation needed now.
