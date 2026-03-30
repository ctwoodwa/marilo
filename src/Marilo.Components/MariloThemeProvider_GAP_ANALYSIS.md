# MariloThemeProvider Gap Analysis

## Summary

MariloThemeProvider is a small component with a focused responsibility: cascade a `MariloTheme` to child components and react to runtime theme changes via `IMariloThemeService`. The implementation aligns well with the documented behavior, but several gaps exist around API naming, dark mode support, initialization, and CSS variable generation.

---

## Spec → Code Gaps

| # | Spec Feature | Status | Severity | Details |
|---|---|---|---|---|
| 1 | Wrap application in `MariloThemeProvider` to cascade theme | **Implemented** | -- | Component uses `<CascadingValue Value="@Theme">` to provide `MariloTheme` to descendants. |
| 2 | `Theme` parameter (`MariloTheme`) | **Implemented** | -- | Defaults to `new MariloTheme()`. Spec shows custom theme passed via `Theme="@customTheme"`. |
| 3 | `ChildContent` parameter (`RenderFragment?`) | **Implemented** | -- | Renders child content inside the cascading value. |
| 4 | Runtime theme switching via `IMariloThemeService` | **Partially implemented** | **Medium** | The component subscribes to `ThemeService.ThemeChanged` and updates `Theme` + calls `StateHasChanged()`. However, the spec documents `ThemeService.SetTheme(...)` as the API for runtime switching, while the actual `IMariloThemeService` interface defines `SetThemeAsync(...)` (async). The docs show a synchronous `SetTheme()` call that does not match the interface. |
| 5 | Default theme when no custom theme supplied | **Implemented** | -- | `Theme` defaults to `new MariloTheme()`. Spec says "uses the provider's default theme values" but the component just uses `MariloTheme`'s own defaults -- no provider-specific defaults are consulted. |
| 6 | `MariloTheme.IsRtl` support | **Partially implemented** | **Low** | `MariloTheme` has an `IsRtl` property per the spec. The base class exposes `IsRtl` via `ThemeService.IsRtl`. However, `MariloThemeProvider` does not render a `dir="rtl"` attribute on any wrapper element -- it is purely a cascading value with no DOM output. RTL must be handled downstream. |
| 7 | CSS variable generation from theme tokens | **Not implemented** | **Medium** | The spec implies that `MariloColorPalette`, `MariloTypographyScale`, and `MariloShape` tokens translate to runtime CSS. The component does not generate any `<style>` element or CSS custom properties from the theme record. Theme tokens are available in C# but never emitted as CSS. Providers must handle this separately. |
| 8 | Dark mode toggle | **Not implemented** | **Medium** | `IMariloThemeService` defines `IsDarkMode` and `ToggleDarkModeAsync()`. The spec's overview does not explicitly show a dark mode toggle, but the service interface supports it. `MariloThemeProvider` does not render any dark-mode-specific class (e.g., `dark` or `mar-dark`) on a root element. |

---

## Code → Spec Gaps

| # | Implemented Feature | Documented? | Severity | Details |
|---|---|---|---|---|
| 1 | `ThemeChanged` EventCallback parameter | **No** | **Medium** | The component exposes `[Parameter] public EventCallback<ThemeChangedEventArgs> ThemeChanged`. This parameter is not documented in the theming overview or any spec page. Consumers can use it to react to theme changes at the provider level. |
| 2 | `ThemeService.ThemeChanged` event subscription | **No** | **Low** | The internal wiring (subscribe in `OnInitialized`, unsubscribe in `Dispose`) is an implementation detail. Not expected to be documented. |
| 3 | `IDisposable` implementation (via base class `Dispose(bool)`) | **No** | **Low** | Correctly unsubscribes from `ThemeService.ThemeChanged`. Implementation detail. |
| 4 | `async void` event handler pattern | **No** | **Low** | `OnThemeServiceChanged` is `async void` because it handles a C# event (not an EventCallback). This is a known pattern but can swallow exceptions. Not a doc gap but a code quality concern. |
| 5 | No DOM output (no wrapper element) | **Not explicitly** | **Low** | The component renders only `<CascadingValue>` and `ChildContent` -- no wrapping `<div>`. This means `Class`, `Style`, and `AdditionalAttributes` inherited from `MariloComponentBase` have no effect. The spec does not mention this limitation. |

---

## Recommended Changes

| # | Priority | Action | Details |
|---|---|---|---|
| 1 | **Medium** | Fix doc: `SetTheme()` → `SetThemeAsync()` | The theming overview doc shows `ThemeService.SetTheme(...)` but the actual interface method is `SetThemeAsync(...)`. Update the doc example to use `await ThemeService.SetThemeAsync(...)`. |
| 2 | **Medium** | Document the `ThemeChanged` EventCallback | Add `ThemeChanged` to the component's documented parameters. It is a useful public API for consumers who want to react to theme changes at the provider level. |
| 3 | **Medium** | Consider generating CSS custom properties | Add a `<style>` block (or a scoped stylesheet) that maps `MariloColorPalette`, `MariloTypographyScale`, and `MariloShape` values to CSS variables (e.g., `--mar-color-primary`, `--mar-radius-md`). Without this, theme tokens are only available in C# and cannot influence CSS-based styling. |
| 4 | **Medium** | Consider adding a wrapper element for RTL and dark mode | Rendering a root `<div dir="rtl" class="mar-dark">` (when applicable) would enable CSS-driven theming. Currently there is no DOM element to attach these attributes to. |
| 5 | **Low** | Consider whether `MariloComponentBase` is the correct base class | Since the component has no DOM output, `Class`, `Style`, and `AdditionalAttributes` are dead parameters. Either add a wrapper element to use them, or switch to a lighter base class. |
| 6 | **Low** | Replace `async void` with a safer pattern | The `OnThemeServiceChanged` handler is `async void`. Consider wrapping in try/catch or using an `InvokeAsync`-based approach that surfaces exceptions. |

---

## Open Questions / Ambiguities

1. **Provider-specific defaults**: The spec says "When no custom theme is supplied, MariloThemeProvider uses the provider's default theme values." In practice, `Theme` defaults to `new MariloTheme()` with hard-coded defaults in the record itself. Should providers be able to supply their own default `MariloTheme` (e.g., via `IMariloCssProvider.GetDefaultTheme()`)?

2. **Multiple providers**: Can multiple `MariloThemeProvider` instances be nested (e.g., a global provider and a section-level override)? The `CascadingValue` approach supports this naturally, but it is not documented. Should nesting be explicitly supported or discouraged?

3. **CSS variable emission**: Who is responsible for turning `MariloTheme` tokens into CSS? Is it the provider's JS interop, the theme provider component, or a separate `<link>` stylesheet? The architecture is unclear from the current docs and code.

4. **`InitializeAsync()` on `IMariloThemeService`**: The service interface has `InitializeAsync()` for loading persisted preferences. The `MariloThemeProvider` does not call this -- it only subscribes to `ThemeChanged`. Where should `InitializeAsync()` be called? Application startup? The provider's `OnInitialized`?
