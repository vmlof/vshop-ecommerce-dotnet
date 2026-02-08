using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Services;

public class CartService : ICartService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions _options;
    private const string ApiEndpoint = "/api/cart";
    private CartViewModel cartVm = new CartViewModel();
    private CartHeaderViewModel cartHeaderVm = new CartHeaderViewModel();

    public CartService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, };
    }


    public async Task<CartViewModel> GetCartByUserIdAsync(string userId, string token)
    {
        var client = _clientFactory.CreateClient("CartApi");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.GetAsync($"{ApiEndpoint}/getcart/{userId}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                cartVm = await JsonSerializer
                    .DeserializeAsync<CartViewModel>
                        (apiResponse, _options);
            }
            else
            {
                return null;
            }
        }

        return cartVm;
    }

    public async Task<CartViewModel> AddItemToCartAsync(CartViewModel cartVm, string token)
    {
        var client = _clientFactory.CreateClient("CartApi");
        PutTokenInHeaderAuthorization(token, client);

        StringContent content = new StringContent(JsonSerializer.Serialize(cartVm),
            Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync($"{ApiEndpoint}/addcart/", content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                cartVm = await JsonSerializer
                    .DeserializeAsync<CartViewModel>(apiResponse, _options);
            }
            else
            {
                var errorApi = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error API ({(int)response.StatusCode}): {errorApi}");

                return null;
            }
        }

        return cartVm;
    }

    public async Task<CartViewModel> UpdateCartAsync(CartViewModel cartVm, string token)
    {
        var client = _clientFactory.CreateClient("CartApi");
        PutTokenInHeaderAuthorization(token, client);

        CartViewModel cartUpdated = new CartViewModel();

        using (var response = await client.PutAsJsonAsync($"{ApiEndpoint}/updatecart/", cartVm))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                cartUpdated = await JsonSerializer
                    .DeserializeAsync<CartViewModel>
                        (apiResponse, _options);
            }
            else
            {
                return null;
            }
        }

        return cartUpdated;
    }

    public async Task<bool> RemoveItemFromCartAsync(int cartId, string token)
    {
        var client = _clientFactory.CreateClient("CartApi");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.DeleteAsync($"{ApiEndpoint}/deletecart/" + cartId))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<bool> ApplyCouponAsync(CartViewModel cartVm, string token)
    {
        var client = _clientFactory.CreateClient("CartApi");
        PutTokenInHeaderAuthorization(token, client);

        StringContent content = new StringContent(JsonSerializer.Serialize(cartVm),
            Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync($"{ApiEndpoint}/applycoupon/", content))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<bool> RemoveCouponAsync(string userId, string token)
    {
        var client = _clientFactory.CreateClient("CartApi");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.DeleteAsync($"{ApiEndpoint}/deletecoupon/{userId}"))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<CartHeaderViewModel> CheckoutAsync(CartHeaderViewModel cartHeaderVm, string token)
    {
        var client = _clientFactory.CreateClient("CartApi");
        PutTokenInHeaderAuthorization(token, client);

        StringContent content = new StringContent(JsonSerializer.Serialize(cartHeaderVm),
            Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync($"{ApiEndpoint}/checkout/", content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                cartHeaderVm = await JsonSerializer
                    .DeserializeAsync<CartHeaderViewModel>
                        (apiResponse, _options);
            }
            else
            {
                var errorApi = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error API ({(int)response.StatusCode}): {errorApi}");

                return null;
            }
        }

        return cartHeaderVm;
    }

    public async Task<bool> ClearCartAsync(string userId, string token)
    {
        throw new NotImplementedException();
    }

    private static void PutTokenInHeaderAuthorization(string token, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
}