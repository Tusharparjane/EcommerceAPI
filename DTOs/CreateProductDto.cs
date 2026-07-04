using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs;

public class CreateProductDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = "";

    [Required]
    public string Description { get; set; } = "";

    [Range(1, 1000000)]
    public decimal Price { get; set; }

    [Range(0, 10000)]
    public int Stock { get; set; }

    [Url]
    public string ImageUrl { get; set; } = "";

    public int CategoryId { get; set; }
}