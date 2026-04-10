---
uid: theming-token-reference
title: Token Reference
description: Complete reference for all --marilo-* CSS custom properties used across Marilo providers.
---

# Token Reference

All Marilo providers expose a shared set of `--marilo-*` CSS custom properties on `:root`. These tokens are the single source of truth consumed by every component's SCSS. Overriding a token at the `:root` level (or on any ancestor element) changes the appearance of all components that use it.

Providers define their own default values for each token. The tables below show the defaults shipped by the three built-in providers.

---

## Colors

### Brand colors

| Token | FluentUI default | Bootstrap default | Material 3 default |
| --- | --- | --- | --- |
| `--marilo-color-primary` | `#0078D4` | `#0d6efd` | `#6750A4` |
| `--marilo-color-primary-hover` | `#106EBE` | `#0b5ed7` | `#7965AF` |
| `--marilo-color-primary-active` | `#005A9E` | `#0a58ca` | `#4F378B` |
| `--marilo-color-primary-subtle` | `#EFF6FC` | `#cfe2ff` | `#EADDFF` |
| `--marilo-color-secondary` | `#2B88D8` | `#6c757d` | `#625B71` |
| `--marilo-color-secondary-hover` | `#1A6EB5` | `#5c636a` | `#7A7289` |
| `--marilo-color-secondary-subtle` | `#EFF6FC` | `#e2e3e5` | `#E8DEF8` |

### Semantic colors

| Token | FluentUI default | Bootstrap default | Material 3 default |
| --- | --- | --- | --- |
| `--marilo-color-success` | `#107C10` | `#198754` | `#386A20` |
| `--marilo-color-success-subtle` | `#DFF6DD` | `#d1e7dd` | `#C3EFAD` |
| `--marilo-color-warning` | `#FFB900` | `#ffc107` | `#6E4A00` |
| `--marilo-color-warning-subtle` | `#FFF4CE` | `#fff3cd` | `#FFE169` |
| `--marilo-color-danger` | `#D13438` | `#dc3545` | `#B3261E` |
| `--marilo-color-danger-subtle` | `#FDE7E9` | `#f8d7da` | `#F9DEDC` |
| `--marilo-color-info` | `#0078D4` | `#0dcaf0` | `#006781` |
| `--marilo-color-info-subtle` | `#EFF6FC` | `#cff4fc` | `#B6EAFF` |

### Surface and background

| Token | FluentUI default | Bootstrap default | Material 3 default |
| --- | --- | --- | --- |
| `--marilo-color-background` | `#FAF9F8` | `#ffffff` | `#FFFBFE` |
| `--marilo-color-surface` | `#ffffff` | `#ffffff` | `#FFFBFE` |
| `--marilo-color-surface-variant` | `#F3F2F1` | `#f8f9fa` | `#E7E0EC` |
| `--marilo-color-subtle-background` | `#F5F5F5` | `#f8f9fa` | `#F4EFF4` |
| `--marilo-color-overlay` | `rgba(0,0,0,0.4)` | `rgba(0,0,0,0.5)` | `rgba(0,0,0,0.32)` |

### Text and border

| Token | FluentUI default | Bootstrap default | Material 3 default |
| --- | --- | --- | --- |
| `--marilo-color-text` | `#323130` | `#212529` | `#1C1B1F` |
| `--marilo-color-text-secondary` | `#605E5C` | `#6c757d` | `#49454F` |
| `--marilo-color-text-disabled` | `#A19F9D` | `#adb5bd` | `#1C1B1F61` |
| `--marilo-color-text-on-primary` | `#ffffff` | `#ffffff` | `#ffffff` |
| `--marilo-color-border` | `#D2D0CE` | `#dee2e6` | `#79747E` |
| `--marilo-color-border-strong` | `#8A8886` | `#adb5bd` | `#49454F` |
| `--marilo-color-disabled-background` | `#F3F2F1` | `#e9ecef` | `#E7E0EC` |
| `--marilo-color-disabled-text` | `#A19F9D` | `#adb5bd` | `#1C1B1F61` |

---

## Typography

### Font family

| Token | FluentUI default | Bootstrap default | Material 3 default |
| --- | --- | --- | --- |
| `--marilo-font-family` | `"Segoe UI", system-ui, sans-serif` | `system-ui, -apple-system, sans-serif` | `"Roboto", system-ui, sans-serif` |
| `--marilo-font-family-mono` | `"Cascadia Code", "Consolas", monospace` | `"SFMono-Regular", "Consolas", monospace` | `"Roboto Mono", "Consolas", monospace` |

### Font size

| Token | Value (all providers) |
| --- | --- |
| `--marilo-font-size-xs` | `0.75rem` (12px) |
| `--marilo-font-size-sm` | `0.875rem` (14px) |
| `--marilo-font-size-md` | `1rem` (16px) |
| `--marilo-font-size-lg` | `1.125rem` (18px) |
| `--marilo-font-size-xl` | `1.25rem` (20px) |
| `--marilo-font-size-2xl` | `1.5rem` (24px) |
| `--marilo-font-size-3xl` | `1.875rem` (30px) |
| `--marilo-font-size-4xl` | `2.25rem` (36px) |

