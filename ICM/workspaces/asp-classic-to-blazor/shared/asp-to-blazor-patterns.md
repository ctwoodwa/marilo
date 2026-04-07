# ASP Classic to Blazor Pattern Mapping

Quick-reference for converting ASP Classic constructs to their Blazor equivalents. Use this during Stage 02 (architecture) and Stage 04 (page conversion).

## Code Blocks and Expressions

| ASP Classic | Blazor Equivalent | Notes |
|-------------|-------------------|-------|
| `<% %>` code blocks | `@code { }` blocks | Place at bottom of .razor file |
| `<%= expression %>` | `@expression` | Auto-encoded by Razor |
| `<% Response.Write(val) %>` | `@val` in markup | Direct Razor expression |
| `<% If condition Then %>` | `@if (condition) { }` | C# conditional rendering |
| `<% For Each item In col %>` | `@foreach (var item in col) { }` | C# iteration |

## Server Includes and Shared Code

| ASP Classic | Blazor Equivalent | Notes |
|-------------|-------------------|-------|
| `<!--#include file="header.inc"-->` | `<HeaderComponent />` | Shared Razor component |
| `<!--#include virtual="/shared/utils.asp"-->` | `@inject IUtilityService Util` | DI-injected service |
| Shared function libraries (.inc files) | Static utility classes or DI services | Prefer DI for testability |
| Global.asa `Application_OnStart` | `Program.cs` service registration | DI container setup |
| Global.asa `Session_OnStart` | Middleware or circuit handler | Blazor Server circuit events |

## State Management

| ASP Classic | Blazor Equivalent | Notes |
|-------------|-------------------|-------|
| `Session("key") = value` | `ProtectedSessionStorage` or cascading parameter | Blazor Server only for session storage |
| `Session("key")` read | `await SessionStorage.GetAsync<T>("key")` | Async in Blazor |
| `Application("key")` | Singleton service via DI | Register in Program.cs |
| `Session.Abandon` | `NavigationManager.NavigateTo("/logout", true)` | Force reload clears circuit |

## Request and Response

| ASP Classic | Blazor Equivalent | Notes |
|-------------|-------------------|-------|
| `Request.Form("field")` | `@bind` or `[Parameter]` with EditContext | Two-way binding |
| `Request.QueryString("param")` | `[Parameter] [SupplyParameterFromQuery]` | Route or query parameter |
| `Request.Cookies("name")` | `IJSRuntime` cookie access or middleware | No direct cookie API in Blazor |
| `Request.ServerVariables` | `IHttpContextAccessor` (Server) | Only available in Blazor Server |
| `Response.Redirect("url")` | `NavigationManager.NavigateTo("url")` | Client-side navigation |
| `Response.ContentType` | Controller or minimal API for file downloads | Blazor components do not set headers |
| `Server.HTMLEncode(val)` | Automatic in Razor | Use `@((MarkupString)val)` for raw HTML |
| `Server.URLEncode(val)` | `Uri.EscapeDataString(val)` | Standard .NET |
| `Server.MapPath("/path")` | `IWebHostEnvironment.ContentRootPath` | Inject via DI |

## Data Access

For standard CRUD operations, use DAB endpoints via HttpClient. For complex logic, use EF Core or Dapper through the custom API service. See `shared/dab-reference.md` for DAB config details.

| ASP Classic | Blazor Equivalent | Notes |
|-------------|-------------------|-------|
| ADO recordset listing (`rs.Open "SELECT * FROM..."`) | DAB GET `/api/entity-name` via HttpClient | DAB handles filtering, sorting, pagination |
| ADO single-record lookup | DAB GET `/api/entity-name/PK` via HttpClient | Returns single entity by primary key |
| ADO insert (`conn.Execute "INSERT..."`) | DAB POST `/api/entity-name` via HttpClient | Send JSON body with field values |
| ADO update (`conn.Execute "UPDATE..."`) | DAB PATCH `/api/entity-name/PK` via HttpClient | Send JSON body with changed fields |
| ADO delete (`conn.Execute "DELETE..."`) | DAB DELETE `/api/entity-name/PK` via HttpClient | Returns 204 on success |
| Complex query with joins/aggregation | Custom API endpoint + EF Core/Dapper | DAB does not handle multi-table logic |
| `Server.CreateObject("ADODB.Connection")` | `DbContext` or `IDbConnection` via DI | For custom API endpoints only |
| `ADODB.Recordset` | `List<T>` from EF Core or Dapper | Strongly typed models |
| `conn.Execute(sql)` (complex logic) | `await dbContext.Database.ExecuteSqlRawAsync()` or Dapper `ExecuteAsync` | Always parameterize |
| `rs.Open sql, conn` (complex query) | `await dbContext.Set<T>().ToListAsync()` | EF Core query for custom API |
| `rs("column")` | `entity.Column` property | Strongly typed access |
| `rs.MoveNext` / `rs.EOF` | `foreach` loop over results | No manual cursor |
| Inline SQL with string concat | Parameterized queries: `$"WHERE Id = @id"` | Prevents SQL injection |
| `conn.ConnectionString = "..."` | `appsettings.json` + `builder.Configuration` | DAB uses `@env('VAR_NAME')` pattern |

## COM Objects

| ASP Classic | Blazor Equivalent | Notes |
|-------------|-------------------|-------|
| `Server.CreateObject("Scripting.FSO")` | `System.IO.File`, `System.IO.Directory` | Standard .NET file I/O |
| `Server.CreateObject("CDOSYS.Message")` | `IEmailSender` or SmtpClient service | Register via DI |
| `Server.CreateObject("MSXML2.XMLHTTP")` | `IHttpClientFactory` / `HttpClient` | Register in Program.cs |
| `Server.CreateObject("Scripting.Dictionary")` | `Dictionary<TKey, TValue>` | Standard .NET collection |
| Custom COM components | Rewrite as C# services or use COM interop as bridge | Prefer rewrite |

## Authentication

| ASP Classic | Blazor Equivalent | Notes |
|-------------|-------------------|-------|
| IIS Basic/Windows auth | `[Authorize]` attribute + Windows auth middleware | Configure in Program.cs |
| Custom login page with Session | ASP.NET Core Identity or cookie auth | Use `AuthenticationStateProvider` |
| Role checks via Session | `[Authorize(Roles = "Admin")]` | Claims-based authorization |
| `Response.Status = "401"` | `AuthorizeView` component | Declarative in Razor |

## Error Handling

| ASP Classic | Blazor Equivalent | Notes |
|-------------|-------------------|-------|
| `On Error Resume Next` | `try/catch` blocks | Explicit error handling |
| `If Err.Number <> 0 Then` | `catch (Exception ex)` | Specific exception types preferred |
| `Err.Clear` | End of catch block | No equivalent needed |
| Custom error page via IIS | `ErrorBoundary` component or middleware | Per-component or global |
