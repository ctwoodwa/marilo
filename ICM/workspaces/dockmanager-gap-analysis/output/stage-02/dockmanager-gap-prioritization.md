# DockManager Gap Prioritization -- Stage 02

**Date:** 2026-04-12
**Worker:** w-dockmanager-gap-analysis
**Component:** MariloDockManager
**Input:** Stage 01 gap inventory (22 gaps)
**Architecture Decision:** Full spec commitment -- all gaps are implementation targets.

---

## Prioritization Rationale

The DockManager is a complex, deeply layered component. The dependency chain is strict: the pane hierarchy model is the foundation; state management serializes that model; drag-and-drop and floating panes operate on that model; events, templates, and polish are layered on top. Waves are ordered by structural dependency, not severity alone.

---

## Wave Plan

### Wave 1: Pane Hierarchy Model (Foundation)

**Goal:** Replace the flat `MariloDockPane` with the spec's three-type pane hierarchy. This is the structural foundation that every other wave builds on.

**Scope:** New component classes, tree data structure, nesting validation, basic rendering.

| Gap ID | Description | Complexity | Notes |
|--------|-------------|-----------|-------|
| GAP-01 | Pane hierarchy model (ContentPane, SplitPane, TabGroupPane) | XL | Core tree structure, `<DockManagerPanes>` container, nesting rules from pane-types.md |
| GAP-02 | Split pane component with orientation and resizable splits | XL | `DockManagerSplitPane` with `Orientation`, `Size`, splitter handles |
| GAP-03 | Tab group pane component with tab selection | L | `DockManagerTabGroupPane` with `SelectedPaneId`, tab strip rendering |
| GAP-08 | Child component naming alignment | M | Resolved naturally -- new components use spec names |
| GAP-11 | Root orientation parameter | S | `DockManagerPaneOrientation` enum on root splitter |
| GAP-18 | HeaderText vs Title rename | S | Rename `Title` to `HeaderText` on content pane |

**Wave 1 Gate:** `<MariloDockManager>` renders a nested pane tree with split panes (resizable) and tab groups. Markup matches the spec's `<DockManagerPanes>/<DockManagerSplitPane>/<DockManagerTabGroupPane>/<DockManagerContentPane>` structure. Basic rendering -- no drag-and-drop, no floating, no state persistence.

**Estimated Complexity:** XL (largest single wave)

---

### Wave 2: State Management + Programmatic API

**Goal:** Implement `DockManagerState` serialization model and the `GetState`/`SetState`/`SetStateAsync`/`Refresh` methods with `OnStateInit`/`OnStateChanged` events.

**Scope:** State object design, serialization, event wiring, method exposure.

| Gap ID | Description | Complexity | Notes |
|--------|-------------|-----------|-------|
| GAP-06 | DockManagerState object with GetState/SetState/SetStateAsync | L | Serializable model of full pane tree; JSON round-trip |
| GAP-09 | Public methods: GetState, SetState, SetStateAsync, Refresh | M | Instance methods on MariloDockManager |

**Wave 2 Gate:** `DockManagerState` can be obtained via `GetState()`, serialized to JSON, deserialized, and applied via `SetState()`. `OnStateInit` fires on initialization; `OnStateChanged` fires on user-driven layout changes. `Refresh()` triggers re-render.

**Estimated Complexity:** L

---

### Wave 3: Drag-and-Drop Docking

**Goal:** Implement the full drag-and-drop docking system with global and inner dock navigators.

**Scope:** JS interop for drag operations, dock navigator overlay UI, hit testing, drop zone logic, `Dockable` parameter.

| Gap ID | Description | Complexity | Notes |
|--------|-------------|-----------|-------|
| GAP-05 | Drag-and-drop docking (global + inner navigators) | XL | Heaviest JS interop. Global navigator at container edges, inner navigator per pane. |
| GAP-22 | Dockable parameter on content panes | S | Boolean gate -- prevents dock-over when false |

