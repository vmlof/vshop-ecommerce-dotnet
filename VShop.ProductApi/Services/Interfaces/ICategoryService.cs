using VShop.ProductApi.DTOs;

namespace VShop.ProductApi.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetCategories();
    Task<IEnumerable<CategoryDto>> GetCategoriesProducts();
    Task<CategoryDto> GetCategoryById(int id);
    Task AddCategory(CategoryDto category);
    Task UpdateCategory(CategoryDto category);
    Task RemoveCategory(int id);
}