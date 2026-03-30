# Gap Analysis Part 2 -- Navigation Components

## 1. MariloPagination.razor vs Pager Spec

**Spec name:** `MariloPager` | **Our name:** `MariloPagination`

- **Missing `Total` + `PageSize` model:** Spec uses `Total` (item count) and `PageSize` to compute pages automatically; we require the caller to pre-compute `TotalPages` manually.
- **Missing `Page` parameter:** Spec uses `Page` (1-based, two-way bindable); we use `CurrentPage` / `CurrentPageChanged` -- naming diverges from spec.
- **Missing `PageSizes` dropdown:** Spec provides a `PageSizes` list that renders a DropDownList for the user to change page size; we have no page-size selector at all.
- **Missing `ButtonCount`:** Spec has `ButtonCount` to control visible page buttons; we have `MaxVisiblePages` (same idea, different name).
- **Missing features:** `InputType` (numeric input mode), `Responsive` (adaptive hiding of elements), `ShowInfo` (page info text), `AdaptiveMode`, `Size` parameter -- none implemented.

## 2. MariloTimeRangeSelector.razor -- No Spec

Custom/internal component with no matching spec.

- Hardcoded range options (`Now`, `1h`, `24h`, `7d`, `30d`) -- should accept a `Ranges` parameter for customization.
- Active-state CSS class `marilo-time-range--active` is hardcoded inline instead of going through `CssProvider`.
- Two callbacks (`SelectedRangeChanged` + `OnRangeChanged`) fire on every selection -- redundant; one is two-way binding, the other is an event. Consider consolidating.

## 3. MariloToolbar.razor vs Toolbar Spec

- **Missing `OverflowMode`:** Spec supports `ToolBarOverflowMode.Menu` to show overflowing items in a popup; we have no overflow handling.
- **Missing `Size` parameter:** Spec provides `Size` for layout scaling; not implemented.
- **Missing scroll adaptive mode:** Spec has `ScrollButtonsPosition` and `ScrollButtonsVisibility`; not implemented.
- **Child component naming diverges:** Spec uses `ToolBarButton`, `ToolBarToggleButton`, `ToolBarButtonGroup`, `ToolBarSeparator`; we use `MariloToolbarButton`, `MariloToolbarGroup`, `MariloToolbarSeparator`, `MariloToolbarToggleButton`.

## 4. MariloToolbarButton.razor (part of Toolbar)

- **Missing `Icon` parameter:** Spec toolbar buttons accept an `Icon` (`ISvgIcon`); we have no icon support.
- **Missing `Overflow` parameter:** Spec supports `ToolBarItemOverflow` (`Auto`, `Always`, `Never`); not implemented.
- **Missing `OverflowText`:** Spec allows different text in overflow popup; not implemented.

## 5. MariloToolbarGroup.razor (part of Toolbar)

- **Missing `SelectionMode`:** Spec `ToolBarButtonGroup` supports `ButtonGroupSelectionMode.Single` / `Multiple` for toggle behavior; we have no selection logic.
- Current implementation is a simple `div` wrapper with no interactive behavior.

## 6. MariloToolbarSeparator.razor (part of Toolbar)

- Minimal component with `role="separator"` -- functionally adequate.
- **Missing spacer variant:** Spec mentions both separators and spacers (flexible space); we only have separators.

## 7. MariloToolbarToggleButton.razor (part of Toolbar)

- **Missing `Icon` parameter:** Same gap as `MariloToolbarButton`.
- **Parameter naming:** We use `IsActive` / `IsActiveChanged`; spec uses `Selected` / `@bind-Selected`. Should align.
- **Missing `Overflow` and `OverflowText`:** Not implemented.

## 8. MariloTreeItem.razor (part of TreeView)

- **Checkbox uses `GetItemId()` returning `Title`:** Fragile -- items with duplicate titles will collide. Spec uses a proper `Id` field.
- **Missing `Icon` support:** Spec data model supports per-item icons (`ISvgIcon`); our tree item has no icon rendering.
- **Missing link/navigation support:** Spec supports URL navigation per node; not implemented.

## 9. MariloTreeView.razor vs TreeView Spec

- **Missing `ExpandedItems` two-way binding:** Spec binds `@bind-ExpandedItems` to `IEnumerable<object>`; we use internal `HashSet<string> _expandedIds` with no external binding.
- **Missing `SelectedItems` binding:** Spec provides selection via `SelectedItems`; our component has no selection state exposed.
- **Missing `Size` parameter:** Spec supports `"sm"`, `"md"`, `"lg"`; not implemented.
- **Missing drag-and-drop:** Spec supports full drag-and-drop between trees with `OnDrag`, `OnDrop`, `DragThrottleInterval`; not implemented.
- **Missing `Rebind` method and `@ref` support:** Spec exposes methods like `Rebind()` and `GetItemFromDropIndex()`; we have no public API surface.
