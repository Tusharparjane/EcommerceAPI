using EcommerceAPI.Data;
using EcommerceAPI.Interfaces;
using EcommerceAPI.Models;

namespace EcommerceAPI.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Product> GetAll()
    {
        return _context.Products.ToList();
    }

    public Product? GetById(int id)
    {
        return _context.Products.Find(id);
    }

    public Product Add(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();

        return product;
    }

    public Product? Update(Product product)
    {
        var existing = _context.Products.Find(product.Id);

        if (existing == null)
            return null;

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.Stock = product.Stock;
        existing.ImageUrl = product.ImageUrl;
        existing.CategoryId = product.CategoryId;

        _context.SaveChanges();

        return existing;
    }

    public bool Delete(int id)
    {
        var product = _context.Products.Find(id);

        if (product == null)
            return false;

        _context.Products.Remove(product);
        _context.SaveChanges();

        return true;
    }
}