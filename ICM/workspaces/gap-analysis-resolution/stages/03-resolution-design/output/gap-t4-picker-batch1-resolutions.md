# T4 Picker Batch 1 — Resolution Records

> Batch scope: 10 high-severity gaps across 5 components (MariloMultiSelect, MariloDateTimePicker, MariloDateRangePicker, MariloTimePicker, MariloUpload)
> Date: 2026-04-03
> Stage: 03-resolution-design

---

### RES-T4B1-001: Shared PopupEventArgs with cancellation support

**Resolves:** GAP-T4X-003, GAP-TP-001, GAP-DRP-002 (event args portion), GAP-DTP-001 (event args portion), GAP-MSEL-001 (event args portion)
**Status:** Proposed

#### Target Pattern

A shared `PopupEventArgs` class in `Marilo.Core.Models` that all popup-bearing components use for `OnOpen` and `OnClose` events. Mirrors the existing `ColorPickerOpenEventArgs` / `ColorPickerCloseEventArgs` pattern already in the codebase but generalizes it.

```csharp
// File: src/Marilo.Core/Models/PopupEventArgs.cs
namespace Marilo.Core.Models;

/// <summary>
/// Event arguments for popup lifecycle events (OnOpen, OnClose).
/// Set <see cref="IsCancelled"/> to <c>true</c> in the handler to prevent
/// the popup from opening or closing.
/// </summary>
public class PopupEventArgs
{
    /// <summary>
    /// Set to <c>true</c> to cancel the popup transition.
    /// When cancelled during OnOpen, the popup remains closed.
    /// When cancelled during OnClose, the popup remains open.
    /// </summary>
    public bool IsCancelled { get; set; }
}
```

Consumer usage in any picker:

```csharp
[Parameter] public EventCallback<PopupEventArgs> OnOpen { get; set; }
[Parameter] public EventCallback<PopupEventArgs> OnClose { get; set; }

private async Task OpenPopupAsync()
{
    var args = new PopupEventArgs();
    await OnOpen.InvokeAsync(args);
    if (args.IsCancelled) return;
    _isOpen = true;
    StateHasChanged();
}

private async Task ClosePopupAsync()
{
    var args = new PopupEventArgs();
    await OnClose.InvokeAsync(args);
    if (args.IsCancelled) return;
    _isOpen = false;
    StateHasChanged();
}
```

#### Options Considered

| Option | Description | Pros | Cons |
|--------|-------------|------|------|
| **A: Shared PopupEventArgs** | Single class in `Marilo.Core.Models`, used by all 5 popup components | One type to learn; consistent API; matches existing `FormUpdateEventArgs` pattern of shared args | Cannot add component-specific properties without casting or subclassing |
| **B: Per-component event args** | `MultiSelectOpenEventArgs`, `DateTimePickerOpenEventArgs`, etc. (like the existing `ColorPickerOpenEventArgs` pattern) | Each component can carry custom context | Proliferates nearly-identical classes; consumers switching between components must learn different types |

#### Decision

**Option A — Shared PopupEventArgs.** All five picker components have identical popup lifecycle needs (open, close, cancellable). Component-specific context (e.g., which date cell triggered the open) belongs in separate, dedicated events — not in the popup lifecycle args. The existing `ColorPickerOpenEventArgs` and `ColorPickerCloseEventArgs` will be retained for backward compatibility but can be deprecated in favor of `PopupEventArgs` in a future major version.

#### Consequences

- All new popup components will use `PopupEventArgs` from day one.
- Existing `ColorPickerOpenEventArgs` / `ColorPickerCloseEventArgs` remain unchanged; no breaking change.
- If a future component needs extra properties on open/close, it subclasses `PopupEventArgs` rather than creating an unrelated type.

#### Success Criteria

- [ ] `PopupEventArgs` class exists in `src/Marilo.Core/Models/PopupEventArgs.cs`
- [ ] `IsCancelled` property prevents popup transition when set to `true` in handler
- [ ] All 5 popup components (`MariloMultiSelect`, `MariloDateTimePicker`, `MariloDateRangePicker`, `MariloTimePicker`, `MariloUpload`) use `EventCallback<PopupEventArgs>` for `OnOpen` / `OnClose`
- [ ] Unit test: setting `IsCancelled = true` in `OnOpen` handler keeps `_isOpen == false`
- [ ] Unit test: setting `IsCancelled = true` in `OnClose` handler keeps `_isOpen == true`

---

### RES-T4B1-002: MariloMultiSelect core events

**Resolves:** GAP-MSEL-001
**Status:** Proposed

#### Target Pattern

Add six event parameters to `MariloMultiSelect.razor` (currently at ~517 lines, no code-behind). The component already has `ValueChanged` and `OnFilter`; these additions complete the event surface.

