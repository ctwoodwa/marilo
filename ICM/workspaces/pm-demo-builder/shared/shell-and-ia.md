# Shell and Information Architecture

Conventions for the PM demo's app shell, navigation, and layout nesting.

## App Shell

The PM demo uses `MariloAppShell` as its outer frame, rendered exclusively in `MainLayout.razor`. The shell provides:

- **Sidebar**: collapsible, with nav groups, nav links, brand slot, and footer slot.
- **Main content**: `ChildContent` slot where routed pages render.
- **Context panel**: optional right-side panel for page-owned detail views.
- **Slide-over**: overlay panel for quick-edit interactions.

## Current Sidebar Navigation

```
Overview
  └── Dashboard (/)

Planning
  ├── Task Board (/board) [Badge: 12]
  ├── Task List (/tasks)
  └── Timeline (/timeline)

Governance
  ├── Budget (/budget)
  ├── Team Resource (/team)
  └── Risk Register (/risk) [Badge: !]

Footer
  └── [Avatar + Name] | [Bell]
      ├── MariloUserMenu (Profile, Settings, Theme, Help, Shortcuts, Sign out)
      └── MariloNotificationBell (feed + More options menu)
```

## Route Conventions

- Top-level entity lists: `/entity` (e.g., `/tasks`, `/risk`, `/budget`)
- Entity detail: `/entity/{id}` (not yet used in the demo — all list pages exist, no detail pages)
- Settings area: `/account/{section}` (e.g., `/account/details`, `/account/preferences`)
- Nested sub-entities: `/entity/{id}/sub` (e.g., `/assets/{id}/inspections`)

## Layout Nesting Rules

1. `MainLayout.razor` — the only component that renders `MariloAppShell`. All pages render inside this by default.
2. `SettingsLayout.razor` (planned) — declares `@layout MainLayout`, renders a 2-column inner frame (settings nav + content). Settings pages declare `@layout SettingsLayout`.
3. Future sub-layouts (e.g., `AssetDetailLayout`) follow the same pattern: declare `@layout MainLayout`, render only their inner chrome.

## Navigation Placement Principles

- **Top-level sidebar groups** are for major workflow domains (Overview, Planning, Governance).
- **New domain groups** (e.g., "Assets", "Operations") should be added as sibling groups, not nested inside existing ones.
- **Settings** is accessed via the user menu and bell "More options", not via the sidebar nav.
- **Badge counts** on nav links indicate actionable items (unread, overdue, flagged).

## Footer Conventions

- Left side: avatar button with identity stack (name, email). Clicking opens `MariloUserMenu`.
- Right side: bell button. Clicking opens `MariloNotificationBell`.
- Both menus are positioned absolutely relative to the footer container.
- Bell panel pops out to the right of the sidebar (`left: calc(100% + 12px)`).

## Slide-Over Usage

Use `MariloAppShellSlideOver` or `MariloDrawer` for:
- Quick-edit forms that don't warrant a full page navigation (e.g., connector config, task detail).
- Preview panels for list items.
- Do NOT use for primary creation flows — those get their own route.
