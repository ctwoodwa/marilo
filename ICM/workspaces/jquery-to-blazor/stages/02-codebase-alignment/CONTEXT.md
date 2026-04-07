# Codebase Alignment

Analyze the existing target Blazor project to extract patterns, conventions, and service structures that the converted page must follow. The converted page must look and behave like it was always part of the project.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Feature inventory | `../01-feature-inventory/output/{{PROJECT_SLUG}}-feature-inventory.md` | Full file | Know what jQuery features need Blazor equivalents |
| Target project | `{{BLAZOR_PROJECT_PATH}}` | Project structure, Program.cs, MainLayout, existing pages | Extract conventions to follow |
| Telerik patterns | `../../shared/telerik-component-map.md` | Full file | Understand which Telerik components the project uses |

## Process

1. Read the target Blazor project's `Program.cs` (or `Startup.cs`). Document: service registration patterns, HttpClient configuration, authentication setup, middleware pipeline order.
2. Read `MainLayout.razor` and any shared layout files. Document: navigation structure, shell components (top bar, sidebar, breadcrumb), Telerik root component placement, theme CSS loading order.
3. Scan existing Blazor pages (.razor files) for established patterns. For each, note: page directive style (`@page`), dependency injection approach (`@inject` vs `[Inject]`), lifecycle method usage, state management approach.
4. Identify the project's data access pattern: How do existing pages call APIs? Named HttpClient, typed HttpClient, service abstraction, DAB client, or direct EF Core? Document the pattern with a concrete example from the codebase.
5. Identify the project's form handling pattern: EditForm vs TelerikForm, validation approach (DataAnnotations, FluentValidation, custom), error display pattern.
6. Identify the project's notification/feedback pattern: How do existing pages show success messages, errors, loading states? TelerikNotification, TelerikLoader, custom toast, or other.
7. Identify the project's dialog/modal pattern: TelerikDialog, TelerikWindow, custom modal component, or other.
8. Document the project's CSS/styling approach: SCSS tokens, Telerik theme overrides, utility classes, scoped CSS (`.razor.css`), or global styles. Note file locations.
9. Document naming conventions: file naming, component naming, service naming, model/DTO naming, folder structure for pages vs components vs services.
10. Produce a "Patterns to Follow" guide that the conversion stage will use as its primary reference.

## Audit

| Check | Pass Condition |
|-------|---------------|
| Service registration documented | At least one concrete example of how services are registered |
| Data access pattern documented | At least one concrete code example from an existing page |
| Form pattern documented | The validation and submit approach is documented with an example |
| Naming conventions extracted | File, component, service, and model naming patterns are listed |
| Layout structure mapped | MainLayout, nav, and shell component structure is documented |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Codebase patterns guide | `output/{{PROJECT_SLUG}}-codebase-patterns.md` | Markdown with sections: service registration, data access, forms, notifications, dialogs, styling, naming, layout. Each section has concrete code examples from the target project. |
