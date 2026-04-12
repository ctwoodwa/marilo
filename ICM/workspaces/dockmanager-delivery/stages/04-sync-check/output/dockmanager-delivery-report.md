# DockManager GREEN Delivery Report — Stage 04 Sync Check

**Date:** 2026-04-12
**Component:** MariloDockManager
**Status:** GREEN

## Summary

MariloDockManager is a full-featured dock manager component with:

- **Pane hierarchy** — flat pane list with logical tab groups via `TabGroupId`
- **State management** — internal pane registration/unregistration with layout change events
- **Tab drag reorder** — drag-and-drop reordering within a tab group via HTML5 drag events
- **Cross-pane tab move** — drag a tab from one tab group to another; `OnPaneMoved` EventCallback with source/target group IDs
- **Floating panes** — panes toggle between docked (tab strip) and floating (overlay) states with title bar, dock/close buttons
- **Drag-move** — floating panes can be moved by dragging the title bar
- **Resize** — 8-direction resize handles on floating panes with min width/height constraints (`MinWidth`/`MinHeight` parameters)
- **Min/max pane constraints** — `MinWidth`/`MinHeight` on `DockPaneDescriptor` and `MariloDockPane` (default "100px"), enforced during resize

**Deferred (non-blocking enhancement):** JS-based drag-docking with dock indicators, snap zones, and true pane hierarchy (split/tabgroup nesting). These require JS interop and are tracked as separate gap items.

## Build Verification

```
dotnet build Marilo.slnx
Build succeeded.
    0 Warning(s)
    0 Error(s)
Time Elapsed 00:00:02.50
```

## Test Verification

```
dotnet test tests/Marilo.Tests.Unit --filter "FullyQualifiedName~DockManager"
Passed!  - Failed: 0, Passed: 25, Skipped: 0, Total: 25, Duration: 377 ms
```

### Test Inventory (25 tests)

| # | Test | Category |
|---|------|----------|
| 1 | FloatingPane_RendersAsOverlay_WithFloatingPaneClass | Floating |
| 2 | FloatingPane_CloseButton_RemovesPane | Floating |
| 3 | ToggleFloat_MovesPaneBetweenDockedAndFloating | Floating |
| 4 | FloatingPane_RespectsPositionAndSize | Floating |
| 5 | FloatingPane_HasTitleBar_WithTitle | Floating |
| 6 | FloatingPane_DoesNotAppearInTabStrip | Floating |
| 7 | FloatingPane_DockButton_ReturnsPaneToTabStrip | Floating |
| 8 | ToggleFloat_InvokesOnPaneFloatCallback | Events |
| 9 | ToggleFloat_InvokesOnLayoutChangedCallback | Events |
| 10 | TabClick_InvokesOnPaneActivatedCallback | Events |
| 11 | CloseLastTab_LeavesTabStripEmpty | Close |
| 12 | NestedSplitWithFloating_BothRender | Layout |
| 13 | TitleBarMouseDown_ShowsDragOverlay | Drag-Move |
| 14 | DragMove_UpdatesFloatingPanePosition | Drag-Move |
| 15 | FloatingPane_RendersResizeHandles | Resize |
| 16 | DragMove_AppliesDraggingCssClass | Drag-Move |
| 17 | DragMove_EndsOnMouseUp | Drag-Move |
| 18 | ResizeHandle_MouseDown_ShowsOverlayAndResizingClass | Resize |
| 19 | ReorderSingleTab_IsNoOp | Reorder |
| 20 | CrossPaneMove_FiresOnPaneMoved | Cross-Pane Move (NEW) |
| 21 | CrossPaneMove_UpdatesTabGroupId | Cross-Pane Move (NEW) |
| 22 | Resize_RespectsMinWidthConstraint | Min Constraints (NEW) |
| 23 | Resize_RespectsMinHeightConstraint | Min Constraints (NEW) |
| 24 | TabGroups_RenderWithGroupIdAttribute | Tab Groups (NEW) |
| 25 | Resize_UsesDefaultMinConstraints | Min Constraints (NEW) |

## Sync Areas

| Area | Status | Details |
|------|--------|---------|
| Source | PASS | `MariloDockManager.razor`, `MariloDockPane.razor`, `DockManagerModels.cs` updated |
| Spec | PASS | `overview.md` and `events.md` updated with OnPaneMoved, TabGroupId, MinWidth/MinHeight |
| Demo | PASS | Cross-Pane Move demo section added; existing demos verified |
| Tests | PASS | 6 new tests added (25 total), all passing |
| SCSS | PASS | Both FluentUI and Bootstrap providers have full BEM coverage |

## Files Changed

- `src/Marilo.Core/Models/DockManagerModels.cs` — Added `MinWidth`/`MinHeight` to `DockPaneDescriptor`; added `DockPaneMoveEventArgs`
- `src/Marilo.Components/Layout/MariloDockManager.razor` — Added `OnPaneMoved` EventCallback, cross-pane move logic, tab group rendering, min constraint enforcement
- `src/Marilo.Components/Layout/MariloDockPane.razor` — Added `TabGroupId`, `MinWidth`, `MinHeight` parameters
- `tests/Marilo.Tests.Unit/Layout/DockManagerFloatingTests.cs` — 6 new tests
- `samples/Marilo.Demo/Pages/Components/DockManager/Overview.razor` — Cross-Pane Move demo section
- `docs/component-specs/dockmanager/overview.md` ��� Documented new parameters and event
- `docs/component-specs/dockmanager/events.md` — Documented OnPaneMoved event
