using Fluxo.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Infrastructure.Persistence.Repositories;

// CategoryRepository centraliza las operaciones de persistencia relacionadas con
// categorías.
//
// Se encarga de abstraer el acceso a la base para que la capa superior no tenga
// que conocer el detalle de cómo se consultan o se guardan las categorías.
//
// En esta versión base, el repositorio expone operaciones simples como:
// - agregar una categoría,
// - listar todas,
// - buscar por Id.
public class CategoryRepository
{
    private readonly FluxoDbContext _context;

    public CategoryRepository(FluxoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
