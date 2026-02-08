using VShop.Web.Models;

namespace VShop.Web.Services.Contracts;

public interface IProductService
{
    Task<IEnumerable<ProductViewModel>> GetAllProducts(string token);
    Task<ProductViewModel> GetProductById(int id, string token);
    Task<ProductViewModel> CreateProduct(ProductViewModel model, string token);
    Task<ProductViewModel> UpdateProduct(ProductViewModel model, string token);
    Task<bool> DeleteProduct(int id, string token);
}