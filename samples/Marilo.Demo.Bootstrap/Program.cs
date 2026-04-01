using Marilo.Core.Extensions;
using Marilo.Providers.Bootstrap.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMarilo().UseBootstrap();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<Marilo.Demo.Bootstrap.App>()
    .AddInteractiveServerRenderMode();

app.Run();
