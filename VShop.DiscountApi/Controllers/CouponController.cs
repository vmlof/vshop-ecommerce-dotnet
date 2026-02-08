using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VShop.DiscountApi.DTOs;
using VShop.DiscountApi.Repositories;

namespace VShop.DiscountApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CouponController : ControllerBase
{
    private readonly ICouponRepository _repository;

    public CouponController(ICouponRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{couponCode}")]
    [Authorize]
    public async Task<ActionResult<CouponDto>> GetDiscountByCode(string couponCode)
    {
        var coupon = await _repository.GetCouponByCode(couponCode);

        if (coupon == null)
            return NotFound($"Coupon code {couponCode} not found");

        return Ok(coupon);
    }
}