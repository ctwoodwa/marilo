# Gap Intake Inventory: MariloDockManager

> Component: MariloDockManager
> Intake date: 2026-04-03
> Intake mode: Fresh analysis (assess mode) -- no source code exists
> Spec docs: 6 files (overview, pane-types, docking-types, events, state, accessibility/wai-aria)
> Demo page: Placeholder only ("Coming soon")

---

## 1. Source Code Status

**No source files exist.** A search of `/workspaces/Marilo/src/Marilo.Components/` found zero DockManager-related files. This is a **standard greenfield intake**.

## 2. Spec Summary

The spec documents a desktop-style dockable panel layout manager with:

- **Pane types:** 3 types -- ContentPane (12 params: AllowFloat, Closeable, Dockable, HeaderText, Id, Maximizable, Size, Unpinnable, Unpinned, UnpinnedSize, Visible, Class), SplitPane (6 base params + 5 floating params), TabGroupPane (5 params).
- **Docking types:** Global docking (dock to component edges) and Inner docking (dock within/beside other panes, or as tab).
- **Floating panes:** DockManagerFloatingPanes container with FloatingHeight, FloatingLeft, FloatingTop, FloatingWidth, FloatingResizable.
- **State management:** GetState/SetState/SetStateAsync methods, OnStateInit/OnStateChanged events, full JSON-serializable DockManagerState object for persist/restore.
- **Events:** 10 events -- OnDock, OnUndock, VisibleChanged, SizeChanged, UnpinnedChanged, UnpinnedSizeChanged, OnPaneResize, OnStateInit, OnStateChanged, OnPin, OnUnpin. Several support IsCancelled for cancellation.
- **Component parameters:** 4 top-level (Class, Height, Width, Orientation).
- **Methods:** GetState, SetState, Refresh.
- **Accessibility:** Full WAI-ARIA spec with role=application, aria-live, keyboard navigation, screen reader testing (NVDA, JAWS). Delegates to Toolbar, TabStrip, Splitter, Window accessibility specs.

**Estimated parameter/feature count:** ~35 pane-level parameters, 4 component-level parameters, 10 events, 3 methods.

## 3. Demo Page Status

The demo page at `/workspaces/Marilo/samples/Marilo.Demo/Pages/Components/DockManager/Overview.razor` is a **placeholder only** -- displays a "Coming soon" alert.

## 4. Rough Gap Count

| Feature Area | Estimated Gaps |
|---|---|
| Core component + layout engine | 4 |
| ContentPane (all params) | 8 |
| SplitPane (base + floating) | 6 |
| TabGroupPane | 4 |
| Docking mechanics (global + inner) | 5 |
| Floating pane support | 4 |
| State management (get/set/persist) | 5 |
| Events (10 total) | 6 |
| Accessibility (ARIA roles, keyboard) | 3 |
| Styling/theming | 2 |
| **Total** | **~47** |

## 5. Severity Breakdown Estimate

| Severity | Count | Examples |
|---|---|---|
| Critical | ~15 | Core rendering, ContentPane basics, SplitPane layout, pane drag-and-dock, state get/set |
| Important | ~20 | TabGroupPane, floating panes, all 10 events, unpinning, pane resize, state persist/restore |
| Nice-to-have | ~12 | Accessibility ARIA roles, Maximizable, AllowEmpty, FloatingResizable, custom CSS class support |

## 6. Delivery Workspace Recommendation

**YES -- merits its own delivery workspace.** The DockManager is a complex layout component with heavy JS interop for drag-and-dock, pane management, and state serialization. It composes multiple inner components (Splitter, TabStrip, Window) and has a substantial accessibility surface. A dedicated `dockmanager-delivery/` workspace already exists. Scope: `systematic`.

---

**Next step:** Proceed to Stage 02 (prioritize) focusing on core pane rendering + split layout as the foundation, then docking mechanics.
