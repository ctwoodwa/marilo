# Resolution Records: T4 Pickers Batch 6 — OnChange / OnItemRender / Virtual Scroll Config

> Date: 2026-04-08
> Source: `stages/01-intake/output/gap-t4-pickers-prioritization.md` (Batch 1/2 leftovers; medium severity)
> Components: MariloMultiSelect
> Scope: batch (skips Stage 04 per workspace gap-scope routing)

These are the next medium-severity items after Batch 5 closed:
- **GAP-MSEL-001 completion** — `OnChange` and `OnItemRender` events. (`OnOpen`/`OnClose`/`OnBlur` already in Batch 1, `OnRead` already in Batch 5.)
- **GAP-MSEL-007** — Virtual scroll configuration parameters (`ItemHeight`, `PageSize`).

GAP-MSEL-005 (`<MultiSelectSettings>` child component) is intentionally still deferred — it requires deliberate cascading-parameter design and is worth its own batch.

---

## RES-T4B6-01: MariloMultiSelect OnChange + OnItemRender events

**Resolves:** GAP-MSEL-001 (final two sub-items: OnChange + OnItemRender)
**Status:** Implemented

### Target Pattern

Two new event callbacks added to `MariloMultiSelect<TItem, TValue>`:

```csharp
/// <summary>
/// Fires after the selected values change. Receives the new value list.
/// Distinct from ValueChanged (which is the @bind-Value plumbing) — OnChange
/// is a consumer-friendly event that fires once per user-driven change.
/// </summary>
[Parameter] public EventCallback<List<TValue>> OnChange { get; set; }

/// <summary>
/// Fires for each item as it is rendered, allowing the consumer to inspect
/// the item and modify per-item rendering attributes (e.g., add a CSS class
/// to highlight specific entries, or mark an item disabled).
/// </summary>
[Parameter] public EventCallback<MultiSelectItemRenderEventArgs<TItem>> OnItemRender { get; set; }
```

New args type in `MultiSelectModels.cs`:

```csharp
public class MultiSelectItemRenderEventArgs<TItem>
{
    /// <summary>The item being rendered.</summary>
    public TItem Item { get; init; } = default!;

    /// <summary>Set additional CSS class(es) to apply to this item's option element.</summary>
    public string? CssClass { get; set; }

    /// <summary>Set true to render this item as disabled (not selectable).</summary>
    public bool IsDisabled { get; set; }
}
```

`OnChange` fires from a single internal helper invoked from `EmitValueChanged`, which is already the central choke-point for value mutations (toggle, remove, clear, custom add). One invocation per user-driven mutation. It does NOT fire from `OnParametersSet` when the consumer sets `Value` externally — that would create echo loops.

`OnItemRender` fires once per visible filtered item during render. Results are cached in `_itemRenderCache` (a `Dictionary<TItem, MultiSelectItemRenderEventArgs<TItem>>`) so the callback is not invoked on every keystroke for every item — the cache is rebuilt only when `_filteredItems` or the underlying data window changes (in `ApplyFilter`, `LoadServerDataAsync`, and `OnParametersSetAsync`). The `IsDisabled` flag prevents `ToggleItem` from selecting the item (early-return guard) and adds an `aria-disabled="true"` attribute to the option div.

### Options Considered

**Option A: OnChange + OnItemRender as coordinated additions, cached render args (chosen)**
- Approach: Two new parameters, one new args type, central invocation point in `EmitValueChanged` for OnChange, build-once cache for OnItemRender.
- Pros: Single source of truth for OnChange firing; render cache prevents per-render callback storms; consistent with the existing `CalendarCellRenderEventArgs` pattern in `MariloDateTimePicker`.
- Cons: One new private dictionary; one new args type.
- Effort: Small.

**Option B: OnItemRender invoked synchronously in markup loop**
- Approach: Call `OnItemRender.InvokeAsync(args)` directly from the `@foreach` loop on every render.
- Pros: No cache state.
- Cons: Async invocation from markup loop is awkward in Blazor; consumers expect render-time callbacks to be sync; the cell-render cache pattern in DateTimePicker (`_cellRenderCache.Clear() → for each day → InvokeAsync → store`) is the established convention here.
- Effort: Small but pattern-divergent.

