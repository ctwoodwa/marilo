# Resolution Records: T4 Pickers Batch 7 — MultiSelectSettings child component API

> Date: 2026-04-08
> Source: `stages/01-intake/output/gap-t4-pickers-inventory.md` GAP-MSEL-005
> Components: MariloMultiSelect (+ two new sibling components: MultiSelectSettings, MultiSelectPopupSettings)
> Scope: batch (skips Stage 04 per workspace gap-scope routing)

This is the final medium-severity item on `MariloMultiSelect`. After this batch, only `GAP-MSEL-007 ScrollMode` (deferred — Blazor `<Virtualize>` lacks the primitive) remains open on the component.

The cerebrum already documents a `MariloWizard CascadingValue bug` class (root cause: parent forgot to wrap `ChildContent` in `<CascadingValue>`). This batch follows the canonical `MariloDataGrid<TItem>` ↔ `MariloGridColumn<TItem>` cascading pattern verified at:

- `src/Marilo.Components/DataGrid/MariloDataGrid.razor:37-39` — `<CascadingValue Value="this" IsFixed="true">@ChildContent</CascadingValue>`
- `src/Marilo.Components/DataGrid/MariloGridColumn.razor:5,83-92` — `[CascadingParameter]` + `OnInitialized` registration + `Dispose` unregistration
- `src/Marilo.Components/DataGrid/MariloDataGrid.razor.cs:253-269` — `RegisterColumn` / `UnregisterColumn` methods that call `StateHasChanged`

---

## RES-T4B7-01: MultiSelectSettings + MultiSelectPopupSettings child components

**Resolves:** GAP-MSEL-005
**Status:** Implemented

### Target Pattern

Two new child components and one internal interface enable a Telerik-shaped declarative API:

```razor
<MariloMultiSelect TItem="Country" TValue="int"
                   Data="@countries"
                   TextField="Name" ValueField="Id"
                   @bind-Value="selected">
    <MultiSelectSettings AdaptiveMode="AdaptiveMode.Auto" />
    <MultiSelectPopupSettings Height="400px"
                              Width="320px"
                              MaxHeight="500px"
                              Class="my-popup-theme" />
</MariloMultiSelect>
```

### Architecture

#### 1. New internal interface: `IMultiSelectSettingsSink`

Lives in `Marilo.Components.Forms.Inputs` (internal). Decouples settings registration from the generic `MariloMultiSelect<TItem, TValue>` type so child components do not need to be generic.

```csharp
internal interface IMultiSelectSettingsSink
{
    void RegisterSettings(MultiSelectSettings settings);
    void UnregisterSettings(MultiSelectSettings settings);
    void RegisterPopupSettings(MultiSelectPopupSettings settings);
    void UnregisterPopupSettings(MultiSelectPopupSettings settings);
}
```

`MariloMultiSelect<TItem, TValue>` implements this interface.

#### 2. New child component: `MultiSelectSettings`

Non-generic. Holds general behavior settings that override parent parameters when the child is present.

```csharp
public class MultiSelectSettings : ComponentBase, IDisposable
{
    [CascadingParameter] internal IMultiSelectSettingsSink? ParentSink { get; set; }

    /// <summary>
    /// Adaptive rendering mode. Overrides MariloMultiSelect.AdaptiveMode when set.
    /// </summary>
    [Parameter] public AdaptiveMode? AdaptiveMode { get; set; }

    protected override void OnInitialized()
    {
        ParentSink?.RegisterSettings(this);
    }

    public void Dispose()
    {
        ParentSink?.UnregisterSettings(this);
    }
}
```

The component renders no markup (no `BuildRenderTree` override needed beyond default — it produces zero DOM).

**Why nullable `AdaptiveMode?`:** The override semantics are "if set, win; if null, fall through to parent param". A nullable enum lets the consumer omit the parameter and inherit the parent's value.

#### 3. New child component: `MultiSelectPopupSettings`