```csharp
// Added to @code block in MariloMultiSelect.razor

/// <summary>Fires after the selection changes. Provides the full selected items list.</summary>
[Parameter] public EventCallback<IEnumerable<TItem>> OnChange { get; set; }

/// <summary>Fires when the component requests data (for virtualization / remote binding).</summary>
[Parameter] public EventCallback<MultiSelectReadEventArgs> OnRead { get; set; }

/// <summary>Fires before the popup opens. Set IsCancelled to prevent.</summary>
[Parameter] public EventCallback<PopupEventArgs> OnOpen { get; set; }

/// <summary>Fires before the popup closes. Set IsCancelled to prevent.</summary>
[Parameter] public EventCallback<PopupEventArgs> OnClose { get; set; }

/// <summary>Fires for each item as it renders in the dropdown. Allows per-item CSS/disabled.</summary>
[Parameter] public EventCallback<MultiSelectItemRenderEventArgs<TItem>> OnItemRender { get; set; }

/// <summary>Fires when the input loses focus.</summary>
[Parameter] public EventCallback OnBlur { get; set; }
```

Supporting event args:

```csharp
// File: src/Marilo.Core/Models/MultiSelectModels.cs
namespace Marilo.Core.Models;

public class MultiSelectReadEventArgs
{
    /// <summary>Current filter text entered by the user.</summary>
    public string? Filter { get; set; }

    /// <summary>Set this to the items to display.</summary>
    public IEnumerable<object>? Data { get; set; }

    /// <summary>Total count for virtualization.</summary>
    public int? Total { get; set; }
}

public class MultiSelectItemRenderEventArgs<TItem>
{
    /// <summary>The item being rendered.</summary>
    public TItem Item { get; set; } = default!;

    /// <summary>Additional CSS class(es) to apply to this item.</summary>
    public string? Class { get; set; }

    /// <summary>Set to true to disable this item in the dropdown.</summary>
    public bool Disabled { get; set; }
}
```

Integration points in existing markup:
- `OnOpen` / `OnClose`: wrap the existing `TogglePopup()` method with cancellation check (uses `PopupEventArgs` from RES-T4B1-001)
- `OnChange`: fire after `ValueChanged` in the selection handler
- `OnBlur`: attach to the root input's `@onfocusout`
- `OnItemRender`: invoke in the item render loop (`@foreach` at ~line 110)
- `OnRead`: invoke in `FilterItemsAsync()` when bound, replacing local filter

#### Options Considered

| Option | Description | Pros | Cons |
|--------|-------------|------|------|
| **A: Add events inline in .razor** | All parameters and handlers in the single `@code` block | Matches current component structure (no code-behind exists) | File grows from ~517 to ~600 lines |
| **B: Extract to code-behind .razor.cs** | Move `@code` block to `MariloMultiSelect.razor.cs` | Cleaner separation | Breaks consistency with other picker components that have no code-behind; adds a file |

#### Decision

**Option A — Inline in .razor.** Maintaining consistency with the existing component structure. The component stays under 650 lines, which is manageable. Code-behind extraction can be a separate refactor if the file grows beyond ~800 lines.

#### Consequences

- `OnFilter` remains as-is for backward compatibility; `OnRead` is additive for remote-data scenarios.
- `OnChange` fires in addition to `ValueChanged` (two-way binding). Consumers can use either or both.
- `OnItemRender` fires synchronously during render; heavy async work should not be done in the handler.

#### Success Criteria

- [ ] `OnChange` fires with current selection after add/remove
- [ ] `OnRead` fires during filter when bound, and populates dropdown from `Data` property
- [ ] `OnOpen` / `OnClose` use `PopupEventArgs` and respect `IsCancelled`
- [ ] `OnItemRender` applies returned `Class` and `Disabled` to the rendered item element
- [ ] `OnBlur` fires when the component root loses focus
- [ ] Existing `ValueChanged` and `OnFilter` behavior unchanged (no regression)

---

### RES-T4B1-003: MariloDateTimePicker spec events

**Resolves:** GAP-DTP-001
**Status:** Proposed

#### Target Pattern

Add five event parameters to `MariloDateTimePicker.razor` (~383 lines). The component currently has `ValueChanged` and `OnConfirm`.

