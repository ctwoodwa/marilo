# Resolution Records: MariloTreeView — CheckboxTemplate (Custom Checkbox Rendering)

## Summary

Gap 21 covers the ability to supply a custom render fragment for tree-node checkboxes. `CheckboxTemplate` (a `RenderFragment<CheckboxContext>`) is fully implemented, including a fall-through to a default `<input type="checkbox">` when the parameter is null. This record is reconstructed retroactively; the implementation predates the record.

---

### RES-TREEVIEW-021: CheckboxTemplate RenderFragment parameter

**Resolves:** GAP-21 (CheckboxTemplate — custom checkbox rendering)
**Status:** Reconstructed — implementation predates this record

#### Problem Statement

MariloTreeView supports checkbox mode, where each node can be checked, unchecked, or indeterminate. By default the component renders a plain `<input type="checkbox">`. Consumers who need a custom visual (e.g., a themed checkbox, an icon-based indicator, or an accessible custom control) had no way to replace the default rendering while still receiving the correct check state and a mutation callback.

A composable escape hatch was required that:

1. Exposes the current logical state (checked, indeterminate, disabled) to the custom render fragment.
2. Provides a typed callback so the custom control can request a state change without needing to reach into component internals.
3. Falls back gracefully to the built-in checkbox when no template is provided.

#### Options Considered

**Option A (Selected): `RenderFragment<CheckboxContext>` with a typed context class**

- Approach: Introduce a nullable `RenderFragment<CheckboxContext>? CheckboxTemplate` parameter. During `RenderNodes`, when the parameter is non-null, instantiate a `CheckboxContext` from the current node's check state and inject the render fragment's output into the builder sequence. When the parameter is null, emit the default `<input type="checkbox">` as before.
- `CheckboxContext` is a model class (in `TreeViewModels.cs`) with four members: `Checked` (bool), `Indeterminate` (bool), `Disabled` (bool), and `OnChange` (Action&lt;bool&gt;).
- The `OnChange` callback wraps `ToggleItemChecked` and applies a change-guard: `ToggleItemChecked` is only called when the incoming value differs from the current checked state, preventing unnecessary state mutation on redundant calls.
- Pros: Clean separation of presentation from state; strongly typed context; zero overhead when null (default path unchanged); change-guard protects against spurious toggles.
- Cons: Consumers must understand `CheckboxContext`; `OnChange` is an `Action<bool>` rather than an `EventCallback`, which means it does not automatically invoke `StateHasChanged` — consumers' custom controls must be written accordingly.
- Effort: Small

**Option B (Not chosen): CSS/class-only customisation**

- Approach: Expose CSS class and style parameters that the internal checkbox element applies.
- Pros: Simple API.
- Cons: Cannot replace the element itself; inadequate for consumers requiring a fundamentally different control (e.g., a toggle switch, an SVG icon, or a third-party checkbox).
- Effort: Small (but insufficient scope)

**Option C (Not chosen): `ChildContent` override for the entire node**

- Approach: Allow full node content replacement via `ChildContent`.
- Pros: Maximum flexibility.
- Cons: The consumer would be responsible for all node rendering including expand icons, labels, and drag handles — far too broad for this gap. Scoped checkbox replacement is the stated requirement.
- Effort: Large

#### Decision

**Chosen:** Option A
**Rationale:** A `RenderFragment<T>` with a typed context class is the established Blazor pattern for composable slot replacement. Scoping the context to checkbox state only keeps the API surface narrow and the consumer contract explicit. The null fall-through preserves full backward compatibility.

#### Target Pattern

```razor
<!-- Default (no template) — renders built-in checkbox -->
<MariloTreeView Nodes="@nodes"
                CheckboxMode="true" />

<!-- Custom checkbox via template -->
<MariloTreeView Nodes="@nodes"
                CheckboxMode="true">
    <CheckboxTemplate Context="ctx">
        <MyCustomCheckbox Checked="@ctx.Checked"
                          Indeterminate="@ctx.Indeterminate"
                          Disabled="@ctx.Disabled"
                          OnChange="@ctx.OnChange" />
    </CheckboxTemplate>
</MariloTreeView>
```

Parameter signature (from `MariloTreeView.razor.cs`, line 128):

```csharp
[Parameter] public RenderFragment<CheckboxContext>? CheckboxTemplate { get; set; }
```

`CheckboxContext` model (from `TreeViewModels.cs`):

```csharp
public class CheckboxContext
{
    public bool Checked { get; set; }
    public bool Indeterminate { get; set; }
    public bool Disabled { get; set; }
    public Action<bool> OnChange { get; set; } = _ => { };
}
```

Render-time branching (from `MariloTreeView.razor.cs`, lines 529–558):

```csharp
if (CheckboxTemplate != null)
{
    var ctx = new CheckboxContext
    {
        Checked = checkState == true,
        Indeterminate = checkState == null,
        Disabled = Disabled || ReadOnly,
        OnChange = (val) => { if (val != (checkState == true)) ToggleItemChecked(cbId); }
    };
    builder.AddContent(30, CheckboxTemplate(ctx));
}
else
{
    // default <input type="checkbox"> rendering
}
```

#### Consequences

- No breaking change: `CheckboxTemplate` is nullable and defaults to null; existing consumers see identical output.
- `CheckboxContext.Disabled` reflects the logical OR of `Disabled` and `ReadOnly` — a node is considered disabled for editing purposes if either flag is set. Custom controls should honour this flag rather than implementing their own disabled logic.
- `OnChange` is an `Action<bool>`, not an `EventCallback<bool>`. Blazor will not automatically schedule a re-render when it is invoked. Custom controls that manage their own rendering state must call `StateHasChanged` themselves if needed; controls that are pure presentational components driven by `ctx.Checked` will re-render correctly on the next tree re-render triggered by `ToggleItemChecked`.
- The change-guard in `OnChange` (`if (val != (checkState == true))`) means that calling `OnChange` with the same value as the current state is a no-op. This prevents double-toggle bugs in controls that fire change events on mount.
- `Indeterminate` is `true` when the underlying check state is `null` (partial selection in cascading-checkbox mode). Custom controls that do not visually represent indeterminate state should treat it as unchecked.

#### Success Criteria

- [ ] `CheckboxTemplate` renders custom content instead of default checkbox (unit test)
- [ ] `CheckboxContext` provides correct `Checked` state (unit test)
- [ ] `CheckboxContext` provides correct `Indeterminate` state for partial check (unit test)
- [ ] `CheckboxContext.Disabled` reflects `Disabled` OR `ReadOnly` (unit test)
- [ ] `CheckboxContext.OnChange` triggers `ToggleItemChecked` when value changes (unit test)
- [ ] Default checkbox renders when `CheckboxTemplate` is null (unit test)

<!-- Reconstructed retroactively — implementation predates this record -->
