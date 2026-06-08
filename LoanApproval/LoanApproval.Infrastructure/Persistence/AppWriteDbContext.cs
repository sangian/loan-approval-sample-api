using LoanApproval.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanApproval.Infrastructure.Persistence;

public sealed class AppWriteDbContext : DbContext
{
    public AppWriteDbContext(DbContextOptions<AppWriteDbContext> options) : base(options)
    {
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
}