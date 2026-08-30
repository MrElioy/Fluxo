using Fluxo.Application.DTOs.FinancialMovements;
using Fluxo.Application.Interfaces.Persistence;
using Fluxo.Application.Interfaces.Services;
using Fluxo.Application.Results;
using Fluxo.Domain.Common;

/// <summary>
/// Coordinates financial movement use cases by validating application input,
/// invoking Domain behavior, collaborating with persistence abstractions and
/// translating outcomes into DTOs and explicit results.
/// </summary>

namespace Fluxo.Application.Services;

public sealed class FinancialMovementService : IFinancialMovementService
{
    private readonly IFinancialMovementRepository _repository;

    public FinancialMovementService(IFinancialMovementRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<FinancialMovementDto>> CreateAsync(CreateFinancialMovementDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.AccountId == Guid.Empty)
        {
            return Result<FinancialMovementDto>.Failure(new Error("ACCOUNT_ID_REQUIRED", "El identificador de la cuenta es obligatorio."));
        }

        if (dto.Amount <= 0)
        {
            return Result<FinancialMovementDto>.Failure(new Error("AMOUNT_INVALID", "El monto debe ser mayor que cero."));
        }

        if (dto.TransactionDate > DateTime.Today)
        {
            return Result<FinancialMovementDto>.Failure(new Error("DATE_INVALID", "La fecha del movimiento no puede ser futura."));
        }

        var movement = new FinancialMovement(
            dto.AccountId,
            dto.Amount,
            dto.TransactionDate,
            dto.Description ?? string.Empty,
            dto.MovementType,
            dto.Currency,
            dto.RelatedMovementId);

        await _repository.AddAsync(movement, cancellationToken);

        return Result<FinancialMovementDto>.Success(new FinancialMovementDto
        {
            Id = movement.Id,
            AccountId = movement.AccountId,
            TransactionDate = movement.TransactionDate,
            RegisteredAt = movement.RegisteredAt,
            Amount = movement.Amount,
            Currency = movement.Currency,
            MovementType = movement.MovementType,
            Description = movement.Description,
            Status = movement.Status,
            RelatedMovementId = movement.RelatedMovementId,
            UpdatedAt = movement.UpdatedAt
        });
    }

    public async Task<Result<List<FinancialMovementDto>>> GetAllAsync(FinancialMovementFilterDto? filter = null, CancellationToken cancellationToken = default)
    {
        var movements = await _repository.GetAllAsync(filter, cancellationToken);

        var items = movements.Select(movement => new FinancialMovementDto
        {
            Id = movement.Id,
            AccountId = movement.AccountId,
            TransactionDate = movement.TransactionDate,
            RegisteredAt = movement.RegisteredAt,
            Amount = movement.Amount,
            Currency = movement.Currency,
            MovementType = movement.MovementType,
            Description = movement.Description,
            Status = movement.Status,
            RelatedMovementId = movement.RelatedMovementId,
            UpdatedAt = movement.UpdatedAt
        }).ToList();

        return Result<List<FinancialMovementDto>>.Success(items);
    }

    public async Task<Result<FinancialMovementDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            return Result<FinancialMovementDto>.Failure(new Error("MOVEMENT_ID_REQUIRED", "El identificador del movimiento es obligatorio."));
        }

        var movement = await _repository.GetByIdAsync(id, cancellationToken);
        if (movement is null)
        {
            return Result<FinancialMovementDto>.Failure(new Error("MOVEMENT_NOT_FOUND", "El movimiento no existe."));
        }

        return Result<FinancialMovementDto>.Success(new FinancialMovementDto
        {
            Id = movement.Id,
            AccountId = movement.AccountId,
            TransactionDate = movement.TransactionDate,
            RegisteredAt = movement.RegisteredAt,
            Amount = movement.Amount,
            Currency = movement.Currency,
            MovementType = movement.MovementType,
            Description = movement.Description,
            Status = movement.Status,
            RelatedMovementId = movement.RelatedMovementId,
            UpdatedAt = movement.UpdatedAt
        });
    }

    public async Task<Result<FinancialMovementDto>> UpdateAsync(Guid id, UpdateFinancialMovementDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return Result<FinancialMovementDto>.Failure(new Error("MOVEMENT_NOT_FOUND", "El movimiento no existe."));
        }

        if (dto.Amount is <= 0)
        {
            return Result<FinancialMovementDto>.Failure(new Error("AMOUNT_INVALID", "El monto debe ser mayor que cero."));
        }

        if (dto.TransactionDate.HasValue && dto.TransactionDate.Value > DateTime.Today)
        {
            return Result<FinancialMovementDto>.Failure(new Error("DATE_INVALID", "La fecha del movimiento no puede ser futura."));
        }

        return Result<FinancialMovementDto>.Success(new FinancialMovementDto
        {
            Id = existing.Id,
            AccountId = existing.AccountId,
            TransactionDate = existing.TransactionDate,
            RegisteredAt = existing.RegisteredAt,
            Amount = existing.Amount,
            Currency = existing.Currency,
            MovementType = existing.MovementType,
            Description = existing.Description,
            Status = existing.Status,
            RelatedMovementId = existing.RelatedMovementId,
            UpdatedAt = existing.UpdatedAt
        });
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var movement = await _repository.GetByIdAsync(id, cancellationToken);
        if (movement is null)
        {
            return Result.Failure(new Error("MOVEMENT_NOT_FOUND", "El movimiento no existe."));
        }

        return Result.Success();
    }
}
