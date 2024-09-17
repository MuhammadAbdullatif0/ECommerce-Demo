
using Core.Entities;
using Core.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext context) : IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetAllAsync(string? brand, string? type, string? sort)
    {
        var products = context.Products.AsQueryable();
        if (!string.IsNullOrWhiteSpace(brand))
        {
            products = products.Where(p => p.Brand == brand);
        }
        if (!string.IsNullOrWhiteSpace(type))
        {
            products = products.Where(p => p.Type == type);
        }
        products = sort switch
        {
            "priceAsc" => products.OrderBy(x => x.Price),
            "priceDesc" => products.OrderByDescending(x => x.Price),
            _ => products.OrderBy(x => x.Name)
        };


        return await products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public void CreateProduct(Product product)
    {
        context.Products.Add(product);
    }

    public void UpdateProduct(Product product)
    {
        context.Entry(product).State = EntityState.Modified;
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    public bool ProductExists(int id)
    {
        return context.Products.Any(x => x.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await context.Products.Select(x => x.Brand).Distinct().ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await context.Products.Select(context => context.Type).Distinct().ToListAsync();
    }
}
