using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VShop.ProductApi.DTOs;
using VShop.ProductApi.Roles;
using VShop.ProductApi.Services.Interfaces;

namespace VShop.ProductApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> Get()
    {
        var categoriesDto = await _categoryService.GetCategories();

        if (categoriesDto is null)
            return NotFound("Categories not found");

        return Ok(categoriesDto);
    }

    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesProducts()
    {
        var categoriesDto = await _categoryService.GetCategoriesProducts();

        if (categoriesDto == null)
        {
            return NotFound("Categories not found");
        }

        return Ok(categoriesDto);
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        var categoryDto = await _categoryService.GetCategoryById(id);
        if (categoryDto == null)
        {
            return NotFound("Category not found");
        }

        return Ok(categoryDto);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CategoryDto categoryDto)
    {
        if (categoryDto == null)
        {
            return BadRequest("Invalid data");
        }

        await _categoryService.AddCategory(categoryDto);

        return CreatedAtRoute("GetCategory", new { id = categoryDto.CategoryId }, categoryDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] CategoryDto categoryDto)
    {
        if (id != categoryDto.CategoryId) return BadRequest();

        if (categoryDto == null == null) return BadRequest();

        await _categoryService.UpdateCategory(categoryDto);

        return Ok(categoryDto);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<ActionResult<CategoryDto>> Delete(int id)
    {
        var categoryDto = await _categoryService.GetCategoryById(id);
        if (categoryDto == null)
        {
            return NotFound("Category not found");
        }

        await _categoryService.RemoveCategory(id);

        return Ok(categoryDto);
    }
}