**Option C: OnChange as an alias for ValueChanged**
- Approach: Don't add OnChange — document that consumers should use `@bind-Value` and watch the property setter.
- Pros: Zero new API.
- Cons: Doesn't satisfy spec; Telerik consumers expect a distinct `OnChange` event; ValueChanged is implementation plumbing for two-way binding, not a public event.
- Effort: None — but doesn't close the gap.

### Decision

**Chosen:** Option A.
**Rationale:** Mirrors the existing `OnCalendarCellRender` cache pattern in `MariloDateTimePicker` (`_cellRenderCache` rebuilt on month change). Same shape, same semantics, same lifecycle. The cache ensures the callback is not invoked on every render, only when filtered items change. Consumers writing `OnItemRender` handlers see consistent semantics across pickers.

OnChange fires from `EmitValueChanged` (the existing single mutation choke-point) so there's no risk of duplicate fires or missed mutations. Both `Toggle`, `RemoveItem`, `ClearAll`, and `HandleAddCustom` already route through `EmitValueChanged`, so no additional plumbing is needed.

### Consequences

- New parameter `OnChange` on `MariloMultiSelect`.
- New parameter `OnItemRender` on `MariloMultiSelect`.
- New args type `MultiSelectItemRenderEventArgs<TItem>` in `MultiSelectModels.cs`.
- New private `Dictionary<TItem, MultiSelectItemRenderEventArgs<TItem>> _itemRenderCache` field.
- New private `RebuildItemRenderCacheAsync()` method called from `ApplyFilter`, `LoadServerDataAsync`, and `OnParametersSetAsync`.
- `EmitValueChanged` extended with `await OnChange.InvokeAsync(list)`.
- `ToggleItem` extended with `IsDisabled` guard — disabled items are not selectable.
- Markup loop (both virtualized and non-virtualized paths) reads from `_itemRenderCache` to apply the consumer-supplied CSS class and `aria-disabled` attribute.
- `ApplyFilter` is now `async Task` (was `void`) because it must rebuild the render cache. Callers updated.
- bUnit tests cover OnChange firing on toggle, OnItemRender being invoked per item, custom CssClass applied, IsDisabled blocking selection.

### Success Criteria

- [x] `OnChange` parameter exists.
- [x] `OnItemRender` parameter exists.
- [x] `MultiSelectItemRenderEventArgs<TItem>` type exists with Item, CssClass, IsDisabled.
- [x] OnChange fires once per user-driven value change.
- [x] OnChange does NOT fire when consumer sets Value externally via OnParametersSet.
- [x] OnItemRender invoked once per filtered item per cache rebuild (not per render).
- [x] CssClass from OnItemRender is applied to the option element.
- [x] IsDisabled from OnItemRender prevents selection and emits aria-disabled.
- [x] bUnit tests cover OnChange firing, OnItemRender CSS class application, OnItemRender disabled blocking.

---

## RES-T4B6-02: MariloMultiSelect virtual scroll configuration

**Resolves:** GAP-MSEL-007 (`ItemHeight`, `PageSize`)
**Status:** Implemented

### Target Pattern

Add two configuration parameters to `MariloMultiSelect<TItem, TValue>` consumed by the existing `<Virtualize>` component:

```csharp
/// <summary>
/// Pixel height of each virtualized item. Defaults to 32. Only used when
/// EnableVirtualization is true.
/// </summary>
[Parameter] public int ItemHeight { get; set; } = 32;

/// <summary>
/// Number of items to render outside the visible viewport (overscan count)
/// when virtualization is enabled. Defaults to 3.
/// </summary>
[Parameter] public int PageSize { get; set; } = 3;
```

Wire them through to the existing `<Virtualize>` element:

