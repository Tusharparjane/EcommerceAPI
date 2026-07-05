namespace EcommerceAPI.DTOs.Order;

public class OrderDto
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = "";

    public List<OrderItemDto> Items { get; set; } = new();
}