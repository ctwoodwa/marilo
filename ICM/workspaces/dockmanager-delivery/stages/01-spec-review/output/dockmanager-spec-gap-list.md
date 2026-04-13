# DockManager Spec Gap List — Stage 01

**Generated:** 2026-04-11  
**Source files reviewed:**  
- `src/Marilo.Components/Layout/MariloDockManager.razor` (lines 1–101)  
- `src/Marilo.Components/Layout/MariloDockPane.razor` (lines 1–35)  

**Spec files reviewed:**  
- `docs/component-specs/dockmanager/overview.md`  
- `docs/component-specs/dockmanager/events.md`  
- `docs/component-specs/dockmanager/state.md`  
- `docs/component-specs/dockmanager/docking-types.md`  
- `docs/component-specs/dockmanager/pane-types.md`  
- `docs/component-specs/dockmanager/accessibility/wai-aria-support.md`  

**Summary:** The source is a stub implementation using a flat tab-strip architecture (`MariloDockManager` + `MariloDockPane`). The spec describes a multi-component hierarchy (`MariloDockManager`, `DockManagerContentPane`, `DockManagerSplitPane`, `DockManagerTabGroupPane`, `DockManagerPanes`, `DockManagerFloatingPanes`). The architectures are fundamentally different. All spec-described sub-components are missing from source entirely. Several source parameters/events have no spec counterpart.

---

## Section 1 — Undocumented (in source, not in spec)

---

### SPEC-dockmanager-001

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-001 |
| Feature area | overview |
| Parameter/event | `OnPaneClose` (`EventCallback<string>`) |
| Gap type | undocumented |
| Source location | `src/Marilo.Components/Layout/MariloDockManager.razor:43` |
| Spec location | missing |
| Description | `OnPaneClose` fires when a pane is closed, passing the pane id. The spec describes `VisibleChanged` per-pane (on `DockManagerContentPane`) but has no `OnPaneClose` event on `MariloDockManager` itself. |
| Priority | P1 |
| Priority rationale | Public API on the root component; callers use it to detect pane closure. |
| Suggested resolution | Add `OnPaneClose` to the overview.md parameters table or reconcile it with the spec's `VisibleChanged` / `OnUndock` pattern. |

---

### SPEC-dockmanager-002

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-002 |
| Feature area | overview |
| Parameter/event | `OnPanePin` (`EventCallback<string>`) |
| Gap type | undocumented |
| Source location | `src/Marilo.Components/Layout/MariloDockManager.razor:46` |
| Spec location | missing |
| Description | `OnPanePin` fires when the user clicks the Pin button in the tab header. The spec has `OnPin` (on `MariloDockManager`, receiving `DockManagerPinEventArgs`), but does not describe a root-level `OnPanePin` that passes a raw `string` id. The signatures differ. |
| Priority | P1 |
| Priority rationale | Public API event; callers bind to this to react to pin actions. |
| Suggested resolution | Reconcile with spec's `OnPin` / `DockManagerPinEventArgs` — update spec or source to align signatures. |

---

### SPEC-dockmanager-003

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-003 |
| Feature area | overview |
| Parameter/event | `OnPaneFloat` (`EventCallback<string>`) |
| Gap type | undocumented |
| Source location | `src/Marilo.Components/Layout/MariloDockManager.razor:49` |
| Spec location | missing |
| Description | `OnPaneFloat` fires when the user clicks the Float button on a tab. The spec describes floating panes via `DockManagerFloatingPanes` / `DockManagerSplitPane` and `OnUndock`, but has no `OnPaneFloat` event with a raw `string` id on the root component. |
| Priority | P1 |
| Priority rationale | Public API; used in the demo page. |
| Suggested resolution | Map to or replace with spec's `OnUndock` / `DockManagerUndockEventArgs`, or document as a separate event. |

---

### SPEC-dockmanager-004

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-004 |
| Feature area | overview |
| Parameter/event | `MariloDockPane` component (entire component) |
| Gap type | undocumented |
| Source location | `src/Marilo.Components/Layout/MariloDockPane.razor:1–35` |
| Spec location | missing |
| Description | The source implements a single `MariloDockPane` component with parameters `Title`, `Id`, `ChildContent`, `Closable`. The spec describes three distinct pane types: `DockManagerContentPane`, `DockManagerSplitPane`, and `DockManagerTabGroupPane`. `MariloDockPane` is not mentioned anywhere in the spec. |
| Priority | P1 |
| Priority rationale | Core pane API; all demos use `MariloDockPane`. |
| Suggested resolution | Either document `MariloDockPane` as a unified pane abstraction in the spec, or replace source with the three-component hierarchy described in spec. |

