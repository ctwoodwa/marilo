# .NET Framework to Blazor Pattern Mapping

Quick-reference for converting .NET Framework constructs to their Blazor/.NET Core equivalents. Use during Stage 02 (architecture) and Stage 04 (UI conversion).

## Project and Configuration

| .NET Framework | .NET Core / Blazor | Notes |
|----------------|-------------------|-------|
| `web.config` app settings | `appsettings.json` + `IConfiguration` | Use `builder.Configuration["Key"]` |
| `web.config` connection strings | `appsettings.json` `ConnectionStrings` section | Use `builder.Configuration.GetConnectionString("Name")` |
| `web.config` transforms (Debug/Release) | `appsettings.{Environment}.json` | Environment-based file loading |
| `Global.asax` `Application_Start` | `Program.cs` service registration | All DI and middleware in `Program.cs` |
| `Global.asax` `Application_Error` | Exception-handling middleware | `app.UseExceptionHandler("/Error")` |
| `Global.asax` `Session_Start` | Middleware or circuit handler | Blazor Server circuit events |
| `packages.config` | `<PackageReference>` in `.csproj` | SDK-style project format |
| `AssemblyInfo.cs` attributes | `.csproj` properties | `<Version>`, `<Authors>`, etc. |

## Dependency Injection

| .NET Framework | .NET Core / Blazor | Notes |
|----------------|-------------------|-------|
| Unity container | `builder.Services` (built-in DI) | `AddScoped`, `AddTransient`, `AddSingleton` |
| Autofac `ContainerBuilder` | `builder.Services` or Autofac integration | Built-in DI preferred; Autofac available via `Autofac.Extensions.DependencyInjection` |
| Ninject kernel | `builder.Services` | Direct replacement |
| `DependencyResolver.SetResolver()` | Built-in DI, no resolver needed | Constructor injection everywhere |
| Manual `new Service()` | `@inject IService` in Razor, constructor injection in classes | Register in `Program.cs` |

## HTTP Pipeline

| .NET Framework | .NET Core / Blazor | Notes |
|----------------|-------------------|-------|
| HTTP modules (`IHttpModule`) | Middleware (`app.Use(...)`) | Order matters; register in `Program.cs` |
| HTTP handlers (`IHttpHandler`) | Minimal API or controller endpoint | `app.MapGet("/path", handler)` |
| `FilterConfig` / action filters | Middleware or `IEndpointFilter` | Or use `[ServiceFilter]` attribute |
| `BundleConfig` (JS/CSS bundles) | Vite, Webpack, or `<link>`/`<script>` in `_Host.cshtml` | Telerik provides its own CSS/JS |
| `RouteConfig.RegisterRoutes()` | `@page "/route"` directive on Blazor components | Attribute routing |
| OWIN middleware | ASP.NET Core middleware | Direct port with minor API changes |

## State and Session

| .NET Framework | .NET Core / Blazor | Notes |
|----------------|-------------------|-------|
| `Session["key"]` | `ProtectedSessionStorage` or scoped service | Blazor Server only for session storage |
| `ViewState["key"]` | Component state (`@code { }` fields) | No ViewState in Blazor |
| `TempData["key"]` | `NavigationManager` query params or cascading parameter | No TempData in Blazor |
| `ViewBag.Property` | `[Parameter]` or `[CascadingParameter]` | Component parameters |
| `ViewData["key"]` | `[Parameter]` or injected service | Explicit data flow |
| `Application["key"]` | Singleton service via DI | Register in `Program.cs` |
| `HttpContext.Current` | `IHttpContextAccessor` (avoid in Blazor) | Prefer Blazor-native alternatives |
| `HttpContext.Current.User` | `AuthenticationStateProvider` | `[CascadingParameter] Task<AuthenticationState>` |

## Request and Response

