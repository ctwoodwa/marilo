# MariloAppShell

A reusable, modern enterprise application shell for Blazor apps. Linear/Perplexity-inspired:
sidebar-first layout, desktop collapse-to-icons, bottom-pinned avatar + notification bell, mobile
off-canvas drawer, and slide-over panels for account/help/notifications instead of a heavy top bar.

Lives in its own RCL: **`Marilo.Components.Shell`** — server- and WebAssembly-compatible.

## Components

| Component | Purpose |
|---|---|
| `MariloAppShell` | Root layout. Hosts sidebar, main content, optional context rail, slide-over host. |
| `MariloAppShellNavGroup` | Section grouping with optional title (hidden when collapsed). |
| `MariloAppShellNavLink` | Nav item: icon + label + optional badge. Uses `NavLink` for active state. |
| `MariloAppShellSlideOver` | Right-side slide-over panel for notifications, account, help. |

## API surface (MariloAppShell)

```csharp
RenderFragment? SidebarBrand
RenderFragment? SidebarContent
RenderFragment? SidebarFooter   // optional override; default renders bell + avatar
RenderFragment? ChildContent    // main page area
RenderFragment? ContextPanel    // optional right rail (page-owned)
RenderFragment? SlideOver       // host for one or more MariloAppShellSlideOver

bool   SidebarCollapsed  (two-way bind)
bool   ShowContextPanel
string BrandText
string? UserName
int    NotificationCount
EventCallback OnNotificationsClick
EventCallback OnAvatarClick
```

## States

- **Expanded** (default, ~256px): brand, group titles, labels, badges, full footer.
- **Collapsed** (~68px): icons only, group titles hidden, footer compresses to icon buttons,
  tooltips via `title` + `aria-label` for screen readers.
- **Mobile** (`<768px`): sidebar becomes off-canvas drawer with scrim; hamburger toggle in
  top-left of main; collapse mode is bypassed so labels remain visible inside the drawer.

Transitions respect `prefers-reduced-motion`.

## Accessibility

- Sidebar uses `<aside aria-label="Primary">` and `<nav role="navigation">`.
- Active nav link uses `aria-current="page"` (via `NavLink`).
- Collapsed icon-only items keep their accessible name via `aria-label` + `title`.
- Slide-over uses `role="dialog" aria-modal="true"` with explicit close control.
- Visible focus ring on nav links.

## Usage

```razor
<MariloAppShell @bind-SidebarCollapsed="_collapsed"
                BrandText="Marilo PM"
                UserName="Avery Chen"
                NotificationCount="3"
                OnNotificationsClick="() => _notificationsOpen = true"
                OnAvatarClick="() => _accountOpen = true">

    <SidebarContent>
        <MariloAppShellNavGroup Title="Planning">
            <MariloAppShellNavLink Href="/board" Label="Task Board" Badge="12">
                <Icon><svg .../></Icon>
            </MariloAppShellNavLink>
        </MariloAppShellNavGroup>
    </SidebarContent>

    <ChildContent>@Body</ChildContent>

    <SlideOver>
        <MariloAppShellSlideOver @bind-Open="_notificationsOpen" Title="Notifications">
            ...
        </MariloAppShellSlideOver>
    </SlideOver>
</MariloAppShell>
```

## PM demo adoption

`samples/Marilo.PmDemo/Marilo.PmDemo.Client/Layout/MainLayout.razor` consumes the shell with
seven nav items grouped as **Overview / Planning / Governance**:

- Overview: Dashboard
- Planning: Task Board, Task List, Timeline
- Governance: Budget, Team Resource, Risk Register

## Tradeoffs / follow-ups

- **Separate library**: AppShell lives in `Marilo.Components.Shell` (not `Marilo.Components`)
  because the latter declares `<FrameworkReference Include="Microsoft.AspNetCore.App" />` which
  trips `NETSDK1082` for Blazor WebAssembly consumers. Long-term, fix the framework reference in
  `Marilo.Components` and merge the shell back if a single distribution is desired.
- **Theming**: scoped CSS uses `--marilo-color-*` tokens with hard-coded fallbacks. It does not
  go through `IMariloCssProvider`, so it works without any provider but won't pick up
  bootstrap/fluent provider classes. Provider integration is a follow-up.
- **Tooltip primitive**: collapsed-mode tooltips use the native `title` attribute for simplicity.
  Could be upgraded to a custom tooltip primitive for richer styling.
- **Slide-over primitive**: a dedicated `MariloAppShellSlideOver` was added rather than reusing
  `MariloDrawer` because Drawer is data-bound and provider-CSS-coupled. If/when Drawer adds a
  child-content + scoped-CSS variant, the slide-over could delegate to it.
