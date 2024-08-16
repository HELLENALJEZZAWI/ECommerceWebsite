using ECommerceWebsite.Constants;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWebsite.Models
{
    public class EventCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }

        [Required, MaxLength(100)]
        public string CategoryName { get; set; }

        [MaxLength(255)]
        public string CategoryDescription { get; set; }

        [EnumDataType(typeof(CategoryStatus))]
        public CategoryStatus CategoryStatus { get; set; } = CategoryStatus.Active;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public ICollection<Events> Products { get; set; } = new List<Events>();
    }
}
