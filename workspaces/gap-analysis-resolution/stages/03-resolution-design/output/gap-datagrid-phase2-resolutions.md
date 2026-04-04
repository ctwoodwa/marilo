# Resolution Records: MariloDataGrid Phase 2 — Pure C# Important Gaps

> Date: 2026-04-04
> Source: `stages/02-prioritize/output/gap-datagrid-backlog.md`
> Component: `MariloDataGrid<TItem>` — `src/Marilo.Components/DataGrid/`

---

## RES-DG-011: DataAnnotations validation integration

**Resolves:** DG-P2-03
**Status:** Ready for implementation

### Target Pattern

Wrap the popup edit form in an `EditForm` with `DataAnnotationsValidator` so that `[Required]`, `[StringLength]`, `[Range]`, etc. attributes on `TItem` properties are enforced. Block save when validation fails.

```razor
@* In popup dialog body *@
<EditForm Model="_editingItem" OnValidSubmit="SaveEdit">
    <DataAnnotationsValidator />
    @foreach (var column in _visibleColumns)
    {
        <div class="mar-datagrid-popup-field">
            <label>@column.DisplayTitle</label>
            @if (column.Editable && column.EditorTemplate != null)
            {
                @column.EditorTemplate(_editingItem)
            }
            else
            {
                <input type="text" value="@column.GetDisplayValue(_editingItem)" disabled />
            }
            <ValidationMessage For="@(() => GetPropertyExpression(column.Field))" />
        </div>
    }
    <div class="mar-datagrid-popup-actions">
        <button type="submit" class="mar-datagrid-cmd-btn">Save</button>
        <button type="button" class="mar-datagrid-cmd-btn" @onclick="CancelEdit">Cancel</button>
    </div>
</EditForm>
```

### Decision

Integrate `EditForm`/`DataAnnotationsValidator` in popup mode only for Phase 2. Inline/InCell validation is more complex and deferred. Add a `ValidateOnSave` parameter (default `true` when `EditMode == Popup`) that gates validation.

### Complexity Note

Full `ValidationMessage<T>` integration requires `Expression<Func<T>>` for each field, which is complex with reflection-based columns. For Phase 2, add EditForm wrapping and prevent invalid submits. Per-field validation messages require `EditorTemplate` authors to include their own `<ValidationMessage>`.

### Simplified Approach for Phase 2

1. Add `EditForm` wrapper around popup body with `OnValidSubmit` binding
2. Add `DataAnnotationsValidator` inside the form
3. Add `ValidationSummary` component for field-agnostic error display
4. Change Save button to `type="submit"` so it triggers validation
5. `SaveEdit` is only called via `OnValidSubmit`, so invalid data is blocked

### Success Criteria
- [ ] Popup edit form is wrapped in `EditForm` with `DataAnnotationsValidator`
- [ ] `ValidationSummary` displays validation errors
- [ ] Save button is `type="submit"` and only fires when model is valid
- [ ] Cancel button bypasses validation
- [ ] Non-popup modes continue to work without validation (unchanged)

---

## RES-DG-012: Composite filter descriptors (AND/OR)

**Resolves:** DG-P2-04
**Status:** Ready for implementation

### Target Pattern

Add a `CompositeFilterDescriptor` that groups multiple `FilterDescriptor` items with a `LogicalOperator` (And/Or).

```csharp
// In Marilo.Core.Data
public class CompositeFilterDescriptor
{
    public FilterCompositionOperator LogicalOperator { get; set; } = FilterCompositionOperator.And;
    public List<FilterDescriptor> Filters { get; set; } = [];
}

// In Marilo.Core.Enums
public enum FilterCompositionOperator { And, Or }
```

Add `CompositeFilterDescriptors` list to `GridState`. The filter pipeline applies composites after individual filters.

### Decision

Add the model classes and wire them into `ProcessDataClientSide`. The filter menu UI enhancement (to allow AND/OR selection) is deferred — consumers can use `AddCompositeFilter()` programmatically.

### Success Criteria
- [ ] `CompositeFilterDescriptor` class with `LogicalOperator` and `Filters` list
- [ ] `FilterCompositionOperator` enum (And, Or)
- [ ] `CompositeFilterDescriptors` list in `GridState`
- [ ] `AddCompositeFilter()` / `ClearCompositeFilters()` public methods
- [ ] AND composites require all filters to match
- [ ] OR composites require any filter to match

