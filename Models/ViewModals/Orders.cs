using System.ComponentModel.DataAnnotations;

namespace ECommerceWebsite.Models.ViewModals
{
    public class Orders
    {
        
            [Key]
            public int OrderID { get; set; }

            [Required]
            public string UserID { get; set; }

            [Required]
            public string FullName { get; set; }

            [Required]
            public string Address { get; set; }

            [Required]
            public string City { get; set; }

            [Required]
            public string State { get; set; }

            [Required]
            public string Country { get; set; }

            [Required]
            public string PostalCode { get; set; }

            [Required]
            public decimal TotalAmount { get; set; }

            public DateTime OrderDate { get; set; } = DateTime.UtcNow;

            [Required]
            public string OrderStatus { get; set; } = "Pending";

            public List<CartItem1> CartItems { get; set; }
        }

    }

