using Marilo.Core.Extensions;
using Marilo.Providers.FluentUI.Extensions;
using Marilo.PmDemo.Authorization;
using Marilo.PmDemo.Components;
using Marilo.PmDemo.Data;
using Marilo.PmDemo.Data.Authorization;
using Marilo.PmDemo.Data.Seeding;
using Marilo.PmDemo.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Wolverine;
using Wolverine.RabbitMQ;
using Wolverine.Postgresql;

var builder = WebApplication.CreateBuilder(args);

// Aspire service defaults (OTEL, health checks, resilience, service discovery).
builder.AddServiceDefaults();

// Tenant context — demo stub in development. Registered BEFORE the DbContext
// because PmDemoDbContext takes ITenantContext as a constructor dependency.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<ITenantContext, DemoTenantContext>();
}

// EF Core DbContext registered manually (not pooled) so it can accept the scoped
// ITenantContext in its constructor — Aspire's AddNpgsqlDbContext uses DbContextPool
// which forbids scoped constructor dependencies. EnrichNpgsqlDbContext layers
// Aspire's instrumentation, health checks, and retry on top.
builder.Services.AddDbContext<PmDemoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("pmdemodb")));
builder.EnrichNpgsqlDbContext<PmDemoDbContext>();

builder.AddRedisOutputCache("pmdemo-redis");

// SignalR with Redis backplane.
builder.Services.AddSignalR()
    .AddStackExchangeRedis(builder.Configuration.GetConnectionString("pmdemo-redis") ?? "localhost");

// Feature flags.
builder.Services.AddFeatureManagement();

// Wolverine messaging — RabbitMQ transport, Postgres outbox.
builder.Host.UseWolverine(opts =>
{
    var rabbitConn = builder.Configuration.GetConnectionString("pmdemo-rabbit");
    if (!string.IsNullOrWhiteSpace(rabbitConn))
    {
        opts.UseRabbitMq(rabbitConn).AutoProvision();
    }

    var pgConn = builder.Configuration.GetConnectionString("pmdemodb");
    if (!string.IsNullOrWhiteSpace(pgConn))
    {
        opts.PersistMessagesWithPostgresql(pgConn, "wolverine");
    }

    opts.Policies.UseDurableOutboxOnAllSendingEndpoints();
    opts.Policies.AutoApplyTransactions();
});

// Dev-only data seed.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<PmDemoSeeder>();
}

// CORS + antiforgery scaffold (no policies yet).
builder.Services.AddCors();
builder.Services.AddAuthorization();

// Marilo component services (FluentUI provider + theme service)
builder.Services.AddMarilo().UseFluentUI();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapHealthChecks("/health");
app.MapHub<PmDemoHub>("/hubs/pmdemo");

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(Marilo.PmDemo.Client._Imports).Assembly);

app.Run();