---

### SPEC-dockmanager-005

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-005 |
| Feature area | pane-types |
| Parameter/event | `MariloDockPane.Title` (`string?`) |
| Gap type | undocumented |
| Source location | `src/Marilo.Components/Layout/MariloDockPane.razor:12` |
| Spec location | missing |
| Description | `MariloDockPane.Title` is used as the tab label. The spec uses `HeaderText` on `DockManagerContentPane` for the same purpose. No spec parameter named `Title` exists on any pane type. |
| Priority | P1 |
| Priority rationale | Visible tab label; directly user-facing. |
| Suggested resolution | Rename source parameter to `HeaderText` to match spec, or add `Title` to spec as an alias. |

---

### SPEC-dockmanager-006

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-006 |
| Feature area | pane-types |
| Parameter/event | `MariloDockPane.Closable` (`bool`, default `true`) |
| Gap type | mismatch |
| Source location | `src/Marilo.Components/Layout/MariloDockPane.razor:21` |
| Spec location | `docs/component-specs/dockmanager/overview.md` — DockManagerContentPane Parameters table |
| Description | Source uses `Closable` (single-L). Spec uses `Closeable` (double-E). The default values also differ: source default is `true`; spec states default is `true` as well, so the default matches — but the spelling diverges. |
| Priority | P1 |
| Priority rationale | Parameter name is part of the public API; a spelling mismatch causes a compile-time error for spec-following consumers. |
| Suggested resolution | Standardize spelling. The spec uses `Closeable`; align source to `Closeable`. |

---

### SPEC-dockmanager-007

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-007 |
| Feature area | overview |
| Parameter/event | `MariloDockManager.Height` default value (`"500px"`) |
| Gap type | mismatch |
| Source location | `src/Marilo.Components/Layout/MariloDockManager.razor:37` |
| Spec location | `docs/component-specs/dockmanager/overview.md:113` |
| Description | Source defaults `Height` to `"500px"`. The spec states "If not set, the component will expand automatically to cover the available space", implying the default is `null` / unset. |
| Priority | P2 |
| Priority rationale | Default behavior affects layout; an IDE user reading the spec will expect auto-expansion, not a fixed 500 px. |
| Suggested resolution | Change source default to `null` and let CSS handle auto-expansion, or document the 500 px default explicitly in the spec. |

---

## Section 2 — Spec-Ahead (in spec, not in source)

The following are grouped by feature area. All spec-ahead items share the root cause: the spec describes a rich multi-component architecture that does not yet exist in source.

---

### SPEC-dockmanager-008

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-008 |
| Feature area | overview |
| Parameter/event | `MariloDockManager.Orientation` (`DockManagerPaneOrientation`, default `Vertical`) |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/overview.md:114` |
| Description | Spec documents an `Orientation` parameter on the root `MariloDockManager`. Source has no such parameter. |
| Priority | P1 |
| Priority rationale | Root-level layout control; affects all child pane arrangement. |
| Suggested resolution | Implement `Orientation` parameter on `MariloDockManager` or add a note in spec that it is deferred. |

---

### SPEC-dockmanager-009

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-009 |
| Feature area | pane-types |
| Parameter/event | `DockManagerContentPane` component (entire) |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/overview.md:117–132` |
| Description | Spec describes `DockManagerContentPane` with 11 parameters (`AllowFloat`, `Class`, `Closeable`, `Dockable`, `HeaderText`, `Id`, `Maximizable`, `Size`, `Unpinnable`, `Unpinned`, `UnpinnedSize`, `Visible`). No such component exists in source. |
| Priority | P1 |
| Priority rationale | Core pane type; the spec's entire usage model depends on it. |
| Suggested resolution | Implement `DockManagerContentPane`, or document that source uses `MariloDockPane` as a placeholder pending full implementation. |

---

