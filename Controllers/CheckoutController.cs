using ECommerceWebsite.Context;
using ECommerceWebsite.Models;
using ECommerceWebsite.Models.ViewModals;
using Microsoft.AspNetCore.Mvc;



public class CheckoutController : Controller
{
    private readonly ApplicationDbContext _context; // Replace with your actual DbContext

    public CheckoutController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Cart1/Checkout
    [HttpGet]
    public IActionResult Checkout()
    {
        var cartId = GetCartId(); // Replace with your method to get the cart ID
        var cartItems = _context.CartItems
            .Where(c => c.CartID == cartId)
            .ToList();

        var totalAmount = cartItems.Sum(i => i.TotalPrice);

        // Create the ViewModel
        var viewModel = new CheckoutViewModel
        {
            CartItems = cartItems,
            //TotalAmount = totalAmount
        };

        // Store values in ViewBag
        ViewBag.TotalAmount = totalAmount;
        ViewBag.CartItems = cartItems;

        return View(viewModel);
    }

    // POST: /Cart1/Checkout
    [HttpPost]
    public IActionResult Checkout(CheckoutViewModel model)
    {
        var cartId = GetCartId(); // Replace with your method to get the cart ID
        var cartItems = _context.CartItems
            .Where(c => c.CartID == cartId)
            .ToList();

        if (cartItems == null || !cartItems.Any())
        {
            ModelState.AddModelError("", "Your cart is empty.");
            return View(model);
        }

        // Here, you can add the logic to process the payment and create an order

        // After successful checkout, clear the cart
        _context.CartItems.RemoveRange(cartItems);
        _context.SaveChanges();

        // Store values in ViewBag for potential use after redirection
        //ViewBag.TotalAmount = model.TotalAmount;
        ViewBag.CartItems = cartItems;

        // Redirect to a confirmation page or home page
        return RedirectToAction("OrderConfirmation");
    }

    private string GetCartId()
    {
        // Implement your logic to retrieve the cart ID from session or user context
        return HttpContext.Session.GetString("CartId") ?? User.Identity.Name;
    }

    // Add an OrderConfirmation action to show the confirmation page after successful checkout
    public IActionResult OrderConfirmation()
    {
        // Retrieve the values if needed for the confirmation page
        ViewBag.TotalAmount = ViewBag.TotalAmount ?? 0;
        ViewBag.CartItems = ViewBag.CartItems ?? new List<CartItem1>();

        return View();
    }
}
