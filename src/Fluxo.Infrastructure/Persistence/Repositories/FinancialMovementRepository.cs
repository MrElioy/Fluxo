using Fluxo.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Infrastructure.Persistence.Repositories;

// FinancialMovementRepository encapsula las consultas y operaciones de acceso
// a datos para los movimientos financieros.
//
// Su función es evitar que la capa de Application o la UI dependan directamente
// de DbContext, DbSet o LINQ de EF Core.
//
// En esta primera versión se ofrecen operaciones básicas como:
// - agregar un movimiento,
// - recuperar todos,
// - obtener uno por Id.
public class FinancialMovementRepository
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

    public async Task<List<FinancialMovement>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.FinancialMovements
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<FinancialMovement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.FinancialMovements
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
