using EcommerceAPI.Data;
using EcommerceAPI.DTOs;
using EcommerceAPI.Interfaces;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    // GET: api/products
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_productService.GetAll());
    }

    // POST: api/products
    [HttpPost]
    public IActionResult Create(CreateProductDto dto)
    {
        var product = _productService.Create(dto);

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
    // GET: api/products/1
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = _productService.GetById(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }
    // PUT: api/products/1
    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateProductDto dto)
    {
        if (!_productService.Update(id, dto))
            return NotFound();

        return NoContent();
    }
    // DELETE: api/products/1
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (!_productService.Delete(id))
            return NotFound();

        return NoContent();
    }
}