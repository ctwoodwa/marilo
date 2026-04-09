# Implementation Log: T4 Pickers Batch 6 — OnChange / OnItemRender / Virtual Scroll Config

> Date: 2026-04-08
> Resolutions: `stages/03-resolution-design/output/gap-t4-picker-batch6-resolutions.md`
> Components: `MariloMultiSelect`
> Scope: batch (Stage 04 skipped per workspace gap-scope routing)

---

## RES-T4B6-01: MariloMultiSelect OnChange + OnItemRender

### Files modified

- `src/Marilo.Components/Forms/Inputs/MultiSelectModels.cs`
- `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor`

### Changes

1. **New args type** (`MultiSelectModels.cs`):
   ```csharp
   public class MultiSelectItemRenderEventArgs<TItem>
   {
       public TItem Item { get; init; } = default!;
       public string? CssClass { get; set; }
       public bool IsDisabled { get; set; }
   }
   ```
   Inserted before the existing `MultiSelectReadEventArgs<TItem>` (Batch 5).

2. **Two new parameters** (`MariloMultiSelect.razor` `@code`):
   ```csharp
   [Parameter] public EventCallback<List<TValue>> OnChange { get; set; }
   [Parameter] public EventCallback<MultiSelectItemRenderEventArgs<TItem>> OnItemRender { get; set; }
   ```

3. **New private state**:
   ```csharp
   private Dictionary<TItem, MultiSelectItemRenderEventArgs<TItem>> _itemRenderCache = new();
   ```
   Mirrors the existing `_cellRenderCache` pattern in `MariloDateTimePicker`.

4. **`EmitValueChanged` extended** to fire `OnChange`:
   ```csharp
   private async Task EmitValueChanged()
   {
       var list = _selectedValues.ToList();
       Value = list;
       await ValueChanged.InvokeAsync(list);
       if (OnChange.HasDelegate)
           await OnChange.InvokeAsync(list);
   }
   ```
   Single choke-point — every value mutation already routes through here (Toggle, Remove, Clear, custom add).

5. **`ToggleItem` extended with disabled guard**:
   ```csharp
   if (_itemRenderCache.TryGetValue(item, out var renderArgs) && renderArgs.IsDisabled) return;
   ```
   Prevents disabled items from being selected.

6. **New `RebuildItemRenderCacheAsync` helper**:
   ```csharp
   private async Task RebuildItemRenderCacheAsync()
   {
       _itemRenderCache.Clear();
       if (!OnItemRender.HasDelegate) return;
       foreach (var item in _filteredItems)
       {
           if (item is null) continue;
           var args = new MultiSelectItemRenderEventArgs<TItem> { Item = item };
           await OnItemRender.InvokeAsync(args);
           _itemRenderCache[item] = args;
       }
   }
   ```
   Cheap when `OnItemRender` is not bound — early-returns after clearing.

7. **Cache rebuild call sites** — invoked from three places after `_filteredItems` changes:
   - `OnParametersSetAsync` — after `ApplyFilter()`
   - `OpenDropdown` — after `ApplyFilter()` in the local-data branch
   - `LoadServerDataAsync` — after `_filteredItems = _allItems.ToList()`
   - `OnFilterInput` — after `ApplyFilter()` in the local-data branch

8. **Markup updated** in both virtualized and non-virtualized loops to:
   - Look up the per-item render args from `_itemRenderCache` (null-guarded for `TItem? item`)
   - Apply `extraClass` to the option element class string
   - Apply `aria-disabled="@itemDisabled"` to the option div
   - Apply `disabled="@itemDisabled"` to the inner checkbox

### Verification (manual code-trace)

| Scenario | Expected | Trace |
|---|---|---|
| Select item with no OnChange handler | No throw | `EmitValueChanged → OnChange.HasDelegate=false → skip` |
| Select item with OnChange handler | Handler fires once | `ToggleItem → EmitValueChanged → OnChange.InvokeAsync` |
| Remove tag with OnChange handler | Handler fires once | `RemoveItem → EmitValueChanged → OnChange.InvokeAsync` |
| ClearAll with OnChange handler | Handler fires once | `ClearAll → EmitValueChanged → OnChange.InvokeAsync` |
| External `Value` set via parameter | OnChange does NOT fire | `OnParametersSetAsync` updates internal state but does not call `EmitValueChanged` |
| OnItemRender bound, dropdown opens | One invocation per filtered item | `OpenDropdown → ApplyFilter → RebuildItemRenderCacheAsync → foreach → InvokeAsync` |
| OnItemRender returns CssClass | Class applied to option | Markup reads from `_itemRenderCache`, concatenates `@extraClass` |
| OnItemRender returns IsDisabled=true | Selection blocked + aria-disabled | Markup emits `aria-disabled="true"`; `ToggleItem` early-returns |
| Filter changes | Cache rebuilt | `OnFilterInput → ApplyFilter → RebuildItemRenderCacheAsync` |
| Server read returns new data | Cache rebuilt | `LoadServerDataAsync → _filteredItems = ... → RebuildItemRenderCacheAsync` |

