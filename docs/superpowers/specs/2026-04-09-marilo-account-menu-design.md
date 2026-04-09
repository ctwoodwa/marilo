# MariloAccountMenu — Design Spec

**Date:** 2026-04-09
**Component:** `MariloAccountMenu`
**Location:** `src/Marilo.Components.Shell/AppShell/`
**Status:** Approved for implementation

---

## Overview

A reusable, self-contained account menu component for Marilo shell experiences. Replaces the current fixed `MariloUserMenu` + inline theme picker assembly with a single component that provides grouped menu sections, nested submenus, keyboard navigation, dark mode compatibility, and host-level customization.

## Goals

- Replace the current fixed shell user menu behaviorally and visually.
- Preserve Marilo visual identity, interaction patterns, and single-component composition style.
- Support zero-config usage, standard configuration through options, and advanced customization through render fragments.
- Support grouped menu sections, nested submenus, keyboard navigation, and dark mode compatibility.

## Non-Goals

- Do not introduce a multi-component consumer composition model using public Section/Item/Submenu child tags.
- Do not require downstream apps to implement billing, upgrades, connectors, or install flows.
- Do not clone Perplexity branding; only adopt a similar information architecture and interaction pattern.

---

## File Structure

Create 5 files in `src/Marilo.Components.Shell/AppShell/`:

| File | Purpose |
|---|---|
| `MariloAccountMenu.razor` | Self-contained trigger, popup, section rendering, submenu rendering |
| `MariloAccountMenu.razor.css` | Component styling, layout, states, submenu visuals |
| `AccountMenuOptions.cs` | Strongly typed options with nested item options |
| `AccountMenuItemModel.cs` | Internal item model for rendering menu items |
| `AccountMenuTemplateContexts.cs` | Typed submenu template context records |

---

## Menu Structure

### Group 1 — Core Settings
1. Account
2. Preferences
3. Personalization
4. Shortcuts
5. Usage and credits *(hidden by default)*
6. Connectors *(hidden by default)*
7. All settings

### Group 2 — Upgrade
8. Upgrade plan *(hidden by default)*

### Group 3 — Install
9. Install apps *(hidden by default)*

### Group 4 — Preferences & Help (Submenus)
10. Appearance → submenu: Light, Dark, System
11. Language → submenu: Default, English
12. Help → submenu: Help & docs, Keyboard shortcuts, Contact support

### Group 5 — Sign Out
13. Sign out

### Rendering Rules
- Each group renders as a distinct section.
- Separators render between groups.
- If all items in a group are hidden, the group and its separator collapse.
- Sign out is always isolated in the final section.
- Appearance and Language show right-aligned current value text (e.g., "Dark", "Default").
- Only submenu items show trailing chevron icons.

---

## Icon Mapping

All icons from `Marilo.Icons` (`src/Marilo.Icons/wwwroot/icons/sprite.svg`):

| Menu Item | Icon Name | Status |
|---|---|---|
| Account | `user-circle` | Available |
| Preferences | `sliders` | Available |
| Personalization | `palette` | Available |
| Shortcuts | `keyboard` | Available |
| Usage and credits | `gauge` | Available |
| Connectors | `webhook` | Available |
| All settings | `settings` | Available |
| Upgrade plan | `zap` | Available |
| Install apps | `download` | Available |
| Appearance | `sun` | Available |
| Language | `globe` | Available |
| Help | `help-circle` | Available |
| Sign out | `log-out` | Available |
| Submenu chevron | `chevron-right` | Available |
| Back arrow | `chevron-left` | Available |

**Missing icons:** None identified.

---

## Public API

### Component Parameters

