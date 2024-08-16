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
        public  IActionResult Index(int? id)
        {

            IEnumerable<Events> eventes = _context.Eventses;

            if (id !=null)
            {
               
                eventes = _context.Eventses.Where(e => e.CategoryEventID == id);
            }


            EventIndexViewModel eventVM = new()
            {
                EventCategories =_context.EventCategories,           
                Events = eventes,
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
