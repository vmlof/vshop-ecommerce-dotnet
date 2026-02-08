using VShop.ProductApi.Models;

namespace VShop.ProductApi.DTOs.Mappings;

public static class ProductDtoMappingExtensions
{
    public static ProductDto? ToProductDto(this Product? product)
    {
        if (product is null) return null;
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description ?? string.Empty,
            Stock =  product.Stock,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty,
        };
    }

    public static Product? ToProduct(this ProductDto? productDto)
    {
        if (productDto == null) return null;

        return new Product
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Price = productDto.Price,
            Description = productDto.Description,
            Stock = productDto.Stock,
            ImageUrl = productDto.ImageUrl,
            CategoryId = productDto.CategoryId,
        };
    }

    public static IEnumerable<ProductDto> ToProductDtoList(this IEnumerable<Product>? products)
    {
        if (products is null)
        {
            return new List<ProductDto>();
        }

        return products.Select(p => p.ToProductDto()!).ToList();
    }
}