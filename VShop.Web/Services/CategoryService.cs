using System.Net.Http.Headers;
using System.Text.Json;
using VShop.Web.Models;

namespace VShop.Web.Services.Contracts;

public class CategoryService : ICategoryService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions _options;
    private const string ApiEndpoint = "api/category";

    public CategoryService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<CategoryViewModel>> GetAllCategories(string token)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        IEnumerable<CategoryViewModel> categories;

        var response = await client.GetAsync(ApiEndpoint);

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync();
            categories = await JsonSerializer
                             .DeserializeAsync<IEnumerable<CategoryViewModel>>(apiResponse, _options)
                         ?? new List<CategoryViewModel>();
        }
        else
        {
            return null;
        }

        return categories;
    }
}