# DockManager Example UX Gap List — Stage 02

**Generated:** 2026-04-11  
**Demo page reviewed:** `samples/Marilo.Demo/Pages/Components/DockManager/Overview.razor`  
**Stage 01 gaps consulted:** `stages/01-spec-review/output/dockmanager-spec-gap-list.md`  
**Spec areas:** overview, pane-types, docking-types, events, state, accessibility  

---

## Current Demo Inventory

The existing demo page (`Overview.razor`) contains two `DemoSection` blocks:

| Section Title | What It Covers | Parameters Demonstrated |
|---------------|---------------|------------------------|
| Basic Usage | Three panes; all three root events | `Height`, `Width`, `OnPaneClose`, `OnPanePin`, `OnPaneFloat`, `MariloDockPane.Id`, `MariloDockPane.Title` |
| Non-Closable Panes | One closable + one non-closable pane | `MariloDockPane.Closable` |

Both scenarios use source-accurate parameter names. No stale snippets detected (the source is a stub and the demo correctly mirrors it). No Telerik references are present.

---

## Classification Key

| Classification | Meaning |
|----------------|---------|
| Missing | No scenario exists in the demo page for this spec area or parameter |
| Partial | A scenario exists but does not fully demonstrate the parameter or is not interactive |
| Blocked-by-source | The spec area cannot be demonstrated because the source component does not implement it yet |

---

## A — Parameters with No Demo Scenario

### UX-001

| Field | Value |
|-------|-------|
| ID | UX-001 |
| Spec area | overview — MariloDockManager |
| Parameter/event | `Orientation` (`DockManagerPaneOrientation`) |
| Classification | Blocked-by-source |
| Spec location | `docs/component-specs/dockmanager/overview.md:114` |
| Source gap reference | SPEC-dockmanager-008 |
| Description | No scenario shows how the root `Orientation` parameter affects the layout. Source does not implement this parameter. |
| Suggested scenario | "Horizontal vs. Vertical Layout" — toggle between `Horizontal` and `Vertical` to show root splitter orientation change. |

---

### UX-002

| Field | Value |
|-------|-------|
| ID | UX-002 |
| Spec area | pane-types — DockManagerContentPane |
| Parameter/event | `AllowFloat`, `Dockable`, `Maximizable`, `Unpinnable`, `Unpinned`, `UnpinnedSize`, `Visible`, `Size` |
| Classification | Blocked-by-source |
| Spec location | `docs/component-specs/dockmanager/overview.md:119–132` |
| Source gap reference | SPEC-dockmanager-009 |
| Description | `DockManagerContentPane` does not exist in source. All 11 of its parameters have no demo coverage. |
| Suggested scenario | "Content Pane Configuration" — show a pane with toggles for `Unpinnable`, `Closeable`, `Dockable`, `Maximizable`, and `Visible`. |

---

### UX-003

| Field | Value |
|-------|-------|
| ID | UX-003 |
| Spec area | pane-types — DockManagerSplitPane |
| Parameter/event | `AllowEmpty`, `Orientation`, `Size`, `FloatingHeight`, `FloatingLeft`, `FloatingResizable`, `FloatingTop`, `FloatingWidth` |
| Classification | Blocked-by-source |
| Spec location | `docs/component-specs/dockmanager/overview.md:134–153` |
| Source gap reference | SPEC-dockmanager-010 |
| Description | `DockManagerSplitPane` does not exist in source. None of its parameters or floating-window parameters are demonstrated. |
| Suggested scenario | "Resizable Split Layout" — horizontal and vertical split with visible splitter handles; "Floating Split Pane" — a floating split pane with configurable position and size. |

---

### UX-004

| Field | Value |
|-------|-------|
| ID | UX-004 |
| Spec area | pane-types — DockManagerTabGroupPane |
| Parameter/event | `AllowEmpty`, `SelectedPaneId`, `Size`, `Visible` |
| Classification | Blocked-by-source |
| Spec location | `docs/component-specs/dockmanager/overview.md:155–163` |
| Source gap reference | SPEC-dockmanager-011 |
| Description | `DockManagerTabGroupPane` does not exist in source. No scenario shows tab-group pane behavior. |
| Suggested scenario | "Tabbed Pane Group" — demonstrate tab navigation, `AllowEmpty` when all tabs are removed, and `SelectedPaneId` for programmatic tab selection. |

