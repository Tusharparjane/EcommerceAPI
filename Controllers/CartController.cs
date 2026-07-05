using EcommerceAPI.DTOs.Cart;
using EcommerceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public IActionResult GetCart()
    {
        var userId = GetUserId();

        var cart = _cartService.GetCart(userId);

        return Ok(cart);
    }
    [HttpPost]
    public IActionResult AddToCart(AddToCartDto dto)
    {
        var userId = GetUserId();

        _cartService.AddToCart(userId, dto);

        return Ok(new { message = "Product added to cart successfully." });
    }
    [HttpPut]
    public IActionResult UpdateCartItem(UpdateCartItemDto dto)
    {
        var userId = GetUserId();

        _cartService.UpdateCartItem(userId, dto);

        return Ok(new { message = "Cart updated successfully." });
    }
    [HttpDelete("{productId}")]
    public IActionResult RemoveCartItem(int productId)
    {
        var userId = GetUserId();

        _cartService.RemoveCartItem(userId, productId);

        return Ok(new { message = "Product removed from cart." });
    }
    [HttpDelete]
    public IActionResult ClearCart()
    {
        var userId = GetUserId();

        _cartService.ClearCart(userId);

        return Ok(new { message = "Cart cleared successfully." });
    }
    private int GetUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User not found.");

        return int.Parse(userId);
    }
}