Non-generic. Holds popup appearance/dimensions that override parent parameters when present.

```csharp
public class MultiSelectPopupSettings : ComponentBase, IDisposable
{
    [CascadingParameter] internal IMultiSelectSettingsSink? ParentSink { get; set; }

    /// <summary>Popup height. Overrides MariloMultiSelect.PopupHeight when set.</summary>
    [Parameter] public string? Height { get; set; }

    /// <summary>Popup max-height. Overrides MariloMultiSelect.PopupMaxHeight when set.</summary>
    [Parameter] public string? MaxHeight { get; set; }

    /// <summary>Popup width. New capability — no parent equivalent (parent has no PopupWidth).</summary>
    [Parameter] public string? Width { get; set; }

    /// <summary>Popup CSS class. Overrides MariloMultiSelect.PopupClass when set.</summary>
    [Parameter] public string? Class { get; set; }

    protected override void OnInitialized()
    {
        ParentSink?.RegisterPopupSettings(this);
    }

    public void Dispose()
    {
        ParentSink?.UnregisterPopupSettings(this);
    }
}
```

`Width` is a new capability (no parent parameter equivalent). All others are overrides for existing parent parameters.

#### 4. Changes to `MariloMultiSelect<TItem, TValue>`

a. **New `ChildContent` parameter:**
```csharp
[Parameter] public RenderFragment? ChildContent { get; set; }
```

b. **Implements `IMultiSelectSettingsSink`** with internal single-instance fields (latest registration wins — there is no documented Telerik behavior for multiple settings tags, and the existing `MariloDataGrid` column pattern uses a list because columns are inherently many; settings tags should be singletons by convention):

```csharp
private MultiSelectSettings? _registeredSettings;
private MultiSelectPopupSettings? _registeredPopupSettings;

void IMultiSelectSettingsSink.RegisterSettings(MultiSelectSettings s)
{
    _registeredSettings = s;
    InvokeAsync(StateHasChanged);
}

void IMultiSelectSettingsSink.UnregisterSettings(MultiSelectSettings s)
{
    if (ReferenceEquals(_registeredSettings, s)) _registeredSettings = null;
    InvokeAsync(StateHasChanged);
}

void IMultiSelectSettingsSink.RegisterPopupSettings(MultiSelectPopupSettings s)
{
    _registeredPopupSettings = s;
    InvokeAsync(StateHasChanged);
}

void IMultiSelectSettingsSink.UnregisterPopupSettings(MultiSelectPopupSettings s)
{
    if (ReferenceEquals(_registeredPopupSettings, s)) _registeredPopupSettings = null;
    InvokeAsync(StateHasChanged);
}
```

`InvokeAsync(StateHasChanged)` is mandatory for thread-safety per the cerebrum learning at `[2026-04-04] Public state APIs should be dispatcher-safe`.

c. **Resolved-value computed properties** in the `@code` block:

```csharp
private AdaptiveMode EffectiveAdaptiveMode =>
    _registeredSettings?.AdaptiveMode ?? AdaptiveMode;

private string? EffectivePopupHeight =>
    _registeredPopupSettings?.Height ?? PopupHeight;

private string? EffectivePopupMaxHeight =>
    _registeredPopupSettings?.MaxHeight ?? PopupMaxHeight;

private string? EffectivePopupWidth =>
    _registeredPopupSettings?.Width;

private string? EffectivePopupClass =>
    _registeredPopupSettings?.Class ?? PopupClass;
```

d. **Markup updates** — replace direct parameter reads with effective-value reads in three places:

- The popup `<div>` `class` attribute: `@CssProvider.MultiSelectPopupClass() @EffectivePopupClass`
- The virtualized container's `style` attribute: `height:@(EffectivePopupHeight ?? "200px")`
- The non-virtualized list container's `style` attribute computed from `EffectivePopupHeight` / `EffectivePopupMaxHeight`. The existing `_popupMaxHeightStyle` getter is rewritten to read effective values
- A new inline `style` segment on the popup `<div>` that adds `width:@EffectivePopupWidth;` only when set

