using Microsoft.EntityFrameworkCore;
using VShop.ProductApi.Context;
using VShop.ProductApi.Models;
using VShop.ProductApi.Repositories.Interfaces;

namespace VShop.ProductApi.Repositories;

public class ProductRepository : IProdutctRepository

{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _context.Products
            .Include(c => c.Category)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Product?> GetById(int id)
    {
        return await _context.Products
            .Include(c => c.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> Create(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Update(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> Delete(int id)
    {
        var product = await GetById(id);
        if (product == null) return null;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return product;
    }
}