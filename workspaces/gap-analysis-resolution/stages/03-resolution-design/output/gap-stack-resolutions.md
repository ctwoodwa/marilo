# Resolution Records: MariloStack

## Summary

MariloStack had 5 gaps around missing parameters, naming mismatches, and a single alignment axis. All gaps resolved through component refactoring and CSS provider interface simplification.

---

### RES-STACK-001: Add Spacing, Width, Height parameters and two-axis alignment

**Resolves:** GAP-1 (no Spacing), GAP-2 (no Width/Height), GAP-3 (single alignment axis), GAP-4 (default orientation), GAP-5 (Direction→Orientation naming)
**Status:** Implemented

#### Target Pattern

```razor
<MariloStack Orientation="StackDirection.Horizontal"
             Spacing="16px"
             Width="100%"
             HorizontalAlign="StackAlignment.Center"
             VerticalAlign="StackAlignment.Center">
    @* children *@
</MariloStack>
```

Component renders a `<div>` with:
- CSS class from provider (handles flex direction)
- Inline styles for `gap`, `width`, `height`, `justify-content`, `align-items`
- `HorizontalAlign`/`VerticalAlign` mapped to correct flex axis based on orientation

#### Options Considered

**Option A: Inline styles for alignment, sizing, spacing; simplified provider interface**
- Approach: Provider handles direction class only; component generates alignment/sizing via inline styles
- Pros: No need to change provider interface for every new parameter; alignment is two-axis; clean separation
- Cons: Alignment not theme-provider-controlled (but this is correct — alignment is layout, not theme)
- Effort: Small

**Option B: Extend provider interface with all new parameters**
- Approach: `StackClass(orientation, hAlign, vAlign, spacing, width, height)`
- Pros: Full provider control
- Cons: Complex interface, providers must handle CSS for generic values (spacing/width/height are arbitrary CSS values)
- Effort: Medium

#### Decision

**Chosen:** Option A
**Rationale:** Spacing, width, height are arbitrary CSS values best handled as inline styles. Alignment mapping (horizontal→justify-content vs align-items) depends on orientation, which is component logic. The provider's role is to supply the base flex class and direction.

#### Changes Made

| File | Change |
|------|--------|
| `IMariloCssProvider.cs` | `StackClass(StackDirection, StackAlignment)` → `StackClass(StackDirection)` |
| `BootstrapCssProvider.cs` | Removed alignment classes, simplified to direction-only |
| `FluentUICssProvider.cs` | Removed alignment class, simplified to direction-only |
| `MariloStack.razor` | Added `Orientation`, `HorizontalAlign`, `VerticalAlign`, `Spacing`, `Width`, `Height`; removed `Direction`, `Alignment`; added `BuildStackStyles()` for inline styles |
| Sample pages (3 files) | Updated to use new parameter names |

#### Consequences

- **Breaking change**: `Direction` → `Orientation`, `Alignment` → `HorizontalAlign`/`VerticalAlign`
- Default orientation changed from `Vertical` to `Horizontal` (matches spec)
- All 3 sample pages updated
- `IMariloCssProvider.StackClass` signature simplified (breaking for custom providers)

#### Success Criteria

- [x] `Spacing` parameter sets CSS `gap` property
- [x] `Width`/`Height` parameters set inline styles
- [x] `HorizontalAlign`/`VerticalAlign` correctly map to `justify-content`/`align-items` based on orientation
- [x] Default orientation is `Horizontal`
- [x] Parameter named `Orientation` (not `Direction`)
- [x] Solution builds with zero errors and no new warnings
- [x] Sample pages updated and compile
