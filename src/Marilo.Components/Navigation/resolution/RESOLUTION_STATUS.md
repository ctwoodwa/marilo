---
component: MariloBreadcrumb, MariloBreadcrumbItem, MariloContextMenu, MariloEnvironmentBadge, MariloMenu, MariloMenuItem, MariloPagination, MariloTimeRangeSelector, MariloToolbar, MariloToolbarButton, MariloToolbarGroup, MariloToolbarSeparator, MariloToolbarToggleButton, MariloTreeItem, MariloTreeView
phase: 2
status: not-started
complexity: mixed
priority: high
owner: ""
last-updated: 2026-03-31
depends-on: [MariloThemeProvider]
external-resources:
  - name: ""
    url: ""
    license: ""
    approved: false
---

# Resolution Status: Navigation

## Current Phase
Phase 2: TreeView, Menu, ContextMenu, Pagination are Phase 2; Breadcrumb, Toolbar are Phase 3; remaining components are Phase 4

## Gap Summary
TreeView has 6 gaps (no expanded/selected binding), Menu has 7 gaps (hierarchy not wired), ContextMenu has 8 gaps (no selector/data binding), Breadcrumb 7 gaps, Toolbar 5 gaps, Pagination 6 gaps. Other components have minor gaps.

## Resolution Progress

### Completed
- [x] **MariloPagination** — IMPLEMENTED (6/6 gaps resolved): Added `Total`+`PageSize` model (auto-computes pages), renamed `CurrentPage`→`Page`, `MaxVisiblePages`→`ButtonCount`, added `PageSizes` dropdown, `PageSizeChanged` event, `ShowInfo` page info text. Updated all sample pages.

### Not Started
- [ ] MariloTreeView — 6 gaps
- [ ] MariloMenu — 7 gaps
- [ ] MariloContextMenu — 8 gaps
- [ ] MariloBreadcrumb — 7 gaps
- [ ] MariloToolbar — 5 gaps
- [ ] Minor components (BreadcrumbItem, EnvironmentBadge, MenuItem, ToolbarButton, etc.)

## Blockers
- None
