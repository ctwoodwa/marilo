---
component: MariloThemeProvider
phase: 1
status: implemented
complexity: multi-pass
priority: critical
owner: ""
last-updated: 2026-03-31
depends-on: []
external-resources:
  - name: ""
    url: ""
    license: ""
    approved: false
---

# Resolution Status: MariloThemeProvider

## Current Phase
Phase 1: Core theme infrastructure

## Gap Summary
8 gaps — no CSS variable generation, dark mode not implemented, sync vs async SetTheme mismatch, no RTL wrapper.

## Resolution Approach
**RES-THEME-001**: Added wrapper `<div>` element with:
- CSS custom properties generated from MariloColorPalette, MariloTypographyScale, MariloShape
- `data-marilo-theme="dark|light"` attribute for dark mode CSS selector support
- `dir="rtl"` attribute when `Theme.IsRtl` is true
- `Class`, `Style`, `AdditionalAttributes` pass-through to wrapper
- Dark palette fallback when `ThemeService.IsDarkMode && Theme.Colors.Dark` exists

**RES-THEME-002**: Fixed async void handler with try/catch for disposal races. Added `InitializeAsync()` call in `OnAfterRenderAsync(firstRender)` to load persisted theme preferences from localStorage.

**RES-THEME-003**: `ThemeChanged` EventCallback confirmed as correct public API. Flagged for documentation update.

## Resolved Gaps
- [x] GAP-4: SetTheme sync/async — interface is `SetThemeAsync()`, doc fix needed (no code change)
- [x] GAP-6: RTL support — wrapper div renders `dir="rtl"` when `Theme.IsRtl`
- [x] GAP-7: CSS variable generation — inline styles emit `--marilo-color-*`, `--marilo-font-*`, `--marilo-radius-*`, `--marilo-shadow-*`
- [x] GAP-8: Dark mode toggle — `data-marilo-theme="dark"` attribute set, dark palette used when available
- [x] Code→Spec GAP-1: ThemeChanged EventCallback — confirmed, flagged for docs
- [x] Code→Spec GAP-4: async void handler — wrapped in try/catch
- [x] Code→Spec GAP-5: No DOM output — wrapper div added
- [x] Open Question 4: InitializeAsync — called in OnAfterRenderAsync(firstRender)

## Blockers
- None

## Pending
- Documentation update for ThemeChanged parameter and SetThemeAsync naming
- Validation stage (bUnit tests)