```razor
<Microsoft.AspNetCore.Components.Web.Virtualization.Virtualize Items="@_filteredItems"
                                                                Context="item"
                                                                ItemSize="@ItemHeight"
                                                                OverscanCount="@PageSize">
```

The `ItemSize` parameter on `Virtualize` was previously hardcoded to `32` — now driven by `ItemHeight`. `OverscanCount` was previously omitted (Blazor default of 3) — now driven by `PageSize`.

### Note on `ScrollMode`

The gap inventory mentioned `ScrollMode` as a third virtual-scroll parameter. Blazor's built-in `<Virtualize>` does not expose a "scroll mode" setting (it always uses absolute-positioned spacers); the Telerik concept of `ScrollMode = Virtual | Endless | Scrollable` does not map cleanly onto the Blazor primitive without rebuilding the virtualization path. **`ScrollMode` is filed as a follow-up gap (deferred)** rather than implemented as a no-op parameter — adding a parameter that does nothing is worse than not adding it.

### Options Considered

**Option A: ItemHeight + PageSize as direct passthroughs to <Virtualize> (chosen)**
- Approach: Two new int parameters, bound directly to `ItemSize` and `OverscanCount` on the existing `<Virtualize>` element.
- Pros: Tiny change; uses existing infrastructure; consumers get real virtualization tuning; defaults preserve existing behavior.
- Cons: ScrollMode left out (filed as follow-up).
- Effort: Tiny.

**Option B: All three parameters including a no-op ScrollMode enum**
- Approach: Add `ScrollMode` as a parameter that documents itself as "reserved for future virtualization rebuild".
- Pros: Surface-area parity with spec.
- Cons: Adding a parameter that does nothing is misleading; consumers expect parameters to have effects; better to defer.
- Effort: Tiny but anti-pattern.

**Option C: Replace <Virtualize> with custom virtualization to support ScrollMode**
- Approach: Build a custom virtualization implementation that supports endless-scroll mode.
- Pros: Full spec parity.
- Cons: Massive scope change for one parameter; YAGNI.
- Effort: Very high.

### Decision

**Chosen:** Option A.
**Rationale:** Tiny change, real value, no surface-area lies. The spec's `ScrollMode` parameter is filed as a deferred follow-up with explicit rationale ("requires custom virtualization implementation"). Consumers asking for tunable virtual scrolling get exactly what they need: `ItemHeight` for variable row heights, `PageSize` for overscan tuning.

`PageSize` is named to match the Telerik spec, even though Blazor calls the same concept `OverscanCount`. The XML doc is explicit about the mapping.

### Consequences

- Two new parameters on `MariloMultiSelect`.
- `<Virtualize>` element's `ItemSize` and `OverscanCount` attributes wired through.
- Defaults (32px height, 3-item overscan) preserve existing behavior — no breaking change for existing consumers.
- `ScrollMode` filed as deferred follow-up.
- bUnit tests cover the parameters defaulting correctly and accepting custom values.

### Success Criteria

- [x] `ItemHeight` parameter exists with default 32.
- [x] `PageSize` parameter exists with default 3.
- [x] `<Virtualize>` element uses `ItemSize="@ItemHeight"` and `OverscanCount="@PageSize"`.
- [x] Defaults preserve existing behavior.
- [x] bUnit tests cover the parameters being applied to virtualized rendering.

---

## Cross-cutting notes

- **Scope:** batch (skips Stage 04 remediation plan per workspace `Gap Scope Routing`).
- **No CSS provider changes** required.
- **No third-party dependencies** added.
- **GAP-MSEL-001 fully closed** by this batch when combined with Batch 1 and Batch 5: OnOpen ✅ B1, OnClose ✅ B1, OnBlur ✅ B1, OnRead ✅ B5, OnChange ✅ B6, OnItemRender ✅ B6.
- **GAP-MSEL-007 partial closure:** ItemHeight + PageSize closed; ScrollMode deferred with rationale (filed as follow-up).
- **Testing convention:** runtime test execution remains gated on the workspace-level `.NET SDK not available` blocker. New tests verified by code inspection.