---

### UX-005

| Field | Value |
|-------|-------|
| ID | UX-005 |
| Spec area | overview — DockManagerContentPane |
| Parameter/event | `HeaderText` |
| Classification | Blocked-by-source |
| Spec location | `docs/component-specs/dockmanager/overview.md:125` |
| Source gap reference | SPEC-dockmanager-005 |
| Description | The spec uses `HeaderText` for the pane title; source uses `Title`. The demo uses `Title`, which matches source but not the spec. When the naming is reconciled, the demo will need updating. |
| Suggested scenario | Update the existing "Basic Usage" scenario to use `HeaderText` once the parameter is renamed. |

---

## B — Parameters with Stale or Incomplete Code Snippets

### UX-006

| Field | Value |
|-------|-------|
| ID | UX-006 |
| Spec area | pane-types |
| Parameter/event | `MariloDockPane.Closable` vs spec `Closeable` |
| Classification | Partial (will become stale when name is reconciled) |
| Demo section | "Non-Closable Panes" (`Overview.razor:40–54`) |
| Spec location | `docs/component-specs/dockmanager/overview.md:123` |
| Source gap reference | SPEC-dockmanager-006 |
| Description | The demo uses `Closable="false"` which matches current source spelling. The spec spells it `Closeable`. When the spelling is standardized, the snippet will become stale and must be updated. |
| Suggested action | Track this as a follow-up; update snippet when spelling is resolved in source. |

---

### UX-007

| Field | Value |
|-------|-------|
| ID | UX-007 |
| Spec area | overview |
| Parameter/event | `MariloDockManager.Height` default |
| Classification | Partial |
| Demo section | "Basic Usage" (`Overview.razor:7`) uses `Height="300px"` |
| Spec location | `docs/component-specs/dockmanager/overview.md:113` |
| Source gap reference | SPEC-dockmanager-007 |
| Description | The demo always supplies an explicit `Height`. No scenario demonstrates the auto-height behavior described in the spec. The source default of `"500px"` means omitting `Height` currently renders at 500 px, not auto-sized. A demo showing both explicit and auto-height would be clearer. |
| Suggested scenario | "Auto-Height Dock Manager" — show `MariloDockManager` without an explicit `Height`, with a note that the component expands to fill its container. |

---

## C — Events with No Demo Scenario

### UX-008

| Field | Value |
|-------|-------|
| ID | UX-008 |
| Spec area | events |
| Parameter/event | `OnDock` / `DockManagerDockEventArgs` |
| Classification | Blocked-by-source |
| Spec location | `docs/component-specs/dockmanager/events.md:27–37` |
| Source gap reference | SPEC-dockmanager-014 |
| Description | No demo scenario shows drag-and-dock and the `OnDock` event. Source has no drag-and-dock engine. |
| Suggested scenario | "Dock a Floating Pane" — drag a floating pane to a docked position; show event log entry; cancel docking onto Pane 1 via `IsCancelled`. |

---

### UX-009

| Field | Value |
|-------|-------|
| ID | UX-009 |
| Spec area | events |
| Parameter/event | `OnUndock` / `DockManagerUndockEventArgs` |
| Classification | Blocked-by-source |
| Spec location | `docs/component-specs/dockmanager/events.md:39–49` |
| Source gap reference | SPEC-dockmanager-015 |
| Description | Source has `OnPaneFloat` but not `OnUndock` with `IsCancelled`. No scenario demonstrates cancelling an undock operation. |
| Suggested scenario | "Cancel Undock" — attempt to undock a protected pane; `OnUndock` handler sets `IsCancelled = true`; pane stays docked. |

---

### UX-010

| Field | Value |
|-------|-------|
| ID | UX-010 |
| Spec area | events |
| Parameter/event | `VisibleChanged` (per `DockManagerContentPane`) |
| Classification | Blocked-by-source |
| Spec location | `docs/component-specs/dockmanager/events.md:51–53` |
| Source gap reference | SPEC-dockmanager-016 |
| Description | No scenario demonstrates `VisibleChanged` with two-way binding on a pane's `Visible` parameter. Source pane has no `Visible` parameter. |
| Suggested scenario | "Show/Hide Pane" — button toggles pane `Visible`; `VisibleChanged` logs state; a restore button re-shows closed panes. |

