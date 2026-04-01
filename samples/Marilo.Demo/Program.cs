using Marilo.Core.Contracts;
using Marilo.Core.Extensions;
using Marilo.Demo.Data;
using Marilo.Demo.Services;
using Marilo.Providers.Bootstrap;
using Marilo.Providers.Bootstrap.Extensions;
using Marilo.Providers.FluentUI;
using Marilo.Providers.FluentUI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register core Marilo services (theme service, notifications, etc.)
builder.Services.AddMarilo().AddMariloCoreServices();

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
        DemoBaseUrl = config["demoBaseUrl"] ?? "http://localhost:8080"
    };
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<Marilo.Demo.App>()
    .AddInteractiveServerRenderMode();

app.Run();
