using EcommerceAPI.Data;
using EcommerceAPI.Interfaces;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public Cart? GetCartByUserId(int userId)
    {
        return _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefault(c => c.UserId == userId);
    }

    public Cart CreateCart(Cart cart)
    {
        _context.Carts.Add(cart);
        _context.SaveChanges();
        return cart;
    }

    public Cart UpdateCart(Cart cart)
    {
        _context.Carts.Update(cart);
        _context.SaveChanges();
        return cart;
    }
    public CartItem? GetCartItem(int cartId, int productId)
    {
        return _context.CartItems
            .FirstOrDefault(ci => ci.CartId == cartId && ci.ProductId == productId);
    }

    public void AddCartItem(CartItem cartItem)
    {
        _context.CartItems.Add(cartItem);
    }
    public void RemoveCartItem(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
    }

    public void RemoveCartItems(IEnumerable<CartItem> cartItems)
    {
        _context.CartItems.RemoveRange(cartItems);
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}