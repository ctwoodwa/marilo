# MariloMap Visual Parity Gaps -- Stage 03 Output

**Date:** 2026-04-12
**Worker:** w-map-delivery
**Component:** MariloMap

---

## Summary

The MariloMap component emits BEM classes from its razor template, but **no SCSS rules exist** in any provider (FluentUI, Bootstrap, or Material) for these classes. The component is entirely unstyled at the provider level -- all current styling is inline in the razor template.

### BEM Classes Emitted by MariloMap.razor

| CSS Class | Used In | Has SCSS Rule (FluentUI) | Has SCSS Rule (Bootstrap) | Has SCSS Rule (Material) |
|-----------|---------|--------------------------|---------------------------|--------------------------|
| `mar-map` | Root div | NO | NO | NO |
| `mar-map__canvas` | Canvas container div | NO | NO | NO |
| `mar-map__marker` | Each marker wrapper div | NO | NO | NO |
| `mar-map__marker-label` | Marker text label span | NO | NO | NO |

### Provider SCSS Files Checked

- **FluentUI:** `src/Marilo.Providers.FluentUI/Styles/components/` -- No `_map.scss` file exists. No references to `mar-map` in any SCSS file.
- **Bootstrap:** `src/Marilo.Providers.Bootstrap/` -- No map-related SCSS files found.
- **Material:** `src/Marilo.Providers.Material/` -- No map-related SCSS files found.

---

## Gap Details

### VP-map-001: No FluentUI SCSS for mar-map

**Class:** `mar-map`
**Provider:** FluentUI
**Severity:** P1 (blocking)
**Current state:** All container styling is inline in the razor template (`width`, `height`, `position`, `overflow`).
**Expected:** A `_map.scss` file in `Styles/components/` providing base container styles, dark-mode support, and token-driven values.
**Blocked by:** Architecture decision specifies that MapLibre owns canvas rendering; providers influence container CSS, control positioning, and marker popup styling. Implementation requires the MapLibre adapter first.

---

### VP-map-002: No FluentUI SCSS for mar-map__canvas

**Class:** `mar-map__canvas`
**Provider:** FluentUI
**Severity:** P2
**Current state:** Canvas background is inline: a CSS grid pattern simulating a map. This will be entirely replaced by the MapLibre canvas element.
**Expected:** After MapLibre integration, SCSS should handle the canvas container sizing, loading state, and dark-mode background fallback.
**Blocked by:** MapLibre adapter implementation.

---

### VP-map-003: No FluentUI SCSS for mar-map__marker

**Class:** `mar-map__marker`
**Provider:** FluentUI
**Severity:** P2
**Current state:** All marker positioning and cursor styles are inline.
**Expected:** After MapLibre integration, markers render as MapLibre symbol layers or HTML markers. Provider SCSS should style HTML marker containers, hover states, and focus states.
**Blocked by:** Marker layer implementation via MapLibre.

---

### VP-map-004: No FluentUI SCSS for mar-map__marker-label

**Class:** `mar-map__marker-label`
**Provider:** FluentUI
**Severity:** P2
**Current state:** Label styling (font size, background, padding, border-radius, box-shadow) is all inline.
**Expected:** Provider SCSS should use Fluent UI tokens for label typography, background, and elevation.
**Blocked by:** Marker layer implementation.

---

### VP-map-005: No Bootstrap Provider SCSS

**Class:** All `mar-map*` classes
**Provider:** Bootstrap
**Severity:** P2
**Expected:** A `_map.scss` in Bootstrap provider matching the FluentUI patterns but using Bootstrap tokens.
**Blocked by:** FluentUI implementation should come first as reference.

---

### VP-map-006: No Material Provider SCSS

**Class:** All `mar-map*` classes
**Provider:** Material
**Severity:** P3
**Expected:** A `_map.scss` in Material provider using Material Design tokens.
**Blocked by:** FluentUI implementation should come first as reference.

---

### VP-map-007: Additional Classes Expected Post-MapLibre

Per the architecture decision, the following BEM classes will likely be needed after MapLibre integration but are not yet emitted:

| Expected Class | Purpose |
|----------------|---------|
| `mar-map__controls` | Container for navigation/zoom/attribution controls |
| `mar-map__controls--top-left` | Position modifier for controls |
| `mar-map__controls--top-right` | Position modifier for controls |
| `mar-map__controls--bottom-left` | Position modifier for controls |
| `mar-map__controls--bottom-right` | Position modifier for controls |
| `mar-map__popup` | Marker popup/tooltip container |
| `mar-map__loading` | Loading state overlay during SSR/prerendering |
| `mar-map--dark` | Dark mode modifier (or theme-driven) |

These are future work and not scored as current gaps.

---

## Parity Score Summary

| Theme | Mode | Score | Notes |
|-------|------|-------|-------|
| FluentUI | Light | 0 | No SCSS exists |
| FluentUI | Dark | 0 | No SCSS exists |
| Bootstrap | Light | 0 | No SCSS exists |
| Bootstrap | Dark | 0 | No SCSS exists |
| Material | Light | 0 | No SCSS exists |
| Material | Dark | 0 | No SCSS exists |

**Score scale:** 0 = no provider styling, 1 = partial, 2 = functional, 3 = visually equivalent, 4 = polished

---

## Summary Counts

| Category | Count |
|----------|-------|
| Unstyled classes (current) | 4 |
| Missing provider SCSS files | 3 (FluentUI, Bootstrap, Material) |
| Future classes (post-MapLibre) | 8+ |
| **Overall parity status** | **NOT STARTED** |
