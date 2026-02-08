using Microsoft.AspNetCore.Mvc;
using VShop.CartApi.DTOs;
using VShop.CartApi.Repositories;
using VShop.CartApi.Services;

namespace VShop.CartApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartRepository _repository;
    private readonly IProductService _productService;

    public CartController(ICartRepository repository, IProductService productService)
    {
        _repository = repository;
        _productService = productService;
    }

    [HttpGet("getcart/{id}")]
    public async Task<ActionResult<CartDto>> GetByUserId(string id)
    {
        var cartDto = await _repository.GetCartByUserIdAsync(id);

        if (cartDto == null)
            return NotFound();

        return Ok(cartDto);
    }

    [HttpPost("addcart")]
    public async Task<ActionResult<CartDto>> AddCart(CartDto cartDto)
    {
        var cart = await _repository.UpdateCartAsync(cartDto);

        if (cart == null)
            return NotFound();

        return Ok(cart);
    }

    [HttpPut("updatecart")]
    public async Task<ActionResult<CartDto>> UpdateCart(CartDto cartDto)
    {
        var cart = await _repository.UpdateCartAsync(cartDto);
        if (cart == null)
            return NotFound();
        return Ok(cart);
    }

    [HttpDelete("deletecart/{id}")]
    public async Task<ActionResult<bool>> DeleteCart(int id)
    {
        var status = await _repository.DeleteItemCartAsync(id);
        if (!status) return BadRequest();
        return Ok(status);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutHeaderDto>> Checkout(CheckoutHeaderDto checkoutDto)
    {
        var cart = await _repository.GetCartByUserIdAsync(checkoutDto.UserId);

        if (cart == null)
        {
            return NotFound($"Cart not found for = {checkoutDto.UserId}");
        }

        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        foreach (var item in cart.CartItems)
        {
            var success = await _productService.UpdateStock(item.ProductId, item.Quantity, token);

            if (!success)
            {
                return StatusCode(500, $"Unable to update stock for product {item.ProductId}");
            }
        }

        checkoutDto.CartItems = cart.CartItems;
        checkoutDto.DateTime = DateTime.Now;

        await _repository.CleanCartAsync(checkoutDto.UserId);

        return Ok(checkoutDto);
    }

    [HttpPost("applycoupon")]
    public async Task<ActionResult<CartDto>> ApplyCoupon(CartDto cartDto)
    {
        var result = await _repository.ApplyCouponAsync(cartDto.CartHeader
            .UserId, cartDto.CartHeader.CouponCode);

        if (!result)
            return NotFound($"CartHeader not found for userId = {cartDto.CartHeader.UserId}");


        return Ok(result);
    }

    [HttpDelete("deletecoupon/{userId}")]
    public async Task<ActionResult<CartDto>> DeleteCoupon(string userId)
    {
        var result = await _repository.DeleteCouponAsync(userId);

        if (!result)
            return NotFound($"Discount Coupon not found for userId = {userId}");

        return Ok(result);
    }
}