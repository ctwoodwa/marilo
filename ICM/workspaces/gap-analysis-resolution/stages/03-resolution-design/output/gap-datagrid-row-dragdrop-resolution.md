# Resolution Design: DG-P3-03 — Row Drag-and-Drop Reorder

## Target State

Rows can be reordered via drag-and-drop. A drag handle column appears when enabled. The consumer receives an `OnRowDrop` event with source/destination info to update their data collection.

## Design

### Parameters (MariloDataGrid)

```csharp
/// <summary>When true, rows show a drag handle and can be reordered via drag-and-drop.</summary>
[Parameter] public bool RowDraggable { get; set; }

/// <summary>Fires when a row is dropped to a new position. Consumer must reorder their data collection.</summary>
[Parameter] public EventCallback<GridRowDropEventArgs<TItem>> OnRowDrop { get; set; }
```

### Event Args

```csharp
public class GridRowDropEventArgs<TItem>
{
    /// <summary>The item being dragged.</summary>
    public TItem Item { get; set; } = default!;
    
    /// <summary>The item at the drop destination (the row dropped onto).</summary>
    public TItem? DestinationItem { get; set; }
    
    /// <summary>The index in the displayed data where the row was dropped.</summary>
    public int DestinationIndex { get; set; }
    
    /// <summary>Whether dropped before or after the destination item.</summary>
    public GridRowDropPosition DropPosition { get; set; }
    
    /// <summary>Set to true to cancel the drop.</summary>
    public bool IsCancelled { get; set; }
}

public enum GridRowDropPosition { Before, After }
```

### Rendering Changes

1. **`<colgroup>`**: When `RowDraggable`, add a fixed 40px `<col>` for the drag handle column (same pattern as detail/checkbox columns)
2. **`<thead>`**: Add an empty `<th class="mar-datagrid-drag-header">` for the handle column
3. **Filter row**: Add empty `<td>` for handle column
4. **`<tbody>` / `RenderDataRow`**: Prepend a `<td class="mar-datagrid-drag-cell">` with a grip icon (`⠿` or SVG handle)
5. **`<tfoot>`**: Add empty `<td>` for handle column

### JS Changes (IIFE Extension)

Add `initRowDrag()` to the IIFE, called when `RowDraggable` is true:

```javascript
function initRowDrag() {
    const tbody = grid.querySelector('tbody');
    if (!tbody) return;
    
    tbody.addEventListener('dragstart', onRowDragStart);
    tbody.addEventListener('dragover', onRowDragOver);
    tbody.addEventListener('dragleave', onRowDragLeave);
    tbody.addEventListener('drop', onRowDrop);
    tbody.addEventListener('dragend', onRowDragEnd);
}
```

Each drag handle `<td>` gets `draggable="true"` and the `dragstart` event sets `dataTransfer` with the row index. Drop highlighting uses `mar-datagrid-row--drop-before` and `mar-datagrid-row--drop-after` CSS classes to show insertion indicator.

The JS fires `dotNetRef.invokeMethodAsync('OnRowDropped', sourceIndex, destIndex, dropPosition)`.

### C# Callback

```csharp
[JSInvokable]
public async Task OnRowDropped(int sourceIndex, int destIndex, string dropPosition)
{
    var displayedItems = GetDisplayedItems(); // current page/filtered/sorted view
    if (sourceIndex < 0 || sourceIndex >= displayedItems.Count) return;
    
    var args = new GridRowDropEventArgs<TItem>
    {
        Item = displayedItems[sourceIndex],
        DestinationItem = destIndex >= 0 && destIndex < displayedItems.Count ? displayedItems[destIndex] : default,
        DestinationIndex = destIndex,
        DropPosition = dropPosition == "after" ? GridRowDropPosition.After : GridRowDropPosition.Before
    };
    
    await OnRowDrop.InvokeAsync(args);
    // Grid does NOT reorder data itself — consumer must update Data/call Rebind
}
```

### Interop Init Guard

Update the init condition in `OnAfterRenderAsync`:
```csharp
if (_resizable || _reorderable || Navigable || RowDraggable)
```

Pass `rowDraggable` in the options object:
```csharp
await _jsModule.InvokeVoidAsync("init", _gridId, _dotNetRef, new
{
    resizable = _resizable,
    reorderable = _reorderable,
    navigable = Navigable,
    rowDraggable = RowDraggable
});
```

### Tests

- RowDraggable=false (default): no drag handle column rendered
- RowDraggable=true: drag handle column appears in colgroup, thead, tbody
- GridRowDropEventArgs has correct property types
- OnRowDrop callback fires (via mock JSInvokable)

## Decision Rationale

- **HTML5 Drag and Drop API** rather than pointer events: consistent with existing column reorder pattern in the same IIFE, simpler browser compat
- **Consumer reorders data** rather than grid auto-reorder: consistent with Marilo's data-not-owned pattern (OnRead, OnSort, etc.)
- **Drag handle column** rather than full-row draggable: avoids conflicting with text selection and cell interactions
