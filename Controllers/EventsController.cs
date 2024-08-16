using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceWebsite.Context;
using ECommerceWebsite.Models;
using Microsoft.Extensions.Hosting;

namespace ECommerceWebsite.Controllers
{
    public class EventsController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Eventses.Include(e => e.EventCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Events/Details/5

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["CategoryEventID"] = new SelectList(_context.EventCategories, "CategoryID", "CategoryName");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Events events, List<IFormFile> photos)
        {
            try
            {
                // Fetch the event category
                EventCategory eventCategory = await _context.EventCategories.FindAsync(events.CategoryEventID);

                // Validate the event category
                if (eventCategory == null || eventCategory.CategoryStatus == Constants.CategoryStatus.Inactive)
                {
                    ModelState.AddModelError("", "Event category not found or is inactive.");
                    ViewData["CategoryEventID"] = new SelectList(_context.EventCategories, "CategoryID", "CategoryName", events.CategoryEventID);
                    return View(events);
                }

                events.EventCategory = eventCategory;

                // Set the directory path for event images
                var webRootPath = Path.Combine(_hostEnvironment.WebRootPath, "images/eventsImages");
                if (!Directory.Exists(webRootPath))
                {
                    Directory.CreateDirectory(webRootPath);
                }

                // Ensure at least one photo is provided
                if (photos == null || !photos.Any())
                {
                    ModelState.AddModelError("", "Please upload at least one image.");
                    ViewData["CategoryEventID"] = new SelectList(_context.EventCategories, "CategoryID", "CategoryName", events.CategoryEventID);
                    return View(events);
                }

                // Adding the main event image
                var mainEventImage = photos.First();
                string mainImageFileName = Guid.NewGuid() + Path.GetExtension(mainEventImage.FileName);
                string mainImagePath = Path.Combine(webRootPath, mainImageFileName);

                using (var fileStream = new FileStream(mainImagePath, FileMode.Create))
                {
                    await mainEventImage.CopyToAsync(fileStream);
                }
                events.ProductImage = mainImageFileName;

                // Adding the rest of the event images
                foreach (var photo in photos)
                {
                    Guid picName = Guid.NewGuid();
                    string fullPath = Path.Combine(webRootPath, picName + Path.GetExtension(photo.FileName));
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }
                    var productPhoto = new EventPhoto
                    {
                        EventId = events.ProductID,
                        FilePath = Path.GetFileName(fullPath)
                    };
                    _context.EventPhotos.Add(productPhoto);

                    events.EventPhoto.Add(productPhoto);
                }

                // Add the event to the database
                _context.Add(events);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("", "Unable to save changes. Please try again, and if the problem persists, contact your system administrator.");
            }

            ViewData["CategoryEventID"] = new SelectList(_context.EventCategories, "CategoryID", "CategoryName", events.CategoryEventID);
            return View(events);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Eventses.FindAsync(id);
            if (events == null)
            {
                return NotFound();
            }
            ViewData["CategoryEventID"] = new SelectList(_context.EventCategories, "CategoryID", "CategoryName", events.CategoryEventID);
            return View(events);
        }

        // POST: Events/Edit/5
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(events);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryEventID"] = new SelectList(_context.EventCategories, "CategoryID", "CategoryName", events.CategoryEventID);
            return View(events);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Eventses
                .Include(e => e.EventCategory)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var events = await _context.Eventses.FindAsync(id);
            if (events != null)
            {
                _context.Eventses.Remove(events);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventsExists(int id)
        {
            return _context.Eventses.Any(e => e.ProductID == id);
        }
    }
}
