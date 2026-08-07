using Fluxo.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Infrastructure.Persistence.Repositories;

public sealed class CategoryFilter
{
    public string? NameContains { get; init; }
    public EntityStatus? Status { get; init; }
    public Guid? ParentCategoryId { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }

    public void Validate()
    {
        if (ParentCategoryId.HasValue && ParentCategoryId.Value == Guid.Empty)
        {
            throw new ArgumentException("El Id de la categoría padre no puede estar vacío.", nameof(ParentCategoryId));
        }

        if (CreatedFrom.HasValue && CreatedTo.HasValue && CreatedFrom.Value > CreatedTo.Value)
        {
            throw new ArgumentException("La fecha inicial de creación no puede ser mayor a la fecha final.", nameof(CreatedFrom));
        }

        if (NameContains is not null && string.IsNullOrWhiteSpace(NameContains))
        {
            throw new ArgumentException("El texto de búsqueda del nombre no puede estar vacío.", nameof(NameContains));
        }
    }
}

// CategoryRepository centraliza las operaciones de persistencia relacionadas con
// categorías.
//
// Se encarga de abstraer el acceso a la base para que la capa superior no tenga
// que conocer el detalle de cómo se consultan o se guardan las categorías.
//
// En esta versión base, el repositorio expone operaciones simples como:
// - agregar una categoría,
// - listar todas con filtros opcionales,
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

    public async Task<List<Category>> GetAllAsync(CategoryFilter? filter = null, CancellationToken cancellationToken = default)
    {
        filter?.Validate();

        IQueryable<Category> query = _context.Categories
            .AsNoTracking();

        if (filter is null)
        {
            return await query.ToListAsync(cancellationToken);
        }

        if (!string.IsNullOrWhiteSpace(filter.NameContains))
        {
            query = query.Where(x => x.Name.Contains(filter.NameContains));
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(x => x.Status == filter.Status.Value);
        }

        if (filter.ParentCategoryId.HasValue)
        {
            query = query.Where(x => x.ParentCategoryId == filter.ParentCategoryId.Value);
        }

        if (filter.CreatedFrom.HasValue)
        {
            query = query.Where(x => x.CreatedAt >= filter.CreatedFrom.Value);
        }

        if (filter.CreatedTo.HasValue)
        {
            query = query.Where(x => x.CreatedAt <= filter.CreatedTo.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("El Id de la categoría no puede estar vacío.", nameof(id));
        }

        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
