using Fluxo.Domain.Common;

/// <summary>
/// Contains the information required to create a financial movement.
/// It represents application input and must not contain business behavior
/// or persistence-specific configuration.
/// </summary>

namespace Fluxo.Application.DTOs.FinancialMovements;

public sealed class CreateFinancialMovementDto
{
    public Guid AccountId { get; init; }
    public decimal Amount { get; init; }
    public DateTime TransactionDate { get; init; }
    public string? Description { get; init; }
    public MovementType MovementType { get; init; }
    public Currency Currency { get; init; }
    public Guid? RelatedMovementId { get; init; }
}
