using EcommerceAPI.Models;

namespace EcommerceAPI.Interfaces;

public interface IOrderRepository
{
    Order Add(Order order);

    List<Order> GetOrdersByUserId(int userId);

    Order? GetById(int orderId);

    void SaveChanges();
}