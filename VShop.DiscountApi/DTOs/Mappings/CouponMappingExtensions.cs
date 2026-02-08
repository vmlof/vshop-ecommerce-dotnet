using VShop.DiscountApi.Models;

namespace VShop.DiscountApi.DTOs;

public static class CouponMappingExtensions
{
    public static CouponDto ToDto(this Coupon coupon)
    {
        if (coupon == null) return null;

        return new CouponDto
        {
            CouponId = coupon.CouponId,
            CouponCode = coupon.CouponCode,
            Discount = coupon.Discount
        };
    }

    public static Coupon ToCoupon(this CouponDto dto)
    {
        if (dto == null) return null;

        return new Coupon
        {
            CouponId = dto.CouponId,
            CouponCode = dto.CouponCode,
            Discount = dto.Discount
        };
    }
}