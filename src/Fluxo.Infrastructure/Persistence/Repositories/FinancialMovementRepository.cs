using Fluxo.Application.DTOs.FinancialMovements;
using Fluxo.Application.Interfaces.Persistence;
using Fluxo.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Infrastructure.Persistence.Repositories;

public sealed class FinancialMovementFilter
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

    public void Validate()
    {
        if (AccountId.HasValue && AccountId.Value == Guid.Empty)
        {
            throw new ArgumentException("El Id de la cuenta no puede estar vacío.", nameof(AccountId));
        }

        if (RelatedMovementId.HasValue && RelatedMovementId.Value == Guid.Empty)
        {
            throw new ArgumentException("El Id del movimiento relacionado no puede estar vacío.", nameof(RelatedMovementId));
        }

        if (FromDate.HasValue && ToDate.HasValue && FromDate.Value > ToDate.Value)
        {
            throw new ArgumentException("La fecha inicial no puede ser mayor a la fecha final.", nameof(FromDate));
        }

        if (RegisteredFrom.HasValue && RegisteredTo.HasValue && RegisteredFrom.Value > RegisteredTo.Value)
        {
            throw new ArgumentException("La fecha de registro inicial no puede ser mayor a la fecha de registro final.", nameof(RegisteredFrom));
        }

        if (MinAmount.HasValue && MaxAmount.HasValue && MinAmount.Value > MaxAmount.Value)
        {
            throw new ArgumentException("El monto mínimo no puede ser mayor al monto máximo.", nameof(MinAmount));
        }

        if (DescriptionContains is not null && string.IsNullOrWhiteSpace(DescriptionContains))
        {
            throw new ArgumentException("El texto de descripción no puede estar vacío.", nameof(DescriptionContains));
        }
    }
}

// FinancialMovementRepository encapsula las consultas y operaciones de acceso
// a datos para los movimientos financieros.
//
// Su función es evitar que la capa de Application o la UI dependan directamente
// de DbContext, DbSet o LINQ de EF Core.
//
// En esta primera versión se ofrecen operaciones básicas como:
// - agregar un movimiento,
// - recuperar todos con filtros opcionales,
// - obtener uno por Id.
public class FinancialMovementRepository : IFinancialMovementRepository
{
    private readonly FluxoDbContext _context;

    public FinancialMovementRepository(FluxoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(FinancialMovement movement, CancellationToken cancellationToken = default)
    {
        await _context.FinancialMovements.AddAsync(movement, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<FinancialMovement>> GetAllAsync(FinancialMovementFilterDto? filter = null, CancellationToken cancellationToken = default)
    {
        IQueryable<FinancialMovement> query = _context.FinancialMovements
            .AsNoTracking();

        if (filter is null)
        {
            return await query.ToListAsync(cancellationToken);
        }

        if (filter.AccountId.HasValue)
        {
            query = query.Where(x => x.AccountId == filter.AccountId.Value);
        }

        if (filter.FromDate.HasValue)
        {
            query = query.Where(x => x.TransactionDate >= filter.FromDate.Value);
        }

        if (filter.ToDate.HasValue)
        {
            query = query.Where(x => x.TransactionDate <= filter.ToDate.Value);
        }

        if (filter.RegisteredFrom.HasValue)
        {
            query = query.Where(x => x.RegisteredAt >= filter.RegisteredFrom.Value);
        }

        if (filter.RegisteredTo.HasValue)
        {
            query = query.Where(x => x.RegisteredAt <= filter.RegisteredTo.Value);
        }

        if (filter.MinAmount.HasValue)
        {
            query = query.Where(x => x.Amount >= filter.MinAmount.Value);
        }

        if (filter.MaxAmount.HasValue)
        {
            query = query.Where(x => x.Amount <= filter.MaxAmount.Value);
        }

        if (filter.Currency.HasValue)
        {
            query = query.Where(x => x.Currency == filter.Currency.Value);
        }

        if (filter.MovementType.HasValue)
        {
            query = query.Where(x => x.MovementType == filter.MovementType.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(x => x.Status == filter.Status.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.DescriptionContains))
        {
            query = query.Where(x => x.Description != null && x.Description.Contains(filter.DescriptionContains));
        }

        if (filter.RelatedMovementId.HasValue)
        {
            query = query.Where(x => x.RelatedMovementId == filter.RelatedMovementId.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<FinancialMovement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("El Id del movimiento financiero no puede estar vacío.", nameof(id));
        }

        return await _context.FinancialMovements
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