```csharp
// Added to @code block in MariloDateTimePicker.razor

/// <summary>Fires when the value changes (date or time portion). Provides the new DateTime? value.</summary>
[Parameter] public EventCallback<DateTime?> OnChange { get; set; }

/// <summary>Fires before the popup opens. Set IsCancelled to prevent.</summary>
[Parameter] public EventCallback<PopupEventArgs> OnOpen { get; set; }

/// <summary>Fires before the popup closes. Set IsCancelled to prevent.</summary>
[Parameter] public EventCallback<PopupEventArgs> OnClose { get; set; }

/// <summary>Fires when the input loses focus.</summary>
[Parameter] public EventCallback OnBlur { get; set; }

/// <summary>Fires for each calendar day cell during render. Allows custom CSS/disabled per date.</summary>
[Parameter] public EventCallback<CalendarCellRenderEventArgs> OnCalendarCellRender { get; set; }
```

Supporting event args:

```csharp
// File: src/Marilo.Core/Models/CalendarCellRenderEventArgs.cs
namespace Marilo.Core.Models;

/// <summary>
/// Event arguments for calendar cell rendering customization.
/// Shared by MariloDateTimePicker, MariloDateRangePicker, and future calendar components.
/// </summary>
public class CalendarCellRenderEventArgs
{
    /// <summary>The date represented by this cell.</summary>
    public DateTime Date { get; set; }

    /// <summary>Additional CSS class(es) to apply to the cell.</summary>
    public string? Class { get; set; }

    /// <summary>Set to true to disable selection of this date.</summary>
    public bool Disabled { get; set; }

    /// <summary>Whether the date is in the currently displayed month.</summary>
    public bool IsOtherMonth { get; set; }
}
```

Integration points:
- `OnOpen` / `OnClose`: wrap the popup toggle logic (~line 170) with `PopupEventArgs` cancellation
- `OnChange`: fire after `ValueChanged` in the value-set path; `OnConfirm` is retained as-is (fires only on explicit confirm button click)
- `OnBlur`: attach to the root `<div>` `@onfocusout`
- `OnCalendarCellRender`: invoke in the day-cell render loop inside the calendar grid

#### Options Considered

| Option | Description | Pros | Cons |
|--------|-------------|------|------|
| **A: OnChange alongside OnConfirm** | Both events coexist — `OnChange` fires on any value mutation, `OnConfirm` fires on explicit confirm | No breaking change; consumers pick the event that fits their UX | Two events for similar purpose may confuse |
| **B: Replace OnConfirm with OnChange** | Remove `OnConfirm`, use `OnChange` only | Simpler API | Breaking change for existing consumers |

#### Decision

**Option A — Coexistence.** `OnConfirm` has distinct semantics (user explicitly pressed Confirm) vs `OnChange` (value changed, possibly from keyboard or programmatic). Both are useful. XML doc comments will clarify the distinction.

#### Consequences

- Consumers upgrading from older versions keep `OnConfirm` working unchanged.
- `CalendarCellRenderEventArgs` is shared with `MariloDateRangePicker` (RES-T4B1-004), avoiding duplication.

#### Success Criteria

- [ ] `OnChange` fires with `DateTime?` when value changes by any means
- [ ] `OnConfirm` continues to fire only on explicit confirm action (no regression)
- [ ] `OnOpen` / `OnClose` use `PopupEventArgs` and respect `IsCancelled`
- [ ] `OnBlur` fires on focus loss
- [ ] `OnCalendarCellRender` applies `Class` and `Disabled` to calendar day cells
- [ ] Unit test: disabling a date via `OnCalendarCellRender` prevents selection

---

### RES-T4B1-004: MariloDateRangePicker events

**Resolves:** GAP-DRP-002
**Status:** Proposed

#### Target Pattern

Add three event parameters to `MariloDateRangePicker.razor` (~486 lines). The component currently has `StartValueChanged`, `EndValueChanged`, and `OnRangeChanged` (no args).

```csharp
// Added to @code block in MariloDateRangePicker.razor

/// <summary>
/// Fires when the date range changes. Provides typed args with Start and End values.
/// Replaces the argument-less OnRangeChanged for consumers who need the values.
/// </summary>
[Parameter] public EventCallback<DateRangeChangedEventArgs> OnChange { get; set; }

/// <summary>Fires before the popup opens. Set IsCancelled to prevent.</summary>
[Parameter] public EventCallback<PopupEventArgs> OnOpen { get; set; }

/// <summary>Fires before the popup closes. Set IsCancelled to prevent.</summary>
[Parameter] public EventCallback<PopupEventArgs> OnClose { get; set; }
```

Supporting event args:

```csharp
// File: src/Marilo.Core/Models/DateRangeModels.cs
namespace Marilo.Core.Models;

public class DateRangeChangedEventArgs
{
    /// <summary>The start date of the range.</summary>
    public DateTime? Start { get; set; }

    /// <summary>The end date of the range.</summary>
    public DateTime? End { get; set; }
}
```

