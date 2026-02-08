using System.Net.Http.Headers;
using System.Text.Json;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Services;

public class CouponService : ICouponService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions? _options;
    private const string ApiEndpoint = "/api/coupon";
    private CouponViewModel couponVm = new CouponViewModel();

    public CouponService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, };
    }

    public async Task<CouponViewModel> GetDiscountCoupon(string couponCode, string token)
    {
        var client = _clientFactory.CreateClient("DiscountApi");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.GetAsync($"{ApiEndpoint}/{couponCode}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                couponVm = await JsonSerializer.DeserializeAsync<CouponViewModel>
                    (apiResponse, _options);
            }
            else
            {
                var errorApi = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Error API ({(int)response.StatusCode}): {errorApi}");

                return null;
            }
        }

        return couponVm;
    }

    private static void PutTokenInHeaderAuthorization(string token, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
}