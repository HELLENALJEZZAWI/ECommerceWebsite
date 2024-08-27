using ECommerceWebsite.Context;
using ECommerceWebsite.Models;
using ECommerceWebsite.Models.ViewModals;
using ECommerceWebsite.Models.ViewModels;
using ECommerceWebsite.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWebsite.Controllers
{
    public class Checkout2Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly CartService _cartService;
        public Checkout2Controller(ApplicationDbContext _context, CartService cartService)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));

        }
       
        [Route("Checkout/OrderConfirmation")]

        public IActionResult Checkout()
        {
            var cartId = GetCartId();  // Assuming GetCartId retrieves the current cart ID
            var cartItems = _cartService.GetCartItems(cartId);

            if (cartItems == null)
            {
                cartItems = new List<CartItem1>(); // Ensure cartItems is not null
            }

            var totalPrice = cartItems.Sum(item => item.Quantity *
                                                   (item.Product != null ? item.Product.Price : item.Event.Price) *
                                                   (1 - (item.Product != null ? item.Product.Discount : item.Event.Discount) / 100));

            var checkoutViewModel = new CheckoutViewModel
            {
                CartItems = cartItems,
                TotalPrice = totalPrice
            };

            return View(checkoutViewModel);
        }
        public List<CartItem1> GetCartItems(string userName)
        {
            // Retrieve cart items from the database
            var cartItems = _context.CartItems.Where(ci => ci.CartID == userName).ToList();

            return cartItems ?? new List<CartItem1>();
        }
        [HttpPost]
        public IActionResult ProcessCheckout(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Process the payment and order here
                // Save the order details to the database

                return RedirectToAction("OrderConfirmation");
            }

            // If something goes wrong, return the view with the model data
            return View("Checkout", model);
        }

        public IActionResult OrderConfirmation(OrderConfirmationViewModel model)
        {
            return View("OrderConfirmation", model);
        }
        private string GetCartId()
        {
            // Check if CartId is stored in the session
            if (HttpContext.Session.GetString("CartId") == null)
            {
                // Generate a new GUID for the cart ID
                var cartId = Guid.NewGuid().ToString();

                // Store the generated CartId in the session
                HttpContext.Session.SetString("CartId", cartId);
            }

            // Return the CartId from the session
            return HttpContext.Session.GetString("CartId");
        }

    }
}
