# DockManager Visual Parity Gaps — Stage 03 Visual Parity

**Date:** 2026-04-12
**Worker:** w-dockmanager-delivery
**Component:** MariloDockManager + MariloDockPane

## BEM Classes in Source

Classes extracted from `MariloDockManager.razor` and `MariloDockPane.razor`:

| # | BEM Class | Element | File |
|---|---|---|---|
| 1 | `mar-dockmanager` | Root container | MariloDockManager.razor:3 |
| 2 | `mar-dockmanager__tabs` | Tab strip container | MariloDockManager.razor:7 |
| 3 | `mar-dockmanager__tab` | Individual tab | MariloDockManager.razor:10 |
| 4 | `mar-dockmanager__tab--active` | Active tab modifier | MariloDockManager.razor:10 |
| 5 | `mar-dockmanager__tab-title` | Tab title text | MariloDockManager.razor:12 |
| 6 | `mar-dockmanager__tab-actions` | Action buttons container | MariloDockManager.razor:13 |
| 7 | `mar-dockmanager__action` | Individual action button (pin/float/close) | MariloDockManager.razor:14-18 |
| 8 | `mar-dockmanager__content` | Content area | MariloDockManager.razor:25 |
| 9 | `mar-dockpane` | Pane wrapper | MariloDockPane.razor:5 |

## SCSS Coverage — FluentUI Provider

**Search result:** No SCSS rules found for any `mar-dock*` class in `src/Marilo.Providers.FluentUI/`.

| BEM Class | SCSS Rule Exists | Status |
|---|---|---|
| `mar-dockmanager` | No | UNSTYLED |
| `mar-dockmanager__tabs` | No | UNSTYLED |
| `mar-dockmanager__tab` | No | UNSTYLED |
| `mar-dockmanager__tab--active` | No | UNSTYLED |
| `mar-dockmanager__tab-title` | No | UNSTYLED |
| `mar-dockmanager__tab-actions` | No | UNSTYLED |
| `mar-dockmanager__action` | No | UNSTYLED |
| `mar-dockmanager__content` | No | UNSTYLED |
| `mar-dockpane` | No | UNSTYLED |

**FluentUI parity score: 0/9 (0%)**

## SCSS Coverage — Bootstrap Provider

**Search result:** No SCSS rules found for any `mar-dock*` class in `src/Marilo.Providers.Bootstrap/`.

| BEM Class | SCSS Rule Exists | Status |
|---|---|---|
| `mar-dockmanager` | No | UNSTYLED |
| `mar-dockmanager__tabs` | No | UNSTYLED |
| `mar-dockmanager__tab` | No | UNSTYLED |
| `mar-dockmanager__tab--active` | No | UNSTYLED |
| `mar-dockmanager__tab-title` | No | UNSTYLED |
| `mar-dockmanager__tab-actions` | No | UNSTYLED |
| `mar-dockmanager__action` | No | UNSTYLED |
| `mar-dockmanager__content` | No | UNSTYLED |
| `mar-dockpane` | No | UNSTYLED |

**Bootstrap parity score: 0/9 (0%)**

## Future BEM Classes Needed (based on spec)

When the spec features are implemented, the following additional BEM classes will be needed:

| Expected Class | Spec Feature | Notes |
|---|---|---|
| `mar-dockmanager__splitter` | Split pane | Resizable splitter bar |
| `mar-dockmanager__split-pane` | Split pane container | Horizontal/vertical layout |
| `mar-dockmanager__tabgroup` | Tab group pane | Tabstrip container for grouped panes |
| `mar-dockmanager__toolbar` | Unpinned pane toolbar | Sidebar showing unpinned pane buttons |
| `mar-dockmanager__floating` | Floating pane window | Draggable window overlay |
| `mar-dockmanager__floating-titlebar` | Floating pane header | Window title bar with controls |
| `mar-dockmanager__navigator` | Dock navigator | Drop target indicator during drag |
| `mar-dockmanager__pane--maximized` | Maximized state | Fullscreen pane modifier |
| `mar-dockmanager__pane--unpinned` | Unpinned state | Collapsed/auto-hide pane modifier |

## Conclusion

**Zero visual parity.** Neither FluentUI nor Bootstrap providers have any SCSS rules for the DockManager component. All 9 existing BEM classes are completely unstyled. The component currently relies entirely on browser defaults and inline styles from the demo page. This is expected given the component is at Phase 1 (skeleton) status in the gap analysis plan.
