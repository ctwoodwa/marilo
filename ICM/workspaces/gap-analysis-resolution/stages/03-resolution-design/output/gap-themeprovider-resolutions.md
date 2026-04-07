# Resolution Records: MariloThemeProvider

## Summary

MariloThemeProvider has 8 gaps across API naming, missing DOM output, CSS variable generation, dark mode, RTL, and code quality. These gaps are tightly interrelated — most are solved by adding a wrapper element that emits CSS custom properties, `data-marilo-theme`, and `dir` attributes.

---

### RES-THEME-001: Add wrapper element with CSS variables, dark mode, and RTL support

**Resolves:** GAP-4 (SetTheme sync/async), GAP-6 (RTL), GAP-7 (CSS variable generation), GAP-8 (dark mode toggle), Code→Spec GAP-5 (no DOM output)
**Status:** Approved

#### Target Pattern

MariloThemeProvider renders a wrapper `<div>` that:
1. Cascades `MariloTheme` via `<CascadingValue>` (existing)
2. Applies `data-marilo-theme="dark"` when `ThemeService.IsDarkMode` is true
3. Applies `dir="rtl"` when `Theme.IsRtl` is true
4. Generates CSS custom properties from `MariloColorPalette`, `MariloTypographyScale`, and `MariloShape` as inline `style` on the wrapper
5. Passes `Class`, `Style`, and `AdditionalAttributes` through to the wrapper div
6. Adds `marilo-theme-provider` CSS class for identification

```razor
<CascadingValue Value="@Theme">
    <div class="@CombineClasses("marilo-theme-provider")"
         style="@GenerateThemeStyles()"
         dir="@(Theme.IsRtl ? "rtl" : null)"
         data-marilo-theme="@(ThemeService.IsDarkMode ? "dark" : "light")"
         @attributes="AdditionalAttributes">
        @ChildContent
    </div>
</CascadingValue>
```

#### Options Considered

**Option A: Inline styles on wrapper div**
- Approach: Generate `--marilo-color-*` etc. as inline style properties on the wrapper div
- Pros: No JS interop, scoped to provider subtree, supports nesting, simple implementation
- Cons: Long style attribute, overrides only within the div scope (which is correct behavior)
- Effort: Small

**Option B: Inject `<style>` element targeting `:root`**
- Approach: Render a `<style>` block with `:root { --marilo-color-primary: ... }`
- Pros: Global scope, matches how SCSS providers work
- Cons: Multiple providers would conflict, harder to scope, style element in body is non-standard
- Effort: Small

**Option C: JS interop to set CSS variables on document root**
- Approach: Call `document.documentElement.style.setProperty()` via IJSRuntime
- Pros: True `:root` scope, survives component re-renders
- Cons: Requires JS interop, SSR/prerender issues, harder to test, can't scope to nested providers
- Effort: Medium

#### Decision

**Chosen:** Option A
**Rationale:** Inline styles on the wrapper div are the simplest approach, require no JS interop, work with SSR/prerendering, and naturally support nested providers (a section-level theme override scopes correctly). Provider SCSS sets the base values on `:root`; the wrapper div's inline styles override them within the component subtree when runtime theme changes occur.

#### Consequences

- All components inside `<MariloThemeProvider>` will have a wrapping `<div>` in the DOM (new DOM element)
- `Class` and `Style` parameters on MariloThemeProvider now have effect
- CSS variables from the C# theme tokens will override provider SCSS defaults at runtime
- Dark mode attribute `[data-marilo-theme="dark"]` matches the selector already used in `_tokens-dark.scss`
- Nested `MariloThemeProvider` instances can override theme within a subtree

#### Success Criteria

- [ ] Wrapper div renders with `class="marilo-theme-provider"` (unit test)
- [ ] CSS custom properties for colors, typography, and shape are emitted as inline styles (unit test)
- [ ] `data-marilo-theme="dark"` is set when `ThemeService.IsDarkMode` is true (unit test)
- [ ] `dir="rtl"` is set when `Theme.IsRtl` is true (unit test)
- [ ] `Class`, `Style`, and `AdditionalAttributes` pass through to the wrapper div (unit test)

<!-- Updated by test-coverage-pass: criteria made testable -->

---

### RES-THEME-002: Fix async void handler and call InitializeAsync

**Resolves:** Code→Spec GAP-4 (async void), Open Question 4 (InitializeAsync)
**Status:** Approved

#### Target Pattern

Replace `async void OnThemeServiceChanged` with a safe `InvokeAsync`-wrapped handler that catches exceptions. Call `ThemeService.InitializeAsync()` in `OnAfterRenderAsync(firstRender)` to load persisted preferences (dark mode state from localStorage).

```csharp
protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (firstRender)
    {
        await ThemeService.InitializeAsync();
    }
}

private async void OnThemeServiceChanged(object? sender, ThemeChangedEventArgs e)
{
    try
    {
        Theme = e.NewTheme;
        await ThemeChanged.InvokeAsync(e);
        await InvokeAsync(StateHasChanged);
    }
    catch (Exception ex) when (ex is ObjectDisposedException or TaskCanceledException)
    {
        // Component disposed during async operation — safe to ignore
    }
}
```

#### Options Considered

**Option A: try/catch in async void**
- Approach: Wrap the handler body in try/catch for disposal races
- Pros: Minimal change, standard Blazor pattern for C# events
- Cons: Still `async void` (can't be awaited from tests)
- Effort: Trivial

**Option B: Rewrite as IDisposable + CancellationToken**
- Approach: Use a CancellationTokenSource, cancel on dispose
- Pros: More testable, proper cancellation
- Cons: Overengineered for this simple handler, C# events require void return
- Effort: Small

#### Decision

**Chosen:** Option A
**Rationale:** The `async void` is required by the C# event delegate signature. Adding try/catch for disposal races is the standard Blazor pattern. `InitializeAsync()` is called in `OnAfterRenderAsync` because it uses `IJSRuntime` (localStorage), which is only available after the first render.

#### Consequences

- Dark mode state will be loaded from localStorage on first render
- Component may re-render after initialization if persisted dark mode differs from default
- Exception swallowing is limited to expected disposal races

#### Success Criteria

- [ ] `InitializeAsync()` is called on first render (unit test)
- [ ] Dark mode state persisted in localStorage is restored on page load (integration test)
- [ ] No unobserved task exceptions on component disposal (unit test)

<!-- Updated by test-coverage-pass: criteria made testable -->

---

### RES-THEME-003: Document ThemeChanged EventCallback

**Resolves:** Code→Spec GAP-1 (undocumented ThemeChanged)
**Status:** Approved

#### Target Pattern

The `ThemeChanged` parameter is a legitimate public API. It should remain as-is in the code. Documentation updates are out of scope for code resolution but flagged for doc generation.

#### Decision

No code change needed. The parameter exists and works correctly. Flag for documentation pass.

#### Success Criteria

- [ ] `ThemeChanged` parameter remains in the component (unit test)
- [ ] Flagged in resolution notes for documentation update (unit test)

<!-- Updated by test-coverage-pass: criteria made testable -->
