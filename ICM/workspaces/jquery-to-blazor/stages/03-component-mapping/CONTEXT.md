# Component Mapping

Map every cataloged jQuery feature to its Telerik Blazor or native Blazor equivalent. This produces the conversion blueprint that Stage 04 follows line by line.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Feature inventory | `../01-feature-inventory/output/{{PROJECT_SLUG}}-feature-inventory.md` | Full file | Every jQuery feature that needs a Blazor equivalent |
| Codebase patterns | `../02-codebase-alignment/output/{{PROJECT_SLUG}}-codebase-patterns.md` | Full file | Conventions the mapped components must follow |
| jQuery patterns | `../../shared/jquery-to-blazor-patterns.md` | Full file | jQuery construct to Blazor equivalents |
| Plugin map | `../../shared/jquery-plugin-map.md` | Full file | Known jQuery plugin to Telerik mappings |
| Telerik guide | `../../shared/telerik-component-map.md` | Full file | Telerik component selection reference |
| JSInterop bridge | `../../shared/js-interop-bridge.md` | Full file | Last-resort JSInterop patterns |

## Process

1. For each jQuery plugin in the inventory, find the Telerik Blazor equivalent using `shared/jquery-plugin-map.md`. If no direct equivalent exists, identify the closest Telerik component or native Blazor approach.
2. For each AJAX call in the inventory, map to the target project's data access pattern (from Stage 02 output). Define: which service/HttpClient to use, request/response model types, error handling approach.
3. For each DOM event handler, map to the Blazor event equivalent: `@onclick`, `@onchange`, `@oninput`, `@bind`, EventCallback, or component event parameter. Specify the exact Blazor syntax.
4. For each DOM manipulation pattern (show/hide, class toggle, element creation), map to the Blazor state-driven equivalent: conditional rendering (`@if`), class binding, `RenderFragment`, or component parameter.
5. For each form and validation rule, map to the target project's form pattern: model property with DataAnnotation, TelerikForm field configuration, custom validation component.
6. For CSS/class manipulation, map to the project's styling approach: utility class usage, CSS variable binding, or scoped CSS.
7. Identify any features that require JSInterop as a bridge. For each, document: what JavaScript is needed, the C# interop wrapper, and a plan to eventually replace with a native solution.
8. For each mapped feature, assign the target file location in the Blazor project: which .razor file, which service class, which model, which CSS file.
9. Produce the conversion blueprint as a structured document.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| 7 | Complete mapping table with any JSInterop items flagged | Approve JSInterop items or request alternative approaches |

## Audit

| Check | Pass Condition |
|-------|---------------|
| Every inventory item mapped | No feature from Stage 01 output is left unmapped |
| Telerik preferred | Every UI feature uses a Telerik component unless no suitable one exists, with rationale documented |
| JSInterop minimized | JSInterop items have documented rationale and a future native replacement plan |
| File locations assigned | Every mapped feature has a target file path in the Blazor project |
| Data access aligned | All AJAX replacements use the established project data access pattern |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Conversion blueprint | `output/{{PROJECT_SLUG}}-conversion-blueprint.md` | Markdown with tables: feature-to-component mapping, AJAX-to-service mapping, event mapping, DOM-to-state mapping, form mapping, JSInterop items, file location assignments |
