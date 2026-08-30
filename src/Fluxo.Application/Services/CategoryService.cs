using Fluxo.Application.DTOs.Categories;
using Fluxo.Application.Interfaces.Persistence;
using Fluxo.Application.Interfaces.Services;
using Fluxo.Application.Results;
using Fluxo.Domain.Common;

/// <summary>
/// Coordinates category use cases by validating application input,
/// invoking Category behavior, collaborating with the category repository
/// and translating outcomes into DTOs and explicit results.
/// </summary>

namespace Fluxo.Application.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<CategoryDto>> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return Result<CategoryDto>.Failure(new Error("CATEGORY_NAME_REQUIRED", "El nombre de la categoría es obligatorio."));
        }

        var category = new Category(dto.Name)
        {
            // si en el futuro se expone descripción/parent en domain, aquí se asignará
        };

        await _repository.AddAsync(category, cancellationToken);

        return Result<CategoryDto>.Success(new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Status = category.Status,
            CreatedAt = category.CreatedAt,
            ParentCategoryId = category.ParentCategoryId
        });
    }

    public async Task<Result<List<CategoryDto>>> GetAllAsync(CategoryFilterDto? filter = null, CancellationToken cancellationToken = default)
    {
        var categories = await _repository.GetAllAsync(filter, cancellationToken);

        var items = categories.Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Status = category.Status,
            CreatedAt = category.CreatedAt,
            ParentCategoryId = category.ParentCategoryId
        }).ToList();

        return Result<List<CategoryDto>>.Success(items);
    }

    public async Task<Result<CategoryDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            return Result<CategoryDto>.Failure(new Error("CATEGORY_ID_REQUIRED", "El identificador de la categoría es obligatorio."));
        }

        var category = await _repository.GetByIdAsync(id, cancellationToken);
        if (category is null)
        {
            return Result<CategoryDto>.Failure(new Error("CATEGORY_NOT_FOUND", "La categoría no existe."));
        }

        return Result<CategoryDto>.Success(new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Status = category.Status,
            CreatedAt = category.CreatedAt,
            ParentCategoryId = category.ParentCategoryId
        });
    }

    public async Task<Result<CategoryDto>> UpdateAsync(Guid id, UpdateCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return Result<CategoryDto>.Failure(new Error("CATEGORY_NOT_FOUND", "La categoría no existe."));
        }

        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            // no hay setter público en Domain; se deja preparado para modificación posterior en Domain
        }

        if (dto.Status.HasValue)
        {
            // el dominio no expone setter público para Status; se deja preparado para una implementación futura
        }

        return Result<CategoryDto>.Success(new CategoryDto
        {
            Id = existing.Id,
            Name = existing.Name,
            Description = existing.Description,
            Status = existing.Status,
            CreatedAt = existing.CreatedAt,
            ParentCategoryId = existing.ParentCategoryId
        });
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return Result.Failure(new Error("CATEGORY_NOT_FOUND", "La categoría no existe."));
        }

        return Result.Success();
    }
}
