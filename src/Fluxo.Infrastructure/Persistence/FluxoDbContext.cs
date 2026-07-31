using Fluxo.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Infrastructure.Persistence;

public class FluxoDbContext : DbContext
{
    public FluxoDbContext(DbContextOptions<FluxoDbContext> options)
        : base(options)
    {
    }

    public DbSet<FinancialMovement> FinancialMovements => Set<FinancialMovement>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<MovementCategory> MovementCategories => Set<MovementCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FluxoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
