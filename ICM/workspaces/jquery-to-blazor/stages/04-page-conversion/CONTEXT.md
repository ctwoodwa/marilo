# Page Conversion

Convert jQuery pages to Blazor components with Telerik UI, following the conversion blueprint and the target project's established patterns.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Conversion blueprint | `../03-component-mapping/output/{{PROJECT_SLUG}}-conversion-blueprint.md` | Full file | The line-by-line mapping to follow |
| Codebase patterns | `../02-codebase-alignment/output/{{PROJECT_SLUG}}-codebase-patterns.md` | Full file | Naming, styling, and service patterns to match |
| Source page | `{{JQUERY_SOURCE_PATH}}` | Full file | The original jQuery page as reference |
| Target project | `{{BLAZOR_PROJECT_PATH}}` | Existing pages for pattern reference | Ensure the new page fits the project |
| jQuery patterns | `../../shared/jquery-to-blazor-patterns.md` | As needed during conversion | Quick reference for syntax mapping |
| Telerik guide | `../../shared/telerik-component-map.md` | As needed during conversion | Component API reference |

## Process

1. Create the page's Blazor component file (.razor) in the correct project location per the blueprint. Add the `@page` directive following the project's URL convention.
2. Add all `@inject` directives for required services, following the project's DI pattern (from Stage 02 output).
3. Build the component markup top-down, replacing jQuery UI elements with their mapped Telerik components per the blueprint. For each element:
   - Replace jQuery DataTable/grid with `TelerikGrid` including columns, sorting, paging, filtering per blueprint
   - Replace jQuery UI dialogs/modals with `TelerikDialog` or `TelerikWindow`
   - Replace jQuery UI date pickers with `TelerikDatePicker` or `TelerikDateTimePicker`
   - Replace jQuery dropdowns/select2 with `TelerikDropDownList` or `TelerikComboBox`
   - Replace jQuery tabs with `TelerikTabStrip`
   - Replace jQuery accordion with `TelerikPanelBar`
   - Replace jQuery tooltips with `TelerikTooltip`
   - Replace jQuery validation with DataAnnotations + `TelerikForm` validation
   - Replace jQuery notification/toast with `TelerikNotification`
4. Build the `@code` block:
   - Define component state fields for all data that was managed via DOM manipulation
   - Implement lifecycle methods (`OnInitializedAsync`, `OnParametersSetAsync`) for data loading that was done via `$(document).ready()` or AJAX on page load
   - Implement event handler methods for each jQuery event handler, using the service calls defined in the blueprint
   - Replace `$.ajax` success/error callbacks with `try/catch` around `await` calls
   - Replace DOM show/hide with boolean state fields and `@if` conditional rendering
   - Replace class toggling with computed CSS class strings or ternary expressions
5. Create the code-behind file (.razor.cs) if the project uses code-behind separation.
6. Create or update service classes for any data access that requires a new service method not already in the project.
7. Create model/DTO classes for request and response shapes identified in the blueprint.
8. Add any required JSInterop JavaScript files and C# wrapper services for bridge items.
9. Register any new services in `Program.cs` following the project's registration pattern.
10. Add scoped CSS (.razor.css) if the project uses scoped styles, or add styles to the project's global stylesheet following its convention.
11. Run the page and verify rendering matches the original jQuery page layout and behavior.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| 4 | Complete .razor file markup and code block | Review component structure before creating supporting files |

## Audit

| Check | Pass Condition |
|-------|---------------|
| Blueprint coverage | Every item in the conversion blueprint has been implemented |
| Pattern compliance | New code follows the project's naming, DI, and data access conventions from Stage 02 |
| No jQuery references | The converted page contains zero jQuery, `$()`, or legacy script references |
| No direct DOM manipulation | No `IJSRuntime` calls that manipulate DOM outside of documented JSInterop bridge items |
| Telerik components used | UI elements use Telerik components per the blueprint, not hand-rolled HTML equivalents |
| Forms validated | Every form field has the validation rules from the original page |
| Services registered | All new services are registered in Program.cs |
| Compilation clean | The page compiles without errors or warnings |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Converted page components | `output/{{PROJECT_SLUG}}-converted-pages/` | .razor, .razor.cs, .razor.css files |
| New service classes | `output/{{PROJECT_SLUG}}-services/` | .cs service files |
| New model classes | `output/{{PROJECT_SLUG}}-models/` | .cs model/DTO files |
| JSInterop files (if any) | `output/{{PROJECT_SLUG}}-jsinterop/` | .js and .cs interop wrapper files |
| Program.cs additions | `output/{{PROJECT_SLUG}}-registration.md` | Markdown listing service registrations to add |
