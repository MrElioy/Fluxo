using Fluxo.Domain.Common;

/// <summary>
/// Represents a category exposed by the Application layer.
/// It provides presentation clients with category data without exposing
/// the internal behavior of the corresponding Domain entity.
/// </summary>

namespace Fluxo.Application.DTOs.Categories;

public sealed class CategoryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public EntityStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid? ParentCategoryId { get; init; }
}
