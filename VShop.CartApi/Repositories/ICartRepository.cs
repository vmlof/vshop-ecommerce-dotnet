using VShop.CartApi.DTOs;

namespace VShop.CartApi.Repositories;

public interface ICartRepository
{
    Task<CartDto> GetCartByUserIdAsync(string userId);
    Task<CartDto> UpdateCartAsync(CartDto cart);
    Task<bool> CleanCartAsync(string userId);
    Task<bool> DeleteItemCartAsync(int carItemId);
    Task<bool> ApplyCouponAsync(string userId, string couponCode);
    Task<bool> DeleteCouponAsync(string userId);
}