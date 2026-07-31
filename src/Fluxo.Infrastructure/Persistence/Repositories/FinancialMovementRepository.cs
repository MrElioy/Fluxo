using Fluxo.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Infrastructure.Persistence.Repositories;

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
