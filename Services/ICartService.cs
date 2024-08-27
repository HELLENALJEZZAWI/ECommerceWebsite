using ECommerceWebsite.Models;

namespace ECommerceWebsite.Services
{
    public interface ICartService
    {
        void AddToCart(int? eventId, int? productId, int quantity);
        void RemoveFromCart(int cartItemId);
        List<CartItem1> GetCartItems();
        decimal GetTotalAmount();
    }
}

