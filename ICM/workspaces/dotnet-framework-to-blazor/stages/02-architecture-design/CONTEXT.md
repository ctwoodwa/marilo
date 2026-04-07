# Architecture Design

Define the target Blazor + DAB architecture, map .NET Framework patterns to modern equivalents, and select Telerik components.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../01-inventory-assessment/output/{{PROJECT_SLUG}}-solution-inventory.md` | Full file | Know what projects exist and their complexity |
| Previous stage | `../01-inventory-assessment/output/{{PROJECT_SLUG}}-dependency-map.md` | Full file | Understand shared dependencies |
| Previous stage | `../01-inventory-assessment/output/{{PROJECT_SLUG}}-api-compat.md` | Full file | Know which APIs need replacement |
| Pattern reference | `../../shared/dotnet-fx-to-blazor-patterns.md` | Full file | Map each Framework pattern to its Blazor equivalent |
| Telerik guide | `../../shared/telerik-component-map.md` | Full file | Select Telerik components for each UI pattern |
| DAB guide | `../../shared/dab-integration-guide.md` | Full file | Design the DAB configuration and entity map |

## Process

1. Read the inventory, dependency map, and API compatibility report from Stage 01.
2. Decide on the Blazor hosting model ({{BLAZOR_HOSTING_MODEL}}) and document the rationale against {{SOURCE_FX_VERSION}} characteristics.
3. Design the solution structure: Blazor project, shared component library, service layer, DAB configuration project, class libraries.
4. Design the migration strategy ({{MIGRATION_STRATEGY}}):
   - **Incremental**: YARP reverse proxy configuration, routing rules for migrated vs. legacy endpoints.
   - **Big-bang**: Complete solution scaffold, parallel development timeline.
   - **Side-by-side**: Shared session/auth strategy, traffic splitting approach.
5. Map configuration migration: `web.config` / `app.config` to `appsettings.json` + `Program.cs`.
6. Map dependency injection: replace Unity/Autofac/Ninject/manual with built-in `IServiceCollection`.
7. Design the DAB entity map: which database tables become DAB entities (CRUD) vs. EF Core DbSets (complex logic).
8. For each UI pattern found in the inventory, select the Telerik Blazor component using the component map.
9. Design authentication migration: {{CURRENT_AUTH_METHOD}} to {{TARGET_AUTH_METHOD}}.
10. Define the component hierarchy: layouts, shared components, page components.
11. Map middleware pipeline: replace HTTP modules/handlers with ASP.NET Core middleware.
12. Produce the architecture decision document.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| 4 | Migration strategy with YARP config sketch | Confirm strategy or adjust approach |
| 7 | DAB entity map and EF Core split proposal | Approve data layer split |
| 8 | Telerik component selections for each UI pattern | Approve component choices or substitute |

## Audit

| Check | Pass Condition |
|-------|---------------|
| Every project mapped | Each project from inventory has a target in the new solution |
| DAB entities defined | Every CRUD table has a DAB entity or EF Core DbSet assignment |
| State management defined | Session, ViewState, and TempData usage has a named replacement |
| Auth path documented | Authentication migration approach is explicit with named packages |
| Middleware mapped | Every HTTP module/handler has a middleware replacement |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Architecture decisions | `output/{{PROJECT_SLUG}}-architecture.md` | Sections: Hosting, Solution Structure, Migration Strategy, DAB Entity Map, Pattern Map, Telerik Selections, Auth, Middleware, DI |

<!-- Target: keep this file under 80 lines. -->
