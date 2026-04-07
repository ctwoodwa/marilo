# ASP Classic to Blazor Migration

Systematic conversion of ASP Classic pages to .NET Core Blazor with Telerik Blazor UI components.

## Task Routing

| Task Type | Go To | Description |
|-----------|-------|-------------|
| Catalog ASP pages | `stages/01-inventory-assessment/CONTEXT.md` | Scan source project, list pages, map dependencies, rate complexity |
| Design architecture | `stages/02-architecture-design/CONTEXT.md` | Choose hosting model, map patterns, select Telerik components |
| Migrate data layer | `stages/03-data-layer-migration/CONTEXT.md` | DAB for CRUD endpoints, EF Core for complex logic, replace ADO/COM |
| Convert pages | `stages/04-page-conversion/CONTEXT.md` | Transform ASP pages to Blazor components one at a time |
| Validate and cut over | `stages/05-integration-validation/CONTEXT.md` | Verify parity, test auth, plan deployment cutover |

## Shared Resources

| Resource | Location | Contains |
|----------|----------|----------|
| Pattern mapping | `shared/asp-to-blazor-patterns.md` | ASP Classic constructs mapped to Blazor equivalents |
| DAB reference | `shared/dab-reference.md` | DAB config syntax, entity definitions, permissions |
| Telerik guide | `shared/telerik-component-map.md` | Which Telerik component to use for each ASP UI pattern |
| Language reference | `shared/vbscript-to-csharp.md` | VBScript to C# conversion with code examples |
