using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceWebsite.Models.ViewModals
{
    public class EventIndexViewModel
    {
        public IEnumerable<Events> Events { get; set; }
        [ValidateNever]
        public IEnumerable<EventCategory> EventCategories { get; set; }
        public int? SelectedCategoryId { get; set; }  // To store the selected category ID

    }
}