```csharp
// Identity (populated from auth context)
[Parameter] public string UserName { get; set; }
[Parameter] public string? UserEmail { get; set; }
[Parameter] public string? UserAvatarUrl { get; set; }
[Parameter] public string? UserBadge { get; set; }  // e.g., "Pro"

// Configuration
[Parameter] public AccountMenuOptions? Options { get; set; }
[Parameter] public bool CompactTrigger { get; set; }  // icon-only for collapsed sidebar

// Events
[Parameter] public EventCallback<string> OnNavigate { get; set; }
[Parameter] public EventCallback OnSignOut { get; set; }
[Parameter] public EventCallback<string> OnAppearanceChange { get; set; }
[Parameter] public EventCallback<string> OnLanguageChange { get; set; }

// Render fragment slots (see Render Fragments section)
[Parameter] public RenderFragment? TriggerTemplate { get; set; }
[Parameter] public RenderFragment? HeaderTemplate { get; set; }
[Parameter] public RenderFragment? AfterSettingsSection { get; set; }
[Parameter] public RenderFragment? AfterUpgradeSection { get; set; }
[Parameter] public RenderFragment? AfterInstallSection { get; set; }
[Parameter] public RenderFragment<AppearanceMenuContext>? AppearanceTemplate { get; set; }
[Parameter] public RenderFragment<LanguageMenuContext>? LanguageTemplate { get; set; }
[Parameter] public RenderFragment<HelpMenuContext>? HelpTemplate { get; set; }
[Parameter] public RenderFragment? BeforeSignOutSection { get; set; }
[Parameter] public RenderFragment? FooterTemplate { get; set; }

// Standard Marilo
[Parameter] public string? Class { get; set; }
[Parameter] public string? Style { get; set; }
```

### CompactTrigger Behavior

When `CompactTrigger="true"`:
- Trigger renders avatar circle only (no name text).
- Used when sidebar is collapsed.

When `CompactTrigger="false"` (default):
- Trigger renders avatar circle + user name.
- Used when sidebar is expanded.

When `TriggerTemplate` is provided:
- Replaces the default trigger entirely.
- Component still manages open/close state via click handler on the template wrapper.

---

## Options Object

### AccountMenuOptions

Uses nested `AccountMenuItemOptions` per item for clean organization:

```csharp
public class AccountMenuOptions
{
    // ── Group 1: Core Settings ──
    public AccountMenuItemOptions Account { get; set; } = new()
        { Label = "Account", Icon = "user-circle", Href = "/account/details" };

    public AccountMenuItemOptions Preferences { get; set; } = new()
        { Label = "Preferences", Icon = "sliders", Href = "/account/preferences" };

    public AccountMenuItemOptions Personalization { get; set; } = new()
        { Label = "Personalization", Icon = "palette", Href = "/account/personalization" };

    public AccountMenuItemOptions Shortcuts { get; set; } = new()
        { Label = "Shortcuts", Icon = "keyboard", Href = "/account/shortcuts" };

    public AccountMenuItemOptions UsageAndCredits { get; set; } = new()
        { Label = "Usage and credits", Icon = "gauge", Visible = false };

    public AccountMenuItemOptions Connectors { get; set; } = new()
        { Label = "Connectors", Icon = "webhook", Visible = false };

    public AccountMenuItemOptions AllSettings { get; set; } = new()
        { Label = "All settings", Icon = "settings", Href = "/account/details" };

    // ── Group 2: Upgrade ──
    public AccountMenuItemOptions UpgradePlan { get; set; } = new()
        { Label = "Upgrade plan", Icon = "zap", Visible = false };

    // ── Group 3: Install ──
    public AccountMenuItemOptions InstallApps { get; set; } = new()
        { Label = "Install apps", Icon = "download", Visible = false };

    // ── Group 4: Submenus ──
    public AccountMenuItemOptions Appearance { get; set; } = new()
        { Label = "Appearance", Icon = "sun" };

    public AccountMenuItemOptions Language { get; set; } = new()
        { Label = "Language", Icon = "globe", CurrentValue = "Default" };

    public AccountMenuItemOptions Help { get; set; } = new()
        { Label = "Help", Icon = "help-circle" };

    // ── Group 5: Sign Out ──
    public AccountMenuItemOptions SignOut { get; set; } = new()
        { Label = "Sign out", Icon = "log-out" };

    // ── Submenu Defaults ──
    public List<string> AppearanceModes { get; set; } = ["Light", "Dark", "System"];
    public string DefaultAppearanceMode { get; set; } = "System";
    public List<string> Languages { get; set; } = ["Default", "English"];
    public string DefaultLanguage { get; set; } = "Default";
}
```

### AccountMenuItemOptions

```csharp
public class AccountMenuItemOptions
{
    public bool Visible { get; set; } = true;
    public string Label { get; set; } = "";
    public string Icon { get; set; } = "";
    public string? Href { get; set; }
    public Func<Task>? Action { get; set; }
    public string? CurrentValue { get; set; }
    public bool Disabled { get; set; }
    public string? TestId { get; set; }
}
```

### Consumer Usage Examples

**Zero config:**
```razor
<MariloAccountMenu UserName="Avery Chen" UserEmail="avery@example.com" />
```

