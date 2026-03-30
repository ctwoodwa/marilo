using Marilo.Core.Extensions;
using Marilo.Demo.Data;
using Marilo.Providers.FluentUI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMarilo().UseFluentUI();

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
