using Marilo.Core.Contracts;
using Marilo.Core.Extensions;
using Marilo.Demo.Data;
using Marilo.Demo.Services;
using Marilo.Providers.Bootstrap;
using Marilo.Providers.Bootstrap.Extensions;
using Marilo.Providers.FluentUI;
using Marilo.Providers.FluentUI.Extensions;

var builder = WebApplication.CreateBuilder(args);

var isContainer = string.Equals(
    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"),
    "true",
    StringComparison.OrdinalIgnoreCase);
var hasExplicitHttpsCertificate =
    !string.IsNullOrWhiteSpace(builder.Configuration["Kestrel:Certificates:Default:Path"]) ||
    !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Path"));
var useHttpOnly = builder.Environment.IsDevelopment() && isContainer && !hasExplicitHttpsCertificate;

if (useHttpOnly)
{
    // Containers often do not have a trusted dev cert; run the demo over HTTP.
    builder.WebHost.UseUrls("http://0.0.0.0:5301");
}

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register core Marilo services (theme service, notifications, etc.)
builder.Services.AddMarilo().AddMariloCoreServices();

// For standalone apps (without provider switching), use:
//   builder.Services.AddMariloIconsTabler();   // Tabler Icons (recommended)
// The demo uses ProviderSwitcher below which delegates to design-specific providers.

// Register both provider implementations as concrete types
builder.Services.AddSingleton(new FluentUIOptions());
builder.Services.AddSingleton(new BootstrapOptions());
builder.Services.AddScoped<FluentUICssProvider>();
builder.Services.AddScoped<FluentUIIconProvider>();
builder.Services.AddScoped<FluentUIJsInterop>();
builder.Services.AddScoped<BootstrapCssProvider>();
builder.Services.AddScoped<BootstrapIconProvider>();
builder.Services.AddScoped<BootstrapJsInterop>();

// Register the switcher as the implementation for all three interfaces
builder.Services.AddScoped<ProviderSwitcher>();
builder.Services.AddScoped<IMariloCssProvider>(sp => sp.GetRequiredService<ProviderSwitcher>());
builder.Services.AddScoped<IMariloIconProvider>(sp => sp.GetRequiredService<ProviderSwitcher>());
builder.Services.AddScoped<IMariloJsInterop>(sp => sp.GetRequiredService<ProviderSwitcher>());

builder.Services.AddScoped<FavoritesService>();

var siteLinksPath = Path.Combine(builder.Environment.ContentRootPath, "..", "..", "site-links.json");
if (File.Exists(siteLinksPath))
    builder.Configuration.AddJsonFile(Path.GetFullPath(siteLinksPath), optional: true);

builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new SiteLinks
    {
        DocsBaseUrl = config["docsBaseUrl"] ?? "http://localhost:8081",
        DemoBaseUrl = config["demoBaseUrl"] ?? "http://localhost:5301"
    };
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

if (!useHttpOnly)
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<Marilo.Demo.App>()
    .AddInteractiveServerRenderMode();

app.Run();
