using VShop.ProductApi.DTOs;
using VShop.ProductApi.DTOs.Mappings;
using VShop.ProductApi.Repositories.Interfaces;
using VShop.ProductApi.Services.Interfaces;

namespace VShop.ProductApi.Services;

public class ProductService : IProductService
{
    private readonly IProdutctRepository _produtctRepository;

    public ProductService(IProdutctRepository produtctRepository)
    {
        _produtctRepository = produtctRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProducts()
    {
        var productsEntity = await _produtctRepository.GetAll();
        return productsEntity.ToProductDtoList();
    }

    public async Task<ProductDto> GetProductById(int id)
    {
        var productEntity = await _produtctRepository.GetById(id);
        return productEntity.ToProductDto()!;
    }

    public async Task AddProduct(ProductDto productDto)
    {
        var productEntity = productDto.ToProduct();
        await _produtctRepository.Create(productEntity!);
        productDto.Id = productEntity!.Id;
    }

    public async Task UpdateProduct(ProductDto productDto)
    {
        var productEntity = productDto.ToProduct();
        await _produtctRepository.Update(productEntity!);
    }

    public async Task RemoveProduct(int id)
    {
        var productEntity = await _produtctRepository.GetById(id);
        await _produtctRepository.Delete(productEntity.Id);
    }

    public async Task UpdateStock(int id, long quantity)
    {
        var productEntity = await _produtctRepository.GetById(id);
        if (productEntity != null)
        {
            productEntity.Stock -= quantity;
            await _produtctRepository.Update(productEntity);
        }
    }
}