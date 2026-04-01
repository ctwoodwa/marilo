# Page-by-Page Conversion

Convert ASP Classic pages to Blazor components one at a time, applying Telerik Blazor UI components per the architecture plan.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Architecture | `../02-architecture-design/output/{{PROJECT_SLUG}}-architecture.md` | Full file | Component hierarchy, Telerik selections, state management |
| Data plan | `../03-data-layer-migration/output/{{PROJECT_SLUG}}-data-plan.md` | "DAB Entities" and "Service Layer" sections | Know which endpoints use DAB vs custom API |
| DAB config | `../03-data-layer-migration/output/{{PROJECT_SLUG}}-dab-config.json` | Full file | DAB entity paths for HttpClient calls |
| Pattern reference | `../../shared/asp-to-blazor-patterns.md` | Full file | Look up each ASP pattern during conversion |
| Telerik guide | `../../shared/telerik-component-map.md` | Full file | Apply correct Telerik components |
| Language reference | `../../shared/vbscript-to-csharp.md` | Full file | Convert VBScript logic to C# |
| Source page | `{{ASP_SOURCE_PATH}}/[page].asp` | Full file | The ASP page being converted |

## Process

1. Read the architecture document and data migration plan from previous stages.
2. Select the next page to convert from the inventory, following the recommended order (simple first).
3. Read the ASP source page completely. Identify: HTML structure, VBScript logic, includes, SQL, Session usage, Response.Redirect calls.
4. Create the Blazor component (.razor file):
   - Convert HTML structure to Razor markup.
   - Replace `<% %>` blocks with `@code { }`.
   - Replace `<%= %>` expressions with `@expression`.
   - Replace `#include` references with `<SharedComponent />` or `@inject` services.
5. Apply Telerik components: replace HTML tables with TelerikGrid, form inputs with TelerikForm fields, dropdowns with TelerikDropDownList, etc. For data grids, use TelerikGrid OnRead to call DAB REST endpoints for server-side paging, sorting, and filtering.
6. Convert VBScript business logic to C# methods using the language reference.
7. Replace data access: for CRUD operations, call DAB endpoints via HttpClient (GET, POST, PATCH, DELETE). For complex logic, inject the appropriate service from the data plan. Do not call the custom API for operations DAB already handles.
8. Replace Session/Application usage with the state management approach from the architecture.
9. Replace Response.Redirect with NavigationManager.NavigateTo().
10. Update the conversion checklist with the page status.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| 5 | Rendered component with Telerik selections highlighted | Confirm UI component choices |

## Audit

| Check | Pass Condition |
|-------|---------------|
| No VBScript remains | Zero `<%` blocks or VBScript keywords in the output |
| No inline SQL | All data access goes through DAB endpoints or injected services |
| Telerik components used | Every data grid, form, and dropdown uses Telerik, not raw HTML |
| Parameterized queries | No string-concatenated SQL anywhere in the component |
| Navigation correct | All redirects use NavigationManager, not Response objects |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Conversion checklist | `output/{{PROJECT_SLUG}}-conversion-checklist.md` | Table: Page, Status, Blazor Component Path, Telerik Components Used, Notes |

<!-- Target: keep this file under 80 lines. -->