Integration points:
- `OnOpen` / `OnClose`: wrap popup toggle at ~line 50 with `PopupEventArgs` cancellation (the popup markup is at line 63 where `PopupClass` is already applied)
- `OnChange`: fire after `StartValueChanged` / `EndValueChanged` in the range-set handler; `OnRangeChanged` retained for backward compatibility
- Existing `NavigateTo()` and `Refresh()` public methods unaffected

#### Options Considered

| Option | Description | Pros | Cons |
|--------|-------------|------|------|
| **A: Add OnChange with typed args, keep OnRangeChanged** | Both events coexist | No breaking change; typed args are additive | Slight redundancy |
| **B: Add args to OnRangeChanged directly** | Change `EventCallback OnRangeChanged` to `EventCallback<DateRangeChangedEventArgs> OnRangeChanged` | Single event, cleaner | Breaking change for consumers using the parameterless handler signature |

#### Decision

**Option A — Additive OnChange.** Changing the signature of `OnRangeChanged` is a binary-breaking change. Adding `OnChange` with typed args provides the capability without breaking existing consumers. `OnRangeChanged` can be marked `[Obsolete]` in a future version.

#### Consequences

- Both `OnRangeChanged` (parameterless) and `OnChange` (typed) fire when the range changes. The firing order is `OnChange` then `OnRangeChanged`.
- `OnOpen` / `OnClose` share `PopupEventArgs` from RES-T4B1-001.

#### Success Criteria

- [ ] `OnChange` fires with `DateRangeChangedEventArgs` containing correct `Start` and `End`
- [ ] `OnRangeChanged` continues to fire (no regression)
- [ ] `OnOpen` / `OnClose` use `PopupEventArgs` and respect `IsCancelled`
- [ ] Unit test: cancelling `OnOpen` keeps the popup closed

---

### RES-T4B1-005: MariloTimePicker PopupClass bug fix + cancellable events

**Resolves:** GAP-TP-003, GAP-TP-001
**Status:** Proposed

#### Target Pattern

Two changes to `MariloTimePicker.razor` (~532 lines):

**Bug fix — PopupClass not applied to markup (line 58):**

```razor
@* BEFORE (line 58): *@
<div class="mar-timepicker__popup"
     role="dialog"
     aria-label="Choose time"

@* AFTER: *@
<div class="mar-timepicker__popup @PopupClass"
     role="dialog"
     aria-label="Choose time"
```

**Event upgrade — cancellable OnOpen/OnClose:**

The component currently declares `OnOpen` and `OnClose` as `EventCallback` (fire-and-forget, no args). Upgrade them to use `PopupEventArgs`:

```csharp
// BEFORE (current):
[Parameter] public EventCallback OnOpen { get; set; }
[Parameter] public EventCallback OnClose { get; set; }

// AFTER:
[Parameter] public EventCallback<PopupEventArgs> OnOpen { get; set; }
[Parameter] public EventCallback<PopupEventArgs> OnClose { get; set; }
```

Update the open/close methods:

```csharp
// BEFORE:
private async Task OpenAsync()
{
    _isOpen = true;
    await OnOpen.InvokeAsync();
}

// AFTER:
private async Task OpenAsync()
{
    var args = new PopupEventArgs();
    await OnOpen.InvokeAsync(args);
    if (args.IsCancelled) return;
    _isOpen = true;
    StateHasChanged();
}

private async Task CloseAsync()
{
    var args = new PopupEventArgs();
    await OnClose.InvokeAsync(args);
    if (args.IsCancelled) return;
    _isOpen = false;
    StateHasChanged();
}
```

#### Options Considered

| Option | Description | Pros | Cons |
|--------|-------------|------|------|
| **A: Change EventCallback to EventCallback\<PopupEventArgs\>** | Upgrade the existing parameter type | Clean API; no duplicate events | Source-breaking if consumer has `void HandleOpen()` — must change to `void HandleOpen(PopupEventArgs args)` |
| **B: Add new OnOpenCancellable / OnCloseCancellable, keep old ones** | Additive parameters | No breaking change | Cluttered API; two events for same lifecycle |

#### Decision

**Option A — In-place upgrade.** The `EventCallback<PopupEventArgs>` is source-compatible with `async Task Handler(PopupEventArgs args)` lambdas. Consumers using the parameterless `() => { }` lambda syntax will need a minor update (`(args) => { }`), but this is a trivial migration. The TimePicker is relatively new and adoption is low, so the break is acceptable. Release notes will document the migration.

#### Consequences

- **Breaking change** for consumers using parameterless `OnOpen` / `OnClose` handlers. Migration is mechanical: add the `PopupEventArgs` parameter.
- PopupClass bug fix is a pure correction — no API change, just markup alignment.
- After this resolution, `MariloTimePicker` is aligned with all other picker popup patterns.

#### Success Criteria

