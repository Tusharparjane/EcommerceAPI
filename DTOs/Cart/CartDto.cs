namespace EcommerceAPI.DTOs.Cart;

public class CartDto
{
    public List<CartItemDto> Items { get; set; } = new();

    public decimal GrandTotal => Items.Sum(i => i.Total);
}