# .NET Framework to Blazor Migration

Systematic upgrade of .NET Framework (4.5-4.8) projects to .NET Core Blazor with Telerik Blazor UI and Data API Builder.

## Task Routing

| Task Type | Go To | Description |
|-----------|-------|-------------|
| Catalog projects and deps | `stages/01-inventory-assessment/CONTEXT.md` | Scan solution, list projects, map NuGet/framework deps, rate complexity |
| Design architecture | `stages/02-architecture-design/CONTEXT.md` | Choose hosting model, design DAB config, map patterns, select Telerik components |
| Migrate data layer | `stages/03-data-layer-migration/CONTEXT.md` | Replace EF6/ADO.NET with DAB + EF Core, build service layer |
| Convert UI | `stages/04-ui-conversion/CONTEXT.md` | Transform WebForms/MVC views to Blazor components with Telerik |
| Validate and cut over | `stages/05-integration-validation/CONTEXT.md` | Verify parity, test auth, YARP incremental cutover, deployment plan |

## Shared Resources

| Resource | Location | Contains |
|----------|----------|----------|
| Pattern mapping | `shared/dotnet-fx-to-blazor-patterns.md` | .NET Framework constructs mapped to Blazor/.NET Core equivalents |
| Telerik guide | `shared/telerik-component-map.md` | Which Telerik component to use for each UI pattern |
| DAB guide | `shared/dab-integration-guide.md` | Data API Builder setup, entity mapping, and Blazor integration |
| WebForms controls | `shared/webforms-controls-map.md` | WebForms server controls mapped to Blazor/Telerik equivalents |
