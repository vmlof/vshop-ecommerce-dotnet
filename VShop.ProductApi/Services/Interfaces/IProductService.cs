using VShop.ProductApi.DTOs;

namespace VShop.ProductApi.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProducts();
    Task<ProductDto> GetProductById(int id);
    Task AddProduct(ProductDto product);
    Task UpdateProduct(ProductDto product);
    Task RemoveProduct(int id);
    Task UpdateStock(int id, long quantity);
}