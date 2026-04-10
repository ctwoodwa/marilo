# Resolution Design: DG-P3-01 — Frozen/Locked Columns

## Target State

Columns can be pinned left or right via a `Locked` parameter. Pinned columns stick during horizontal scroll using `position:sticky` CSS.

## Design

### Parameters (MariloGridColumn)

```csharp
/// <summary>When true, the column is frozen and sticks during horizontal scroll.</summary>
[Parameter] public bool Locked { get; set; }

/// <summary>When Locked=true, the side to pin to. Default: Start (left in LTR).</summary>
[Parameter] public GridColumnFrozenPosition FrozenPosition { get; set; } = GridColumnFrozenPosition.Start;
```

### Enum

```csharp
public enum GridColumnFrozenPosition { Start, End }
```

### Sizing Contract Extension

Extend `ColumnSizingEntry` with `Locked` and `FrozenPosition` properties. Extend `GridLayoutContract` with:
- `FrozenOffsets: Dictionary<string, double>` — cumulative pixel offset for each frozen column
- Left-frozen columns: offset = sum of widths of all preceding left-frozen columns
- Right-frozen columns: offset = sum of widths of all following right-frozen columns

`FixedWidthProvider.Resolve()` computes these offsets when building the contract.

### CSS Application

In `GetColumnCellStyle(column)` and the `<colgroup>` rendering:
- When `column.Locked`:
  - Add `position:sticky;` 
  - Left-frozen: `left:{offset}px;`
  - Right-frozen: `right:{offset}px;`
  - Add `z-index:2;` (headers get `z-index:3`)
  - Add `background:var(--marilo-color-surface,#fff);` (prevents see-through during scroll)

CSS class: `mar-datagrid-col--locked` and `mar-datagrid-col--locked-end`

### Rendering Changes (MariloDataGrid.razor)

1. **`<colgroup>`**: No special handling needed (sticky is on cells, not cols)
2. **`<thead>` header cells**: When column is locked, apply sticky styles + z-index:3
3. **Filter row cells**: Same sticky styles
4. **`<tbody>` data cells**: In `RenderDataRow` (Rendering.cs), apply sticky styles + z-index:2
5. **`<tfoot>` footer cells**: Same sticky styles

### Frozen Column Width Constraint

Frozen columns MUST have explicit pixel widths. If a locked column has `Width=null` or a non-pixel width, default to `150px` and log a warning. `position:sticky` with `auto` width causes layout instability.

### JS Changes

Minimal. The existing `initResize` and `initReorder` code should:
- Skip frozen columns from column reorder (frozen columns don't move)
- Allow resizing frozen columns (resize works fine with sticky)
- After a frozen column resize, call a new callback `OnFrozenColumnResized` so C# can recalculate sticky offsets for subsequent columns

Add to the IIFE: `getDataColumnIndex` already works correctly (skips special cells, returns 0-based data index). No changes needed there.

### Tests

- Column renders with Locked=true, has sticky CSS
- Locked=false (default), no sticky CSS
- FrozenPosition.Start/End applies correct left/right
- Multiple frozen columns get correct cumulative offsets
- Frozen column with no explicit width gets default 150px
- FixedWidthProvider computes frozen offsets correctly

## Decision Rationale

- **`position:sticky` on cells** rather than separate table/div: preserves single-table structure, no markup restructuring
- **Sticky offsets computed in C#** rather than JS: server-side calculation is deterministic, avoids JS measurement round-trips
- **Explicit pixel width constraint**: sticky positioning requires predictable widths; auto-sized sticky columns cause layout thrashing
