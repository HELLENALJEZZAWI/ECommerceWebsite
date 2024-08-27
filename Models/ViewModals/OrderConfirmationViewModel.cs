using System;
using System.Collections.Generic;

namespace ECommerceWebsite.Models.ViewModels
{
    public class OrderConfirmationViewModel
    {
        public string OrderNumber { get; set; }          // Unique order number
        public DateTime OrderDate { get; set; }          // Date when the order was placed
        public decimal TotalAmount { get; set; }         // Total amount of the order
        public List<OrderItemViewModel> OrderItems { get; set; }  // List of items in the order

        public string CustomerName { get; set; }         // Optional: Name of the customer
        public string ShippingAddress { get; set; }      // Optional: Shipping address
        public string PaymentMethod { get; set; }        // Optional: Payment method used
    }

    public class OrderItemViewModel
    {
        public string ProductName { get; set; }          // Name of the product or event
        public int Quantity { get; set; }                // Quantity of the product or event ordered
        public decimal Price { get; set; }               // Price per unit of the product or event
        public decimal Discount { get; set; }            // Optional: Discount applied to the item
    }
}
