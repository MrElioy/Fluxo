using Fluxo.Domain.Common;

/// <summary>
/// Contains the information required to update an existing financial movement.
/// The Application layer uses it to locate the movement and coordinate changes
/// through behavior explicitly supported by the Domain model.
/// </summary>

namespace Fluxo.Application.DTOs.FinancialMovements;

public sealed class UpdateFinancialMovementDto
{
    public Guid? AccountId { get; init; }
    public decimal? Amount { get; init; }
    public DateTime? TransactionDate { get; init; }
    public string? Description { get; init; }
    public MovementType? MovementType { get; init; }
    public Currency? Currency { get; init; }
    public EntityStatus? Status { get; init; }
    public Guid? RelatedMovementId { get; init; }
}
