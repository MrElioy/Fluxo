using Fluxo.Application.DTOs.Categories;
using Fluxo.Application.Results;

/// <summary>
/// Defines the category use cases available to presentation clients.
/// It forms a stable boundary shared by Console, WPF and future clients.
/// </summary>

namespace Fluxo.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<Result<CategoryDto>> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default);
    Task<Result<List<CategoryDto>>> GetAllAsync(CategoryFilterDto? filter = null, CancellationToken cancellationToken = default);
    Task<Result<CategoryDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<CategoryDto>> UpdateAsync(Guid id, UpdateCategoryDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
