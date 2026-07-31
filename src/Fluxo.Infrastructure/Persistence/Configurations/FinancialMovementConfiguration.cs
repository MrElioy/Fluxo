using Fluxo.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fluxo.Infrastructure.Persistence.Configurations;

// Esta configuración define cómo la entidad FinancialMovement se persiste en SQLite.
// Aquí se indican aspectos técnicos como:
// - el nombre de la tabla,
// - la clave primaria,
// - columnas requeridas,
// - tipo/longitud de propiedades,
// - conversión de enums a texto,
// - índices para búsquedas frecuentes.
//
// La intención es que el dominio siga definiendo el significado del movimiento,
// mientras Infrastructure define cómo se guarda en la base.
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
