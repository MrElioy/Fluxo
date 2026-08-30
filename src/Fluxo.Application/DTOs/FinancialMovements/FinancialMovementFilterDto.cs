using Fluxo.Domain.Common;

/// <summary>
/// Defines the optional criteria supported when searching for financial
/// movements through the Application layer. It remains independent from
/// Entity Framework Core, SQLite and other persistence technologies.
/// </summary>

namespace Fluxo.Application.DTOs.FinancialMovements;

public sealed class FinancialMovementFilterDto
{
    public Guid? AccountId { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public DateTime? RegisteredFrom { get; init; }
    public DateTime? RegisteredTo { get; init; }
    public decimal? MinAmount { get; init; }
    public decimal? MaxAmount { get; init; }
    public Currency? Currency { get; init; }
    public MovementType? MovementType { get; init; }
    public EntityStatus? Status { get; init; }
    public string? DescriptionContains { get; init; }
    public Guid? RelatedMovementId { get; init; }
}