### SPEC-dockmanager-010

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-010 |
| Feature area | pane-types |
| Parameter/event | `DockManagerSplitPane` component (entire) |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/overview.md:134–153` |
| Description | Spec describes `DockManagerSplitPane` with 6 standard parameters (`AllowEmpty`, `Class`, `Id`, `Orientation`, `Size`, `Visible`) and 5 floating-only parameters (`FloatingHeight`, `FloatingLeft`, `FloatingResizable`, `FloatingTop`, `FloatingWidth`). No such component exists in source. |
| Priority | P1 |
| Priority rationale | Required for horizontal/vertical split layout; fundamental to the component's value proposition. |
| Suggested resolution | Implement `DockManagerSplitPane` or note as deferred in spec. |

---

### SPEC-dockmanager-011

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-011 |
| Feature area | pane-types |
| Parameter/event | `DockManagerTabGroupPane` component (entire) |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/overview.md:155–163` |
| Description | Spec describes `DockManagerTabGroupPane` with 5 parameters (`AllowEmpty`, `Id`, `SelectedPaneId`, `Size`, `Visible`). No such component exists in source. |
| Priority | P1 |
| Priority rationale | Required for tab-group layout within docked panes. |
| Suggested resolution | Implement `DockManagerTabGroupPane` or note as deferred in spec. |

---

### SPEC-dockmanager-012

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-012 |
| Feature area | pane-types |
| Parameter/event | `DockManagerPanes` child content wrapper |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/overview.md:20–22` |
| Description | Spec requires panes to be declared inside a `<DockManagerPanes>` tag. Source uses flat `ChildContent` with no such wrapper. |
| Priority | P1 |
| Priority rationale | Changes the public Razor markup API for all consumers. |
| Suggested resolution | Implement `DockManagerPanes` RenderFragment tag, or update spec to reflect the flat `ChildContent` pattern. |

---

### SPEC-dockmanager-013

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-013 |
| Feature area | pane-types |
| Parameter/event | `DockManagerFloatingPanes` child content wrapper |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/overview.md:24–26` |
| Description | Spec supports a `<DockManagerFloatingPanes>` section for panes that float outside the main layout. Source has no floating pane concept (only an `OnPaneFloat` event with no actual floating window rendering). |
| Priority | P1 |
| Priority rationale | Core feature distinguishing docked vs. floating panes. |
| Suggested resolution | Implement `DockManagerFloatingPanes` wrapper and floating window rendering, or document the stub `OnPaneFloat` callback as the interim approach. |

---

### SPEC-dockmanager-014

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-014 |
| Feature area | events |
| Parameter/event | `OnDock` / `DockManagerDockEventArgs` |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/events.md:27–37` |
| Description | Spec defines `OnDock` event with `DockManagerDockEventArgs` (`DockPosition`, `IsCancelled`, `PaneId`, `TargetPaneId`). Source has no drag-and-dock capability and no `OnDock` event. |
| Priority | P1 |
| Priority rationale | Core interactive docking event; blocks all docking-type demo scenarios. |
| Suggested resolution | Implement drag-and-dock with `OnDock` event, or mark as future milestone in spec. |

---

### SPEC-dockmanager-015

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-015 |
| Feature area | events |
| Parameter/event | `OnUndock` / `DockManagerUndockEventArgs` |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/events.md:39–49` |
| Description | Spec defines `OnUndock` with `DockManagerUndockEventArgs` (`IsCancelled`, `PaneId`). Source has `OnPaneFloat` (`EventCallback<string>`) which is semantically related but architecturally different (no `IsCancelled` support). |
| Priority | P1 |
| Priority rationale | Cancellable undock is a key behavioral contract; `OnPaneFloat` cannot substitute. |
| Suggested resolution | Implement `OnUndock` with `DockManagerUndockEventArgs`, or acknowledge `OnPaneFloat` as interim. |

---

### SPEC-dockmanager-016

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-016 |
| Feature area | events |
| Parameter/event | `VisibleChanged` (per `DockManagerContentPane`) |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/events.md:51–53` |
| Description | Spec defines `VisibleChanged` on `DockManagerContentPane` to fire when the user hides a pane. Source's pane (`MariloDockPane`) has no `Visible` parameter or `VisibleChanged` event. |
| Priority | P1 |
| Priority rationale | The `Visible` two-way binding and its changed event are part of the public pane API. |
| Suggested resolution | Implement `Visible` / `VisibleChanged` on the pane component. |

---

### SPEC-dockmanager-017

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-017 |
| Feature area | events |
| Parameter/event | `SizeChanged` (per pane) |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/events.md:55–57` |
| Description | Spec defines `SizeChanged` on panes when their `Size` parameter changes. Source has no `Size` parameter or `SizeChanged` event on any pane. |
| Priority | P2 |
| Priority rationale | Resizing is a secondary feature; important for state-tracking scenarios. |
| Suggested resolution | Implement `Size` / `SizeChanged` on the pane component. |

---

