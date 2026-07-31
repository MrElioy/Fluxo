using Fluxo.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fluxo.Infrastructure.Persistence.Configurations;

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
