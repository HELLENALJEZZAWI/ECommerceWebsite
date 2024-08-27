using ECommerceWebsite.Constants;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ECommerceWebsite.Models
{
    public class Events
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [Required, MaxLength(100)]
        public string ProductName { get; set; }

        [MaxLength(255)]
        public string ProductDescription { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Discount { get; set; } = 0;
        public string ?ProductImage { get; set; }
  


        [EnumDataType(typeof(ProductStatus))]
        public ProductStatus ProductStatus { get; set; } = ProductStatus.Active;
        public int CategoryEventID { get; set; }

        [ForeignKey("CategoryEventID")]
        [ValidateNever]
        public EventCategory EventCategory { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; } = 0;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string Location { get; set; }

        public DateTime EventDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }

        public ICollection<EventPhoto> EventPhoto { get; set; } = new List<EventPhoto>();


        //public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // we might need it to calculate how many times products is ordered
    }
}