### SPEC-dockmanager-018

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-018 |
| Feature area | events |
| Parameter/event | `UnpinnedChanged` / `UnpinnedSizeChanged` (per `DockManagerContentPane`) |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/events.md:59–63` |
| Description | Spec defines `UnpinnedChanged` and `UnpinnedSizeChanged` events on `DockManagerContentPane`, driven by `Unpinned` and `UnpinnedSize` two-way parameters. Source has no unpinning concept (only an `OnPanePin` root callback). |
| Priority | P1 |
| Priority rationale | Pinning/unpinning is a primary DockManager interaction; two-way binding and per-event callbacks are part of the documented public API. |
| Suggested resolution | Implement `Unpinned`, `UnpinnedSize`, `UnpinnedChanged`, `UnpinnedSizeChanged` on the pane component. |

---

### SPEC-dockmanager-019

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-019 |
| Feature area | events |
| Parameter/event | `OnPaneResize` / `DockManagerPaneResizeEventArgs` |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/events.md:65–75` |
| Description | Spec defines `OnPaneResize` on `MariloDockManager` with `DockManagerPaneResizeEventArgs` (`PaneId`, `Size`). Source has no resize event. |
| Priority | P2 |
| Priority rationale | Needed by consumers embedding charts or other components that must repaint on resize. |
| Suggested resolution | Implement `OnPaneResize` event on `MariloDockManager`. |

---

### SPEC-dockmanager-020

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-020 |
| Feature area | state |
| Parameter/event | `OnStateInit` / `DockManagerStateEventArgs` |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/state.md:33–40` |
| Description | Spec defines `OnStateInit` event for programmatic initial state. Source has no state model. |
| Priority | P1 |
| Priority rationale | Required for the state persistence use-case demonstrated in the spec's code sample. |
| Suggested resolution | Implement `DockManagerState`, `DockManagerStateEventArgs`, and `OnStateInit`. |

---

### SPEC-dockmanager-021

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-021 |
| Feature area | state |
| Parameter/event | `OnStateChanged` / `DockManagerStateEventArgs` |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/state.md:42–44` |
| Description | Spec defines `OnStateChanged` event fired on every user action that alters state. Source has no state model. |
| Priority | P1 |
| Priority rationale | Primary mechanism for persisting layout; state persistence demo is broken without it. |
| Suggested resolution | Implement `OnStateChanged` alongside `DockManagerState`. |

---

### SPEC-dockmanager-022

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-022 |
| Feature area | state |
| Parameter/event | `GetState()` method |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/overview.md:169–173` |
| Description | Spec documents a `GetState()` method on the `MariloDockManager` reference returning a `DockManagerState` object. Source has no such method. |
| Priority | P1 |
| Priority rationale | Required for the state persistence flow; `@ref` usage in spec demos depends on it. |
| Suggested resolution | Implement `GetState()` as part of the state model work. |

---

### SPEC-dockmanager-023

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-023 |
| Feature area | state |
| Parameter/event | `SetState()` / `SetStateAsync()` method |
| Gap type | mismatch |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/overview.md:173` / `docs/component-specs/dockmanager/state.md:50–52` |
| Description | `overview.md` lists the method as `SetState`; `state.md` refers to it as `SetStateAsync`. The naming is inconsistent within the spec itself. Source has neither. |
| Priority | P2 |
| Priority rationale | Spec-internal inconsistency; low risk until implemented, but creates confusion. |
| Suggested resolution | Decide on `SetState` or `SetStateAsync`; update both spec files to use the same name. |

---

### SPEC-dockmanager-024

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-024 |
| Feature area | state |
| Parameter/event | `Refresh()` method |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/overview.md:171` |
| Description | Spec documents a `Refresh()` method to programmatically re-render the component. Source has no such method. |
| Priority | P2 |
| Priority rationale | Useful utility method; not blocking for core functionality. |
| Suggested resolution | Implement `Refresh()` calling `StateHasChanged()`. |

---

### SPEC-dockmanager-025

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-025 |
| Feature area | events |
| Parameter/event | `OnPin` / `DockManagerPinEventArgs` |
| Gap type | mismatch |
| Source location | `src/Marilo.Components/Layout/MariloDockManager.razor:46` |
| Spec location | `docs/component-specs/dockmanager/events.md:88–97` |
| Description | Source has `OnPanePin` (`EventCallback<string>`). Spec defines `OnPin` (`DockManagerPinEventArgs` with `IsCancelled` and `PaneId`). Name, event args type, and cancellability all differ. |
| Priority | P1 |
| Priority rationale | The cancellable `IsCancelled` property is an important behavioral contract that the `EventCallback<string>` signature cannot support. |
| Suggested resolution | Replace `OnPanePin` with `OnPin` using `DockManagerPinEventArgs`. |

---

### SPEC-dockmanager-026

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-026 |
| Feature area | events |
| Parameter/event | `OnUnpin` / `DockManagerUnpinEventArgs` |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/events.md:99–108` |
| Description | Spec defines `OnUnpin` with `DockManagerUnpinEventArgs` (`IsCancelled`, `PaneId`). Source has no unpin event. |
| Priority | P1 |
| Priority rationale | Symmetric to `OnPin`; required for cancellable unpin behavior documented in the spec example. |
| Suggested resolution | Implement `OnUnpin` event. |

