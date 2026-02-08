using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly ICouponService _couponService;

    public CartController(ICartService cartService, ICouponService couponService)
    {
        _cartService = cartService;
        _couponService = couponService;
    }


    private async Task<string> GetAccessToken()
    {
        return await HttpContext.GetTokenAsync("access_token");
    }

    private string GetUserId()
    {
        return User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
    }


    private async Task<CartViewModel> GetCartByUser()
    {
        var cart = await _cartService.GetCartByUserIdAsync(GetUserId(), await GetAccessToken());

        if (cart?.CartHeader is not null)
        {
            if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
            {
                var coupon = await _couponService.GetDiscountCoupon(cart.CartHeader.CouponCode,
                    await GetAccessToken());
                if (coupon?.CouponCode is not null)
                {
                    cart.CartHeader.Discount = coupon.Discount;
                }
            }

            foreach (var item in cart.CartItems)
            {
                cart.CartHeader.TotalAmount += (item.Product.Price * item.Quantity);
            }

            cart.CartHeader.TotalAmount -=
                (cart.CartHeader.TotalAmount *
                 cart.CartHeader.Discount) / 100;
        }

        return cart;
    }

    [HttpGet]
    public async Task<ActionResult> Checkout()
    {
        CartViewModel cartVm = await GetCartByUser();
        return View(cartVm);
    }

    [HttpPost]
    public async Task<ActionResult> CheckOut(CartViewModel cartVm)
    {
        if (ModelState.IsValid)
        {
            var result = await _cartService.CheckoutAsync(cartVm.CartHeader, await GetAccessToken());

            if (result != null)
            {
                return RedirectToAction(nameof(CheckoutCompleted));
            }
        }

        var cartUser = await GetCartByUser();
        cartVm.CartItems = cartUser.CartItems;

        return View(cartVm);
    }

    [HttpGet]
    public ActionResult CheckoutCompleted()
    {
        return View();
    }


    [HttpPost]
    public async Task<ActionResult> ApplyCoupon(CartViewModel cartVm)
    {
        if (ModelState.IsValid)
        {
            var result = await _cartService.ApplyCouponAsync(cartVm, await GetAccessToken());

            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> DeleteCoupon()
    {
        var result = await _cartService.RemoveCouponAsync(GetUserId(), await GetAccessToken());

        if (result)
        {
            return RedirectToAction(nameof(Index));
        }

        return View();
    }

    [Authorize]
    public async Task<ActionResult> Index()
    {
        CartViewModel cartVm = await GetCartByUser();
        if (cartVm == null)
        {
            ModelState.AddModelError("CartNotFound", "Cart does not exist. Come on Shopping...");
            return View("/Views/Cart/CartNotFound.cshtml");
        }

        return View(cartVm);
    }

    public async Task<ActionResult> RemoveItem(int id)
    {
        var result = await _cartService.RemoveItemFromCartAsync(id, await GetAccessToken());
        if (result)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(id);
    }
}