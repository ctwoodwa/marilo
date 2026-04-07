# Design Language - Blazor and Telerik Implementation

How the technology-agnostic design language maps to Blazor + Telerik Fluent.
This is the stack implementation guide. Source of truth is design-language-reference.md.
Do not duplicate rules from that file here - only add implementation-specific mappings.

---

## 1. Token Mapping

All design tokens map to Telerik Fluent CSS variables or SCSS tokens.
All mappings live in _telerik-overrides.scss. No raw hex values are permitted outside _tokens.scss.

| Design token | Telerik / CSS variable | Notes |
|---|---|---|
| color-accent | --kendo-color-primary | Buttons, links, focused rings |
| color-bg-page | --kendo-color-bg | Applied to html, body |
| color-bg-surface | Card and panel background | Override .k-card background |
| color-text-primary | --kendo-color-text | All body text |
| color-text-muted | --kendo-subtle-text | Labels, captions, secondary copy |
| color-border-subtle | --kendo-border-color | Grid lines, card borders |
| color-success | --kendo-color-success | Success pills, check icons |
| color-warning | --kendo-color-warning | Warning pills, alert icons |
| color-error | --kendo-color-error | Error states, validation messages |
| space-1 through space-6 | Sass variables in _tokens.scss | Used in .k-* overrides and utility classes |
| radius-sm | Applied to inputs, buttons | .k-input, .k-button border-radius |
| radius-md | Applied to cards, panels | .k-card, .k-dialog border-radius |
| shadow-subtle | Applied to .k-card | box-shadow override |
| shadow-modal | Applied to .k-dialog | box-shadow override |

---

## 2. Theme Loading Rules

- Exactly one Telerik theme CSS is loaded at a time. Multiple theme links cause flicker and inconsistent styles.
- Load order: Telerik base theme first, then _telerik-overrides.scss, then app utilities.
- Override global colors via CSS variables at :root - this updates all Telerik components automatically.
- Use ThemeBuilder or Sass source build for large-scale customization (more than color tweaks).
- After any Telerik NuGet version upgrade, rebuild or re-export any ThemeBuilder or Sass-based themes.
- Scan Telerik release notes for CSS/HTML breaking changes after upgrades.

Example correct theme load order in index.html or _Host.cshtml:

    link rel=stylesheet href=_content/Telerik.UI.for.Blazor.Themes.fluent.css
    link rel=stylesheet href=css/telerik-fluent-overrides.css

---

## 3. CSS Variable Override Pattern

Use CSS variable overrides at :root for global token wiring. This is the preferred approach
for color changes. No need to override dozens of .k-* selectors.

    :root {
      --kendo-color-primary: var(--color-accent);
      --kendo-color-text: var(--color-text-primary);
      --kendo-color-bg: var(--color-bg-page);
      --kendo-border-color: var(--color-border-subtle);
    }

For light and dark theme switching:

    :root[data-theme='light'] {
      --color-bg-page: #F3F3F3;
      --color-bg-surface: #FFFFFF;
      --color-text-primary: #1E1E1E;
      --color-text-muted: #7F7F7F;
      --color-accent: #0078D4;
      --color-border-subtle: #E0E0E0;
    }

    :root[data-theme='dark'] {
      --color-bg-page: #111318;
      --color-bg-surface: #1C1F26;
      --color-text-primary: #F3F3F3;
      --color-text-muted: #9E9E9E;
      --color-accent: #4CA3DD;
      --color-border-subtle: #2E3038;
    }

Charts and gauges must be explicitly refreshed after a runtime theme switch.

---

## 4. Shell Implementation

| Shell element | Blazor implementation |
|---|---|
| Top bar | TelerikAppBar or custom component using shell token utility classes |
| Left nav rail | TelerikPanelBar or custom nav using u-nav-* utility classes |
| Breadcrumb | TelerikBreadcrumb or custom using u-breadcrumb utility |
| Context panels | TelerikSplitter containing TelerikDockManager panes |
| Page header | Custom Razor component: H1, metadata chips, primary action button |
| Content canvas | Standard HTML div with u-content-canvas utility class |

All shell components use utility classes driven by design tokens.
No per-page CSS is applied to shell elements.

