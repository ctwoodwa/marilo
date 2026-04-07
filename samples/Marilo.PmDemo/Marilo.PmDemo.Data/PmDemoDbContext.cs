using Marilo.PmDemo.Data.Authorization;
using Marilo.PmDemo.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marilo.PmDemo.Data;

public class PmDemoDbContext : DbContext
{
    private readonly string _currentTenantId;

    public PmDemoDbContext(DbContextOptions<PmDemoDbContext> options, ITenantContext tenant)
        : base(options)
    {
        _currentTenantId = tenant.TenantId;
    }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<Subtask> Subtasks => Set<Subtask>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Milestone> Milestones => Set<Milestone>();
    public DbSet<Risk> Risks => Set<Risk>();
    public DbSet<BudgetLine> BudgetLines => Set<BudgetLine>();
    public DbSet<AuditRecord> AuditRecords => Set<AuditRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>().HasQueryFilter(e => e.TenantId == _currentTenantId);
        modelBuilder.Entity<TaskItem>().HasQueryFilter(e => e.TenantId == _currentTenantId);
        modelBuilder.Entity<AuditRecord>().HasQueryFilter(e => e.TenantId == _currentTenantId);
    }
}
