using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWebsite.Models
{
    public class EventPhoto
    {
        [Key]
        public int PhotoID { get; set; }

        [Required]
        public string FilePath { get; set; }

        public int EventId { get; set; }

        [ForeignKey("ProductID")]
        public Events events { get; set; }
    }
}
