---
uid: troubleshooting-general
title: General Issues
description: Solutions to common issues when using Marilo components.
---

# General Issues

This article covers the most frequently reported setup and runtime problems with Marilo components and their solutions.

## Components Do Not Render

**Symptoms:** The page loads but Marilo components show no output, render as empty elements, or throw a `InvalidOperationException` during initialization.

**Causes and solutions:**

1. **Missing service registration.** Ensure `AddMarilo().UseFluentUI()` (or your chosen provider) is called in `Program.cs`:

   ```csharp
   builder.Services.AddMarilo().UseFluentUI();
   ```

   Without this, required services (`IMariloCssProvider`, `IIconProvider`, `ThemeService`) are not in the DI container and components will throw on first render.

2. **Missing provider CSS link.** Add the provider stylesheet to `App.razor`:

   ```html
   <link rel="stylesheet" href="_content/Marilo.Providers.FluentUI/css/marilo-fluentui.css" />
   ```

   Without this, components render structurally but with no styling.

3. **Missing `MariloThemeProvider` wrapper.** All Marilo components must be descendants of a `MariloThemeProvider`. Add it to `MainLayout.razor`:

   ```razor
   <MariloThemeProvider>
       @Body
   </MariloThemeProvider>
   ```

   Components outside this wrapper cannot resolve the CSS custom property token block and will appear unstyled or throw when accessing theme services.

## Styles Look Wrong

**Symptoms:** Components render but the visual appearance is incorrect — colors are off, spacing is wrong, or components look like unstyled HTML.

**Causes and solutions:**

1. **Wrong provider CSS loaded.** Verify that the `<link>` href matches your registered provider. If you called `UseBootstrap()` but linked the FluentUI CSS (or vice versa), class names will not match the expected selectors.

2. **SCSS not compiled.** If you have modified any `.scss` source files, the compiled `.css` files must be regenerated:

   ```bash
   npm run scss:build
   ```

   The compiled CSS files are static assets checked into the repository — they are not auto-compiled on `dotnet build`. Style changes are invisible until the SCSS is rebuilt.

3. **Conflicting CSS.** An existing global stylesheet may override Marilo's `--marilo-*` CSS custom properties or its component class rules. Open browser DevTools, inspect the affected component, and look for overridden properties. Add a higher-specificity rule or adjust the conflicting stylesheet.

4. **Stylesheet load order.** The Marilo provider stylesheet must load after any CSS reset or global stylesheet that sets baseline properties. Ensure the `<link>` for Marilo CSS appears after other stylesheets in `App.razor`.

## Dark Mode Does Not Work

**Symptoms:** Toggling dark mode has no effect; the component always displays in light mode.

**Causes and solutions:**

1. **`data-marilo-theme` attribute not set.** Dark mode is activated by `MariloThemeProvider` setting `data-marilo-theme="dark"` on its root element. If `ThemeService.IsDarkMode` is `true` but the attribute is not present, the theme provider may not have rendered or may have been disposed. Inspect the DOM to confirm the attribute exists.

2. **Missing dark token definitions.** If you are using a custom provider or have overridden the CSS file, ensure that the `[data-marilo-theme="dark"]` block is present in the stylesheet with overrides for all `--marilo-*` color tokens. Without this block, tokens fall back to their light-mode values.

3. **`SetDarkModeAsync` vs `ToggleDarkModeAsync`.** When restoring dark mode from user preference (e.g., from `localStorage`), always use `SetDarkModeAsync(bool)` — the idempotent setter. Never use `ToggleDarkModeAsync()` for restoration; it flips the current state and may inadvertently switch back to light mode if the service was already initialized.

## Provider Switching Does Not Take Effect

**Symptoms:** Changing the provider in `ProviderSwitcher` does not update the page appearance, or the page appears with mixed styles.

**Causes and solutions:**

1. **`localStorage` not cleared.** The provider preference is stored in `localStorage`. If you previously set a provider and changed it in code without updating the stored value, the old value is restored on page load. Clear `localStorage` in DevTools or call the `ProviderSwitcher` API to reset it.

2. **Page not reloaded.** Switching providers requires a full page reload (`location.reload()`) because the provider stylesheet is loaded via a `<link>` tag in `App.razor`. This is intentional — swapping stylesheets without a reload can leave stale CSS custom property values. Verify that the `ProviderSwitcher` service calls `location.reload()` after updating the stored preference.

3. **`ProviderSwitcher` not registered.** If you are using a custom provider-switcher implementation, ensure it is registered in DI and that all `IMariloCssProvider` interface methods are delegated. Missing method implementations will cause a build error. See `samples/Marilo.Demo/Services/ProviderSwitcher.cs` for the reference implementation.

## Build Errors After Adding Material Provider

**Symptoms:** Adding `Marilo.Providers.Material` causes build errors such as `The type or namespace 'Material' does not exist` or `No service for type 'IMariloCssProvider' has been registered`.

**Causes and solutions:**

1. **Missing project or package reference.** Add the NuGet package:

   ```bash
   dotnet add package Marilo.Providers.Material
   ```

   If working in a solution with project references instead of NuGet, add the project reference:

   ```bash
   dotnet add reference ../Marilo.Providers.Material/Marilo.Providers.Material.csproj
   ```

2. **Missing DI registration.** Call `UseMaterial()` in `Program.cs`:

   ```csharp
   builder.Services.AddMarilo().UseMaterial();
   ```

   Without this call, `IMariloCssProvider` is not registered and components will throw at runtime.

3. **Missing CSS link.** Add the Material stylesheet to `App.razor`:

   ```html
   <link rel="stylesheet" href="_content/Marilo.Providers.Material/css/marilo-material.css" />
   ```

4. **Duplicate provider registration.** Only one provider can be active at a time. If `UseFluentUI()` and `UseMaterial()` are both called, the last registration wins. Remove the unused registration.
