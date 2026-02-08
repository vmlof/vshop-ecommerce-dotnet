namespace VShop.CartApi.Services;

public interface IProductService
{
    Task<bool> UpdateStock(int productId, int quantity, string token);
}