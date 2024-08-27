namespace ECommerceWebsite.Models.ViewModals
{
    
        public class CheckoutViewModel
        {
        public List<CartItem1> CartItems { get; set; } = new List<CartItem1>(); // Initialized to avoid null reference
        public decimal TotalPrice { get; set; }
            public string FullName { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public string CreditCardNumber { get; set; }
            public string ExpirationDate { get; set; }
            public string CVV { get; set; }
        }

    }