- [ ] `PopupClass` value appears in the rendered popup `<div>` class attribute
- [ ] `OnOpen` handler receives `PopupEventArgs`; setting `IsCancelled = true` prevents popup from opening
- [ ] `OnClose` handler receives `PopupEventArgs`; setting `IsCancelled = true` prevents popup from closing
- [ ] Existing `ValueChanged`, `OnChange`, `OnBlur` behavior unchanged
- [ ] Visual test: popup element has custom class when `PopupClass="my-custom-class"` is set

---

### RES-T4B1-006: MariloUpload chunk resume fix

**Resolves:** GAP-UPL-004
**Status:** Proposed

#### Target Pattern

Fix the chunk upload resume logic in `MariloUpload.razor.cs` (~635 lines code-behind). Currently, when a paused upload is resumed, it always restarts from byte 0. The fix tracks the last successfully uploaded byte offset per file.

```csharp
// File: src/Marilo.Components/Forms/Inputs/MariloUpload.razor.cs

// Add to the per-file tracking state (likely a dictionary or the UploadFileInfo model):
private readonly Dictionary<string, long> _pausedByteOffsets = new();

// In the Pause handler:
private async Task PauseUploadAsync(UploadFileInfo file)
{
    _pausedByteOffsets[file.Id] = file.UploadedBytes;  // track current progress
    file.Status = UploadStatus.Paused;
    await OnPause.InvokeAsync(new UploadPauseEventArgs { File = file });
}

// In the Resume handler:
private async Task ResumeUploadAsync(UploadFileInfo file)
{
    long startByte = _pausedByteOffsets.GetValueOrDefault(file.Id, 0);
    file.Status = UploadStatus.Uploading;
    await OnResume.InvokeAsync(new UploadResumeEventArgs { File = file });
    await UploadChunksAsync(file, startByte);  // pass offset to chunk loop
}

// In the chunk upload loop:
private async Task UploadChunksAsync(UploadFileInfo file, long startByte = 0)
{
    long offset = startByte;  // <-- was previously always 0
    while (offset < file.Size && file.Status == UploadStatus.Uploading)
    {
        int chunkSize = (int)Math.Min(ChunkSize, file.Size - offset);
        var chunk = await ReadChunkAsync(file, offset, chunkSize);

        // Send chunk with Content-Range header
        await SendChunkAsync(file, chunk, offset, chunkSize);

        offset += chunkSize;
        file.UploadedBytes = offset;
        await OnProgress.InvokeAsync(new UploadProgressEventArgs
        {
            File = file,
            Progress = (double)offset / file.Size * 100
        });
    }
}
```

#### Options Considered

| Option | Description | Pros | Cons |
|--------|-------------|------|------|
| **A: Track offset in separate dictionary** | `Dictionary<string, long> _pausedByteOffsets` alongside existing state | Minimal change to existing model classes | Extra state to clean up on remove/clear |
| **B: Add PausedByteOffset property to UploadFileInfo** | Extend the model with `public long PausedByteOffset { get; set; }` | Self-contained per file; auto-cleaned when file removed | Modifies a public model (additive, non-breaking) |

#### Decision

**Option B — Add to UploadFileInfo.** The byte offset is intrinsically per-file state. Adding it to the model keeps the resume logic self-contained and avoids synchronization issues between the dictionary and file lifecycle. The property is additive (non-breaking).

```csharp
// In UploadModels.cs, add to UploadFileInfo:
/// <summary>
/// Byte offset from which to resume a paused chunked upload.
/// Automatically set when the upload is paused.
/// </summary>
public long PausedByteOffset { get; set; }
```

#### Consequences

- Paused uploads resume from the correct offset, avoiding re-uploading already-transferred data.
- The server must support `Content-Range` headers for partial uploads (this is already implied by the chunked upload design).
- `PausedByteOffset` is reset to 0 when a file is removed, cleared, or upload completes.

#### Success Criteria

- [ ] `UploadFileInfo.PausedByteOffset` property exists in `UploadModels.cs`
- [ ] Pausing at 50% and resuming sends chunks starting from the 50% byte offset, not from 0
- [ ] `OnProgress` reports correct percentage after resume (continues from paused %, not 0%)
- [ ] Clearing or removing a file resets `PausedByteOffset` to 0
- [ ] Unit test: pause at byte 1024, resume, verify first chunk request starts at offset 1024

---

### RES-T4B1-007: MariloMultiSelect AllowCustom parameter

**Resolves:** GAP-MSEL-002
**Status:** Proposed

#### Target Pattern

Add an `AllowCustom` parameter to `MariloMultiSelect.razor` that permits users to enter values not present in the bound data source.

