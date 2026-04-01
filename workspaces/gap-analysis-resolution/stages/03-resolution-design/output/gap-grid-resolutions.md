# Resolution Records: MariloGrid (Layout)

## Summary

MariloGrid had 7 gaps: missing CSS Grid Layout support (column/row/item child components), no spacing, no width, and no alignment. Resolved by extending MariloGrid with dual-mode rendering (flex container for backward compat, CSS Grid when grid parameters are used) and creating 3 new child components.

---

### RES-GRID-001: Add CSS Grid Layout support with child components

**Resolves:** GAP-1 (no GridLayoutColumn), GAP-2 (no GridLayoutRow), GAP-3 (no GridLayoutItem), GAP-4 (no ColumnSpacing), GAP-5 (no RowSpacing), GAP-6 (no Width), GAP-7 (no HorizontalAlign/VerticalAlign)
**Status:** Implemented

#### Target Pattern

Declarative child component approach:
```razor
<MariloGrid>
    <ColumnDefinitions>
        <MariloGridLayoutColumn Width="200px" />
        <MariloGridLayoutColumn Width="1fr" />
        <MariloGridLayoutColumn Width="200px" />
    </ColumnDefinitions>
    <RowDefinitions>
        <MariloGridLayoutRow Height="auto" />
        <MariloGridLayoutRow Height="1fr" />
    </RowDefinitions>
    <ChildContent>
        <MariloGridLayoutItem Row="1" Column="1" ColumnSpan="3">Header</MariloGridLayoutItem>
        <MariloGridLayoutItem Row="2" Column="1">Sidebar</MariloGridLayoutItem>
        <MariloGridLayoutItem Row="2" Column="2">Content</MariloGridLayoutItem>
    </ChildContent>
</MariloGrid>
```

Or string parameter shorthand:
```razor
<MariloGrid Columns="200px 1fr 200px" Rows="auto 1fr" ColumnSpacing="16px" RowSpacing="8px">
    <MariloGridLayoutItem Row="1" Column="1" ColumnSpan="3">Header</MariloGridLayoutItem>
    ...
</MariloGrid>
```

Backward-compatible flex mode:
```razor
<MariloGrid>
    <MariloRow>
        <MariloColumn Span="6">Left</MariloColumn>
        <MariloColumn Span="6">Right</MariloColumn>
    </MariloRow>
</MariloGrid>
```

#### Options Considered

**Option A: Dual-mode (flex container + CSS Grid) with child components**
- Approach: When grid parameters are set (Columns/Rows or child definitions), render as `display: grid`. Otherwise keep existing flex container behavior.
- Pros: Backward compatible, no breaking changes, supports both 12-column and CSS Grid patterns
- Cons: Slightly more complex component logic
- Effort: Medium

**Option B: Replace flex container entirely with CSS Grid**
- Approach: Always render as CSS Grid, deprecate MariloRow/MariloColumn usage
- Pros: Clean single-mode implementation
- Cons: Breaking change for existing usage patterns; 12-column flex grid is still useful
- Effort: Medium

#### Decision

**Chosen:** Option A
**Rationale:** The 12-column flex grid pattern (MariloGrid/Row/Column) is already working and used in demos. CSS Grid Layout is a complementary pattern, not a replacement. Dual-mode lets users choose the right layout for their needs without breaking changes.

#### New Components Created

| Component | File | Purpose |
|-----------|------|---------|
| `MariloGridLayoutColumn` | `Layout/MariloGridLayoutColumn.razor` | Defines a grid column width; registers with parent MariloGrid via CascadingValue |
| `MariloGridLayoutRow` | `Layout/MariloGridLayoutRow.razor` | Defines a grid row height; registers with parent MariloGrid via CascadingValue |
| `MariloGridLayoutItem` | `Layout/MariloGridLayoutItem.razor` | Positions content in a grid cell with Row/Column/RowSpan/ColumnSpan |

#### Parameters Added to MariloGrid

| Parameter | Type | Default | CSS Property |
|-----------|------|---------|--------------|
| `Columns` | string? | null | grid-template-columns |
| `Rows` | string? | null | grid-template-rows |
| `ColumnSpacing` | string? | null | column-gap |
| `RowSpacing` | string? | null | row-gap |
| `Width` | string? | null | width |
| `HorizontalAlign` | StackAlignment | Stretch | justify-items |
| `VerticalAlign` | StackAlignment | Stretch | align-items |
| `ColumnDefinitions` | RenderFragment? | null | (collects child GridLayoutColumn widths) |
| `RowDefinitions` | RenderFragment? | null | (collects child GridLayoutRow heights) |

#### Consequences

- No breaking changes — existing MariloGrid/Row/Column usage continues to work
- When Columns/Rows are set (or child definitions registered), grid switches to `display: grid` mode
- Child components use CascadingValue pattern to register with parent
- `IMariloCssProvider.GridClass()` interface unchanged
- All sample pages continue to compile without modification

#### Success Criteria

- [x] MariloGrid renders as CSS Grid when Columns/Rows parameters are set
- [x] MariloGridLayoutColumn registers width with parent grid
- [x] MariloGridLayoutRow registers height with parent grid
- [x] MariloGridLayoutItem positions content with Row/Column/RowSpan/ColumnSpan
- [x] ColumnSpacing/RowSpacing set CSS gap properties
- [x] Width parameter sets container width
- [x] HorizontalAlign/VerticalAlign set justify-items/align-items
- [x] Existing flex container mode (MariloRow/MariloColumn) continues to work
- [x] Full solution builds with zero errors