Font size values are identical across all three providers because they are defined in the shared Marilo.Core token layer.

### Font weight

| Token | Value (all providers) |
| --- | --- |
| `--marilo-font-weight-regular` | `400` |
| `--marilo-font-weight-medium` | `500` |
| `--marilo-font-weight-semibold` | `600` |
| `--marilo-font-weight-bold` | `700` |

### Line height

| Token | Value (all providers) |
| --- | --- |
| `--marilo-line-height-tight` | `1.25` |
| `--marilo-line-height-normal` | `1.5` |
| `--marilo-line-height-relaxed` | `1.75` |

---

## Spacing

The spacing scale is shared across all providers.

| Token | Value |
| --- | --- |
| `--marilo-space-xxs` | `0.25rem` (4px) |
| `--marilo-space-xs` | `0.5rem` (8px) |
| `--marilo-space-sm` | `0.75rem` (12px) |
| `--marilo-space-md` | `1rem` (16px) |
| `--marilo-space-lg` | `1.5rem` (24px) |
| `--marilo-space-xl` | `2rem` (32px) |
| `--marilo-space-2xl` | `3rem` (48px) |
| `--marilo-space-3xl` | `4rem` (64px) |

---

## Shape

| Token | FluentUI default | Bootstrap default | Material 3 default |
| --- | --- | --- | --- |
| `--marilo-radius-sm` | `2px` | `0.25rem` | `4px` |
| `--marilo-radius-md` | `4px` | `0.375rem` | `8px` |
| `--marilo-radius-lg` | `8px` | `0.5rem` | `12px` |
| `--marilo-radius-xl` | `16px` | `1rem` | `16px` |
| `--marilo-radius-full` | `9999px` | `9999px` | `9999px` |

---

## Elevation

Box shadows encode depth. Dark mode overrides are co-located in each provider's `_elevation.scss`.

| Token | FluentUI default | Bootstrap default | Material 3 default |
| --- | --- | --- | --- |
| `--marilo-shadow-sm` | `0 1px 2px rgba(0,0,0,0.12)` | `0 0.125rem 0.25rem rgba(0,0,0,0.075)` | `0 1px 2px rgba(0,0,0,0.3)` |
| `--marilo-shadow-md` | `0 2px 8px rgba(0,0,0,0.14)` | `0 0.5rem 1rem rgba(0,0,0,0.15)` | `0 2px 6px rgba(0,0,0,0.15)` |
| `--marilo-shadow-lg` | `0 4px 16px rgba(0,0,0,0.14)` | `0 1rem 3rem rgba(0,0,0,0.175)` | `0 4px 8px rgba(0,0,0,0.2)` |
| `--marilo-shadow-xl` | `0 8px 24px rgba(0,0,0,0.18)` | `0 1rem 3rem rgba(0,0,0,0.2)` | `0 6px 10px rgba(0,0,0,0.2)` |

---

## Motion

| Token | Value (all providers) |
| --- | --- |
| `--marilo-transition-fast` | `100ms ease` |
| `--marilo-transition-normal` | `200ms ease` |
| `--marilo-transition-slow` | `350ms ease` |

Use these on `transition` properties rather than hard-coding durations, so a future density or accessibility setting can adjust all transitions centrally.

---

## Z-index

| Token | Value (all providers) |
| --- | --- |
| `--marilo-z-dropdown` | `1000` |
| `--marilo-z-sticky` | `1020` |
| `--marilo-z-fixed` | `1030` |
| `--marilo-z-overlay` | `1040` |
| `--marilo-z-modal` | `1050` |
| `--marilo-z-popover` | `1060` |
| `--marilo-z-tooltip` | `1070` |
| `--marilo-z-toast` | `1080` |

---

## Focus

| Token | FluentUI default | Bootstrap default | Material 3 default |
| --- | --- | --- | --- |
| `--marilo-focus-ring` | `0 0 0 2px #fff, 0 0 0 4px #0078D4` | `0 0 0 0.25rem rgba(13,110,253,0.25)` | `0 0 0 3px #6750A4` |
| `--marilo-focus-ring-offset` | `2px` | `0` | `2px` |

---

## Overriding tokens

Tokens can be overridden at any CSS scope. To change the primary color for a single section without affecting the rest of the application:

```css
.my-section {
  --marilo-color-primary: #E91E63;
  --marilo-color-primary-hover: #C2185B;
  --marilo-color-primary-subtle: #FCE4EC;
}
```

To override globally via `MariloTheme`, set `MariloColorPalette.Primary` and `MariloThemeProvider` will emit the token to `:root` automatically. See [Theming Overview](xref:theming-overview) for the full theme API.

## See also

- [Theming Overview](xref:theming-overview) -- `MariloTheme` and `MariloThemeProvider`.
- [Dark Mode](xref:theming-dark-mode) -- how dark overrides are layered on top of these tokens.
- [Providers](xref:theming-providers) -- how providers implement these tokens in SCSS.