```csharp
// Added to @code block in MariloMultiSelect.razor

/// <summary>
/// When true, the user can type a custom value that is not in the data source
/// and add it to the selection by pressing Enter or the delimiter key.
/// </summary>
[Parameter] public bool AllowCustom { get; set; }
```

Behavioral changes in the filter/input handler:

```csharp
private async Task HandleKeyDown(KeyboardEventArgs e)
{
    if (e.Key == "Enter" && AllowCustom && !string.IsNullOrWhiteSpace(_filterText))
    {
        var customItem = CreateCustomItem(_filterText);
        if (customItem is not null && !SelectedItems.Contains(customItem))
        {
            var updated = SelectedItems.Append(customItem).ToList();
            await ValueChanged.InvokeAsync(updated);
            await OnChange.InvokeAsync(updated);
        }
        _filterText = string.Empty;
    }
}
```

The `CreateCustomItem` method depends on the `TItem` type:
- For `string`-typed MultiSelect: returns the filter text directly.
- For complex types: requires a `CustomItemFactory` parameter.

```csharp
/// <summary>
/// Factory function to create a TItem from a custom text entry.
/// Required when AllowCustom is true and TItem is not string.
/// </summary>
[Parameter] public Func<string, TItem>? CustomItemFactory { get; set; }

private TItem? CreateCustomItem(string text)
{
    if (typeof(TItem) == typeof(string))
        return (TItem)(object)text;

    if (CustomItemFactory is not null)
        return CustomItemFactory(text);

    return default;
}
```

#### Options Considered

| Option | Description | Pros | Cons |
|--------|-------------|------|------|
| **A: AllowCustom + CustomItemFactory** | Bool flag + optional factory for complex types | Works for both string and complex TItem; explicit contract | Two parameters for one feature |
| **B: AllowCustom only, auto-convert via reflection** | Attempt to construct TItem from string via TypeConverter | Single parameter | Fragile; fails silently for types without string constructor; magic behavior |

#### Decision

**Option A — AllowCustom + CustomItemFactory.** Explicit is better than implicit. String-typed MultiSelects work with just `AllowCustom="true"`. Complex types require the factory, and omitting it when needed throws a clear `InvalidOperationException` at runtime.

#### Consequences

- String-typed `MariloMultiSelect<string>` gets custom entry with a single parameter.
- Complex-typed selects require `CustomItemFactory` when `AllowCustom` is true.
- Custom items appear in the tag list and are included in `ValueChanged` / `OnChange` payloads.
- The filter input clears after a custom item is added.

#### Success Criteria

- [ ] `AllowCustom="true"` on `MariloMultiSelect<string>` allows typing and pressing Enter to add a tag
- [ ] Custom items appear in the selected items collection and render as tags
- [ ] Duplicate custom entries are rejected (no duplicates in selection)
- [ ] `CustomItemFactory` is invoked for non-string `TItem` types
- [ ] `InvalidOperationException` thrown if `AllowCustom` is true, `TItem` is not string, and `CustomItemFactory` is null
- [ ] Existing non-custom selection behavior unchanged

---

### RES-T4B1-008: MariloMultiSelect template slots

**Resolves:** GAP-MSEL-004
**Status:** Proposed

#### Target Pattern

Add five `RenderFragment` template parameters to `MariloMultiSelect.razor` for customizing the visual presentation of tags, popup header/footer, and empty state.

```csharp
// Added to @code block in MariloMultiSelect.razor

/// <summary>Template for the summary area showing selected count (e.g., "3 items selected").</summary>
[Parameter] public RenderFragment<MultiSelectSummaryContext<TItem>>? SummaryTagTemplate { get; set; }

/// <summary>Template for individual tags in the input area.</summary>
[Parameter] public RenderFragment<TItem>? TagTemplate { get; set; }

/// <summary>Rendered at the top of the popup dropdown, above the item list.</summary>
[Parameter] public RenderFragment? HeaderTemplate { get; set; }

/// <summary>Rendered at the bottom of the popup dropdown, below the item list.</summary>
[Parameter] public RenderFragment? FooterTemplate { get; set; }

/// <summary>Rendered inside the popup when the filtered item list is empty.</summary>
[Parameter] public RenderFragment? NoDataTemplate { get; set; }
```

Supporting context class:

```csharp
// File: src/Marilo.Core/Models/MultiSelectModels.cs (append to existing file from RES-T4B1-002)

public class MultiSelectSummaryContext<TItem>
{
    /// <summary>The full list of currently selected items.</summary>
    public IReadOnlyList<TItem> SelectedItems { get; set; } = [];

    /// <summary>Total number of selected items.</summary>
    public int Count => SelectedItems.Count;
}
```

Markup integration (in `MariloMultiSelect.razor`):

