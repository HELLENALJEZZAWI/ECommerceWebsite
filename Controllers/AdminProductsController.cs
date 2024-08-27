﻿using ECommerceWebsite.Context;
using ECommerceWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc_first_task.ActionFilters;

namespace ECommerceWebsite.Controllers
{
    [RoleValidation(Constants.Roles.Admin)]

    public class AdminProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminProductsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: AdminProducts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, List<IFormFile> photos)
        {
            Category category = await _context.Categories.FindAsync(product.CategoryID);

            if (category == null || category.CategoryStatus == Constants.CategoryStatus.Inactive)
                throw new Exception("Category not found");

            product.Category = category;

            category.Products.Add(product);

            //adding  productImages

            var webRootPath = Path.Combine(_hostEnvironment.WebRootPath, "images/productsImages");
            if (!Directory.Exists(webRootPath))
            {
                Directory.CreateDirectory(webRootPath);
            }

            //adding main product image
            var mainProductImage = photos[0];
            Guid mainImage = Guid.NewGuid();
            string fullPathForMain = Path.Combine(webRootPath, mainImage + Path.GetExtension(mainProductImage.FileName));

            using (var fileStream = new FileStream(fullPathForMain, FileMode.Create))
            {
                mainProductImage.CopyTo(fileStream);
            }
            product.ProductImage = Path.GetFileName(fullPathForMain);

            //adding the rest of the product images
            foreach (var photo in photos)
            {
                Guid picName = Guid.NewGuid();
                string fullPath = Path.Combine(webRootPath, picName + Path.GetExtension(photo.FileName));
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }
                var productPhoto = new ProductPhoto
                {
                    ProductID = product.ProductID,
                    FilePath = Path.GetFileName(fullPath)
                };
                _context.ProductPhotos.Add(productPhoto);

                product.ProductPhotos.Add(productPhoto);
            }
            try
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,ProductDescription,Price,Discount,ProductImage,ProductStatus,CategoryEventID,Stock,DateCreated,Location,EventDate")] Events events)
        {
            if (id != events.ProductID)
            {
                return NotFound();
            }

            // Retrieve the existing event from the database
            var eventToChange = await _context.Eventses.FindAsync(id);

            if (eventToChange == null)
            {
                return NotFound();
            }

            // Manually update the properties of eventToChange with the new values from events
            eventToChange.ProductName = events.ProductName;
            eventToChange.ProductDescription = events.ProductDescription;
            eventToChange.Price = events.Price;
            eventToChange.Discount = events.Discount;
            eventToChange.ProductImage = events.ProductImage;
            eventToChange.ProductStatus = events.ProductStatus;
            eventToChange.CategoryEventID = events.CategoryEventID;
            eventToChange.Stock = events.Stock;
            eventToChange.DateCreated = events.DateCreated;
            eventToChange.Location = events.Location;
            eventToChange.EventDate = events.EventDate;

            try
            {
                // Save the changes to the database
                await _context.SaveChangesAsync();
                TempData["Message"] = "Event has been successfully updated.";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventsExists(events.ProductID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Repopulate the ViewData for category dropdown
            ViewData["CategoryEventID"] = new SelectList(_context.EventCategories, "CategoryID", "CategoryName", events.CategoryEventID);

            return View(events);
        }

        // Method to check if the event exists
        private bool EventsExists(int id)
        {
            return _context.Eventses.Any(e => e.ProductID == id);
        }


        [HttpPost]
        public async Task<IActionResult> SoftDelete(int id)
        {

            if (!ProductExists(id))
                return NotFound();

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            product.ProductStatus = Constants.ProductStatus.Deleted;
            await _context.SaveChangesAsync();

            TempData["Message"] = "Product has been successfully deleted.";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [HttpPost]
        public async Task<IActionResult> ActiveProduct(int id)
        {

            if (!ProductExists(id))
                return NotFound();

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            product.ProductStatus = Constants.ProductStatus.Active;
            await _context.SaveChangesAsync();

            TempData["Message"] = "Product has been successfully avtivated.";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [HttpPost]
        public async Task<IActionResult> SetProductToOutOfStuck(int id)
        {

            if (ProductExists(id))
            {
                var product = await _context.Products
            .FirstOrDefaultAsync(m => m.ProductID == id);
                product.ProductStatus = Constants.ProductStatus.OutOfStock;
                product.Stock = 0;
                await _context.SaveChangesAsync();
                TempData["Message"] = "Product has been successfully set to out of stuck";
                return Redirect(Request.Headers["Referer"].ToString());

            }
            TempData["Message"] = "some thing went wrong";
            return Redirect(Request.Headers["Referer"].ToString());

        }



        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
