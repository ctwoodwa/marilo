# MariloDiagram -- Visual Parity Audit (Stage 03)

**Date:** 2026-04-12
**Worker:** w-diagram-delivery
**Component source:** `src/Marilo.Components/DataDisplay/MariloDiagram.razor`

---

## BEM Class Inventory (Source)

The component uses the following CSS classes:

| # | BEM Class | Element | Location |
|---|---|---|---|
| 1 | `mar-diagram` | Root container `<div>` | L3 |
| 2 | `mar-diagram__canvas` | SVG element | L7 |

**Total BEM classes: 2**

---

## Provider SCSS Coverage

### FluentUI Provider

**Search result:** No files found matching `*diagram*` or `*Diagram*` under `src/Marilo.Providers.FluentUI/`.

**SCSS rules for `mar-diagram` or `mar-diagram__canvas`: NONE**

### Bootstrap Provider

**Search result:** No files found matching `*diagram*` or `*Diagram*` under `src/Marilo.Providers.Bootstrap/`.

**SCSS rules for `mar-diagram` or `mar-diagram__canvas`: NONE**

---

## Unstyled Classes

| # | BEM Class | FluentUI Status | Bootstrap Status | Impact |
|---|---|---|---|---|
| 1 | `mar-diagram` | UNSTYLED | UNSTYLED | Root container has no provider styling; all visual properties are inline `style` attributes |
| 2 | `mar-diagram__canvas` | UNSTYLED | UNSTYLED | SVG canvas has no provider styling; positioned via inline `style` |

---

## Inline Style Analysis

The component relies **entirely** on inline styles and hardcoded SVG attributes rather than provider-driven CSS:

| Element | Inline Styles / Hardcoded Values | Should Be Provider-Driven |
|---|---|---|
| Root div | `width`, `height`, `position:relative`, `overflow:hidden` | `position` and `overflow` could be in SCSS |
| SVG canvas | `position:absolute;top:0;left:0` | Should be in SCSS |
| Arrow marker | `fill="#666"` | Should use CSS custom property or provider token |
| Edge lines | `stroke="#666"`, `stroke-width="2"` | Should use provider color/width tokens |
| Rect shapes | `fill="#e3f2fd"`, `stroke="#1976d2"`, `stroke-width="2"`, `rx="4"`, `ry="4"` | Should use provider color tokens |
| Ellipse shapes | `fill="#e3f2fd"`, `stroke="#1976d2"`, `stroke-width="2"` | Should use provider color tokens |
| Node text | `fill="#333"`, `font-size="14"` | Should use provider text tokens |
| Edge text | `fill="#666"`, `font-size="12"` | Should use provider text tokens |
| All interactive elements | `cursor:pointer` | Should be in SCSS |
| foreignObject content | `display:flex;align-items:center;justify-content:center` | Should be in SCSS |

---

## Visual Parity Score

| Dimension | Score | Notes |
|---|---|---|
| BEM class coverage | 2/2 classes exist | Low total -- component needs many more BEM classes as features are added |
| FluentUI SCSS rules | 0/2 | Zero styling rules in FluentUI provider |
| Bootstrap SCSS rules | 0/2 | Zero styling rules in Bootstrap provider |
| Hardcoded colors | 6 instances | `#666`, `#e3f2fd`, `#1976d2`, `#333`, `#666`, `#666` -- none use CSS custom properties |
| Inline style reliance | HIGH | Nearly all visual properties are inline |

**Overall parity: 0% (no provider SCSS exists)**

---

## Recommendations

1. **Create provider SCSS files** for both FluentUI and Bootstrap with at minimum:
   - `mar-diagram` root: position, overflow, default dimensions
   - `mar-diagram__canvas` SVG: positioning
   - CSS custom properties for all colors (`--mar-diagram-node-fill`, `--mar-diagram-node-stroke`, `--mar-diagram-edge-stroke`, `--mar-diagram-text-color`, etc.)

2. **Replace hardcoded SVG attributes** with `var(--mar-diagram-*)` custom properties or classes.

3. **As features are implemented**, add BEM classes for:
   - `mar-diagram__node` (shapes)
   - `mar-diagram__node--selected`
   - `mar-diagram__edge` (connections)
   - `mar-diagram__edge--selected`
   - `mar-diagram__connector` (hover dots)
   - `mar-diagram__label` (text labels)
   - `mar-diagram__marker` (arrow markers)
   - Shape type modifiers: `mar-diagram__node--rectangle`, `mar-diagram__node--ellipse`, etc.

4. **Priority:** This is blocked on source development. Provider SCSS should be created alongside source features, not retroactively.
