using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _clientFactory;
    private const string ApiEndpoint = "/api/product";
    private readonly JsonSerializerOptions _options;
    private ProductViewModel productVm;
    private IEnumerable<ProductViewModel> productsVm;

    public ProductService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, };
    }

    public async Task<IEnumerable<ProductViewModel>> GetAllProducts(string token)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.GetAsync(ApiEndpoint))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();

                productsVm = await JsonSerializer
                                 .DeserializeAsync<IEnumerable<ProductViewModel>>(apiResponse, _options)
                             ?? new List<ProductViewModel>();
            }
            else
            {
                return Enumerable.Empty<ProductViewModel>();
            }
        }

        return productsVm;
    }

    public async Task<ProductViewModel> GetProductById(int id, string token)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.GetAsync(ApiEndpoint + "/" + id))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer
                           .DeserializeAsync<ProductViewModel>(apiResponse, _options)
                       ?? new ProductViewModel();
            }

            return null;
        }
    }

    public async Task<ProductViewModel> CreateProduct(ProductViewModel productVm, string token)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        PutTokenInHeaderAuthorization(token, client);

        StringContent content = new StringContent(JsonSerializer.Serialize(productVm),
            Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync(ApiEndpoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productVm = await JsonSerializer.DeserializeAsync<ProductViewModel>(apiResponse, _options)
                            ?? new ProductViewModel();
            }
            else
            {
                return null;
            }
        }

        return productVm;
    }

    public async Task<ProductViewModel> UpdateProduct(ProductViewModel productVm, string token)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        PutTokenInHeaderAuthorization(token, client);

        ProductViewModel productUpdated = new ProductViewModel();

        using (var response = await client.PutAsJsonAsync(ApiEndpoint + "/" + productVm.Id, productVm))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productUpdated = await JsonSerializer.DeserializeAsync<ProductViewModel>(apiResponse, _options)
                                 ?? new ProductViewModel();
            }
            else
            {
                return null;
            }
        }

        return productUpdated;
    }

    public async Task<bool> DeleteProduct(int id, string token)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.DeleteAsync(ApiEndpoint + "/" + id))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }

    private static void PutTokenInHeaderAuthorization(string token, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}