The existing `AdaptiveMode` parameter is read once today via `AdaptiveMode` — the markup currently does not actively render based on it (it's a placeholder per Batch 3). No markup change needed for AdaptiveMode beyond replacing future reads with `EffectiveAdaptiveMode`. Ship the override plumbing now so future Adaptive rendering reads the right value.

e. **Wrap `ChildContent` in a `<CascadingValue>`** at the end of the component markup (outside the visible region — children render no DOM):

```razor
@if (ChildContent != null)
{
    <CascadingValue Value="(IMultiSelectSettingsSink)this" IsFixed="true">
        @ChildContent
    </CascadingValue>
}
```

Cast to the interface so non-generic child components match without juggling generic type parameters.

### Options Considered

**Option A: Single-instance registration (latest wins) with `IsFixed="true"` cascade (chosen)**
- Approach: Each settings type stored as a single field; subsequent registrations overwrite. Cascade is `IsFixed="true"` because the parent reference does not change after first render.
- Pros: Mirrors the canonical Telerik shape (settings tags are singletons); minimal state; matches the established `MariloDataGrid` pattern except with single-instance fields instead of lists; `IsFixed="true"` is a perf win.
- Cons: Multiple `<MultiSelectSettings>` tags would silently lose all but the last — but this is correct behavior because the API is designed for one settings tag per type.
- Effort: Small.

**Option B: List-based registration (mirrors `MariloDataGrid._columns`)**
- Approach: Store registrations in `List<MultiSelectSettings>` and `List<MultiSelectPopupSettings>`; merge on read.
- Pros: Allows multiple tags.
- Cons: Merge semantics are undefined (which AdaptiveMode wins if two tags set different values?); creates ambiguity; columns are inherently many but settings are singletons.
- Effort: Small.

**Option C: Generic settings components (`MultiSelectSettings<TItem, TValue>`)**
- Approach: Make child components generic to match the parent.
- Pros: Direct cascade of `MariloMultiSelect<TItem, TValue>` without an interface.
- Cons: Forces consumers to write `<MultiSelectSettings TItem="..." TValue="...">` — terrible ergonomics; settings have nothing to do with TItem/TValue.
- Effort: Small but worse UX.

**Option D: Interface-based cascade with non-generic children (chosen base)**
- See Option A; this is the same approach. The interface decouples generics.

### Decision

**Chosen:** Option A + Option D combined — interface-based cascade with non-generic child components and single-instance registration.

**Rationale:**
- Mirrors the canonical `MariloDataGrid` cascading pattern (verified at the file:line references above) so no new architecture is introduced.
- Non-generic children give consumers ergonomic markup (`<MultiSelectSettings AdaptiveMode="..."/>` not `<MultiSelectSettings TItem="..." TValue="..." AdaptiveMode="..."/>`).
- Single-instance registration matches Telerik's actual API shape for settings tags (you write one `<TelerikComboBoxSettings>`, not many).
- `InvokeAsync(StateHasChanged)` on register/unregister is mandatory per cerebrum `[2026-04-04] Public state APIs should be dispatcher-safe` — registration can happen on a non-renderer thread.
- The cerebrum's `MariloWizard CascadingValue bug` is avoided by following the canonical pattern: **parent always wraps `ChildContent` in `<CascadingValue>`**. The implementer must verify this is in place.

### Consequences

- New file `src/Marilo.Components/Forms/Inputs/MultiSelectSettings.cs` containing the interface, the two child component classes, and any necessary using statements.
- New `[Parameter] public RenderFragment? ChildContent { get; set; }` on `MariloMultiSelect`.
- New `IMultiSelectSettingsSink` implementation on `MariloMultiSelect` (single-instance fields + 4 register/unregister methods).
- New `Effective*` computed properties for AdaptiveMode, PopupHeight, PopupMaxHeight, PopupWidth, PopupClass.
- Markup updated in three locations: popup div class, virtualized container style, non-virtualized list container style. Plus a new `width:@EffectivePopupWidth` style segment when Width is set.
- New `<CascadingValue Value="(IMultiSelectSettingsSink)this" IsFixed="true">@ChildContent</CascadingValue>` wrapping `@ChildContent` at the end of the component markup.
- Existing parameters (`PopupHeight`, `PopupMaxHeight`, `PopupClass`, `AdaptiveMode`) stay in place — child settings override but do not replace them. This preserves backward compatibility for consumers using the flat-parameter form.
- `Width` is a new capability with no flat-parameter equivalent. Documented in the closure report.
- Tests cover: settings present override parent params, settings absent fall through to parent params, popup width applied when set, dispose unregisters correctly.

### Success Criteria

- [ ] `IMultiSelectSettingsSink` internal interface exists in `Marilo.Components.Forms.Inputs`.
- [ ] `MultiSelectSettings : ComponentBase, IDisposable` class exists with `[Parameter] AdaptiveMode? AdaptiveMode`, registers in `OnInitialized`, unregisters in `Dispose`.
- [ ] `MultiSelectPopupSettings : ComponentBase, IDisposable` class exists with `[Parameter] string? Height/MaxHeight/Width/Class`, registers in `OnInitialized`, unregisters in `Dispose`.
- [ ] `MariloMultiSelect<TItem, TValue>` implements `IMultiSelectSettingsSink` with single-instance fields and `InvokeAsync(StateHasChanged)` on each register/unregister.
- [ ] `MariloMultiSelect` has a `ChildContent` parameter wrapped in `<CascadingValue Value="(IMultiSelectSettingsSink)this" IsFixed="true">@ChildContent</CascadingValue>` somewhere in the markup.
- [ ] `Effective*` computed properties resolve registered child overrides, falling through to parent parameters when child is null.
- [ ] Popup markup reads from `Effective*` properties for height, max-height, width, class.
- [ ] When `<MultiSelectPopupSettings Width="320px"/>` is present, the popup div has `width:320px` in its style.
- [ ] When no settings child is present, all current behavior is unchanged (no regressions in existing tests).
- [ ] bUnit tests cover:
  - `MultiSelectPopupSettings` Height overrides parent `PopupHeight`
  - `MultiSelectPopupSettings` MaxHeight overrides parent `PopupMaxHeight`
  - `MultiSelectPopupSettings` Width applies to popup style
  - `MultiSelectPopupSettings` Class concatenates onto popup class
  - No settings child → parent parameters used
  - `<MariloMultiSelect>` accepts a `ChildContent` parameter without rendering a visible DOM region for settings tags

### Cross-cutting notes

- **No CSS provider changes** required.
- **No third-party dependencies** added.
- **Backward compatibility preserved** — existing flat parameters still work; settings tags only override when present.
- **Lifecycle ordering is safe:** child registers in `OnInitialized` (called once when child first attaches to render tree). Parent's `InvokeAsync(StateHasChanged)` triggers a re-render that picks up the new effective values. There is no cycle risk because `OnInitialized` runs once per component instance.
- **Cerebrum guard:** the canonical fix for the Wizard CascadingValue bug is "parent must wrap ChildContent in `<CascadingValue>`". The implementer must verify this wrap is present and that the cascaded value is `(IMultiSelectSettingsSink)this`, not just `this` (which would be the generic `MariloMultiSelect<TItem, TValue>` and would not match the children's non-generic cascade parameter).
- **Disposal safety:** child `Dispose()` checks `ReferenceEquals` before nulling the parent's field, so a child being disposed AFTER another has already replaced it does not accidentally null out the new one. (Harmless for single-instance singletons in practice, but defensive.)
