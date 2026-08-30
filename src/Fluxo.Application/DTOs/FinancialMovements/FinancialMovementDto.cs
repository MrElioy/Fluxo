using Fluxo.Domain.Common;

/// <summary>
/// Represents a financial movement exposed by the Application layer.
/// It transfers movement data to presentation clients without exposing
/// the mutable lifecycle or internal behavior of the Domain entity.
/// </summary>

namespace Fluxo.Application.DTOs.FinancialMovements;

public sealed class FinancialMovementDto
{
    public Guid Id { get; init; }
    public Guid AccountId { get; init; }
    public DateTime TransactionDate { get; init; }
    public DateTime RegisteredAt { get; init; }
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }
    public MovementType MovementType { get; init; }
    public string? Description { get; init; }
    public EntityStatus Status { get; init; }
    public Guid? RelatedMovementId { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
