using Fluxo.Application.DTOs.Categories;
using Fluxo.Domain.Common;

/// <summary>
/// Defines the persistence operations required by category use cases.
/// Application owns this abstraction, and Infrastructure implements it
/// using the configured persistence mechanism.
/// </summary>

namespace Fluxo.Application.Interfaces.Persistence;

public interface ICategoryRepository
{
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
    Task<List<Category>> GetAllAsync(CategoryFilterDto? filter = null, CancellationToken cancellationToken = default);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
