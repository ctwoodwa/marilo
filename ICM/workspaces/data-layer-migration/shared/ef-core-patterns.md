# EF Core Patterns

Reference patterns for EF Core usage in ad-hoc API endpoints. Covers DbContext setup,
entity configuration, migrations, and Dapper integration for performance-sensitive queries.

## DbContext

```csharp
public class CometDbContext : DbContext
{
    public CometDbContext(DbContextOptions<CometDbContext> options) : base(options) { }

    // Include only sets needed by ad-hoc endpoints
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<GlobalSettings> Settings => Set<GlobalSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all IEntityTypeConfiguration<T> classes from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CometDbContext).Assembly);
    }
}
```

## DI Registration (Program.cs)

```csharp
builder.Services.AddDbContext<CometDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CometDb"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 3)
    )
);
```

`appsettings.json`:
```json
{
  "ConnectionStrings": {
    "CometDb": "Server=localhost;Database=Comet;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

## Entity Configuration

```csharp
public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects", "dbo");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).UseIdentityColumn();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.CategoryId);
    }
}
```

## Migrations

```bash
# Add initial migration
dotnet ef migrations add InitialCreate --project src/Parsons.Comet.ApiService

# Apply migrations to dev database
dotnet ef database update --project src/Parsons.Comet.ApiService

# Generate SQL script (for review before prod)
dotnet ef migrations script --idempotent --output migrations.sql
```

## Seeder Pattern

```csharp
public interface IDbSeeder
{
    Task SeedAsync(CometDbContext context, CancellationToken ct = default);
}

public class ProjectSeeder : IDbSeeder
{
    public async Task SeedAsync(CometDbContext context, CancellationToken ct = default)
    {
        if (await context.Projects.AnyAsync(ct)) return; // already seeded

        context.Projects.AddRange(
            new Project { Name = "Bridge Rehabilitation", ... },
            new Project { Name = "Pavement Overlay", ... }
        );
        await context.SaveChangesAsync(ct);
    }
}

// In Program.cs — run after app.Build()
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<CometDbContext>();
await context.Database.MigrateAsync();
foreach (var seeder in scope.ServiceProvider.GetServices<IDbSeeder>())
    await seeder.SeedAsync(context);
```

## Repository Pattern

```csharp
public interface IProjectRepository
{
    Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken ct = default);
    Task<Project?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Project> AddAsync(Project project, CancellationToken ct = default);
    Task UpdateAsync(Project project, CancellationToken ct = default);
}

public class ProjectRepository : IProjectRepository
{
    private readonly CometDbContext _db;
    public ProjectRepository(CometDbContext db) => _db = db;

    public async Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken ct = default)
        => await _db.Projects.AsNoTracking().ToListAsync(ct);

    public async Task<Project?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Project> AddAsync(Project project, CancellationToken ct = default)
    {
        _db.Projects.Add(project);
        await _db.SaveChangesAsync(ct);
        return project;
    }

    public async Task UpdateAsync(Project project, CancellationToken ct = default)
    {
        _db.Projects.Update(project);
        await _db.SaveChangesAsync(ct);
    }
}
```

DI registration:
```csharp
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
```

## Dapper for Performance-Sensitive Reads

Use when EF Core's query translation produces inefficient SQL (e.g., complex aggregations
or reporting queries). Inject `IDbConnection` alongside `DbContext`.

```csharp
builder.Services.AddScoped<IDbConnection>(_ =>
    new SqlConnection(builder.Configuration.GetConnectionString("CometDb")));
```

```csharp
public class ProjectSummaryRepository
{
    private readonly IDbConnection _db;
    public ProjectSummaryRepository(IDbConnection db) => _db = db;

    public async Task<IEnumerable<ProjectSummary>> GetSummariesAsync()
    {
        const string sql = """
            SELECT p.Id, p.Name, c.Name AS CategoryName, COUNT(a.Id) AS AssetCount
            FROM dbo.Projects p
            JOIN dbo.Categories c ON c.Id = p.CategoryId
            LEFT JOIN dbo.Assets a ON a.ProjectId = p.Id
            GROUP BY p.Id, p.Name, c.Name
            ORDER BY p.Name
            """;

        return await _db.QueryAsync<ProjectSummary>(sql);
    }
}
```

## Aspire Integration (AppHost)

```csharp
var sqlServer = builder.AddSqlServer("sqlserver")
    .AddDatabase("CometDb");

var api = builder.AddProject<Projects.Parsons_Comet_ApiService>("apiservice")
    .WithReference(sqlServer);
```
