using Fluxo.Application.DTOs.FinancialMovements;
using Fluxo.Application.Results;

/// <summary>
/// Defines the financial movement use cases available to presentation clients.
/// Its operations accept and return Application models and do not expose
/// Domain entities or Infrastructure implementations.
/// </summary>

namespace Fluxo.Application.Interfaces.Services;

public interface IFinancialMovementService
{
    Task<Result<FinancialMovementDto>> CreateAsync(CreateFinancialMovementDto dto, CancellationToken cancellationToken = default);
    Task<Result<List<FinancialMovementDto>>> GetAllAsync(FinancialMovementFilterDto? filter = null, CancellationToken cancellationToken = default);
    Task<Result<FinancialMovementDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<FinancialMovementDto>> UpdateAsync(Guid id, UpdateFinancialMovementDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