```razor
@* Tag area (~line 85) — replace default tag rendering: *@
@foreach (var item in SelectedItems)
{
    @if (TagTemplate is not null)
    {
        @TagTemplate(item)
    }
    else
    {
        <span class="mar-multiselect__tag">
            @GetItemText(item)
            <button @onclick="() => RemoveItem(item)" class="mar-multiselect__tag-remove">&times;</button>
        </span>
    }
}

@* Summary tag (when MaxSummaryTags exceeded): *@
@if (SummaryTagTemplate is not null && SelectedItems.Count > MaxSummaryTags)
{
    @SummaryTagTemplate(new MultiSelectSummaryContext<TItem> { SelectedItems = SelectedItems.ToList().AsReadOnly() })
}

@* Popup body (~line 110): *@
@if (HeaderTemplate is not null)
{
    <div class="mar-multiselect__header">@HeaderTemplate</div>
}

@if (filteredItems.Any())
{
    @* existing item list *@
}
else
{
    @if (NoDataTemplate is not null)
    {
        @NoDataTemplate
    }
    else
    {
        <div class="mar-multiselect__no-data">No data found</div>
    }
}

@if (FooterTemplate is not null)
{
    <div class="mar-multiselect__footer">@FooterTemplate</div>
}
```

#### Options Considered

| Option | Description | Pros | Cons |
|--------|-------------|------|------|
| **A: RenderFragment parameters on the component** | Standard Blazor template pattern | Simple; consistent with `MariloForm`'s `FormItems`/`FormButtons` pattern | Five new parameters on an already parameter-heavy component |
| **B: Child component approach** | `<MultiSelectTagTemplate>`, `<MultiSelectHeader>`, etc. as child components | Cleaner razor syntax | Requires CascadingValue plumbing; more files; over-engineered for presentation-only templates |

#### Decision

**Option A — RenderFragment parameters.** This matches the existing Marilo convention (e.g., `MariloForm` uses `FormValidation`, `FormItems`, `FormButtons` as `RenderFragment` parameters). The five templates are presentation-only — no behavior — so the simpler pattern is appropriate.

#### Consequences

- Default rendering is preserved when templates are null (no visual regression).
- `TagTemplate` provides `TItem` as context, so consumers can render any property.
- `NoDataTemplate` replaces the hard-coded "No data found" string, enabling localization.
- CSS classes on wrapper `<div>`s (`mar-multiselect__header`, `mar-multiselect__footer`) are stable hooks for CssProvider.

#### Success Criteria

- [ ] `TagTemplate` renders custom markup for each selected item tag
- [ ] `SummaryTagTemplate` renders when selected count exceeds `MaxSummaryTags`
- [ ] `HeaderTemplate` appears above item list in popup
- [ ] `FooterTemplate` appears below item list in popup
- [ ] `NoDataTemplate` appears when filtered results are empty
- [ ] All templates are optional; null templates fall back to default rendering
- [ ] Visual test: custom templates render correctly alongside default popup chrome

---

### RES-T4B1-009: MariloUpload template slots

**Resolves:** GAP-UPL-001
**Status:** Proposed

#### Target Pattern

Add three `RenderFragment` template parameters to `MariloUpload.razor` (~184 lines) for customizing the file selection button, file list items, and file info display.

```csharp
// Added to @code block in MariloUpload.razor

/// <summary>
/// Template for the file selection trigger button.
/// When set, replaces the default "Select files..." button.
/// The entire content is wrapped in a clickable area that triggers the file input.
/// </summary>
[Parameter] public RenderFragment? SelectFilesButtonTemplate { get; set; }

/// <summary>
/// Template for each file entry in the file list.
/// Receives the file info as context for rendering name, size, progress, actions.
/// </summary>
[Parameter] public RenderFragment<UploadFileInfo>? FileTemplate { get; set; }

/// <summary>
/// Template for the file info/metadata area within the default file list item.
/// Receives the file info as context. Only used when FileTemplate is null
/// (i.e., this customizes the info section of the default file row).
/// </summary>
[Parameter] public RenderFragment<UploadFileInfo>? FileInfoTemplate { get; set; }
```

Markup integration:

```razor
@* Select button area: *@
<div class="mar-upload__select" @onclick="TriggerFileInput">
    @if (SelectFilesButtonTemplate is not null)
    {
        @SelectFilesButtonTemplate
    }
    else
    {
        <button class="mar-upload__button" type="button">
            @(SelectFilesButtonText ?? "Select files...")
        </button>
    }
</div>

@* File list: *@
@foreach (var file in Files)
{
    @if (FileTemplate is not null)
    {
        @FileTemplate(file)
    }
    else
    {
        <div class="mar-upload__file">
            @if (FileInfoTemplate is not null)
            {
                @FileInfoTemplate(file)
            }
            else
            {
                <span class="mar-upload__file-name">@file.Name</span>
                <span class="mar-upload__file-size">@FormatFileSize(file.Size)</span>
            }
            @* default action buttons (remove, pause, etc.) *@
        </div>
    }
}
```

