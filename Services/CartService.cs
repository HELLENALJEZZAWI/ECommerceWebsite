using ECommerceWebsite.Context;
using ECommerceWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWebsite.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddToCart(string cartId, int? productId = null, int? eventId = null, int quantity = 1)
        {
            var cartItem = _context.CartItems.SingleOrDefault(
                c => c.CartID == cartId &&
                     c.ProductID == productId &&
                     c.EventID == eventId);

            if (cartItem == null)
            {
                cartItem = new CartItem1
                {
                    CartID = cartId,
                    ProductID = productId,
                    EventID = eventId,
                    Quantity = quantity,
                    DateCreated = DateTime.UtcNow
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            _context.SaveChanges();
        }

        public List<CartItem1> GetCartItems(string cartId)
        {
            return _context.CartItems
                           .Where(c => c.CartID == cartId)
                           .Include(c => c.Product)
                           .Include(c => c.Event)
                           .ToList();
        }

        public decimal GetTotalAmount(string cartId)
        {
            return _context.CartItems
                .Where(c => c.CartID == cartId)
                .Sum(c => c.Quantity * (c.Product != null ? c.Product.Price : c.Event.Price) *
                           (1 - (c.Product != null ? c.Product.Discount : c.Event.Discount) / 100));
        }

        public void RemoveFromCart(int cartItemId)
        {
            var cartItem = _context.CartItems.SingleOrDefault(c => c.CartItemID == cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                _context.SaveChanges();
            }
        }

        public void ClearCart(string cartId)
        {
            var cartItems = _context.CartItems.Where(c => c.CartID == cartId);
            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();
        }
    }

}
