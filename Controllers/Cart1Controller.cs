using ECommerceWebsite.Context;
using ECommerceWebsite.Models.ViewModals;
using ECommerceWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWebsite.Controllers
{
public class Cart1Controller : Controller
{
    private readonly CartService _cartService;
    private readonly string _cartId;

    public Cart1Controller(CartService cartService, IHttpContextAccessor httpContextAccessor)
    {
        _cartService = cartService;
        _cartId = httpContextAccessor.HttpContext.Session.GetString("CartId") 
                  ?? Guid.NewGuid().ToString();
        httpContextAccessor.HttpContext.Session.SetString("CartId", _cartId);
    }

    public IActionResult Index()
    {
        var cartItems = _cartService.GetCartItems(_cartId);
        var totalAmount = _cartService.GetTotalAmount(_cartId);
        var model = new CartViewModel
        {
            CartItems = cartItems,
            TotalAmount = totalAmount
        };
        return View(model);
    }

    public IActionResult AddToCart(int? productId = null, int? eventId = null, int quantity = 1)
    {
        _cartService.AddToCart(_cartId, productId, eventId, quantity);
        return RedirectToAction("Index");
    }

    public IActionResult RemoveFromCart(int cartItemId)
    {
        _cartService.RemoveFromCart(cartItemId);
        return RedirectToAction("Index");
    }

    public IActionResult ClearCart()
    {
        _cartService.ClearCart(_cartId);
        return RedirectToAction("Index");
    }
}
}