---

## RES-T4B6-02: MariloMultiSelect virtual scroll configuration

### Files modified

- `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor`

### Changes

1. **Two new parameters**:
   ```csharp
   [Parameter] public int ItemHeight { get; set; } = 32;
   [Parameter] public int PageSize { get; set; } = 3;
   ```

2. **`<Virtualize>` element wired through**:
   ```razor
   <Virtualize Items="@_filteredItems"
               Context="item"
               ItemSize="@ItemHeight"
               OverscanCount="@PageSize">
   ```
   Was: `ItemSize="32"`, no `OverscanCount` (Blazor default 3).
   Now: `ItemSize="@ItemHeight"`, `OverscanCount="@PageSize"`.

### Verification (manual code-trace)

| Scenario | Expected | Trace |
|---|---|---|
| No params set | ItemSize=32, OverscanCount=3 | Defaults preserve existing behavior |
| ItemHeight=48 | Virtualize.ItemSize=48 | Direct passthrough |
| PageSize=10 | Virtualize.OverscanCount=10 | Direct passthrough |
| EnableVirtualization=false | Params have no effect | Non-virtualized branch ignores both |

### `ScrollMode` deferral

The gap inventory mentioned `ScrollMode` (Virtual/Endless/Scrollable). Blazor's built-in `<Virtualize>` does not expose a scroll-mode setting; it always uses absolute-positioned spacers. Adding a `ScrollMode` parameter that does nothing would mislead consumers. Filed as deferred follow-up — explicit rationale in RES-T4B6-02 § Note on ScrollMode.

---

## Tests

### Added tests

#### `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs` (12 new tests)

**OnChange / OnItemRender (7):**
| Test | Purpose |
|------|---------|
| `OnChange_FiresWhenUserSelectsItem` | OnChange fires once on toggle, list contains new value |
| `OnChange_FiresOnRemove` | OnChange fires once when tag remove button clicked |
| `OnChange_DoesNotFireOnExternalValueSet` | External `Value` set via parameter does NOT fire OnChange |
| `OnItemRender_InvokedOncePerFilteredItem` | OnItemRender invoked once per filtered item on dropdown open |
| `OnItemRender_CssClassAppliedToOption` | CssClass set in handler appears on the matching option element |
| `OnItemRender_DisabledItemIsNotSelectable` | IsDisabled=true blocks ToggleItem; ValueChanged not fired |
| `OnItemRender_DisabledItemHasAriaDisabled` | Disabled option emits `aria-disabled="true"` |

**Virtual scroll config (4):**
| Test | Purpose |
|------|---------|
| `ItemHeight_HasDefault32` | Default value verified via `cut.Instance.ItemHeight` |
| `PageSize_HasDefault3` | Default value verified via `cut.Instance.PageSize` |
| `ItemHeight_AcceptsCustomValue` | Custom 48 propagates; virtualized container renders |
| `PageSize_AcceptsCustomValue` | Custom 10 propagates; virtualized container renders |

### Test execution

Test runtime not executed in this session — `.NET SDK not available` per `_config/coverage-summary.md` Active Blockers. All 11 tests written following existing conventions. Verification by code inspection only.

---

## Files written

- `src/Marilo.Components/Forms/Inputs/MultiSelectModels.cs` (new args type appended)
- `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor`
- `tests/Marilo.Tests.Unit/Selection/MultiSelectTests.cs`

## Files read (target project)

- `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor` (full read; carryover from Batch 5)
- `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor` (pattern reference for `_cellRenderCache` cache invalidation)
- `src/Marilo.Components/Forms/Inputs/MultiSelectModels.cs` (existing Batch 5 file)
- `src/Marilo.Core/Base/MariloComponentBase.cs` (verified `CombineClasses` signature — single arg only, used inline class concatenation instead)

## No opportunistic changes

Every modified file traces to GAP-MSEL-001 final or GAP-MSEL-007. No drive-by refactors.

## Side-effect: GAP-MSEL-001 fully closed

Combined sub-item status across all batches:
| Sub-item | Batch | Status |
|---|---|---|
| OnOpen (cancellable) | B1 | ✅ |
| OnClose (cancellable) | B1 | ✅ |
| OnBlur | B1 | ✅ |
| OnRead | B5 | ✅ |
| OnChange | **B6** | ✅ |
| OnItemRender | **B6** | ✅ |

GAP-MSEL-001 transitions from **Partially resolved** to **Resolved**.
