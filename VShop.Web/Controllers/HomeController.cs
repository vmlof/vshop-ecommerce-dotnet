using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(IProductService productService, ICartService cartService)
    {
        _productService = productService;
        _cartService = cartService;
    }

    public async Task<ActionResult> Index()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var products = await _productService.GetAllProducts(token);

        if (products is null)
            return View("Error");

        return View(products);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ProductViewModel>> ProductDetails(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var product = await _productService.GetProductById(id, token);

        if (product == null)
            return View("Error");


        return View(product);
    }

    [HttpPost]
    [ActionName("ProductDetails")]
    [Authorize]
    public async Task<ActionResult<ProductViewModel>> ProductDetailsPost(ProductViewModel productVm)
    {
        var token = await HttpContext.GetTokenAsync("access_token");


        CartViewModel cart = new()
        {
            CartHeader = new CartHeaderViewModel
            {
                UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
            }
        };


        CartItemViewModel cartItem = new()
        {
            Product = await _productService.GetProductById(productVm.Id, token),
            Quantity = productVm.Quantity,
            ProductId = productVm.Id,
        };


        List<CartItemViewModel> cartItemsVm = new List<CartItemViewModel>();
        cartItemsVm.Add(cartItem);
        cart.CartItems = cartItemsVm;

        var result = await _cartService.AddItemToCartAsync(cart, token);

        if (result is not null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(productVm);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public async Task<ActionResult> Login()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        return RedirectToAction(nameof(Index));
    }

    public ActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }
}