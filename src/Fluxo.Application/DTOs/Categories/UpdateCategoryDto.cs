namespace Fluxo.Application.DTOs.Categories;

/// <summary>
/// Contains the information required to update an existing category.
/// Changes described by this object must be applied through behavior
/// explicitly provided by the Domain entity.
/// </summary>

using Fluxo.Domain.Common;

public sealed class UpdateCategoryDto
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public EntityStatus? Status { get; init; }
    public Guid? ParentCategoryId { get; init; }
}
