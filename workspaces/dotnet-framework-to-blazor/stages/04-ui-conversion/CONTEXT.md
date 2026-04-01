# UI Conversion

Convert .NET Framework pages, views, and controls to Blazor components, applying Telerik Blazor UI components and DAB-backed services per the architecture plan.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Architecture | `../02-architecture-design/output/{{PROJECT_SLUG}}-architecture.md` | Full file | Component hierarchy, Telerik selections, state management |
| Data plan | `../03-data-layer-migration/output/{{PROJECT_SLUG}}-data-plan.md` | "Service Layer" section | Know which services to inject |
| Pattern reference | `../../shared/dotnet-fx-to-blazor-patterns.md` | Full file | Look up each Framework pattern during conversion |
| Telerik guide | `../../shared/telerik-component-map.md` | Full file | Apply correct Telerik components |
| WebForms map | `../../shared/webforms-controls-map.md` | Full file | Map WebForms server controls to Blazor equivalents |
| Source page | `{{SOLUTION_PATH}}/[page-or-view]` | Full file | The page being converted |

## Process

1. Read the architecture document and data migration plan from previous stages.
2. Select the next page/view to convert from the inventory, following the recommended order.
3. Read the source completely. Identify:
   - **WebForms**: `.aspx` markup, code-behind `.aspx.cs`, server controls, ViewState, postbacks, master pages, user controls.
   - **MVC**: Controller actions, Razor views `.cshtml`, partial views, HTML helpers, model binding, `@Html.*` helpers, bundled JS/CSS.
   - **Web API**: ApiController methods, route attributes, media formatters, model binding.
4. Create the Blazor component (.razor file):
   - WebForms: replace server controls with Telerik components, code-behind becomes `@code { }`, master pages become layouts.
   - MVC views: convert `@Html.*` helpers to Razor component syntax, replace `@model` with `[Parameter]` or injected services.
   - Convert `@ViewBag`/`@ViewData`/`TempData` to component parameters or cascading values.
5. Apply Telerik components per the component map: GridView/table to TelerikGrid, form inputs to TelerikForm fields, dropdowns to TelerikDropDownList, etc.
6. Replace data access: inject DAB API service or EF Core service from the data plan instead of direct DB calls.
7. Replace state management:
   - ViewState to component state or `ProtectedSessionStorage`.
   - `Session` to scoped services or `ProtectedSessionStorage`.
   - `TempData` to cascading parameters or navigation state.
8. Replace navigation: `Response.Redirect` / `RedirectToAction` to `NavigationManager.NavigateTo()`.
9. Migrate client-side: replace jQuery/JS with Blazor interop (`IJSRuntime`) or Telerik built-in features.
10. Update the conversion checklist with the page status.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| 5 | Rendered component with Telerik selections highlighted | Confirm UI component choices |

## Audit

| Check | Pass Condition |
|-------|---------------|
| No Framework markup | Zero `<asp:*>` controls, `runat="server"`, or `@Html.*` helpers in output |
| No direct DB access | All data access goes through injected DAB or EF Core services |
| Telerik components used | Every data grid, form, and dropdown uses Telerik, not raw HTML |
| No ViewState | No ViewState references; state managed via component state or services |
| Navigation correct | All redirects use NavigationManager, not Response or RedirectToAction |
| No bundled JS | jQuery/JS dependencies replaced with Blazor interop or Telerik features |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Conversion checklist | `output/{{PROJECT_SLUG}}-conversion-checklist.md` | Table: Page/View, Source Type, Status, Blazor Component Path, Telerik Components Used, Notes |

<!-- Target: keep this file under 80 lines. -->
