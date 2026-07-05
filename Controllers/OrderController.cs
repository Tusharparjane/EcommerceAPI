using EcommerceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("checkout")]
    public IActionResult Checkout()
    {
        var userId = GetUserId();

        var order = _orderService.Checkout(userId);

        return Ok(order);
    }

    [HttpGet]
    public IActionResult GetOrders()
    {
        var userId = GetUserId();

        var orders = _orderService.GetOrders(userId);

        return Ok(orders);
    }

    [HttpGet("details/{orderId}")]
    public IActionResult GetOrder(int orderId)
    {
        var order = _orderService.GetOrderById(orderId);

        if (order == null)
            return NotFound();

        return Ok(order);
    }
    private int GetUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User not found.");

        return int.Parse(userId);
    }
}