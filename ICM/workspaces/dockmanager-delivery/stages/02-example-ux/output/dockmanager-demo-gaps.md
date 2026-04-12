# DockManager Demo Gaps — Stage 02 Example UX

**Date:** 2026-04-12
**Worker:** w-dockmanager-delivery
**Component:** MariloDockManager

## Current Demo Coverage

**File:** `samples/Marilo.Demo/Pages/Components/DockManager/Overview.razor`

The demo page has two sections:

| # | Section | What It Shows |
|---|---|---|
| 1 | Basic Usage | 3 panes (Explorer, Properties, Output) with close/pin/float action callbacks |
| 2 | Non-Closable Panes | 2 panes demonstrating `Closable=false` |

## Missing Demo Scenarios

Based on the spec feature surface and what a comprehensive demo page should cover:

| # | Missing Scenario | Spec Feature | Priority | Notes |
|---|---|---|---|---|
| DG-1 | Split pane layout | `DockManagerSplitPane` with vertical/horizontal orientation | Critical | Core layout feature — not implementable until SA-2 is resolved |
| DG-2 | Tab group pane | `DockManagerTabGroupPane` with multiple tabs | Critical | Not implementable until SA-3 is resolved |
| DG-3 | Nested pane hierarchy | Split pane containing tab groups and content panes | Critical | Requires SA-1, SA-2, SA-3 |
| DG-4 | Floating panes | `DockManagerFloatingPanes` with draggable windows | Critical | Requires SA-4 |
| DG-5 | Drag-and-drop docking | User drags pane to dock at edge or center | Critical | Requires SA-5 |
| DG-6 | Pin/unpin with toolbar | Pane unpins to sidebar, shows in toolbar | Major | Requires SA-6 |
| DG-7 | State save/restore | `OnStateInit`/`OnStateChanged` with localStorage | Major | Requires SA-7 |
| DG-8 | State get/set via methods | `GetState()`/`SetState()` buttons | Major | Requires SA-7 |
| DG-9 | Event log with cancellation | All events shown in a log, some cancelled via `IsCancelled` | Major | Requires SA-8 |
| DG-10 | Maximize pane | Click maximize button on a pane | Moderate | Requires SA-9 |
| DG-11 | Disable docking per pane | `Dockable=false` on individual panes | Moderate | Requires SA-5 |
| DG-12 | Visible toggle | Show/hide panes programmatically | Moderate | Requires SA-13 |
| DG-13 | Orientation control | Toggle root splitter orientation | Moderate | Requires SA-12 |
| DG-14 | IDE-style layout | Full IDE-like layout (explorer, editor, properties, output, terminal) | Nice-to-have | Showcase scenario |
| DG-15 | Accessibility demo | Keyboard navigation through panes and tabs | Moderate | Requires SA-14 |

## Existing Demo Quality Assessment

| Criterion | Status | Notes |
|---|---|---|
| Code samples shown | Yes | `_basicCode` and `_nonClosableCode` constants provided |
| Event handling demonstrated | Partial | Close/pin/float callbacks shown but only as text log |
| Interactive controls | No | No parameter toggles, no runtime configuration |
| Realistic content | Partial | IDE-like content in basic demo is reasonable |
| Error/edge case coverage | No | No empty state, no single-pane, no all-closed scenario |

## Conclusion

The demo covers only the **current source capabilities** (basic tabbed panel + closable toggle). It cannot demonstrate spec features that are not yet implemented. 13 of 15 missing scenarios are blocked on source implementation gaps from Stage 01. The demo page structure is sound and can be extended once source catches up.
