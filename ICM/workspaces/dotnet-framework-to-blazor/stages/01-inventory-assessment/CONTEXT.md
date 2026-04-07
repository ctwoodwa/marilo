# Inventory and Assessment

Catalog all projects in the .NET Framework solution, map dependencies, identify incompatible APIs, and rate migration complexity.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Source solution | `{{SOLUTION_PATH}}` | All .sln, .csproj, .config files | The solution to catalog |
| Pattern reference | `../../shared/dotnet-fx-to-blazor-patterns.md` | "Framework API" column | Recognize patterns during inventory |

## Process

1. Parse the `.sln` file and list every project with its type (WebForms, MVC, Web API, WCF, Class Library, etc.), target framework, and output type.
2. For each project, scan `.csproj` and `packages.config` or `PackageReference` entries. Record every NuGet package with its version.
3. Run the .NET Upgrade Assistant analyzer (`upgrade-assistant analyze`) if available, and capture the output. Record API compatibility issues.
4. Identify framework-only APIs: `System.Web`, `System.ServiceModel` (WCF), `System.Web.UI` (WebForms), `HttpContext.Current`, `ConfigurationManager`, `Global.asax`, `web.config` transforms.
5. Catalog all pages/views/controllers:
   - WebForms: `.aspx`, `.aspx.cs`, `.ascx`, `.master`, `Global.asax`
   - MVC: Controllers, Views (`.cshtml`), Areas, Filters, Bundles
   - Web API: ApiControllers, route config, message handlers
   - WCF: `.svc`, service contracts, data contracts, bindings
6. Map inter-project references and shared library dependencies.
7. Identify third-party libraries needing .NET Core equivalents (e.g., log4net to Serilog, Unity/Autofac to built-in DI).
8. Classify each project/module by complexity:
   - **Simple**: class library with no framework-specific APIs, test projects (estimate: 2-4 hours)
   - **Standard**: MVC controllers with basic CRUD, Web API endpoints (estimate: 8-16 hours)
   - **Complex**: WebForms pages with ViewState, heavy server controls, custom HTTP modules (estimate: 16-40 hours)
   - **Critical**: WCF services, authentication modules, payment/integration endpoints (estimate: 40+ hours)
9. Produce the inventory artifact with totals per complexity tier and a recommended migration order (libraries first, then API, then UI, critical last).

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Solution inventory | `output/{{PROJECT_SLUG}}-solution-inventory.md` | Table with columns: Project, Type, Framework, Complexity, NuGet Count, Framework API Issues, Notes |
| Dependency map | `output/{{PROJECT_SLUG}}-dependency-map.md` | Adjacency list: project references, NuGet deps, and framework API usage |
| API compatibility report | `output/{{PROJECT_SLUG}}-api-compat.md` | Table: API, Location, Replacement, Effort |

<!-- Target: keep this file under 80 lines. -->