**Standard customization:**
```razor
<MariloAccountMenu UserName="@user.Name"
                   UserEmail="@user.Email"
                   Options="@_options"
                   OnSignOut="HandleSignOut"
                   OnAppearanceChange="HandleAppearance" />

@code {
    private AccountMenuOptions _options = new()
    {
        UpgradePlan = { Visible = true, Href = "/billing/upgrade" },
        UsageAndCredits = { Visible = true, Action = async () => await ShowUsageDialog() },
        Connectors = { Visible = false },
        AllSettings = { Href = "/admin/settings" },
    };
}
```

**Advanced with templates:**
```razor
<MariloAccountMenu UserName="@user.Name" UserEmail="@user.Email" Options="@_options">
    <HeaderTemplate>
        <MyCustomProfileCard User="@user" />
    </HeaderTemplate>
    <AppearanceTemplate Context="ctx">
        <MyAdvancedThemePicker CurrentMode="@ctx.CurrentMode"
                              OnSelect="@ctx.SelectMode" />
    </AppearanceTemplate>
    <BeforeSignOutSection>
        <MariloAccountMenuItem Label="Switch workspace" Icon="building" />
    </BeforeSignOutSection>
</MariloAccountMenu>
```

---

## Render Fragment Slots

### Structural Slots (plain `RenderFragment`)

| Slot | Behavior |
|---|---|
| `TriggerTemplate` | Replaces the default trigger button |
| `HeaderTemplate` | Replaces the avatar + name + email header |
| `AfterSettingsSection` | Injects content after Group 1 |
| `AfterUpgradeSection` | Injects content after Group 2 |
| `AfterInstallSection` | Injects content after Group 3 |
| `BeforeSignOutSection` | Injects content before Group 5 |
| `FooterTemplate` | Renders after the full menu |

### Submenu Slots (typed `RenderFragment<T>`)

| Slot | Context Type |
|---|---|
| `AppearanceTemplate` | `AppearanceMenuContext` |
| `LanguageTemplate` | `LanguageMenuContext` |
| `HelpTemplate` | `HelpMenuContext` |

### Template Context Types

```csharp
public record AppearanceMenuContext(
    string CurrentMode,
    IReadOnlyList<string> Modes,
    Func<string, Task> SelectMode,
    Func<Task> GoBack,
    Func<Task> CloseMenu);

public record LanguageMenuContext(
    string CurrentLanguage,
    IReadOnlyList<string> Languages,
    Func<string, Task> SelectLanguage,
    Func<Task> GoBack,
    Func<Task> CloseMenu);

public record HelpMenuContext(
    IReadOnlyList<AccountMenuItemModel> DefaultItems,
    Func<AccountMenuItemModel, Task> InvokeItem,
    Func<Task> GoBack,
    Func<Task> CloseMenu);
```

`GoBack` returns to the root menu. This lets custom template content close the submenu without direct access to component internals.

`CloseMenu` is a `Func<Task>` delegate that closes the entire root menu (equivalent to pressing Escape from the root). It is provided through the context record — templates do not have direct access to `_isOpen`.

Use `GoBack` when the correct behavior is to return to the root menu. Use `CloseMenu` when the action should dismiss the menu entirely (e.g., a "navigate and close" pattern, or a destructive action confirmation that ends the interaction).

### Rules
- Default rendering works when no templates are supplied.
- Templates replace only their target area, not surrounding structure.
- Injected content must not break separators, layout rhythm, keyboard behavior, or close behavior.
- Templates and Options work together — a consumer can set `Options.Appearance.CurrentValue` and also provide `AppearanceTemplate`.

---

## Internal Item Model

```csharp
public class AccountMenuItemModel
{
    public string Id { get; set; } = "";
    public string Label { get; set; } = "";
    public string Icon { get; set; } = "";
    public string? SecondaryText { get; set; }
    public string? ShortcutText { get; set; }
    public string? RightValueText { get; set; }
    public string? Badge { get; set; }
    public string? Href { get; set; }
    public Func<Task>? Action { get; set; }
    public List<AccountMenuItemModel>? Children { get; set; }
    public bool IsVisible { get; set; } = true;
    public bool IsDisabled { get; set; }
    public bool IsDestructive { get; set; }
    public bool IsSelected { get; set; }
    public bool IsSubmenu { get; set; }
    public int SortOrder { get; set; }
    public string? TestId { get; set; }
}
```

