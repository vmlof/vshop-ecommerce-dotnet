using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VShop.ProductApi.DTOs;
using VShop.ProductApi.Roles;
using VShop.ProductApi.Services.Interfaces;

namespace VShop.ProductApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
    {
        var productsDto = await _productService.GetAllProducts();
        if (productsDto == null)
        {
            return NotFound("Products not found");
        }

        return Ok(productsDto);
    }

    [HttpGet("{id:int}", Name = "GetProduct")]
    public async Task<ActionResult<ProductDto>> Get(int id)
    {
        var productDto = await _productService.GetProductById(id);
        if (productDto == null)
        {
            return NotFound("Product not found");
        }

        return Ok(productDto);
    }

    [HttpPost]
    [Authorize(Roles = Role.Admin)]
    public async Task<ActionResult> Post([FromBody] ProductDto productDto)
    {
        if (productDto == null)
            return BadRequest("Invalid data");

        await _productService.AddProduct(productDto);

        return new CreatedAtRouteResult("GetProduct", new { id = productDto.Id }, productDto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<ActionResult> Put(int id, [FromBody] ProductDto productDto)
    {
        if (id != productDto.Id)
        {
            return BadRequest("Invalid data");
        }

        await _productService.UpdateProduct(productDto);

        return Ok(productDto);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<ActionResult<ProductDto>> Delete(int id)
    {
        var productDto = await _productService.GetProductById(id);

        if (productDto == null)
        {
            return NotFound("Product not found");
        }

        await _productService.RemoveProduct(id);

        return Ok(productDto);
    }

    [HttpPost("updatestock/{id}")]
    [Authorize]
    public async Task<ActionResult> UpdateStock(int id, [FromBody] int quantity)
    {
        if (quantity <= 0)
            return BadRequest("Invalid quantity");

        var product = await _productService.GetProductById(id);
        if (product == null)
            return NotFound("Product not found");

        if (product.Stock < quantity)
            return BadRequest("Insufficient stock");

        await _productService.UpdateStock(id, quantity);
        return Ok();
    }
}