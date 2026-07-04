using EcommerceAPI.DTOs;

namespace EcommerceAPI.Interfaces;

public interface IProductService
{
    IEnumerable<ProductDto> GetAll();

    ProductDto? GetById(int id);

    ProductDto Create(CreateProductDto dto);

    bool Update(int id, UpdateProductDto dto);

    bool Delete(int id);
}