This model is used internally by the component and exposed through `HelpMenuContext.DefaultItems`. It is not the consumer-facing API — consumers use `AccountMenuOptions`.

---

## Behavior

### Trigger
- Clicking the trigger opens the account menu popup.
- Clicking outside or pressing Escape closes it.
- `CompactTrigger` toggles between avatar-only and avatar+name layouts.
- `TriggerTemplate` replaces the default trigger entirely.

### Root Menu
- Items render in 5 sections from the options + defaults.
- Only visible items render.
- Hidden sections (all items invisible) collapse including their separator.

### Submenus
- Clicking a submenu row (Appearance, Language, Help) opens the submenu panel.
- Submenus replace the root menu content (slide/swap, not flyout).
- Each submenu has a back row (chevron-left + title) to return to root.

### Navigation / Actions
- Items with `Href` navigate via `NavigationManager.NavigateTo()` and close the menu.
- Items with `Action` invoke the callback and close the menu.
- If both are set, `Href` takes precedence.
- `OnNavigate` fires for all navigation-based items with the route as the argument.
- `OnSignOut` fires when Sign out is clicked.
- `OnAppearanceChange` fires with the selected mode string.
- `OnLanguageChange` fires with the selected language string.

---

## Accessibility & Keyboard

### ARIA Model

Uses `role="menu"` / `role="menuitem"` pattern, matching existing `MariloUserMenu` and `MariloContextMenu`. This pattern follows the W3C APG Menu Button pattern (`aria-haspopup` + `aria-expanded` on the trigger, `role="menu"` on the panel).

- Root container: `role="menu"`, `aria-label="Account menu"`
- Menu items: `role="menuitem"`
- Submenu rows: `aria-haspopup="true"`, `aria-expanded="true|false"`
- Disabled items: `aria-disabled="true"`. See Styling / Disabled State Tokens for visual treatment.

**Trigger button ARIA requirements:**

- The trigger `<button>` carries the `aria-label`. The avatar SVG or image gets `aria-hidden="true"`. Do not place `aria-label` on the SVG itself.
- The trigger button also requires:
  - `aria-haspopup="menu"`
  - `aria-expanded="false"` (toggled to `"true"` when menu is open)
  - `aria-controls="{menu-panel-id}"`
- `aria-label` phrasing:
  - When `UserName` is non-empty: `aria-label="Account menu for {UserName}"`
  - When `UserName` is empty or not set: `aria-label="Account menu"`
- In default (expanded) trigger mode, visible text is present so `aria-label` is still recommended for consistency, but the button's text content alone satisfies the accessible name requirement.
- Do NOT include the word "button" in the label — screen readers announce the role automatically.

### Keyboard Navigation

| Key | Behavior |
|---|---|
| Arrow Down | Focus next visible item |
| Arrow Up | Focus previous visible item |
| Enter / Space | Activate item or open submenu |
| Escape | Close submenu first, then close root menu |
| Arrow Right | Open submenu from a submenu-capable row |
| Arrow Left | Return from submenu to root |
| Home | Focus first item |
| End | Focus last item |

### Focus
- Focus-visible outlines on all interactive rows.
- Opening the menu focuses the first item.
- Opening a submenu focuses the first submenu item.
- Returning from submenu focuses the submenu parent row.

---

## Styling

### Design Tokens

Uses inherited `--marilo-color-*` CSS variables. Local aliases follow the `--mam-*` prefix pattern:

```css
.mar-account-menu {
    --mam-bg: var(--marilo-color-surface, #ffffff);
    --mam-border: color-mix(in srgb, var(--marilo-color-text, #1f2328) 8%, transparent);
    --mam-text: var(--marilo-color-text, #1f2328);
    --mam-text-muted: color-mix(in srgb, var(--marilo-color-text, #1f2328) 50%, transparent);
    --mam-primary: var(--marilo-color-primary, #5b6cff);
    --mam-hover: color-mix(in srgb, var(--marilo-color-text, #1f2328) 6%, transparent);
    --mam-radius: var(--marilo-radius-md, 8px);
    --mam-shadow: 0 4px 24px rgba(0, 0, 0, 0.10);
    --mam-width: 300px;
    --mam-row-h: 38px;
    --mam-icon-w: 20px;
}
```

