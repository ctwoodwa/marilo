# jQuery to Blazor Migration

Systematic conversion of jQuery-based pages to .NET Core Blazor with Telerik Blazor UI components within an existing Blazor project.

## Task Routing

| Task Type | Go To | Description |
|-----------|-------|-------------|
| Catalog jQuery features | `stages/01-feature-inventory/CONTEXT.md` | Scan source page, list jQuery usage, map plugins, rate complexity |
| Analyze target project | `stages/02-codebase-alignment/CONTEXT.md` | Read existing Blazor project patterns, services, conventions to follow |
| Map to Telerik components | `stages/03-component-mapping/CONTEXT.md` | Map each jQuery feature to its Telerik Blazor or native Blazor equivalent |
| Convert pages | `stages/04-page-conversion/CONTEXT.md` | Transform jQuery pages to Blazor components one at a time |
| Validate and cut over | `stages/05-integration-validation/CONTEXT.md` | Verify parity, test integration, plan deployment cutover |

## Shared Resources

| Resource | Location | Contains |
|----------|----------|----------|
| Pattern mapping | `shared/jquery-to-blazor-patterns.md` | jQuery constructs mapped to Blazor equivalents |
| Plugin mapping | `shared/jquery-plugin-map.md` | Common jQuery plugins mapped to Telerik Blazor components |
| Telerik guide | `shared/telerik-component-map.md` | Which Telerik component to use for each UI pattern |
| JSInterop bridge | `shared/js-interop-bridge.md` | Patterns for when JSInterop is the only viable path |
