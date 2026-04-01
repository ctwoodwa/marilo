# Implementation Notes: MariloThemeProvider

## Design Decisions

1. **Wrapper div over `<style>` injection**: Chose inline CSS custom properties on a wrapper `<div>` instead of injecting a `<style>` block or using JS interop. This scopes theme overrides to the provider subtree, supports nested providers, and works with SSR/prerendering.

2. **Dark palette selection in C#**: When `ThemeService.IsDarkMode` is true and `Theme.Colors.Dark` is not null, the dark palette is used for CSS variable generation. This works alongside the SCSS-based `[data-marilo-theme="dark"]` selector in providers.

3. **InitializeAsync in OnAfterRenderAsync**: Called on first render because it uses `IJSRuntime` to read localStorage, which is not available during server prerendering.

## Approach

- Added a `<div class="marilo-theme-provider">` wrapper inside the `<CascadingValue>`
- The wrapper receives `Class`, `Style`, `AdditionalAttributes`, `dir`, and `data-marilo-theme`
- `GenerateThemeStyles()` builds CSS custom properties from `MariloColorPalette`, `MariloTypographyScale`, and `MariloShape` using `StyleBuilder`
- Provider SCSS sets base values on `:root`; the wrapper div's inline styles override within the subtree at runtime

## Code Notes

### CSS Variable Mapping

| C# Property | CSS Variable |
|---|---|
| `Colors.Primary` | `--marilo-color-primary` |
| `Colors.Secondary` | `--marilo-color-secondary` |
| `Colors.Danger` | `--marilo-color-danger` |
| `Colors.Warning` | `--marilo-color-warning` |
| `Colors.Info` | `--marilo-color-info` |
| `Colors.Success` | `--marilo-color-success` |
| `Colors.Background` | `--marilo-color-bg` |
| `Colors.Surface` | `--marilo-color-surface` |
| `Colors.OnBackground` | `--marilo-color-text` |
| `Typography.FontFamily` | `--marilo-font-family` |
| `Typography.FontSizeBase` | `--marilo-font-size-base` |
| `Shape.BorderRadius` | `--marilo-radius-md` |
| `Shape.BorderRadiusLarge` | `--marilo-radius-lg` |
| `Shape.Elevation1` | `--marilo-shadow-sm` |
| `Shape.Elevation2` | `--marilo-shadow-md` |
| `Shape.Elevation3` | `--marilo-shadow-lg` |

### Naming Convention

CSS variable names follow the existing `_tokens.scss` convention (`--marilo-{category}-{name}`) to ensure runtime overrides align with SCSS-compiled defaults.

### Not Mapped (Provider-Specific)

The following CSS variables from `_tokens.scss` are not mapped from C# theme tokens because they are provider-specific computed values (derived from Sass functions like `shade-color`, `tint-color`):
- `--marilo-color-primary-hover`, `--marilo-color-primary-active`, `--marilo-color-primary-light`
- `--marilo-color-*-light` variants
- `--marilo-font-size-*` variants beyond base (xs, sm, md, lg, xl, 2xl, 3xl)
- `--marilo-space-*` spacing tokens
- `--marilo-radius-sm`, `--marilo-radius-xl`, `--marilo-radius-full`
- `--marilo-transition-*`, `--marilo-z-*`

These remain controlled by the CSS provider's SCSS compilation.
