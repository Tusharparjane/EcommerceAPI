using EcommerceAPI.DTOs.Cart;

namespace EcommerceAPI.Interfaces;

public interface ICartService
{
    CartDto GetCart(int userId);

    void AddToCart(int userId, AddToCartDto dto);
    void UpdateCartItem(int userId, UpdateCartItemDto dto);

    void RemoveCartItem(int userId, int productId);

    void ClearCart(int userId);
}