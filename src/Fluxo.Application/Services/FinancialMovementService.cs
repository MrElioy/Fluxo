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


//CreateAsync: metodo creado para crear un movimiento financiero, recibe un DTO de tipo CreateFinancialMovementDto
// y un token de cancelación opcional. Valida los datos del DTO y si son válidos, crea una instancia de FinancialMovement
// y la guarda en el repositorio. Devuelve un Result con el DTO del movimiento financiero creado o un error si la validación falla.
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

//GetAllAsync: metodo creado para obtener todos los movimientos financieros, recibe un filtro opcional de tipo FinancialMovementFilterDto
//  y un token de cancelación opcional. Llama al repositorio para obtener los movimientos financieros que
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


//GetByIdAsync: metodo creado para obtener un movimiento financiero por su identificador, recibe un Guid id y un token de cancelación opcional.
//  Valida el id y llama al repositorio para obtener el movimiento financiero correspondiente. Devuelve un Result con el DTO del movimiento
// financiero o un error si no se encuentra.
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

//UpdateAsync: metodo creado para actualizar un movimiento financiero, recibe un Guid id, un DTO de tipo UpdateFinancialMovementDto
//  y un token de cancelación opcional. Valida el id y los datos del DTO, llama al repositorio para obtener el movimiento financiero
// correspondiente y actualiza sus propiedades. Devuelve un Result con el DTO del movimiento financiero actualizado o un error si la validación
//  falla o no se encuentra el movimiento.
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

//DeleteAsync: metodo creado para eliminar un movimiento financiero, recibe un Guid id y un token de cancelación opcional. Valida el id
//  y llama al repositorio para obtener el movimiento financiero correspondiente. Devuelve un Result indicando éxito o un error si no se encuentra.
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
