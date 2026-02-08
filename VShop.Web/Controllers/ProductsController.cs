using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VShop.Web.Models;
using VShop.Web.Roles;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Controllers;

[Authorize(Roles = Role.Admin)]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductsController(IProductService productService,
        ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    private async Task<string?> GetAccessToken()
    {
        return await HttpContext.GetTokenAsync("access_token");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductViewModel>>> Index()
    {
        var result = await _productService.GetAllProducts(await GetAccessToken());

        if (result == null)
            return View("Error");

        return View(result);
    }


    [HttpGet]
    public async Task<ActionResult> CreateProduct()
    {
        ViewBag.CategoryId = new SelectList(await
            _categoryService.GetAllCategories(await GetAccessToken()), "CategoryId", "Name");

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> CreateProduct(ProductViewModel productVm)
    {
        if (ModelState.IsValid)
        {
            var result = await _productService.CreateProduct(productVm, await GetAccessToken());

            if (result == null)
                return RedirectToAction(nameof(Index));
        }
        else
        {
            ViewBag.CategoryId = new SelectList(await
                _categoryService.GetAllCategories(await GetAccessToken()), "CategoryId", "Name");
        }

        return View(productVm);
    }

    [HttpGet]
    public async Task<ActionResult> UpdateProduct(int id)
    {
        var result = await _productService.GetProductById(id, await GetAccessToken());

        if (result == null)
            return View("Error");

        ViewBag.CategoryId = new SelectList(await
            _categoryService.GetAllCategories(await GetAccessToken()), "CategoryId", "Name", result.CategoryId);

        return View(result);
    }

    [HttpPost]
    public async Task<ActionResult> UpdateProduct(ProductViewModel productVm)
    {
        if (ModelState.IsValid)
        {
            var result = await _productService.UpdateProduct(productVm, await GetAccessToken());

            if (result != null)
                return RedirectToAction(nameof(Index));
        }

        ViewBag.CategoryId = new SelectList(await
            _categoryService.GetAllCategories(await GetAccessToken()), "CategoryId", "Name", productVm.CategoryId);

        return View(productVm);
    }

    [HttpGet]
    public async Task<ActionResult<ProductViewModel>> DeleteProduct(int id)
    {
        var result = await _productService.GetProductById(id, await GetAccessToken());

        if (result == null)
            return View("Error");

        return View(result);
    }

    [HttpPost(), ActionName("DeleteProduct")]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var result = await _productService.DeleteProduct(id, await GetAccessToken());

        if (!result)
            return View("Error");

        return RedirectToAction("Index");
    }
}