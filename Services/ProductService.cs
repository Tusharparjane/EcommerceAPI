using EcommerceAPI.DTOs;
using EcommerceAPI.Interfaces;
using EcommerceAPI.Models;

namespace EcommerceAPI.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
    IProductRepository repository,
    ILogger<ProductService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public IEnumerable<ProductDto> GetAll()
    {
        _logger.LogInformation("Fetching all products.");

        return _repository.GetAll().Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            ImageUrl = p.ImageUrl,
            CategoryId = p.CategoryId
        });
    }

    public ProductDto? GetById(int id)
    {
        var product = _repository.GetById(id);
        _logger.LogInformation("Fetching product with ID {ProductId}", id);

        if (product == null)
            return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId
        };
    }

    public ProductDto Create(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            ImageUrl = dto.ImageUrl,
            CategoryId = dto.CategoryId
        };

        _repository.Add(product);
        _logger.LogInformation(
    "Product '{ProductName}' created successfully.",
    product.Name);

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId
        };
    }

    public bool Update(int id, UpdateProductDto dto)
    {
        var product = new Product
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            ImageUrl = dto.ImageUrl,
            CategoryId = dto.CategoryId
        };

        var updated = _repository.Update(product);

        if (updated != null)
        {
            _logger.LogInformation(
                "Product {ProductId} updated successfully.",
                id);

            return true;
        }

        _logger.LogWarning(
            "Product {ProductId} not found for update.",
            id);

        return false;
    }

    public bool Delete(int id)
    {
        var deleted = _repository.Delete(id);

        if (deleted)
        {
            _logger.LogInformation(
                "Product {ProductId} deleted successfully.",
                id);
        }
        else
        {
            _logger.LogWarning(
                "Product {ProductId} not found for deletion.",
                id);
        }

        return deleted;
    }
}