| .NET Framework | .NET Core / Blazor | Notes |
|----------------|-------------------|-------|
| `Request.Form["field"]` | `@bind` or `EditContext` | Two-way binding |
| `Request.QueryString["param"]` | `[Parameter] [SupplyParameterFromQuery]` | Route or query parameter |
| `Request.Cookies["name"]` | `IJSRuntime` cookie access or middleware | No direct cookie API in Blazor components |
| `Response.Redirect("url")` | `NavigationManager.NavigateTo("url")` | Client-side navigation |
| `RedirectToAction("action")` | `NavigationManager.NavigateTo("/route")` | Named routes become URL paths |
| `Server.MapPath("~/path")` | `IWebHostEnvironment.ContentRootPath` | Inject via DI |
| `Server.HtmlEncode(val)` | Automatic in Razor | Use `@((MarkupString)val)` for raw HTML |

## Data Access

| .NET Framework | .NET Core / Blazor | Notes |
|----------------|-------------------|-------|
| EF6 `DbContext` | EF Core `DbContext` or DAB REST endpoint | Migrate Fluent API configs |
| EF6 `Database.SetInitializer` | EF Core migrations (`dotnet ef migrations`) | Code-first migrations |
| EF6 `EntityTypeConfiguration<T>` | EF Core `IEntityTypeConfiguration<T>` | Similar API, different namespace |
| `ObjectContext` (EF4) | EF Core `DbContext` | Full rewrite needed |
| LINQ to SQL `DataContext` | EF Core `DbContext` or DAB | Full rewrite needed |
| `SqlConnection` / `SqlCommand` (ADO.NET) | DAB HTTP endpoint or EF Core | Prefer DAB for CRUD |
| `SqlDataAdapter` / `DataSet` / `DataTable` | Strongly-typed models via DAB or EF Core | No DataSets in modern .NET |
| Stored procedures (EF6) | DAB stored-proc entity or EF Core `FromSqlRaw` | DAB supports stored procs as entities |
| `ConfigurationManager.ConnectionStrings` | `IConfiguration.GetConnectionString()` | `appsettings.json` source |

## Authentication

| .NET Framework | .NET Core / Blazor | Notes |
|----------------|-------------------|-------|
| Forms Authentication (`web.config`) | ASP.NET Core Identity or cookie auth | Configure in `Program.cs` |
| `FormsAuthentication.SetAuthCookie()` | `HttpContext.SignInAsync()` | Claims-based |
| `[Authorize]` (MVC) | `[Authorize]` attribute or `<AuthorizeView>` | Works on components and pages |
| Role-based `[Authorize(Roles="Admin")]` | Same attribute, claims-based roles | Configure role claims in auth provider |
| Windows Authentication (IIS) | Windows auth middleware | `builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)` |
| OWIN OAuth/OIDC | `AddAuthentication().AddOpenIdConnect()` | Standard OIDC middleware |
| Custom `ActionFilterAttribute` auth | `AuthorizeAttribute` or policy-based auth | `builder.Services.AddAuthorization(options => ...)` |
| `Membership` / `RoleProvider` | ASP.NET Core Identity | Full replacement |

## Error Handling

| .NET Framework | .NET Core / Blazor | Notes |
|----------------|-------------------|-------|
| `customErrors` in `web.config` | `app.UseExceptionHandler()` middleware | Or `ErrorBoundary` component |
| `Application_Error` in `Global.asax` | Exception-handling middleware | `app.UseExceptionHandler("/Error")` |
| `HandleErrorAttribute` (MVC) | Exception-handling middleware or `IExceptionFilter` | Global or per-endpoint |
| `try/catch` with `HttpException` | `try/catch` with `Results.Problem()` | Standard problem details |

## Logging

| .NET Framework | .NET Core / Blazor | Notes |
|----------------|-------------------|-------|
| `log4net` | `ILogger<T>` (built-in) or Serilog | Built-in logging preferred |
| `NLog` | `ILogger<T>` or NLog.Extensions.Logging | NLog has .NET Core adapter |
| `System.Diagnostics.Trace` | `ILogger<T>` | Built-in logging |
| Enterprise Library Logging | `ILogger<T>` + Serilog sinks | Full replacement |
| `ConfigurationManager.AppSettings["LogLevel"]` | `appsettings.json` `Logging` section | Standard log level config |
