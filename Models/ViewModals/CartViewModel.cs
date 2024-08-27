namespace ECommerceWebsite.Models.ViewModals
{
    public class CartViewModel
    {
        public List<CartItem1> CartItems { get; set; } = new List<CartItem1>();
        public decimal TotalAmount { get; set; }
    }
}
