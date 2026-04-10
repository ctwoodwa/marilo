# Marilo Bootstrap Provider — Styles Architecture

## Folder Structure

```
Styles/
├── foundation/    Design tokens derived from Bootstrap Sass variables
├── patterns/      Cross-component patterns (placeholders — Bootstrap handles natively)
├── components/    One file per Marilo component (bridge styles with mar-bs-* classes)
├── _index.scss    Import aggregator (foundation → patterns → components)
├── _variables.scss   Bootstrap variable overrides (loaded before BS variables)
└── marilo-bootstrap.scss   Build entrypoint (includes full Bootstrap 5.3 + Marilo styles)
```

## What Belongs Where

### `foundation/`
- **Design tokens**: CSS custom properties (`--marilo-*`) derived from Bootstrap Sass variables
- **`_colors.scss`**: Color tokens (light + dark mode), both `:root` and `[data-marilo-theme="dark"]`
- **Typography, spacing, radius, elevation, motion**: Scale definitions using BS Sass vars
- **`_tokens.scss`**: Z-index hierarchy

### `patterns/`
- Placeholders — Bootstrap handles interactive states, field chrome, overlays, and validation natively
- Files exist for parity with FluentUI and Material providers

### `components/`
- One SCSS partial per Marilo component
- Bridge styles using `mar-bs-*` class prefixes
- May use Bootstrap Sass variables (`$primary`, `$border-color`, etc.) directly — they are globally available via `@import` chain

## Import Order

The `_index.scss` uses this exact order:

1. **Foundation** (A): colors → spacing → typography → radius → elevation → motion → functions → mixins → tokens
2. **Patterns** (B): interactive-states → field-base → overlay → validation → density
3. **Components** (C): Alphabetical by file name

**Important**: `marilo-bootstrap.scss` imports full Bootstrap 5.3 first (steps 1-6), then `@import "index"` for Marilo styles. Bootstrap Sass variables are globally available to all component files.

## Naming Conventions

| Element | Convention | Example |
|---------|-----------|---------|
| Folders | lowercase, singular | `foundation/`, `patterns/`, `components/` |
| SCSS files | underscore partial, kebab-case | `_text-field.scss`, `_data-grid.scss` |
| CSS custom properties | `--marilo-*` namespace | `--marilo-color-primary`, `--marilo-spacing-4` |
| Sass variables | `$bootstrap-*` for new maps; BS native vars OK | `$bootstrap-colors` |
| Bridge classes | `mar-bs-*` prefix | `.mar-bs-icon-button`, `.mar-bs-fab` |
| Selectors | Preserve existing class names | `.mar-bs-stepper`, `.mar-splitter__handle` |

## Adding a New Component Partial

1. Create `components/_my-component.scss`
2. Add `@forward 'components/my-component';` to `_index.scss` in alphabetical position within section C
3. Use `var(--marilo-*)` tokens for runtime-consumable values
4. Bootstrap Sass variables (`$primary`, etc.) are available for compile-time composition

## Token Mapping

All provider tokens map to `--marilo-*` CSS custom properties, which are consumed by `MariloThemeProvider` for runtime theme switching. Bootstrap Sass variables are used at compile time to derive these tokens.

## Build

```bash
npm run scss:build:bootstrap
npm run scss:watch:bootstrap
```