---

### UX-011

| Field | Value |
|-------|-------|
| ID | UX-011 |
| Spec area | events |
| Parameter/event | `SizeChanged` (per pane) |
| Classification | Blocked-by-source |
| Spec location | `docs/component-specs/dockmanager/events.md:55–57` |
| Source gap reference | SPEC-dockmanager-017 |
| Description | No scenario demonstrates `SizeChanged` fired when a pane's `Size` parameter changes. Source has no `Size` parameter. |
| Suggested scenario | "Respond to Pane Resize" — resize a pane; `SizeChanged` callback updates a status label with the new size. |

---

### UX-012

| Field | Value |
|-------|-------|
| ID | UX-012 |
| Spec area | events |
| Parameter/event | `UnpinnedChanged` / `UnpinnedSizeChanged` |
| Classification | Blocked-by-source |
| Spec location | `docs/component-specs/dockmanager/events.md:59–63` |
| Source gap reference | SPEC-dockmanager-018 |
| Description | No scenario demonstrates pinning/unpinning with two-way binding and changed events. Source has no unpin concept. |
| Suggested scenario | "Pin and Unpin a Pane" — toggle `Unpinned`; show the pane sliding out as a toolbar strip; `UnpinnedChanged` and `UnpinnedSizeChanged` update event log. |

---

### UX-013

| Field | Value |
|-------|-------|
| ID | UX-013 |
| Spec area | events |
| Parameter/event | `OnPaneResize` / `DockManagerPaneResizeEventArgs` |
| Classification | Blocked-by-source |
| Spec location | `docs/component-specs/dockmanager/events.md:65–75` |
| Source gap reference | SPEC-dockmanager-019 |
| Description | No scenario demonstrates the root-level `OnPaneResize` event. Source has no resize event. |
| Suggested scenario | "Refresh Nested Component on Pane Resize" — embed a chart inside a pane; `OnPaneResize` calls chart `Refresh()` to repaint. |

---

### UX-014

| Field | Value |
|-------|-------|
| ID | UX-014 |
| Spec area | events |
| Parameter/event | `OnPin` / `DockManagerPinEventArgs` |
| Classification | Partial (source has `OnPanePin` but wrong signature) |
| Demo section | "Basic Usage" (`Overview.razor:9`) — `OnPanePin="HandlePin"` |
| Spec location | `docs/component-specs/dockmanager/events.md:88–97` |
| Source gap reference | SPEC-dockmanager-025 |
| Description | The demo wires `OnPanePin` (source event, `EventCallback<string>`). Spec requires `OnPin` with `DockManagerPinEventArgs` and `IsCancelled` support. The demo does not show cancellation. |
| Suggested action | Update demo to use `OnPin` and show cancellation once source is updated. |

---

### UX-015

| Field | Value |
|-------|-------|
| ID | UX-015 |
| Spec area | events |
| Parameter/event | `OnUnpin` / `DockManagerUnpinEventArgs` |
| Classification | Blocked-by-source |
| Spec location | `docs/component-specs/dockmanager/events.md:99–108` |
| Source gap reference | SPEC-dockmanager-026 |
| Description | No demo scenario for `OnUnpin`. Source has no unpin event. |
| Suggested scenario | "Cancel Unpin" — cancel unpinning a specific pane via `IsCancelled = true` in `OnUnpin` handler. |

---

### UX-016

| Field | Value |
|-------|-------|
| ID | UX-016 |
| Spec area | state |
| Parameter/event | `OnStateInit` / `OnStateChanged` |
| Classification | Blocked-by-source |
| Spec location | `docs/component-specs/dockmanager/state.md:33–44` |
| Source gap reference | SPEC-dockmanager-020, SPEC-dockmanager-021 |
| Description | No demo scenario shows state persistence (save to `localStorage`, restore on init). Source has no state model. |
| Suggested scenario | "Persist DockManager Layout" — serialize state on `OnStateChanged`; restore on `OnStateInit`; demonstrate page refresh preserving layout. |

---

## D — Edge Cases Not Demonstrated

### UX-017

