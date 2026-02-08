using VShop.DiscountApi.DTOs;

namespace VShop.DiscountApi.Repositories;

public interface ICouponRepository
{
    Task<CouponDto> GetCouponByCode(string couponCode);
}