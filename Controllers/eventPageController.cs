using ECommerceWebsite.Context;
using ECommerceWebsite.Models;
using ECommerceWebsite.Models.ViewModals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWebsite.Controllers
{
    public class eventPageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;


        public eventPageController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(int? id)
        {
            // Filter to exclude soft-deleted events
            IQueryable<Events> eventsQuery = _context.Eventses.Where(e => !e.IsDeleted);

            if (id != null)
            {
                // Further filter by CategoryEventID if provided
                eventsQuery = eventsQuery.Where(e => e.CategoryEventID == id);
            }

            // Fetch the event categories to populate the view model
            var eventCategories = _context.EventCategories.ToList();

            // Create and populate the ViewModel
            EventIndexViewModel eventVM = new EventIndexViewModel
            {
                EventCategories = eventCategories,
                Events = eventsQuery.ToList(), // Execute the query to get the list of events
            };

            return View(eventVM);
        }
        public async Task<IActionResult> AdminProductIndex()
        {
            /* var products = await _context.Products
                 .Include(p => p.Category)
                 .Include(p => p.ProductPhotos)
                 //.Take(3)
                 .ToListAsync();

             var categories = await _context.Categories.ToListAsync();

             var viewModel = new ProductIndexViewModel
             {
                 Products = products,
                 Categories = categories
             };*/

            return View();
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product =  _context.Eventses
                .Include(p => p.EventCategory)
                .Include(p => p.EventPhoto)
                .FirstOrDefault(m => m.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
