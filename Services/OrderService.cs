using Microsoft.Extensions.Logging;
using EcommerceAPI.DTOs.Order;
using EcommerceAPI.Interfaces;
using EcommerceAPI.Models;
using System.Linq;

namespace EcommerceAPI.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
     IOrderRepository orderRepository,
     ICartRepository cartRepository,
     ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _logger = logger;
    }

    public OrderDto Checkout(int userId)
    {
        var cart = _cartRepository.GetCartByUserId(userId);
        _logger.LogInformation(
    "Checkout started for User {UserId}.",
    userId);

        if (cart == null || !cart.CartItems.Any())
        {
            _logger.LogWarning(
                "Checkout failed. Cart is empty for User {UserId}.",
                userId);

            throw new Exception("Cart is empty.");
        }

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            TotalAmount = 0
        };

        foreach (var item in cart.CartItems)
        {
            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Product.Price
            });

            order.TotalAmount += item.Product.Price * item.Quantity;
        }

        _orderRepository.Add(order);

        _cartRepository.RemoveCartItems(cart.CartItems);
        _cartRepository.SaveChanges();
        _logger.LogInformation(
    "Order {OrderId} created successfully for User {UserId}. Total: {TotalAmount}",
    order.Id,
    userId,
    order.TotalAmount);

        return new OrderDto
        {
            OrderId = order.Id,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            Items = order.OrderItems.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };
    }

    public List<OrderDto> GetOrders(int userId)
    {
        var orders = _orderRepository.GetOrdersByUserId(userId);
        _logger.LogInformation(
    "User {UserId} viewed order history.",
    userId);

        return orders.Select(order => new OrderDto
        {
            OrderId = order.Id,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            Items = order.OrderItems.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList()
        }).ToList();
    }

    public OrderDto? GetOrderById(int orderId)
    {
        var order = _orderRepository.GetById(orderId);
        _logger.LogInformation(
    "Viewing Order {OrderId}.",
    orderId);

        if (order == null)
        {
            _logger.LogWarning(
                "Order {OrderId} not found.",
                orderId);

            return null;
        }

        return new OrderDto
        {
            OrderId = order.Id,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            Items = order.OrderItems.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList()
        };
    }
}