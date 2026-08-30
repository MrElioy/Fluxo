namespace Fluxo.Application.DTOs.Categories;

/// <summary>
/// Contains the information required to create a category.
/// It describes application input and does not define persistence details
/// or duplicate rules protected by the Domain model.
/// </summary>

public sealed class CreateCategoryDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid? ParentCategoryId { get; init; }
}
