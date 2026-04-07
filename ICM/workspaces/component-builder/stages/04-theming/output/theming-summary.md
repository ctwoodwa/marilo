# Theming Summary: MariloResizableContainer

## Provider Implementations

Both FluentUI and Bootstrap providers implement these methods:
- `ResizableContainerClass(bool isResizing, bool isDisabled)`
- `ResizableContainerContentClass()`
- `ResizableContainerHandleClass(MariloResizeEdges edge, bool isActive, bool isFocused)`

## SCSS Files Created

| File | Provider |
|------|----------|
| `src/Marilo.Providers.FluentUI/Styles/_resizable-container.scss` | FluentUI |
| `src/Marilo.Providers.Bootstrap/Styles/_bridge-resizable-container.scss` | Bootstrap |

## SCSS Imports Added

- FluentUI: `@forward 'resizable-container'` in `marilo-fluentui.scss`
- Bootstrap: `@import "bridge-resizable-container"` in `marilo-bootstrap.scss`

## SCSS Build Status

Both providers compile successfully via `npm run scss:build`.

## Style Features

- Container: positioned, bordered, overflow hidden, disabled opacity
- Content: full width/height with overflow auto (scroll support)
- Handle positions: right, bottom, bottom-right, left, top, and all corner variants
- Handle visual: subtle bar/grip indicators using `::after` pseudo-elements
- Hover/active state: handle indicator turns primary color
- Focus state: visible focus ring (FluentUI: solid box-shadow, Bootstrap: 0.25rem RGBA)
- Ghost outline: dashed primary border with transparent fill during drag
- Reduced motion: transitions disabled via `prefers-reduced-motion`
- FluentUI uses Marilo design tokens (`--marilo-color-*`)
- Bootstrap uses Bootstrap variables (`--bs-*`, `--bs-border-color`)
