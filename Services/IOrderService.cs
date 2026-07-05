using EcommerceAPI.DTOs.Order;

namespace EcommerceAPI.Interfaces;

public interface IOrderService
{
    OrderDto Checkout(int userId);

    List<OrderDto> GetOrders(int userId);

    OrderDto? GetOrderById(int orderId);
}