#### Options Considered

| Option | Description | Pros | Cons |
|--------|-------------|------|------|
| **A: Three targeted RenderFragments** | `SelectFilesButtonTemplate`, `FileTemplate`, `FileInfoTemplate` | Covers all customization needs; `FileInfoTemplate` is a lighter option when only info display needs changing | Three new parameters |
| **B: Single ItemTemplate** | One `RenderFragment<UploadFileInfo>` for the entire file row | Simpler API | Forces consumers to re-implement the entire file row including action buttons for minor changes |

#### Decision

**Option A — Three targeted templates.** The upload component has three distinct customization surfaces: the trigger, the full file row, and just the file info. Providing all three lets consumers customize at the right granularity. `FileInfoTemplate` is especially useful for adding metadata (thumbnails, custom icons) without rewriting the action buttons.

#### Consequences

- `SelectFilesButtonTemplate` wraps in a clickable `<div>` — consumers do not need to wire up click handlers.
- `FileTemplate` replaces the entire file row; when used, `FileInfoTemplate` is ignored (documented in XML docs).
- `FileInfoTemplate` only applies within the default file row layout.
- `UploadFileInfo` (already public in `UploadModels.cs`) serves as the template context — no new model needed.

#### Success Criteria

- [ ] `SelectFilesButtonTemplate` replaces the default button and still triggers file selection on click
- [ ] `FileTemplate` renders custom markup for each file; receives `UploadFileInfo` with name, size, status, progress
- [ ] `FileInfoTemplate` customizes only the info section while preserving default action buttons
- [ ] `FileTemplate` takes precedence over `FileInfoTemplate` when both are set
- [ ] All templates are optional; null templates fall back to default rendering
- [ ] Upload functionality (select, upload, progress, pause, resume) works identically with or without templates

---

## Deferred Items

| Gap | Component | Reason | Target Phase |
|-----|-----------|--------|--------------|
| GAP-DTP-002 (format customization) | MariloDateTimePicker | Lower severity; requires format parser infrastructure | Phase 2 |
| GAP-DRP-003 (preset ranges) | MariloDateRangePicker | Enhancement, not a gap in core events | Phase 2 |
| GAP-TP-002 (step configuration) | MariloTimePicker | Lower severity; independent of event system | Phase 2 |
| GAP-UPL-002 (drag-drop zone template) | MariloUpload | Requires JS interop changes; separate batch | Phase 2 |
| GAP-UPL-003 (async validation) | MariloUpload | Complex feature; depends on OnSelect event args redesign | Phase 3 |

---

## New Types Summary

| Type | File | Used By |
|------|------|---------|
| `PopupEventArgs` | `src/Marilo.Core/Models/PopupEventArgs.cs` | All 5 components |
| `MultiSelectReadEventArgs` | `src/Marilo.Core/Models/MultiSelectModels.cs` | MariloMultiSelect |
| `MultiSelectItemRenderEventArgs<TItem>` | `src/Marilo.Core/Models/MultiSelectModels.cs` | MariloMultiSelect |
| `MultiSelectSummaryContext<TItem>` | `src/Marilo.Core/Models/MultiSelectModels.cs` | MariloMultiSelect |
| `CalendarCellRenderEventArgs` | `src/Marilo.Core/Models/CalendarCellRenderEventArgs.cs` | MariloDateTimePicker, MariloDateRangePicker |
| `DateRangeChangedEventArgs` | `src/Marilo.Core/Models/DateRangeModels.cs` | MariloDateRangePicker |

## Implementation Order

The resolutions should be implemented in this order due to dependencies:

1. **RES-T4B1-001** (PopupEventArgs) — foundation for all popup events
2. **RES-T4B1-005** (TimePicker bug fix + events) — smallest scope, validates the PopupEventArgs pattern
3. **RES-T4B1-006** (Upload chunk resume) — independent bug fix, no event dependencies
4. **RES-T4B1-004** (DateRangePicker events) — uses PopupEventArgs
5. **RES-T4B1-003** (DateTimePicker events) — uses PopupEventArgs + creates CalendarCellRenderEventArgs
6. **RES-T4B1-002** (MultiSelect events) — uses PopupEventArgs, largest event surface
7. **RES-T4B1-007** (MultiSelect AllowCustom) — depends on OnChange from RES-T4B1-002
8. **RES-T4B1-008** (MultiSelect templates) — independent of events but ships together
9. **RES-T4B1-009** (Upload templates) — independent, can parallelize with 007/008
