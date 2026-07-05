using EcommerceAPI.Models;

namespace EcommerceAPI.Interfaces;

public interface ICartRepository
{
    Cart? GetCartByUserId(int userId);

    Cart CreateCart(Cart cart);

    Cart UpdateCart(Cart cart);

    CartItem? GetCartItem(int cartId, int productId);

    void AddCartItem(CartItem cartItem);
    void RemoveCartItem(CartItem cartItem);

    void RemoveCartItems(IEnumerable<CartItem> cartItems);

    void SaveChanges();
}