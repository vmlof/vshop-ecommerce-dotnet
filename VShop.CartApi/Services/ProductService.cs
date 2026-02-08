using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace VShop.CartApi.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _clientFactory;

    public ProductService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<bool> UpdateStock(int productId, int quantity, string token)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


        var content = new StringContent(
            JsonSerializer.Serialize(quantity),
            Encoding.UTF8,
            "application/json");

        var response = await client.PostAsync($"/api/product/updatestock/{productId}", content);

        return response.IsSuccessStatusCode;
    }
}