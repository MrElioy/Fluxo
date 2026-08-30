using Fluxo.Application.DTOs.FinancialMovements;
using Fluxo.Domain.Common;

/// <summary>
/// Defines the persistence operations required by financial movement use cases.
/// Application owns this contract, while Infrastructure provides its concrete
/// implementation without exposing Entity Framework Core or SQLite.
/// </summary>

namespace Fluxo.Application.Interfaces.Persistence;

public interface IFinancialMovementRepository
{
    Task AddAsync(FinancialMovement movement, CancellationToken cancellationToken = default);
    Task<List<FinancialMovement>> GetAllAsync(FinancialMovementFilterDto? filter = null, CancellationToken cancellationToken = default);
    Task<FinancialMovement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
