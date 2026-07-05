using Microsoft.Extensions.Logging;
using EcommerceAPI.DTOs.Cart;
using EcommerceAPI.Interfaces;
using EcommerceAPI.Models;

namespace EcommerceAPI.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly ILogger<CartService> _logger;

    public CartService(
       ICartRepository cartRepository,
       ILogger<CartService> logger)
    {
        _cartRepository = cartRepository;
        _logger = logger;
    }

    public void AddToCart(int userId, AddToCartDto dto)
    {
        var cart = _cartRepository.GetCartByUserId(userId);

        if (cart == null)
        {
            cart = _cartRepository.CreateCart(new Cart
            {
                UserId = userId
            });
        }

        var cartItem = _cartRepository.GetCartItem(cart.Id, dto.ProductId);

        if (cartItem != null)
        {
            cartItem.Quantity += dto.Quantity;
        }
        else
        {
            _cartRepository.AddCartItem(new CartItem
            {
                CartId = cart.Id,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            });
        }

        _cartRepository.SaveChanges();
        _logger.LogInformation(
    "User {UserId} added Product {ProductId} (Quantity: {Quantity}) to cart.",
    userId,
    dto.ProductId,
    dto.Quantity);
    }

    public CartDto GetCart(int userId)
    {
        var cart = _cartRepository.GetCartByUserId(userId);
        _logger.LogInformation(
    "User {UserId} viewed cart.",
    userId);

        if (cart == null)
        {
            return new CartDto();
        }

        return new CartDto
        {
            Items = cart.CartItems.Select(ci => new CartItemDto
            {
                ProductId = ci.ProductId,
                ProductName = ci.Product.Name,
                Price = ci.Product.Price,
                Quantity = ci.Quantity
            }).ToList()
        };
    }
    public void UpdateCartItem(int userId, UpdateCartItemDto dto)
    {
        var cart = _cartRepository.GetCartByUserId(userId);

        if (cart == null)
            throw new Exception("Cart not found.");

        var cartItem = _cartRepository.GetCartItem(cart.Id, dto.ProductId);

        if (cartItem == null)
            throw new Exception("Product not found in cart.");

        if (dto.Quantity <= 0)
        {
            _cartRepository.RemoveCartItem(cartItem);
        }
        else
        {
            cartItem.Quantity = dto.Quantity;
        }

        _cartRepository.SaveChanges();
        _logger.LogInformation(
    "User {UserId} updated Product {ProductId} quantity to {Quantity}.",
    userId,
    dto.ProductId,
    dto.Quantity);
    }
    public void RemoveCartItem(int userId, int productId)
    {
        var cart = _cartRepository.GetCartByUserId(userId);

        if (cart == null)
            throw new Exception("Cart not found.");

        var cartItem = _cartRepository.GetCartItem(cart.Id, productId);

        if (cartItem == null)
            throw new Exception("Product not found in cart.");

        _cartRepository.RemoveCartItem(cartItem);

        _cartRepository.SaveChanges();
        _logger.LogInformation(
    "User {UserId} removed Product {ProductId} from cart.",
    userId,
    productId);
    }
    public void ClearCart(int userId)
    {
        var cart = _cartRepository.GetCartByUserId(userId);

        if (cart == null)
            throw new Exception("Cart not found.");

        _cartRepository.RemoveCartItems(cart.CartItems);

        _cartRepository.SaveChanges();
        _logger.LogInformation(
    "User {UserId} cleared the cart.",
    userId);
    }
}