using Fluxo.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fluxo.Infrastructure.Persistence.Configurations;

// MovementCategoryConfiguration modela la relación entre movimientos y categorías.
// Esta entidad actúa como tabla intermedia de una relación muchos a muchos.
//
// En esta configuración se define:
// - la clave compuesta MovementId + CategoryId,
// - las claves foráneas hacia FinancialMovement y Category,
// - el comportamiento de eliminación en cascada.
//
// La idea es evitar duplicados de la misma asociación y mantener la integridad
// referencial entre las entidades relacionadas.
public class MovementCategoryConfiguration : IEntityTypeConfiguration<MovementCategory>
{
    public void Configure(EntityTypeBuilder<MovementCategory> builder)
    {
        builder.ToTable("MovementCategories");
        builder.HasKey(x => new { x.MovementId, x.CategoryId });

        builder.Property(x => x.MovementId).IsRequired();
        builder.Property(x => x.CategoryId).IsRequired();

        builder.HasOne<FinancialMovement>()
            .WithMany()
            .HasForeignKey(x => x.MovementId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