**Wave 3 Gate:** User can drag a pane and dock it globally (to container edges) or inner-dock (to another pane's edges or center for tab grouping). Dock navigator overlays appear during drag. `Dockable=false` prevents docking over that pane. State updates reflect dock operations.

**Estimated Complexity:** XL

---

### Wave 4: Floating Panes

**Goal:** Implement the `<DockManagerFloatingPanes>` container with floating window behavior -- position, resize, window chrome.

**Scope:** JS interop for window management, floating pane rendering, undock-to-float, dock-from-float.

| Gap ID | Description | Complexity | Notes |
|--------|-------------|-----------|-------|
| GAP-04 | Floating panes container and window behavior | XL | FloatingHeight/Left/Top/Width/Resizable params, window chrome, overlay rendering |
| GAP-16 | AllowFloat parameter | S | Boolean gate on undock-to-float action |

**Wave 4 Gate:** `<DockManagerFloatingPanes>` renders panes as draggable, resizable floating windows positioned over the dock manager. Panes can be undocked to float (respecting `AllowFloat`) and docked back from float. Floating pane position/size persists in `DockManagerState`.

**Estimated Complexity:** XL

---

### Wave 5: Events, Parameters, and Advanced Features

**Goal:** Complete the event model, implement remaining parameters (Visible, pin/unpin, maximizable, AllowEmpty), clean up source-ahead gaps.

**Scope:** Typed event args, cancellation pattern, two-way binding parameters, toolbar for unpinned panes, maximize toggle.

| Gap ID | Description | Complexity | Notes |
|--------|-------------|-----------|-------|
| GAP-07 | Typed event args with IsCancelled (OnDock, OnUndock, OnPin, OnUnpin, OnPaneResize) | L | New event arg classes, cancellation pattern |
| GAP-10 | Pin/unpin behavior with toolbar | L | Unpinnable/Unpinned/UnpinnedSize params, toolbar, slide-out |
| GAP-12 | Visible parameter with two-way binding | M | VisibleChanged event, replaces close-by-removal |
| GAP-15 | Maximizable parameter | M | Max/restore toggle, expand within parent |
| GAP-17 | AllowEmpty parameter on split/tab group | S | Empty placeholder vs. collapse on child removal |
| GAP-19 | Remove OnPaneFloat (replaced by OnUndock) | S | Breaking change cleanup |
| GAP-20 | Remove OnPaneClose (replaced by VisibleChanged) | S | Breaking change cleanup |

**Wave 5 Gate:** All spec events fire with correct typed args and cancellation support. Pin/unpin works with toolbar. Visible two-way binding works on all pane types. Maximize toggle works. AllowEmpty controls empty-space behavior. Source-ahead callbacks removed.

**Estimated Complexity:** L-XL (breadth of changes)

---

### Wave 6: SCSS Parity + Accessibility

**Goal:** Full SCSS styling for both FluentUI and Bootstrap providers. Complete ARIA attributes and keyboard navigation.

**Scope:** SCSS authoring for all BEM classes, provider-specific theming, ARIA roles, keyboard handlers.

| Gap ID | Description | Complexity | Notes |
|--------|-------------|-----------|-------|
| GAP-13 | Accessibility/ARIA attributes and keyboard navigation | M | role=application, aria-live, aria-hidden on navigator, keyboard nav for toolbar/tabstrip/splitter/window |
| GAP-14 | SCSS parity for both providers (0% currently) | L | BEM classes for dock manager, split pane, tab group, floating window, toolbar, navigator, splitter handles |
| GAP-21 | Spec CSS class prefix update (k- to mar-) | S | Spec doc update only |

**Wave 6 Gate:** Both FluentUI and Bootstrap providers render a visually complete dock manager. All interactive elements have correct ARIA attributes. Keyboard navigation works for all sub-components (toolbar, tab strip, splitter, floating window). Spec docs updated to use `mar-` prefix.

**Estimated Complexity:** L

---

## Wave Summary

| Wave | Focus | Gaps | Complexity | Dependencies |
|------|-------|------|-----------|--------------|
| 1 | Pane Hierarchy Model | GAP-01, 02, 03, 08, 11, 18 | XL | None (foundation) |
| 2 | State Management | GAP-06, 09 | L | Wave 1 |
| 3 | Drag-and-Drop Docking | GAP-05, 22 | XL | Wave 1, Wave 2 |
| 4 | Floating Panes | GAP-04, 16 | XL | Wave 1, Wave 2, Wave 3 |
| 5 | Events + Advanced Features | GAP-07, 10, 12, 15, 17, 19, 20 | L-XL | Wave 1 (some items need Wave 3/4) |
| 6 | SCSS Parity + Accessibility | GAP-13, 14, 21 | L | Waves 1-5 (needs final component structure) |

**Total gaps:** 22
**All gaps assigned to waves:** Yes (22/22)

---

## Risk Notes

1. **Wave 1 is the critical path.** If the pane hierarchy model is wrong, every subsequent wave is affected. This wave deserves the most design scrutiny before implementation.
2. **Waves 3 and 4 are JS-interop heavy.** They require careful design of the Blazor-to-JS boundary. Consider whether to use a single JS module or per-feature modules.
3. **Wave 5 has internal ordering.** GAP-07 (event model) should come before GAP-10 (pin/unpin) since pin/unpin events depend on the new event arg pattern. GAP-19/GAP-20 (source-ahead removal) should come last as breaking changes.
4. **Wave 6 depends on structural stability.** SCSS and a11y work is most efficient when the component structure is finalized. Starting SCSS too early risks rework.
5. **The existing `MariloDockPane` is effectively replaced.** Wave 1 introduces `DockManagerContentPane` (and siblings), making the current `MariloDockPane` obsolete. Migration path: the old component is removed, not wrapped.
