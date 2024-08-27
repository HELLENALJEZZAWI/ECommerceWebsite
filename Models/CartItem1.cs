using System.ComponentModel.DataAnnotations;

namespace ECommerceWebsite.Models
{
    public class CartItem1
    {
        [Key]
        public int CartItemID { get; set; }

        public int? ProductID { get; set; } // Nullable to allow either Product or Event
        public Product Product { get; set; }

        public int? EventID { get; set; } // Nullable to allow either Event or Product
        public Events Event { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Quantity { get; set; }

        [Required]
        public string CartID { get; set; } // Cart ID to associate with a specific session or user

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        // Calculate the total price considering both Product and Event
        public decimal TotalPrice => Quantity * (Product?.Price ?? Event?.Price ?? 0) * (1 - (Product?.Discount ?? Event?.Discount ?? 0) / 100);
    }
}