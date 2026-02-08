using VShop.ProductApi.DTOs;
using VShop.ProductApi.DTOs.Mappings;
using VShop.ProductApi.Repositories.Interfaces;
using VShop.ProductApi.Services.Interfaces;

namespace VShop.ProductApi.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategories()
    {
        var categoriesEntity = await _categoryRepository.GetAll();
        return categoriesEntity.ToCategoryDtoList();
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesProducts()
    {
        var categoriesEntity = await _categoryRepository.GetCategoriesProducts();
        return categoriesEntity.ToCategoryDtoList();
    }

    public async Task<CategoryDto> GetCategoryById(int id)
    {
        var categoryEntity = await _categoryRepository.GetById(id);
        return categoryEntity.ToCategoryDto()!;
    }

    public async Task AddCategory(CategoryDto categoryDto)
    {
        var categoryEntity = categoryDto.ToCategory();
        await _categoryRepository.Create(categoryEntity!);
        categoryDto.CategoryId = categoryEntity!.CategoryId;
    }

    public async Task UpdateCategory(CategoryDto categoryDto)
    {
        var categoryEntity = categoryDto.ToCategory();
        await _categoryRepository.Update(categoryEntity!);
    }

    public async Task RemoveCategory(int id)
    {
        await  _categoryRepository.Delete(id);
    }
}