---

## 5. Screen Pattern Implementations

### Explorer (List View)

| Element | Telerik component | Notes |
|---|---|---|
| Data grid | TelerikGrid | Filter row, column menu, pagination or virtual scroll |
| Search input | TelerikTextBox | Bound to grid filter state |
| Filter panel | TelerikDropDownList, TelerikDatePicker | Top or side panel |
| Row actions | TelerikButton icon-only | Grouped right; overflow to TelerikContextMenu |
| Bulk actions | TelerikButton + TelerikCheckBox | Appear on row selection |

### Workspace (Detail View)

| Element | Telerik component | Notes |
|---|---|---|
| Tab sections | TelerikTabStrip | Overview, Details, Related, Activity |
| Side-by-side layout | TelerikSplitter | Form left, visualization or results right |
| Context pane | TelerikDockManager pane | Inspector, help, history |
| Forms | TelerikForm or manual layout | Field groups with section headings |
| Dialogs | TelerikDialog or TelerikWindow | Confirm, edit, and detail pop-overs |

### Admin Hub (Settings)

| Element | Telerik component | Notes |
|---|---|---|
| Top tab sections | TelerikTabStrip | One tab per admin domain |
| Record list | TelerikGrid | Left side, single-select |
| Detail form | TelerikForm | Right side, responds to grid selection |
| Save/cancel | TelerikButton Primary and Secondary | Right-aligned in form footer |

---

## 6. Component Rules

### Buttons
- Use ThemeColor=Primary to map to color-accent. Do not override per component.
- Use ThemeColor=Secondary for supporting actions.
- Icon-only buttons in grids: use FillMode=Flat and Icon property.

### Grids
- Do not apply per-page CSS overrides to TelerikGrid.
- Column widths set via Width property; avoid CSS column overrides.
- Header styling via .k-grid .k-grid-header .k-header only, scoped and minimal.

### Dialogs and Windows
- TelerikDialog for confirmations and simple forms.
- TelerikWindow for floating inspectors and detail panels.
- Both inherit border-radius and shadow from _telerik-overrides.scss tokens.

### Charts
- Use the Telerik chart theme; do not inline series colors except for status mapping.
- Primary series color = color-accent. Status series = color-success/warning/error.
- Explicitly refresh chart components after a runtime theme switch.

### Inputs
- TelerikTextBox, TelerikDropDownList, TelerikDatePicker all inherit from _telerik-overrides.scss.
- Required field marking: use the Required parameter and a custom label class.
- Validation messages: use TelerikValidationMessage; style via color-error token.

---

## 7. Override Quality Rules

- All overrides must reference Parsons design tokens, not raw hex or px values.
- Use stable, documented .k-* class names. Avoid DOM-depth selectors.
- Scope app-level global rules to app containers, not bare HTML elements.
- Keep override files small and purpose-specific (_telerik-overrides.scss, _utilities.scss).

GOOD:

    .k-grid .k-grid-header .k-header {
      font-size: var(--font-size-caption);
      font-weight: 600;
    }

BAD:

    .k-grid > .k-grid-header > .k-grid-header-wrap > table > thead > tr > th {
      font-size: 12px;
    }

---

## 8. Pre-Merge Theming Checklist

- Only one Telerik theme CSS is loaded.
- All new styles reference design tokens or CSS vars, not raw values.
- Overrides are scoped to .k-* and app containers; no bare button/table/input hacks.
- Light and dark switching works by toggling data-theme; charts update correctly.
- Custom theme rebuild steps are documented after any Telerik upgrade.
- Key screens (Explorer, Workspace, Admin Hub) visually verified at two resolutions.
- Shell and Telerik components look like one consistent system (no mismatched grays or font sizes).

---

## 9. File Layout Reference

| File | Purpose |
|---|---|
| _tokens.scss | Single source of truth for all design token values |
| _telerik-overrides.scss | Maps design tokens to Telerik CSS variables and .k-* selectors |
| _utilities.scss | Token-driven utility classes for layout, spacing, typography |
| _base.scss | Global html/body styles using tokens |
| THEMING.md | Documents the customization approach and rebuild steps for this project |