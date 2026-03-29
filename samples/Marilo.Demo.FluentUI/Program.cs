using Marilo.Core.Extensions;
using Marilo.Providers.FluentUI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMarilo().UseFluentUI();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<Marilo.Demo.FluentUI.App>()
    .AddInteractiveServerRenderMode();

app.Run();
