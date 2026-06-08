using LoanApproval.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanApproval.Infrastructure.Persistence;

public sealed class AppQueryDbContext : DbContext
{
    public AppQueryDbContext(DbContextOptions<AppQueryDbContext> options) : base(options)
    {
        ChangeTracker.AutoDetectChangesEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
    }
    
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Loan> Loans => Set<Loan>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppWriteDbContext).Assembly);
    }
    
    public override int SaveChanges()
    {
        throw new NotSupportedException("This context is read-only. Use AppWriteDbContext for write operations.");
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        throw new NotSupportedException("This context is read-only. Use AppWriteDbContext for write operations.");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("This context is read-only. Use AppWriteDbContext for write operations.");
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("This context is read-only. Use AppWriteDbContext for write operations.");
    }
}