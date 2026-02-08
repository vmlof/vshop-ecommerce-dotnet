using VShop.ProductApi.Models;

namespace VShop.ProductApi.DTOs.Mappings;

public static class CategoryDtoMappingExtensions
{
    public static CategoryDto? ToCategoryDto(this Category? category)
    {
        if (category is null) return null;

        return new CategoryDto
        {
            CategoryId = category.CategoryId,
            Name = category.Name ?? string.Empty,
            Products = category.Products?.Select(p => p.ToProductDto()!).ToList() ?? new List<ProductDto>()
        };
    }

    public static Category? ToCategory(this CategoryDto categoryDto)
    {
        return new Category
        {
            CategoryId = categoryDto.CategoryId,
            Name = categoryDto.Name,
        };
    }

    public static IEnumerable<CategoryDto> ToCategoryDtoList(this IEnumerable<Category> categories)
    {
        if (categories is null)
        {
            return new List<CategoryDto>();
        }

        return categories.Select(c => c.ToCategoryDto()!).ToList();
    }
}