---

### SPEC-dockmanager-027

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-027 |
| Feature area | docking-types |
| Parameter/event | `DockManagerContentPane.Dockable` (`bool`) |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/overview.md:124` / `docs/component-specs/dockmanager/docking-types.md:29` |
| Description | Spec documents a `Dockable` parameter on `DockManagerContentPane` to disable docking over specific panes. Source has no docking behaviour at all. |
| Priority | P1 |
| Priority rationale | Core docking-types feature. |
| Suggested resolution | Implement drag-and-dock engine with per-pane `Dockable` control. |

---

### SPEC-dockmanager-028

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-028 |
| Feature area | pane-types |
| Parameter/event | `DockManagerContentPane.Maximizable` (`bool`) |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/overview.md:127` |
| Description | Spec documents a `Maximizable` parameter allowing a pane to be maximized. Source has no maximize feature. |
| Priority | P2 |
| Priority rationale | Useful UX feature but not blocking core layout. |
| Suggested resolution | Implement maximize button and `Maximizable` parameter on the pane component. |

---

### SPEC-dockmanager-029

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-029 |
| Feature area | pane-types |
| Parameter/event | `DockManagerContentPane.Unpinnable` (`bool`) |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/overview.md:129` |
| Description | Spec documents a `Unpinnable` parameter controlling whether a pane can be unpinned. Source has no unpin concept. |
| Priority | P2 |
| Priority rationale | Depends on unpinning feature being implemented first. |
| Suggested resolution | Implement unpinning with `Unpinnable`, `Unpinned`, `UnpinnedSize` parameters. |

---

### SPEC-dockmanager-030

| Field | Value |
|-------|-------|
| ID | SPEC-dockmanager-030 |
| Feature area | pane-types |
| Parameter/event | `DockManagerTabGroupPane.SelectedPaneId` (`int`) |
| Gap type | spec-ahead |
| Source location | missing |
| Spec location | `docs/component-specs/dockmanager/overview.md:161` |
| Description | Spec documents `SelectedPaneId` on `DockManagerTabGroupPane` to set the initially selected tab. Source has no `DockManagerTabGroupPane`. Additionally, the type `int` seems inconsistent with `Id` being typed as `string` on other pane types — a potential spec-internal mismatch. |
| Priority | P2 |
| Priority rationale | Depends on `DockManagerTabGroupPane` being implemented. The `int` vs `string` type inconsistency should be addressed at spec time. |
| Suggested resolution | Implement `DockManagerTabGroupPane.SelectedPaneId`, and clarify whether the type should be `string` (consistent with `Id` parameters elsewhere) or `int`. |

---

## Section 3 — Mismatches (both exist but differ)

*Already captured above as part of the relevant sections (SPEC-dockmanager-006, SPEC-dockmanager-007, SPEC-dockmanager-023, SPEC-dockmanager-025). Listed here for quick reference.*

| ID | Item | Issue |
|----|------|-------|
| SPEC-dockmanager-006 | `Closable` vs `Closeable` | Spelling diverges between source and spec |
| SPEC-dockmanager-007 | `Height` default `"500px"` vs `null` | Contradicts spec's "auto-expand" description |
| SPEC-dockmanager-023 | `SetState` vs `SetStateAsync` | Inconsistent naming within spec itself |
| SPEC-dockmanager-025 | `OnPanePin: EventCallback<string>` vs `OnPin: DockManagerPinEventArgs` | Name and args type differ |

---

## Gap Count Summary

| Type | Count |
|------|-------|
| Undocumented (source not in spec) | 7 (001–007) |
| Spec-ahead (spec not in source) | 19 (008–030, minus the 4 recategorized as mismatch) |
| Mismatch (both exist but differ) | 4 (006, 007, 023, 025) |
| **Total** | **30** |

| Priority | Count |
|----------|-------|
| P1 | 21 |
| P2 | 9 |
| P3 | 0 |