---

## RES-DG-013: Auto-generate columns with [Display]/[Editable] attributes

**Resolves:** DG-P2-05
**Status:** Ready for implementation

### Target Pattern

Enhance `GenerateColumnsFromModel` to respect `System.ComponentModel.DataAnnotations` attributes:
- `[Display(Name = "...")]` → column `Title`
- `[Display(Order = N)]` → column ordering
- `[Display(AutoGenerateField = false)]` → skip column
- `[Editable(false)]` → column `Editable = false`

### Decision: Enhance existing method

```csharp
var displayAttr = prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>();
var editableAttr = prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.EditableAttribute>();

if (displayAttr?.GetAutoGenerateField() == false) continue;

var title = displayAttr?.GetName() ?? SplitCamelCase(prop.Name);
var order = displayAttr?.GetOrder();
var editable = editableAttr?.AllowEdit ?? true;
```

### Success Criteria
- [ ] `[Display(Name)]` sets column Title
- [ ] `[Display(AutoGenerateField = false)]` skips column
- [ ] `[Display(Order)]` controls column ordering
- [ ] `[Editable(false)]` sets column Editable = false
- [ ] Without attributes, existing behavior (CamelCase split) is preserved

---

## RES-DG-014: Group aggregate functions

**Resolves:** DG-P2-06
**Status:** Ready for implementation

### Target Pattern

Add aggregate computation to `GridGroupHeaderContext<TItem>` so `GroupHeaderTemplate` and `GroupFooterTemplate` can display aggregates.

```csharp
// In GridGroupHeaderContext<TItem>
public decimal Sum(Func<TItem, decimal> selector) => Items.Sum(selector);
public decimal Average(Func<TItem, decimal> selector) => Items.Average(selector);
public TResult Min<TResult>(Func<TItem, TResult> selector) => Items.Min(selector)!;
public TResult Max<TResult>(Func<TItem, TResult> selector) => Items.Max(selector)!;
```

### Decision: Extension methods on the context

Add aggregate helper methods directly to `GridGroupHeaderContext<TItem>`. This avoids new types and lets template authors call `context.Sum(x => x.Salary)` directly.

### Success Criteria
- [ ] `Sum`, `Average`, `Min`, `Max` methods on `GridGroupHeaderContext<TItem>`
- [ ] `Count` already exists (via `Items.Count`)
- [ ] Methods work in `GroupHeaderTemplate` and `GroupFooterTemplate`
- [ ] Type-safe with generic selectors

---

## RES-DG-015: Export lifecycle events and ExportAllPages

**Resolves:** DG-P2-07
**Status:** Ready for implementation

### Target Pattern

```csharp
[Parameter] public EventCallback<GridExportEventArgs> OnBeforeExport { get; set; }
[Parameter] public EventCallback<GridExportEventArgs> OnAfterExport { get; set; }
[Parameter] public bool ExportAllPages { get; set; } = true;

public class GridExportEventArgs
{
    public string Format { get; init; } = "csv";
    public bool IsCancelled { get; set; }
    public string? Data { get; set; }
    public int RowCount { get; set; }
}
```

### Decision

Add lifecycle events and `ExportAllPages` parameter to `ExportToCsv`. When `ExportAllPages` is false, export only the current page.

### Success Criteria
- [ ] `OnBeforeExport` fires before export begins; can cancel
- [ ] `OnAfterExport` fires after export completes
- [ ] `ExportAllPages` defaults to true (current behavior)
- [ ] When `ExportAllPages` is false, only current page data is exported

---

## RES-DG-016: CancellationToken in GridReadEventArgs

**Resolves:** DG-P2-08
**Status:** Ready for implementation

### Target Pattern

```csharp
// In GridReadEventArgs<TItem>
public CancellationToken CancellationToken { get; init; }
```

Grid creates a `CancellationTokenSource` per data request. If a new request starts before the previous completes, the old token is cancelled.

### Decision

Add `_currentCts` field, cancel on new request. Pass token in event args.

### Success Criteria
- [ ] `CancellationToken` property on `GridReadEventArgs<TItem>`
- [ ] Previous request's token is cancelled when a new request starts
- [ ] Token is usable in consumer's `OnRead` handler
