using Fluxo.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fluxo.Infrastructure.Persistence.Configurations;

// CategoryConfiguration describe el mapeo de la entidad Category hacia la base.
// Aquí se define la estructura de almacenamiento de categorías, incluyendo:
// - tabla de categorías,
// - nombre obligatorio y longitud máxima,
// - descripción opcional,
// - estado activo/inactivo,
// - fecha de creación,
// - relación jerárquica con ParentCategoryId.
//
// Toda esta información pertenece a Infrastructure porque describe el formato
// físico del almacenamiento, no las reglas de negocio de la categoría.
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(120);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.Status).HasConversion<string>().IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.ParentCategoryId);

        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasIndex(x => x.ParentCategoryId);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.ParentCategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
