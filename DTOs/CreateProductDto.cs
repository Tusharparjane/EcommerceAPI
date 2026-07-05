using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs;

public class CreateProductDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = "";

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = "";

    [Range(1, 1000000)]
    public decimal Price { get; set; }

    [Range(0, 10000)]
    public int Stock { get; set; }

    [Required]
    [Url]
    public string ImageUrl { get; set; } = "";

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}