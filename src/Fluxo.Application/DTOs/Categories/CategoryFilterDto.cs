namespace Fluxo.Application.DTOs.Categories;

/// <summary>
/// Defines the optional criteria supported when searching for categories.
/// The criteria belong to the Application contract and remain independent
/// from the database technology used to execute the query.
/// </summary>

using Fluxo.Domain.Common;

public sealed class CategoryFilterDto
{
    public string? NameContains { get; init; }
    public EntityStatus? Status { get; init; }
    public Guid? ParentCategoryId { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
}