### Visual Constraints
- Width: 300px (adjustable via `--mam-width`)
- Row height: 38px
- Fixed 20px icon column
- 12px border-radius on panel
- 6px padding inside panel
- 8px border-radius on rows
- Separators: 1px solid `--mam-border`, 4px vertical margin
- Right-aligned value text in `--mam-text-muted`
- Sign out row: standard color, no destructive red treatment (matches Perplexity pattern)
- Submenu transition: slide-in from right (CSS transform)

### Disabled State Tokens

Disabled items use dedicated opacity tokens rather than hardcoded values:

```css
--mam-disabled-content-opacity: 0.38;
--mam-disabled-text: color-mix(
    in srgb,
    var(--marilo-color-text) calc(var(--mam-disabled-content-opacity) * 100%),
    transparent
);
```

- 0.38 (38%) is the Material Design 3 standard for disabled content (text and icons). It meets accessibility contrast requirements while clearly indicating non-interactivity.
- Disabled items apply `--mam-disabled-text` to both the label and icon.
- Disabled items have no hover background.
- `pointer-events: none` on disabled rows.
- Disabled items are skipped in keyboard focus traversal (not focusable).
- `aria-disabled="true"` is set; the HTML `disabled` attribute is NOT used on `<button>` or `<a>` tags inside menu rows (to preserve focusability for screen reader announcement if needed in future).

### Dark Mode
- Inherits from `MariloThemeProvider` via `[data-marilo-theme="dark"]`.
- All local tokens derive from `--marilo-color-*` which ThemeProvider sets.
- Shadow adjusts to `rgba(0, 0, 0, 0.28)` in dark mode.

---

## State Management

Internal state:

| State | Type | Purpose |
|---|---|---|
| `_isOpen` | `bool` | Menu open/closed |
| `_activeSubmenu` | `string?` | Which submenu is open (null = root) |
| `_focusedIndex` | `int` | Keyboard focus position |
| `_currentAppearance` | `string` | Selected appearance mode |
| `_currentLanguage` | `string` | Selected language |

Custom templates interact through context callbacks (`SelectMode`, `SelectLanguage`, `GoBack`, etc.) rather than mutating internal state directly.

---

## Integration with MainLayout

After implementation, `MainLayout.razor`'s `<SidebarFooter>` simplifies from ~90 lines of manual trigger + menu + theme picker wiring to:

```razor
<SidebarFooter>
    <MariloAccountMenu UserName="Avery Chen"
                       UserEmail="avery.chen@example.com"
                       CompactTrigger="_collapsed"
                       OnSignOut="HandleSignOut"
                       OnAppearanceChange="HandleAppearance" />
</SidebarFooter>
```

The component handles collapsed/expanded trigger layout, popup rendering, theme submenu, and close behavior internally.

---

## Acceptance Criteria

1. Component renders the exact 5-group structure.
2. Works with zero configuration (all defaults).
3. Optional items (Usage, Connectors, Upgrade, Install) are hidden by default and togglable via Options.
4. Appearance, Language, and Help submenus work with defaults and custom templates.
5. CompactTrigger mode works for collapsed sidebar.
6. TriggerTemplate allows full trigger replacement.
7. Keyboard navigation and ARIA roles are correct.
8. Dark mode styling works via inherited theme tokens.
9. All icons resolve from Marilo.Icons sprite.
10. Options and render fragments work together without conflicts.
11. MainLayout can be simplified to a single `<MariloAccountMenu>` tag.
12. Build succeeds with 0 errors.

---

## Decisions Made

| Decision | Choice | Rationale |
|---|---|---|
| Separate vs replace MariloUserMenu | Separate component | MariloUserMenu is a generic popup menu; account menu has domain-specific concerns |
| Flat vs nested options | Nested `AccountMenuItemOptions` | 13 items x ~8 properties = ~100 props in flat; nested keeps it organized |
| ARIA model | `role="menu"` / `role="menuitem"` | Matches existing MariloUserMenu and MariloContextMenu patterns |
| Appearance submenu scope | Light/Dark/System only | Theme presets belong on the Preferences page; quick-access handles the common toggle |
| Trigger included | Yes, self-contained | Trigger + popup are tightly coupled; packaging eliminates wiring bugs |
| TriggerTemplate | Included in V1 | Single RenderFragment, trivial to implement, needed escape hatch |
| Telemetry metadata | TestId only in V1 | Zero cost, immediately useful; analytics keys can be added later |
| Submenu style | Slide/swap (not flyout) | Contained within fixed-width popup; flyouts would overflow sidebar bounds |
