using Fluxo.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fluxo.Infrastructure.Persistence.Configurations;

public class FinancialMovementConfiguration : IEntityTypeConfiguration<FinancialMovement>
{
    public void Configure(EntityTypeBuilder<FinancialMovement> builder)
    {
        builder.ToTable("FinancialMovements");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccountId).IsRequired();
        builder.Property(x => x.TransactionDate).IsRequired();
        builder.Property(x => x.RegisteredAt).IsRequired();
        builder.Property(x => x.Amount).HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.Currency).HasConversion<string>().IsRequired();
        builder.Property(x => x.MovementType).HasConversion<string>().IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.Status).HasConversion<string>().IsRequired();
        builder.Property(x => x.RelatedMovementId);
        builder.Property(x => x.UpdatedAt);

        builder.HasIndex(x => x.AccountId);
        builder.HasIndex(x => x.TransactionDate);
    }
}