| Field | Value |
|-------|-------|
| ID | UX-017 |
| Spec area | pane-types |
| Parameter/event | Empty/no-pane state |
| Classification | Missing |
| Spec location | `docs/component-specs/dockmanager/overview.md` — `DockManagerSplitPane.AllowEmpty`, `DockManagerTabGroupPane.AllowEmpty` |
| Description | No scenario shows what happens when all panes in a split or tab group are removed (closed/undocked). `AllowEmpty` controls whether an empty container remains. This edge case is undemonstrated and `AllowEmpty` is blocked-by-source. |
| Suggested scenario | "Empty Pane Containers" — close all tabs in a group; show the empty drop zone when `AllowEmpty=true` vs. re-rendered layout when `false`. |

---

### UX-018

| Field | Value |
|-------|-------|
| ID | UX-018 |
| Spec area | docking-types |
| Parameter/event | Global docking navigator / inner docking navigator |
| Classification | Blocked-by-source |
| Spec location | `docs/component-specs/dockmanager/docking-types.md:17–31` |
| Description | No scenario demonstrates the global or inner docking navigator that appears during drag. Source has no drag-and-dock implementation. |
| Suggested scenario | "Dock to Edge vs. Drop in Tab" — drag a floating pane; hover over the global navigator to dock to a root edge; hover over an inner navigator to create a tab. |

---

### UX-019

| Field | Value |
|-------|-------|
| ID | UX-019 |
| Spec area | state |
| Parameter/event | Programmatic `GetState` / `SetState` via `@ref` |
| Classification | Blocked-by-source |
| Spec location | `docs/component-specs/dockmanager/overview.md:166–189` |
| Source gap reference | SPEC-dockmanager-022, SPEC-dockmanager-023 |
| Description | No demo scenario uses a component `@ref` to call `GetState()` / `SetState()`. Source has neither method. |
| Suggested scenario | "Save and Restore Layout Programmatically" — two buttons: Get State (serialize and display as JSON), Set State (apply a pre-defined state). |

---

### UX-020

| Field | Value |
|-------|-------|
| ID | UX-020 |
| Spec area | accessibility |
| Parameter/event | Keyboard navigation / ARIA roles |
| Classification | Missing |
| Spec location | `docs/component-specs/dockmanager/accessibility/wai-aria-support.md` |
| Description | No demo scenario demonstrates keyboard navigation, focus management, or ARIA roles. The spec documents `role=application` on the dock manager and `aria-live=polite` for screen reader announcements. |
| Suggested scenario | "Keyboard Navigation" — narrative section (no interactive demo needed) showing keyboard shortcuts for moving between panes, invoking actions, and using the dock navigator. |

---

## Gap Count Summary

| Classification | Count |
|----------------|-------|
| Missing | 6 (UX-001 through UX-005, UX-017, UX-018, UX-019, UX-020 — grouped) |
| Partial | 3 (UX-006, UX-007, UX-014) |
| Blocked-by-source | 14 (UX-001, UX-002, UX-003, UX-004, UX-008, UX-009, UX-010, UX-011, UX-012, UX-013, UX-015, UX-016, UX-018, UX-019) |
| **Total distinct gap records** | **20** |

> Note: most "Missing" items are also "Blocked-by-source". Items classified as Blocked-by-source cannot be addressed by updating the demo alone; the corresponding source gap (from Stage 01) must be resolved first.

## Parameters with Adequate Coverage (No Action Needed)

| Parameter | Demo Section | Notes |
|-----------|-------------|-------|
| `MariloDockManager.Height` | "Basic Usage" | Demonstrated with explicit value; auto-height scenario missing (UX-007) |
| `MariloDockManager.Width` | "Basic Usage" | Demonstrated |
| `MariloDockManager.OnPaneClose` | "Basic Usage" | Demonstrated (source API); needs update when reconciled to spec |
| `MariloDockManager.OnPanePin` | "Basic Usage" | Partially demonstrated (source API); see UX-014 |
| `MariloDockManager.OnPaneFloat` | "Basic Usage" | Demonstrated (source API); see UX-009 for spec equivalent |
| `MariloDockPane.Id` | "Basic Usage" | Demonstrated |
| `MariloDockPane.Title` | "Basic Usage" | Demonstrated (will need rename to `HeaderText` per SPEC-dockmanager-005) |
| `MariloDockPane.Closable` | "Non-Closable Panes" | Demonstrated (spelling pending — see UX-006) |
| `MariloDockPane.ChildContent` | Both sections | Demonstrated |
