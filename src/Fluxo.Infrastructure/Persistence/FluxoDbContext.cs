using Fluxo.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Infrastructure.Persistence;

// FluxoDbContext es la puerta de entrada de Entity Framework Core hacia SQLite.
// Representa la unidad de trabajo de la aplicación y expone los DbSet para las
// entidades persistibles del dominio.
//
// Su responsabilidad principal es:
// - conectar el modelo del dominio con la base de datos,
// - aplicar las configuraciones de mapeo,
// - trackear entidades y persistir cambios con SaveChanges/SaveChangesAsync.
//
// Este archivo no contiene lógica de negocio; solo define la sesión de persistencia.
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
