# Stage 04: Theming

Implement FluentUI and Bootstrap provider styles for the new component.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| API design | `../02-api-design/output/api-design.md` | "CSS Provider Methods" section | The CSS method signatures to implement |
| Reference | `../../shared/css-naming.md` | Full file | CSS class naming conventions |
| Reference | `../../shared/file-organization.md` | "Provider Files" section | Where SCSS and provider files go |

## Process

1. Read the CSS provider method signatures from the API design
2. Implement the methods in `FluentUICssProvider.cs`:
   - Use `CssClassBuilder` to compose class strings
   - Follow `mar-[component]` prefix with BEM-like modifiers
   - Map each variant/size/state combination to CSS classes
3. Implement the same methods in `BootstrapCssProvider.cs`:
   - Map to Bootstrap-compatible class names where possible
   - Use `mar-[component]` bridge classes where Bootstrap has no equivalent
4. Create SCSS file for FluentUI provider:
   - Add `_[component].scss` in `src/Marilo.Providers.FluentUI/Styles/`
   - Import it in `marilo-fluentui.scss`
   - Define all `mar-[component]` classes with FluentUI design tokens
5. Create SCSS file for Bootstrap provider:
   - Add `_bridge-[component].scss` in `src/Marilo.Providers.Bootstrap/Styles/`
   - Import it in `marilo-bootstrap.scss`
   - Bridge Bootstrap classes to `mar-[component]` names
6. Build SCSS to verify compilation: `npm run scss:build`
7. Run the audit checks below. If any fail, fix before saving.
8. Write the theming summary listing all files created

## Audit

| Check | Pass Condition |
|-------|---------------|
| Method parity | Both providers implement identical method signatures |
| CSS class coverage | Every variant/size/state combination has a corresponding CSS rule |
| SCSS compiles | `npm run scss:build` completes without errors |
| Token usage | FluentUI styles use design tokens, not hardcoded values |
| Bridge correctness | Bootstrap bridge classes correctly map to Marilo class names |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Theming summary | `output/theming-summary.md` | List of provider files and SCSS files created, compilation status |
